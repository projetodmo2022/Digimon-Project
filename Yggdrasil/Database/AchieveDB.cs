using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
	public class AchieveDB
	{
		public static Dictionary<int, AchieveSINFO> achieves = new Dictionary<int, AchieveSINFO>();

		public static void Load(string fileName)
		{
            if (File.Exists(fileName) == false) return;
			using (Stream s = File.OpenRead(fileName))
			{
                
				using (BitReader read = new BitReader(s))
				{
					int dcount = 18;   //Parte sem funçao alguma ainda nao descobri o sentido dela
					for (int i = 0; i < dcount; i++)
					{
						sINFO achieve = new sINFO();
						achieve.s_szName = read.ReadZString(Encoding.Unicode, 32 * 2);   // wchar_t colocar valor multiplicar por 2
						achieve.s_listChild = read.ReadInt();
					}
					int count = read.ReadInt();
					for (int i = 0; i < count; i++)
					{

						AchieveSINFO achieve = new AchieveSINFO();
						achieve.s_nQuestID = read.ReadInt();
						achieve.s_nIcon = read.ReadInt();
						achieve.s_nPoint = read.ReadUShort();
						achieve.s_bDisplay = read.ReadByte();
						achieve.s_bDisplay2 = read.ReadByte();
						achieve.s_szName = read.ReadZString(Encoding.Unicode, 64 * 2);   // wchar_t colocar valor multiplicar por 2
						achieve.s_szComment = read.ReadZString(Encoding.Unicode, 256 * 2);   // wchar_t colocar valor multiplicar por 2
						achieve.s_szTitle = read.ReadZString(Encoding.Unicode, 64 * 2);   // wchar_t colocar valor multiplicar por 2
						achieve.s_nGroup = read.ReadInt();
						achieve.s_nSubGroup = read.ReadInt();
						achieve.s_nType = read.ReadInt();
						achieve.s_nBuffCode = read.ReadInt();
						if (!achieves.ContainsKey(achieve.s_nQuestID))
						{
							achieves.Add(achieve.s_nQuestID, achieve);
						}
					}
				}
			}
			SysCons.LogDB("Achieve.bin", "Loaded {0} Achieve.", achieves.Count);
		}
	}



	public class AchieveSINFO
	{
		public int s_nQuestID;
		public int s_nIcon;
		public ushort s_nPoint;
		public byte s_bDisplay;
		public byte s_bDisplay2;
		public string s_szName;
		public string s_szComment;
		public string s_szTitle;
		public int s_nGroup;
		public int s_nSubGroup;
		public int s_nType;
		public int s_nBuffCode;
	}
	public class sINFO
    {
		public string s_szName;
		public int s_listChild;

	}
}
