using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game
{
    public class ChangeSize : Packet
    {
        public enum ChangeType
        {
            /// <summary>
            /// A permanent size change.
            /// </summary>
            Permanent = 0,
            /// <summary>
            /// A temporary size change. Lasts 3 min
            /// </summary>
            Temporary = 2
        }

        public ChangeSize(uint handle, int Size, ChangeType Type)
        {
            packet.Type(9942);
            packet.WriteUInt(handle);
            packet.WriteInt(Size);
            packet.WriteShort((short)Type); //Unknown
        }
    }
}