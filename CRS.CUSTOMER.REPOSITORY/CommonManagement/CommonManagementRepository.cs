using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.CommonManagement;
using CRS.CUSTOMER.SHARED.Enquiry;
using CRS.CUSTOMER.SHARED.Home;
using DocumentFormat.OpenXml.Office2016.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace CRS.CUSTOMER.REPOSITORY.CommonManagement
{
    public class CommonManagementRepository : ICommonManagementRepository
    {
        private RepositoryDao _DAO;
        public CommonManagementRepository()
        {
            _DAO = new RepositoryDao();
        }

        public Dictionary<string, string> GetDropDown(string Flag, string Extra1 = "", string Extra2 = "")
        {
            Dictionary<string, string> response = new Dictionary<string, string>();
            string SQL = "EXEC sproc_dropdown_management ";
            SQL += "@Flag=" + _DAO.FilterString(Flag);
            SQL += ",@SearchField1=" + _DAO.FilterString(Extra1);
            SQL += ",@SearchField2=" + _DAO.FilterString(Extra2);
            var dbResponse = _DAO.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                foreach (DataRow item in dbResponse.Rows)
                    response.Add(item["Value"].ToString(), item["Text"].ToString());
            }
            return response;
        }
        public List<StaticDataCommon> GetDDL(string Flag, string Extra1 = "", string Extra2 = "")
        {
            string SQL = "EXEC sproc_dropdown_management ";
            SQL += "@Flag=" + _DAO.FilterString(Flag);
            SQL += ",@SearchField1=" + _DAO.FilterString(Extra1);
            SQL += ",@SearchField2=" + _DAO.FilterString(Extra2);
            var dbResponse = _DAO.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0) return _DAO.DataTableToListObject<StaticDataCommon>(dbResponse).ToList();
            return new List<StaticDataCommon>();
        }

        public List<StaticDataCommon> GetDropDown_V2(string Flag, string SearchField1 = "", string SearchField2 = "")
        {
            string SQL = $"EXEC sproc_dropdown_management_v2 @Flag={_DAO.FilterString(Flag)}, @SearchField1={_DAO.FilterString(SearchField1)},@SearchField2={_DAO.FilterString(SearchField2)}";
            var dbResponse = _DAO.ExecuteDataTable(SQL);
            return dbResponse != null && dbResponse.Rows.Count > 0
                ? _DAO.DataTableToListObject<StaticDataCommon>(dbResponse).ToList()
                : new List<StaticDataCommon>();
        }

        public List<PrivilegesCommon> GetCustomerPrivileges()
        {
            string SQL = $"EXEC sproc_get_function @Flag = 'gcfl'";
            var dbResponse = _DAO.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
                return _DAO.DataTableToListObject<PrivilegesCommon>(dbResponse).ToList();
            return new List<PrivilegesCommon>();
        }
        #region System Links
        public List<SystemLinkCommon> GetSystemLink()
        {
            string SQL = "EXEC sproc_system_links_management";
            var dbResponse = _DAO.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
                return _DAO.DataTableToListObject<SystemLinkCommon>(dbResponse).ToList();
            return new List<SystemLinkCommon>();
        }
        #endregion

        public string GetForceFulLogout(string UserId)
        {
            string isforcefullogout = null;
            string SQL = "SELECT dbo.fn_IsForceFulLogout (";
            SQL += _DAO.FilterString(UserId) + " )";
            var dbResponse = _DAO.ExecuteDataTable(SQL);
            if (dbResponse != null && dbResponse.Rows.Count > 0)
            {
                isforcefullogout = Convert.ToString(dbResponse.Rows[0][0]);
            }
            return Convert.ToString(isforcefullogout);
        }

        public Tuple<int, int> GetMetaTagInfo(string type, string locationId = "", string clubId = "")
        {
            var response = new Tuple<int, int>(0, 0);
            string SQL = $"EXEC sproc_get_meta_tag_info @Flag= {type}";
            SQL += !string.IsNullOrEmpty(locationId) ? $", @LocationId={locationId}" : string.Empty;
            SQL += !string.IsNullOrEmpty(clubId) ? $", @AgentId={clubId}" : string.Empty;
            var dbResponse = _DAO.ExecuteDataRow(SQL);
            if (dbResponse != null)
            {
                int Item1 = _DAO.ParseColumnValue(dbResponse, "Item1") is int value1 ? value1 : 0;
                int Item2 = _DAO.ParseColumnValue(dbResponse, "Item2") is int value2 ? value2 : 0;
                response = new Tuple<int, int>(Item1, Item2);
            }
            return response;
        }

        public CommonDbResponse PostEnquiryAsync(EnquiryRequestcommon request)
        {
            string SQL = "EXEC sproc_customer_request_enquiry";
            SQL += " @emailAddress=" + _DAO.FilterString(request.EmailAddress);
            SQL += !string.IsNullOrEmpty(request.Message)?",@message=N" + _DAO.FilterString(request.Message) : ",@message=" + _DAO.FilterString(request.Message);    
            return _DAO.ParseCommonDbResponse(SQL);
        }
        public List<AdvertisementCommon> GetAdvertisement()
        {
            var advertisementManagement = new List<AdvertisementCommon>();
            var sql = "Exec sproc_advertisement_management @Flag='s'";
            var dt = _DAO.ExecuteDataTable(sql);

            if (dt != null)
            {
                foreach (DataRow item in dt.Rows)
                {                
                    var advertisement = new AdvertisementCommon();
                    advertisement.image = item["ImgPath"].ToString();
                    advertisement.link = item["Link"].ToString();
                    advertisement.status = item["Status"].ToString();
                    advertisementManagement.Add(advertisement);
                }
            }
            return advertisementManagement;
        }


    }
}
