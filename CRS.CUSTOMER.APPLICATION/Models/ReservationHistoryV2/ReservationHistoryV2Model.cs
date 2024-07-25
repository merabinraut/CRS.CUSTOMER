using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.ReservationHistoryV2
{
    public class ReservationCommonModel
    {
        public string rsvtab { get; set; }
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
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string InvoiceId { get; set; }
        public string TransactionStatus { get; set; }
        public string ClubNameEng { get; set; }
        public string ClubNameJp { get; set; }
        public string GroupName { get; set; }
        public string Price { get; set; }
        public string CustomerId { get; set; }
        public string ClubLogo { get; set; }
        public string LocationURL { get; set; }
        public string MobileNumber { get; set; }
        public string NoOfPeople { get; set; }
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
        public string Tag4 { get; set; }
        public string Tag5 { get; set; }

    }
    public class VisitedHistoryModel
    {
        public string ReservationId { get; set; }
        public string ClubId { get; set; }
        public string ReservedDate { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string InvoiceId { get; set; }
        public string TransactionStatus { get; set; }
        public string ClubNameEng { get; set; }
        public string ClubNameJp { get; set; }
        public string GroupName { get; set; }
        public string Price { get; set; }
        public string CustomerId { get; set; }
        public string ClubLogo { get; set; }
        public string LocationURL { get; set; }
        public string MobileNumber { get; set; }
        public string NoOfPeople { get; set; }
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
        public string Tag4 { get; set; }
        public string Tag5 { get; set; }
    }
    public class CancelledHistoryModel
    {
        public string ReservationId { get; set; }
        public string ClubId { get; set; }
        public string ReservedDate { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string InvoiceId { get; set; }
        public string TransactionStatus { get; set; }
        public string ClubNameEng { get; set; }
        public string ClubNameJp { get; set; }
        public string GroupName { get; set; }
        public string Price { get; set; }
        public string CustomerId { get; set; }
        public string ClubLogo { get; set; }
        public string LocationURL { get; set; }
        public string MobileNumber { get; set; }
        public string NoOfPeople { get; set; }
        public string LocationId { get; set; }
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
        public string Tag4 { get; set; }
        public string Tag5 { get; set; }
        public string ClubLocationURL { get; set; }
        public string ClubCode { get; set; }
    }
    public class AllHistoryModel
    {
        public string ReservationId { get; set; }
        public string ClubId { get; set; }
        public string ReservedDate { get; set; }
        public string VisitDate { get; set; }
        public string FormattedVisitDate { get; set; }
        public string VisitTime { get; set; }
        public string InvoiceId { get; set; }
        public string TransactionStatus { get; set; }
        public string ClubNameEng { get; set; }
        public string ClubNameJp { get; set; }
        public string GroupName { get; set; }
        public string Price { get; set; }
        public string CustomerId { get; set; }
        public string ClubLogo { get; set; }
        public string LocationURL { get; set; }
        public string MobileNumber { get; set; }
        public string NoOfPeople { get; set; }
        public string LocationId { get; set; }
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
        public string Tag4 { get; set; }
        public string Tag5 { get; set; }
        public string ClubLocationURL { get; set; }
        public string ClubCode { get; set; }
        public string OTPVerificationStatus { get; set; }
        public string IsManual { get; set; }
        public string ManualRemarkId { get; set; }
    }
    #region " Reservation History Detail Model"
    public class ReservationHistoryDetailModel
    {
        public string ReservationId { get; set; }
        public string ClubId { get; set; }
        public string ReservedDate { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string TransactionStatus { get; set; }
        public string ClubNameEng { get; set; }
        public string ClubNameJp { get; set; }
        public string Price { get; set; }
        public string CustomerId { get; set; }
        public string ClubLogo { get; set; }
        public string HostImages { get; set; }
        public string LocationName { get; set; }
        public string NoOfPeople { get; set; }
        public string PlanTime { get; set; }
        public string Nomination { get; set; }
        public string HostId { get; set; }
        public string[] HostIdList { get; set; }
        public string[] HImages { get; set; }
    }

    #endregion
}

