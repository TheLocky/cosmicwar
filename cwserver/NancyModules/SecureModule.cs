using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace cwserver.NancyModules {
    using Nancy.Security;

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