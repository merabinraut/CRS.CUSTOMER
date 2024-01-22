using CRS.CUSTOMER.REPOSITORY.LogManagement.ErrorLogManagement;
using System;

namespace CRS.CUSTOMER.BUSINESS.LogManagement.ErrorLogManagement
{
    public class ErrorLogManagementBusiness : IErrorLogManagementBusiness
    {
        IErrorLogManagementRepository _REPO;
        public ErrorLogManagementBusiness()
        {
            _REPO = new ErrorLogManagementRepository();
        }

        public string LogError(Exception Exception, string Page)
        {
            return _REPO.LogError(Exception, Page);
        }
    }
}
