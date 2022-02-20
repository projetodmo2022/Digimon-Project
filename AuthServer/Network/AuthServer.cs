using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using AuthServer.Database.Interfaces;
using Yggdrasil;
using Digital_World;
using Microsoft.Extensions.Configuration;
using Yggdrasil.Network;
using Yggdrasil.Packets;


namespace AuthServer.Network
{
    /// <summary>
    /// Description of AuthServer.
    /// Sistema de senha =  MD5+SHA512
    /// GhostNigth
    /// </summary>
    public class AuthServ : Server
    {
        
        private readonly ILoginDatabase _loginDatabase;
        private readonly IConfiguration _configuration;
        
        
        //Sistema de codificaçao da senha
        public static string SHA2(string value)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 shaM = new SHA256Managed())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(value);
                buffer = shaM.ComputeHash(buffer);
                for (int i = 0; i < buffer.Length; i++)
                    sb.Append(buffer[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string SHA5(string value)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA512 shaM = new SHA512Managed())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(value);
                buffer = shaM.ComputeHash(buffer);
                for (int i = 0; i < buffer.Length; i++)
                    sb.Append(buffer[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public AuthServ(ILoginDatabase loginDatabase, IConfiguration configuration)
		{
			OnConnect += AuthServ_OnConnect;
            OnDisconnect += AuthServ_OnDisconnect;
            DataReceived += AuthServ_OnDataReceived;
            _loginDatabase = loginDatabase;
            _configuration = configuration;
        }

        private void AuthServ_OnConnect(object sender, ClientEventArgs e)
        {
            SysCons.LogInfo("Client connected: {0}", e.Client.ToString());
            e.Client.User = new AuthClient(e.Client);
            PacketWriter writer = new PacketWriter();
            e.Client.Handshake = (short)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() & 0xFFFF);
            writer.Type(0xFFFF);
            writer.WriteShort(e.Client.Handshake);
            if (e.Client.IsConnected) e.Client.Send(writer.Finalize());
            //if (e.Client.IsConnected) e.Client.Send(new PacketFFFF(e.Client.Handshake));

        }

        private void AuthServ_OnDisconnect(object sender, ClientEventArgs e)
        {
            AuthClient client = ((AuthClient) e.Client.User);
            SysCons.LogInfo("Client disconnected: {0}", e.Client.ToString());
        }

        private void AuthServ_OnDataReceived(object sender, ClientEventArgs e, byte[] data)
        {
            Process((AuthClient)e.Client.User, data);
        }
		
		public override void Run()
        {
            Console.Title = "Auth Server";
            var serverInformation = _configuration.GetSection("LoginServer");
            var hostIP = serverInformation["HostIP"];
            var serverPort = serverInformation["HostPort"];
            if (!this.Listen(hostIP, int.Parse(serverPort))) return;
            SysCons.LogInfo("AuthServer is listening on {0}:{1}...", hostIP, serverPort);
        }

        public static byte[] ToByteArray(String HexString)
        {
            int NumberChars = HexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }
            return bytes;
        }
        private byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
        public PacketWriter ServerList(Dictionary<int, string> servers, string user, int characters)
        {
            PacketWriter writer = new PacketWriter();
            writer.Type(0x06A5);
            writer.WriteByte((byte)servers.Count);
            foreach (KeyValuePair<int, string> server in servers)
            {
                writer.WriteInt(server.Key); // - > PORT
                writer.WriteString(server.Value);// - > IP
                writer.WriteByte((byte)_loginDatabase.ServerMaintenance(server.Value)); // ->  // Maintenience 1 = yes | 0 = no
                writer.WriteByte((byte)_loginDatabase.ServerLoad(server.Value)); // ->  Server Load 0 = low | 1 = mid | 2 = full
                writer.WriteByte((byte)characters); //Characters - > Count
                writer.WriteByte((byte)_loginDatabase.ServerNewOrNo(server.Value));
                writer.WriteByte(0x5);
                writer.WriteByte(0x5);
            }

            return writer;
            //writer.WriteString(user);
        }

        private static Random RNG = new Random();
        public void Process(AuthClient client, byte[] buffer)
        {
            PacketReader packet = null;
            IClient Client = client.Client;
            try
            {
                packet = new PacketReader(buffer);
            }
            catch
            {
                return;
            }

            PacketDefinitions.LogPacketData(packet);

            switch (packet.Type)
            {
                case -1:
                    {
                        PacketWriter writer = new PacketWriter();
                        uint time_t = (uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        short data = (short)(client.Client.Handshake ^ 0x7e41);
                        writer.Type(-2);
                        writer.WriteShort(data);
                        writer.WriteUInt(time_t);
                        Client.Send(writer.Finalize());
                        break;
                    }
                case -3:
                    {
                        break;
                    }

                case 3301:
                    {
                        uint Version = packet.ReadUInt();
                        int UserType_size = packet.ReadByte();
                        byte[] UserType_get = new byte[UserType_size];

                        for (int i = 0; i < UserType_size; i++)
                        {
                            UserType_get[i] = packet.ReadByte();
                        }
                        string UserType = Encoding.ASCII.GetString(UserType_get).Trim();
                        int u_size = packet.ReadByte();
                        byte[] username_get = new byte[u_size];
                        for (int i = 0; i < u_size; i++)
                        {
                            username_get[i] = packet.ReadByte();
                        }
                        string username = Encoding.ASCII.GetString(username_get).Trim();
                        packet.ReadByte();
                        int p_size = packet.ReadByte();
                        byte[] password_get = new byte[p_size];
                        for (int i = 0; i < p_size; i++)
                        {
                            password_get[i] = packet.ReadByte();
                        }
                        string password = Encoding.ASCII.GetString(password_get).Trim();
                        int c_size = packet.ReadByte();
                        byte[] cpu_get = new byte[c_size];
                        for (int i = 0; i < c_size; i++)
                        {
                            cpu_get[i] = packet.ReadByte();
                        }
                        string cpu = Encoding.ASCII.GetString(cpu_get).Trim();
                        int f_size = packet.ReadByte();
                        byte[] gpu_get = new byte[f_size];
                        for (int i = 0; i < f_size; i++)
                        {
                            gpu_get[i] = packet.ReadByte();
                        }
                        string gpu = Encoding.ASCII.GetString(gpu_get).Trim();
                        int check = _loginDatabase.Validate(client, username, password);
                        SysCons.LogInfo("USER LOGIN-IN: {0}", username);
                        switch (check)
                        {
                            default:
                                //int checksecond = _loginDatabase.CheckSecurity(client.Username, client.SecondaryPassword);
                                PacketWriter writer = new PacketWriter();
                                writer.Type(0x0CE5);
                                writer.WriteInt(0);
                                writer.WriteByte(1);
                                Client.Send(writer.Finalize());
                                break;

                            case -3:
                            case -2:
                            case -1:
                                PacketWriter writer2 = new PacketWriter();
                                writer2.Type(0x0CE5);
                                writer2.WriteByte(0x12);
                                writer2.WriteByte(0x27);
                                writer2.WriteByte(0x0);
                                writer2.WriteByte(0x0);
                                writer2.WriteByte(0x0);
                                Client.Send(writer2.Finalize());
                                break;
                        }

                        break;
                    }
                case 1701:
                    {
                        Client.Send(ServerList(_loginDatabase.GetServers(), client.Username, client.Characters).Finalize());
                        break;
                    }
                case 1702:
                    {

                        int serverID = BitConverter.ToInt32(buffer, 4);

                        //A PARTIR DAQUI VOCÊ TERÁ PORT & IP
                        KeyValuePair<int, string> server = _loginDatabase.GetServer(serverID);

                        //CARREGAR O USUÁRIO - > CARREGAR TODAS AS INFORMAÇÕES DO CLIENTE CONECTANDO NA CLASSE CLIENTE
                        _loginDatabase.LoadUser(client);
                        PacketWriter Server = new PacketWriter();
                        Server.Type(901);
                        Server.WriteUInt(client.AccountID, 4);
                        Server.WriteInt(client.UniqueID, 8);
                        Server.WriteString(server.Value, 12);
                        Server.WriteUInt((uint)server.Key, 12 + server.Value.Length + 2);
                        Client.Send(Server.Finalize());
                        break;
                    }
                default:
                    //Unknown Packets!

                    PacketDefinitions.LogPacketData(packet);
                    break;


            }

        }
    }
}
