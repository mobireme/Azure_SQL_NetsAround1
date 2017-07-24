using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(Azure_SQL_NetsAround1.Startup))]
namespace Azure_SQL_NetsAround1
{
    public partial class Startup {

        public void Configuration(IAppBuilder app) {
            //            ConfigureAuth(app);

        }
    }
}  


