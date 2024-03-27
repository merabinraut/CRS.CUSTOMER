using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.BUSINESS.CommonManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SessionExpiryFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Functions = new List<Privileges>();
            HttpContext httpContext = HttpContext.Current;
            string ControllerName = string.Empty;
            string ActionName = string.Empty;
            var RouteValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            var DataTokens = HttpContext.Current.Request.RequestContext.RouteData.DataTokens;
            if (RouteValues != null)
            {
                Functions = httpContext.Session["Functions"] as List<Privileges>;
                if (Functions == null || Functions.Count <= 0)
                {
                    var CommonBusiness = new CommonManagementBusiness();
                    var GetCustomerPrivileges = CommonBusiness.GetCustomerPrivileges();
                    Functions = GetCustomerPrivileges.MapObjects<Privileges>();
                    httpContext.Session["Functions"] = Functions;
                }
                if (RouteValues.ContainsKey("action")) ActionName = RouteValues["action"].ToString().ToUpper();
                if (RouteValues.ContainsKey("controller")) ControllerName = RouteValues["controller"].ToString().ToUpper();
                if (Functions.Count > 0)
                {
                    Functions.ForEach(x => x.FunctionURL = x.FunctionURL.ToUpper());
                    var ActionURL = $"/{ControllerName}/{ActionName}";
                    if (Functions.Any(x => x.FunctionURL.Contains(ActionURL)) && httpContext.Session["UserName"] == null)
                    {
                        var FunctionName = Functions.FirstOrDefault(x => x.FunctionURL == ActionURL);
                        var RedirectURL = new UriBuilder(HttpContext.Current.Request.Url.Scheme,
                            HttpContext.Current.Request.Url.Host,
                            HttpContext.Current.Request.Url.Port,
                            "/Home/Index");
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            var queryString = string.Empty;
                            if (HttpContext.Current.Request.RequestType == "POST")
                                queryString = HttpContext.Current.Request.Form.ToString();
                            else
                                queryString = HttpContext.Current.Request.Url.Query;
                            if (queryString.StartsWith("?"))
                                queryString = queryString.Substring(1);
                            var dynamicParameters = queryString;
                            var ReturnURL = HttpContext.Current.Request.UrlReferrer?.PathAndQuery;
                            if (string.IsNullOrEmpty(ReturnURL) || ReturnURL.Trim() == "/")
                                ReturnURL = "/DashboardV2/Index";
                            if (!string.IsNullOrEmpty(FunctionName.AdditionalValue))
                            {
                                var separator = ReturnURL.Contains("?") ? "&" : "?";
                                ReturnURL += $"{separator}IsRedirectURL=true&FunctionName={FunctionName.AdditionalValue}&{dynamicParameters}";
                            }
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
        private class Privileges
        {
            public string FunctionURL { get; set; }
            public string AdditionalValue { get; set; }
        }
    }
}