using CRS.CUSTOMER.APPLICATION.Filters;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new SessionExpiryFilterAttribute());
            ////filters.Add(new HandleErrorAttribute());
            //filters.Add(new ActivityLogFilter());
        }
    }
}
