using System.Web.Mvc;
using System.Web.Routing;

namespace CRS.CUSTOMER.APPLICATION
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes(); // Enable Attribute Routing

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "DashboardV2", action = "Index", id = UrlParameter.Optional }
                //defaults: new { controller = "Home", action = "HomePage", id = UrlParameter.Optional }
            );
        }
    }
}
