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
                Functions.Add("/ReservationManagementV2/InitiateClubReservationProcess");
                Functions.Add("/BookmarkManagement/AddBookmarks");
                Functions.Add("/BookmarkManagement/RemoveBookmarks");
                Functions.Add("/LOCATIONMANAGEMENT/CLUBDETAIL_V2");
                if (Functions.Count > 0)
                {
                    var Function = Functions.ConvertAll(x => x.ToUpper());
                    var ActionURL = $"/{ControllerName}/{ActionName}";
                    if (Function.Contains(ActionURL) && httpContext.Session["UserName"] == null)
                    {
                        var RedirectURL = new UriBuilder(HttpContext.Current.Request.Url.Scheme,
                            HttpContext.Current.Request.Url.Host,
                            HttpContext.Current.Request.Url.Port,
                            "/Home/Index");
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            var queryString = HttpContext.Current.Request.Url.Query;
                            if (queryString.StartsWith("?"))
                                queryString = queryString.Substring(1);
                            var dynamicParameters = queryString;
                            var ReturnURL = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
                            if (string.IsNullOrEmpty(ReturnURL) || ReturnURL.Trim() == "/")
                                ReturnURL = "/DashboardV2/Index";

                            ReturnURL += $"?IsRedirectURL=true&FunctionName=InitiateClubReservationFunction&{dynamicParameters}";
                            RedirectURL.Query = $"ReturnURL={HttpUtility.UrlEncode(ReturnURL)}";

                            filterContext.Result = new JsonResult
                            {
                                Data = new
                                {
                                    Code = 999,
                                    RedirectURL = RedirectURL.Uri.ToString()
                                },
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            };
                        }
                        else
                        {
                            var ReturnURL = HttpContext.Current.Request.Url.PathAndQuery;
                            RedirectURL.Query = $"ReturnURL={HttpUtility.UrlEncode(ReturnURL)}";
                            filterContext.Result = new RedirectResult(RedirectURL.Uri.ToString());
                        }
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    HttpContext ctx = HttpContext.Current;
        //    if (ctx.Session["Username"] == null)
        //    {
        //        filterContext.Result = new RedirectToRouteResult(
        //                new RouteValueDictionary {
        //                { "Controller", "Home" },
        //                { "Action", "LogOff" }
        //                });
        //    }
        //    base.OnActionExecuting(filterContext);
        //}
    }
}