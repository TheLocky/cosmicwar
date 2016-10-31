using System;
using System.Configuration;
using System.IO;
using System.Web;
using CWServer.Models;
using Microsoft.Data.Entity;

namespace CWServer.DBConnection {
    public static class DB {
        public static DatabaseContext Context { get; private set; }

        public static void Init() {
            var dbPath = Environment.GetEnvironmentVariable("CWSERVER_DBPATH") ??
                         Path.Combine(HttpRuntime.AppDomainAppPath, "db");

            var baseName = Path.Combine(dbPath, "cwserver.db3");
            Directory.CreateDirectory(dbPath);

            Context = new DatabaseContext(baseName);
            if (Context.Database.EnsureCreated())
                UserDatabase.RegisterUser("admin", "password", User.Claim.Admin);

            if (bool.Parse(ConfigurationManager.AppSettings["dbLogging"]))
                DBLoggerProvider.AddToContext(Context);
        }

        public class DatabaseContext : DbContext {
            private readonly string _dbName;

            public DatabaseContext(string dbName) {
                _dbName = dbName;
            }

            //Models
            public DbSet<User> Users { get; set; }
            public DbSet<Lobby> Lobbies { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
                optionsBuilder.UseSqlite($"DataSource={_dbName}");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
                modelBuilder.Entity<User>().HasAlternateKey(c => c.UserName);
            }
        }
    }
}
