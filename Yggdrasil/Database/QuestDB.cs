using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yggdrasil.Helpers;
using System.IO;
using Digital_World;


namespace Yggdrasil.Database
{
    public class QuestDB
    {
        public static Dictionary<int, QuestInfo> quests = new();
        public static Dictionary<int, QuestInfo> activequests = new();
        public static Dictionary<int, QuestInfo> notactivequests = new();
        public static Dictionary<int, QuestInfo> DailyQuests = new();

        public static void Load(string fileName)
        {
            if (File.Exists(fileName) == false) return;
            using (Stream s = File.OpenRead(fileName))
            {
                using (BitReader read = new BitReader(s))
                {
                    int questcount = read.ReadInt();
                    for (int i = 0; i < questcount; i++)
                    {
                        QuestInfo quest = new QuestInfo();

                        quest.UniqID = read.ReadInt();
                        quest.Model = read.ReadInt();
                        quest.Model2 = read.ReadInt();
                        quest.Level = read.ReadShort();
                        quest.Pos = read.ReadInt();
                        quest.Pos2 = read.ReadInt();

                        quest.ManagedID = read.ReadInt();
                        quest.Active = read.ReadShort();

                        quest.Unknown = read.ReadByte();
                        //quest.Immediate = read.ReadInt();
                        //quest.ResetQuest = read.ReadShort();

                        quest.Type = (eQUEST_TYPE)read.ReadInt();
                        quest.StartTargetType = read.ReadInt();
                        quest.StartTargetID = read.ReadInt();
                        quest.Target = read.ReadInt();
                        quest.TargetValue = read.ReadInt();
                        quest.TitleTab = read.ReadZString(Encoding.Unicode, 160);
                        quest.TitleText = read.ReadZString(Encoding.Unicode, 160);
                        quest.Body = read.ReadZString(Encoding.Unicode, 4096);
                        quest.Simple = read.ReadZString(Encoding.Unicode, 256);
                        quest.Helper = read.ReadZString(Encoding.Unicode, 1024);
                        quest.Process = read.ReadZString(Encoding.Unicode, 640);
                        quest.Complete = read.ReadZString(Encoding.Unicode, 1400);
                        quest.Expert = read.ReadZString(Encoding.Unicode, 640);

                        quest.Itemgiven = read.ReadInt(); //items, given for quest
                        for (int a = 0; a < quest.Itemgiven; a++)
                        {
                            quest.Itemgiven2 = read.ReadInt();
                            quest.ItemgivenType = read.ReadInt();
                            quest.ItemgivenAmount = read.ReadInt();
                        }

                        quest.condition = read.ReadInt(); //conditions for starting the quest
                        for (int g = 0; g < quest.condition; g++)
                        {
                            quest.ConditionType = read.ReadInt();
                            quest.ConditionId = read.ReadInt();
                            quest.ConditionCount = read.ReadInt();

                        }
                        quest.Goals = read.ReadInt(); //quest goals
                        for (int f = 0; f < quest.Goals; f++)
                        {
                            quest.GoalType = read.ReadInt();
                            quest.GoalId = read.ReadInt();
                            quest.goalAmount = read.ReadInt(); //?
                            quest.goalCurTypeCount = read.ReadInt(); //?
                            quest.SubValue = read.ReadInt(); //?
                            quest.SubValue2 = read.ReadInt(); //?
                        }
                        quest.RewardNumber = read.ReadInt(); //reward
                        for (int f = 0; f < quest.RewardNumber; f++)
                        {
                            quest.Reward = read.ReadInt();
                            quest.RewardType = read.ReadInt();

                            if (quest.RewardType <= 0) //money
                            {
                                quest.RewardMoney = read.ReadInt();
                                quest.RewardUnk = read.ReadInt();
                            }
                            if (quest.RewardType > 0) //item
                            {
                                quest.RewardItem = read.ReadInt();
                                quest.RewardAmount = read.ReadInt();
                            }
                        }
                        quest.unk = read.ReadInt(); //?
                        quest.unk2 = read.ReadInt(); //?
                        quest.unk3 = read.ReadInt(); //?
                        quest.unk4 = read.ReadInt(); //?
                        quest.unk5 = read.ReadInt(); //?

                        quests.Add(quest.UniqID, quest);
                        if (quest.Active <= 0)
                        {
                            activequests.Add(quest.UniqID, quest);
                        }
                        else
                        {
                            notactivequests.Add(quest.UniqID, quest);
                        }
                        if (quest.Type == eQUEST_TYPE.QT_DAILY)
                        {
                            DailyQuests.Add(quest.UniqID, quest);
                        }

                        /*using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(@"C:\Digimon-Project-main\487\Build\Game\Debug\net6.0-windows\Data\quests.txt", true))
                        {
                            file.WriteLine("{0} | {1} | Level: {2} | {3} | Conditions: {4} | Goals: {5} | Reward: {6} |", quest.UniqID, quest.TitleText, quest.Level, quest.Itemgiven, quest.condition, quest.Goals, quest.RewardNumber);
                        }*/
                    }

                    SysCons.LogDB("Quest.bin", "Loaded {0} Quest.", questcount);

                }
            }
        }

