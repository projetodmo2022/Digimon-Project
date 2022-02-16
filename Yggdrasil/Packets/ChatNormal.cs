using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game.Chat
{
    public class ChatNormal : Packet
    {
        public ChatNormal(uint hSpeaker, string message)
        {
            packet.Type(1006);
            packet.WriteShort((short)ChatType.Normal);
            packet.WriteUInt(hSpeaker);
            packet.WriteString(message);
            packet.WriteByte(0);
        }

    }
}