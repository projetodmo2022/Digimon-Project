using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;


namespace Yggdrasil.Packets.Game
{
    public class Status:Packet
    {
        public Status(short hMap, DigimonStats stats)
        {
            packet.Type(1023);
            packet.WriteInt(hMap);
            packet.WriteShort(stats.HP);
            packet.WriteShort(stats.DS);
            packet.WriteInt(0);
        }
    }
}
