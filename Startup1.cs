
using System;

using System.Threading.Tasks;

using Microsoft.Owin;

using Owin;

using Microsoft.Owin.Security.Cookies;

using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(WebApplication9.Startup1))]

namespace WebApplication9
{

    public class Startup1
    {

        public void Configuration(IAppBuilder app)
        {

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {

                ExpireTimeSpan = TimeSpan.FromMinutes(20),
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,

                LoginPath = new PathString("/Home/Index"),

            });


            //signalR();
            app.MapSignalR();
        }

    }

}