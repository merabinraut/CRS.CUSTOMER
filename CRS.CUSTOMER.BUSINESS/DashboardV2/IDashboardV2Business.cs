using CRS.CUSTOMER.SHARED.DashboardV2;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.DashboardV2
{
    public interface IDashboardV2Business
    {
        List<ClubDetailCommon> GetNewClub(string LocationId, string CustomerId, string Type = "");
        List<HostDetailCommon> GetNewHost(string LocationId, string CustomerId, string Type = "");
        #region CLUB AVAILABILITY
        List<ClubAvailabilityDetailCommon> GetAvailabilityClub(string LocationId, string CustomerId, string AvailabilityType);
        #endregion
    }
}
