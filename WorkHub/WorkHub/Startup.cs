using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WorkHub.Startup))]
namespace WorkHub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
