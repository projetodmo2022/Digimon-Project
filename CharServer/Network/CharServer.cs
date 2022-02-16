using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CharServer.Database.Interfaces;
using Digital_World;
using Microsoft.Extensions.Configuration;
using Yggdrasil.Database;
using Yggdrasil.Entities;
using Yggdrasil.Helpers;
using Yggdrasil.Network;
using Yggdrasil.Packets;

namespace CharServer.Network
{
    public class CharServ : Server
    {
        private readonly ICharacterDatabase _characterDatabase;
        private readonly IConfiguration _configuration;
        
        public CharServ(ICharacterDatabase characterDatabase, IConfiguration configuration)
		{
			OnConnect += LobbyServer_OnConnect;
            OnDisconnect += LobbyServer_OnDisconnect;
            DataReceived += LobbyServer_OnDataReceived;
            
            _characterDatabase = characterDatabase;
            _configuration = configuration;
        }

        private void LobbyServer_OnConnect(object sender, ClientEventArgs e)
        {
            SysCons.LogInfo("Client connected: {0}", e.Client.ToString());
            e.Client.User = new CharClient(e.Client);
            PacketWriter writer = new PacketWriter();
            short time_t = (short)(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            e.Client.Handshake = (short)(time_t & 0xFFFF);
            writer.Type(0xFFFF);
            writer.WriteShort(e.Client.Handshake); 
            if (e.Client.IsConnected) e.Client.Send(writer.Finalize());
        }

        private void LobbyServer_OnDisconnect(object sender, ClientEventArgs e)
        {
            CharClient client = ((CharClient) e.Client.User);
            SysCons.LogInfo("Client disconnected: {0}", e.Client.ToString());
        }

        private void LobbyServer_OnDataReceived(object sender, ClientEventArgs e, byte[] data)
        {
            Process((CharClient)e.Client.User, data);
        }
		
		public override void Run()
        {
            DigimonListDB.Load("Data\\Digimon_List.bin");
            MapListDB.Load("Data\\MapList.bin");
            Console.Title = "Gate Server";
            var serverInformation = _configuration.GetSection("CharacterServer");
            var hostIP = serverInformation["HostIP"];
            var serverPort = int.Parse(serverInformation["HostPort"]);
            if (!this.Listen(hostIP, serverPort)) return;
            SysCons.LogInfo("GateServer is listening on {0}:{1}...", hostIP, serverPort);
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
        private static Random RNG = new Random();


        public PacketWriter CharList(List<Character> listTamers)
        {
            PacketWriter writer = new PacketWriter();
            writer.Type(1301);
            foreach (Character Tamer in listTamers)
            {
                writer.WriteByte((byte)Tamer.CharacterPos);
                writer.WriteShort((short)Tamer.Location.Map);
                writer.WriteInt((int)Tamer.Model);
                writer.WriteByte((byte)Tamer.Level);
                writer.WriteString(Tamer.Name);
                for (int i = 0; i < 13; i++)
                {
                    Item item = Tamer.Equipment[i];
                    writer.WriteBytes(item.ToArray());
                }
                writer.WriteInt(Tamer.Partner.Species);
                writer.WriteByte((byte)Tamer.Partner.Level);
                writer.WriteString(Tamer.Partner.Name);
                writer.WriteShort(Tamer.Partner.Size);
                writer.WriteByte(0);
                writer.WriteByte(0);
                writer.WriteByte(0);
                writer.WriteByte(0);
                writer.WriteByte(0);
                writer.WriteByte(0);
            }
            writer.WriteByte(99);
            return writer;
        }

        public PacketWriter CharList_0chars()
        {
            PacketWriter writer = new PacketWriter();
            writer.Type(1301);
            writer.WriteByte(99);
            return writer;
        }
        public void Process(CharClient client, byte[] buffer)
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

            switch (packet.Type)
            {
                case -1:
                    {
                        PacketWriter writer = new PacketWriter();
                        short time_t = (short)(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                        short data = (short)(client.Client.Handshake ^ 0x7e41);
                        writer.Type(-2);
                        writer.WriteShort(data);
                        writer.WriteInt(time_t);
                        Client.Send(writer.Finalize());
                        break;
                    }
                case -3:
                    {
                        break;
                    }
                case 1706:
                    {
                        packet.Seek(8);

                        uint AcctID = packet.ReadUInt();
                        int uniID = packet.ReadInt();

                        _characterDatabase.LoadUser(client, AcctID, uniID);
                        List<Character> list_of_tamers = _characterDatabase.GetCharacters(AcctID);
                        if (list_of_tamers.Count > 0)
                        {
                            Client.Send(CharList(list_of_tamers).Finalize());
                        }
                        else if (list_of_tamers.Count == 0)
                        {
                            Client.Send(CharList_0chars().Finalize());
                        }
                        break;
                    }
                case 1302:
                    {
                        string check_name = packet.ReadString();

                        PacketWriter writer = new PacketWriter();

                        writer.Type(1302);

                        if (_characterDatabase.NameAvail(check_name))
                        {
                            writer.WriteInt(1);
                            Client.Send(writer.Finalize());
                        }
                        else
                        {
                            writer.WriteInt(0);
                            Client.Send(writer.Finalize());

                        }

                        break;
                    }
                case 1303:
                    {
                        int position = packet.ReadByte();
                        int model = packet.ReadInt();
                        string tamer_name = packet.ReadZString();

                        packet.Seek(42);

                        int digi_model = packet.ReadInt();
                        string digi_name = packet.ReadZString();

                        int charID = _characterDatabase.CreateCharacter(position, client.AccountID, position, model, tamer_name, digi_model);
                        int digiID = (int)_characterDatabase.CreateDigimon((uint)charID, digi_name, digi_model);
                        _characterDatabase.SetPartner(charID, digiID);
                        _characterDatabase.SetTamer(charID, digiID);

                        PacketWriter writer = new PacketWriter();

                        int time_t = (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                        short handshake = (short)(time_t & 0xFFFF);
                        writer.Type(0x051A);
                        writer.WriteShort(handshake);
                        writer.WriteShort(0);
                        writer.WriteShort(0);
                        writer.WriteShort(0);
                        writer.WriteShort(handshake);
                        writer.WriteShort(0);
                        writer.WriteShort(0);
                        writer.WriteShort(0);
                        writer.WriteByte((byte)position);
                        writer.WriteByte(0x3);
                        writer.WriteByte(0);
                        writer.WriteInt(model);
                        writer.WriteByte(0x1);
                        writer.WriteString(tamer_name);
                        for (int i = 0; i < 884; i++)
                        {
                            writer.WriteByte(0);
                        }
                        writer.WriteInt(digi_model);
                        writer.WriteByte(0x1);
                        writer.WriteString(digi_name);
                        writer.WriteByte(0);
                        writer.WriteByte(0);
                        writer.WriteByte(0);
                        writer.WriteByte(0);
                        writer.WriteByte(0);
                        writer.WriteByte(0);
                        writer.WriteByte(0);
                        writer.WriteByte(0);
                        Client.Send(writer.Finalize());
                        //File.WriteAllBytes("C:\\testt.packet", writer.Finalize());

                        break;
                    }
                case 1304:
                    {
                        PacketWriter writer = new PacketWriter();
                        writer.Type(1304);
                        int slot = packet.ReadInt();
                        string code = packet.ReadString();
                        bool canDelete = _characterDatabase.VerifyCode(client.AccountID, code);

                        if (canDelete)
                        {
                            if (_characterDatabase.DeleteTamer(client.AccountID, slot))
                            {
                                writer.WriteInt(1);
                                Client.Send(writer.Finalize());
                            }
                            else
                            {
                                writer.WriteInt(0);
                                Client.Send(writer.Finalize());
                            }
                        }
                        else
                        {
                            writer.WriteInt(2);
                            Client.Send(writer.Finalize());
                        }
                        break;
                    }
                case 1305:
                    {
                        int g_slot = packet.ReadInt();
                        Position pLoc = null;
                        try
                        {
                            _characterDatabase.SetLastChar(client.AccountID, g_slot);
                            pLoc = _characterDatabase.GetTamerPosition(client.AccountID, g_slot);
                        }
                        catch (Exception e)
                        {
                            SysCons.LogError("{0}", e);
                        }

                        PacketWriter writer = new PacketWriter();
                        writer.Type(1308);
                        writer.WriteString("127.0.0.1");
                        writer.WriteInt(7031);
                        writer.WriteInt(pLoc.Map);
                        writer.WriteString(pLoc.MapName);   // Encoding.ASCII = char 


                        Client.Send(writer.Finalize());

                        break;
                    }
                case 1703:
                    {
                        PacketWriter writer = new PacketWriter();

                        writer.Type(1703);

                        Client.Send(writer.Finalize());
                    }
                    break;
                default:
                    //Unknown Packets!

                    PacketDefinitions.LogPacketData(packet);
                    break;


            }

        }
    }
}
