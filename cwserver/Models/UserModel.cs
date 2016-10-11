using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using Nancy.Security;
using System.Data.SQLite.EF6;
using System.Data.SQLite.EF6.Properties;

namespace cwserver.Models {
    public class UserModel : IUserIdentity {
        public string UserName { get; set; }
        public IEnumerable<string> Claims { get; set; }

        public UserModel(string username, IEnumerable<string> claims) {
            UserName = username;
            Claims = claims;
        }
    }
}