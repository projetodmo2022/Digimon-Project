using System;
using Yggdrasil.Network;
using Yggdrasil.Packets;

namespace AuthServer.Network
{
    /// <summary>
    /// Description of AuthClient.
    /// </summary>
    public class AuthClient : IUser
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
        public int UniqueID;
        public int AccessLevel;
        public string Username;
        public string Password;
        public string SecondaryPassword;
        public int Characters;
        
		public AuthClient(IClient client)
		{
			Client = client;
		}

        public void SendHandShakeRes()
        {
            //int time_t = (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            short time_t = (short)(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            short data = (short)(Client.Handshake ^ 0x7e41);
            PacketWriter writer = new PacketWriter();
            writer.Type(-2);
            writer.WriteShort(data);
            writer.WriteShort(time_t);
            Client.Send(writer.Finalize());
        }

        public void SendLoginDisconnectResponse()
        {
   
        }

        public void SendLoginResponse(byte[] data)
        {
  
        }
	}
}
