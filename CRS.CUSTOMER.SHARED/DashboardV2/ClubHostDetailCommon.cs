namespace CRS.CUSTOMER.SHARED.DashboardV2
{
    public class ClubDetailCommon
    {
        public string ClubId { get; set; }
        public string ClubCode { get; set; }
        public string ClubName { get; set; }
        public string ClubNameJapanese { get; set; }
        public string ClubLogo { get; set; }
        public string ClubLocationId { get; set; }
        public string LocationURL { get; set; }
    }
    public class HostDetailCommon
    {
        public string ClubId { get; set; }
        public string ClubCode { get; set; }
        public string ClubNameEnglish { get; set; }
        public string ClubNameJapanese { get; set; }
        public string ClubLogo { get; set; }
        public string HostId { get; set; }
        public string HostCode { get; set; }
        public string HostNameEnglish { get; set; }
        public string HostNameJapanese { get; set; }
        public string HostLogo { get; set; }
        public string ClubLocationId { get; set; }
        public string IsBookmarked { get; set; }
        public string LocationURL { get; set; }
    }
}
