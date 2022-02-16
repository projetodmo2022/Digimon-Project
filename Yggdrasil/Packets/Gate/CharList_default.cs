using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Entities;

namespace Yggdrasil.Packets
{
    public class CharList_default : Packet
    {
        public CharList_default(List<Character> listTamers)
        {
            packet.Type(1301);

            //Console.WriteLine(packet.ToString());

            byte iChar = 0;
            int pos = 4;
            foreach (Character Tamer in listTamers)
            {
                packet.WriteByte(iChar, pos); pos += 1;
                packet.WriteShort((short)Tamer.Location.Map, pos); pos += 2;
                packet.WriteInt((int)Tamer.Model); pos += 4;
                packet.WriteByte((byte)Tamer.Level); pos += 1;
                packet.WriteString(Tamer.Name); pos += (2 + Tamer.Name.Length);
                /*for (int i = 0; i < 560; i++)
                {

                    packet.WriteByte(0);
                    pos += 1;

                }*/
                for (int i = 0; i < 9; i++)
                {
                    Item item = Tamer.Equipment[i];
                    packet.WriteBytes(item.ToArray());
                    /*packet.WriteInt(0); //Unknown
                    packet.WriteShort((short)item.Attributes[0]);
                    packet.WriteShort((short)item.Attributes[1]);
                    packet.WriteInt(0); //Unknown
                    packet.WriteInt(0); //Unknown
                    packet.WriteInt(0); //Unknown. 0 or -1;
                    */
                }


                packet.WriteInt(Tamer.Partner.Species);
                packet.WriteByte((byte)Tamer.Partner.Level);
                packet.WriteString(Tamer.Partner.Name);
                packet.WriteShort((short)Tamer.Partner.Size); //Partner Size

                iChar++;
            }
            packet.WriteByte(99); pos += 1;
        }
    }
}
