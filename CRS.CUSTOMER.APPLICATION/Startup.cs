using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CRS.CUSTOMER.APPLICATION.Startup))]
namespace CRS.CUSTOMER.APPLICATION
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Map SignalR
            app.MapSignalR();
        }
    }
}