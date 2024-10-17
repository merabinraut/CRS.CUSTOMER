using CRS.CUSTOMER.REPOSITORY.Search;
using CRS.CUSTOMER.SHARED.Search;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.Search
{
    public class SearchBusiness : ISearchBusiness
    {
        private readonly ISearchRepository _repo;
        public SearchBusiness(SearchRepository repo) => (_repo) = _repo = repo;

        public List<SearchFilterClubDetailCommon> GetNewClub(string LocationId, string CustomerId, ClubPreferenceFilterRequest dbRequest)
        {
            return _repo.GetNewClub(LocationId, CustomerId, dbRequest);
        }

        public List<SearchFilterClubDetailCommon> ClubPreferenceFilter(ClubPreferenceFilterRequest Request)
        {
            return _repo.ClubPreferenceFilter(Request);
        }

        public List<HostPreferenceFilterResponse> HostPreferenceFilter(HostPreferenceFilterRequest Request)
        {
            return _repo.HostPreferenceFilter(Request);
        }

        public List<SearchFilterClubDetailCommon> ClubFilterViewDateTimeAndOthers(ClubDateTimeAndOtherFilterRequest Request)
        {
            return _repo.ClubFilterViewDateTimeAndOthers(Request);
        }
        #region Club map details common
        public List<ClubMapDetailCommon> GetClubMapDetail(string LocationId = "")
        {
            return _repo.GetClubMapDetail(LocationId);
        }
        #endregion
    }
}
