using System;

namespace CRS.CUSTOMER.REPOSITORY.LogManagement.ErrorLogManagement
{
    public interface IErrorLogManagementRepository
    {
        string LogError(Exception Exception, string Page);
    }
}
