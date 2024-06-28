using CRS.CUSTOMER.APPLICATION.Models.CommonModel;
using CRS.CUSTOMER.APPLICATION.Models.DashboardV2;
using CRS.CUSTOMER.APPLICATION.Models.Search;
using CRS.CUSTOMER.APPLICATION.Models.SearchFilterManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.SearchV2
{
    public class SearchV2FilterRequestModel
    {
        public SearchV2FilterClubTabModel SearchV2FilterClubTabModel { get; set; } = new SearchV2FilterClubTabModel();
        public SearchV2FilterHostTabModel SearchV2FilterHostTabModel { get; set; } = new SearchV2FilterHostTabModel();
        public SearchV2FilterNewTabModel SearchV2FilterNewTabModel { get; set; } = new SearchV2FilterNewTabModel();
        public SearchV2FilterMapTabModel SearchV2FilterMapTabModel { get; set; } = new SearchV2FilterMapTabModel();
    }

    #region CLUB TAB 
    public class SearchV2FilterClubTabModel
    {
        public List<StaticDataModel> LocationModel { get; set; } = new List<StaticDataModel>();
        public List<StaticDataModel> ClubCategoryModel { get; set; } = new List<StaticDataModel>();
        public List<StaticDataModel> PlanPriceModel { get; set; } = new List<StaticDataModel>();
        public List<StaticDataModel> ClubAvailabilityModel { get; set; } = new List<StaticDataModel>();
    }

    public class SearchV2FilterClubTabRequestModel
    {
        public string SearchFilter { get; set; }
        public string LocationId { get; set; }
        public string ClubCategory { get; set; }
        public string Price { get; set; }
        public string Shift { get; set; }
        public string Time { get; set; }
        public string ClubAvailability { get; set; }
    }

    public class SearchV2FilterClubTabResponseModel
    {
        public List<SearchFilterClubDetailModel> FilteredClubModel { get; set; } = new List<SearchFilterClubDetailModel>();
        public List<ClubRecommendationListModel> RecommendedClubModel { get; set; } = new List<ClubRecommendationListModel>();
    }

    public class SearchFilterClubDetailModel
    {
        public string ClubId { get; set; }
        public string ClubCode { get; set; }
        public string LocationURL { get; set; }
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
    #endregion

    #region HOST TAB
    public class SearchV2FilterHostTabModel
    {
        public List<StaticDataModel> LocationModel { get; set; } = new List<StaticDataModel>();
        public List<StaticDataModel> HeightModel { get; set; } = new List<StaticDataModel>();
        public List<StaticDataModel> AgeModel { get; set; } = new List<StaticDataModel>();
        public List<StaticDataModel> BloodTypeModel { get; set; } = new List<StaticDataModel>();
        public List<StaticDataModel> ConstellationModel { get; set; } = new List<StaticDataModel>();
        public List<StaticDataModel> OccupationModel { get; set; } = new List<StaticDataModel>();
    }

    public class SearchV2FilterHostTabRequestModel
    {
        public int StartIndex { get; set; } = 0;
        public int PageSize { get; set; } = 12;
        public string SearchFilter { get; set; }
        public string LocationId_2 { get; set; }
        public string Height { get; set; }
        public string Age { get; set; }
        public string BloodType { get; set; }
        public string ConstellationGroup { get; set; }
        public string Occupation { get; set; }
    }

    public class SearchV2FilterHostTabResponseModel
    {
        public List<DashboardV2HostDetailModel> FilteredHostModel { get; set; } = new List<DashboardV2HostDetailModel>();
        public List<DashboardV2HostDetailModel> HostRecommendationModel { get; set; } = new List<DashboardV2HostDetailModel>();
        public HostSearchFilterRequestModel RequestModel { get; set; }
    }
    #endregion

    #region NEW TAB
    public class SearchV2FilterNewTabModel
    {
        public List<DashboardV2ClubDetailModel> ClubModel { get; set; } = new List<DashboardV2ClubDetailModel>();
        public List<DashboardV2HostDetailModel> HostModel { get; set; } = new List<DashboardV2HostDetailModel>();
    }
    #endregion

    #region MAP TAB
    public class SearchV2FilterMapTabModel
    {
        public List<StaticDataModel> LocationModel { get; set; } = new List<StaticDataModel>();
    }
    #endregion

    public class SearchV2ClubFilterRequestModel
    {
        public string SearchFilter { get; set; }
        public string ClubCategory { get; set; }
        public string Price { get; set; }
        public string Shift { get; set; }
        public string Time { get; set; }
        public string ClubAvailability { get; set; }
    }

    public class SearchV2HostFilterRequestModel
    {
        public string SearchFilter { get; set; }
        public string Height { get; set; }
        public string Age { get; set; }
        public string BloodType { get; set; }
        public string ConstellationGroup { get; set; }
        public string Occupation { get; set; }
    }

    public class SearchV2ClubDateTimeFilterRequestModel
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string NoOfPeople { get; set; }
        public string ResultType { get; set; }
        public string FilteredTime { get; set; }
    }
}