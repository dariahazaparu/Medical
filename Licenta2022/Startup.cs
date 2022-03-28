using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Licenta2022.Startup))]
namespace Licenta2022
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
