using System;
using cwserver.DBConnection;
using Nancy;

namespace cwserver.NancyModules {
    public class RegistrationModule : NancyModule {
        public RegistrationModule() {
            Get["/register"] = args => View["register.cshtml",
                new { register = true }];

            Post["/register"] = args => {
                string username = Request.Form.Username;
                string password = Request.Form.Password;

                try {
                    UserDatabase.RegisterUser(username, password);
                }
                catch (Exception e) {
                    return View["register.cshtml", new {
                        register = false,
                        message = e.Message
                    }];
                }

                return View["register.cshtml", new {
                    register = false,
                    message = "Register succesfull"
                }];
            };
        }
    }
}
