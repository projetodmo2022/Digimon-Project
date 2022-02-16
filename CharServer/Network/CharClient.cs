using System;
using System.Collections.Generic;
using Yggdrasil.Entities;
using Yggdrasil.Network;
using Yggdrasil.Packets;

namespace CharServer.Network
{
    public class CharClient : IUser
	{
		/// <summary>
        /// TCP connection.
        /// </summary>
        private IClient _Client = null;
        public IClient Client {
        	get {return _Client;}
        	set {_Client = value;}
        }

        public uint AccountID;
        public string Username;
        public string Password;
        public int UniqueID;
        public int AccessLevel;
        public List<Character> Chars;
        
		public CharClient(IClient client)
		{
			Client = client;
		}

        public void SendHandShakeRes()
        {
            short time_t = (short)(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            short data = (short)(Client.Handshake ^ 0x7e41);
            PacketWriter writer = new PacketWriter();
            writer.Type(-2);
            writer.WriteShort(data);
            writer.WriteInt(time_t);
            Client.Send(writer.Finalize());
        }

        public void SendLoginResponse(byte[] data)
        {
            
        }

        public void SendServerList(bool isOnlyOne)
        {
            
        }

        public void SendCharacterLoad(byte[] data)
        {
            
        }

        public void SendCharacterCreate(byte[] data)
        {

        }

        public void SendDisconnect(byte[] data)
        {
           
        }

        public void SendConnectWaitCheckResult(byte[] data)
        {
            
        }

        public void SendConnectWaitCancelResult(byte[] data)
        {
           
        }

        public void SendCharacterSelectResult(byte[] data)
        {
           
        }
    }
}
