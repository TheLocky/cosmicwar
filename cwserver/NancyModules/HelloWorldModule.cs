using System;
using System.Dynamic;
using cwserver.DBConnection;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Extensions;

namespace cwserver.NancyModules {
    public class HelloWorldModule : NancyModule {
        public HelloWorldModule() {
            StaticConfiguration.DisableErrorTraces = false;
            Get["/"] = args => View["index"];

            Get["/login"] = args => {
                dynamic model = new ExpandoObject();
                model.Errored = Request.Query.error.HasValue;

                return View["login.cshtml", model];
            };

            Post["/login"] = args => {
                var userGuid = UserDatabase.ValidateUser((string) Request.Form.Username, (string) Request.Form.Password);

                if (userGuid == null)
                    return Context.GetRedirect("~/login?error=true&username=" + (string) Request.Form.Username);

                DateTime? expiry = null;
                if (Request.Form.RememberMe.HasValue) expiry = DateTime.Now.AddDays(7);

                return this.LoginAndRedirect(userGuid.Value, expiry);
            };

            Get["/logout"] = args => this.LogoutAndRedirect("~/");

            Get["/register"] = args => View["register.cshtml", new {register = true}];

            Post["/register"] = args => {
                string username = Request.Form.Username;
                string password = Request.Form.Password;

                if ((username == null) || (password == null))
                    return
                        View[
                            "register.cshtml", new {register = true, message = "Please type your username and password"}
                        ];

                var errorMessage = "Register succesfull";

                try {
                    UserDatabase.RegisterUser(username, password);
                }
                catch (Exception e) {
                    errorMessage = e.Message;
                }

                return View["register.cshtml", new {register = false, message = errorMessage}];
            };
        }
    }
}