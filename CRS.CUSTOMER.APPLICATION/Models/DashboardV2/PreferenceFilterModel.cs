using CRS.CUSTOMER.APPLICATION.Models.CommonModel;
using CRS.CUSTOMER.APPLICATION.Models.Dashboard;
using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.DashboardV2
{
    public class PreferenceFilterModel
    {
        public List<StaticDataModel> ClubCategoryModel { get; set; }
        public List<StaticDataModel> PlanPriceModel { get; set; }
        public List<StaticDataModel> ClubAvailabilityModel { get; set; }
        public List<StaticDataModel> HeightModel { get; set; }
        public List<StaticDataModel> AgeModel { get; set; }
        public List<StaticDataModel> BloodTypeModel { get; set; }
        public List<StaticDataModel> ConstellationModel { get; set; }
        public List<DashboardRecommendedClubModel> ClubModel { get; set; }
        public List<DashboardRecommendedHostModel> HostModel { get; set; }
    }
}