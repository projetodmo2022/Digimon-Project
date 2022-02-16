using Yggdrasil.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game
{
    public class Inventory:Packet
    {
        public Inventory(Character Tamer)
        {
            packet.Type(16038);
            packet.WriteInt(0);
            packet.WriteInt(Tamer.Money);
            packet.WriteInt(0);
            packet.WriteByte(0);
            packet.WriteShort((short)Tamer.InventorySize);
            packet.WriteBytes(Tamer.Inventory.ToArray());
        }
    }

    public class Storage : Packet
    {
        public Storage(Character Tamer)
        {
            packet.Type(16038);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteByte(1);
            packet.WriteShort((short)Tamer.StorageSize);
            packet.WriteBytes(Tamer.Storage.ToArray());
        }
    }

    public class GiftStorage : Packet
    {
        public GiftStorage(Character Tamer)
        {
            packet.Type(3935);
            packet.WriteShort(0);
        }
    }

    public class RewardStorage : Packet
    {
        public RewardStorage(Character Tamer)
        {
            packet.Type(16001);
            packet.WriteInt(0);
        }
    }

    public class StorageExpand:Packet
    {
        public StorageExpand(Character Tamer)
        {
            packet.Type(3925);
            packet.WriteInt(Tamer.StorageSize);
        }
    }

    public class InventoryExpand : Packet
    {
        public InventoryExpand(Character Tamer)
        {
            packet.Type(3924);
            packet.WriteInt(Tamer.InventorySize);
        }
    }

    public class MercenarySlotExpand : Packet
    {
        public MercenarySlotExpand(Character Tamer)
        {
            packet.Type(1102);
            packet.WriteInt(Tamer.mercenaryLimit);
        }
    }
    public class ArchiveExpand : Packet
    {
        public ArchiveExpand(Character Tamer)
        {
            packet.Type(3926);
            packet.WriteInt(Tamer.ArchiveSize);
        }
    }

    public class BoxToItems : Packet
    {
        public BoxToItems(Character Tamer, int UsedItem, int slot, int itemcount, uint[] Items, int[] amounts)
        {
            packet.Type(3948);
            packet.WriteInt(UsedItem);
            packet.WriteInt(0);
            packet.WriteShort((short)slot);
            packet.WriteInt(itemcount);
            for (int i = 0; i < itemcount; i++)
            {
                packet.WriteUInt(Items[i]);
                packet.WriteInt(amounts[i]);
                packet.WriteInt(0);
            }
        }
    }
}
