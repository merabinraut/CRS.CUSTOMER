using CRS.CUSTOMER.SHARED;
using CRS.CUSTOMER.SHARED.Home;

namespace CRS.CUSTOMER.BUSINESS.Home
{
    public interface IHomeBusiness
    {
        #region Registration Management
        CommonDbResponse RegistrationHold(RegistrationHoldCommon Request);
        CommonDbResponse ResendRegistrationOTP(ResendRegistrationOTPCommon Request);
        CommonDbResponse Register(RegistrationCommon Request);
        CommonDbResponse SetRegistrationPassword(SetRegistrationPasswordCommon Request);
        #endregion
        CommonDbResponse Login(LoginRequestCommon Request);
        CommonDbResponse VerifyAffiliateOtp(RegistrationCommon common);

        #region Forgot Password
        CommonDbResponse ForgotPassword(ForgotPasswordCommon common);
        CommonDbResponse ForgotPasswordOTP(RegistrationCommon common);
        CommonDbResponse SetNewPassword(SetRegistrationPasswordCommon common);
        ReferralModelCommon ValidateReferralCode(ReferralModelCommon referCommon);
        CommonDbResponse ResendForgotPasswordOTP(ResendRegistrationOTPCommon Request);
        #endregion
    }
}
