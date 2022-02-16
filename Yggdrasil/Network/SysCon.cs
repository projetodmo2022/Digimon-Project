using System;
using System.IO;
using Yggdrasil.Packets;

namespace Digital_World
{
    public static class SysCons
    {
        public static void LogInfo(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[{0}] ", DateTime.Now.ToString(@"MM/dd/yyyy HH:mm:ss"));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[INFO] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            if (args.Length == 0)
                Console.WriteLine(text);
            else
                Console.WriteLine(text, args);
        }

        public static void LogWarn(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[{0}] ", DateTime.Now.ToString(@"MM/dd/yyyy HH:mm:ss"));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[WARN] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            if (args.Length == 0)
                Console.WriteLine(text);
            else
                Console.WriteLine(text, args);
        }

        public static void LogDB(string text2, string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[{0}] ", DateTime.Now.ToString(@"MM/dd/yyyy HH:mm:ss"));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"[{text2}] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            if (args.Length == 0)
                Console.WriteLine(text);
            else
                Console.WriteLine(text, args);
        }

        public static void LogError(string text, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[{0}] ", DateTime.Now.ToString(@"MM/dd/yyyy HH:mm:ss"));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            if (args.Length == 0)
                Console.WriteLine(text);
            else
                Console.WriteLine(text, args);
        }
        
        public static void SavePacket(PacketReader pkt)
        {
            string path = @".\packets\";
            string filename = String.Format(
                "{0}_{1}.dat",
                PacketDefinitions.getPacketName((ushort)pkt.Type),
                DateTime.Now.ToString(@"MM-dd-yyyy_HH-mm-ss")
            );
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                FileStream fs = new FileStream(path + filename, FileMode.OpenOrCreate);
                fs.Write(pkt.ToArray(), 0, pkt.Length);
                fs.Close();
            }
            catch(Exception ex)
            {
                SysCons.LogError("Exception: {0}", ex);
            }
        }
    }
}
