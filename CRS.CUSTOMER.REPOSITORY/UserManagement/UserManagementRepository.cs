namespace CRS.CUSTOMER.REPOSITORY.UserManagement
{
    public class UserManagementRepository : IUserManagementRepository
    {
        RepositoryDao _DAO;
        public UserManagementRepository()
        {
            _DAO = new RepositoryDao();
        }
    }
}
