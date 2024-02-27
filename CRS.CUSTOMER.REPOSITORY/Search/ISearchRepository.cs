using CRS.CUSTOMER.SHARED.Search;
using System.Collections.Generic;

namespace CRS.CUSTOMER.REPOSITORY.Search
{
    public interface ISearchRepository
    {
        List<SearchFilterClubDetailCommon> GetNewClub(string LocationId, string CustomerId);
    }
}
