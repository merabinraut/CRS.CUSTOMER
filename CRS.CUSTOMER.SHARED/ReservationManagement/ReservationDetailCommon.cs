namespace CRS.CUSTOMER.SHARED.ReservationManagement
{
    public class CreateReservationDetailCommon : Common
    {
        public string CustomerId { get; set; }
        public string ClubId { get; set; }
        public string PlanId { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string NoOfPeople { get; set; }
        public string HostIdList { get; set; }
    }

    public class ManageReservationDetailCommon : Common
    {
        public string CustomerId { get; set; }
        public string ReservationId { get; set; }
        public string PaymentType { get; set; }
    }

    public class ReservationDetailCommon
    {
        public string ReservationId { get; set; }
        public string InvoiceId { get; set; }
        public string CustomerId { get; set; }
        public string ClubId { get; set; }
        public string PlanId { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string NoOfPeople { get; set; }
        public string ReservedDate { get; set; }
        public string PlanDetailId { get; set; }
        public string HostDetailId { get; set; }
        public string PaymentType { get; set; }
        public string TransactionStatus { get; set; }
    }

    public class ReservationHistoryCommon
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
}
