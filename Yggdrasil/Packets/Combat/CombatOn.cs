using Yggdrasil.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game.Combat
{
    public class CombatOn:Packet
    {
        /// <summary>
        /// Toggles combat on
        /// </summary>
        /// <param name="Handle"></param>
        public CombatOn(Character Tamer,int dHandle, int eHandle)
        {
            packet.Type(1006);
            packet.WriteShort(262);
            packet.WriteByte(0);
            packet.WriteInt(eHandle);


            packet.Type(1034);
            packet.WriteInt(dHandle);
            
            packet.Type(2308);
            packet.WriteInt(dHandle);
            packet.WriteInt(9999);
            packet.WriteInt(0);
            
            packet.Type(1101); //Uses a skill lol
            packet.WriteInt(dHandle);
            packet.WriteInt(dHandle);
            packet.WriteShort(1);
            packet.WriteShort(0);
            packet.WriteByte(200);
            packet.WriteByte(3);
            packet.WriteShort(9999);
            packet.WriteByte(0xff);
             
            
            packet.Type(1035);
            packet.WriteInt(dHandle);
            packet.WriteInt(17985);
            packet.WriteInt(0);
             
        }
    }
}
