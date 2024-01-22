using System.Web.Mvc;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
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