using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.Data.Common;
using System.Data;
using System.IO;

namespace cwserver.DBConnection {
    public static class DBConnection {
        private static readonly SQLiteConnection Connection;

        static DBConnection() {
            string dbPath = ConfigurationManager.AppSettings["DB_PATH"];

            if (dbPath == null) {
                throw new Exception("DB_PATH variable is not set in config file");
            }

            string baseName = Path.Combine(dbPath, "cwserver.db3");
            SQLiteConnection.CreateFile(baseName);

            var factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
            Connection = (SQLiteConnection)factory.CreateConnection();
            Connection.ConnectionString = "Data Source = " + baseName;
            Connection.Open();

            using (var command = new SQLiteCommand(Connection)) {
                command.CommandText =
                    @"CREATE TABLE users (
                    id integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    name char(100) NOT NULL UNIQUE,
                    pass_hash char(100) NOT NULL,
                    GUID char(100) NOT NULL UNIQUE
                    );";
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                UserDatabase.RegisterUser("admin", "password");
            }
        }

        public static SQLiteCommand GetCommand() {
            return new SQLiteCommand(Connection);
        }
    }
}
