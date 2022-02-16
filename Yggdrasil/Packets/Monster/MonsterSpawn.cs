using Yggdrasil.Database;
using Yggdrasil.Entities;
using Yggdrasil.Helpers;

namespace Yggdrasil.Packets.Monster
{
    public class MonsterSpawn : Packet
    {
        public MonsterSpawn(DigimonData monster, Position targetPosition, int handle)
        {
            packet.Type(1006);
            packet.WriteByte(3);
            packet.WriteShort(1);
            packet.WriteInt(targetPosition.PosX);
            packet.WriteInt(targetPosition.PosY);
            packet.WriteInt(handle);
            packet.WriteInt(monster.Species);
            packet.WriteInt(targetPosition.PosX + 1000 + 50);
            packet.WriteInt(targetPosition.PosY + 50);
            packet.WriteByte(0xff);
            packet.WriteShort((short)10);
            packet.WriteShort(2);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteByte(0);
        }
    }
}