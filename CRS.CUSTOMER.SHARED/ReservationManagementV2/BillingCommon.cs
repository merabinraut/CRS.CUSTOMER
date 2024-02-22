namespace CRS.CUSTOMER.SHARED.ReservationManagementV2
{
    public class BillingResponseCommon
    {
        public string CustomerUserName { get; set; }
        public string CustomerCostAmount { get; set; }
        public string ReservationRemark { get; set; }
    }

    public class BillingRequestModel
    {
        public string ClubId { get; set; }
        public string CustomerId { get; set; }
        public string PlanId { get; set; }
        public string NoOfPeople { get; set; }
    }
}
