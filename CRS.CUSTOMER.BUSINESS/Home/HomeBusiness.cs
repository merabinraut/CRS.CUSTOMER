using CRS.CUSTOMER.REPOSITORY.Home;
using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.Home;

namespace CRS.CUSTOMER.BUSINESS.Home
{
    public class HomeBusiness : IHomeBusiness
    {
        IHomeRepository _REPO;
        public HomeBusiness(HomeRepository REPO)
        {
            _REPO = REPO;
        }

        public CommonDbResponse ForgotPassword(ForgotPasswordCommon common)
        {
            return _REPO.ForgotPassword(common);
        }

        public CommonDbResponse ForgotPasswordOTP(RegistrationCommon common)
        {
            return _REPO.ForgotPasswordOTP(common);
        }

        public CommonDbResponse Login(LoginRequestCommon Request)
        {
            return _REPO.Login(Request);
        }

        #region Registration Management
        public CommonDbResponse Register(RegistrationCommon Request)
        {
            return _REPO.Register(Request);
        }

        public CommonDbResponse RegistrationHold(RegistrationHoldCommon Request)
        {
            return _REPO.RegistrationHold(Request);
        }

        public CommonDbResponse ResendRegistrationOTP(ResendRegistrationOTPCommon Request)
        {
            return _REPO.ResendRegistrationOTP(Request);
        }

        public CommonDbResponse SetNewPassword(SetRegistrationPasswordCommon common)
        {
            return _REPO.SetNewPassword(common);
        }

        public CommonDbResponse SetRegistrationPassword(SetRegistrationPasswordCommon Request)
        {
            return _REPO.SetRegistrationPassword(Request);
        }

        public ReferralModelCommon ValidateReferralCode(ReferralModelCommon referCommon)
        {
            return _REPO.ValidateReferralCode(referCommon);
        }

        public CommonDbResponse VerifyAffiliateOtp(RegistrationCommon common)
        {
            return _REPO.VerifyAffiliateOtp(common);
        }

        public CommonDbResponse ResendForgotPasswordOTP(ResendRegistrationOTPCommon Request)
        {
            return _REPO.ResendForgotPasswordOTP(Request);
        }
        #endregion
    }
}
