using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
    public class SkillDB
    {
        public static Dictionary<int, SkillData> SkillList = new Dictionary<int, SkillData>();

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
                        read.Seek(4 + i * 736);
                        SkillData skill = new SkillData();
                        skill.SkillID = read.ReadInt();
                        skill.DisplayName = read.ReadZString(Encoding.Unicode, 64);
                        skill.Desc = read.ReadZString(Encoding.Unicode, 224);
                        SkillList.Add(skill.SkillID, skill);
                    }
                }
            }
            SysCons.LogDB("Skill.bin", "Loaded {0} skills.", SkillList.Count);
        }

        public static SkillData GetSkill(int skillID)
        {
            if (SkillList.ContainsKey(skillID))
                return SkillList[skillID];
            else
                return null;
        }
    }

    public class SkillData
    {
        public int SkillID;
        public string Desc;
        public string DisplayName;

        public override string ToString()
        {
            return string.Format("{1} {0}", DisplayName, SkillID);
        }
    }
}
