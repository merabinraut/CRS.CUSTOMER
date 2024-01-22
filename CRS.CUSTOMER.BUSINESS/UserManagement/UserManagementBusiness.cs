using CRS.CUSTOMER.REPOSITORY.UserManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.Home;

namespace CRS.CUSTOMER.BUSINESS.UserManagement
{
    public class UserManagementBusiness : IUserManagementBusiness
    {
        IUserManagementRepository _REPO;
        public UserManagementBusiness(UserManagementRepository REPO)
        {
            _REPO = REPO;
        }

    }
}
