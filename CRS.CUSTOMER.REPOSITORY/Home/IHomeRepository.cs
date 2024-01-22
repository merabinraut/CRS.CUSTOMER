using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.Home;

namespace CRS.CUSTOMER.REPOSITORY.Home
{
    public interface IHomeRepository
    {
        #region Registration Management
        CommonDbResponse RegistrationHold(RegistrationHoldCommon Request);
        CommonDbResponse ResendRegistrationOTP(ResendRegistrationOTPCommon Request);
        CommonDbResponse Register(RegistrationCommon Request);
        CommonDbResponse SetRegistrationPassword(SetRegistrationPasswordCommon Request);
        #endregion
        CommonDbResponse Login(LoginRequestCommon Request);
        #region Forgot Password
        CommonDbResponse ForgotPassword(ForgotPasswordCommon common);
        CommonDbResponse ForgotPasswordOTP(RegistrationCommon common);
        CommonDbResponse SetNewPassword(SetRegistrationPasswordCommon common);
        ReferralModelCommon ValidateReferralCode(ReferralModelCommon referCommon);
        CommonDbResponse VerifyAffiliateOtp(RegistrationCommon common);
        CommonDbResponse ResendForgotPasswordOTP(ResendRegistrationOTPCommon Request);
        #endregion
    }
}
