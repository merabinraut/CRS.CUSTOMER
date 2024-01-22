using CRS.CUSTOMER.REPOSITORY.RecommendedClubHost;
using CRS.CUSTOMER.SHARED.RecommendedClubHost;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.RecommendedClubHost
{
    public class RecommendedClubHostBusiness : IRecommendedClubHostBusiness
    {
        private readonly IRecommendedClubHostRepository _repo;
        public RecommendedClubHostBusiness(RecommendedClubHostRepository repo)
        {
            _repo = repo;
        }

        public List<RecommendedClubResponseCommon> GetRecommendedClub(RecommendedClubRequestCommon Request)
        {
            return _repo.GetRecommendedClub(Request);
        }

        public List<RecommendedHostResponseCommon> GetRecommendedHost(RecommendedHostRequestCommon Request)
        {
            return _repo.GetRecommendedHost(Request);
        }

        public int GetTotalRecommendedPageCount()
        {
            return _repo.GetTotalRecommendedPageCount();
        }
    }
}
