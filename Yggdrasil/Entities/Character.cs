using System;
using Yggdrasil.Entities;
using Yggdrasil.Helpers;
using Yggdrasil.Network;

namespace Yggdrasil.Entities
{
    public enum CharacterModel : int
    {
        NULL = -1,
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
        Ryo = 80017
    }

    /// <summary>
    /// Character Class
    /// </summary>
    public class Character
    {
        public uint AccountId = 0;
        public uint CharacterId = 0;
        public int CharacterPos = 0;
        public int Starter = 0;
        public uint intHandle = 0;

        public IClient? Client { get; set; } = null;

        public ItemList Equipment = new ItemList(15);
        public int Level = 1;
        public CharacterModel Model = CharacterModel.NULL;
        public string Name = string.Empty;
        public string GuildName = string.Empty;
        public int lastChar = 0;
        public int Money = 0;
        public int crowns = 0;
        public int CurrentTitle = 0;
        public int CurrentSealLeader = 0;
        public int MaxHP = 0;
        public int MaxDS = 0;
        public int HP = 0;
        public int DS = 0;
        public int AT = 0;
        public int DE = 0;
        public int EXP = 0;
        public int MS = 0;
        public int Fatigue = 0;
        public int InventorySize = 21;
        public int StorageSize = 21;
        public int ArchiveSize = 1;
        public int mercenaryLimit = 0;
        public ItemList Inventory = new ItemList(150);  //150 quantidade de slot
        public ItemList Storage = new ItemList(315);    //315 quantidade de slot
        public ItemList AccountStorage = new ItemList(14);  //14 quantidade de slot
        public Position Location = new Position();
        public ItemList TempCashVault = new ItemList(7);
        public ItemList ChipSets = new ItemList(13);
        public ItemList JogChipSet = new ItemList(1);
        public FriendList friends = new FriendList(100);
        public SealList Seals = new SealList(120);
        public int XAI = 0;
        public int XGauge = 0;



        /// <summary>
        /// The current egg in the incubator
        /// </summary>
        public int Incubator = 0;
        /// <summary>
        /// The level of the egg in the incubator
        /// </summary>
        public int IncubatorLevel = 0;

        public int IncubatorBackup = 0;

        /// <summary>
        /// A list of digiIds in the Archive
        /// </summary>
        public uint[] ArchivedDigimon = new uint[200]; // 200 Slot 
        public Digimon[] DigimonList = new Digimon[5];
        public QuestList Quests;

        public bool Riding = false;
        public int RidingInt = 0;

        /// <summary>
        /// The current active Digimon
        /// </summary>
        public Digimon Partner
        {
            get
            {
                return DigimonList[0];
            }
            set
            {
                DigimonList[0] = value;
            }
        }

        public Character()
        {
            Equipment = new ItemList(15);
            Inventory = new ItemList(150);
            Storage = new ItemList(315);
            AccountStorage = new ItemList(14);
            Quests = new QuestList();
            friends = new FriendList(20);
            ArchivedDigimon = new uint[200];
            DigimonList = new Digimon[5];
            TempCashVault = new ItemList(7);
        }

        public Character(uint AcctId, string charName, int charModel)
        {
            AccountId = AcctId;
            Name = charName;
            Model = (CharacterModel)charModel;
            Equipment = new ItemList(15);
            Inventory = new ItemList(150);
            Storage = new ItemList(315);
            AccountStorage = new ItemList(14);
            Quests = new QuestList();
            friends = new FriendList(20);
            ArchivedDigimon = new uint[200];
            DigimonList = new Digimon[5];
            TempCashVault = new ItemList(7);
        }
        static Random Rand = new Random();

        public override string ToString()
        {
            return string.Format("Tamer: Lv {1} {0}", Name, Level);
        }

        public override bool Equals(object obj)
        {
            if (typeof(Character) != obj.GetType())
            {
                return base.Equals(obj);
            }
            else
            {
                return (obj as Character).AccountId == this.AccountId;
            }
        }

        public uint ProperModel
        {
            get
            {
                uint iModel = 0x9C40A0;
                iModel += (((uint)Model - 80001) * 128);
                return (iModel << 8);
            }
        }

        /// <summary>
        /// UID to the Tamer's entity
        /// </summary>
        /// 


        public uint UID { get; set; } = 0;

        public uint IDX => UID - (2 << 14);


        /// <summary>
        /// UID to the Digimon's entity
        /// </summary>
        public uint DigimonUID { get; set; } = 0;

        public MonsterEntity TargetMonster { get; set; }
    }
}
