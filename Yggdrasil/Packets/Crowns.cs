using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game
{
    public class Crowns : Packet
    {
        public Crowns(int crowns)
        {
            packet.Type(3404);
            packet.WriteInt(0);
            packet.WriteInt(crowns);
            packet.WriteInt(0);
        }

    }

    public class Cashshop : Packet
    {
        public Cashshop()
        {
            packet.Type(3412);
            packet.WriteByte(0);
        }

        public Cashshop(byte u1)
        {
            packet.Type(3415);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteShort(0);
        }
    }
}