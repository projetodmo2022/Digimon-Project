using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Database;

namespace Yggdrasil.Packets.Game
{
    /// <summary>
    /// Map Change Packet Type 1709
    /// </summary>
    public class MapChange : Packet
    {
        public MapChange(string IP, int Port, Portal Portal, string Map)
        {
            packet.Type(1709);
            packet.WriteString(IP);
            packet.WriteInt(Port);
            packet.WriteInt(Portal.MapId);
            packet.WriteInt(Portal.uInts1[1]);           //X?
            packet.WriteInt(Portal.uInts1[2]);           //Y?
            packet.WriteString(Map);
        }

        public MapChange(string IP, int Port, int MapId, int X, int Y, string Map)
        {
            packet.Type(1709);
            packet.WriteString(IP);
            packet.WriteInt(Port);
            packet.WriteInt(MapId);
            packet.WriteInt(X);
            packet.WriteInt(Y);
            packet.WriteString(Map);
        }
    }
}
