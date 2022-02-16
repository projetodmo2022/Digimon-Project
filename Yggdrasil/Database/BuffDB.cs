using System;
using System.Collections.Generic;
using System.IO;
using Yggdrasil.Helpers;
using System.Text;
using Yggdrasil;
using Digital_World;

namespace Yggdrasil.Database
{
    public class BuffDB
    {
        public static Dictionary<int, BuffData> BuffList = new Dictionary<int, BuffData>();

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
                        BuffData buff = new BuffData();
                        buff.BuffID = read.ReadUShort();
                        BuffList.Add(buff.BuffID, buff);
                    }
                }
            }
            SysCons.LogDB("Buff.bin", "Loaded {0} buffs.", BuffList.Count);
        }

        public static BuffData GetBuff(ushort buffID)
        {
            if (BuffList.ContainsKey(buffID))
                return BuffList[buffID];
            else
                return null;
        }
    }

    public class BuffData
    {
        public ushort BuffID;
        public string Desc;
        public string DisplayName;

        public override string ToString()
        {
            return string.Format("{1} {0}", DisplayName, BuffID);
        }
    }
}
