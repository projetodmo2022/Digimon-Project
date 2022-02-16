using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Entities;

namespace Yggdrasil.Packets.Game
{
    public class DespawnPlayer : Packet
    {
        public DespawnPlayer(uint hTamer, uint hDigimon)
        {
            packet.Type(1006);
            packet.WriteShort(514);
            packet.WriteByte(0);
            packet.WriteUInt(hTamer);
            packet.WriteUInt(hDigimon);
            packet.WriteByte(0);
        }
    }
}