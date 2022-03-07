using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Yggdrasil.Entities;
using Digital_World;


namespace Yggdrasil.Database
{
    public class CuidDB
    {
        public static Dictionary<string, Cuid> Cuid = new Dictionary<string, Cuid>();
        public static Dictionary<string, CuidChat> CuidChat = new Dictionary<string, CuidChat>();

        public static void Load(string fileName)
        {
            if (File.Exists(fileName) == false) return;
            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        Cuid id = new Cuid();
                        id.s_Name = read.ReadZString(Encoding.Unicode, 32 * 2);
                        id.s_MsgType = read.ReadInt();



                        if (!Cuid.ContainsKey(id.s_Name))
                        {
                            Cuid.Add(id.s_Name, id);
                        }

                    }
                    int scount = read.ReadInt();
                    for (int g = 0; g < scount; g++)
                    {
                        CuidChat chat = new CuidChat();
                        chat.s_Name = read.ReadZString(Encoding.Unicode, 32 * 2);
                     



                        if (!CuidChat.ContainsKey(chat.s_Name))
                        {
                            CuidChat.Add(chat.s_Name, chat);
                        }

                    }
                }
            }
            SysCons.LogDB("CuidDB.bin", $"Loaded {Cuid.Count} CuidDB. Loaded {CuidChat.Count} CuidChat.");
        }
    }

    public class Cuid
    {

        public string s_Name;
        public int s_MsgType;

    }
    public class CuidChat
    {

        public string s_Name;





    }

}

