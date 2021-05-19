using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace EMPMGMT.Web.ViewModels
{
    public class ActionItemVM: ITreeNode<ActionItemVM>
    {
        public string ActionItemId { get; set; }
        public string ParentActionItemId { get; set; }
        public string ActionListId { get; set; }
        public string RCAId { get; set; }
        public string ItemName { get; set; }
        public string ResponsibleUserId { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public bool IsArchived { get; set; }
        public int Status { get; set; }
        public int StatusDrop { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> DueDate { get; set; }
        public Nullable<System.DateTime> ETA { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> RecordDeleted { get; set; }
        public string ResponsibleUserName { get; set; }
        public String ResponsiblesFullName {get;set;}

        public string RCATitle { get; set; }
        public bool IsSendEmailNotification { get; set; }
        public List<ActionItemResponsibleVM> ListActionItemResponsible { get; set; }
        public bool HasChild { get; set; }
        public bool FetchChild { get; set; }
        public Nullable<int> Childern { get; set; }
        public DateTime MaxDueDate { get; set; }
        public DateTime MinStartDate { get; set; }
        public Nullable<System.DateTime> ActionListMinStartDate { get; set; }
        public Nullable<System.DateTime> ActionListMaxDueDate { get; set; }
        public string Title { get; set; }
        public decimal ActionItemStatus { get; set; }
        public decimal Minutes { get; set; }
        public Nullable<System.DateTime> StatusDate { get; set; }
    


        public bool OnTime { get; set; }
        public bool OverDue { get; set; }
        public bool BeforeDue { get; set; }
        public string SearchText { get; set; }

        public string ObjectId { get; set; }
        public string ParentObjectId { get; set; }
        public List<ActionItemVM> Children { get; set; }
        public string MaxMinutes { get; set; }


        public int Underlevel { get; set; }
        public int child { get; set; }
        public decimal ActualTime { get; set; }
       public string DocumentName{get; set;}
       public string FileName { get; set; }

        public int MinStartDatesDiff
        {

            get
            {

                return (Convert.ToDateTime(this.MinStartDate) - Convert.ToDateTime(this.ActionListMinStartDate)).Days;
            }
        }
        public int MaxDueDatesDiff
        {

            get
            {

                return  (Convert.ToDateTime(this.ActionListMaxDueDate) - Convert.ToDateTime(this.MaxDueDate)).Days;

            }
        }
        public int ActionListCurrentDateDiff
        {

            get
            {

                return (DateTime.Now - Convert.ToDateTime(this.ActionListMinStartDate)).Days;
            }
        }

        public int ActionListMaxMinDatesTotalDiff
        {

            get
            {

                return (Convert.ToDateTime(this.ActionListMaxDueDate) - Convert.ToDateTime(this.ActionListMinStartDate)).Days;
            }
        }

        public string PriorityDescription
        {
            get
            {
                return EnumHelper.GetDescription((Utility.Enums.ActionTaskPriority)Priority);
            }
        }


    }

}