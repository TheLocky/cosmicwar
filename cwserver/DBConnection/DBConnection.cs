using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Web;

namespace cwserver.DBConnection {
    public static class DBConnection {
        private static SQLiteConnection _connection;

        private static void AddFixtures() {
            UserDatabase.RegisterUser("admin", "password");
        }

        private static void AddFixturesIfNotExist() {
            using (var command = GetCommand()) {
                command.CommandText =
                    @"SELECT COUNT(*) FROM users";
                command.CommandType = CommandType.Text;
                var count = (long)command.ExecuteScalar();

                if (count == 0) {
                    AddFixtures();
                }
            }
        }

        private static void MakeTables() {
            using (var command = GetCommand()) {
                command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS users (
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
            var dbPath = Environment.GetEnvironmentVariable("CWSERVER_DBPATH") ??
                         Path.Combine(HttpRuntime.AppDomainAppPath, "db");

            var baseName = Path.Combine(dbPath, "cwserver.db3");
            Directory.CreateDirectory(dbPath);
            if (!File.Exists(baseName))
                SQLiteConnection.CreateFile(baseName);

            var factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
            _connection = (SQLiteConnection)factory.CreateConnection();
            _connection.ConnectionString = "Data Source = " + baseName;
            _connection.Open();
        }

        static DBConnection() {
            InitConnection();

            MakeTables();
            AddFixturesIfNotExist();
        }

        public static SQLiteCommand GetCommand() {
            return new SQLiteCommand(_connection);
        }
    }
}