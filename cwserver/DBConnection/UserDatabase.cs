using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using cwserver.Models;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;

namespace cwserver.DBConnection {
    public class UserDatabase : IUserMapper {
        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context) {
            using (var cmd = DBConnection.GetCommand()) {
                cmd.CommandText = @"SELECT name FROM Users WHERE GUID = @GUID";
                cmd.Parameters.AddWithValue("@GUID", identifier.ToString());
                var result = cmd.ExecuteScalar();
                return result == null
                    ? null
                    : new UserModel(result.ToString(), new List<string>());
            }
        }

        public static Guid? GetGuid(string data) {
            using (var md5 = MD5.Create()) {
                var hash = md5.ComputeHash(Encoding.Default.GetBytes(data));
                return new Guid(hash);
            }
        }

        public static string GetPasswordHash(string data) {
            using (var sha1 = SHA1.Create()) {
                return System.Convert.ToBase64String(sha1.ComputeHash(Encoding.Default.GetBytes(data)));
            }
        }

        public static Guid? ValidateUser(string name, string password) {
            using (var cmd = DBConnection.GetCommand()) {
                cmd.CommandText = @"SELECT GUID FROM Users WHERE name = @name AND pass_hash = @pass_hash;";
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@pass_hash", GetPasswordHash(password));
                var result = (string)cmd.ExecuteScalar();
                if (result == null) {
                    return null;
                }
                return new Guid(result);
            }
        }

        public static void RegisterUser(string username, string password) {
            using (var cmd = DBConnection.GetCommand()) {
                cmd.CommandText = @"INSERT INTO users(name, pass_hash, GUID) VALUES (@user, @pass_hash, @GUID);";
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass_hash", GetPasswordHash(password).ToString());
                cmd.Parameters.AddWithValue("@GUID", GetGuid(username).ToString());
                try {
                    cmd.ExecuteNonQuery();
                } catch (SQLiteException e) {
                    if (e.ResultCode == SQLiteErrorCode.Constraint) {
                        throw new Exception(message: $"Username: '{username}' already existed");
                    }
                }
            }
        }
    }
}