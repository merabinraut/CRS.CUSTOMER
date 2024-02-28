using CRS.CUSTOMER.APPLICATION.CustomValidations;
using CRS.CUSTOMER.APPLICATION.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRS.CUSTOMER.APPLICATION.Models.Home
{
    public class LoginRequestModel
    {
        [Display(Name = "MobileNumber", ResourceType = typeof(Resource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "This_field_is_required")]
        //[RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Only_numbers_are_allowed")]
        //[MinLength(11, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_11_characters")]
        //[MobileNumber(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Invalid_mobile_number")]
        public string LoginId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordRequired")]
        [Display(Name = "Password")]
        [MaxLength(16, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Maximum_length_is_16_characters")]
        //[MinLength(8, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_8_characters")]
        public string Password { get; set; }
        public string SessionId { get; set; }
        public string clubURL { get; set; }
        public string affiliateURL { get; set; }
    }
    public class LoginResponseModel
    {
        public string AgentId { get; set; }
        public string UserId { get; set; }
        public string NickName { get; set; }
        public string EmailAddress { get; set; }
        public string ProfileImage { get; set; }
        public string SessionId { get; set; }
        public string ActionDate { get; set; }
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