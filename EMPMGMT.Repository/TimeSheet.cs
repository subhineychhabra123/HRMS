//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EMPMGMT.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class TimeSheet
    {
        public int ActionItemId { get; set; }
        public int UserId { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public Nullable<decimal> TimeTaken { get; set; }
        public string Comment { get; set; }
        public int ProjectId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<bool> DeletedRecord { get; set; }
        public int TimeSheetId { get; set; }
    
        public virtual Project Project { get; set; }
        public virtual ActionItem ActionItem { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
