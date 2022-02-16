using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Entities;

namespace Yggdrasil.Packets.Game
{
    public class CashWHItem : Packet
    {

        //WITHDRAW - FROM CASHWAREHOUSE TO INVENTORY
        public CashWHItem(byte type, int slot, Item item, int amount, int max)
        {

            packet.Type(3936);
            packet.WriteByte(type); //original value = 0
            packet.WriteByte((byte)slot);
            packet.WriteInt(item.ItemId);
            packet.WriteShort((short)amount);
            packet.WriteByte(1);
            packet.WriteInt(item.time_t);
            packet.WriteInt(0);
        }

        public CashWHItem(int invslot,int slot, Item item, int amount, int max)
        {

            packet.Type(3931);
            packet.WriteInt(invslot);
            packet.WriteInt(0);
            packet.WriteInt(slot);
            packet.WriteInt(item.ItemId);
            packet.WriteInt(amount);
            packet.WriteByte(0);
            packet.WriteInt(item.time_t);
            packet.WriteInt(0);
        }
    }
}