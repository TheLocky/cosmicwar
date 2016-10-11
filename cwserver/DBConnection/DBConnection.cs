using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace cwserver.DBConnection {
    public static class DBConnection {
        private static readonly SQLiteConnection Connection;

        static DBConnection() {
            var dbPath = Environment.GetEnvironmentVariable("CWSERVER_DBPATH");

            if (dbPath == null) throw new Exception("CWSERVER_DBPATH environment variable is not set");

            var baseName = Path.Combine(dbPath, "cwserver.db3");
            Directory.CreateDirectory(dbPath);
            SQLiteConnection.CreateFile(baseName);

            var factory = (SQLiteFactory) DbProviderFactories.GetFactory("System.Data.SQLite");
            Connection = (SQLiteConnection) factory.CreateConnection();
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