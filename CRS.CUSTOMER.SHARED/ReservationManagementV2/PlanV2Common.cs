namespace CRS.CUSTOMER.SHARED.ReservationManagementV2
{
    public class PlanV2Common
    {
        public ResponseCode Code { get; set; } = ResponseCode.Failed;
        public string Message { get; set; }
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public string PlanTime { get; set; }
        public string PlanPrice { get; set; }
        public string PlanNomination { get; set; }
        public string PlanRemarks { get; set; }
        public string PlanLiquor { get; set; }
    }
}
