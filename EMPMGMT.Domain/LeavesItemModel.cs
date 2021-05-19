using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
  public  class LeavesItemModel
    {
        public int LeaveId { get; set; }
        public int TypeId { get; set; }
        public int EmpId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? ApprovedStatus { get; set; }
        public int ApprovedById { get; set; }
        public int ApprovedLeaveId { get; set; }
        public string Reason { get; set; }
        public int FullDay { get; set; }
        public int FirstHalf { get; set; }
        public DateTime? FromDateForFirstHalf { get; set; }
        public DateTime? FromDateForSecondHalf { get; set; }
        public int SecondHalf { get; set; }
        public string ReasonForFirstHalf { get; set; }
        public string ReasonForSecondHalf { get; set; }
        public int shortleave { get; set; }
        public DateTime? FromDateForshortleave { get; set; }
        public string ReasonForshortleave { get; set; }
        public string Comp { get; set; }
        public string FirstName { get; set; }
        public string UserEmail { get; set; }
        public int UserId { get; set; }
        public string  UserName { get; set; }
    }
}
