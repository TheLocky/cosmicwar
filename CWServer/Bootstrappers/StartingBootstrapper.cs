﻿using CWServer.DBConnection;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;

namespace CWServer.Bootstrappers {
    public class StartingBootstrapper : DefaultNancyBootstrapper {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container) {
            // We don't call "base" here to prevent auto-discovery of
            // types/dependencies
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context) {
            base.ConfigureRequestContainer(container, context);

            // Here we register our user mapper as a per-request singleton.
            // As this is now per-request we could inject a request scoped
            // database "context" or other request scoped services.
            container.Register<IUserMapper, UserDatabase>();
        }

        protected override void ConfigureConventions(NancyConventions conventions) {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Res"));
        }

        protected override void RequestStartup(TinyIoCContainer requestContainer, IPipelines pipelines,
            NancyContext context) {
            // At request startup we modify the request pipelines to
            // include forms authentication - passing in our now request
            // scoped user name mapper.
            //
            // The pipelines passed in here are specific to this request,
            // so we can add/remove/update items in them as we please.
            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration {
                    RedirectUrl = "~/login",
                    UserMapper = requestContainer.Resolve<IUserMapper>()
                };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines) {
            DB.Init();
        }
    }
}
