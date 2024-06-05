using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    [OverrideActionFilters]
    public class FooterController : Controller
    {
        [HttpGet, Route("Footer/customervoice")]
        public ActionResult customervoice()
        {
            return View();
        }

        [HttpGet, Route("Footer/faq")]
        public ActionResult faq()
        {
            return View();
        }

        [HttpGet, Route("Footer/inquery")]
        public ActionResult inquery()
        {
            return View();
        }

        [HttpGet, Route("Footer/operatingcompany")]
        public ActionResult operatingcompany()
        {
            return View();
        }

        [HttpGet, Route("Footer/privacypolicy")]
        public ActionResult privacypolicy()
        {
            return View();
        }

        [HttpGet, Route("Footer/termsAndCondition")]
        public ActionResult termsAndCondition()
        {
            return View();
        }
    }
}