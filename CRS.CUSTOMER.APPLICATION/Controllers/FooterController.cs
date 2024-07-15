using CRS.CUSTOMER.BUSINESS.CommonManagement;
using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    [OverrideActionFilters]
    public class FooterController : Controller
    {
        private readonly ICommonManagementBusiness _commonBusiness;
        public FooterController(ICommonManagementBusiness commonBusiness)
        {
            _commonBusiness = commonBusiness;
        }
        private void PopulateMetaTagInfo()
        {
            var metaTagDBResponse = _commonBusiness.GetMetaTagInfo("1");
            ViewBag.MetaClubCount = metaTagDBResponse.Item1;
            ViewBag.MetaHostCount = metaTagDBResponse.Item2;
        }

        [HttpGet, Route("customervoice")]
        public ActionResult customervoice()
        {
            PopulateMetaTagInfo();
            return View();
        }

        [HttpGet, Route("faq")]
        public ActionResult faq()
        {
            PopulateMetaTagInfo();
            return View();
        }

        [HttpGet, Route("support")]
        public ActionResult inquery()
        {
            PopulateMetaTagInfo();
            return View();
        }

        [HttpGet, Route("company")]
        public ActionResult operatingcompany()
        {
            PopulateMetaTagInfo();
            return View();
        }

        [HttpGet, Route("policy")]
        public ActionResult privacypolicy()
        {
            PopulateMetaTagInfo();
            return View();
        }

        [HttpGet, Route("rule")]
        public ActionResult termsAndCondition()
        {
            PopulateMetaTagInfo();
            return View();
        }
    }
}