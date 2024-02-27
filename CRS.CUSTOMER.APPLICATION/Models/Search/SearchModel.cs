using CRS.CUSTOMER.APPLICATION.Models.SearchFilterManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.Search
{
    public class ClubSearchResultModel
    {
        public List<SearchFilterClubDetailModel> FilteredClubModel { get; set; } = new List<SearchFilterClubDetailModel>();
        public List<ClubRecommendationListModel> RecommendedClubModel { get; set; } = new List<ClubRecommendationListModel>();
    }
    public class SearchFilterClubDetailModel
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
        public int TotalComment { get; set; }
        public int AverageRating { get; set; }
        public List<string> HostGalleryImage { get; set; } = new List<string>();
    }
}