using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
   public  class DailyActionItemsModel
    {

        public Nullable<int> Dates { get; set; }
        public string WorkHours { get; set; }
        public string ProjectName { get; set; }
        public string ItemName { get; set; }
        public int ProjectId { get; set; }
        public int ActionItemId { get; set; }

        public int ActionItemCommentId { get; set; }
        public string TimeSheetComment { get; set; }
        public string ActionItemComment { get; set; }
       public string Title { get; set; }
       public int ActionListId { get; set; }
    
       public int TimeSheetId { get; set; }
       public string SelectDate { get; set; }
       public Nullable <decimal> TimeTaken1 { get; set; }
   
    }
}
