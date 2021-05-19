using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMPMGMT.Web.ViewModels
{
    public class ActionItemResponsibleVM
    {
        public string ActionItemResponsibleId { get; set; }
        public string ActionItemId { get; set; }
        public string ResponsibleUserId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public string ActionItemName { get; set; }
        public string ResponsibleUserName { get; set; }
        public int StatusDrop { get; set; }
    }
}