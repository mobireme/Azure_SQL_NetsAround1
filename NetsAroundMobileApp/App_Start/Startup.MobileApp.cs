using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;

//using Owin;
//using Microsoft.Owin.Security;
//using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.OpenIdConnect;

using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
//using Microsoft.Azure.Mobile.Server.Config;
using NetsAroundMobileApp.DataObjects;
using NetsAroundMobileApp.Models;
using System.Threading.Tasks;
using Microsoft.Azure.Mobile.Server.Config;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Globalization;

namespace NetsAroundMobileApp
{
    public partial class Startup
    {

/*        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"]; */

        

        public static void ConfigureMobileApp(IAppBuilder app)
        {
            //string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

            HttpConfiguration config = new HttpConfiguration();

            new Microsoft.Azure.Mobile.Server.Config.MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new MobileServiceInitializer());

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();
            
                        if (string.IsNullOrEmpty(settings.HostName))
                        {
                            app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                            {
                                // This middleware is intended to be used locally for debugging. By default, HostName will
                                // only have a value when running in an App Service application.
                                SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                                ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                                ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                                TokenHandler = config.GetAppServiceTokenHandler()
                            });
                        } 

/*            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,
                    PostLogoutRedirectUri = postLogoutRedirectUri,
                }); */


            app.UseWebApi(config);
        }


    }


    public class MobileServiceInitializer : CreateDatabaseIfNotExists<MobileServiceContext>
    {
        protected override void Seed(MobileServiceContext context)
        {
            base.Seed(context);
            
        }
    }
}

