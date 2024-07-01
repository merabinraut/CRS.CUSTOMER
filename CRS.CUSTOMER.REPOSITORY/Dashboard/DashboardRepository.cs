using CRS.CUSTOMER.SHARED.Dashboard;
using CRS.CUSTOMER.SHARED.DashboardV2;
using CRS.CUSTOMER.SHARED.SearchFilterManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CRS.CUSTOMER.REPOSITORY.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly RepositoryDao _dao;
        public DashboardRepository() => _dao = new RepositoryDao();

        public List<BannerCommon> GetBanners()
        {
            var bannerLists = new List<BannerCommon>();
            var sql = "Exec sp_customer_dashboard @Flag='bl'";
            var dbResp = _dao.ExecuteDataTable(sql);
            if (dbResp != null && dbResp.Rows.Count > 0)
            {
                foreach (DataRow dr in dbResp.Rows)
                {
                    bannerLists.Add(new BannerCommon()
                    {
                        BannerId = dr["BannerId"]?.ToString(),
                        BannerName = dr["BannerName"]?.ToString(),
                        BannerImage = dr["BannerImage"]?.ToString(),
                    });
                }
            }
            return bannerLists;
        }

        public List<LocationListCommon> GetLocationList()
        {
            var locationList = new List<LocationListCommon>();
            string sp_name = "EXEC sp_customer_dashboard @Flag='gll'";
            var dbResp = _dao.ExecuteDataTable(sp_name);
            if (dbResp != null && dbResp.Rows.Count > 0)
            {
                foreach (DataRow item in dbResp.Rows)
                {
                    locationList.Add(new LocationListCommon
                    {
                        LocationID = item["LocationId"].ToString(),
                        LocationURL = item["LocationURL"].ToString(),
                        LocationName = item["LocationName"].ToString(),
                        LocationImage = item["LocationImage"].ToString(),
                        LocationURl = item["LocationURL"].ToString(),
                        Latitude = item["Latitude"].ToString(),
                        Longitude = item["Longitude"].ToString(),
                        LocationDisplayName = item["LocationDisplayName"].ToString(),
                        LocationStatus = item["LocationStatus"].ToString(),
                    });
                }
            }
            return locationList;
        }

        public List<DashboardPlanCommon> GetPlansList()
        {
            string SQL = "EXEC sp_customer_dashboard @Flag='pd'";
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) return _dao.DataTableToListObject<DashboardPlanCommon>(dbResponse).ToList();
            return new List<DashboardPlanCommon>();
        }

        public List<DashboardRecommendedHostCommon> GetRecommendedHost()
        {
            string SQL = "EXEC sp_customer_dashboard @Flag='grh'";
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) return _dao.DataTableToListObject<DashboardRecommendedHostCommon>(dbResponse).ToList();
            return new List<DashboardRecommendedHostCommon>();
        }

        public List<DashboardRecommendedClubCommon> GetRecommendedClub()
        {
            string SQL = "EXEC sp_customer_dashboard @Flag='grc'";
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) return _dao.DataTableToListObject<DashboardRecommendedClubCommon>(dbResponse).ToList();
            return new List<DashboardRecommendedClubCommon>();
        }

        #region Recommendation Management
        public List<ClubRecommendationListCommon> GetRecommendedClub(string LocationId, string PageType = "")
        {
            var Response = new List<ClubRecommendationListCommon>();
            string SQL = "EXEC sproc_get_customer_recommendation @Flag = 'ghpcrvl'";
            SQL += ",@PageType=" + _dao.FilterString(PageType);
            SQL += ",@LocationId=" + _dao.FilterString(LocationId);
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) Response = _dao.DataTableToListObject<ClubRecommendationListCommon>(dbResponse).ToList();
            return Response;
        }

        public List<HostSearchManagementResponseCommon> GetRecommendedHost(string LocationId, string CustomerId = "")
        {
            var Response = new List<HostSearchManagementResponseCommon>();
            string SQL = "EXEC sproc_get_customer_recommendation @Flag = 'ghphrvl'";
            SQL += ",@LocationId=" + _dao.FilterString(LocationId);
            SQL += !string.IsNullOrEmpty(CustomerId) ? ",@CustomerId=" + _dao.FilterString(CustomerId) : string.Empty;
            var dbResponse = _dao.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) Response = _dao.DataTableToListObject<HostSearchManagementResponseCommon>(dbResponse).ToList();
            return Response;
        }
        #endregion        
    }
}