using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMPMGMT.Web.ViewModels
{
    public class DesignationVM
    {
        public string DesignationId { get; set; }
        public string DesignationName { get; set; }
        public Nullable<int> CompanyId { get; set; }
    
    }
}