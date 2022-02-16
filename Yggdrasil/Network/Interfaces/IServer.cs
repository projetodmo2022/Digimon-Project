using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Yggdrasil.Network.Interfaces
{
    public interface IServer
    {
        public delegate void ClientEventHandler(object sender, ClientEventArgs e);
        public delegate void ClientDataSendEventHandler(object sender, ClientDataEventArgs e);
        public delegate void ClientDataReceiveEventHandler(object sender, ClientDataEventArgs e, byte[] data);
        
        public void Run();
        public bool Listen(string bindIP, int port);
        public void SendToAll(byte[] buffer);
        public int Send(Client client, byte[] buffer, int start, int count, SocketFlags flags);
        public void OnClientConnection(ClientEventArgs e);
        public void OnClientDisconnect(ClientEventArgs e);
        public void OnDataReceived(ClientDataEventArgs e);
        public void OnDataSent(ClientDataEventArgs e);
        public IEnumerable<IClient> GetClients();
        public void DisconnectAll();
        public void Disconnect(Client client);
        public void Shutdown();
        public void Dispose();
        public void Dispose(bool disposing);
    }
}