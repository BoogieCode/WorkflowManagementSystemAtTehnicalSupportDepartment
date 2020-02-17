using licenta.Helpers;
using licenta.Migrations;
using licenta.Services;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(licenta.Startup))]
namespace licenta
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
