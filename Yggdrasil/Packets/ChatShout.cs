using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game.Chat
{
    public class ChatShout :Packet
    {
        public ChatShout(short hSpeaker, string message)
        {
            packet.Type(1006);
            packet.WriteShort((short)ChatType.Shout);
            packet.WriteShort(hSpeaker);
            packet.WriteString(message);
            packet.WriteByte(0);
        }
    }
}
