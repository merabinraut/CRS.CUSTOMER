using CRS.CUSTOMER.SHARED.Dashboard;
using CRS.CUSTOMER.SHARED.DashboardV2;
using CRS.CUSTOMER.SHARED.SearchFilterManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.REPOSITORY.Dashboard
{
    public interface IDashboardRepository
    {
        List<BannerCommon> GetBanners();
        List<LocationListCommon> GetLocationList();
        List<DashboardPlanCommon> GetPlansList();
        List<DashboardRecommendedHostCommon> GetRecommendedHost();
        List<DashboardRecommendedClubCommon> GetRecommendedClub();
        #region Recommendation Management
        List<ClubRecommendationListCommon> GetRecommendedClub(string LocationId);
        List<HostSearchManagementResponseCommon> GetRecommendedHost(string LocationId);
        #endregion
    }
}
