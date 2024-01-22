namespace CRS.CUSTOMER.SHARED.ReviewManagement
{
    public class ManageReviewRequestCommon : Common
    {
        public string CustomerId { get; set; }
        public string ReservationId { get; set; }
        public string ClubId { get; set; }
        public string HostList { get; set; }
        public string MVPHostId { get; set; }
        public string DichotomousList { get; set; }
        public string RemarkList { get; set; }
        public int RatingScale { get; set; }
    }
}
