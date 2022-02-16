using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game
{
    public class Channels:Packet
    {
        public Channels()
        {
            packet.Type(1713);
            packet.WriteShort(1);
            packet.WriteByte(0xFF);
        }
    }

    public class UpdateEXP : Packet
    {
        public UpdateEXP(int exp, int theme)
        {
            packet.Type(0x041E);
            packet.WriteInt(1);
            packet.WriteInt(exp);
            packet.WriteInt(theme); //1 - 2 - 3
            packet.WriteInt(0);
            packet.WriteInt(100);
        }
    }

    public class XGauge : Packet
    {
        public XGauge(int xgauge ,short type)
        {
            packet.Type(16033);
            packet.WriteInt(xgauge);
            packet.WriteShort(type);
        }

        public XGauge(short type, int xgauge)
        {
            packet.Type(16032);
            packet.WriteInt(xgauge);
            packet.WriteShort(type);
        }
    }
    public class Packet390C : Packet
    {
        public Packet390C()
        {
            packet.Type(3129);
            packet.WriteByte(0);
            packet.WriteShort(9);
        }
    }
}
