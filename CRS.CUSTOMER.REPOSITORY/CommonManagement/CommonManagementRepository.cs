using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.CommonManagement;
using CRS.CUSTOMER.SHARED.Home;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
    }
}
