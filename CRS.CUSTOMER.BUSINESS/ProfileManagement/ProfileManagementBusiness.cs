using CRS.CUSTOMER.REPOSITORY.ProfileManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ProfileManagement;
using System.Collections.Generic;

namespace CRS.CUSTOMER.BUSINESS.ProfileManagement
{
    public class ProfileManagementBusiness : IProfileManagementBusiness
    {
        private readonly IProfileManagementRepository _repo;

        public ProfileManagementBusiness() => this._repo = new ProfileManagementRepository();

        public CommonDbResponse ChangePassword(PasswordCommon passwordCommon)
        {
            return _repo.ChangePassword(passwordCommon);
        }

        public CommonDbResponse ChangeProfileImage(UserProfileCommon common)
        {
            return _repo.ChangeProfileImage(common);
        }

        public CommonDbResponse DeleteCustomerAccount(string customerId, string actionUser, string actionIp, string actionPlatform)
        {
            return _repo.DeleteCustomerAccount(customerId, actionUser, actionIp, actionPlatform);
        }

        public UserProfileCommon GetUserProfileDetail(UserProfileCommon userProfileCommon)
        {
            return _repo.GetUserProfileDetail(userProfileCommon);
        }

        public CommonDbResponse UpdateUserProfileDetail(UserProfileCommon userProfileCommon)
        {
            return _repo.UpdateUserProfileDetail(userProfileCommon);
        }
        public List<PointReportCommon> GetCustomerPointsReport(string customerId, string TxnType)
        {
            return _repo.GetCustomerPointsReport(customerId, TxnType);
        }
    }
}