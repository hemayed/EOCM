using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EOCM.Startup))]
namespace EOCM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
