using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.CommonManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.REPOSITORY.CommonManagement
{
    public interface ICommonManagementRepository
    {
        Dictionary<string, string> GetDropDown(string Flag, string Extra1 = "", string Extra2 = "");
        List<StaticDataCommon> GetDDL(string Flag, string Extra1 = "", string Extra2 = "");
        List<StaticDataCommon> GetDropDown_V2(string Flag, string SearchField1 = "", string SearchField2 = "");
        List<PrivilegesCommon> GetCustomerPrivileges();
    }
}
