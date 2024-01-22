using CRS.CUSTOMER.APPLICATION.Models.LocationManagement;
using CRS.CUSTOMER.APPLICATION.Models.ReservationHistory;
using CRS.CUSTOMER.APPLICATION.Models.UserProfileManagement;
using CRS.CUSTOMER.SHARED.ReservationHistoryManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.ReservationManagement
{
    public class CreateReservationDetailModel
    {
        public string CustomerId { get; set; }
        public string ClubId { get; set; }
        public string PlanId { get; set; }
        public string HostIdList { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string NoOfPeople { get; set; }
    }

    public class ManageReservationDetailModel
    {
        public string CustomerId { get; set; }
        public string ReservationId { get; set; }
        public string PaymentType { get; set; }
    }

    public class ReservationHistoryModel
    {
        public string ReservationId { get; set; }
        public string CustomerId { get; set; }
        public string ClubId { get; set; }
        public string InvoiceId { get; set; }
        public string ReservedDate { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string ClubName { get; set; }
        public string ClubNameJapanese { get; set; }
        public string GroupName { get; set; }
        public string Dated { get; set; }
        public string ClubLogo { get; set; }
        public string TransactionStatus { get; set; }
        public string LocationURL { get; set; }
    }

    public class ReservationDetailModel
    {
        public string PlanId { get; set; }
        public string ClubId { get; set; }
        public List<LocationHostListModel> HostListModel { get; set; }
        public LocationClubListModel ClubDetailModel { get; set; }
        public UserProfileModel CustomerDetailModel { get; set; }
        public List<ReservationHostDetailModel> ReservationHostDetailModel { get; set; }
    }
    public class ReservationConfirmationModel
    {
        public string ClubId { get; set; }
        public string PlanId { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string NoOfPeople { get; set; }
    }

    public class PaymentMethodDetailModel
    {
        public string ReservationId { get; set; }
        public string CustomerDetail { get; set; }
        public string ReservationDetail { get; set; }
        public string PlannDetail { get; set; }
        public string TotalAmount { get; set; }
    }
}