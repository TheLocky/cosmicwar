using System;
using CWServer.DBConnection;
using Nancy;
using Nancy.Authentication.Forms;

namespace CWServer.NancyModules {
    public class AuthorizationModule : NancyModule {
        public AuthorizationModule() {
            Get["/login"] = args => View["login.cshtml",
                new {errorMessage = (string) null}];

            Post["/login"] = args => {
                string username = Request.Form.Username;
                string password = Request.Form.Password;

                var userGuid = UserDatabase.ValidateUser(username, password);

                if (userGuid == null)
                    return View["login.cshtml",
                        new {errorMessage = "Invalid username or password"}];

                DateTime? expiry = null;
                if (Request.Form.RememberMe.HasValue) expiry = DateTime.Now.AddDays(7);

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };

            Get["/logout"] = args => this.LogoutAndRedirect("~/");
        }
    }
}
