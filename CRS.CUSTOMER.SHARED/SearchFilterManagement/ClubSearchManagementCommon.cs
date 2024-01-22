using CRS.CUSTOMER.SHARED.LocationManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.SHARED.SearchFilterManagement
{
    public class ClubSearchManagementRequestCommon
    {
        public string AgentId { get; set; }
        public string SearchText { get; set; }
        public string LocationId { get; set; }
        public string ClubCategory { get; set; }
        public string Shift { get; set; }
        public string CustomerAgentId { get; set; }
    }
    public class ClubSearchManagementResponseCommon
    {
        public string ClubId { get; set; }
        public string LocationId { get; set; }
        public string ClubName { get; set; }
        public string ClubNameJapanese { get; set; }
        public string GroupName { get; set; }
        public string FullName { get; set; }
        public string ClubLogo { get; set; }
        public string ClubCoverPhoto { get; set; }
        public string ClubDescription { get; set; }
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
        public string Tag4 { get; set; }
        public string Tag5 { get; set; }
        public string ClubOpeningTime { get; set; }
        public string ClubClosingTime { get; set; }
        public string IsBookmarked { get; set; }
        public int AverageRating { get; set; }
        public int TotalComment { get; set; }
        public string TodaysClubSchedule { get; set; }
        public string ClubTimePeriod { get; set; }
        public List<string> HostGalleryImage { get; set; } = new List<string>();
        public List<ClubWeeklyScheduleCommon> ClubWeeklyScheduleList { get; set; } = new List<ClubWeeklyScheduleCommon>();
    }

    public class ClubRecommendationListCommon
    {
        public string LocationId { get; set; }
        public string ClubId { get; set; }
        public string ClubNameEnglish { get; set; }
        public string ClubNameJapanese { get; set; }
        public string ClubLogo { get; set; }
    }
}
