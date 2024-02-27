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
    }
}
