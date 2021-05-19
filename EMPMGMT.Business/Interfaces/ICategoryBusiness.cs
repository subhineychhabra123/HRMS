using EMPMGMT.Domain;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business.Interfaces
{
   public  interface ICategoryBusiness
    {
        List<CategoryModel> GetCategories(int companyId, ref int totalRecords);
        List<CategoryModel> GetAllCategories(ListingParameters listingParameters, ref int totalRecords);
        bool AddCategory(CategoryModel categoryModel);
        string UpdateCategory(CategoryModel categoryModel);
        bool DeleteCategory(int categoryId, int companyId);
    }
}
