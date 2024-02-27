using CRS.CUSTOMER.SHARED.Search;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CRS.CUSTOMER.REPOSITORY.Search
{
    public class SearchRepository : ISearchRepository
    {
        private readonly RepositoryDao _dao;
        public SearchRepository() => (_dao) = new RepositoryDao();

        public List<SearchFilterClubDetailCommon> GetNewClub(string LocationId, string CustomerId)
        {
            var Response = new List<SearchFilterClubDetailCommon>();
            string SQL = $"EXEC sproc_cp_dashboard_v2 @flag='4', @LocationId = {_dao.FilterString(LocationId)}" +
               $",@CustomerId = {_dao.FilterString(CustomerId)}";
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                Response = _dao.DataTableToListObject<SearchFilterClubDetailCommon>(dbResponse).ToList();
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
    }
}
