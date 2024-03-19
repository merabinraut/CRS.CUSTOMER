using CRS.CUSTOMER.REPOSITORY.Dashboard;
using CRS.CUSTOMER.SHARED.Dashboard;
using CRS.CUSTOMER.SHARED.SearchFilterManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.Dashboard
{
    public class DashboardBusiness : IDashboardBusiness
    {
        private readonly IDashboardRepository _repository;
        public DashboardBusiness(DashboardRepository repository) => _repository = repository;

        public List<BannerCommon> GetBanners()
        {
            return _repository.GetBanners();
        }

        public List<LocationListCommon> GetLocationList()
        {
            return _repository.GetLocationList();
        }
        public List<DashboardPlanCommon> GetPlansList()
        {
            return _repository.GetPlansList();
        }
        public List<DashboardRecommendedHostCommon> GetRecommendedHost()
        {
            return _repository.GetRecommendedHost();
        }
        public List<DashboardRecommendedClubCommon> GetRecommendedClub()
        {
            return _repository.GetRecommendedClub();
        }

        #region Recommendation Management
        public List<ClubRecommendationListCommon> GetRecommendedClub(string LocationId, string PageType = "")
        {
            return _repository.GetRecommendedClub(LocationId);
        }
        public List<HostSearchManagementResponseCommon> GetRecommendedHost(string LocationId, string CustomerId = "")
        {
            return _repository.GetRecommendedHost(LocationId, CustomerId);
        }
        #endregion
    }
}