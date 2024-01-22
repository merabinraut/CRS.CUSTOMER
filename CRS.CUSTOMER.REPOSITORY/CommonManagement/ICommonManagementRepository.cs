using CRS.CUSTOMER.SHARED;
using System.Collections.Generic;

namespace CRS.CUSTOMER.REPOSITORY.CommonManagement
{
    public interface ICommonManagementRepository
    {
        Dictionary<string, string> GetDropDown(string Flag, string Extra1 = "", string Extra2 = "");
        List<StaticDataCommon> GetDDL(string Flag, string Extra1 = "", string Extra2 = "");
    }
}
