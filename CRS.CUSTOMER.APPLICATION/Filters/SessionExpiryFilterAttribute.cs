using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CRS.CUSTOMER.APPLICATION.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SessionExpiryFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            // If the browser session or authentication session has expired...
            if (ctx.Session["Username"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                        { "Controller", "Home" },
                        { "Action", "LogOff" }
                        });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}