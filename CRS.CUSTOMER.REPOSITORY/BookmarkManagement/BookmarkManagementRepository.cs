using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.BookmarkManagement;
using CRS.CUSTOMER.SHARED.LocationManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CRS.CUSTOMER.REPOSITORY.BookmarkManagement
{
    public class BookmarkManagementRepository : IBookmarkManagementRepository
    {
        private readonly RepositoryDao _dao;
        public BookmarkManagementRepository() => _dao = new RepositoryDao();

        public List<BookmarkedClubCommon> GetBookmarkedClubList(string agentId)
        {
            string sql = "sproc_bookmark_management @Flag = 'clvb'";
            sql += ", @CustomerId=" + _dao.FilterString(agentId);
            var dbResponse = _dao.ExecuteDataTable(sql);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                var response = _dao.DataTableToListObject<BookmarkedClubCommon>(dbResponse).ToList();

                foreach (var item in response)
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
                    string SQL3 = "EXEC sproc_club_schedule_management @Flag ='gcws'";
                    SQL3 += ",@ClubId=" + _dao.FilterString(item.ClubId);
                    var dbResponse3 = _dao.ExecuteDataTable(SQL3);
                    if (dbResponse3 != null && dbResponse3.Rows.Count > 0) item.ClubWeeklyScheduleList = _dao.DataTableToListObject<ClubWeeklyScheduleCommon>(dbResponse3).ToList();
                    #region Club galary image
                    //List<string> galleryImages = new List<string>();
                    //string sqlCommand = "sproc_bookmark_management @Flag='gcgilvc'";
                    //sqlCommand += ",@CustomerId=" + _dao.FilterString(agentId);
                    //sqlCommand += ",@ClubId=" + _dao.FilterString(item.ClubId);
                    //var dbResponse2 = _dao.ExecuteDataTable(sqlCommand);
                    //if (dbResponse2 != null &&
                    //    dbResponse2.Rows.Count > 0 &&
                    //    dbResponse2.Columns.Contains("ImagePath"))
                    //{
                    //    foreach (DataRow item2 in dbResponse2.Rows)
                    //    {
                    //        string imagePath = item2["ImagePath"]?.ToString();
                    //        if (imagePath != null)
                    //        {
                    //            galleryImages.Add(imagePath);
                    //        }
                    //    }

                    //}
                    //item.ClubGalleryImage = galleryImages;
                    #endregion
                }
                return response;

            }
            return new List<BookmarkedClubCommon>();
        }
        public List<BookmarkedHostCommon> GetBookmarkedHostList(string agentId)
        {
            string sql = "sproc_bookmark_management @Flag = 'hlvb'";
            sql += ", @CustomerId=" + _dao.FilterString(agentId);
            var dbResponse = _dao.ExecuteDataTable(sql);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
                return _dao.DataTableToListObject<BookmarkedHostCommon>(dbResponse).ToList();
            return new List<BookmarkedHostCommon>();
        }

        public List<GalleryImageCommon> GetGalleryImage(ClubHostManagementCommon galleryImage, string imageFor)
        {
            string sql = "sproc_bookmark_management @Flag =" + imageFor.ToLower() == "club" ? "gcgilvc" : "ghgilvh";
            sql += " ,@CustomerId=" + _dao.FilterString(galleryImage.AgentId);
            if (galleryImage.ClubId != null)
                sql += " ,@ClubId=" + _dao.FilterString(galleryImage.ClubId);
            if (galleryImage.HostId != null)
                sql += " ,@HostId=" + _dao.FilterString(galleryImage.HostId);

            var dbResponse = _dao.ExecuteDataTable(sql);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
                return _dao.DataTableToListObject<GalleryImageCommon>(dbResponse).ToList();
            return new List<GalleryImageCommon>();
        }

        public CommonDbResponse ManageBoookmark(ClubHostManagementCommon manageClubBookmark, string manageFor)
        {
            string sql = "sproc_bookmark_management @Flag='mb'";
            sql += " ,@CustomerId=" + _dao.FilterString(manageClubBookmark.AgentId);
            sql += " ,@AgentType=" + _dao.FilterString(manageFor);
            sql += " ,@ClubId=" + _dao.FilterString(manageClubBookmark.ClubId);
            //sql += " ,@Status=" + _dao.FilterString(manageClubBookmark.Status);
            if (!string.IsNullOrEmpty(manageClubBookmark.HostId))
                sql += " ,@HostId=" + _dao.FilterString(manageClubBookmark.HostId);

            return _dao.ParseCommonDbResponse(sql);
        }
    }
}