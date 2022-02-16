using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Yggdrasil;
using Digital_World;

namespace Yggdrasil.Database
{
    public class TamerListDB
    {
        public static Dictionary<int, TamerListsINFO> TamerListsINFOList = new Dictionary<int, TamerListsINFO>();
        public static Dictionary<int, CsEmotion> CsEmotionList = new Dictionary<int, CsEmotion>();
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
                        read.Seek(4 + i * 476);
                        TamerListsINFO tamerListsINFO = new TamerListsINFO();
                        tamerListsINFO.s_dwTamerID = read.ReadInt();
                        tamerListsINFO.s_cSoundDirName = read.ReadUShort();
                        tamerListsINFO.s_nTamerType = read.ReadByte();
                        if (!TamerListsINFOList.ContainsKey(tamerListsINFO.s_dwTamerID))
                        {
                            TamerListsINFOList.Add(tamerListsINFO.s_dwTamerID, tamerListsINFO);
                        }
                        
                    }
                }
            }

            if (File.Exists(fileName) == false) return;
            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        read.Seek(4 + i * 476);
                        CsEmotion Emotion = new CsEmotion();
                        Emotion.s_nID = read.ReadInt();
                        Emotion.s_nAniID = read.ReadUInt();
                        Emotion.s_nUseCmdCount = read.ReadByte();
                        if (!CsEmotionList.ContainsKey(Emotion.s_nID))
                        {
                            CsEmotionList.Add(Emotion.s_nID, Emotion);
                        }
                        
                    }
                }
            }
            SysCons.LogDB("TamerList.bin", "Loaded {0} TamerList.", TamerListsINFOList.Count);
            SysCons.LogDB("TamerList.bin", "Loaded {0} TamerList.", CsEmotionList.Count);
        }
    }

    public class TamerListsINFO
    {
		public int s_dwTamerID;
		//TCHAR s_szName[MAX_FILENAME];

		public int s_cSoundDirName/*[MAX_FILENAME]*/;

		// ÄÉ¸¯ÅÍ Å¸ÀÔ
		public byte s_nTamerType;

		// ½ºÅ³
		public sSKILL s_Skill;
        public int s_dwID = 5;

		public string s_szComment;
		public string s_szPart;
		public string s_szGender;

	}
	public class sSKILL
    {
        
    }

	public class CsEmotion
    {
		public int s_nID;

		//TCHAR s_szName[EMOTION_STR_LEN];
		//TCHAR s_szCmd[EMOTION_CMD_COUNT][EMOTION_STR_LEN];

		public uint s_nAniID;
		public byte s_nUseCmdCount;
	}
}
