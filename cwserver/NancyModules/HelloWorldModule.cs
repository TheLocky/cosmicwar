using Nancy;
using Nancy.Security;

namespace CWServer.NancyModules {
    public class HelloWorldModule : NancyModule {
        public HelloWorldModule() {
            this.RequiresAuthentication();

            Get["/"] = args => View["index", new {user = Context.CurrentUser}];
        }
    }
}
