using CRS.CUSTOMER.SHARED.Search;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.Search
{
    public interface ISearchBusiness
    {
        List<SearchFilterClubDetailCommon> GetNewClub(string LocationId, string CustomerId, ClubPreferenceFilterRequest dbRequest);
        List<SearchFilterClubDetailCommon> ClubPreferenceFilter(ClubPreferenceFilterRequest Request);
        List<HostPreferenceFilterResponse> HostPreferenceFilter(HostPreferenceFilterRequest Request);
        List<SearchFilterClubDetailCommon> ClubFilterViewDateTimeAndOthers(ClubDateTimeAndOtherFilterRequest Request);
        #region Club map details common
        List<ClubMapDetailCommon> GetClubMapDetail(string LocationId = "");
        #endregion
    }
}
