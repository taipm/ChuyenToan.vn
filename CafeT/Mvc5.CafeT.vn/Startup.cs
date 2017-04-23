using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mvc5.CafeT.vn.Startup))]
namespace Mvc5.CafeT.vn
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
