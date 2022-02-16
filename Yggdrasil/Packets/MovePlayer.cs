using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Entities;

namespace Yggdrasil.Packets.Game
{
    public class MovePlayer : Packet
    {
        public MovePlayer(Character Tamer, int X, int Y, short u)
        {
            Digimon Partner = Tamer.Partner;
            packet.Type(1006);
            packet.WriteShort(u);
            packet.WriteByte(0);
            packet.WriteUInt(Tamer.UID);
            packet.WriteInt(Tamer.Location.PosX);
            packet.WriteInt(Tamer.Location.PosX);
            packet.WriteUInt(Tamer.DigimonUID);
            packet.WriteInt(Tamer.Partner.Location.PosX);
            packet.WriteInt(Tamer.Partner.Location.PosY);
            packet.WriteByte(0);
        }

        public MovePlayer(Character Tamer, int X, int Y)
        {
            Digimon Partner = Tamer.Partner;
            packet.Type(1006);
            packet.WriteShort(0x0205);
            packet.WriteByte(0);
            packet.WriteUInt(Tamer.UID);
            packet.WriteInt(X);
            packet.WriteInt(Y);
            packet.WriteUInt(Tamer.DigimonUID);
            packet.WriteInt(Partner.Location.PosX);
            packet.WriteInt(Partner.Location.PosY);
            packet.WriteInt(0);
        }
    }
}