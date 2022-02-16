using System.Collections.Generic;
using AuthServer.Network;
using MySql.Data.MySqlClient;
using Yggdrasil.Network;

namespace AuthServer.Database.Interfaces
{
    public interface ILoginDatabase
    {

        public int CheckAccount(string username, string password);
        public int CheckSecurity(string username, string secondary);
        public int GetAccountID(string username);
        public string GetSecondary(string username);
        public void UserDataExecute(string query, params object[] args);
        public List<Dictionary<string, object>> UserDataQuery(string query, params object[] args);
        public MySqlConnection Connects();
        public int Validate(AuthClient client, string user, string pass);
        public int SecurityCode(Client client);
        public int ServerStatus();
        public int ServerMaintenance(string name);
        public void LoadUser(AuthClient client);
        public void LoadUser(AuthClient client, uint AccountID, int UniId);
        public Dictionary<int, string> GetServers();
        public int ServerLoad(string name);
        public int ServerNewOrNo(string name);
        public KeyValuePair<int, string> GetServer(int ID);
        public int GetNumChars(uint AcctId);
    }
}