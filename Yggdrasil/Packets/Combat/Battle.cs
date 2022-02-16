using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game
{
    public class Battle : Packet
    {

        public Battle(int user, int target)
        {
            packet.Type(1013);
            packet.WriteInt(user);
            packet.WriteInt(target);

        }

        public Battle(int check, int target, ushort damage, short hp_after_damage, short original_hp)
        {
            packet.Type(1013);
            packet.WriteInt(check);
            packet.WriteInt(target); // short or byte 
            packet.WriteUShort(damage);//Possibly byte Int
            packet.WriteShort(0);
            packet.WriteShort(hp_after_damage);
            packet.WriteShort(0);
            packet.WriteInt(original_hp);
            packet.WriteShort(0);
        }
    }
}