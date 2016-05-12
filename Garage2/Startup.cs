using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Garage2.Startup))]
namespace Garage2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
