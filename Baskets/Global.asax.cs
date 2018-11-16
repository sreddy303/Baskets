using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Baskets.IoC;
using Castle.Windsor;

namespace Baskets
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IWindsorContainer _container;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            ConfigureInstallers(GlobalConfiguration.Configuration);
        }

        private void ConfigureInstallers(HttpConfiguration config)
        {
            _container = new WindsorContainer();
            _container.Install(new BasketsApiInstaller());
            config.DependencyResolver = new WindsorDependencyResolver(_container.Kernel);
        }
    }
}
