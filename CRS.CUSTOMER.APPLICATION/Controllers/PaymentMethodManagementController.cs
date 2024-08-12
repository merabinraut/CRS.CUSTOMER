using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class PaymentMethodManagementController : CustomController
    {
        public ActionResult Index()
        {
            ViewBag.ActionPageName = "NavMenu";
            ViewBag.PageTitle = Resources.Resource.AllPaymentMethods;
            return View();
        }
        public ActionResult AddCreditCard()
        {
            ViewBag.ActionPageName = "NavMenu";
            ViewBag.PageTitle = Resources.Resource.AllPaymentMethods;
            return View();
        }
    }
}