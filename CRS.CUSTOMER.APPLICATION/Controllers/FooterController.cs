using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    [OverrideActionFilters]
    public class FooterController : Controller
    {
        public ActionResult customervoice()
        {
            return View();
        }
        public ActionResult faq()
        {
            return View();
        }
        public ActionResult inquery()
        {
            return View();
        }
        public ActionResult operatingcompany()
        {
            return View();
        }
        public ActionResult privacypolicy()
        {
            return View();
        }
        public ActionResult termsAndCondition()
        {
            return View();
        }
    }
}