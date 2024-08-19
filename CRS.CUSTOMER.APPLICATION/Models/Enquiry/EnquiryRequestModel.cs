
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace CRS.CUSTOMER.APPLICATION.Models.Enquiry
{
    public  class EnquiryRequestModel
    {
        [Required(ErrorMessage ="Email_address_is_required!")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage ="Message_is_required!")]
        public string Message { get; set; }
    }
}