using System.Configuration;
using System.Web.Mvc;
using CRS.CUSTOMER.APPLICATION.Library;
using CRS.CUSTOMER.APPLICATION.Models.ReservationHistoryV2;
using CRS.CUSTOMER.BUSINESS.ReservationManagementV2;

namespace CRS.CUSTOMER.APPLICATION.Controllers
{
    public class ReservationHistoryManagementV2Controller: CustomController
    {
		private readonly IReservationManagementV2Business _buss;
		public ReservationHistoryManagementV2Controller(IReservationManagementV2Business buss)
		{
			_buss = buss;
		}
		public ActionResult ReservationHistory()
		{
			var FileLocationPath = "";
			ReservationCommonModel responseInfo = new ReservationCommonModel();
            var customerId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();

			#region "Reserved History"
            var dbReservedInfo = _buss.GetReservedList(customerId);
			responseInfo.GetReservedList = dbReservedInfo.MapObjects<ReservationHistoryV2Model>();
			foreach(var item in responseInfo.GetReservedList)
			{
				item.ClubId = item.ClubId.EncryptParameter();
				item.ReservationId = item.ReservationId.EncryptParameter();
				item.CustomerId = item.CustomerId.EncryptParameter();
				item.InvoiceId = item.InvoiceId.EncryptParameter();
            }
			#endregion

			#region "Visited History"
			var dbVisitedInfo = _buss.GetVisitedHistoryList(customerId);
			responseInfo.GetVisitedHistoryList = dbVisitedInfo.MapObjects<VisitedHistoryModel>();
			foreach(var visitedItem in responseInfo.GetVisitedHistoryList)
			{
				visitedItem.ClubId = visitedItem.ClubId.EncryptParameter();
				visitedItem.ReservationId = visitedItem.ReservationId.EncryptParameter();
				visitedItem.CustomerId = visitedItem.CustomerId.EncryptParameter();
				visitedItem.InvoiceId = visitedItem.InvoiceId.EncryptParameter();
			}
            #endregion

            #region "Cancelled History"

            #endregion
            if (ConfigurationManager.AppSettings["Phase"]!=null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper()!="DEVELOPMENT") FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();
			responseInfo.GetReservedList.ForEach(x => x.ClubLogo = FileLocationPath + x.ClubLogo);
			responseInfo.GetVisitedHistoryList.ForEach(x => x.ClubLogo = FileLocationPath + x.ClubLogo);
			return View(responseInfo);
		}
	}
}

