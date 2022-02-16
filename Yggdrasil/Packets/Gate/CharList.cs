using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Entities;

namespace Yggdrasil.Packets.Game
{
    public class CharList : Packet
    {
        public CharList(List<Character> listTamers)
        {
            packet.Type(1301);
            foreach (Character Tamer in listTamers)
            {
                packet.WriteByte((byte)Tamer.CharacterPos);
                packet.WriteShort((short)Tamer.Location.Map);
                packet.WriteInt((int)Tamer.Model);
                packet.WriteByte((byte)Tamer.Level);
                packet.WriteString(Tamer.Name);
                for (int i = 0; i < 13; i++)
                {
                    Item item = Tamer.Equipment[i];
                    packet.WriteBytes(item.ToArray());
                }
                packet.WriteBytes(Tamer.ChipSets.ToArray());
                packet.WriteBytes(Tamer.Equipment[13].ToArray());
                packet.WriteBytes(new byte[68 * 5]);
                packet.WriteBytes(new byte[768]);
                packet.WriteBytes(new byte[140]);
                packet.WriteInt(Tamer.Incubator);
            }
            packet.WriteByte(99);
        }

    }
}
