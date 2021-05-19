using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMPMGMT.Web.ViewModels
{
    public class ResetPasswordVM
    {

        [Required(ErrorMessage = "Password Required")]
        [Display(Name = "password")]
        
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Confirm Password Required")]
        [Display(Name = "cpassword")]

        [DataType(DataType.Password)]
        public string cpassword{ get; set; }

        public string UserId { get; set; }


    }
}