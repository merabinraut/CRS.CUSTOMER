using CRS.CUSTOMER.REPOSITORY.ProfileManagement;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.ProfileManagement;

namespace CRS.CUSTOMER.BUSINESS.ProfileManagement
{
    public class ProfileManagementBusiness : IProfileManagementBusiness
    {
        private readonly IProfileManagementRepository _repo;

        public ProfileManagementBusiness(ProfileManagementRepository profileManagementRepository) => this._repo = profileManagementRepository;

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
    }
}