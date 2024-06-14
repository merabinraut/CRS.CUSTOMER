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

        [HttpGet, Route("faq")]
        public ActionResult faq()
        {
            return View();
        }

        [HttpGet, Route("support")]
        public ActionResult inquery()
        {
            return View();
        }

        [HttpGet, Route("company")]
        public ActionResult operatingcompany()
        {
            return View();
        }

        [HttpGet, Route("policy")]
        public ActionResult privacypolicy()
        {
            return View();
        }

        [HttpGet, Route("rule")]
        public ActionResult termsAndCondition()
        {
            return View();
        }
    }
}