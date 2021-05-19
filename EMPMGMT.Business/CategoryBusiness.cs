using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;
using EMPMGMT.Repository;
using EMPMGMT.Repository.Infrastructure.Contract;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business
{
    public class CategoryBusiness : ICategoryBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CategoryRepository categoryRepository;

        public CategoryBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            categoryRepository = new CategoryRepository(unitOfWork);
        }



        public List<CategoryModel> GetCategories(int companyId ,ref int totalRecords)
        {
            List<Category> categoryList = new List<Category>();
            List<CategoryModel> categoryModelList = new List<CategoryModel>();
            totalRecords = categoryRepository.Count(r => r.RecordDeleted == false);//r.CompanyId == companyId &&
            categoryList = categoryRepository.GetAll(r => r.RecordDeleted == false).OrderByDescending(x => x.CreatedDate).ToList();//r.CompanyId == companyId &&
            AutoMapper.Mapper.Map(categoryList, categoryModelList);
            return categoryModelList;
        }

        public List<CategoryModel> GetAllCategories(ListingParameters listingParameters, ref int totalRecords)
        {
            List<Category> categoryList = new List<Category>();
            List<CategoryModel> categoryModelList = new List<CategoryModel>();
            totalRecords = categoryRepository.Count(r =>  r.RecordDeleted == false); //r.CompanyId == listingParameters.CompanyId &&

            switch (listingParameters.OrderByColumn)
            {
                case "Title":
                    if (listingParameters.OrderBy == "asc")
                    {
                        categoryList = categoryRepository.GetAll(r => r.RecordDeleted == false).OrderBy(x => x.CategoryName).ToList();//r.CompanyId == listingParameters.CompanyId &&
                    }
                    else
                    {
                        categoryList = categoryRepository.GetAll(r => r.RecordDeleted == false).OrderByDescending(x => x.CategoryName).ToList();//r.CompanyId == listingParameters.CompanyId &&
                    }
                    break;
                default:
                    categoryList = categoryRepository.GetAll(r =>  r.RecordDeleted == false).OrderByDescending(x => x.CreatedDate).ToList();//r.CompanyId == listingParameters.CompanyId &&
                    break;
            }

            AutoMapper.Mapper.Map(categoryList, categoryModelList);
            return categoryModelList;
        }
        public bool AddCategory(CategoryModel categoryModel)
        {
            Category category = new Category();
            bool isExists = categoryRepository.Exists(r => r.CategoryName == categoryModel.CategoryName && r.RecordDeleted == false);//r.CompanyId == categoryModel.CompanyId &&
            if (!isExists)
            {
                category = new Category();
                AutoMapper.Mapper.Map(categoryModel, category);
                categoryRepository.Insert(category);
                return true;
            }
            else
            {
                return false;
            }
        }


        public string UpdateCategory(CategoryModel categoryModel)
        {

            var isExists = categoryRepository.Exists(r => r.CategoryName == categoryModel.CategoryName && r.CategoryId != categoryModel.CategoryId && r.RecordDeleted == false);//r.CompanyId == categoryModel.CompanyId &&
            if (isExists)
            {
                return "AlreadyExist";
            }
            else
            {
                Category category = categoryRepository.SingleOrDefault(r => r.CategoryId == categoryModel.CategoryId && r.RecordDeleted == false); // r.CompanyId == categoryModel.CompanyId &&
                if (category != null)
                {
                    category.CategoryName = categoryModel.CategoryName;
                    category.ModifiedDate = categoryModel.ModifiedDate;
                    category.ModifiedBy = categoryModel.ModifiedBy;
                    categoryRepository.Update(category);
                    return "Success";
                }
                return "Error";

            }
        }

        public bool DeleteCategory(int categoryId, int companyId)
        {
            try
            {
                int userid = (int)SessionManagement.LoggedInUser.UserId;
                Category objCategory = new Category();
                objCategory = categoryRepository.SingleOrDefault(r => r.CategoryId == categoryId && r.RecordDeleted == false); // r.CompanyId == companyId &&
                if (objCategory != null)
                {
                    objCategory.RecordDeleted = true;                  
                    categoryRepository.Update(objCategory);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
