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
    
    public partial class InterviewDetail
    {
        public InterviewDetail()
        {
            this.EmployeeSkill = new HashSet<EmployeeSkill>();
            this.InterviewDetailSkill = new HashSet<InterviewDetailSkill>();
        }
    
        public int InterviewDetailId { get; set; }
        public Nullable<int> InterviewerId { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> CandidateId { get; set; }
        public Nullable<System.DateTime> ScheduledDate { get; set; }
        public Nullable<int> InterviewStatus { get; set; }
    
        public virtual ICollection<EmployeeSkill> EmployeeSkill { get; set; }
        public virtual ICollection<InterviewDetailSkill> InterviewDetailSkill { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
