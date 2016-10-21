using System;
using System.IO;
using System.Web;
using cwserver.Models;
using ServiceStack.OrmLite;

namespace cwserver.DBConnection {
    public static class DB {
        static DB() {
            InitConnection();
            InitDatabase();
        }

        public static OrmLiteConnection Connection { get; private set; }

        private static void InitConnection() {
            var dbPath = Environment.GetEnvironmentVariable("CWSERVER_DBPATH") ??
                         Path.Combine(HttpRuntime.AppDomainAppPath, "db");

            var baseName = Path.Combine(dbPath, "cwserver.db3");
            Directory.CreateDirectory(dbPath);
            var factory = new OrmLiteConnectionFactory(baseName, SqliteDialect.Provider);
            Connection = factory.Open() as OrmLiteConnection;
        }

        private static void InitDatabase() {
            Connection.CreateTableIfNotExists<User>();
            if (Connection.Count<User>() == 0) UserDatabase.RegisterUser("admin", "password", "Admin");
        }
    }
}
