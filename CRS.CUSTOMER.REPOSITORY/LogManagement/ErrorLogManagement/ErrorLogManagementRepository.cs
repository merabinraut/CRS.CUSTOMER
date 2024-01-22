using System;
using System.Diagnostics;

namespace CRS.CUSTOMER.REPOSITORY.LogManagement.ErrorLogManagement
{
    public class ErrorLogManagementRepository : IErrorLogManagementRepository
    {
        RepositoryDao _REPO;
        public ErrorLogManagementRepository()
        {
            _REPO = new RepositoryDao();
        }
        public string LogError(Exception Exception, string Page)
        {
            Exception error = Exception;
            if (Exception.InnerException != null) { error = Exception.InnerException; }
            var errorPage = _REPO.FilterString(Page);
            var errorMessage = _REPO.FilterString(error.Message);
            var errorDetails = _REPO.FilterString(Exception.StackTrace);

            var stackTrace = new StackTrace(error, true);
            var frame = stackTrace.GetFrame(0);
            var line = frame.GetFileLineNumber();

            var SQL = " sproc_error_management @flag = 'i', @ErrorDesc =" + _REPO.FilterString("Message => " + error.Message +
                      " and Line Number => " + line.ToString()) + ",@ErrorCategory='Application'" + ",@ErrorSource=" + _REPO.FilterString(Page);
            var rowId = _REPO.ExecuteDataRow(SQL);
            return _REPO.ParseColumnValue(rowId, "id").ToString();
        }
    }
}
