using CRS.CUSTOMER.APPLICATION.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRS.CUSTOMER.APPLICATION.Models.Home
{
    public class LoginRequestModel
    {
        [Display(Name = "MobileNumber", ResourceType = typeof(Resource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Enter_your_mobile_phone_number_or_nickname")]
        //[RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Only_numbers_are_allowed")]
        //[MinLength(11, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_11_characters")]
        //[MobileNumber(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Invalid_mobile_number")]
        public string LoginId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Please_enter_your_password")]
        [Display(Name = "Password")]
        [MaxLength(32, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Maximum_length_is_32_characters")]
        //[MinLength(8, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_8_characters")]
        public string Password { get; set; }
        public string SessionId { get; set; }
        public string clubURL { get; set; }
        public string ReturnURL { get; set; }
        public string affiliateURL { get; set; }
    }
    public class LoginResponseModel
    {
        public string AgentId { get; set; }
        public string UserId { get; set; }
        public string NickName { get; set; }
        public string IsPasswordForceful { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string ProfileImage { get; set; }
        public string SessionId { get; set; }
        public string ActionDate { get; set; }
        public int Amount { get; set; }
        public List<SystemLinkModel> SystemLink { get; set; } = new List<SystemLinkModel>();
    }
    public class SystemLinkModel
    {
        public string PlatformName { get; set; }
        public string Link { get; set; }
        public string OrderPosition { get; set; }
        public string PlatformImage { get; set; }
    }
}