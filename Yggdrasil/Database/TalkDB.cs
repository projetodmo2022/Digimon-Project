using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
    public class PropsTalk
    {
        public int FT_TALK_TUTORIAL_LEN { get; set; } = 256;
        public int SIMPLE_TOOLTIP { get; set; }
        public int FT_TOOLTIP_TITLE_BODY_LEN { get; set; } = 512;
        public int FT_TOOLTIP_MSG_BODY_LEN { get; set; } = 1024;
        public int FT_TALK_TIP_LEN { get; set; } = 200;
        public int FT_TALK_MSG_TITLE_LEN { get; set; } = 16;
        public int FT_TALK_MSG_BODY_LEN { get; set; } = 256;
        public int FT_EVENTTALK_LEN { get; set; } = 200;
        public int FT_TALK_DIGIMON_LEN { get; set; } = 100;
    }

    public class TalkDB
    {
        public static Dictionary<string, CsTalk_Tutorial> tutorialList = new Dictionary<string, CsTalk_Tutorial>();
        public static Dictionary<int, CsTalk_Tooltip> tooltipList = new Dictionary<int, CsTalk_Tooltip>();
        public static Dictionary<string, CsTalk_Tip> tipRewardList = new Dictionary<string, CsTalk_Tip>();
        public static Dictionary<int, CsTalk_Digimon> digimonRewardList = new Dictionary<int, CsTalk_Digimon>();
        public static Dictionary<int, TalkMessage> messageList = new Dictionary<int, TalkMessage>();
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
                        CsTalk_Tutorial tuto = new CsTalk_Tutorial();
                        tuto.strings_szText = read.ReadZString(Encoding.ASCII);


                        if (!tutorialList.ContainsKey(tuto.strings_szText))
                        {
                            tutorialList.Add(tuto.strings_szText, tuto);
                        }
                    }
                }
            }
            SysCons.LogDB("Talk.bin", "Loaded {0} Tutorial.", tutorialList.Count);
            if (tooltipList.Count > 0) return;
            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        //read.Seek(4 + i * 476);
                        CsTalk_Tooltip tooltiip = new CsTalk_Tooltip();
                        tooltiip.s_dwID = read.ReadInt();
                        tooltiip.s_Title = read.ReadZString(Encoding.ASCII);
                        tooltiip.s_Message = read.ReadZString(Encoding.ASCII);

                        if (!tooltipList.ContainsKey(tooltiip.s_dwID))
                        {
                            tooltipList.Add(tooltiip.s_dwID, tooltiip);
                        }
                    }
                }
            }
            SysCons.LogDB("Talk.bin", "Loaded {0} ToolTips.", tooltipList.Count);
            if (tipRewardList.Count > 0) return;
            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        //read.Seek(4 + i * 476);
                        CsTalk_Tip taltip = new CsTalk_Tip();
                        taltip.s_szTip = read.ReadZString(Encoding.ASCII);
                        taltip.s_szLoadingTip = read.ReadZString(Encoding.ASCII);
                        taltip.s_nLevel = read.ReadUShort();

                        if (!tipRewardList.ContainsKey(taltip.s_szTip))
                        {
                            tipRewardList.Add(taltip.s_szTip, taltip);
                        }
                    }
                }
            }
            SysCons.LogDB("Talk.bin", "Loaded {0} Tips.", tipRewardList.Count);
            if (digimonRewardList.Count > 0) return;
            using (Stream s = File.OpenRead(fileName))
            {
                if (File.Exists(fileName) == false) return;
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        //read.Seek(4 + i * 476);
                        CsTalk_Digimon talkDigi = new CsTalk_Digimon();
                        talkDigi.s_dwParam = read.ReadInt();
                        talkDigi.s_nType = read.ReadUShort();
                        talkDigi.s_szText = read.ReadZString(Encoding.ASCII);
                        talkDigi.s_szList = read.ReadZString(Encoding.ASCII);

                        if (!digimonRewardList.ContainsKey(talkDigi.s_dwParam))
                        {
                            digimonRewardList.Add(talkDigi.s_dwParam, talkDigi);
                        }
                    }
                }
            }
            SysCons.LogDB("Talk.bin", "Loaded {0} Digimons.", digimonRewardList.Count);
            if (messageList.Count > 0) return;
            using (Stream s = File.OpenRead(fileName))
            {
                if (File.Exists(fileName) == false) return;
                using (BitReader read = new BitReader(s))
                {
                    int count = read.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        //read.Seek(4 + i * 476);
                        TalkMessage talkDigi = new TalkMessage();
                        talkDigi.s_dwID = read.ReadInt();
                        talkDigi.s_TitleName = read.ReadZString(Encoding.ASCII);
                        talkDigi.s_Message = read.ReadZString(Encoding.ASCII);
                        talkDigi.s_dwLinkID = read.ReadInt();

                        if (!messageList.ContainsKey(talkDigi.s_dwID))
                        {
                            messageList.Add(talkDigi.s_dwID, talkDigi);
                        }
                    }
                }
            }
            SysCons.LogDB("Talk.bin", "Loaded {0} Messages.", messageList.Count);
        }

    }

    public class CsTalk_Tutorial
    {
        public string strings_szText;
    }

    public class CsTalk_Tooltip
    {
        public int s_dwID;
        public string s_Title;
        public string s_Message;
    }

    public class CsTalk_Tip
    {
        public string s_szTip;
        public string s_szLoadingTip;
        public int s_nLevel;
    }

    public class CsTalk_Digimon
    {

        public int TP_QUEST = 0x00000001;
        public int TP_NPC = 0x00000002;
        public int TP_DIGIMON = 0x00000004;
        public int TP_SKILL = 0x00000008;

        public int s_dwParam;
        public ushort s_nType;
        public string s_szText;
        public string s_szList;
    }

    public class TalkMessage
    {

        public int MT_BOX = 1;
        public int MT_SYSTEM = 2;
        public int MT_CHAT = 3;
        public int MT_NOTICE = 4;
        public int MT_MONSTER_SKILL = 5;

        public int MT_OK = 1;
        public int MT_OK_CANCEL = 2;
        public int MT_CANCEL = 3;
        public int MT_NOBUTTON = 4;


        public int s_dwID;
        public string s_TitleName;
        public string s_Message;
        public int s_dwLinkID;
    }

}
