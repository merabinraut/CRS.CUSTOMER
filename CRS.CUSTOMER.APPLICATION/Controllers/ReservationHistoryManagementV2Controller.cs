using System;
using System.Web.Mvc;
using CRS.CUSTOMER.BUSINESS.ReservationManagementV2;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
	public class ReservationHistoryManagementV2Controller: CustomController
    {
		//private readonly IReservationManagementV2Business _buss;
		//public ReservationHistoryManagementV2Controller(IReservationManagementV2Business buss)
		//{
		//	_buss = buss;
		//}
		public ActionResult ReservationHistory()
		{
			return View();
		}
	}
}

