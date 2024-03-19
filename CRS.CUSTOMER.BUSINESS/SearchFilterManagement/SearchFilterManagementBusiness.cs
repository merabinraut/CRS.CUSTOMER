using CRS.CUSTOMER.REPOSITORY.SearchFilterManagement;
using CRS.CUSTOMER.SHARED.SearchFilterManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.SearchFilterManagement
{
    public class SearchFilterManagementBusiness : ISearchFilterManagementBusiness
    {
        private readonly ISearchFilterManagementRepository _repo;
        public SearchFilterManagementBusiness(SearchFilterManagementRepository repo) => _repo = repo;

        public List<SearchFilterViewNewClubCommon> GetNewClubList()
        {
            return _repo.GetNewClubList();
        }

        public List<ClubSearchManagementResponseCommon> GetSearchedClub(ClubSearchManagementRequestCommon Request)
        {
            return _repo.GetSearchedClub(Request);
        }

        public List<HostSearchManagementResponseCommon> GetSearchedHost(HostSearchManagementRequestCommon Request)
        {
            return _repo.GetSearchedHost(Request);
        }
        #region Recommendation Management
        public List<ClubRecommendationListCommon> GetRecommendedClub(string LocationId)
        {
            return _repo.GetRecommendedClub(LocationId);
        }
        public List<HostSearchManagementResponseCommon> GetRecommendedHost(string LocationId, string CustomerId)
        {
            return _repo.GetRecommendedHost(LocationId, CustomerId);
        }
        #endregion
    }
}
