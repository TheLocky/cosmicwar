using System;
using cwserver.DBConnection;
using Nancy;
using Nancy.Authentication.Forms;

namespace cwserver.NancyModules {
    public class AuthorizationModule : NancyModule {
        public AuthorizationModule() {
            Get["/login"] = args => View["login.cshtml", 
                    new { Errored = false }];

            Post["/login"] = args => {
                string username = Request.Form.Username;
                string password = Request.Form.Password;

                var userGuid = UserDatabase.ValidateUser(username, password);

                if (userGuid == null) {
                    return View["login.cshtml",
                        new { Errored = true }];
                }

                DateTime? expiry = null;
                if (Request.Form.RememberMe.HasValue) {
                    expiry = DateTime.Now.AddDays(7);
                }

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };

            Get["/logout"] = args => this.LogoutAndRedirect("~/");
        }
    }
}
