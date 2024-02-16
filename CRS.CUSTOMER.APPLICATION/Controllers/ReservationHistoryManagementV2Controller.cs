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
			var dbCancelledInfo = _buss.GetCancelledHistory(customerId);
			responseInfo.GetCancelledHistoryList = dbCancelledInfo.MapObjects<CancelledHistoryModel>();
			foreach(var cancelItem in responseInfo.GetCancelledHistoryList)
			{
				cancelItem.ClubId = cancelItem.ClubId.EncryptParameter();
				cancelItem.ReservationId = cancelItem.ReservationId.EncryptParameter();
				cancelItem.CustomerId = cancelItem.CustomerId.EncryptParameter();
				cancelItem.InvoiceId = cancelItem.InvoiceId.EncryptParameter();
			}
			#endregion

			#region "All History"
			var dbAllInfo = _buss.GetAllHistoryList(customerId);
			responseInfo.GetAllHistoryList = dbAllInfo.MapObjects<AllHistoryModel>();
			foreach(var allItem in responseInfo.GetAllHistoryList)
			{
				allItem.ClubId = allItem.ClubId.EncryptParameter();
				allItem.ReservationId = allItem.ReservationId.EncryptParameter();
				allItem.CustomerId = allItem.CustomerId.EncryptParameter();
				allItem.InvoiceId = allItem.InvoiceId.EncryptParameter();
			}
			#endregion
			if (ConfigurationManager.AppSettings["Phase"]!=null && ConfigurationManager.AppSettings["Phase"].ToString().ToUpper()!="DEVELOPMENT") FileLocationPath = ConfigurationManager.AppSettings["ImageVirtualPath"].ToString();
			responseInfo.GetReservedList.ForEach(x => x.ClubLogo = FileLocationPath + x.ClubLogo);
			responseInfo.GetVisitedHistoryList.ForEach(x => x.ClubLogo = FileLocationPath + x.ClubLogo);
			responseInfo.GetCancelledHistoryList.ForEach(x => x.ClubLogo = FileLocationPath + x.ClubLogo);
			responseInfo.GetAllHistoryList.ForEach(x => x.ClubLogo = FileLocationPath + x.ClubLogo);
			return View(responseInfo);
		}
		public ActionResult ViewHistoryDetail(string ReservationId="")
		{
			string CustomerId = ApplicationUtilities.GetSessionValue("AgentId").ToString().DecryptParameter();
			string reservationId = "";
			if (!string.IsNullOrEmpty(ReservationId)) reservationId = ReservationId.DecryptParameter();
			ReservationHistoryDetailModel responseinfo = new ReservationHistoryDetailModel();
			var dbResponseInfo = _buss.GetReservationHistoryDetail(CustomerId, reservationId);
			responseinfo = dbResponseInfo.MapObject<ReservationHistoryDetailModel>();
			return View(responseinfo);
		}
		public ActionResult RescheduleReservation()
		{
			return View();
		}
	}
}

