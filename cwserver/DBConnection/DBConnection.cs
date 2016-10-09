using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SQLite;
using System.Data.Common;
using System.Data;
using System.IO;

namespace cwserver {
    public static class DBConnection {
        private static SQLiteConnection connection;

        static DBConnection() {
            string dbPath = ConfigurationManager.AppSettings["DB_PATH"];

            if (dbPath == null) {
                throw new Exception("DB_PATH variable is not set in config file");
            }

            string baseName = Path.Combine(dbPath, "cwserver.db3");
            SQLiteConnection.CreateFile(baseName);

            SQLiteFactory factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
            connection = (SQLiteConnection)factory.CreateConnection();
            connection.ConnectionString = "Data Source = " + baseName;
            connection.Open();

            using (var command = new SQLiteCommand(connection)) {
                command.CommandText =
                    @"CREATE TABLE users (
                    id integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    name char(100) NOT NULL
                    );
                    INSERT INTO users(name) VALUES ('User');";
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        public static SQLiteCommand GetCommand() {
            return new SQLiteCommand(connection);
        }
    }
}
