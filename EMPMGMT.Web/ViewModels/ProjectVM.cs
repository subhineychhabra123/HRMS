using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMPMGMT.Web.ViewModels
{
    public class ProjectVM
    {
        public string ProjectId { get; set; }
          [Required(ErrorMessage = "Project Name is Required")]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Project Code is Required")]
        public string ProjectCode { get; set; }
         //  [Required(ErrorMessage = "Project Description is Required")]
        public string ProjectDescription { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0: MM/dd/yyyy}", ApplyFormatInEditMode=true)]
        public Nullable<System.DateTime> StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}", ApplyFormatInEditMode=true)]
        public Nullable<System.DateTime> EndDate { get; set; }
          // [Required(ErrorMessage = "Client Email Address is Required")]
        [EmailAddress(ErrorMessage="Please Enter Valid Email Address")]
        public string CommunicationEmailId { get; set; }
         //  [Required(ErrorMessage = "Client User Password is Required")]
        public string CommunicationEmailPassword { get; set; }
       //    [Required(ErrorMessage = "Project Location Address is Required")]
        public string SourceControlDetail { get; set; }
        public string ProjectUrl { get; set; }
        public Nullable<int> Status { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public Nullable<bool> RecordDeleted { get; set; }        
        public string ProjectLead { get; set; }
          [Required(ErrorMessage = "Project Lead is Required")]
        public string FullName { get; set; }
        public List<ResourcesVM> ListResources { get; set; }
        public string GblprojectLead { get; set; }
        public Nullable<decimal> TIMECONSUMED { get; set; }
        public string StatusName
        {
            get
            {
                // return  Enum.GetName(typeof(Utility.Enums.DataType),DataType);
                return EnumHelper.GetDescription((Utility.Enums.ActionListStatus)Status);
            }

        }

        public string ErrorProjectName { get; set; }
        public string ErrorProjectCode { get; set; }

        public virtual ICollection<ResourcesVM> Resources { get; set; }




     
        
    }
}