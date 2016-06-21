using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication9
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // Code that runs on application startup
            Application["OnlineUsers"] = 0;

        }
        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
          
        }
        void Session_End(object sender, EventArgs e)
        {
        //    Session[User.Identity.Name] = "false"; 
        }

        void Application_PostAuthenticateRequest()
        {
            if (User.Identity.IsAuthenticated)
            {
                var name = User.Identity.Name; // Get current user name.

                SSDDatingEntities21 context = new SSDDatingEntities21();
                var user = context.AspNetUsers.Where(u => u.UserName == name).FirstOrDefault();
                IQueryable<string> roleQuery = from r in context.AspNetUserRoles
                                               where r.UserId== user.Id
                                               select r.AspNetRole.Name;

                string[] roles = roleQuery.ToArray();

                HttpContext.Current.User = Thread.CurrentPrincipal =
                                           new GenericPrincipal(User.Identity, roles);
            }
        }

    }
}
