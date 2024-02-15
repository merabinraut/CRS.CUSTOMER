using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.ReservationHistoryV2
{
    public class ReservationCommonModel
	{
		public List<ReservationHistoryV2Model> GetReservedList { get; set; }
		public List<VisitedHistoryModel> GetVisitedHistoryList { get; set; }
		public List<CancelledHistoryModel> GetCancelledHistoryList { get; set; }
		public List<AllHistoryModel> GetAllHistoryList { get; set; }
	}
	public class ReservationHistoryV2Model
	{
		public string ReservationId { get; set; }
		public string ClubId { get; set; }
		public string ReservedDate { get; set; }
		public string VisitedDate { get; set; }
		public string VisitTime { get; set; }
		public string InvoiceId { get; set; }
		public string TransactionStatus { get; set; }
		public string ClubNameEng { get; set; }
		public string ClubNameJp { get; set; }
		public string Price { get; set; }
		public string CustomerId { get; set; }
		public string ClubLogo { get; set; }
		public string LocationURL { get; set; }

	}
	public class VisitedHistoryModel
	{
        public string ReservationId { get; set; }
        public string ClubId { get; set; }
        public string ReservedDate { get; set; }
        public string VisitedDate { get; set; }
        public string VisitTime { get; set; }
        public string InvoiceId { get; set; }
        public string TransactionStatus { get; set; }
        public string ClubNameEng { get; set; }
        public string ClubNameJp { get; set; }
        public string Price { get; set; }
        public string CustomerId { get; set; }
        public string ClubLogo { get; set; }
        public string LocationURL { get; set; }
    }
    public class CancelledHistoryModel
    {
        public string ReservationId { get; set; }
        public string ClubId { get; set; }
        public string ReservedDate { get; set; }
        public string VisitedDate { get; set; }
        public string VisitTime { get; set; }
        public string InvoiceId { get; set; }
        public string TransactionStatus { get; set; }
        public string ClubNameEng { get; set; }
        public string ClubNameJp { get; set; }
        public string Price { get; set; }
        public string CustomerId { get; set; }
        public string ClubLogo { get; set; }
        public string LocationURL { get; set; }
    }
    public class AllHistoryModel
    {
        public string ReservationId { get; set; }
        public string ClubId { get; set; }
        public string ReservedDate { get; set; }
        public string VisitedDate { get; set; }
        public string VisitTime { get; set; }
        public string InvoiceId { get; set; }
        public string TransactionStatus { get; set; }
        public string ClubNameEng { get; set; }
        public string ClubNameJp { get; set; }
        public string Price { get; set; }
        public string CustomerId { get; set; }
        public string ClubLogo { get; set; }
        public string LocationURL { get; set; }
    }
    #region " Reservation History Detail Model"
    public class ReservationHistoryDetailModel
    {
        public string ReservationId { get; set; }
        public string ClubId { get; set; }
        public string ReservedDate { get; set; }
        public string VisitedDate { get; set; }
        public string VisitTime { get; set; }
        public string TransactionStatus { get; set; }
        public string ClubNameEng { get; set; }
        public string ClubNameJp { get; set; }
        public string Price { get; set; }
        public string CustomerId { get; set; }
        public string ClubLogo { get; set; }
    }

    #endregion
}

