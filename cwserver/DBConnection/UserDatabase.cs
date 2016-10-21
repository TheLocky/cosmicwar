using System;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using cwserver.Models;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
using ServiceStack.OrmLite;

namespace cwserver.DBConnection {
    public class UserDatabase : IUserMapper {
        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context) {
            return DB.Connection.Single<User>(u => u.Guid == identifier.ToString());
        }

        public static Guid? GetGuid(string data) {
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

        private static bool CheckUsernameAndPassword(string username, string password) {
            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
        }

        public static Guid? ValidateUser(string username, string password) {
            if (!CheckUsernameAndPassword(username, password)) return null;
            var user = DB.Connection.Single<User>(u => (u.UserName == username) && (u.Pass == GetPasswordHash(password)));
            if (user == null) return null;
            return new Guid(user.Guid);
        }

        public static void RegisterUser(string username, string password, string claim = "User") {
            if (!CheckUsernameAndPassword(username, password))
                throw new Exception("Please type your username and password");

            var newUser = new User {
                UserName = username,
                Pass = GetPasswordHash(password),
                Guid = GetGuid(username).ToString(),
                Claims = new[] {claim}
            };
            try {
                DB.Connection.Insert(newUser);
            }
            catch (SQLiteException e) {
                if (e.ResultCode == SQLiteErrorCode.Constraint)
                    throw new Exception($"Username: '{username}' already existed");
            }
        }
    }
}
