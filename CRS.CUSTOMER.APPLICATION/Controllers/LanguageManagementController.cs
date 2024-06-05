using CRS.CUSTOMER.APPLICATION.Library;
using System;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class LanguageManagementController : CustomController
    {
        [HttpGet, Route("LanguageManagement/Index")]
        public ActionResult Index()
        {
            ViewBag.ActionPageName = "NavMenu";
            ViewBag.PageTitle = Resources.Resource.Langauge;
            return View();
        }

        [HttpPost, Route("LanguageManagement/Index")]
        public JsonResult Index(string lang)
        {
            try
            {
                ViewBag.ActionPageName = "NavMenu";
                ViewBag.PageTitle = Resources.Resource.Langauge;
                new LanguageMang().SetLanguage(lang);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }
    }
}