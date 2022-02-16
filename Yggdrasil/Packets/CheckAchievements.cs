using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Entities;

namespace Yggdrasil.Packets.Game.Interface
{
    public class Achievements : Packet
    {
        public Achievements()
        {

            packet.Type(3204);
            packet.WriteInt(0);
            packet.WriteInt(51);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(1888);
        }
    }

    public class EquipAchievement : Packet
    {
        public EquipAchievement(short title, uint btamerhandle)
        {

            packet.Type(15);
            packet.WriteUInt(btamerhandle);
            packet.WriteShort(title);
        }
    }

    public class Encyclopedia : Packet
    {
        public Encyclopedia()
        {

            packet.Type(3234);
            packet.WriteInt(1);
            packet.WriteInt(31029);
            packet.WriteShort(1);
            packet.WriteShort(7);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteShort(8242);
            packet.WriteByte(0);
        }
    }

    public class Incubator : Packet
    {
        public Incubator()
        {

            packet.Type(3226);
            packet.WriteInt(1);
            packet.WriteInt(31029);
            packet.WriteShort(1);
            packet.WriteShort(7);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteShort(8242);
            packet.WriteByte(0);
        }
    }
}