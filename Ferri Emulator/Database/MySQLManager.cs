using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using NHibernate.Cfg;
using Ferri_Emulator.Database.Mappings;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Automapping;
using MySql.Data.MySqlClient;
using System.Data;

namespace Ferri_Emulator.Database
{
    public class MySQLManager
    {
        private string ConnectionString;

        public void CreateConnectionString()
        {
            string Server;
            Engine.Configuration.PopValue<string>("mysql_hostname", out Server);

            string UserID;
            Engine.Configuration.PopValue<string>("mysql_username", out UserID);

            string Password;
            Engine.Configuration.PopValue<string>("mysql_password", out Password);

            string Database;
            Engine.Configuration.PopValue<string>("mysql_database", out Database);

            this.ConnectionString = new MySqlConnectionStringBuilder()
            {
                Server = Server,
                UserID = UserID,
                Password = Password,
                Database = Database
            }.ToString();
        }

        public void DoQuery(string Qry)
        {
            MySqlHelper.ExecuteNonQuery(this.ConnectionString, Qry);
        }

        public DataTable ReadTable(string Qry)
        {
            return MySqlHelper.ExecuteDataset(this.ConnectionString, Qry).Tables[0];
        }

        public DataRow ReadRow(string Qry)
        {
            return MySqlHelper.ExecuteDataRow(this.ConnectionString, Qry);
        }

        public int ReadInt(string Qry)
        {
            return (int)MySqlHelper.ExecuteScalar(this.ConnectionString, Qry);
        }

        public string ReadString(string Qry)
        {
            return (string)MySqlHelper.ExecuteScalar(this.ConnectionString, Qry);
        }
    }
}
