using CRS.CUSTOMER.REPOSITORY.Search;
using CRS.CUSTOMER.SHARED.Search;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.Search
{
    public class SearchBusiness : ISearchBusiness
    {
        private readonly ISearchRepository _repo;
        public SearchBusiness(SearchRepository repo) => (_repo) = _repo = repo;

        public List<SearchFilterClubDetailCommon> GetNewClub(string LocationId, string CustomerId)
        {
            return _repo.GetNewClub(LocationId, CustomerId);
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
    }
}
