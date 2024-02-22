using CRS.CUSTOMER.SHARED;

namespace CRS.CUSTOMER.APPLICATION.Models.ReservationManagementV2
{
    public class ReservationConfirmationRequestModel
    {
        public string ClubId { get; set; }
        public string PlanId { get; set; }
        public string VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string NoOfPeople { get; set; }
        public string PaymentType { get; set; }
    }
}