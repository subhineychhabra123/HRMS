using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
    public class ActionItemModel
    {
        public int ActionItemId { get; set; }
        public int ParentActionItemId { get; set; }
        public int ActionListId { get; set; }
        public Nullable<int> RCAId { get; set; }
        public Nullable<System.DateTime> ETA { get; set; }
        public string ItemName { get; set; }
        public int ResponsibleUserId { get; set; }
        public List<ActionItemResponsibleModel> Responsible { get; set; }
        public bool IsSendEmailNotification { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public bool IsArchived { get; set; }
        public int Status { get; set; }
        public int StatusDrop { get; set; }
        
        public bool FetchChild { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> RecordDeleted { get; set; }
        public EmployeeModel userModel { get; set; }

        public bool HasChild { get; set; }
        public Nullable<int> Childern { get; set; }
        public DateTime MaxDueDate { get; set; }
        public DateTime MinStartDate { get; set; }
        public string ResponsibleUserName { get; set; }
        public string ResponsiblesFullName { get; set; }
        public Nullable<System.DateTime> ActionListMinStartDate { get; set; }
        public Nullable<System.DateTime> ActionListMaxDueDate { get; set; }
        public string Title { get; set; }
        public decimal ActionItemStatus { get; set; }
        public decimal Minutes { get; set; }
        public Nullable<System.DateTime> StatusDate { get; set; }

        public string MaxMinutes { get; set; }
        public int Underlevel { get; set; }
        public int child { get; set; }
        public decimal ActualTime { get; set; }


        private ActionListModel _actionListModel;
        public ActionListModel ActionListModel
        {
            get
            {
                if (this._actionListModel == null)
                    this._actionListModel = new ActionListModel();
                return this._actionListModel;
            }
            set { this._actionListModel = value; }
        }

    }
}
