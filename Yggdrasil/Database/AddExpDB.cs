using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
    public class AddExpDB
    {
        public static Dictionary<int, sAddExp> AddExpList = new Dictionary<int, sAddExp>();
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
                        sAddExp addExp = new sAddExp();
                        addExp.s_nID = read.ReadInt();

                        addExp.s_nSkillID = read.ReadInt();
                        addExp.s_nIconID = read.ReadInt();
                        addExp.s_szTitle = read.ReadZString(Encoding.Unicode, 64 * 2);   // wchar_t colocar valor multiplicar por 2
                        addExp.s_szComment = read.ReadZString(Encoding.Unicode, 512 * 2);   // wchar_t colocar valor multiplicar por 2
                        if (!AddExpList.ContainsKey(addExp.s_nID))
                        {
                            AddExpList.Add(addExp.s_nID, addExp);
                        }
                    }
                }
            }

            SysCons.LogDB("AddExp.bin", "Loaded {0} Exp.", AddExpList.Count);
        }

    }

    public class sAddExp
    {
        public int s_nID;
        public int s_nSkillID;
        public int s_nIconID;

        public string s_szTitle;
        public string s_szComment;

    }
}
