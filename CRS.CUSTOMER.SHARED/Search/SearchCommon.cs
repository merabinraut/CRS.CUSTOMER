using System.Collections.Generic;

namespace CRS.CUSTOMER.SHARED.Search
{
    public class ClubPreferenceFilterRequest
    {
        public string LocationId { get; set; }
        public string SearchFilter { get; set; }
        public string ClubCategory { get; set; }
        public string Price { get; set; }
        public string Shift { get; set; }
        public string Time { get; set; }
        public string ClubAvailability { get; set; }
        public string CustomerId { get; set; }
    }
    public class SearchFilterClubDetailCommon
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
    }

    public class HostPreferenceFilterRequest
    {
        public string LocationId { get; set; }
        public string SearchFilter { get; set; }
        public string Height { get; set; }
        public string Age { get; set; }
        public string BloodType { get; set; }
        public string ConstellationGroup { get; set; }
        public string Occupation { get; set; }
        public string CustomerId { get; set; }
    }

    public class HostPreferenceFilterResponse
    {
        public string ClubId { get; set; }
        public string ClubNameEnglish { get; set; }
        public string ClubNameJapanese { get; set; }
        public string ClubLogo { get; set; }
        public string HostId { get; set; }
        public string HostNameEnglish { get; set; }
        public string HostNameJapanese { get; set; }
        public string HostLogo { get; set; }
        public string ClubLocationId { get; set; }
        public string IsBookmarked { get; set; }
    }
}
