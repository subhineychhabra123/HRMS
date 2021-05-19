using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
  public class ActionItemResponsibleModel
    {
        public int ActionItemResponsibleId { get; set; }
        public int ActionItemId { get; set; }
        public int ResponsibleUserId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public EmployeeModel userModel { get; set; }
        public int StatusDrop { get; set; }
        private ActionItemModel _actionItemModel { get; set; }

        public ActionItemModel ActionItemModel
        {
            get
            {
                if (this._actionItemModel == null)
                    this._actionItemModel = new ActionItemModel();
                return this._actionItemModel;
            }
            set { this._actionItemModel = value; }
        }
    }
}
