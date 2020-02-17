using Autofac;
using Autofac.Integration.Mvc;
using licenta.Controllers;
using licenta.DatabaseConnection;
using licenta.Helpers;
using licenta.Services;
using System.Configuration;
using System.Web.Mvc;

namespace licenta
{
    public class AutofacRegistration
    {
        public static void BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<CustomEmailService>().As<ICustomEmailService>();

            // Register your MVC controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Now grab your connection string and wire up your db context
            builder.Register(c => new TehnicalDepartmentDb());

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}