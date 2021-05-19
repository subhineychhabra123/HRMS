using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMPMGMT.Web.ViewModels
{
    public class CompanyVM
    {
        public string CompanyId { get; set; }
        [Required(ErrorMessage = "Company Name is required")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Created by")]
        public string CreatedBy { get; set; }
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }
        [Display(Name = "Status")]
        public string BreakThroughObjectiveYear
        {
            get;
            set;
        }
        public bool IsActive { get; set; }
    }
}