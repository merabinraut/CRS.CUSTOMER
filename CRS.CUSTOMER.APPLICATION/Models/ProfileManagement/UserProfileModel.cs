using CRS.CUSTOMER.APPLICATION.Resources;
using System.ComponentModel.DataAnnotations;

namespace CRS.CUSTOMER.APPLICATION.Models.UserProfileManagement
{
    public class UserProfileModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "This_field_is_required")]
        [MinLength(1, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_1_characters")]
        [MaxLength(30, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Maximum_length_is_30_characters")]
        public string NickName { get; set; }

      /*  [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "This_field_is_required")]
        [MinLength(1, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_1_characters")]
        [MaxLength(30, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Maximum_length_is_30_characters")]*/
        public string FirstName { get; set; }

       /* [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "This_field_is_required")]
        [MinLength(1, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_1_characters")]
        [MaxLength(30, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Maximum_length_is_30_characters")]*/
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string DateOfBirth { get; set; }

        /*[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "This_field_is_required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessageResourceType = typeof(Resource),
            ErrorMessageResourceName = "Invalid_Email")]*/
        public string EmailAddress { get; set; }

       /* [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "This_field_is_required")]*/
        public string Gender { get; set; }
        public string PreferredLocation { get; set; }
        /*[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "This_field_is_required")]
        [RegularExpression(@"^\d{3}-\d{4}$", ErrorMessage = "無効な郵便番号")]*/
        public string PostalCode { get; set; }
        public string Prefecture { get; set; }

      /*  [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "This_field_is_required")]
        [MinLength(1, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_1_characters")]
        [MaxLength(30, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Maximum_length_is_30_characters")]*/
        public string City { get; set; }

       /* [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "This_field_is_required")]
        [MinLength(1, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_1_characters")]
        [MaxLength(30, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Maximum_length_is_30_characters")]*/
        public string Street { get; set; }

       /* [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "This_field_is_required")]
        [MinLength(1, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Minimum_length_is_1_characters")]
        [MaxLength(30, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Maximum_length_is_30_characters")]*/
        public string ResidenceNumber { get; set; }
        public string ProfileImage { get; set; }
        public string Session { get; set; }

        /*[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "This_field_is_required")]
        [StringLength(6, MinimumLength = 4, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Length_should_be_4_characters")]*/
        public string DOBYear { get; set; }

       /* [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "This_field_is_required")]
        [StringLength(4, MinimumLength = 2, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Length_should_be_2_characters")]*/
        public string DOBMonth { get; set; }

       /* [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "This_field_is_required")]
        [StringLength(4, MinimumLength = 2, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Length_should_be_2_characters")]*/
        public string DOBDay { get; set; }
    }
}