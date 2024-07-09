using CRS.CUSTOMER.SHARED.RecommendedClubHost;
using System.Collections.Generic;

namespace CRS.CUSTOMER.REPOSITORY.RecommendedClubHost
{
    public interface IRecommendedClubHostRepository
    {
        List<RecommendedClubResponseCommon> GetRecommendedClub(RecommendedClubRequestCommon Request);
        List<RecommendedHostResponseCommon> GetRecommendedHost(RecommendedHostRequestCommon Request);
        int GetTotalRecommendedPageCount(string LocationId);
    }
}
