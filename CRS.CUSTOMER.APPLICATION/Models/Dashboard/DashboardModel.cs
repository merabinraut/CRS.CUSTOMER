using System.Collections.Generic;

namespace CRS.CUSTOMER.APPLICATION.Models.Dashboard
{
    public class DashboardModel
    {
        public List<LocationListModel> Location { get; set; } = new List<LocationListModel>();
        public List<BannersModel> Banners { get; set; } = new List<BannersModel>();
        public List<DashboardPlanModel> PlanModel { get; set; } = new List<DashboardPlanModel>();
        public List<DashboardRecommendedHostModel> RecommendedHostModel { get; set; } = new List<DashboardRecommendedHostModel>();
        public List<DashboardRecommendedClubModel> RecommendedClubModel { get; set; } = new List<DashboardRecommendedClubModel>();
    }
}