using CRS.CUSTOMER.SHARED.LocationManagement;
using CRS.CUSTOMER.SHARED.ReviewManagement;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CRS.CUSTOMER.REPOSITORY.LocationManagement
{
    public class LocationManagementRepository : ILocationManagementRepository
    {
        private readonly RepositoryDao _dao;
        public LocationManagementRepository(RepositoryDao dao) => _dao = dao;

        public ClubDetailCommon GetClubDetailById(string clubId, string CustomerId = "")
        {
            string sql = "sproc_customer_club_and_host_management @Flag='cd'";
            sql += " ,@ClubId=" + _dao.FilterString(clubId);
            sql += " ,@customerAgentId=" + _dao.FilterString(CustomerId);
            var dbResp = _dao.ExecuteDataTable(sql);
            if (dbResp != null && dbResp.Rows.Count == 1)
            {
                var response = _dao.DataTableToListObject<ClubDetailCommon>(dbResp).ToList();
                string SQL2 = "EXEC sproc_club_schedule_management @Flag ='gcws'";
                SQL2 += ",@ClubId=" + _dao.FilterString(clubId);
                var dbResponse2 = _dao.ExecuteDataTable(SQL2);

                string sql3 = "sproc_customer_club_and_host_management @Flag='gce'";
                sql3 += " ,@ClubId=" + _dao.FilterString(clubId);
                var dbResp3 = _dao.ExecuteDataTable(sql3);
                foreach (var item in response)
                {
                    if (dbResponse2 != null && dbResponse2.Rows.Count > 0) item.ClubWeeklyScheduleList = _dao.DataTableToListObject<ClubWeeklyScheduleCommon>(dbResponse2).ToList();
                    if (dbResp3 != null && dbResp3.Rows.Count > 0) item.ClubEventList = _dao.DataTableToListObject<ClubEventCommon>(dbResp3).ToList();
                }
                return response.First();
            }
            return new ClubDetailCommon();
        }

        public List<LocationManagementCommon> GetLocations()
        {
            throw new System.NotImplementedException();
        }

        #region "Club Reservation"
        public List<TimeIntervalListCommon> GetTimeInterval()
        {
            var timeInterval = new List<TimeIntervalListCommon>();
            string sp_name = "sproc_gettimeinterval @Flag='gti'";
            var dbResponseInfo = _dao.ExecuteDataTable(sp_name);
            if (dbResponseInfo != null && dbResponseInfo.Rows.Count > 0)
            {
                foreach (DataRow row in dbResponseInfo.Rows)
                {
                    timeInterval.Add(new TimeIntervalListCommon
                    {
                        ClubOpeningTime = row["ClubOpeningTime"].ToString(),
                        ClubClosingTime = row["ClubClosingTime"].ToString(),
                    });
                }
            }
            return timeInterval;
        }
        #endregion

        #region Club/Host list via location
        public List<LocationClubListCommon> GetClubListViaLoaction(string LocationId, string customerId)
        {
            var Response = new List<LocationClubListCommon>();
            string SQL = "EXEC sproc_customer_club_and_host_management @Flag='cl'";
            SQL += ",@LocationId=" + _dao.FilterString(LocationId);
            SQL += ",@customerAgentId=" + _dao.FilterString(customerId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                Response = _dao.DataTableToListObject<LocationClubListCommon>(dbResponse).ToList();
                foreach (var item in Response)
                {
                    List<string> stringList = new List<string>();
                    string SQL2 = "EXEC sproc_customer_club_and_host_management @Flag='gcgil'";
                    SQL2 += ",@ClubId=" + _dao.FilterString(item.ClubId);
                    var dbResponse2 = _dao.ExecuteDataTable(SQL2);
                    if (dbResponse2 != null && dbResponse2.Rows.Count > 0)
                    {
                        if (dbResponse2.Columns.Contains("ImagePath"))
                        {
                            foreach (DataRow item2 in dbResponse2.Rows)
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
                    item.ClubGalleryImage = stringList as List<string>;
                    string SQL3 = "EXEC sproc_club_schedule_management @Flag ='gcws'";
                    SQL3 += ",@ClubId=" + _dao.FilterString(item.ClubId);
                    var dbResponse3 = _dao.ExecuteDataTable(SQL3);
                    if (dbResponse3 != null && dbResponse3.Rows.Count > 0) item.ClubWeeklyScheduleList = _dao.DataTableToListObject<ClubWeeklyScheduleCommon>(dbResponse3).ToList();
                }
            }
            return Response;

        }

        public List<LocationHostListCommon> GetHostList(string LocationId, string ClubId = "", string customerId = "", string Type = "")
        {
            string SQL = "EXEC sproc_customer_club_and_host_management ";
            SQL += (!string.IsNullOrEmpty(Type) && Type.Trim().ToLower() == "trhl") ? "@Flag='trhl'" : "@Flag='hl'";
            SQL += LocationId.Trim() != "" ? ",@LocationId=" + _dao.FilterString(LocationId) : "";
            SQL += ClubId.Trim() != "" ? ",@ClubId=" + _dao.FilterString(ClubId) : "";
            SQL += ",@customerAgentId=" + _dao.FilterString(customerId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null) return _dao.DataTableToListObject<LocationHostListCommon>(dbResponse).ToList();
            return new List<LocationHostListCommon>();
        }

        public List<string> GetClubGalleryImage(string ClubId, string SelectType = "")
        {
            string SQL = "EXEC sproc_customer_club_and_host_management @Flag='gcgil'";
            SQL += ",@ClubId=" + _dao.FilterString(ClubId);
            SQL += !string.IsNullOrEmpty(SelectType) ? ",@SelectType=" + _dao.FilterString(SelectType) : "";
            var dbResponse = _dao.ExecuteDataTable(SQL);
            List<string> Response = new List<string>();
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                foreach (DataRow row in dbResponse.Rows)
                {
                    Response.Add(row["ImagePath"].ToString());
                }
            }
            return Response;
        }

        public List<GetClubReviewsCommon> GetClubReviewAndRatings(string ClubId)
        {
            var Response = new List<GetClubReviewsCommon>();
            string SQL = "EXEC sproc_reviewrating_customer_panel @Flag = 'gctr'";
            SQL += ",@ClubId=" + _dao.FilterString(ClubId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                Response = _dao.DataTableToListObject<GetClubReviewsCommon>(dbResponse).ToList();
                foreach (var item in Response)
                {
                    var SQL2 = "EXEC sproc_reviewrating_customer_panel @Flag = 'ghlfr'";
                    SQL2 += ",@ClubId=" + _dao.FilterString(ClubId);
                    SQL2 += ",@ReviewId=" + _dao.FilterString(item.ReviewId);
                    var dbResponse2 = _dao.ExecuteDataTable(SQL2);
                    if (dbResponse2 != null && dbResponse2.Rows.Count > 0)
                    {
                        item.GetClubReviewHostList = _dao.DataTableToListObject<GetClubReviewHostCommon>(dbResponse2).ToList();
                    }

                    var SQL3 = "EXEC sproc_reviewrating_customer_panel @Flag = 'grlfr'";
                    SQL3 += ",@ClubId=" + _dao.FilterString(ClubId);
                    SQL3 += ",@ReviewId=" + _dao.FilterString(item.ReviewId);
                    var dbResponse3 = _dao.ExecuteDataTable(SQL3);
                    if (dbResponse3 != null && dbResponse3.Rows.Count > 0)
                    {
                        item.GetClubReviewRemarkList = _dao.DataTableToListObject<GetClubReviewRemarkCommon>(dbResponse3).ToList();
                    }
                }
            }
            return Response;
        }

        public List<ClubWeeklyScheduleCommon> GetClubReservationSchedule(string ClubId)
        {
            var Response = new List<ClubWeeklyScheduleCommon>();
            string SQL = "EXEC sproc_get_reservation_details @Flag = 'grtbc'";
            SQL += ",@ClubId=" + _dao.FilterString(ClubId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                Response = _dao.DataTableToListObject<ClubWeeklyScheduleCommon>(dbResponse).ToList();
            }
            return Response;
        }
        #endregion

        #region Host view details
        public ViewHostDetailCommon ViewHostDetails(string HostId, string customerId)
        {
            var Response = new ViewHostDetailCommon();
            string SQL = "EXEC sproc_customer_club_and_host_management @Flag='vhd'";
            SQL += ",@HostId=" + _dao.FilterString(HostId);
            SQL += ",@customerAgentId=" + _dao.FilterString(customerId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count == 1)
            {
                var ResponseMapper = _dao.DataTableToListObject<ViewHostDetailCommon>(dbResponse).ToList();
                Response = ResponseMapper[0];
                string SQL2 = "EXEC sproc_customer_club_and_host_management @Flag='ghgil'";
                SQL2 += ",@HostId=" + _dao.FilterString(HostId);
                var dbResponse2 = _dao.ExecuteDataTable(SQL2);
                List<string> stringList = new List<string>();
                if (dbResponse2 != null && dbResponse2.Rows.Count > 0)
                {
                    if (dbResponse2.Columns.Contains("ImagePath"))
                    {
                        foreach (DataRow item2 in dbResponse2.Rows)
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
                Response.HostGalleryImageList = stringList as List<string>;
                return Response;
            }
            return Response;
        }

        public ViewHostDetailCommonV2 ViewHostDetailsV2(string HostId, string customerId)
        {
            var Response = new ViewHostDetailCommonV2();
            string SQL = "EXEC sproc_cpanel_host_management @Flag='ghd'";
            SQL += ",@HostId=" + _dao.FilterString(HostId);
            SQL += ",@CustomerAgentId=" + _dao.FilterString(customerId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count == 1)
            {
                var ResponseMapper = _dao.DataTableToListObject<ViewHostDetailCommonV2>(dbResponse).ToList();
                Response = ResponseMapper[0];
                string SQL2 = "EXEC sproc_cpanel_host_management @Flag='ghgil'";
                SQL2 += ",@HostId=" + _dao.FilterString(HostId);
                var dbResponse2 = _dao.ExecuteDataTable(SQL2);
                List<string> stringList = new List<string>();
                if (dbResponse2 != null && dbResponse2.Rows.Count > 0)
                {
                    if (dbResponse2.Columns.Contains("ImagePath"))
                    {
                        foreach (DataRow item2 in dbResponse2.Rows)
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
                Response.HostGalleryImageList = stringList;
                var HostIdentityDetailsModelResponse = new List<HostIdentityDetailsCommon>();
                string SQL3 = "EXEC sproc_cpanel_host_management @Flag = 'ghs'";
                SQL3 += ",@HostId=" + _dao.FilterString(HostId);
                var dbResponse3 = _dao.ExecuteDataTable(SQL3);
                if (dbResponse3 != null && dbResponse3.Rows.Count > 0) HostIdentityDetailsModelResponse.AddRange(_dao.DataTableToListObject<HostIdentityDetailsCommon>(dbResponse3).ToList());
                string SQL4 = "EXEC sproc_cpanel_host_management @Flag = 'ghpnl'";
                SQL4 += ",@HostId=" + _dao.FilterString(HostId);
                var dbResponse4 = _dao.ExecuteDataTable(SQL4);
                if (dbResponse4 != null && dbResponse4.Rows.Count > 0) HostIdentityDetailsModelResponse.AddRange(_dao.DataTableToListObject<HostIdentityDetailsCommon>(dbResponse4).ToList());
                Response.HostIdentityDetailsModel = HostIdentityDetailsModelResponse;
                return Response;
            }
            return Response;
        }
        #endregion
    }
}
