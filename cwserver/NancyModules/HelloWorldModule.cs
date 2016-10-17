using Nancy;
using Nancy.Security;

namespace cwserver.NancyModules {
    public class HelloWorldModule : NancyModule {
        public HelloWorldModule() {
            Get["/"] = args => View["index"];
        }
    }
}
