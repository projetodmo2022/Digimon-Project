using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Yggdrasil.Entities;

namespace Yggdrasil.Helpers
{
    public class Utils
    {
        private static readonly DateTime REFERENCE = new DateTime(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        public static ulong GetTimestamp(DateTime dt)
        {
            return Convert.ToUInt64((dt - REFERENCE).TotalSeconds);
        }

        public static Random Rand = new Random();

        public static uint GetModel(uint Model)
        {
            uint hEntity = Model;
            return (uint)(hEntity + Rand.Next(1, 255));
        }

        public static uint GetModel(uint Model, byte Id)
        {
            uint hEntity = Model;
            return (uint)(hEntity + Id);
        }

        public static short GetHandle(uint Model, byte type)
        {
            byte[] b = new byte[] { (byte)((Model >> 32) & 0xFF), type };
            return BitConverter.ToInt16(b, 0);
        }

        /*
        public static void MakeHandles(Character Tamer, uint time_t)
        {
            Tamer.intHandle = (uint)(Tamer.ProperModel + Rand.Next(1, 255));

            for (int i = 0; i < Tamer.DigimonList.Count; i++)
            {
                if (Tamer.DigimonList[i] == null) continue;
                Digimon mon = Tamer.DigimonList[i];
                mon.Model = GetModel(mon.ProperModel());
            }
            Tamer.DigimonUID = Tamer.Partner.UID;
        }
        */
        /*
         *  from: http://www.codeproject.com/Articles/36747/Quick-and-Dirty-HexDump-of-a-Byte-Array
         */
        public static string HexDump(byte[] bytes, int length = 0, int bytesPerLine = 16)
        {
            //if (bytes == null) return "<null>";
            int bytesLength;
            if (length == 0)
                bytesLength = bytes.Length;
            else
                bytesLength = length;

            char[] HexChars = "0123456789ABCDEF".ToCharArray();

            int firstHexColumn =
                  8                   // 8 characters for the address
                + 3;                  // 3 spaces

            int firstCharColumn = firstHexColumn
                + bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
                + (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
                + 2;                  // 2 spaces 

            int lineLength = firstCharColumn
                + bytesPerLine           // - characters to show the ascii value
                + Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

            char[] line = (new String(' ', lineLength - 2) + Environment.NewLine).ToCharArray();
            int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
            StringBuilder result = new StringBuilder(expectedLines * lineLength);

            for (int i = 0; i < bytesLength; i += bytesPerLine)
            {
                line[0] = HexChars[(i >> 28) & 0xF];
                line[1] = HexChars[(i >> 24) & 0xF];
                line[2] = HexChars[(i >> 20) & 0xF];
                line[3] = HexChars[(i >> 16) & 0xF];
                line[4] = HexChars[(i >> 12) & 0xF];
                line[5] = HexChars[(i >> 8) & 0xF];
                line[6] = HexChars[(i >> 4) & 0xF];
                line[7] = HexChars[(i >> 0) & 0xF];

                int hexColumn = firstHexColumn;
                int charColumn = firstCharColumn;

                for (int j = 0; j < bytesPerLine; j++)
                {
                    if (j > 0 && (j & 7) == 0) hexColumn++;
                    if (i + j >= bytesLength)
                    {
                        line[hexColumn] = ' ';
                        line[hexColumn + 1] = ' ';
                        line[charColumn] = ' ';
                    }
                    else
                    {
                        byte b = bytes[i + j];
                        line[hexColumn] = HexChars[(b >> 4) & 0xF];
                        line[hexColumn + 1] = HexChars[b & 0xF];
                        line[charColumn] = (b < 32 ? '·' : (char)b);
                    }
                    hexColumn += 3;
                    charColumn++;
                }
                result.Append(line);
            }
            return result.ToString();
        }
    }
}
