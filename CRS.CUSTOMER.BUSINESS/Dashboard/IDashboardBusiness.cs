using CRS.CUSTOMER.SHARED.Dashboard;
using CRS.CUSTOMER.SHARED.SearchFilterManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.Dashboard
{
    public interface IDashboardBusiness
    {
        List<BannerCommon> GetBanners();
        List<LocationListCommon> GetLocationList();
        List<DashboardPlanCommon> GetPlansList();
        List<DashboardRecommendedHostCommon> GetRecommendedHost();
        List<DashboardRecommendedClubCommon> GetRecommendedClub();
        #region Recommendation Management
        List<ClubRecommendationListCommon> GetRecommendedClub(string LocationId, string PageType = "");
        List<HostSearchManagementResponseCommon> GetRecommendedHost(string LocationId);
        #endregion
    }
}
