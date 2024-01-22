using CRS.CUSTOMER.REPOSITORY.LogManagement.EmailLogManagement;

namespace CRS.CUSTOMER.BUSINESS.LogManagement.EmailLogManagement
{
    public class EmailLogManagementBusiness : IEmailLogManagementBusiness
    {
        IEmailLogManagementRepository _REPO;
        public EmailLogManagementBusiness(EmailLogManagementRepository REPO)
        {
            _REPO = REPO;
        }
    }
}
