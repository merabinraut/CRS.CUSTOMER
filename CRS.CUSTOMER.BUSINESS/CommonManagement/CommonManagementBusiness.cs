using CRS.CUSTOMER.REPOSITORY.CommonManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.CommonManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.CommonManagement
{
    public class CommonManagementBusiness : ICommonManagementBusiness
    {
        private ICommonManagementRepository _REPO;
        public CommonManagementBusiness()
        {
            _REPO = new CommonManagementRepository();
        }

        public List<StaticDataCommon> GetDDL(string Flag, string Extra1 = "", string Extra2 = "")
        {
            return _REPO.GetDDL(Flag, Extra1, Extra2);
        }

        public Dictionary<string, string> GetDropDown(string Flag, string Extra1 = "", string Extra2 = "")
        {
            return _REPO.GetDropDown(Flag, Extra1, Extra2);
        }

        public List<StaticDataCommon> GetDropDown_V2(string Flag, string SearchField1 = "", string SearchField2 = "")
        {
            return _REPO.GetDropDown_V2(Flag, SearchField1, SearchField2);
        }
        public List<PrivilegesCommon> GetCustomerPrivileges()
        {
            return _REPO.GetCustomerPrivileges();
        }
    }
}
