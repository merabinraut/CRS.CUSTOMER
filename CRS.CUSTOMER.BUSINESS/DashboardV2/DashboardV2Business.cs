using CRS.CUSTOMER.REPOSITORY.DashboardV2;
using CRS.CUSTOMER.SHARED.DashboardV2;
using CRS.CUSTOMER.SHARED.Search;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.DashboardV2
{
    public class DashboardV2Business : IDashboardV2Business
    {
        private readonly IDashboardV2Repository _repo;
        public DashboardV2Business(DashboardV2Repository repo) => (_repo) = (repo);

        public List<ClubDetailCommon> GetNewClub(string LocationId, string CustomerId, string Type = "")
        {
            return _repo.GetNewClub(LocationId, CustomerId, Type);
        }

        public List<HostDetailCommon> GetNewHost(string LocationId, string CustomerId, HostPreferenceFilterRequest dbRequest, string Type = "")
        {
            return _repo.GetNewHost(LocationId, CustomerId, dbRequest, Type);
        }
        #region CLUB AVAILABILITY
        public List<ClubAvailabilityDetailCommon> GetAvailabilityClub(string LocationId, string CustomerId, string AvailabilityType, ClubPreferenceFilterRequest Request)
        {
            return _repo.GetAvailabilityClub(LocationId, CustomerId, AvailabilityType, Request);
        }
        #endregion
    }
}
