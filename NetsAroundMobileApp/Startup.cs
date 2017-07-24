using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(NetsAroundMobileApp.Startup))]

namespace NetsAroundMobileApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            ConfigureMobileApp(app);
            //ConfigureAuth(app);
        }
    }
}