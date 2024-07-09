using CRS.CUSTOMER.SHARED.RecommendedClubHost;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.RecommendedClubHost
{
    public interface IRecommendedClubHostBusiness
    {
        List<RecommendedClubResponseCommon> GetRecommendedClub(RecommendedClubRequestCommon Request);
        List<RecommendedHostResponseCommon> GetRecommendedHost(RecommendedHostRequestCommon Request);
        int GetTotalRecommendedPageCount(string LocationId);
    }
}
