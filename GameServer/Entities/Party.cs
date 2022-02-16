using System.Collections.Generic;
using GameServer.Network;

namespace GameServer.Entities
{
    public class Party
    {

        public List<GameClient> Clients = new List<GameClient>(4);

        public Party() { }

        public Party(string LeaderName)
        {
        }

        public void Send(byte[] buffer)
        {
            foreach(GameClient client in Clients)
            {
                client.Client.Send(buffer);
            }
        }

        public void EnterParty(GameClient client)
        {
            Clients.Add(client);
        }
        public void LeaveParty(GameClient client)
        {
            Clients.Remove(client);
        }
    }
}
