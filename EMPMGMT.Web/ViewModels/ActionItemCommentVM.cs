using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMPMGMT.Web.ViewModels
{
    public class ActionItemCommentVM
    {
        public string ActionItemCommentId { get; set; }
        public string Comment { get; set; }
        public string ActionItemId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> RecordDeleted { get; set; }

    }
}