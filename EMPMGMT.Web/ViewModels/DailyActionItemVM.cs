using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMPMGMT.Web.ViewModels
{
    public class DailyActionItemVM
    {
        public Nullable<int> Dates { get; set; }
        public string WorkHours { get; set; }
        public string ProjectName { get; set; }
        public string ItemName { get; set; }
        public string ProjectId { get; set; }
        public string ActionItemId { get; set; }

     //   public int ActionItemCommentId { get; set; }
        public string TimeSheetComment { get; set; }
        public string ActionItemComment { get; set; }
       // public string Title { get; set; }
       // public string ActionListId { get; set; }
        public string TimeSheetId { get; set; }
        public string SelectDate { get; set; }
        public Nullable<decimal> TimeTaken1 { get; set; }
   
    }
}