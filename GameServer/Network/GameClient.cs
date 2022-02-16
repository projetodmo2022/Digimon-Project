using static GameServer.Network.GameServ;
using GameServer.Entities;
using Yggdrasil.Entities;
using Yggdrasil.Helpers;
using Yggdrasil.Network;

namespace GameServer.Network
{
    /// <summary>
    /// Description of AuthClient.
    /// </summary>
    public class GameClient : IUser
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
        public uint UniqueID;
        public int AccessLevel;
        public int crowns;
        public uint CharID;
        public uint time_t;

        public int Membership = 0;
        public Character Tamer = null;
        public ItemList CashVault = new ItemList(149);
        public ItemList AccountStorage = new ItemList(14);
        public GameMap ActiveMap = null;
        public Party ActiveParty = null;

        public GameClient(IClient client)
		{
			Client = client;
		}
	}
}
