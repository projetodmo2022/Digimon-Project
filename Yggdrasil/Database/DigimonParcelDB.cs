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
    public class DigimonParcelDB
    {
        public static Dictionary<int, Parcel> Parcel = new Dictionary<int, Parcel>();

        public static void Load(string fileName)
        {
            
            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                      
                        Parcel parcel = new Parcel();
                        parcel.m_listDigimon = read.ReadInt();
                       
                        if (!Parcel.ContainsKey(parcel.m_listDigimon))
                        {
                            Parcel.Add(parcel.m_listDigimon, parcel);
                        }

                    }
                }
            }
            SysCons.LogDB("DigimonParcel.bin", "Loaded {0} DigimonParcel.", Parcel.Count);
        }
    }

    public class Parcel
    {
        public int m_listDigimon;



    }


}

