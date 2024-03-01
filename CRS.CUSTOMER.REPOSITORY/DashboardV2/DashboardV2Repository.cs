using CRS.CUSTOMER.SHARED.DashboardV2;
using System;
using System.Collections.Generic;
using System.Data;
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
        #region CLUB AVAILABILITY
        public List<ClubAvailabilityDetailCommon> GetAvailabilityClub(string LocationId, string CustomerId, string AvailabilityType)
        {
            var Response = new List<ClubAvailabilityDetailCommon>();
            string SQL = $"EXEC sproc_cp_dashboard_v2 @flag='3', @LocationId = {_dao.FilterString(LocationId)}" +
                $",@CustomerId = {_dao.FilterString(CustomerId)}, @AvailabilityType = {_dao.FilterString(AvailabilityType)}";
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                Response = _dao.DataTableToListObject<ClubAvailabilityDetailCommon>(dbResponse).ToList();
                foreach (var item in Response)
                {
                    string reviewSQL = "EXEC sproc_get_customer_recommended_clubandhost @Flag = 'grdvci'";
                    reviewSQL += ",@ClubId=" + _dao.FilterString(item.ClubId);
                    var reviewDBResponse = _dao.ExecuteDataRow(reviewSQL);
                    if (reviewDBResponse != null)
                    {
                        item.AverageRating = Convert.ToInt32(_dao.ParseColumnValue(reviewDBResponse, "AverageRating").ToString());
                        item.TotalComment = Convert.ToInt32(_dao.ParseColumnValue(reviewDBResponse, "TotalComment").ToString());
                    }
                    #region Host Image
                    List<string> stringList = new List<string>();
                    string hostImageSQL = "EXEC sproc_customer_club_and_host_management @Flag='ghgil'";
                    hostImageSQL += ",@ClubId=" + _dao.FilterString(item.ClubId);
                    var hostImageDBResponse = _dao.ExecuteDataTable(hostImageSQL);
                    if (hostImageDBResponse != null && hostImageDBResponse.Rows.Count > 0)
                    {
                        if (hostImageDBResponse.Columns.Contains("ImagePath"))
                        {
                            foreach (DataRow item2 in hostImageDBResponse.Rows)
                            {
                                object imagePathValue = item2["ImagePath"];
                                if (imagePathValue != null)
                                {
                                    string imagePath = imagePathValue.ToString();
                                    stringList.Add(imagePath);
                                }
                            }
                        }
                    }
                    item.HostGalleryImage = stringList;
                    #endregion
                }
            }
            return Response;
        }
        #endregion
    }
}
