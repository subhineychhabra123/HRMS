using EMPMGMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMPMGMT.Web.ViewModels
{
    public class CategoryVM
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CompanyId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }


        //public List<CategoryModel> listCategory { get; set; }
    }
}