﻿using System;
namespace CRS.CUSTOMER.SHARED.ReservationHistoryManagementV2
{
    public class ReservationHistoryV2ModelCommon
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
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
        public string Tag4 { get; set; }
        public string Tag5 { get; set; }
    }
    public class VisitedHistoryModelCommon
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
    public class CancelledHistoryModelCommon
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
    public class AllHistoryModelCommon
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
    public class ReservationHistoryDetailModelCommon
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
    }

    #endregion
}

