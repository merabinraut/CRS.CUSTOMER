using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ProfileManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.REPOSITORY.ProfileManagement
{
    public interface IProfileManagementRepository
    {
        UserProfileCommon GetUserProfileDetail(UserProfileCommon userProfileCommon);
        CommonDbResponse ChangePassword(PasswordCommon passwordCommon);
        CommonDbResponse UpdateUserProfileDetail(UserProfileCommon userProfileCommon);
        CommonDbResponse ChangeProfileImage(UserProfileCommon common);
        CommonDbResponse DeleteCustomerAccount(string customerId, string actionUser, string actionIp, string actionPlatform);
        List<PointReportCommon> GetCustomerPointsReport(string customerId, string TxnType);
    }
}