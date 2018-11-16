using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Baskets.Repository;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Baskets.IoC
{
    public class BasketsApiInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly().BasedOn<ApiController>().LifestyleScoped()
                ,Component.For<IBasketRepository>().ImplementedBy<BasketRepository>().LifestyleSingleton());
        }
    }
}