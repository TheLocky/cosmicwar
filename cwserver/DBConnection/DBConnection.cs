using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace cwserver.DBConnection {
    public static class DBConnection {
        private static SQLiteConnection _connection;

        private static void AddFixtures() {
            UserDatabase.RegisterUser("admin", "password");
        }

        private static void MakeTables() {
            using (var command = GetCommand()) {
                command.CommandText =
                    @"CREATE TABLE users (
                        id integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                        name char(100) NOT NULL UNIQUE,
                        pass_hash char(100) NOT NULL,
                        GUID char(100) NOT NULL UNIQUE
                    );";
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        private static void InitConnection() {
            var dbPath = Environment.GetEnvironmentVariable("CWSERVER_DBPATH");

            if (dbPath == null) {
                throw new Exception("CWSERVER_DBPATH environment variable is not set");
            }

            var baseName = Path.Combine(dbPath, "cwserver.db3");
            Directory.CreateDirectory(dbPath);
            SQLiteConnection.CreateFile(baseName);

            var factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
            _connection = (SQLiteConnection)factory.CreateConnection();
            _connection.ConnectionString = "Data Source = " + baseName;
            _connection.Open();
        }

        static DBConnection() {
            InitConnection();

            MakeTables();
            AddFixtures();
        }

        public static SQLiteCommand GetCommand() {
            return new SQLiteCommand(_connection);
        }
    }
}