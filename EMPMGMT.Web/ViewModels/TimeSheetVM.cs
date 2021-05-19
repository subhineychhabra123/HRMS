using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EMPMGMT.Web.ViewModels
{
    public class TimeSheetVM
    {
        public string TimeSheetId { get; set; }
       
        [Required(ErrorMessage="Task Name is required")]
        public string ActionItemId { get; set; }
        
       
        [Required(ErrorMessage = " Time is required")]
        public decimal TimeTaken { get; set; }
       
        public string Comment { get; set; }
       
        [Required(ErrorMessage = "Project is required")]
        public string ProjectId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<bool> DeletedRecord { get; set; }
        public string Months { get; set; }
        public string MonthList { get; set; }
        public Nullable<int> Years { get; set; }
        public string TotalTime { get; set; }
        public int Date { get; set; }
        public int FullDate { get; set; }
        public string UserId { get; set; }
        public DateTime EntryDate { get; set; }

        public int ErrorStatus { get; set; }

        public int RunningMonth { get; set; }
    }
}