using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Devoir3_Patrick_Dufour.Startup))]
namespace Devoir3_Patrick_Dufour
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
