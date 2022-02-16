using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Entities;

namespace Yggdrasil.Packets.Game
{
    public class UpdateEquipment : Packet
    {
        /// <summary>
        /// Update Model?
        /// </summary>
        /// <param name="Slot"></param>
        public UpdateEquipment(uint UID, short Slot)
        {
            packet.Type(1310);
            packet.WriteUInt(UID);
            packet.WriteInt(Slot);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteShort(0);
        }
    }
}