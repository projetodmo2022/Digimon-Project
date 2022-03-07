using System;
using System.Collections.Generic;
using System.IO;
using Yggdrasil.Helpers;
using Digital_World;
using System.Linq;

namespace Yggdrasil.Database
{
    /// <summary>
    /// Parse DigimonEvolve.bin
    /// </summary>
    public static class DigimonEvoDB
    {
        public static Dictionary<int, Evolution> EvolutionList = new Dictionary<int, Evolution>();

        public static void Load(string fileName)
        {
            int get_position = 0;

            if (EvolutionList.Any())
                return;

            if (File.Exists(fileName) == false) 
                return;

            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        Evolution evo = new Evolution();
                        evo.digiId = read.ReadInt(); // ->GET ID
                        //Console.WriteLine(evo.digiId);
                        get_position += 4;
                        read.ReadInt(); //IGNORE THAT 02
                        get_position += 4;
                        evo.Digivolutions = read.ReadInt(); //GET EVO
                        get_position += 4;
                        //Console.WriteLine(evo.Digivolutions);

                        //BEGIN PARSING DIGI-EVO LINE 
                        for (int j = 0; j < evo.Digivolutions; j++)
                        {

                            EvolutionLine line = new EvolutionLine();
                            line.digiId = read.ReadInt(); //GET THE DIGI-ID
                            get_position += 4;
                            line.iLevel = read.ReadShort(); //GET THE DIGI-LEVEL - ROOKIE, ETC
                            get_position += 4;
                            line.uShort1 = read.ReadShort(); //CONST FOR ALL - > REMOVED IN NEW BIN FILE

                            //Console.WriteLine(line.digiId + " = DIGIID");
                            //Console.WriteLine(line.iLevel + " = DIGI-LEVEL");
                            //Console.WriteLine(line.uShort1 + " = WEIRD CONST");


                            line.Line = new int[][] { new int[10], new int[10] };
                            for (int k = 0; k < 10; k++) // -> Change to 9 for ver 128
                            {

                                line.Line[0][k] = read.ReadInt(); //GET KEY
                                line.Line[1][k] = read.ReadInt(); //Value of the key
                            }

                            line.uInts1 = new int[11];

                            for (int m = 0; m < 11; m++)
                            {

                                line.uInts1[m] = read.ReadInt();
                                //Console.Write(line.uInts1[m]);
                                //Console.WriteLine("");

                            }

                            //Console.WriteLine("");


                            //Console.WriteLine("STATS!");


                            //Console.WriteLine("");

                            line.uStats = new int[8];

                            for (int m = 0; m < 8; m++)
                            {
                                line.uStats[m] = read.ReadInt();
                                //Console.WriteLine(line.uStats[m]);
                                //Console.WriteLine("");
                            }

                            //Console.WriteLine("");

                            //Console.WriteLine("END OF STATS!");

                            //Console.WriteLine("");

                            //////////////////////////////

                            //Console.WriteLine("UNKNOWN!"); //->901702

                            //Console.WriteLine("");

                            line.uInts2 = new int[5];
                            for (int k = 0; k < 5; k++)
                            {
                                line.uInts2[k] = read.ReadInt();
                                //Console.WriteLine(line.uInts2[k]);
                                //Console.WriteLine("");

                            }


                            /////////////////////////////////

                            //Console.WriteLine("UNKNOWN!");

                            //Console.WriteLine("");

                            line.uShorts1 = new short[4];
                            for (int k = 0; k < 4; k++)
                            {
                                line.uShorts1[k] = read.ReadShort();
                                //Console.WriteLine(line.uShorts1[k]);
                                //Console.WriteLine("");

                            }

                            ////////////////////////// -> DONE

                            line.uInts3 = new int[34];
                            for (int k = 0; k < 34; k++)
                            {
                                line.uInts3[k] = read.ReadInt();
                                //Console.WriteLine(line.uInts3[k]);
                            }

                            evo.Evolutions.Add(line);

                            /*using (System.IO.StreamWriter file =
                            new System.IO.StreamWriter(@"logs\DigiEvoTEst.txt", true))
                            {
                                file.WriteLine("{0} | {1}", evo.digiId, evo.Digivolutions);
                            }*/
                        }
                        EvolutionList.Add(evo.digiId, evo);

                    }
                }

            }
            SysCons.LogDB("DigimonEvo.bin", "Loaded {0} Digimon evolutions", EvolutionList.Count);
        }

        public static EvolutionLine GetLine(int digiType, int evolvedType)
        {
            Evolution evo = DigimonEvoDB.EvolutionList[digiType];
            EvolutionLine line = evo.Evolutions.Find(
                delegate (EvolutionLine evoline)
                {
                    return evoline.digiId == evolvedType;
                });
            return line;
        }

    }

    public class Evolution
    {
        public int digiId = 0;
        public int Digivolutions = 0;
        public List<EvolutionLine> Evolutions = new List<EvolutionLine>();

        public Evolution() { }
    }

    public class EvolutionLine
    {
        public enum EvoLevel
        {
            Rookie = 1,
            Rookie_X = 7,
            Champion = 2,
            Champion_X = 8,
            Ultimate = 3,
            Ultimate_X = 9,
            Mega = 4,
            Mega_X = 10,
            Burst = 5,
            Burst_X = 11,
            Jogress = 6,
            Jogress_X = 12,
            Variant = 13,
            Z_Hybrid = 20
        };

        public int digiId = 0;
        public int iLevel = 0;
        public EvoLevel Level
        {
            get
            {
                return (EvoLevel)iLevel;
            }
        }
        public short uShort1 = 0;
        public int[][] Line;

        public int[] uInts1 = new int[11];
        public int[] uStats = new int[8];
        public int[] uInts2 = new int[5];
        public short[] uShorts1 = new short[4];
        public int[] uInts3 = new int[28];
    }
}
