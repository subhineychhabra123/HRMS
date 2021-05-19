using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMPMGMT.Web.ViewModels
{
    public class ActionListVM
    {
        public string ActionListId { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Title")]
        public string Title { get; set; }
        public string ResponsibleUserId { get; set; }
        public string Description { get; set; }
        public string Objective { get; set; }
        public string RiskIssues { get; set; }
        public int Status { get; set; }
        public string StatusImagePath { get; set; }
        public int CompanyId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> RecordDeleted { get; set; }
        [Required(ErrorMessage = "Responsible is required")]
        [Display(Name = "ResponsibleUserName")]
        public string ResponsibleUserName { get; set; }
        
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }

        public string TotalWorkTime { get; set; }

        public string ImagePath
        {
            get
            {
                return Constants.CONTENT_IMAGES_PATH;
            }
        }
        public string StatusName
        {
            get
            {
                // return  Enum.GetName(typeof(Utility.Enums.DataType),DataType);
                return EnumHelper.GetDescription((Utility.Enums.ActionListStatus)Status);
            }


        }
        //public List<FileAttachmentsVM> FileAttachments { get; set; }
        public string Url { get; set; }
    }
}