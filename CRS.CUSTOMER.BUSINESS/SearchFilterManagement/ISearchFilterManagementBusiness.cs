﻿using CRS.CUSTOMER.SHARED.SearchFilterManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.SearchFilterManagement
{
    public interface ISearchFilterManagementBusiness
    {
        List<SearchFilterViewNewClubCommon> GetNewClubList();
        List<ClubSearchManagementResponseCommon> GetSearchedClub(ClubSearchManagementRequestCommon Request);
        List<HostSearchManagementResponseCommon> GetSearchedHost(HostSearchManagementRequestCommon Request);
        #region Recommendation Management
        List<ClubRecommendationListCommon> GetRecommendedClub(string LocationId);
        List<HostSearchManagementResponseCommon> GetRecommendedHost(string LocationId, string CustomerId);
        #endregion
    }
}
