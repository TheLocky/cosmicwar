using Nancy;
using Nancy.Security;

namespace CWServer.NancyModules {
    public class SecureModule : NancyModule {
        public SecureModule() : base("/secure") {
            this.RequiresAuthentication();

            Get["/"] = args => {
                var model = Context.CurrentUser;
                return View["secure.cshtml", model];
            };
        }
    }
}
