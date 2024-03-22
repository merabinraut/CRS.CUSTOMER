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
            if (ctx.Session["Username"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                        { "Controller", "Home" },
                        { "Action", "LogOff" }
                        });
            }
            //var controllerName = string.Empty;
            //var actionName = string.Empty;
            //var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            //var dataTokens = HttpContext.Current.Request.RequestContext.RouteData.DataTokens;
            //if (routeValues != null)
            //{
            //    if (routeValues.ContainsKey("action")) actionName = routeValues["action"].ToString().ToUpper();
            //    if (routeValues.ContainsKey("controller")) controllerName = routeValues["controller"].ToString().ToUpper();
            //    var functions = new List<string>();
            //    if (functions.Count > 0)
            //    {
            //        var func = functions.ConvertAll(x => x.ToUpper());
            //        var actionURL = $"/{controllerName}/{actionName}";
            //        if (func.Contains(actionURL) == true && func.Equals(actionURL) == true)
            //        {
            //            filterContext.Result = new RedirectToRouteResult(
            //                new RouteValueDictionary
            //                {
            //                    { "Controller", ""},
            //                    { "Action", ""}
            //                });
            //        }
            //    }
            //}
            base.OnActionExecuting(filterContext);
        }
    }
}