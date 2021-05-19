using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMPMGMT.Web.ViewModels
{
    public class ResourcesVM
    {
        public string ResourceId { get; set; }
        public string ProjectId { get; set; }
        public string UserId { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public string FullName { get; set; }
      
    }

}