using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    [OverrideActionFilters]
    public class ErrorManagementController : CustomController
    {
        public ActionResult Index(string Id = "")
        {
            if (string.IsNullOrEmpty(Id)) return RedirectToAction("DashboardV2", "Home");
            ViewBag.ErrorId = Id;
            return View();
        }
        public ActionResult Error_403()
        {
            return View();
        }
        public ActionResult Error_404()
        {
            return View();
        }
    }
}