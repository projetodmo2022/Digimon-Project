using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game
{
    public class UpdateLevel : Packet
    {
        public UpdateLevel(uint Handle, byte Level)
        {
            packet.Type(1019);
            packet.WriteUInt(Handle);
            packet.WriteByte(Level);
        }

        public UpdateLevel(short Handle, byte Level)
        {
            packet.Type(1019);
            packet.WriteInt(Handle);
            packet.WriteByte(Level);
        }
    }
}