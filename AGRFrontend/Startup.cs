using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AGRFrontend.Startup))]
namespace AGRFrontend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
