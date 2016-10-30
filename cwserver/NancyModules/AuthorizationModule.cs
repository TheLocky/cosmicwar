using System;
using CWServer.DBConnection;
using CWServer.Exceptions;
using Nancy;
using Nancy.Authentication.Forms;

namespace CWServer.NancyModules {
    public class AuthorizationModule : NancyModule {
        public AuthorizationModule() {
            Get["/login"] = args => View["login.cshtml"];

            Post["/login"] = args => {
                string username = Request.Form.Username;
                string password = Request.Form.Password;

                try {
                    var userGuid = UserDatabase.ValidateUser(username, password);
                    DateTime? expiry = null;
                    if (Request.Form.RememberMe.HasValue) expiry = DateTime.Now.AddDays(7);
                    return this.LoginAndRedirect(userGuid, expiry);
                }
                catch (CWException e) {
                    return View["login.cshtml", new {Exception = e}];
                }
            };

            Get["/logout"] = args => this.LogoutAndRedirect("~/");
        }
    }
}
