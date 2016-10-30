using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CWServer.Exceptions;
using CWServer.Models;
using Microsoft.Data.Sqlite;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;

namespace CWServer.DBConnection {
    public class UserDatabase : IUserMapper {
        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context) {
            return DB.Context.Users.SingleOrDefault(u => u.Guid == identifier.ToString());
        }

        public static Guid GetGuid(string data) {
            using (var md5 = MD5.Create()) {
                var hash = md5.ComputeHash(Encoding.Default.GetBytes(data));
                return new Guid(hash);
            }
        }

        public static string GetPasswordHash(string data) {
            using (var sha1 = SHA1.Create()) {
                return Convert.ToBase64String(sha1.ComputeHash(Encoding.Default.GetBytes(data)));
            }
        }

        private static void CheckUsernameAndPassword(string username, string password) {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new CWException("Please type your username and password");
        }

        public static Guid ValidateUser(string username, string password) {
            CheckUsernameAndPassword(username, password);
                
            var user = DB.Context.Users.SingleOrDefault(u =>
                    (u.UserName == username) && (u.PassHash == GetPasswordHash(password)));

            if (user == null)
                throw new CWException("Invalid username or password");
            return new Guid(user.Guid);
        }

        public static void RegisterUser(string username, string password, User.Claim role = User.Claim.User) {
            CheckUsernameAndPassword(username, password);

            try {
                DB.Context.Users.Add(new User {
                    UserName = username,
                    PassHash = GetPasswordHash(password),
                    Guid = GetGuid(username).ToString(),
                    Role = role
                });
                DB.Context.SaveChanges();
            }
            catch (SqliteException e) {
                //SQLITE_CONSTRAINT
                if (e.SqliteErrorCode == 19) throw new CWException($"Username: '{username}' already existed");
            }
        }
    }
}
