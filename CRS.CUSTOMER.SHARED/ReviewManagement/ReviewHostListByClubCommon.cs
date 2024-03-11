namespace CRS.CUSTOMER.SHARED.ReviewManagement
{
    public class ReviewHostListByClubRequestCommon
    {
        public string ClubId { get; set; }
    }
    public class ReviewHostListByClubResponseCommon
    {
        public string ClubId { get; set; }
        public string HostId { get; set; }
        public string HostName { get; set; }
        public string HostNameJapanese { get; set; }
        public string HostRank { get; set; }
        public string HostPosition { get; set; }
        public string HostImage { get; set; }
    }
}
