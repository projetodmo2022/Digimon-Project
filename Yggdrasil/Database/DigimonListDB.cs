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
    public class DigimonListDB
    {
        public static Dictionary<int, DigimonData> Digimon = new Dictionary<int, DigimonData>();
        private static readonly bool IsDebug = false;
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
                            read.Seek(4 + i * 572);

                            DigimonData digiData = new DigimonData();
                            digiData.Species = read.ReadInt();
                            digiData.Model = read.ReadInt();
                            digiData.DisplayName = read.ReadZString(Encoding.Unicode);
                            //Console.WriteLine(digiData.DisplayName);
                            read.Seek(4 + 136 + i * 572);
                            digiData.Name = read.ReadZString(Encoding.ASCII);
                            //Console.WriteLine(digiData.Name);
                            read.Seek(4 + 332 + i * 572);
                            digiData.EvolutionType = read.ReadInt();
                            digiData.AttributeType = read.ReadInt();
                            digiData.Family1 = read.ReadInt();
                            digiData.Family2 = read.ReadInt();
                            digiData.Family3 = read.ReadInt();
                            digiData.NatureType = read.ReadInt();
                            digiData.Nature1 = read.ReadInt();
                            digiData.Nature2 = read.ReadInt();
                            digiData.Nature3 = read.ReadInt();
                            digiData.BaseLevel = read.ReadInt();
                            read.Seek(4 + 372 + i * 572); //240 + 4 = 244 -> NEW BIN
                            digiData.HP = read.ReadShort();
                            //Console.WriteLine(digiData.HP);
                            digiData.DS = read.ReadShort();
                            //Console.WriteLine(digiData.DS);
                            digiData.DE = read.ReadShort();
                            //Console.WriteLine(digiData.DE);
                            digiData.EV = read.ReadShort();
                            //Console.WriteLine(digiData.EV);
                            digiData.MS = read.ReadShort();
                            //Console.WriteLine(digiData.MS);
                            digiData.CR = read.ReadShort();
                            //Console.WriteLine(digiData.CR);
                            digiData.AT = read.ReadShort();
                            //Console.WriteLine(digiData.AT);
                            digiData.AS = read.ReadShort();
                            //Console.WriteLine(digiData.AS);
                            digiData.AR = read.ReadShort();
                            //Console.WriteLine(digiData.uStat);
                            digiData.HT = read.ReadShort();
                            //Console.WriteLine(digiData.HT);
                            digiData.uShort1 = read.ReadShort();
                            // Console.WriteLine(digiData.uShort1);;
                            digiData.BL = 0;
                            read.Skip(4);
                            digiData.Skill1_ID = read.ReadInt();
                            read.Skip(4);
                            digiData.Skill2_ID = read.ReadInt();
                            read.Skip(4);
                            digiData.Skill3_ID = read.ReadInt();
                            read.Skip(4);
                            digiData.Skill4_ID = read.ReadInt();
                            read.Skip(4);
                            digiData.Skill5_ID = read.ReadInt();

                            

                            Digimon.Add(digiData.Species, digiData);


                        }
                    
                }
            }
            SysCons.LogDB("Digimon_List.bin", "Loaded {0} digimons.", Digimon.Count);

            
        }

        public static DigimonData GetDigimon(int Species)
        {
            if (Digimon.ContainsKey(Species))
                return Digimon[Species];
            else
                return null;
        }

        public static int GetEvolutionType(int Species)
        {
            if (Digimon.ContainsKey(Species))
                return Digimon[Species].EvolutionType;
            else
                return -1;
        }

        public static List<int> GetSpecies(string Name)
        {
            List<int> species = new List<int>();
            foreach (KeyValuePair<int, DigimonData> kvp in Digimon)
            {
                DigimonData dData = kvp.Value;
                if (dData.DisplayName.Contains(Name) || dData.Name.Contains(Name))
                    species.Add(dData.Species);
            }
            return species;
        }
    }

    public class DigimonData
    {
        public int Species, Model;
        public int EvolutionType, AttributeType, NatureType;
        public int Family1, Family2, Family3;
        public int Nature1, Nature2, Nature3;
        public int BaseLevel;
        public string Name;
        public string DisplayName;
        public short HP, DS, DE, AS, MS, CR, AT, EV, AR, HT, BL, uShort1;
        public int Skill1_ID, Skill2_ID, Skill3_ID, Skill4_ID, Skill5_ID;

        public DigimonData() { }

        public DigimonStats Stats(short Size)
        {
            DigimonStats Stats = new DigimonStats();

            Stats.MaxHP = (short)((decimal)HP * ((ushort)Size / 10000));
            Stats.HP = (short)((decimal)HP * ((ushort)Size / 10000));
            Stats.MaxDS = (short)((decimal)DS * ((ushort)Size / 10000));
            Stats.DS = (short)((decimal)DS * ((ushort)Size / 10000));
            Stats.DE = (short)((decimal)DE * ((ushort)Size / 10000));
            Stats.MS = (short)((decimal)MS * ((ushort)Size / 10000));
            Stats.CR = (short)((decimal)CR * ((ushort)Size / 10000));
            Stats.AT = (short)((decimal)AT * ((ushort)Size / 10000));
            Stats.EV = EV;
            Stats.AR = AR;
            Stats.HT = HT;
            Stats.BL = 0;
            return null;
        }

        public DigimonStats Default(Character Tamer, int Sync, int Size)
        {
            DigimonStats Stats = new DigimonStats();

            Stats.MaxHP = (short)(Math.Min(Math.Floor((decimal)HP * ((ushort)Size / 10000)) + Math.Floor((decimal)Tamer.HP * (Sync / 100)), short.MaxValue));
            Stats.HP = (short)(Math.Min(Math.Floor((decimal)HP * ((ushort)Size / 10000)) + Math.Floor((decimal)Tamer.HP * (Sync / 100)), short.MaxValue));
            Stats.MaxDS = (short)(Math.Min(Math.Floor((decimal)DS * ((ushort)Size / 10000)) + Math.Floor((decimal)Tamer.DS * (Sync / 100)), short.MaxValue));
            Stats.DS = (short)(Math.Max(Math.Floor((decimal)DS * ((ushort)Size / 10000)) + Math.Floor((decimal)Tamer.DS * (Sync / 100)), short.MaxValue));

            Stats.DE = (short)(Math.Min(Math.Floor((decimal)DE * ((ushort)Size / 10000)) + Math.Floor((decimal)Tamer.DE * (Sync / 100)), short.MaxValue));
            Stats.MS = (short)(Math.Min(Math.Floor((decimal)MS * ((ushort)Size / 10000)) + Math.Floor((decimal)Tamer.MS * (Sync / 100)), short.MaxValue));
            Stats.CR = (short)(Math.Min(Math.Floor((decimal)CR * ((ushort)Size / 10000)), short.MaxValue));
            Stats.AT = (short)(Math.Min(Math.Floor((decimal)AT * ((ushort)Size / 10000)) + Math.Floor((decimal)Tamer.AT * (Sync / 100)), short.MaxValue));
            Stats.EV = EV;
            Stats.AR = AR;
            Stats.HT = HT;

            Stats.Intimacy = (short)Sync;
            return Stats;
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", DisplayName, Species);
        }
    }
}
