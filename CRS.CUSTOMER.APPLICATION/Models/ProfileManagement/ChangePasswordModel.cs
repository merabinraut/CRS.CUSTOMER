using CRS.CUSTOMER.APPLICATION.Resources;
using System.ComponentModel.DataAnnotations;

namespace CRS.CUSTOMER.APPLICATION.Models.ProfileManagement
{
    public class ChangePasswordModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        //[RegularExpression(@"^.*(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$", ErrorMessage = "Must be of length 8 to 16 and must contain a-z,A-Z,0-9,@#$%^&+=")]
        //[RegularExpression("^.{8,16}$", ErrorMessage = "Must be of length 8 to 16")]
        public string OldPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        //[RegularExpression(@"^.*(?=.{8,32})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$", ErrorMessage = "Must be of length 8 to 32 and must contain a-z,A-Z,0-9,@#$%^&+=")]
        //[RegularExpression("^.{8,16}$", ErrorMessage = "Must be of length 8 to 16")]
        [RegularExpression(@"^.*(?=.{8,32})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$", ErrorMessage = "Must be of length 8 to 32 and must contain a-z, A-Z, and 0-9")]
        public string NewPassword { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Resource))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Compare("NewPassword", ErrorMessage = "Password  Mismatch")]
        //[RegularExpression(@"^.*(?=.{8,16})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$", ErrorMessage = "Must be of length 8 to 16 and must contain a-z,A-Z,0-9,@#$%^&+=")]
        //[RegularExpression("^.{8,16}$", ErrorMessage = "Must be of length 8 to 16")]
        public string ConfirmPassword { get; set; }
    }
}