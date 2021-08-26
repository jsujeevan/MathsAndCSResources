using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MShed_Web.Startup))]
namespace MShed_Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
