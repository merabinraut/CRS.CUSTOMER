using System.Collections.Generic;
using System.Reflection;

namespace CRS.CUSTOMER.SHARED.Home
{
    #region Registration Management
    public class RegistrationHoldCommon : Common
    {
        public string NickName { get; set; }
        public string MobileNumber { get; set; }
    }

    public class RegistrationCommon : Common
    {
        public string AgentId { get; set; }
        public string MobileNumber { get; set; }
        public string VerificationCode { get; set; }
        public string ReferCode { get; set; }
    }

    public class ResendRegistrationOTPCommon : Common
    {
        public string AgentId { get; set; }
        public string MobileNumber { get; set; }
    }

    public class SetRegistrationPasswordCommon : Common
    {
        public string AgentId { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string IsPasswordForceful { get; set; }
    }
    #endregion

    #region Login
    public class LoginRequestCommon : Common
    {
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string SessionId { get; set; }
    }
    public class LoginResponseCommon
    {
        public string AgentId { get; set; }
        public string UserId { get; set; }
        public string NickName { get; set; }
        public string EmailAddress { get; set; }
        public string ProfileImage { get; set; }
        public string SessionId { get; set; }
        public string ActionDate { get; set; }
        public string IsPasswordForceful { get; set; }
        public string MobileNumber { get; set; }
        public List<SystemLinkCommon> SystemLink { get; set; } = new List<SystemLinkCommon>();
    }

    public class SystemLinkCommon
    {
        public string PlatformName { get; set; }
        public string Link { get; set; }
        public string OrderPosition { get; set; }
        public string PlatformImage { get; set; }
    }
    #endregion

    #region Forgot Password
    public class ForgotPasswordCommon : Common
    {
        public string MobileNo { get; set; }
        public string CreatedPlatform { get; set; }
        public string Username { get; set; }
    }
    #endregion

    #region "Referral Model"
    public class ReferralModelCommon : Common
    {
        public string ReferCode { get; set; }
    }
    #endregion
}