namespace CRS.CUSTOMER.SHARED.ReservationManagementV2
{
    public class ReservationConfirmationRequestCommon : Common
    {
        public string CustomerId { get; set; }
        public string ClubId { get; set; }
        public string PlanId { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string NoOfPeople { get; set; }
        public string PaymentType { get; set; }
        public string HostIdList { get; set; }
    }
}
