using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication9.Controllers
{
    public class LogFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        controller = "Home",
                        action = "Square",
                        UserName = filterContext.HttpContext.User.Identity.Name
                    }
                ));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}