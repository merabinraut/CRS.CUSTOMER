namespace CRS.CUSTOMER.SHARED.SearchFilterManagement
{
    public class HostSearchManagementRequestCommon
    {
        public string AgentId { get; set; }
        public string LocationId { get; set; }
        public string SearchText { get; set; }
        public string Rank { get; set; }
        public string Height { get; set; }
        public string BloodType { get; set; }
        public string ZodiacGroup { get; set; }
        public string LiquorStrength { get; set; }
        public string PreviousOccupation { get; set; }
        public string Handsomeness { get; set; }
        public string AgeRange { get; set; }
    }
    public class HostSearchManagementResponseCommon
    {
        public string LocationId { get; set; }
        public string ClubId { get; set; }
        public string HostId { get; set; }
        public string HostName { get; set; } 
        public string HostNameJapanese { get; set; }
        public string HostImage { get; set; }
        public string Occupation { get; set; }
        public string Rank { get; set; }
        public string ClubNameEnglish { get; set; }
        public string ClubNameJapanese { get; set; }
        public string ClubLogo { get; set; }
        public string IsBookmarked { get; set; }
    }

    public class HostRecommendationListCommon
    {
        public string LocationId { get; set; }
        public string ClubId { get; set; }
        public string HostId { get; set; }
        public string HostName { get; set; }
        public string HostImage { get; set; }
        public string ClubNameEnglish { get; set; }
        public string ClubNameJapanese { get; set; }
        public string ClubLogo { get; set; }
    }
}