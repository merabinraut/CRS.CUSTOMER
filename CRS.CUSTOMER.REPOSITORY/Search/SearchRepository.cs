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

        public List<SearchFilterClubDetailCommon> GetNewClub(string LocationId, string CustomerId, ClubPreferenceFilterRequest dbRequest)
        {
            var Response = new List<SearchFilterClubDetailCommon>();
            string SQL = $"EXEC sproc_cp_dashboard_v2 @flag='4', @LocationId = {_dao.FilterString(LocationId)}" +
               $",@CustomerId = {_dao.FilterString(CustomerId)}";
            SQL += ",@Skip=" + dbRequest.Skip;
            SQL += ",@Take=" + dbRequest.Take;

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

        public List<SearchFilterClubDetailCommon> ClubPreferenceFilter(ClubPreferenceFilterRequest Request)
        {
            var Response = new List<SearchFilterClubDetailCommon>();
            string SQL = $"EXEC sproc_cp_search_filter_management @Flag = '1', @LocationId= {_dao.FilterString(Request.LocationId)}, @ClubCategory= {_dao.FilterString(Request.ClubCategory)}, @Price= {_dao.FilterString(Request.Price)}, @Shift = {_dao.FilterString(Request.Shift)}, @Time= {_dao.FilterString(Request.Time)}, @ClubAvailability = {_dao.FilterString(Request.ClubAvailability)}, @CustomerId = {_dao.FilterString(Request.CustomerId)}" +
                $"{(!string.IsNullOrEmpty(Request.SearchFilter) ? $", @SearchFilter = N{_dao.FilterString(Request.SearchFilter)}" : "")}";

            SQL += ",@Skip=" + Request.Skip;
            SQL += ",@Take=" + Request.Take;

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
                    //List<HostSearchDetail> stringList = new List<HostSearchDetail>();
                    //string hostImageSQL = "EXEC sproc_customer_club_and_host_management @Flag='ghgil'";
                    //hostImageSQL += ",@ClubId=" + _dao.FilterString(item.ClubId);
                    //var hostImageDBResponse = _dao.ExecuteDataTable(hostImageSQL);
                    //if (hostImageDBResponse != null && hostImageDBResponse.Rows.Count > 0)
                    //{
                    //    if (hostImageDBResponse.Columns.Contains("ImagePath"))
                    //    {
                    //        foreach (DataRow item2 in hostImageDBResponse.Rows)
                    //        {
                    //            object imagePathValue = item2["ImagePath"];
                    //            object HostId = item2["HostId"];
                    //            if (imagePathValue != null)
                    //            {
                    //                string imagePath = imagePathValue.ToString();

                    //                HostSearchDetail imageModel = new HostSearchDetail
                    //                {
                    //                    ImagePath = imagePath,
                    //                    HostId = Convert.ToString(HostId)
                    //                };
                    //                stringList.Add(imageModel);

                    //            }
                    //        }
                    //    }
                    //}
                    //item.HostDetail = stringList;

                    #endregion
                }
            }
            return Response;
        }

        public List<HostPreferenceFilterResponse> HostPreferenceFilter(HostPreferenceFilterRequest Request)
        {
            var Response = new List<HostPreferenceFilterResponse>();
            string SQL = $"EXEC sproc_cp_search_filter_management @Flag = '2', @LocationId= {_dao.FilterString(Request.LocationId)}, @Height = {_dao.FilterString(Request.Height)}, @Age = {_dao.FilterString(Request.Age)}, @BloodType = {_dao.FilterString(Request.BloodType)}, @ConstellationGroup = {_dao.FilterString(Request.ConstellationGroup)}, @Occupation = {_dao.FilterString(Request.Occupation)}, @CustomerId = {_dao.FilterString(Request.CustomerId)} " +
                 $"{(!string.IsNullOrEmpty(Request.SearchFilter) ? $", @SearchFilter = N{_dao.FilterString(Request.SearchFilter)}" : "")}";
            if (!string.IsNullOrEmpty(Request.Type) && Request.Type.Trim() == "1")
            {
                SQL += ",@Type=" + _dao.FilterString(Request.Type);
                SQL += ",@Skip=" + Request.Skip;
                SQL += ",@Take=" + Request.Take;
            }
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
                Response = _dao.DataTableToListObject<HostPreferenceFilterResponse>(dbResponse).ToList();
            return Response;
        }

        public List<SearchFilterClubDetailCommon> ClubFilterViewDateTimeAndOthers(ClubDateTimeAndOtherFilterRequest Request)
        {
            var Response = new List<SearchFilterClubDetailCommon>();
            var flag = (!string.IsNullOrEmpty(Request.ResultType) && Request.ResultType == "1") ? "4" : (!string.IsNullOrEmpty(Request.ResultType) && Request.ResultType == "2") ? "5" : "3";
            var time = !string.IsNullOrEmpty(Request.FilteredTime) ? _dao.FilterString(Request.FilteredTime) : _dao.FilterString(Request.Time);
            string SQL = $"EXEC sproc_cp_search_filter_management @Flag = '{flag}', @LocationId= {_dao.FilterString(Request.LocationId)}, @Time={time} , @Date = {_dao.FilterString(Request.Date)}, @CustomerId = {_dao.FilterString(Request.CustomerId)}, @NoOfPeople = {_dao.FilterString(Request.NoOfPeople)}";
            SQL += ",@Skip=" + Request.Skip;
            SQL += ",@Take=" + Request.Take;
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
        #region Club map details common
        public List<ClubMapDetailCommon> GetClubMapDetail(string LocationId = "")
        {
            var response = new List<ClubMapDetailCommon>();
            string SQL = $"EXEC sproc_cp_dashboard_v2 @flag='5', @LocationId = {_dao.FilterString(LocationId)}";
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
                response = _dao.DataTableToListObject<ClubMapDetailCommon>(dbResponse).ToList();
            return response;
        }
        #endregion
    }
}
