using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Yggdrasil.Database.Interfaces
{
    public interface IDatabaseContext
    {
        public MySqlConnection GetNewConnection();
        public MySqlConnection Connection { get; set; }
        public MySqlConnection Connect();
        public MySqlConnection Connect(string connectionString);
        public void Execute(string query, params object[] args);
        public List<Dictionary<string, object>> Query(string query, params object[] args);
    }
}