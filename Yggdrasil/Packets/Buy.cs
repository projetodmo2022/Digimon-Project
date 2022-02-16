using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Entities;

namespace Yggdrasil.Packets.Game
{
    public class Buy : Packet
    {
        public Buy(int npc, short slot, short amount, int money)
        {
            packet.Type(3915);
            packet.WriteInt(money);
        }

    }
    public class Repurchase : Packet
    {
        public Repurchase()
        {
            packet.Type(3979);
            packet.WriteInt(0);
        }

    }

    public class Resell : Packet
    {
        public Resell(byte u1, byte slot)
        {
            packet.Type(3916);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteByte(u1);
            packet.WriteByte(slot);
            packet.WriteInt(0);
            packet.WriteShort(0);
        }

    }
}