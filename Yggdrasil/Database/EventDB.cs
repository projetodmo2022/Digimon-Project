using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
	public static class Props
    {
		public static int EVENT_MSG_LEN { get; set; } = 512;
		public static int EVENT_ITEM_MAXCNT { get; set; } = 6;
		public static int MONTHLY_ITEM_MAXCNT { get; set; } = 28;

	}
    public class EventDB
    {
		public static Dictionary<int, sRECOMMENDE> sRECOMMENDEList = new Dictionary<int, sRECOMMENDE>();
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
						sRECOMMENDE sInfo = new sRECOMMENDE();
						sInfo.s_nTableNo = read.ReadInt();
						sInfo.s_nUse = read.ReadInt();
						sInfo.s_nIndex = read.ReadInt();
						sInfo.s_nType = read.ReadInt();
						sInfo.s_nSuccessType = read.ReadInt();
						sInfo.s_nSuccessValue = read.ReadInt();
						sInfo.s_nItemKind = read.ReadInt();
						sInfo.s_nItemID = read.ReadInt();
						sInfo.s_nItemCnt = read.ReadUShort();

						if (!sRECOMMENDEList.ContainsKey(sInfo.s_nTableNo))
						{
							sRECOMMENDEList.Add(sInfo.s_nTableNo, sInfo);
						}	
					}
				}
			}
			SysCons.LogDB("Event.bin", "Loaded {0} buffs.", sRECOMMENDEList.Count);
		}
	}

    public class EventsEnums
    {
		enum eEVENT_TYPE
		{
			ET_DAILY = 10000,
			ET_ATTENDANCE = 20000,
			ET_NOT_LOGIN = 30000,
			ET_PCBANG = 40000,
			ET_LEVELUP = 50000,
			ET_MONTHLY = 60000, //2017-03-07-nova ¿ù°£Á¢¼Óº¸»ó 
			ET_DIGIMONPARCEL = 70000,   //µðÁö¸ó ºÐ¾ç ÀÌº¥Æ® Áö±Þ ¾ÆÀÌÅÛ
			ET_HOTTIME = 80000,
		};

		//sATTENDANCE_INFO m_AttendanceInfo;
		//std::map<int, sEVENT*> m_mapEvent;
		//std::map<int, sRECOMMENDE*> m_mapRecommendE;

		//std::map<int, sMonthlyEvent*> m_mapMonthlyEvent;    ////2017-03-07-nova ¿ù°£Á¢¼Óº¸»ó
		//std::map<int, sHotTimeEvent*> m_mapHotTimeEvent;
	}

	public class sMonthlyEvent
    {
		//public sMonthlyEvent() { s_nTableNo = 0; s_szMessage[0] = null; memset(s_nItemID, 0, sizeof(int) * Props.MONTHLY_ITEM_MAXCNT); memset(s_nItemCnt, 0, sizeof(ushort) * Props.MONTHLY_ITEM_MAXCNT); }
		
		public int s_nTableNo;
		//public TCHAR s_szMessage = Props.EVENT_MSG_LEN;

		public int s_nItemID = Props.MONTHLY_ITEM_MAXCNT;
		public ushort s_nItemCnt = Convert.ToUInt16(Props.MONTHLY_ITEM_MAXCNT);
	}
	public class sEVENT
    {
		public sEVENT() { }

		
		public int s_nTableNo;

		public int s_nMinute;  //2017-03-07-nova ÀÏ°£ ÀÌº¥Æ® ½Ã°£

		public int s_nItemID = Props.EVENT_ITEM_MAXCNT;
		public ushort s_nItemCnt = Convert.ToUInt16(Props.EVENT_ITEM_MAXCNT);
		public int s_szMessage = Props.EVENT_MSG_LEN;

	}

	public class sRECOMMENDE
    {
		public sRECOMMENDE()
        {
			s_nTableNo = 0; 
			s_nIndex = 0; 
			s_nType = 0; 
			s_nSuccessType = 0; 
			s_nSuccessValue = 0; 
			//memset(s_nItemID, 0, sizeof(int) * Props.EVENT_ITEM_MAXCNT);
			//memset(s_nItemCnt, 0, sizeof(ushort) * Props.EVENT_ITEM_MAXCNT);
		}

		public int s_nTableNo;
		public int s_nUse;

		public int s_nIndex;
		public int s_nType;
		public int s_nSuccessType;
		public int s_nSuccessValue;
		// 		int		s_nSYear;
		// 		int		s_nSMonth;
		// 		int		s_nSDay;
		// 		int		s_nSHour;
		// 		int		s_nSMin;
		// 		int		s_nSSec;
		// 		int		s_nEYear;
		// 		int		s_nEMonth;
		// 		int		s_nEDay;
		// 		int		s_nEHour;
		// 		int		s_nEMin;
		// 		int		s_nESec;

		public int s_nItemKind;
		public int s_nItemID = Props.EVENT_ITEM_MAXCNT;
		public ushort s_nItemCnt = Convert.ToUInt16(Props.EVENT_ITEM_MAXCNT);

		// Ä£±¸ÃßÃµ ±â°£ Ã¼Å© ¼öÁ¤ chu8820
		//TCHAR s_cStartTime[32];
		//TCHAR s_cEndTime[32];
	}

	public class sATTENDANCE_INFO
    {
		public DateTime s_StartTime;
		public DateTime s_EndTime;
	}

	public class sHotTimeEvent
    {
		//public sHotTimeEvent() { s_nEventNo(0), s_nDay(0), s_nItemNo(0), s_nItemCnt(0) }

		int s_nEventNo;
		//wstring s_szStartDate;
		//wstring s_szEndDate;
		string s_szStartDate;
		string s_szEndDate;
		int s_nDay;
		//wstring s_szStartTime;
		//wstring s_szEndTime;
		string s_szStartTime;
		string s_szEndTime;
		int s_nItemNo;
		int s_nItemCnt;
	}


}
