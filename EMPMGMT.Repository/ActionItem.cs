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
    
    public partial class ActionItem
    {
        public ActionItem()
        {
            this.ActionItemComment = new HashSet<ActionItemComment>();
            this.ActionItemResponsible = new HashSet<ActionItemResponsible>();
            this.FileAttachments = new HashSet<FileAttachments>();
            this.TimeSheet = new HashSet<TimeSheet>();
        }
    
        public int ActionItemId { get; set; }
        public Nullable<int> ParentActionItemId { get; set; }
        public Nullable<int> ActionListId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<System.DateTime> ETA { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> IsArchived { get; set; }
        public Nullable<int> Priority { get; set; }
        public bool IsSendEmailNotification { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public Nullable<decimal> Minutes { get; set; }
        public Nullable<System.DateTime> StatusDate { get; set; }
        public Nullable<int> StatusDrop { get; set; }
    
        public virtual ActionList ActionList { get; set; }
        public virtual ICollection<ActionItemComment> ActionItemComment { get; set; }
        public virtual ICollection<ActionItemResponsible> ActionItemResponsible { get; set; }
        public virtual ICollection<FileAttachments> FileAttachments { get; set; }
        public virtual ICollection<TimeSheet> TimeSheet { get; set; }
    }
}
