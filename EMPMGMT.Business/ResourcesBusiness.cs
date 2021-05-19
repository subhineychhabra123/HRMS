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
    public class ResourcesBusiness : IResourcesBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ResourcesRepository resourcesRepository;

        public ResourcesBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            resourcesRepository = new ResourcesRepository(unitOfWork);

        }




        public void SaveResources(List<ResourcesModel> listResourcesModel)
        {
           // int userId = SessionManagement.LoggedInUser.UserId;
            List<ResourcesModel> actionItemResponsibleModelList = new List<ResourcesModel>();
            List<Resources> listActionItemResponsible = new List<Resources>();

            if (listResourcesModel.Count > 0)
            {
                int Projectid = listResourcesModel[0].ProjectId.Value;
                listActionItemResponsible = resourcesRepository.GetAll(x => x.ProjectId == Projectid && x.UserId != 662).ToList();
                listActionItemResponsible.ForEach(x => x.RecordDeleted = true);
                resourcesRepository.UpdateAll(listActionItemResponsible);
            }
            
            Resources objResources = null;           
            foreach (ResourcesModel lstModel in listResourcesModel)
            {
                objResources = new Resources();
                Resources isExists = resourcesRepository.SingleOrDefault(r => r.UserId == lstModel.UserId && r.ProjectId == lstModel.ProjectId); //r.RecordDeleted = false && r.ResourceId == lstModel.ResourceId && 
                if (isExists == null)
                {
                   // AutoMapper.Mapper.Map(lstModel, objResources);
                    if (lstModel.UserId > 0)                    {
                        objResources.UserId = lstModel.UserId;
                        objResources.CreatedBy = lstModel.CreatedBy;
                        objResources.ProjectId = lstModel.ProjectId;
                        objResources.CreatedDate = DateTime.UtcNow;                       
                        objResources.RecordDeleted = false;
                        resourcesRepository.Insert(objResources);
                    }
                }
                else
                {
                   // isExists = resources.Where(x => x.ResourceId == isExists.ResourceId).SingleOrDefault();
                   // lstModel.ResourceId = isExists.ResourceId;                 
                  //  isExists.ModifiedBy = userId;   
                    isExists.ModifiedBy = lstModel.ModifiedBy;                    
                    isExists.ModifiedDate = DateTime.UtcNow;
                    isExists.RecordDeleted = false;
                    resourcesRepository.Update(isExists);
                }
            }
        }


        public List<ResourcesModel> GetActionItemResponsibleByActionItem(int projectId)
        {
            List<ResourcesModel> resourcesModellist = new List<ResourcesModel>();
            List<Resources> listResources = resourcesRepository.GetAll(x => x.ProjectId == projectId && x.RecordDeleted == false).ToList();
            AutoMapper.Mapper.Map(listResources, resourcesModellist);
            return resourcesModellist;

        }


        public void DeleteResource(ResourcesModel resourcesModel)
        {
            Resources resources = new Resources();

            if (resourcesModel.UserId > 0)
            {
                resources = resourcesRepository.SingleOrDefault(x => x.UserId == resourcesModel.UserId && x.ProjectId==resourcesModel.ProjectId && x.RecordDeleted != true);
                if (resources != null)
                {
                    resources.RecordDeleted = true;
                    resources.ModifiedBy = resourcesModel.ModifiedBy;
                    resources.ModifiedDate = DateTime.UtcNow;
                    resourcesRepository.Update(resources);
                }
            }
        }
    }
}
