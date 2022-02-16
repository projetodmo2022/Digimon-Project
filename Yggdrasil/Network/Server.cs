using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Yggdrasil.Network.Interfaces;
using Digital_World;

namespace Yggdrasil.Network
{
    /// <summary>
    /// Server Socket
    /// </summary>
    public class Server : IServer, IDisposable
    {
        private bool _IsListening = false;
        public bool IsListening
        {
            get { return _IsListening; }
            private set { _IsListening = value; }
        }
        public int _Port = 0;
        public int Port
        {
            get { return _Port; }
            private set { _Port = value; }
        }

        protected Socket Listener;
        protected ManualResetEvent allDone = new ManualResetEvent(true);
        public Dictionary<Socket, IClient> Clients = new Dictionary<Socket, IClient>();
        protected object ClientLock = new object();

        public delegate void ClientEventHandler(object sender, ClientEventArgs e);
        public delegate void ClientDataSendEventHandler(object sender, ClientDataEventArgs e);
        public delegate void ClientDataReceiveEventHandler(object sender, ClientDataEventArgs e, byte[] data);

        public event ClientEventHandler OnConnect;
        public event ClientEventHandler OnDisconnect;
        public event ClientDataReceiveEventHandler DataReceived;
        public event ClientDataSendEventHandler DataSent;

        private bool _disposed;

        public virtual void Run() { }

        public virtual bool Listen(string bindIP, int port)
        {
            // Check if the server has been disposed.
            if (_disposed) throw new ObjectDisposedException(this.GetType().Name, "Server has been disposed.");

            // Check if the server is already listening.
            if (IsListening) throw new InvalidOperationException("Server is already listening.");

            // Create new TCP socket and set socket options.
            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Setup our options:
            // * NoDelay - don't use packet coalescing
            // * DontLinger - don't keep sockets around once they've been disconnected
            Listener.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            Listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);

            try
            {
                // Bind.
                Listener.Bind(new IPEndPoint(IPAddress.Parse(bindIP), port));
                this.Port = port;
            }
            catch (SocketException)
            {
                SysCons.LogError(string.Format("{0} can't bind on {1}, server shutting down..", this.GetType().Name, bindIP));
                this.Shutdown();
                return false;
            }

            // Start listening for incoming connections.
            Listener.Listen(10);
            IsListening = true;

            // Begin accepting any incoming connections asynchronously.
            Listener.BeginAccept(AcceptCallback, null);
            return true;
        }

        private void AcceptCallback(IAsyncResult result)
        {
            if (Listener == null) return;

            try
            {
                Socket socket = Listener.EndAccept(result); // Finish accepting the incoming connection.
                Client client = new Client(this, socket); // Add the new connection to the dictionary.

                lock (ClientLock) Clients[socket] = client; // add the connection to list.

                OnClientConnection(new ClientEventArgs(client)); // Raise the OnConnect event.

                client.BeginReceive(ReceiveCallback, client); // Begin receiving on the new connection connection.
                Listener.BeginAccept(AcceptCallback, null); // Continue receiving other incoming connection asynchronously.
            }
            catch (NullReferenceException) { } // we recive this after issuing server-shutdown, just ignore it.
            catch (Exception e)
            {
                SysCons.LogError("AcceptCallback: {0}", e);
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            Client client = result.AsyncState as Client; // Get the connection connection passed to the callback.
            if (client == null) return;

            try
            {
                int bytesRecv = client.EndReceive(result); // Finish receiving data from the socket.

                if (bytesRecv > 0)
                {
                    OnDataReceived(new ClientDataEventArgs(client, client.RecvBuffer)); // Raise the DataReceived event.

                    if (client.IsConnected) client.BeginReceive(ReceiveCallback, client); // Begin receiving again on the socket, if it is connected.
                    else RemoveClient(client, true); // else remove it from connection list.
                }
                else RemoveClient(client, true); // Connection was lost.
            }
            catch (SocketException)
            {
                RemoveClient(client, true); // An error occured while receiving, connection has disconnected.
            }
            catch (Exception e)
            {
                SysCons.LogError("ReceiveCallback: {0}", e);
            }
        }

        public void SendToAll(byte[] buffer)
        {
            foreach (Client client in Clients.Values)
            {
                if (client.IsConnected)
                {
                    client.Send(buffer);
                }
            }


        }

        public virtual int Send(Client client, byte[] buffer, int start, int count, SocketFlags flags)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (buffer == null) throw new ArgumentNullException("buffer");

            int totalBytesSent = 0;
            int bytesRemaining = buffer.Length;

            try
            {
                while (bytesRemaining > 0) // Ensure we send every byte.
                {
                    int bytesSent = client.Socket.Send(buffer, totalBytesSent, bytesRemaining, flags); // Send the remaining data.
                    if (bytesSent > 0)
                        OnDataSent(new ClientDataEventArgs(client, buffer)); // Raise the Data Sent event.

                    // Decrement bytes remaining and increment bytes sent.
                    bytesRemaining -= bytesSent;
                    totalBytesSent += bytesSent;
                }
            }
            catch (SocketException)
            {
                RemoveClient(client, true); // An error occured while sending, connection has disconnected.
            }
            catch (Exception e)
            {
                SysCons.LogError("Send: {0}", e);
            }

            return totalBytesSent;
        }

        public virtual void OnClientConnection(ClientEventArgs e)
        {
            ClientEventHandler handler = OnConnect;
            if (handler != null) handler(this, e);
        }

        public virtual void OnClientDisconnect(ClientEventArgs e)
        {
            ClientEventHandler handler = OnDisconnect;
            if (handler != null) handler(this, e);
        }

        public virtual void OnDataReceived(ClientDataEventArgs e)
        {
            ClientDataReceiveEventHandler handler = DataReceived;
            if (handler != null) handler(this, e, e.Data);
        }

        public virtual void OnDataSent(ClientDataEventArgs e)
        {
            ClientDataSendEventHandler handler = DataSent;
            if (handler != null) handler(this, e);
        }

        public IEnumerable<IClient> GetClients()
        {
            lock (ClientLock)
                foreach (IClient client in Clients.Values)
                    yield return client;
        }

        public virtual void DisconnectAll()
        {
            lock (ClientLock)
            {
                foreach (Client client in Clients.Values)
                {
                    // Disconnect and raise the OnDisconnect event.
                    if (client.IsConnected)
                    {
                        client.Socket.Disconnect(false);
                        OnClientDisconnect(new ClientEventArgs(client));
                    }
                }

                Clients.Clear();
            }
        }

        public virtual void Disconnect(Client client)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (!client.IsConnected) return;

            client.Socket.Disconnect(false);
            RemoveClient(client, true);
        }

        public void RemoveClient(Client client, bool raiseEvent)
        {
            // Remove the connection from the dictionary and raise the OnDisconnection event.
            lock (ClientLock)
                if (Clients.Remove(client.Socket) && raiseEvent)
                    OnClientDisconnect(new ClientEventArgs(client));
        }

        public virtual void Shutdown()
        {
            // Check if the server has been disposed.
            if (_disposed) throw new ObjectDisposedException(this.GetType().Name, "Server has been disposed.");

            // Check if the server is actually listening.
            if (!IsListening) return;

            // Close the listener socket.
            if (Listener != null)
            {
                Listener.Close();
                Listener = null;
            }

            DisconnectAll();

            Listener = null;
            IsListening = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Shutdown(); // Close the listener socket.
                DisconnectAll(); // Disconnect all users.
            }

            // Dispose of unmanaged resources here.

            _disposed = true;
        }
    }
}