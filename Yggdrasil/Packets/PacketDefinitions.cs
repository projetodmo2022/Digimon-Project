using System;
using Digital_World;

namespace Yggdrasil.Packets
{
    public class PacketDefinitions
    {
        public static string getPacketName(ushort opcode) {
            if (PacketDefinitions.IsDefined(opcode))
            {
                return ((PacketOpcodes)opcode).ToString();
            }
            else {
                return String.Format("UNKNOW_PACKET_{0}", opcode);
            }
        }

        public static bool IsDefined(ushort opcode) {
            return Enum.IsDefined(typeof(PacketOpcodes), opcode);
        }

        public static void LogPacketData(PacketReader pkt) {
            //SysCons.LogWarn("Recv Packet({0}) Type({4}) Len({1}) Enc({2}) Opcode({3})", PacketDefinitions.getPacketName((ushort)pkt.Type), pkt.Length, 1, pkt.Type, pkt.GetType());
            SysCons.SavePacket(pkt);
        }
    }
}
