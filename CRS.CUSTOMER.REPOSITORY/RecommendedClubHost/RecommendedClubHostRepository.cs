using CRS.CUSTOMER.SHARED.LocationManagement;
using CRS.CUSTOMER.SHARED.RecommendedClubHost;
using DocumentFormat.OpenXml.Office2016.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CRS.CUSTOMER.REPOSITORY.RecommendedClubHost
{
    public class RecommendedClubHostRepository : IRecommendedClubHostRepository
    {
        private readonly RepositoryDao _dao;
        public RecommendedClubHostRepository(RepositoryDao dao)
        {
            _dao = dao;
        }

        public List<RecommendedClubResponseCommon> GetRecommendedClub(RecommendedClubRequestCommon Request)
        {
            var Response = new List<RecommendedClubResponseCommon>();
            string SQL = "EXEC dbo.sproc_get_customer_recommended_clubandhost @Flag = 'grcl'";
            SQL += !string.IsNullOrEmpty(Request.PositionId) ? ",@PositionId=" + Request.PositionId : "";
            SQL += ",@CustomerId=" + _dao.FilterString(Request.CustomerId);
            SQL += ",@LocationId=" + _dao.FilterString(Request.LocationId);
            SQL += ",@PageType=" + _dao.FilterString(Request.PageType);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                Response = _dao.DataTableToListObject<RecommendedClubResponseCommon>(dbResponse).ToList();
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
                    List<string> stringList = new List<string>();
                    #region Club Image
                    //string SQL2 = "EXEC sproc_customer_club_and_host_management @Flag='gcgil'";
                    //SQL2 += ",@ClubId=" + _dao.FilterString(item.ClubId);
                    //var dbResponse2 = _dao.ExecuteDataTable(SQL2);
                    //if (dbResponse2 != null && dbResponse2.Rows.Count > 0)
                    //{
                    //    if (dbResponse2.Columns.Contains("ImagePath"))
                    //    {
                    //        foreach (DataRow item2 in dbResponse2.Rows)
                    //        {
                    //            object imagePathValue = item2["ImagePath"];
                    //            if (imagePathValue != null)
                    //            {
                    //                string imagePath = imagePathValue.ToString();
                    //                stringList.Add(imagePath);
                    //            }
                    //        }
                    //    }
                    //}
                    //item.ClubGalleryImage = stringList;
                    #endregion

                    #region Host Image
                    stringList = new List<string>();
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
                    //string SQL3 = "EXEC sproc_club_schedule_management @Flag ='gcws'";
                    //SQL3 += ",@ClubId=" + _dao.FilterString(item.ClubId);
                    //var dbResponse3 = _dao.ExecuteDataTable(SQL3);
                    //if (dbResponse3 != null && dbResponse3.Rows.Count > 0) item.ClubWeeklyScheduleList = _dao.DataTableToListObject<ClubWeeklyScheduleCommon>(dbResponse3).ToList();
                }
                return Response;
            }
            return new List<RecommendedClubResponseCommon>();
        }

        public List<RecommendedHostResponseCommon> GetRecommendedHost(RecommendedHostRequestCommon Request)
        {
            string SQL = "EXEC dbo.sproc_get_customer_recommended_clubandhost @Flag = 'grhl'";
            SQL += !string.IsNullOrEmpty(Request.PositionId) ? ",@PositionId=" + Request.PositionId : "";
            SQL += ",@CustomerId=" + _dao.FilterString(Request.CustomerId);
            SQL += ",@LocationId=" + _dao.FilterString(Request.LocationId);
            SQL += ",@PageType=" + _dao.FilterString(Request.PageType);
            SQL += !string.IsNullOrEmpty(Request.ClubId) ? ",@ClubId=" + _dao.FilterString(Request.ClubId) : "";
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) return _dao.DataTableToListObject<RecommendedHostResponseCommon>(dbResponse).ToList();
            return new List<RecommendedHostResponseCommon>();
        }

        public int GetTotalRecommendedPageCount()
        {
            int Response = 0;
            string SQL = "EXEC dbo.sproc_get_customer_recommended_clubandhost @Flag = 'gtgl'";
            var dbResponse = _dao.ExecuteDataRow(SQL);
            if (dbResponse != null)
            {
                if (!string.IsNullOrEmpty(_dao.ParseColumnValue(dbResponse, "TotalPages").ToString())) Response = Convert.ToInt32(_dao.ParseColumnValue(dbResponse, "TotalPages").ToString());
            }
            return Response;
        }
    }
}
