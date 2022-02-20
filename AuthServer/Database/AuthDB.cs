using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using AuthServer.Network;
using AuthServer.Database.Interfaces;
using Yggdrasil.Database.Interfaces;
using Yggdrasil.Network;

namespace AuthServer.Database
{
    public class AuthDB : ILoginDatabase
    {
        public readonly IDatabaseContext _context;


        //Sistema de codificaçao da senha
        public static string SHA2(string value)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 shaM = new SHA256Managed())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(value);
                buffer = shaM.ComputeHash(buffer);
                for (int i = 0; i < buffer.Length; i++)
                    sb.Append(buffer[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string SHA5(string value)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA512 shaM = new SHA512Managed())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(value);
                buffer = shaM.ComputeHash(buffer);
                for (int i = 0; i < buffer.Length; i++)
                    sb.Append(buffer[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public AuthDB(IDatabaseContext context)
        {
            _context = context;
        }
        
        public int CheckAccount(string username, string password)
        {
            int _res = 0;
            var res = UserDataQuery("CALL `getAccountAuthResult`('{0}', '{1}');", username, password);
            foreach (var r in res[0]) _res = (int)r.Value;
            return _res;
        }

        public int CheckSecurity(string username, string secondary)
        {
            int _res = 0;
            var res = UserDataQuery("CALL `getAccountSecondPasswordResult`('{0}'), '{1}');", username, secondary);
            foreach (var r in res[0]) _res = (int)r.Value;
            return _res;
        }

        public int GetAccountID(string username)
        {
            int _res = 0;
            var res = UserDataQuery("CALL `getAccountID`('{0}');", username);
            foreach (var r in res[0]) _res = (int)r.Value;
            return _res;
        }

        public string GetSecondary(string username)
        {
            //string res = AuthDB.UserDataQuery("CALL `getAccountSecondary`('{0}');", username);
            return "";
        }

        public void UserDataExecute(string query, params object[] args)
        {
            _context.Connection = _context.Connect();
            _context.Execute(query, args);
            _context.Connection = null;
        }

        public List<Dictionary<string, object>> UserDataQuery(string query, params object[] args)
        {
            _context.Connection = _context.Connect();
            var res = _context.Query(query, args);
            _context.Connection = null;
            return res;
        }
        public MySqlConnection Connects()
        {
            return _context.GetNewConnection();
        }
        
        public int Validate(AuthClient client, string user, string pass)
        {
            var RNG = new Random();
            int level = 0;
            try
            {
                using (MySqlConnection Connection = Connects())
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `username` = @user", Connection))
                {
                    cmd.Parameters.AddWithValue("@user", user);

                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            if (read.Read())
                            {
                                if (read["username"].ToString() == user && read["password"].ToString() == SHA2(pass))
                                {
                                    level = (int)read["level"];
                                    client.AccessLevel = (int)read["level"];
                                    client.Username = user;
                                    client.UniqueID = RNG.Next(1, int.MaxValue);
                                    client.AccountID = Convert.ToUInt32((int)read["accountId"]);
                                    client.SecondaryPassword = (string)read["securitycode"];
                                }
                                else
                                {
                                    //Wrong Pass
                                    level = -2;
                                }
                            }
                        }
                        else

                        { level = -3; }
                    }

                }
                using (MySqlConnection Connection = Connects())
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `chars` WHERE `accountId` = @id", Connects()))
                {
                    cmd.Parameters.AddWithValue("@id", client.AccountID);
                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            while (read.Read())
                            {
                                client.Characters++;
                            }
                        }
                        else client.Characters = 0;
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: Validate\n{0}", e);
                level = -1;
            }
            return level;
        }
        
        public int SecurityCode(Client client)
        {
            int securitymessage = -1;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `securitycode` FROM `acct` WHERE `accoundId`= @acct", Connects()))
                {
                    cmd.Parameters.AddWithValue("@acct", client.AccountID);
                    using (MySqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                if (data["securitycode"] == null)
                                {
                                    securitymessage = 0;
                                }
                                if (data["securitycode"] != null)
                                {
                                    securitymessage = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: GetServersInfo\n{0}", e);
            }
            return securitymessage;
        }

        public int ServerStatus()
        {
            int message = -1;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `maintenance` FROM `servers` WHERE `serverId`= 0", Connects()))
                {
                    using (MySqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                message = (int)data["maintenance"];
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: GetServersInfo\n{0}", e);
            }
            return message;
        }
        
        public int ServerMaintenance(string name)
        {
            int message = -1;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `maintenance` FROM `servers` WHERE `name`= @name", Connects()))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    using (MySqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                message = (int)data["maintenance"];
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: GetServersInfo\n{0}", e);
            }
            return message;
        }
        
        public void LoadUser(AuthClient client)
        {
            var RNG = new Random();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `username` = @user", Connects()))
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
                                    new MySqlCommand("UPDATE `acct` SET `uniId` = @uniId WHERE `accountId` = @id", Connects()))
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
        public void LoadUser(AuthClient client, uint AccountID, int UniId)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM `acct` WHERE `accountId` = @id", Connects()))
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
        public Dictionary<int, string> GetServers()
        {
            Dictionary<int, string> servers = new Dictionary<int, string>();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `serverid`, `name` FROM servers", Connects()))
                {
                    using (MySqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                servers.Add((int)data["serverid"], data["name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: GetServers\n{0}", e);
            }
            return servers;
        }


        public int ServerLoad(string name)
        {
            int message = -1;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `serverload` FROM `servers` WHERE `name`= @name", Connects()))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    using (MySqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                message = (int)data["serverload"];
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: GetServersInfo\n{0}", e);
            }
            return message;
        }

        public int ServerNewOrNo(string name)
        {
            int message = -1;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT `isNew?` FROM `servers` WHERE `name`= @name", Connects()))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    using (MySqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                message = (int)data["isNew?"];
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: GetServersInfo\n{0}", e);
            }
            return message;
        }
        
        public KeyValuePair<int, string> GetServer(int ID)
        {
            KeyValuePair<int, string> k = new KeyValuePair<int, string>(7030, "127.0.0.1");
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT port, INET_NTOA(ip) ip FROM servers WHERE `serverId` = @id", Connects()))
                {
                    cmd.Parameters.AddWithValue("@id", ID);

                    using (MySqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                k = new KeyValuePair<int, string>((int)data["port"], data["ip"].ToString());
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error: GetServer\n{0}", e);
            }
            return k;
        }

        
        /// <summary>
        /// Get the number of characters for a specific account id
        /// </summary>
        /// <param name="AcctId">Account Id to match</param>
        /// <returns>The number of characters tied to AcctId</returns>
        public int GetNumChars(uint AcctId)
        {
            int characters = 0;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM `chars` WHERE `accountId` = @id"
                    , Connects()))
                {
                    cmd.Parameters.AddWithValue("@id", AcctId);

                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            while (read.Read())
                            {
                                characters++;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: GetCharacters\n{0}", e);
            }
            return characters;
        }

       
    }
}
