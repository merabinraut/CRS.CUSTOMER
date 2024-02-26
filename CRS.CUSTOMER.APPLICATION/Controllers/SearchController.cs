using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class SearchController : Controller
    {
        [HttpGet]
        public ActionResult ClubSearchResult()
        {
            ViewBag.ActionPageName = "SearchFilter";
            return View();
        }

        [HttpGet]
        public ActionResult HostSearchResult()
        {
            ViewBag.ActionPageName = "SearchFilter";
            return View();
        }
    }
}