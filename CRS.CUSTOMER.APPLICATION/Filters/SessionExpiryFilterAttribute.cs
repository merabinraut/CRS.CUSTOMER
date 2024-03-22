using System;
using System.Collections.Generic;
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
            //HttpContext ctx = HttpContext.Current;
            //if (ctx.Session["Username"] == null)
            //{
            //    filterContext.Result = new RedirectToRouteResult(
            //            new RouteValueDictionary {
            //            { "Controller", "Home" },
            //            { "Action", "LogOff" }
            //            });
            //}
            var referringUrl = HttpContext.Current.Request.UrlReferrer;
            var controllerName = string.Empty;
            var actionName = string.Empty;
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            var dataTokens = HttpContext.Current.Request.RequestContext.RouteData.DataTokens;
            if (routeValues != null)
            {
                if (routeValues.ContainsKey("action")) actionName = routeValues["action"].ToString().ToUpper();
                if (routeValues.ContainsKey("controller")) controllerName = routeValues["controller"].ToString().ToUpper();
                var functions = new List<string>();
                functions.Add("/DashboardV2/GetLocationFilterPopUp");
                if (functions.Count > 0)
                {
                    var func = functions.ConvertAll(x => x.ToUpper());
                    var actionURL = $"/{controllerName}/{actionName}";
                    // Get the current URL to use as the return URL
                    var returnUrl = HttpContext.Current.Request.Url.PathAndQuery;

                    // Append the return URL as a query parameter to the redirect URL
                    var redirectUrl = new UriBuilder(HttpContext.Current.Request.Url.Scheme,
                                                     HttpContext.Current.Request.Url.Host,
                                                     HttpContext.Current.Request.Url.Port,
                                                     "/Login/Index");
                    redirectUrl.Query = $"ReturnURL={HttpUtility.UrlEncode(returnUrl)}&TargetURL={referringUrl}";
                    //if (func.Contains(actionURL))
                    //{
                    //    filterContext.Result = new RedirectToRouteResult(
                    //        new RouteValueDictionary {
                    //            { "Controller", "Home" },
                    //            { "Action", "LogOff" }
                    //        });
                    //}
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}