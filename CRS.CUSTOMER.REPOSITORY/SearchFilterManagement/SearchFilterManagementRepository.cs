using CRS.CUSTOMER.SHARED.LocationManagement;
using CRS.CUSTOMER.SHARED.SearchFilterManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CRS.CUSTOMER.REPOSITORY.SearchFilterManagement
{
    public class SearchFilterManagementRepository : ISearchFilterManagementRepository
    {
        private readonly RepositoryDao _dao;
        public SearchFilterManagementRepository()
        {
            _dao = new RepositoryDao();
        }

        public List<SearchFilterViewNewClubCommon> GetNewClubList()
        {
            string SQL = "EXEC sproc_search_management @Flag='gncl'";
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) return _dao.DataTableToListObject<SearchFilterViewNewClubCommon>(dbResponse).ToList();
            return new List<SearchFilterViewNewClubCommon>();
        }

        public List<ClubSearchManagementResponseCommon> GetSearchedClub(ClubSearchManagementRequestCommon Request)
        {
            var Response = new List<ClubSearchManagementResponseCommon>();
            string SQL = "EXEC sproc_search_management @Flag = 'csm'";

            if (!string.IsNullOrWhiteSpace(Request.AgentId))
                SQL += ",@AgentId=" + _dao.FilterString(Request.AgentId);
            else
            {
                SQL += !string.IsNullOrEmpty(Request.SearchText) ? ",@SearchText=" + _dao.FilterString(Request.SearchText) : "";
                SQL += !string.IsNullOrEmpty(Request.LocationId) ? ",@LocationId=" + _dao.FilterString(Request.LocationId) : "";
                SQL += !string.IsNullOrEmpty(Request.ClubCategory) ? ",@ClubCategory=" + _dao.FilterString(Request.ClubCategory) : "";
                SQL += !string.IsNullOrEmpty(Request.Shift) ? ",@Shift=" + _dao.FilterString(Request.Shift) : "";
            }
            SQL += ",@CustomerAgentId=" + _dao.FilterString(Request.CustomerAgentId);

            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                Response = _dao.DataTableToListObject<ClubSearchManagementResponseCommon>(dbResponse).ToList();
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
                }
                return Response;
            }
            return Response;
        }

        public List<HostSearchManagementResponseCommon> GetSearchedHost(HostSearchManagementRequestCommon Request)
        {
            string SQL = "EXEC sproc_search_management @Flag = 'hsm'";
            SQL += !string.IsNullOrEmpty(Request.AgentId) ? ",@AgentId=" + _dao.FilterString(Request.AgentId) : "";
            SQL += !string.IsNullOrEmpty(Request.LocationId) ? ",@LocationId=" + _dao.FilterString(Request.LocationId) : "";
            SQL += !string.IsNullOrEmpty(Request.SearchText) ? ",@SearchText=" + _dao.FilterString(Request.SearchText) : "";
            SQL += !string.IsNullOrEmpty(Request.Rank) ? ",@Rank=" + _dao.FilterString(Request.Rank) : "";
            SQL += !string.IsNullOrEmpty(Request.Height) ? ",@Height=" + _dao.FilterString(Request.Height) : "";
            SQL += !string.IsNullOrEmpty(Request.BloodType) ? ",@BloodType=" + _dao.FilterString(Request.BloodType) : "";
            SQL += !string.IsNullOrEmpty(Request.ZodiacGroup) ? ",@Zodiac=" + _dao.FilterString(Request.ZodiacGroup) : "";
            SQL += !string.IsNullOrEmpty(Request.LiquorStrength) ? ",@LiquorStrength=" + _dao.FilterString(Request.LiquorStrength) : "";
            SQL += !string.IsNullOrEmpty(Request.PreviousOccupation) ? ",@PrevOccupation=" + _dao.FilterString(Request.PreviousOccupation) : "";
            SQL += !string.IsNullOrEmpty(Request.Handsomeness) ? ",@Handsomeness=" + _dao.FilterString(Request.Handsomeness) : "";
            SQL += !string.IsNullOrEmpty(Request.AgeRange) ? ",@AgeRange=" + _dao.FilterString(Request.AgeRange) : "";
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) return _dao.DataTableToListObject<HostSearchManagementResponseCommon>(dbResponse).ToList();
            return new List<HostSearchManagementResponseCommon>();
        }

        #region Recommendation Management
        public List<ClubRecommendationListCommon> GetRecommendedClub(string LocationId)
        {
            var Response = new List<ClubRecommendationListCommon>();
            string SQL = "EXEC sproc_get_customer_recommendation @Flag = 'gspcrvl'";
            SQL += ",@LocationId=" + _dao.FilterString(LocationId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) Response = _dao.DataTableToListObject<ClubRecommendationListCommon>(dbResponse).ToList();
            return Response;
        }

        public List<HostSearchManagementResponseCommon> GetRecommendedHost(string LocationId, string CustomerId)
        {
            var Response = new List<HostSearchManagementResponseCommon>();
            string SQL = "EXEC sproc_get_customer_recommendation @Flag = 'gsphrvl'";
            SQL += ",@LocationId=" + _dao.FilterString(LocationId);
            SQL += ",@CustomerId=" + _dao.FilterString(CustomerId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) Response = _dao.DataTableToListObject<HostSearchManagementResponseCommon>(dbResponse).ToList();
            return Response;
        }
        #endregion
    }
}
