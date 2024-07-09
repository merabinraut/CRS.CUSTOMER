using Aspose.Cells;
using CRS.CUSTOMER.APPLICATION.CustomValidations;
using CRS.CUSTOMER.APPLICATION.Resources;
using CRS.CUSTOMER.SHARED;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CRS.CUSTOMER.APPLICATION.Models.Home
{
    #region Registration Management
    public class RegistrationHoldModel
    {
        [Display(Name = "Nickname", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Please_enter_a_nickname")]
        //[RegularExpression("^\\p{L}\\p{N}._\\-]*$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Only_numbers_are_allowed")]
        [MinLength(4, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_4_characters")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Maximum_length_is_20_characters")]
        public string NickName { get; set; }

        [Display(Name = "MobileNumber", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Please_enter_your_mobile_phone_number")]
        [MinLength(11, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_11_characters")]
        [RegularExpression("^(?![eE])[0-9]*$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Only_numbers_are_allowed")]
        //[MobileNumber(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Invalid_mobile_number")]
        //[RegularExpression(@"^(090|6490|3254)\d{8}$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Invalid_mobile_number")]
        public string MobileNumber { get; set; }
    }

    public class RegistrationModel
    {
        public string AgentId { get; set; }
        public string MobileNumber { get; set; }
        public string ReferCode { get; set; }
        public string Type { get; set; }
        public string NickName { get; set; }
        //public string OTP1 { get; set; }
        //public string OTP2 { get; set; }
        //public string OTP3 { get; set; }
        //public string OTP4 { get; set; }
        //public string OTP5 { get; set; }
        //public string OTP6 { get; set; }
        public string OTPCode { get; set; }
    }

    public class ResendRegistrationOTPModel
    {
        public string AgentId { get; set; }
        public string MobileNumber { get; set; }
    }

    public class SetRegistrationPasswordModel
    {
        public string AgentId { get; set; }
        public string UserId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        //[MaxLength(32, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Maximum_length_is_16_characters")]
        [MaxLength(32, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Maximum_length_is_32_characters")]
        //[RegularExpression(@"^.*(?=.{8,32})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$", ErrorMessage = "Must be of length 8 to 32 and must contain a-z,A-Z,0-9,@#$%^&+=")]
        //[RegularExpression(@"^.*(?=.{8,32})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$", ErrorMessage = "Must be of length 8 to 32 and must contain a-z, A-Z, and 0-9")]
        //[MinLength(8, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_8_characters")]
        [RegularExpression(@"^.*(?=.{8,32})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$", ErrorMessage = "Must be of length 8 to 32 and must contain a-z, A-Z, and 0-9")]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [MaxLength(32, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Maximum_length_is_32_characters")]
        //[RegularExpression(@"^.*(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$", ErrorMessage = "Must be of length 8 to 16 and must contain a-z,A-Z,0-9,@#$%^&+=")]
        //[MinLength(8, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_8_characters")]
        [Compare("Password", ErrorMessage = "Password  Mismatch")]
        public string ConfirmPassword { get; set; }
        public string MobileNumber { get; set; }
        public string NickName { get; set; }
        public string IsPasswordForceful { get; set; }
    }
    #endregion
    #region Forgot Password
    public class ForgotPasswordModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Please_enter_your_mobile_phone_number")]
        [MinLength(11, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_11_characters")]
        [RegularExpression("^(?![eE])[0-9]*$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Only_numbers_are_allowed")]

        //[MobileNumber(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Invalid_mobile_number")]
        //[Display(Name = "MobileNumber", ResourceType = typeof(Resource))]
        public string MobileNo { get; set; }
        //[Display(Name = "Nickname", ResourceType = typeof(Resource))]
        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "This_field_is_required")]
        //[MinLength(1, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_1_characters")]
        //[MaxLength(30, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Maximum_length_is_30_characters")]
        //public string Username { get; set; }
        public string CaptureTime { get; set; }
    }
    #endregion

    #region "Referral Model"
    public class ReferralModel : Common
    {
        public string ReferCode { get; set; }
    }
    #endregion
}