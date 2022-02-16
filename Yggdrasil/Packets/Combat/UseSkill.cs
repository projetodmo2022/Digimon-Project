using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game.Combat
{
    public class UseSkill:Packet
    {
        public UseSkill(int hCaster, int hTarget, byte skillSlot, byte RemainingHP, ushort Damage)
        {
            packet.Type(1101); //Uses a skill lol
            packet.WriteInt(hCaster);
            packet.WriteInt(hTarget);
            packet.WriteShort(skillSlot);
            packet.WriteShort(0);
            packet.WriteByte(RemainingHP);
            packet.WriteByte(3);
            packet.WriteUShort(Damage);
            packet.WriteByte(0xff);
        }
    }
}
