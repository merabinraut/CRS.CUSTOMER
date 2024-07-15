using System.Collections.Generic;

namespace CRS.CUSTOMER.SHARED.DashboardV2
{
    public class ClubAvailabilityDetailCommon
    {
        public string ClubId { get; set; }
        public string ClubLocationId { get; set; }
        public string ClubNameEnglish { get; set; }
        public string ClubNameJapanese { get; set; }
        public string ClubLogo { get; set; }
        public string IsBookmarked { get; set; }
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
        public string Tag4 { get; set; }
        public string Tag5 { get; set; }
        public string ClubOpeningTime { get; set; }
        public string ClubClosingTime { get; set; }
        public string ClubTimePeriod { get; set; }
        public string TodaysClubSchedule { get; set; }
        public string ClubDescription { get; set; }
        public string GroupName { get; set; }
        public int TotalComment { get; set; }
        public int AverageRating { get; set; }
        public List<string> HostGalleryImage { get; set; } = new List<string>();
        public string ClubCode { get; set; }
        public string LocationURL { get; set; }
        public string TotalClubCount { get; set; }
    }
}
