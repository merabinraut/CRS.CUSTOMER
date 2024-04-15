using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.CommonManagement;
using CRS.CUSTOMER.SHARED.Home;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.CommonManagement
{
    public interface ICommonManagementBusiness
    {
        Dictionary<string, string> GetDropDown(string Flag, string Extra1 = "", string Extra2 = "");
        List<StaticDataCommon> GetDDL(string Flag, string Extra1 = "", string Extra2 = "");
        List<StaticDataCommon> GetDropDown_V2(string Flag, string SearchField1 = "", string SearchField2 = "");
        List<PrivilegesCommon> GetCustomerPrivileges();
        #region System Links
        List<SystemLinkCommon> GetSystemLink();
        #endregion
    }
}
