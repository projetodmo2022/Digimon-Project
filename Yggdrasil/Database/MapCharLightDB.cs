using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
    public class MapCharLightDB
    {
        public static Dictionary<int, sINFO_BASE> sinfoBaseList = new Dictionary<int, sINFO_BASE>();

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
                        //read.Seek(4 + i * 476);
                        sINFO_BASE sinfoBase = new sINFO_BASE();
                        sinfoBase.s_dwMapID = read.ReadUShort();
                        //sinfoBase.s_bChar = read.ReadUShort();
                        //sinfoBase.s_bShadow = read.ReadUShort();
                        sinfoBase.s_nType = read.ReadInt();
                        //sinfoBase.s_bEnableLight = read.ReadUShort();
                        sinfoBase.s_fDiffuse = read.ReadFloat();
                        sinfoBase.s_fAmbient = read.ReadFloat();
                        sinfoBase.s_fSpecular = read.ReadFloat();
                        sinfoBase.s_fPower = read.ReadFloat();

                        if (!sinfoBaseList.ContainsKey(sinfoBase.s_dwMapID))
                        {
                            sinfoBaseList.Add(sinfoBase.s_dwMapID, sinfoBase);
                        }
                    }
                }
            }
            SysCons.LogDB("MapCharLight.bin", "Loaded {0} MapCharLights.", sinfoBaseList.Count);
        }
    }

    public class sINFO_BASE
    {
        public ushort s_dwMapID;

        public bool s_bChar;
        public bool s_bShadow;

        public int s_nType;
        public bool s_bEnableLight;
        public float s_fDiffuse;
        public float s_fAmbient;
        public float s_fSpecular;
        public float s_fPower;
    }
    public class sINFO_DIR : sINFO_BASE
    {
        public float s_fRot;
		public float s_fPos;
        public float s_fScale;
    }

    public class sINFO_POINT : sINFO_BASE
    {
        public float s_Pos;
        public float s_C;
        public float s_L;
        public float s_Q;
        public float s_Range;
    }
}
