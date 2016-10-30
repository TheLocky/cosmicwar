using System;
using CWServer.DBConnection;
using Nancy;

namespace CWServer.NancyModules {
    public class RegistrationModule : NancyModule {
        public RegistrationModule() {
            Get["/register"] = args => View["register.cshtml",
                new {errorMessage = (string) null}];

            Post["/register"] = args => {
                string username = Request.Form.Username;
                string password = Request.Form.Password;

                try {
                    UserDatabase.RegisterUser(username, password);
                }
                catch (Exception e) {
                    return View["register.cshtml",
                        new {errorMessage = e.Message}];
                }

                return View["success_register.cshtml"];
            };
        }
    }
}
