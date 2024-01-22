using CRS.CUSTOMER.REPOSITORY.LogManagement.APILogManagement;

namespace CRS.CUSTOMER.BUSINESS.LogManagement.APILogManagement
{
    public class APILogManagementBusiness : IAPILogManagementBusiness
    {
        IAPILogManagementRepository _REPO;
        public APILogManagementBusiness(APILogManagementRepository REPO)
        {
            _REPO = REPO;
        }
    }
}
