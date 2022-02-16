using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game
{
    public class RidingMode : Packet
    {
        public RidingMode(uint hTamer, uint hDigimon)
        {
            packet.Type(1325);
            packet.WriteUInt(hTamer);
            packet.WriteUInt(hDigimon);
        }
    }
    public class StopRideMode : Packet
    {
        public StopRideMode(uint hTamer, uint hDigimon)
        {
            packet.Type(1326);
            packet.WriteUInt(hTamer);
            packet.WriteUInt(hDigimon);
        }
    }
}