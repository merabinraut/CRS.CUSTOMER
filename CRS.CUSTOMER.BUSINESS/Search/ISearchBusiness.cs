using CRS.CUSTOMER.SHARED.Search;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.Search
{
    public interface ISearchBusiness
    {
        List<SearchFilterClubDetailCommon> GetNewClub(string LocationId, string CustomerId);
        List<SearchFilterClubDetailCommon> ClubPreferenceFilter(ClubPreferenceFilterRequest Request);
        List<HostPreferenceFilterResponse> HostPreferenceFilter(HostPreferenceFilterRequest Request);
    }
}
