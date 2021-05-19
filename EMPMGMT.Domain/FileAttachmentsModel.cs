using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
    public class FileAttachmentsModel
    {
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public string ActionListPath { get; set; }
        public int AttachedBy { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> ActionItemId { get; set; }
        public int ActionListId { get; set; }

        public EmployeeModel UserModel { get; set; }

    }
}
