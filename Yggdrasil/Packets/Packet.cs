using Yggdrasil.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Packets
{
    public interface IPacket
    {
        byte[] ToArray();
    }

    public class Packet : IPacket
    {
        internal PacketWriter packet;

        public Packet()
        {
            packet = new PacketWriter();
        }

        public byte[] ToArray()
        {
            return packet.Finalize();
        }

        public byte[] ToBuffer()
        {
            return packet.PacketToBuffer();
        }

        public string Visualize()
        {
            return Visualize(packet.Finalize());
        }

        public static string Visualize(byte[] buffer)
        {
            StringBuilder sb = new StringBuilder();
            int rows = (int)Math.Ceiling(buffer.Length / 16.0);
            for (int i = 0; i < rows; i++)
            {
                StringBuilder text = new StringBuilder();
                for (int col = 0; col < 16; col++)
                {
                    int pos = col + (i * 16);
                    if (pos < buffer.Length)
                    {
                        sb.Append(buffer[pos].ToString("X2"));
                        if (buffer[pos] > 32 && buffer[pos] < 126)
                            text.Append((char)buffer[pos]);
                        else
                            text.Append(".");
                    }
                    else
                    {
                        sb.Append("  ");
                        text.Append(" ");
                    }
                    sb.Append(" ");
                }
                sb.Append("    ");
                sb.AppendLine(text.ToString());
            }
            return sb.ToString();
        }
    }
}
