using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SessionExpiryFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //HttpContext ctx = HttpContext.Current;
            //if (ctx.Session["Username"] == null)// If the browser session or authentication session has expired...
            //{
            //    filterContext.Result = new RedirectToRouteResult(
            //            new RouteValueDictionary {
            //            { "Controller", "Home" },
            //            { "Action", "LogOff" }
            //            });
            //}
            var Functions = new List<string>();
            HttpContext httpContext = HttpContext.Current;
            string ControllerName = string.Empty;
            string ActionName = string.Empty;
            var RouteValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            var DataTokens = HttpContext.Current.Request.RequestContext.RouteData.DataTokens;
            if (RouteValues != null)
            {
                if (RouteValues.ContainsKey("action")) ActionName = RouteValues["action"].ToString().ToUpper();
                if (RouteValues.ContainsKey("controller")) ControllerName = RouteValues["controller"].ToString().ToUpper();
                Functions.Add("/NotificationManagement/ViewAllNotifications");
                Functions.Add("/BookmarkManagement/Index");
                if (Functions.Count > 0)
                {
                    var Function = Functions.ConvertAll(x => x.ToUpper());
                    var ActionURL = $"/{ControllerName}/{ActionName}";
                    if (Function.Contains(ActionURL) && httpContext.Session["UserName"] == null)
                    {
                        var ReturnURL = HttpContext.Current.Request.Url.PathAndQuery;
                        var RedirectURL = new UriBuilder(HttpContext.Current.Request.Url.Scheme,
                            HttpContext.Current.Request.Url.Host,
                            HttpContext.Current.Request.Url.Port,
                            "/Home/Index");
                        RedirectURL.Query = $"ReturnURL={HttpUtility.UrlEncode(ReturnURL)}";
                        filterContext.Result = new RedirectResult(RedirectURL.Uri.ToString());
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}