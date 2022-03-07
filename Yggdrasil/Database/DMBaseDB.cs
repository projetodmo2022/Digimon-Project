using System.Collections.Generic;
using System.Linq;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;

namespace Yggdrasil.Database
{
    public class DMBaseDB
    {
        public static List<DMBaseInfo> TamerBaseInfo = new List<DMBaseInfo>();
        public static List<DigiBaseInfo> DigimonBaseInfo = new List<DigiBaseInfo>();
        public static List<CsBaseMapInfo> CsBaseMapInfo = new List<CsBaseMapInfo>(); 
        public static List<Jumpbooster> JumpBooster = new List<Jumpbooster>();

        public static void Load(string fileName)
        {
            if (File.Exists(fileName) == false) 
                return;

            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    int charHandle = 80001;

                    int count = read.ReadInt();
                    for (int i = 1; i < count+1; i++)
                    {
                        DMBaseInfo dmbaseInfo = new DMBaseInfo();
                        dmbaseInfo.Id = read.ReadInt();
                        dmbaseInfo.Level = read.ReadUShort();
                        dmbaseInfo.Unknow1 = read.ReadUShort();
                        dmbaseInfo.Exp = read.ReadLong()/100;
                        dmbaseInfo.Hp = read.ReadInt();
                        dmbaseInfo.Ds = read.ReadInt();
                        dmbaseInfo.Ms = read.ReadUShort();
                        dmbaseInfo.De = read.ReadUShort();
                        dmbaseInfo.Ev = read.ReadUShort();
                        dmbaseInfo.Ct = read.ReadUShort();
                        dmbaseInfo.At = read.ReadInt();
                        dmbaseInfo.Ht = read.ReadUShort();
                        dmbaseInfo.Unknow2 = read.ReadUShort();
                        dmbaseInfo.characterModel = (BaseInfoCharacterModel)charHandle;

                        if (i % 120 == 0)
                            charHandle++;

                        if (!TamerBaseInfo.Contains(dmbaseInfo))
                            TamerBaseInfo.Add(dmbaseInfo);
                    }

                    int digimonHandle = 31001;
                    int dcount = read.ReadInt();
                    for (int i = 1; i < dcount + 1; i++)
                    {
                        DigiBaseInfo digibaseInfo = new DigiBaseInfo();
                        digibaseInfo.Id = read.ReadInt();
                        digibaseInfo.Level = read.ReadUShort();
                        digibaseInfo.Unknow1 = read.ReadUShort();
                        digibaseInfo.Exp = read.ReadLong() / 100;
                        digibaseInfo.Hp = read.ReadInt();
                        digibaseInfo.Ds = read.ReadInt();
                        digibaseInfo.Ms = read.ReadUShort();
                        digibaseInfo.De = read.ReadUShort();
                        digibaseInfo.Ev = read.ReadUShort();
                        digibaseInfo.Ct = read.ReadUShort();
                        digibaseInfo.At = read.ReadInt();
                        digibaseInfo.Ht = read.ReadUShort();
                        digibaseInfo.Unknow2 = read.ReadUShort();
                        digibaseInfo.digimonModel = (BaseInfoDigimonModel)digimonHandle;

                        if (i % 120 == 0)
                            digimonHandle++;

                        if (!DigimonBaseInfo.Contains(digibaseInfo))
                            DigimonBaseInfo.Add(digibaseInfo);
                    }

                    int scount = read.ReadInt();
                    for (int i = 0; i < scount; i++)
                    {
                        CsBaseMapInfo csbasemapinfo = new CsBaseMapInfo();
                        csbasemapinfo.s_nMapID = read.ReadInt();
                        csbasemapinfo.s_nShoutSec = read.ReadInt();
                        csbasemapinfo.s_bEnableCheckMacro = read.ReadUShort();
                        csbasemapinfo.unk = read.ReadUShort();

                        if (!CsBaseMapInfo.Contains(csbasemapinfo))
                            CsBaseMapInfo.Add(csbasemapinfo);
                    }

                    int jcount = read.ReadInt();
                    for (int j = 0; j < jcount; j++)
                    {
                        Jumpbooster jumpbooster = new Jumpbooster();
                        jumpbooster.dwItemID = read.ReadInt();
                        jumpbooster.mapcount = read.ReadInt();
                        for (int a = 0; a < jumpbooster.mapcount; a++)
                        {
                            jumpbooster.dwMapID = read.ReadInt();
                        }
                        if(!JumpBooster.Contains(jumpbooster))
                            JumpBooster.Add(jumpbooster);
                    }

                }
            }
            SysCons.LogDB("DMBase.bin", "Loaded {0} DMBase.", TamerBaseInfo.Count);
        }

        public static DMBaseInfo GetTamerBaseInfoByExp(int model, int exp)
        {
            return TamerBaseInfo.FirstOrDefault(x => (int)x.characterModel == model && x.Exp > exp * 100);
        }

        public static DMBaseInfo GetTamerBaseInfoByLevel(int model, int level)
        {
            return TamerBaseInfo.FirstOrDefault(x => (int)x.characterModel == model && x.Level == level);
        }
        
        public static DigiBaseInfo GetDigimonBaseInfoByExp(int model, int exp)
        {
            return DigimonBaseInfo.FirstOrDefault(x => (int)x.digimonModel == model && x.Exp > exp * 100);
        }

        public static DigiBaseInfo GetDigimonBaseInfoByLevel(int model, int level)
        {
            return DigimonBaseInfo.FirstOrDefault(x => (int)x.digimonModel == model && x.Level == level);
        }
    }

    public class DMBaseInfo
    {
        public BaseInfoCharacterModel characterModel;
        public int Id;
        public ushort Level;
        public long Exp;
        public int Hp;
        public int Ds;
        public int At;
        public ushort De;
        public ushort Ev;
        public ushort Ct;
        public ushort Ht;
        public ushort Ms;
        public ushort Unknow1;
        public ushort Unknow2;
    }

    public class DigiBaseInfo
    {
        public BaseInfoDigimonModel digimonModel;
        public int Id;
        public ushort Level;
        public long Exp;
        public int Hp;
        public int Ds;
        public int At;
        public ushort De;
        public ushort Ev;
        public ushort Ct;
        public ushort Ht;
        public ushort Ms;
        public ushort Unknow1;
        public ushort Unknow2;
    }

    public class CsBaseMapInfo
    {
       public int s_nMapID;
       public int s_nShoutSec;
       public ushort s_bEnableCheckMacro;
       public ushort unk;
    }

    public class Jumpbooster
    {
        public int dwItemID;
        public int mapcount;
        public int dwMapID;
    }

    public enum BaseInfoCharacterModel
    {
        Masaru = 80001,
        Tohma = 80002,
        Yoshino = 80003,
        Ikuto = 80004,
        Tai = 80005,
        Mimi = 80006,
        Yamato = 80007,
        Takeru = 80008,
        Hikari = 80009,
        Sora = 80010,
        Takato = 80011,
        Rika = 80012,
        Henry = 80013,
        Izzy = 80014,
        Joe = 80015,
        Jeri = 80016,
        Ryo = 80017,
        End
    }

    public enum BaseInfoDigimonModel
    {
        Agumon = 31001,
        Lalamon = 31002,
        Goamon = 31003,
        Falcomon = 31004,
        End
    }

}

