namespace CRS.CUSTOMER.REPOSITORY.DashboardV2
{
    public class DashboardV2Repository : IDashboardV2Repository
    {
        private readonly RepositoryDao _dao;
        public DashboardV2Repository() => (_dao) = new RepositoryDao();

    }
}
