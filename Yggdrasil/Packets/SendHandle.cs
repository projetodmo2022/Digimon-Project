using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game
{
    public class SendHandle:Packet
    {
        public SendHandle(uint Handle)
        {
            packet.Type(1016);
            packet.WriteUInt(Handle);
        }
    }
}
