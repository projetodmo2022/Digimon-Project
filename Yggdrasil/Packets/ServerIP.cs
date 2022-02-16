using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game
{
    public class ServerIP : Packet
    {
        public ServerIP(string IP, int Port, int MapId, string Map)
        {
            packet.Type(1308);
            packet.WriteString(IP);
            packet.WriteInt(Port);
            packet.WriteInt(MapId);
            packet.WriteString(Map);
        }
    }
}
