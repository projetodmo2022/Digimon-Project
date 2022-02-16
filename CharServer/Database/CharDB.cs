using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MySql.Data.MySqlClient;
using System;
using CharServer.Network;
using System.Data;
using CharServer.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Yggdrasil.Database;
using Yggdrasil.Database.Interfaces;
using Yggdrasil.Entities;
using Yggdrasil.Helpers;

namespace CharServer.Database
{
    public class CharDB : ICharacterDatabase
    {
        private readonly IDatabaseContext _databaseContext;

        public CharDB(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        
        public  byte[][] TamerData = new byte[][] { new byte[] { 10, 2, 90, 80 }, new byte[] { 8, 1, 100, 90 }, new byte[] { 8, 1, 90, 100 }, new byte[] { 8, 1, 90, 100 } };
        public void UserDataExecute(string query, params object[] args)
        {
            //_databaseContext.GetNewConnection() = _databaseContextConnect(UserDataDBConfig.Instance.Host, UserDataDBConfig.Instance.Username, UserDataDBConfig.Instance.Password, UserDataDBConfig.Instance.Database);
            _databaseContext.Execute(query, args);
        }

        public List<Dictionary<string, object>> UserDataQuery(string query, params object[] args)
        {
            //_databaseContext.GetNewConnection() = CharDB.Connect(UserDataDBConfig.Instance.Host, UserDataDBConfig.Instance.Username, UserDataDBConfig.Instance.Password, UserDataDBConfig.Instance.Database);
            var res = _databaseContext.Query(query, args);
            //CharDB.Connection = null;
            return res;
        }

        public  List<Character> GetCharacters(int AccountID)
        {
            List<Character> chars = new List<Character>();
           // CharDB.Connection = CharDB.Connect(UserDataDBConfig.Instance.Host, UserDataDBConfig.Instance.Username, UserDataDBConfig.Instance.Password, UserDataDBConfig.Instance.Database);
            return chars;
        }
        
        private  Random RNG = new Random();
        public  void LoadUser(CharClient client)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `username` = @user", _databaseContext.GetNewConnection()))
                {
                    cmd.Parameters.AddWithValue("@user", client.Username);

                    using (MySqlDataReader read = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (read.HasRows)
                        {
                            if (read.Read())
                            {
                                client.AccessLevel = (int)read["level"];
                                client.AccountID = Convert.ToUInt32((int)read["accountId"]);

                                int uniId = RNG.Next(1, int.MaxValue);
                                using (MySqlCommand updateUniId =
                                    new MySqlCommand("UPDATE `acct` SET `uniId` = @uniId WHERE `accountId` = @id", _databaseContext.GetNewConnection()))
                                {
                                    updateUniId.Parameters.AddWithValue("@uniId", uniId);
                                    updateUniId.Parameters.AddWithValue("@id", read["accountId"]);
                                    updateUniId.ExecuteNonQuery();

                                    client.UniqueID = uniId;
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

        /// <summary>
        /// Loads all user data into the Client class. Used by the Lobby Server
        /// </summary>
        /// <param name="client">client</param>
        /// <param name="AccountID">AccountID to find</param>
        /// <param name="UniId">Unique ID</param>
        public  void LoadUser(CharClient client, uint AccountID, int UniId)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `accountId` = @id", _databaseContext.GetNewConnection()))
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
                                client.UniqueID = UniId;
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
        public  List<Character> GetCharacters(uint AcctId)
        {
            List<Character> chars = new List<Character>();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM `chars` WHERE `accountId` = @id"
                    , _databaseContext.GetNewConnection()))
                {
                    cmd.Parameters.AddWithValue("@id", AcctId);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Character Tamer = new Character();
                                Tamer.AccountId = AcctId;
                                Tamer.CharacterId = Convert.ToUInt32((int)dr["characterId"]);
                                Tamer.CharacterPos = (int)dr["characterPos"];
                                Tamer.Model = (CharacterModel)(int)dr["charModel"];
                                Tamer.Name = (string)dr["charName"];
                                Tamer.Level = (int)dr["charLv"];
                                Tamer.Location = new Position((short)(int)dr["map"], (int)dr["x"], (int)dr["y"]);

                                Tamer.Partner = GetDigimon((uint)(int)dr["partner"]);
                                if (dr["mercenary1"] != DBNull.Value)
                                {
                                    int mercId = (int)dr["mercenary1"];
                                    Digimon merc = GetDigimon((uint)mercId);
                                    Tamer.DigimonList[1] = merc;
                                }
                                if (dr["mercenary2"] != DBNull.Value)
                                {
                                    int mercId = (int)dr["mercenary2"];
                                    Digimon merc = GetDigimon((uint)mercId);
                                    Tamer.DigimonList[2] = merc;
                                }

                                try
                                {
                                    BinaryFormatter f = new BinaryFormatter();
                                    using (MemoryStream m = new MemoryStream((byte[])dr["archive"]))
                                    {
                                        Tamer.ArchivedDigimon = (uint[])f.Deserialize(m);
                                    }
                                    for (int i = 0; i < Tamer.ArchivedDigimon.Length; i++)
                                    {
                                        if (Tamer.ArchivedDigimon[i] != 0)
                                        {
                                            Digimon digi = GetDigimon(Tamer.ArchivedDigimon[i]);
                                            ResetModel(digi.DigiId, digi.Species);
                                        }
                                    }

                                }
                                catch { Tamer.ArchivedDigimon = new uint[150]; }

                                try
                                {
                                    Tamer.Equipment = ItemList.Deserialize((byte[])dr["equipment"]);
                                }
                                catch
                                {
                                    Tamer.Equipment = new ItemList(15);
                                }

                                chars.Add(Tamer);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: GetCharacters\n{0}", e);
            }

            return chars;
        }
        
        public  Digimon GetDigimon(uint DigiId)
        {
            Digimon digimon = null;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM `digimon` WHERE `digimonId` = @id"
                    , _databaseContext.GetNewConnection()))
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

        public  void ResetModel(uint DigiId, int digiType)
        {
            try
            {
                using (MySqlConnection mysql = _databaseContext.GetNewConnection())
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
        public  uint CreateDigimon(uint charId, string digiName, int digiModel)
        {
            uint digiId = 0;
            try
            {
                digiId = CreateMercenary(charId, digiName, digiModel, 0, 10000, 5);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: CreateDigimon()\n{0}", e);
            }
            return digiId;
        }
        public  uint CreateMercenary(uint charId, string digiName, int digiModel, int digiScale, int digiSize, int intimacy)
        {
            uint digiId = 0;
            try
            {
                DigimonData dData = DigimonListDB.GetDigimon(digiModel);
                using (MySqlConnection con = _databaseContext.GetNewConnection())
                {
                    var qry = new Query(Query.QueryMode.INSERT, "digimon");
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
                    _databaseContext.GetNewConnection()))
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
        public  int CreateCharacter(int position, uint AcctId, int pos, int charModel, string charName, int digiModel)
        {
            int charId = -1;
            try
            {
                using (MySqlConnection con = _databaseContext.GetNewConnection())
                {
                    var qry = new Query(Query.QueryMode.INSERT, "chars");
                    qry.Add("charName", charName);
                    qry.Add("charModel", charModel);
                    qry.Add("characterPos", position);
                    qry.Add("accountId", AcctId);

                    qry.Add("inventory", new ItemList(150).Serialize());
                    qry.Add("storage", new ItemList(315).Serialize());
                    qry.Add("equipment", new ItemList(15).Serialize());
                    //qry.Add("quests", new QuestList().Serialize());
                    //qry.Add("friendList", new FriendList(20).Serialize());

                    qry.Add("maxHP", TamerData[charModel - 80001][2]);
                    qry.Add("maxDS", TamerData[charModel - 80001][3]);
                    qry.Add("HP", TamerData[charModel - 80001][2]);
                    qry.Add("DS", TamerData[charModel - 80001][3]);
                    qry.Add("AT", TamerData[charModel - 80001][0]);
                    qry.Add("DE", TamerData[charModel - 80001][1]);

                    using (MySqlCommand cmd = qry.GetCommand(con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                using (MySqlConnection con = _databaseContext.GetNewConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(
                           "SELECT * FROM `chars` WHERE `accountId` = @acctId AND `charName` = @charName",
                           con))
                    {
                        cmd.Parameters.AddWithValue("@acctId", AcctId);
                        cmd.Parameters.AddWithValue("@charName", charName);

                        using (MySqlDataReader read = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                        {
                            if (read.HasRows)
                            {
                                if (read.Read())
                                {
                                    charId = (int)read["characterId"];
                                }
                            }
                        }
                    }
                }
                using (MySqlConnection con = _databaseContext.GetNewConnection())
                {
                    using (MySqlCommand
                        cmd = new MySqlCommand(
                            string.Format("UPDATE `acct` SET `char{0}` = @charId WHERE `accountId` = @acct", pos + 1),
                            con))
                    {
                        cmd.Parameters.AddWithValue("@charId", charId);
                        cmd.Parameters.AddWithValue("@acct", AcctId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: CreateCharacter()\n{0}", e);
            }
            return charId;
        }

        public  void SetPartner(int charId, int digiId)
        {
            try
            {
                using (MySqlConnection con = _databaseContext.GetNewConnection())
                using (MySqlCommand cmd = new MySqlCommand(
                "UPDATE `chars` SET `partner` = @digiId WHERE `characterId` = @charId", con))
                {
                    cmd.Parameters.AddWithValue("@digiId", digiId);
                    cmd.Parameters.AddWithValue("@charId", charId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: SetPartner()\n{0}", e);
            }
        }

        public  void SetTamer(int charId, int digiId)
        {
            try
            {
                using (MySqlConnection con = _databaseContext.GetNewConnection())
                using (MySqlCommand cmd = new MySqlCommand(
                    "UPDATE `digimon` SET `characterId` = @charId WHERE `digimonId` = @digiId", con))
                {
                    cmd.Parameters.AddWithValue("@digiId", digiId);
                    cmd.Parameters.AddWithValue("@charId", charId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: SetTamer()\n{0}", e);
            }
        }

        public  bool VerifyCode(uint acctId, string code)
        {
            bool allow = false;
            try
            {
                using (MySqlConnection con = _databaseContext.GetNewConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `email` from `acct` WHERE `accountId` = @acct",
                    con))
                    {
                        cmd.Parameters.AddWithValue("@acct", acctId);
                        using (MySqlDataReader read = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                        {
                            if (read.HasRows && read.Read())
                            {
                                if (code.Equals((string)read["email"]))
                                    allow = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: VerifyCode()\n{0}", e);
            }
            return allow;
        }

        /// <summary>
        /// Deletes a Tamer and unbinds the emptied slot from account
        /// </summary>
        /// <param name="acctId">Id of the account to delete from</param>
        /// <param name="slot">The slot to delete</param>
        public  bool DeleteTamer(uint acctId, int slot)
        {
            int charId = -1;
            bool completed = false;
            try
            {
                MySqlConnection con = _databaseContext.GetNewConnection();
                string sSlot = string.Format("char{0}", slot + 1);
                using (MySqlCommand cmd = new MySqlCommand(
                    string.Format("SELECT `char{0}` FROM `acct` WHERE `accountId` = @acct", slot + 1), con))
                {
                    cmd.Parameters.AddWithValue("@acct", acctId);
                    using (MySqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                    {

                        if (dr.HasRows && dr.Read())
                        {
                            charId = (int)dr[sSlot];
                        }
                    }
                }
                if (charId != -1)
                {

                    DeleteDigimons(charId);

                    using (MySqlCommand cmd = new MySqlCommand("DELETE FROM `chars` WHERE `characterId` = @char", con))
                    {
                        cmd.Parameters.AddWithValue("@char", charId);
                        cmd.ExecuteNonQuery();
                    }
                    using (MySqlCommand cmd = new MySqlCommand(
                        string.Format("UPDATE `acct` SET `char{0}` = NULL WHERE `accountId` = @acct", slot + 1)
                        , _databaseContext.GetNewConnection()))
                    {
                        cmd.Parameters.AddWithValue("@acct", acctId);
                        cmd.ExecuteNonQuery();
                    }
                    completed = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: DeleteTamer()\n{0}", e);
            }
            return completed;
        }

        /// <summary>
        /// Deletes all Digimon belonging to the character with the id charId
        /// </summary>
        /// <param name="charId">Id of the character whose Digimon to delete</param>
        public  void DeleteDigimons(int charId)
        {
            try
            {
                MySqlConnection con = _databaseContext.GetNewConnection();
                using (MySqlCommand cmd = new MySqlCommand(
                    "DELETE FROM `digimon` WHERE `characterId` = @char"
                    , con))
                {
                    cmd.Parameters.AddWithValue("@char", charId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: DeleteDigimon()\n{0}", e);
            }
        }

        /// <summary>
        /// Update the last character used
        /// </summary>
        /// <param name="acctId">Id of the account to update</param>
        /// <param name="slot">Slot last used</param>
        public  void SetLastChar(uint acctId, int slot)
        {
            try
            {
                using (MySqlConnection Connection = _databaseContext.GetNewConnection())
                using (MySqlCommand cmd = new MySqlCommand("UPDATE `acct` SET `lastChar` = @char WHERE `accountId` = @acct",
                    Connection))
                {
                    cmd.Parameters.AddWithValue("@acct", acctId);
                    cmd.Parameters.AddWithValue("@char", slot);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: SetLastChar()\n{0}", e);
            }
        }

        public  bool NameAvail(string name)
        {
            //if (Connection == null) Connection = Connect();
            bool avail = false;
            try
            {
                using (MySqlConnection con = _databaseContext.GetNewConnection())
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `chars` WHERE `charName` = @name", con))
                {
                    cmd.Parameters.AddWithValue("@name", name);

                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows) avail = false;
                        else avail = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: NameAvail({1})\n{0}", e, name);
            }
            return avail;
        }

        public  Position GetTamerPosition(uint acctId, int slot)
        {
            Position Location = null;
            int charId = -1;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `accountId` = @acct", _databaseContext.GetNewConnection()))
                {
                    cmd.Parameters.AddWithValue("@acct", acctId);
                    using (MySqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                    {
                        if (dr.HasRows && dr.Read())
                        {
                            if (slot != -1)
                            {
                                charId = (int)dr[string.Format("char{0}", slot + 1)];
                            }
                        }
                    }
                }

                if (slot != -1)
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT `map`,`x`,`y` FROM `chars` WHERE `characterId` = @char", _databaseContext.GetNewConnection()))
                    {
                        cmd.Parameters.AddWithValue("@char", charId);
                        using (MySqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleRow))
                        {

                            if (dr.HasRows && dr.Read())
                            {
                                Location = new Position((int)dr["map"], (int)dr["x"], (int)dr["y"]);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return Location;
        }
    

  
    }
}
