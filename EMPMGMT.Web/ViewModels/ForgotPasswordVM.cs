using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMPMGMT.Web.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Login.EmailRequired")]
        [Display(Name = "Email Id")]
        [EmailAddress(ErrorMessage = "Login.InvalidEmailAddress")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }
    }
}