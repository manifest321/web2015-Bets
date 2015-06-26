using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BetsTheBest.Startup))]
namespace BetsTheBest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
