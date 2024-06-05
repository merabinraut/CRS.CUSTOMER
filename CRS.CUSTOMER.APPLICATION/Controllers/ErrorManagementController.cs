using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    [OverrideActionFilters]
    public class ErrorManagementController : CustomController
    {
        [HttpGet, Route("ErrorManagement/Index")]
        public ActionResult Index(string Id = "")
        {
            if (string.IsNullOrEmpty(Id)) return RedirectToAction("LogOff", "Home");
            ViewBag.ErrorId = Id;
            return View();
        }

        [HttpGet, Route("ErrorManagement/Error_403")]
        public ActionResult Error_403()
        {
            return View();
        }

        [HttpGet, Route("ErrorManagement/Error_404")]
        public ActionResult Error_404()
        {
            return View();
        }
    }
}