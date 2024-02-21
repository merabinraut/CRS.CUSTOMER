using CRS.CUSTOMER.REPOSITORY.DashboardV2;

namespace CRS.CUSTOMER.BUSINESS.DashboardV2
{
    public class DashboardV2Business : IDashboardV2Business
    {
        private readonly IDashboardV2Repository _repo;
        public DashboardV2Business(DashboardV2Repository repo) => (_repo) = (repo);
    }
}
