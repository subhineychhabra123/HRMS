using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
  public  class TimeSheetModel
    {
        public int TimeSheetId { get; set; }
        public int ActionItemId { get; set; }
        public int UserId { get; set; }
        public System.DateTime EntryDate { get; set; }
        public decimal TimeTaken { get; set; }
        public string Comment { get; set; }
        public int ProjectId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<bool> DeletedRecord { get; set; }
        public string Months { get; set; }
        public string MonthList { get; set; }
        public Nullable<int> Years { get; set; }
        public string TotalTime { get; set; }
        public int Date { get; set; }

        public int RunningMonth { get; set; }

    }
}
