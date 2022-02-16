using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game
{
    public class DataInputFailure : Packet
    {
        public DataInputFailure(ushort Handle, bool Broken)
        {
            packet.Type(1040);
            packet.WriteInt(Handle);
            packet.WriteByte((byte)(Broken ? 0 : 1));
        }
    }
}
