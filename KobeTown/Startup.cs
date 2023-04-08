using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KobeTown.Startup))]
namespace KobeTown
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
