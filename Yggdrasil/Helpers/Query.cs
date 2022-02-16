using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Yggdrasil.Helpers
{
    public class Query
    {
        public enum QueryMode
        {
            UPDATE = 0,
            INSERT = 1
        }
        public QueryMode Mode;
        public Tuple<string, object> Where = null;

        private string table;
        private List<Tuple<string, string, object>> Parameters = new List<Tuple<string, string, object>>();

        public Query(QueryMode Mode, string Table, Tuple<string, object> WhereCond)
        {
            this.Mode = Mode;
            table = Table;
            Where = WhereCond;
        }

        public Query(QueryMode Mode, string Table)
        {
            this.Mode = Mode;
            table = Table;
        }

        public void Add(string Column, string ParamName, object value)
        {
            Parameters.Add(new Tuple<string, string, object>(Column, ParamName, value));
        }

        public void Add(string Column, object value)
        {
            Parameters.Add(new Tuple<string, string, object>(Column, Column, value));
        }

        public MySqlCommand GetCommand(MySqlConnection Connection)
        {
            MySqlCommand cmd = null;
            cmd = new MySqlCommand(CommandText, Connection);
            foreach (Tuple<string, string, object> Entry in Parameters)
            {
                cmd.Parameters.AddWithValue(Entry.Item2, Entry.Item3);
            }
            if (Where != null)
                cmd.Parameters.AddWithValue(Where.Item1, Where.Item2);
            return cmd;
        }

        public string CommandText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (Mode == QueryMode.UPDATE)
                {
                    sb.AppendFormat("UPDATE `{0}` SET ", table);
                    for (int i = 0; i < Parameters.Count; i++)
                    {
                        Tuple<string, string, object> Entry = Parameters[i];
                        sb.AppendFormat("`{0}` = @{1}", Entry.Item1, Entry.Item2);
                        if (i < Parameters.Count - 1)
                            sb.Append(", ");
                    }
                    if (Where != null)
                        sb.AppendFormat(" WHERE `{0}` = @{1}", Where.Item1, Where.Item1);
                }
                else if (Mode == QueryMode.INSERT)
                {
                    sb.AppendFormat("INSERT INTO `{0}` (", table);

                    StringBuilder values = new StringBuilder();
                    for (int i = 0; i < Parameters.Count; i++)
                    {
                        Tuple<string, string, object> Entry = Parameters[i];
                        sb.AppendFormat("`{0}`", Entry.Item1);
                        values.AppendFormat("@{0}", Entry.Item2);
                        if (i < Parameters.Count - 1)
                        {
                            sb.Append(", ");
                            values.Append(", ");
                        }
                    }
                    sb.AppendFormat(") VALUES ({0})", values);
                    if (Where != null)
                    {
                        sb.AppendFormat("WHERE `{1}` = @{2}", Where.Item1, Where.Item1);
                    }
                }
                return sb.ToString();
            }
        }
    }
}
