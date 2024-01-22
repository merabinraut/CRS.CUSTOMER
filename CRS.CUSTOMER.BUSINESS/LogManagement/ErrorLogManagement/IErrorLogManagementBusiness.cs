using System;

namespace CRS.CUSTOMER.BUSINESS.LogManagement.ErrorLogManagement
{
    public interface IErrorLogManagementBusiness
    {
        string LogError(Exception Exception, string Page);
    }
}