        public static QuestInfo GetQuest(int fullId)
        {
            QuestInfo qData = null;
            foreach (KeyValuePair<int, QuestInfo> kvp in quests)
            {
                if (kvp.Value.UniqID == fullId)
                {
                    qData = kvp.Value;
                    break;
                }
            }
            return qData;
        }

        public enum eQUEST_TYPE
        {
            QT_SUB = 0,
            QT_MAIN,
            QT_DAILY,
            QT_REPEAT,
            QT_EVENTREPEAT,
            QT_ACHIEVE,
            QT_JOINTPROGRESS,
            QT_TUTORIAL,
            QT_XANTI_JOINTPROGRESS,
            QT_TIME,
        };

        public class QuestInfo
        {
            public int UniqID;
            public int Model;
            public int Model2;
            public short Level;
            public int Pos;
            public int Pos2;
            public int ManagedID;
            public short Active;
            public byte Unknown;
            public int Immediate;
            public short ResetQuest;
            public eQUEST_TYPE Type;
            public int StartTargetType;
            public int StartTargetID;
            public int Target;
            public int TargetValue;
            public string TitleTab;
            public string TitleText;
            public string Body;
            public string Simple;
            public string Helper;
            public string Process;
            public string Complete;
            public string Expert;
            public int Itemgiven;
            public int Itemgiven2;
            public int ItemgivenType;
            public int ItemgivenAmount;
            public int condition;
            public int ConditionType;
            public int ConditionId;
            public int ConditionCount;
            public int Goals;
            public int Goal;
            public int GoalType;
            public int GoalId;
            public int GoalCount;
            public int goalAmount;
            public int goal2;
            public int goal3;
            public int goal4;
            public int goalCurTypeCount;
            public int SubValue;
            public int SubValue2;
            public int GoalQuestID;
            public int RewardNumber;
            public int Reward;
            public int RewardType;
            public int RewardMoney;
            public int RewardUnk;
            public int RewardItem;
            public int RewardAmount;
            public int unk;
            public int unk2;
            public int unk3;
            public int unk4;
            public int unk5;
        }
        public enum eTYPE { ITEM = 0, /*TIME*/ };
        public enum eTYPE2
        {
            QUEST = 0, T_LEVEL, ITEM, D_LEVEL, D_TYPE, REGION,
            D_EVO, D_EVO_DISABLE,
            D_BASE_TABLE_IDX, //
            TAMER_TABLE_IDX,        //
            QUEST_ID_CANNOT_PROGRESS,           // 
            QUEST_ID_CANNOT_COMPLETED,      // 
        };

        public enum eTYPE3 // ¿Ï·á Á¶°Ç Å¸ÀÔ
        {
            // Å×ÀÌºí ¼³Á¤°ª
            NONE = -1,
            MONSTER = 0,
            ITEM,
            CLIENT,
            REGION,
            NPCTALK,
            USE_ITEM,
            USE_ITEM_NPC,
            USE_ITEM_MONSTER,
            USE_ITEM_REGION,
            T_LEVEL,
            D_TYPE,
            MAP = 100,
            EMPLOYMENT = 101,
        };
        public class ApplyRequite
        {
            //public int Type;
            public eTYPE Type;
            public uint TypeID;
            public int TypeCount;

        }
        public class PreRequire
        {
            //public int Type;
            public eTYPE2 Type;
            public uint TypeID;
            public int TypeCount;
        }
        public class CompRequire
        {
            //public int Type;
            public int Slot { get; set; }
            public eTYPE3 Type { get; set; }
            public uint TypeID { get; set; }
            public int TypeCount { get; set; }
            public int CurTypeCount { get; set; }
            public uint TargetID { get; set; }
            public int SubValue { get; set; }
            public int SubValue2 { get; set; }
            public int QuestID { get; set; }
        }
        public enum eMETHOD { RM_GIVE = 0, RM_SEL1, };
        public enum eTYPE4 : int { MONEY = 0, EXP, ITEM, EVOSLOT };
        public class Requite
        {
            //public int Method;
            public eMETHOD Method;
            //public int Type;
            public int Type;
            public uint TypeID;
            public int TypeCount;
        }
    }
}