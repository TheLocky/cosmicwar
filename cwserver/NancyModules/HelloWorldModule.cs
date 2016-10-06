using Nancy;

namespace cwserver.NancyModules
{
    public class HelloWorldModule : NancyModule
    {
        public HelloWorldModule()
        {
            Get["/"] = x => "<p>Hello World</p>";
        }
    }
}