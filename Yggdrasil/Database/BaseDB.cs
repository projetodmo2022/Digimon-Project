
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;
using Yggdrasil.Database.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Yggdrasil.Database
{
    public class BaseDB : IDatabaseContext
    {
        private readonly IConfiguration Configuration;

        private static MySqlConnection m_con;

        public MySqlConnection GetNewConnection()
        {
            Connection = Connect();
            return Connection;
        }

        public MySqlConnection Connection
        {
            get
            {
                if (m_con == null || m_con.State != ConnectionState.Open || m_con.Database == "")
                    m_con = Connect();
                return m_con;
            }
            set { m_con = value; }
        }

        public BaseDB(IConfiguration configuration)
        {
            Configuration = configuration;
            Connection = Connect();
        }

        public MySqlConnection Connect()
        {
            return Connect(Configuration.GetConnectionString("DefaultConnection"));
        }

        public MySqlConnection Connect(string connectionString)
        {
            try
            {
                //server={0};uid={1};pwd={2};database={3}
                var conn = new MySqlConnection(connectionString);
                conn.Open();
                return conn;
            }
            catch (MySqlException)
            {
                return null;
            }
        }

        public void Execute(string query, params object[] args)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(query, args), Connection);
            cmd.ExecuteNonQuery();
        }

        public List<Dictionary<string, object>> Query(string query, params object[] args)
        {
            MySqlCommand cmd = new MySqlCommand(String.Format(query, args), Connection);
            MySqlDataReader reader = cmd.ExecuteReader();

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            while (reader.Read())
            {
                Dictionary<string, object> row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(reader.GetName(i), reader.GetValue(i));
                }
                rows.Add(row);
            }
            reader.Close();

            return rows;
        }
    }
}
