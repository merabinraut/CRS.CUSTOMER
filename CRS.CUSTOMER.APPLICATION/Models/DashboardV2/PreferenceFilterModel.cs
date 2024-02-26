using CRS.CUSTOMER.APPLICATION.Models.CommonModel;
using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.DashboardV2
{
    public class PreferenceFilterModel
    {
        public List<StaticDataModel> ClubCategoryModel { get; set; } = new List<StaticDataModel>();
        public List<StaticDataModel> PlanPriceModel { get; set; } = new List<StaticDataModel>();
        public List<StaticDataModel> ClubAvailabilityModel { get; set; } = new List<StaticDataModel>();
        public List<StaticDataModel> HeightModel { get; set; } = new List<StaticDataModel>();
        public List<StaticDataModel> AgeModel { get; set; } = new List<StaticDataModel>();
        public List<StaticDataModel> BloodTypeModel { get; set; } = new List<StaticDataModel>();
        public List<StaticDataModel> ConstellationModel { get; set; } = new List<StaticDataModel>();
        public List<StaticDataModel> OccupationModel { get; set; } = new List<StaticDataModel>();
        public List<DashboardV2ClubDetailModel> ClubModel { get; set; } = new List<DashboardV2ClubDetailModel>();
        public List<DashboardV2HostDetailModel> HostModel { get; set; } = new List<DashboardV2HostDetailModel>();
    }
    public class DashboardV2ClubDetailModel
    {
        public string ClubId { get; set; }
        public string ClubName { get; set; }
        public string ClubNameJapanese { get; set; }
        public string ClubLogo { get; set; }
        public string ClubLocationId { get; set; }
        public string IsBookmarked { get; set; }
    }
    public class DashboardV2HostDetailModel
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

    public class ClubAvailabilityDetailModel
    {
        public string ClubId { get; set; }
        public string ClubLocationId { get; set; }
        public string ClubNameEnglish { get; set; }
        public string ClubNameJapanese { get; set; }
        public string ClubLogo { get; set; }
        public string GroupName { get; set; }
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