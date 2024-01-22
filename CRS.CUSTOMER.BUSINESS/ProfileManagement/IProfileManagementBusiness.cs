using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ProfileManagement;

namespace CRS.CUSTOMER.BUSINESS.ProfileManagement
{
    public interface IProfileManagementBusiness
    {
        UserProfileCommon GetUserProfileDetail(UserProfileCommon userProfileCommon);
        CommonDbResponse ChangePassword(PasswordCommon passwordCommon);
        CommonDbResponse UpdateUserProfileDetail(UserProfileCommon userProfileCommon);
        CommonDbResponse ChangeProfileImage(UserProfileCommon common);
        CommonDbResponse DeleteCustomerAccount(string customerId, string actionUser, string actionIp, string actionPlatform);
    }
}