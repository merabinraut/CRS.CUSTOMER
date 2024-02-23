using CRS.CUSTOMER.SHARED.DashboardV2;
using System.Collections.Generic;

namespace CRS.CUSTOMER.REPOSITORY.DashboardV2
{
    public interface IDashboardV2Repository
    {
        List<ClubDetailCommon> GetNewClub(string LocationId, string CustomerId, string Type = "");
        List<HostDetailCommon> GetNewHost(string LocationId, string CustomerId, string Type = "");
    }
}
