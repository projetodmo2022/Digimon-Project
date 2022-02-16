using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GameServer.Database.Interfaces;
using GameServer.Network;
using MySql.Data.MySqlClient;
using Yggdrasil.Database;
using Yggdrasil.Database.Interfaces;
using Yggdrasil.Entities;
using Yggdrasil.Helpers;

namespace GameServer.Database
{
    class GameDB : IGameDatabase
    {
        public readonly IDatabaseContext _context;

        public GameDB(IDatabaseContext context)
        {
            _context = context;
        }
        public void UserDataExecute(string query, params object[] args)
        {
            _context.Connection = _context.GetNewConnection();
            _context.Execute(query, args);
        }

        public List<Dictionary<string, object>> UserDataQuery(string query, params object[] args)
        {
            _context.Connection = _context.Connect();
            var res = _context.Query(query, args);
            return res;
        }
        public MySqlConnection Connects()
        {
            return _context.GetNewConnection();
        }
        
        public void ResetModel(uint DigiId, int digiType)
        {
            try
            {
                using (MySqlConnection mysql = Connects())
                using (MySqlCommand cmd = new MySqlCommand("UPDATE `digimon` SET `digiModel` = @type WHERE `digimonId` = @id", mysql))
                {
                    cmd.Parameters.AddWithValue("@id", DigiId);
                    cmd.Parameters.AddWithValue("@type", digiType);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: ResetModel({1}, {2})\n{0}", e, DigiId, digiType);
            }
        }

        public Digimon GetDigimon(uint DigiId)
        {
            Digimon digimon = null;
            try
            {
                using (MySqlConnection mysql = Connects())
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM `digimon` WHERE `digimonId` = @id"
                    , mysql))
                {
                    cmd.Parameters.AddWithValue("@id", DigiId);

                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            if (read.Read())
                            {
                                digimon = new Digimon();
                                digimon.DigiId = DigiId;
                                digimon.CharacterId = (int)read["characterId"];
                                digimon.Name = (string)read["digiName"];
                                digimon.Level = (int)read["digiLv"];
                                digimon.Species = (int)read["digiType"];
                                digimon.CurrentForm = digimon.Species;
                                digimon.Size = (short)(int)read["digiSize"];
                                digimon.Scale = (int)read["digiScale"];

                                ResetModel(DigiId, digimon.Species);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: GetDigimon({1})\n{0}", e, DigiId);
            }
            return digimon;
        }

        public uint CreateMercenary(uint charId, string digiName, int digiModel, int digiScale, int digiSize, int intimacy)
        {
            uint digiId = 0;
            try
            {
                DigimonData dData = DigimonListDB.GetDigimon(digiModel);
                using (MySqlConnection con = Connects())
                {
                    Query qry = new Query(Query.QueryMode.INSERT, "digimon");
                    qry.Add("digiName", digiName);
                    qry.Add("digiModel", digiModel);
                    qry.Add("digiType", digiModel);
                    qry.Add("characterId", charId);

                    qry.Add("digiScale", digiScale);
                    qry.Add("digiSize", digiSize);

                    qry.Add("maxHP", dData.HP);
                    qry.Add("maxDS", dData.DS);
                    qry.Add("HP", dData.HP);
                    qry.Add("DS", dData.DS);

                    qry.Add("DE", dData.DE);
                    qry.Add("AT", dData.AT);
                    qry.Add("sync", intimacy);
                    qry.Add("HT", dData.HT);
                    qry.Add("EV", dData.EV);
                    qry.Add("CR", dData.CR);
                    qry.Add("MS", dData.MS);
                    qry.Add("AS", dData.AS);

                    using (MySqlCommand cmd = qry.GetCommand(con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                using (MySqlCommand
                cmd = new MySqlCommand(
                    "SELECT * FROM `digimon` WHERE `characterId` = @charId AND `digiName` = @charName",
                    Connects()))
                {
                    cmd.Parameters.AddWithValue("@charId", charId);
                    cmd.Parameters.AddWithValue("@charName", digiName);
                    using (MySqlDataReader read = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                    {
                        if (read.HasRows && read.Read())
                        {
                            digiId = (uint)(int)read["digimonId"];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: CreateDigimon()\n{0}", e);
            }
            return digiId;
        }

        public void LoadUser(GameClient client, uint AccountID, int UniId)
        {
            try
            {
                using (MySqlConnection con = Connects())
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `accountId` = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", AccountID);

                    using (MySqlDataReader read = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (read.HasRows)
                        {
                            if (read.Read() && (int)read["uniId"] == UniId)
                            {
                                client.AccessLevel = (int)read["level"];
                                client.AccountID = AccountID;

                                client.UniqueID = (uint)UniId;
                                client.crowns = (int)read["crowns"];
                                client.Membership = (int)read["membership"];
                                try
                                {
                                    client.CashVault = ItemList.Deserialize((byte[])read["cashvault"]);
                                }
                                catch
                                {
                                    client.CashVault = new ItemList(149);
                                }
                                try
                                {
                                    client.AccountStorage = ItemList.Deserialize((byte[])read["accountStorage"]);
                                }
                                catch
                                {
                                    client.AccountStorage = new ItemList(14);
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: Validate\n{0}", e);
            }
        }
        public Position GetTamerPosition(uint AccountID)
        {
            Position currentmap = null;
            int lastChar = -1, charId = -1;
            try
            {
                using (MySqlConnection con = Connects())
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `accountId` = @acct", con))
                {
                    cmd.Parameters.AddWithValue("@acct", AccountID);
                    using (MySqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                    {
                        if (dr.HasRows && dr.Read())
                        {
                            lastChar = (int)dr["lastChar"];
                            if (lastChar != -1)
                            {
                                charId = (int)dr[string.Format("char{0}", lastChar + 1)];
                            }
                        }
                    }
                }
                if (lastChar != -1)
                {
                    using (MySqlConnection con = Connects())
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `chars` WHERE `characterId` = @char", con))
                    {
                        cmd.Parameters.AddWithValue("@char", charId);
                        using (MySqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                        {

                            if (dr.HasRows && dr.Read())
                            {

                                currentmap = new Position((int)dr["map"], (int)dr["x"], (int)dr["y"]);


                                return currentmap;
                            }
                            return currentmap;
                        }
                        return currentmap;
                    }
                    return currentmap;
                }
            }
            catch
            {

            }
            return currentmap;
        }

        public Character LoadTamer(int AccountID)
        {
            int lastChar = -1, charId = -1;
            Character tamer = null;
            try
            {
                using (MySqlConnection con = Connects())
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `accountId` = @acct", con))
                {
                    cmd.Parameters.AddWithValue("@acct", (uint)AccountID);
                    using (MySqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                    {
                        if (dr.HasRows && dr.Read())
                        {
                            lastChar = (int)dr["lastChar"];
                            if (lastChar != -1)
                            {
                                charId = (int)dr[string.Format("char{0}", lastChar + 1)];
                            }
                        }
                    }
                }

                if (lastChar != -1)
                {
                    using (MySqlConnection con = Connects())
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `chars` WHERE `characterId` = @char", con))
                    {
                        cmd.Parameters.AddWithValue("@char", charId);
                        using (MySqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                        {

                            if (dr.HasRows && dr.Read())
                            {

                                tamer.CharacterId = Convert.ToUInt32((int)dr["characterId"]);
                                tamer.AccountId = Convert.ToUInt32((int)dr["accountId"]);
                                tamer.Model = (CharacterModel)(int)dr["charModel"];
                                tamer.Name = (string)dr["charName"];
                                tamer.Level = (int)dr["charLv"];
                                tamer.InventorySize = (int)dr["inventoryLimit"];
                                tamer.StorageSize = (int)dr["storageLimit"];
                                tamer.ArchiveSize = (int)dr["archiveLimit"];
                                tamer.mercenaryLimit = (int)dr["mercenaryLimit"];
                                tamer.Location = new Position((int)dr["map"], (int)dr["x"], (int)dr["y"]);
                                tamer.MaxHP = (int)dr["maxHP"];
                                tamer.MaxDS = (int)dr["maxDS"];
                                tamer.HP = (int)dr["HP"];
                                tamer.DS = (int)dr["DS"];
                                tamer.AT = (int)dr["AT"];
                                tamer.DE = (int)dr["DE"];
                                tamer.EXP = (int)dr["experience"];
                                tamer.MS = (int)dr["MS"];
                                tamer.Fatigue = (int)dr["Fatigue"];
                                tamer.Starter = (int)dr["starter"];
                                tamer.Money = (int)dr["money"];
                                tamer.CurrentTitle = (int)dr["title"];
                                tamer.XAI = (int)dr["XAI"];
                                tamer.XGauge = (int)dr["XGauge"];
                                tamer.CurrentSealLeader = (int)dr["currentsealleader"];

                                //Incubator
                                tamer.Incubator = (int)dr["incubator"];
                                tamer.IncubatorLevel = (int)dr["incubatorLevel"];
                                tamer.IncubatorBackup = (int)dr["incubatorBackup"];
                                if (tamer.Incubator == 0) tamer.IncubatorLevel = 0;

                                try { tamer.Inventory = ItemList.Deserialize((byte[])dr["inventory"]); }
                                catch { tamer.Inventory = new ItemList(150); }
                                try
                                {
                                    tamer.Equipment = ItemList.Deserialize((byte[])dr["equipment"]);
                                }
                                catch
                                {
                                    tamer.Equipment = new ItemList(15);
                                }
                                try
                                {
                                    tamer.ChipSets = ItemList.Deserialize((byte[])dr["chipset"]);
                                }
                                catch
                                {
                                    tamer.ChipSets = new ItemList(12);
                                }
                                try
                                {
                                    tamer.JogChipSet = ItemList.Deserialize((byte[])dr["jogchipset"]);
                                }
                                catch
                                {
                                    tamer.JogChipSet = new ItemList(12);
                                }
                                try
                                {
                                    tamer.Seals = SealList.Deserialize((byte[])dr["sealmaster"]);
                                }
                                catch
                                {
                                    tamer.Seals = new SealList(120);
                                }
                                try
                                {
                                    tamer.TempCashVault = ItemList.Deserialize((byte[])dr["tempcashvault"]);
                                }
                                catch
                                {
                                    tamer.TempCashVault = new ItemList(7);
                                }
                                try { tamer.Storage = ItemList.Deserialize((byte[])dr["storage"]); }
                                catch { tamer.Storage = new ItemList(315); };
                                try { tamer.Quests = QuestList.Deserialize((byte[])dr["quests"]); }
                                catch { tamer.Quests = new QuestList(); };
                                try
                                {
                                    BinaryFormatter f = new BinaryFormatter();
                                    using (MemoryStream m = new MemoryStream((byte[])dr["archive"]))
                                        tamer.ArchivedDigimon = (uint[])f.Deserialize(m);
                                }
                                catch { tamer.ArchivedDigimon = new uint[150]; }

                                Digimon partner = LoadDigimon((uint)(int)dr["partner"]);
                                tamer.Partner = partner;
                                tamer.Partner.Location = tamer.Location.Clone(); ;

                                if (dr["mercenary1"] != DBNull.Value)
                                {
                                    int mercId = (int)dr["mercenary1"];
                                    Digimon merc = LoadDigimon((uint)mercId);
                                    tamer.DigimonList[1] = merc;
                                }
                                if (dr["mercenary2"] != DBNull.Value)
                                {
                                    int mercId = (int)dr["mercenary2"];
                                    Digimon merc = LoadDigimon((uint)mercId);
                                    tamer.DigimonList[2] = merc;
                                }
                                if (dr["mercenary3"] != DBNull.Value)
                                {
                                    int mercId = (int)dr["mercenary3"];
                                    Digimon merc = LoadDigimon((uint)mercId);
                                    tamer.DigimonList[3] = merc;
                                }
                                if (dr["mercenary4"] != DBNull.Value)
                                {
                                    int mercId = (int)dr["mercenary4"];
                                    Digimon merc = LoadDigimon((uint)mercId);
                                    tamer.DigimonList[4] = merc;
                                }


                                return tamer;
                            }
                            return tamer;
                        }
                        return tamer;
                    }
                    return tamer;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return tamer;

        }

        public void LoadTamer(GameClient client)
        {
            int lastChar = -1, charId = -1;
            try
            {
                using (MySqlConnection con = Connects())
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `accountId` = @acct", con))
                {
                    cmd.Parameters.AddWithValue("@acct", client.AccountID);
                    using (MySqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                    {
                        if (dr.HasRows && dr.Read())
                        {
                            lastChar = (int)dr["lastChar"];
                            if (lastChar != -1)
                            {
                                charId = (int)dr[string.Format("char{0}", lastChar + 1)];
                            }
                        }
                    }
                }

                if (lastChar != -1)
                {
                    using (MySqlConnection con = Connects())
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `chars` WHERE `characterId` = @char", con))
                    {
                        cmd.Parameters.AddWithValue("@char", charId);
                        using (MySqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                        {

                            if (dr.HasRows && dr.Read())
                            {
                                Character tamer = new Character();

                                tamer.CharacterId = Convert.ToUInt32((int)dr["characterId"]);
                                tamer.AccountId = Convert.ToUInt32((int)dr["accountId"]);
                                tamer.Model = (CharacterModel)(int)dr["charModel"];
                                tamer.Name = (string)dr["charName"];
                                tamer.Level = (int)dr["charLv"];
                                tamer.InventorySize = (int)dr["inventoryLimit"];
                                tamer.StorageSize = (int)dr["storageLimit"];
                                tamer.ArchiveSize = (int)dr["archiveLimit"];
                                tamer.mercenaryLimit = (int)dr["mercenaryLimit"];
                                tamer.Location = new Position((int)dr["map"], (int)dr["x"], (int)dr["y"]);
                                tamer.MaxHP = (int)dr["maxHP"];
                                tamer.MaxDS = (int)dr["maxDS"];
                                tamer.HP = (int)dr["HP"];
                                tamer.DS = (int)dr["DS"];
                                tamer.AT = (int)dr["AT"];
                                tamer.DE = (int)dr["DE"];
                                tamer.EXP = (int)dr["experience"];
                                tamer.MS = (int)dr["MS"];
                                tamer.Fatigue = (int)dr["Fatigue"];
                                tamer.Starter = (int)dr["starter"];
                                tamer.Money = (int)dr["money"];
                                tamer.crowns = client.crowns;
                                tamer.CurrentTitle = (int)dr["title"];
                                tamer.XAI = (int)dr["XAI"];
                                tamer.XGauge = (int)dr["XGauge"];
                                tamer.CurrentSealLeader = (int)dr["currentsealleader"];

                                //Incubator
                                tamer.Incubator = (int)dr["incubator"];
                                tamer.IncubatorLevel = (int)dr["incubatorLevel"];
                                tamer.IncubatorBackup = (int)dr["incubatorBackup"];
                                if (tamer.Incubator == 0) tamer.IncubatorLevel = 0;

                                try { tamer.Inventory = ItemList.Deserialize((byte[])dr["inventory"]); }
                                catch { tamer.Inventory = new ItemList(150); }
                                try
                                {
                                    tamer.Equipment = ItemList.Deserialize((byte[])dr["equipment"]);
                                }
                                catch
                                {
                                    tamer.Equipment = new ItemList(15);
                                }
                                try
                                {
                                    tamer.ChipSets = ItemList.Deserialize((byte[])dr["chipset"]);
                                }
                                catch
                                {
                                    tamer.ChipSets = new ItemList(12);
                                }
                                try
                                {
                                    tamer.JogChipSet = ItemList.Deserialize((byte[])dr["jogchipset"]);
                                }
                                catch
                                {
                                    tamer.JogChipSet = new ItemList(12);
                                }
                                try
                                {
                                    tamer.Seals = SealList.Deserialize((byte[])dr["sealmaster"]);
                                }
                                catch
                                {
                                    tamer.Seals = new SealList(120);
                                }
                                try
                                {
                                    tamer.TempCashVault = ItemList.Deserialize((byte[])dr["tempcashvault"]);
                                }
                                catch
                                {
                                    tamer.TempCashVault = new ItemList(7);
                                }
                                try { tamer.Storage = ItemList.Deserialize((byte[])dr["storage"]); }
                                catch { tamer.Storage = new ItemList(315); };
                                try { tamer.Quests = QuestList.Deserialize((byte[])dr["quests"]); }
                                catch { tamer.Quests = new QuestList(); };
                                try
                                {
                                    BinaryFormatter f = new BinaryFormatter();
                                    using (MemoryStream m = new MemoryStream((byte[])dr["archive"]))
                                        tamer.ArchivedDigimon = (uint[])f.Deserialize(m);
                                }
                                catch { tamer.ArchivedDigimon = new uint[150]; }

                                Digimon partner = LoadDigimon((uint)(int)dr["partner"]);
                                tamer.Partner = partner;
                                tamer.Partner.Location = tamer.Location.Clone(); ;

                                if (dr["mercenary1"] != DBNull.Value)
                                {
                                    int mercId = (int)dr["mercenary1"];
                                    Digimon merc = LoadDigimon((uint)mercId);
                                    tamer.DigimonList[1] = merc;
                                }
                                if (dr["mercenary2"] != DBNull.Value)
                                {
                                    int mercId = (int)dr["mercenary2"];
                                    Digimon merc = LoadDigimon((uint)mercId);
                                    tamer.DigimonList[2] = merc;
                                }
                                if (dr["mercenary3"] != DBNull.Value)
                                {
                                    int mercId = (int)dr["mercenary3"];
                                    Digimon merc = LoadDigimon((uint)mercId);
                                    tamer.DigimonList[3] = merc;
                                }
                                if (dr["mercenary4"] != DBNull.Value)
                                {
                                    int mercId = (int)dr["mercenary4"];
                                    Digimon merc = LoadDigimon((uint)mercId);
                                    tamer.DigimonList[4] = merc;
                                }

                                client.Tamer = tamer;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        public Digimon LoadDigimon(uint DigiId)
        {
            Digimon digimon = null;
            try
            {
                using (MySqlConnection con = Connects())
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `digimon` WHERE `digimonId` = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", DigiId);

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                if (dr.Read())
                                {
                                    digimon = new Digimon();

                                    digimon.DigiId = DigiId;
                                    digimon.CharacterId = (int)dr["characterId"];
                                    digimon.Name = (string)dr["digiName"];
                                    digimon.Level = (int)dr["digiLv"];

                                    digimon.Species = (int)dr["digiType"];
                                    digimon.CurrentForm = (int)dr["digiModel"];

                                    digimon.EXP = (int)dr["exp"];
                                    digimon.Size = (short)(int)dr["digiSize"];
                                    digimon.Scale = (int)dr["digiScale"];

                                    digimon.Stats = new DigimonStats();
                                    digimon.Stats.Intimacy = (short)(int)dr["sync"];
                                    digimon.levels_unlocked = (int)dr["unlocked_levels"];

                                    BinaryFormatter bf = new BinaryFormatter();
                                    int forms = DigimonEvoDB.EvolutionList[digimon.Species].Digivolutions;
                                    try
                                    {
                                        using (MemoryStream m = new MemoryStream((byte[])dr["forms"]))
                                            digimon.Forms = (EvolvedForms)bf.Deserialize(m);

                                    }
                                    catch { }
                                    if (digimon.Forms.Count != forms)
                                        digimon.Forms = new EvolvedForms(forms);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: GetDigimon({1})\n{0}", e, DigiId);
            }
            return digimon;
        }

        public void SaveClient(GameClient client)
        {
            try
            {
                using (MySqlConnection con = Connects())
                {
                    Query qry = new Query(Query.QueryMode.UPDATE, "acct", new Tuple<string, object>("accountId", client.AccountID));
                    qry.Add("crowns", client.crowns);
                    qry.Add("membership", client.Membership);
                    qry.Add("cashvault", client.CashVault.Serialize());
                    qry.Add("accountStorage", client.AccountStorage.Serialize());
                    using (MySqlCommand cmd = qry.GetCommand(con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void SaveTamer(GameClient client)
        {
            int lastChar = -1, charId = -1;

            try
            {
                using (MySqlConnection con = Connects())
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `accountId` = @acct", con))
                {
                    cmd.Parameters.AddWithValue("@acct", client.AccountID);
                    using (MySqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                        if (dr.HasRows && dr.Read())
                        {
                            lastChar = (int)dr["lastChar"];
                            if (lastChar != -1)
                            {
                                charId = (int)dr[string.Format("char{0}", lastChar + 1)];
                            }
                        }
                }
                SaveClient(client);
                if (lastChar != -1)
                {
                    Character Tamer = client.Tamer;
                    using (MySqlConnection con = Connects())
                    {
                        Query qry = new Query(Query.QueryMode.UPDATE, "chars", new Tuple<string, object>("characterId", Tamer.CharacterId));
                        qry.Add("charModel", (int)Tamer.Model);
                        qry.Add("charName", Tamer.Name);
                        qry.Add("charLv", Tamer.Level);
                        qry.Add("experience", Tamer.EXP);
                        qry.Add("money", Tamer.Money);


                        qry.Add("partner", Tamer.DigimonList[0].DigiId);
                        if (Tamer.DigimonList[1] == null) qry.Add("mercenary1", null);
                        else qry.Add("mercenary1", Tamer.DigimonList[1].DigiId);
                        if (Tamer.DigimonList[2] == null) qry.Add("mercenary2", null);
                        else qry.Add("mercenary2", Tamer.DigimonList[2].DigiId);
                        if (Tamer.DigimonList[3] == null) qry.Add("mercenary3", null);
                        else qry.Add("mercenary3", Tamer.DigimonList[3].DigiId);
                        if (Tamer.DigimonList[4] == null) qry.Add("mercenary4", null);
                        else qry.Add("mercenary4", Tamer.DigimonList[4].DigiId);


                        qry.Add("map", Tamer.Location.Map);
                        qry.Add("x", Tamer.Location.PosX);
                        qry.Add("y", Tamer.Location.PosY);

                        qry.Add("inventoryLimit", Tamer.InventorySize);
                        qry.Add("storageLimit", Tamer.StorageSize);
                        qry.Add("archiveLimit", Tamer.ArchiveSize);
                        qry.Add("mercenaryLimit", Tamer.mercenaryLimit);

                        qry.Add("maxHP", Tamer.MaxHP);
                        qry.Add("maxDS", Tamer.MaxDS);
                        qry.Add("HP", Tamer.HP);
                        qry.Add("DS", Tamer.DS);
                        qry.Add("AT", Tamer.AT);
                        qry.Add("DE", Tamer.DE);
                        qry.Add("MS", Tamer.MS);
                        qry.Add("Fatigue", Tamer.Fatigue);
                        qry.Add("XAI", Tamer.XAI);
                        qry.Add("XGauge", Tamer.XGauge);

                        qry.Add("incubator", Tamer.Incubator);
                        qry.Add("incubatorLevel", Tamer.IncubatorLevel);
                        qry.Add("incubatorBackup", Tamer.IncubatorBackup);
                        qry.Add("title", Tamer.CurrentTitle);

                        BinaryFormatter f = new BinaryFormatter();
                        using (MemoryStream m = new MemoryStream())
                        {
                            f.Serialize(m, Tamer.ArchivedDigimon);
                            qry.Add("archive", m.ToArray());
                        }


                        //Trying to add cashvault and tempvault
                        qry.Add("inventory", Tamer.Inventory.Serialize());
                        qry.Add("equipment", Tamer.Equipment.Serialize());
                        qry.Add("chipset", Tamer.ChipSets.Serialize());
                        qry.Add("jogchipset", Tamer.ChipSets.Serialize());
                        qry.Add("storage", Tamer.Storage.Serialize());
                        qry.Add("quests", Tamer.Quests.Serialize());
                        qry.Add("sealmaster", Tamer.Seals.Serialize());
                        //qry.Add("cashvault",Tamer.CashVault.Serialize());
                        //qry.Add("tempcashvault", Tamer.TempCashVault.Serialize());
                        qry.Add("friendList", Tamer.friends.Serialize());

                        using (MySqlCommand cmd = qry.GetCommand(con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    for (int i = 0; i < Tamer.DigimonList.Length; i++)
                    {
                        if (Tamer.DigimonList[i] != null)
                            SaveDigimon(Tamer.DigimonList[i]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void SaveDigimon(Digimon digimon)
        {
            try
            {
                using (MySqlConnection connection = Connects())
                {
                    Query qry = new Query(Query.QueryMode.UPDATE, "digimon", new Tuple<string, object>("digimonId", digimon.DigiId));
                    qry.Add("digiModel", digimon.CurrentForm);

                    qry.Add("digiName", digimon.Name);
                    qry.Add("digiLv", digimon.Level);
                    qry.Add("exp", digimon.EXP);
                    qry.Add("digiSize", digimon.Size);
                    qry.Add("sync", digimon.Stats.Intimacy);
                    qry.Add("unlocked_levels", digimon.levels_unlocked);

                    qry.Add("forms", digimon.Forms.Serialize());

                    using (MySqlCommand cmd = qry.GetCommand(connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void SaveTamerPosition(GameClient client)
        {
            int lastChar = -1, charId = -1;
            try
            {
                using (MySqlConnection con = Connects())
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `accountId` = @acct", con))
                {
                    cmd.Parameters.AddWithValue("@acct", client.AccountID);

                    using (MySqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                    {
                        if (dr.HasRows && dr.Read())
                        {
                            lastChar = (int)dr["lastChar"];
                            if (lastChar != -1)
                            {
                                charId = (int)dr[string.Format("char{0}", lastChar + 1)];
                            }
                        }
                    }
                }
                if (lastChar != -1)
                {
                    Character Tamer = client.Tamer;
                    using (MySqlConnection con = Connects())
                    {
                        Query qry = new Query(Query.QueryMode.UPDATE, "chars", new Tuple<string, object>("characterId", Tamer.CharacterId));
                        qry.Add("map", Tamer.Location.Map);
                        qry.Add("x", Tamer.Location.PosX);
                        qry.Add("y", Tamer.Location.PosY);
                        using (MySqlCommand cmd = qry.GetCommand(con))
                        {
                            cmd.ExecuteNonQuery();
                        }

                        for (int i = 0; i < Tamer.DigimonList.Length; i++)
                        {
                            if (Tamer.DigimonList[i] != null)
                                SaveDigimon(Tamer.DigimonList[i]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                SaveTamerPosition(client);
            }
        }
    }
}
