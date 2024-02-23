using CRS.CUSTOMER.SHARED.DashboardV2;
using System.Collections.Generic;
using System.Linq;

namespace CRS.CUSTOMER.REPOSITORY.DashboardV2
{
    public class DashboardV2Repository : IDashboardV2Repository
    {
        private readonly RepositoryDao _dao;
        public DashboardV2Repository() => (_dao) = new RepositoryDao();

        public List<ClubDetailCommon> GetNewClub(string LocationId, string CustomerId, string Type = "")
        {
            string SQL = $"EXEC sproc_cp_dashboard_v2 @flag = '1', @LocationId = {_dao.FilterString(LocationId)}";
            SQL += !string.IsNullOrEmpty(CustomerId) ? $", @CustomerId = {_dao.FilterString(Type)}" : string.Empty;
            SQL += !string.IsNullOrEmpty(Type) ? $", @ResultType = {_dao.FilterString(Type)}" : string.Empty;
            var dbResponse = _dao.ExecuteDataTable(SQL);
            return dbResponse != null && dbResponse.Rows.Count > 0
                ? _dao.DataTableToListObject<ClubDetailCommon>(dbResponse).ToList()
                : new List<ClubDetailCommon>();
        }
        public List<HostDetailCommon> GetNewHost(string LocationId, string CustomerId, string Type = "")
        {
            string SQL = $"EXEC sproc_cp_dashboard_v2 @flag = '2', @LocationId = {_dao.FilterString(LocationId)}";
            SQL += !string.IsNullOrEmpty(CustomerId) ? $", @CustomerId = {_dao.FilterString(Type)}" : string.Empty;
            SQL += !string.IsNullOrEmpty(Type) ? $", @ResultType = {_dao.FilterString(Type)}" : string.Empty;
            var dbResponse = _dao.ExecuteDataTable(SQL);
            return dbResponse != null && dbResponse.Rows.Count > 0
                ? _dao.DataTableToListObject<HostDetailCommon>(dbResponse).ToList()
                : new List<HostDetailCommon>();
        }
    }
}
