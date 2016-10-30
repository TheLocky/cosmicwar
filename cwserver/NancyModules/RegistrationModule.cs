using CWServer.DBConnection;
using CWServer.Exceptions;
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
                    return View["success_register.cshtml"];
                }
                catch (CWException e) {
                    return View["register.cshtml", new {Exception = e}];
                }
            };
        }
    }
}
