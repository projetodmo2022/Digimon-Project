using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets.Game
{
    public class ShopStorage : Packet
    {
        public ShopStorage()
        {
            packet.Type(1523);
            packet.WriteInt(0);
            packet.WriteInt(0);
        }
    }

    public class ConsignmentShop : Packet
    {
        public ConsignmentShop()
        {
            packet.Type(1510);
            packet.WriteInt(5);
        }
    }
}
