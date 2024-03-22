using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public JsonResult Index()
        {
            var response = string.Empty;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Validate()
        {
            return Json(null);
        }
    }
}