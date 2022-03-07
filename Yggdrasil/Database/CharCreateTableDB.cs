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
    public class CharCreateTableDB
    {
        public static Dictionary<int, CharCreateTable> charCreateTable = new Dictionary<int, CharCreateTable>();
        public static Dictionary<int, DigimonCreateTable> digimonCreateTable = new Dictionary<int, DigimonCreateTable>();

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
                        CharCreateTable create = new CharCreateTable();

                        create.dwTamerID = read.ReadInt();
                        create.m_bShow = read.ReadByte();
                        create.m_bEnable = read.ReadByte();
                        create.m_nSeasonType = read.ReadInt();
                        create.m_sVoiceSize = read.ReadInt();
                        create.m_nIconIdx = read.ReadInt();
                        create.CountDC = read.ReadInt();
                        for (int h = 0; h < create.CountDC; h++)
                        {
                            create.ItemID = read.ReadInt();
                        }

                        if (!charCreateTable.ContainsKey(create.dwTamerID))
                        {
                            charCreateTable.Add(create.dwTamerID, create);
                        }

                    }
                    int digcount = read.ReadInt();
                    for (int a = 0; a < digcount; a++)
                    {
                        DigimonCreateTable create = new DigimonCreateTable();

                        create.m_digimonID = read.ReadInt();
                        create.d_bShow = read.ReadByte();
                        create.d_bEnable = read.ReadByte();
                        create.m_sVoiceSize = read.ReadInt();
                        
                        if (!digimonCreateTable.ContainsKey(create.m_digimonID))
                        {
                            digimonCreateTable.Add(create.m_digimonID, create);
                        }

                    }

                }
            }
            SysCons.LogDB("CharCreateTable.bin", $"Loaded {charCreateTable.Count} CharCreateTable. Loaded {digimonCreateTable.Count} DigimonCreateTable.");
        }




        public class CharCreateTable
        {
            public int tcount;
            public int dwTamerID;
            public byte m_bShow;   //bool
            public byte m_bEnable; //bool
            public int m_nSeasonType;
            public int m_sVoiceSize;
            public string m_sVoiceFile;   //std::string	
            public int m_nIconIdx;
            public int CountDC;
            public int ItemID;

        }

        public class DigimonCreateTable
        {
            public int m_digimonID;
            public byte d_bShow;
            public byte d_bEnable;
            public int m_sVoiceSize;
            public char m_sVoiceFile;

        }

    }


}

