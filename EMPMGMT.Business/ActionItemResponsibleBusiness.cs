using EMPMGMT.Repository;
using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;
using EMPMGMT.Repository;
using EMPMGMT.Repository.Infrastructure.Contract;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace EMPMGMT.Business
{
    public class ActionItemResponsibleBusiness : IActionItemResponsibleBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ActionItemResponsibleRepository actionItemResponsibleRepository;

        public ActionItemResponsibleBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            actionItemResponsibleRepository = new ActionItemResponsibleRepository(unitOfWork);

        }

        //public bool DeleteActionList(ActionListModel actionListModel)
        //{
        //    ActionList objMetricDashboard = actionListRepository.SingleOrDefault(r => r.ActionListId == actionListModel.ActionListId && r.CompanyId == actionListModel.CompanyId && r.RecordDeleted == false);
        //    if (objMetricDashboard != null)
        //    {
        //        objMetricDashboard.RecordDeleted = actionListModel.RecordDeleted;
        //        objMetricDashboard.ModifiedBy = actionListModel.ModifiedBy;
        //        objMetricDashboard.ModifiedDate = actionListModel.ModifiedDate;
        //        actionListRepository.Update(objMetricDashboard);
        //        return true;
        //    }
        //    return false;

        //}
        public void SaveActionItemResponsible(List<ActionItemResponsibleModel> listActionItemResponsibleModel)
        {
            int userId=SessionManagement.LoggedInUser.UserId;
            List<ActionItemResponsibleModel> actionItemResponsibleModelList = new List<ActionItemResponsibleModel>();
            List<ActionItemResponsible> listActionItemResponsible = new List<ActionItemResponsible>();
            ActionItemResponsible objActionItemResponsible = new ActionItemResponsible();
          //  AutoMapper.Mapper.Map(actionItemResponsibleModelList, listActionItemResponsible);
            

            foreach (ActionItemResponsibleModel lstModel in listActionItemResponsibleModel)
            {
                objActionItemResponsible = new ActionItemResponsible();
                ActionItemResponsible isExists = actionItemResponsibleRepository.SingleOrDefault(r => r.ActionItemId == lstModel.ActionItemId && r.ResponsibleUserId == lstModel.ResponsibleUserId);
                if (isExists == null )
                {
                    AutoMapper.Mapper.Map(lstModel, objActionItemResponsible);
                    actionItemResponsibleRepository.Insert(objActionItemResponsible);
                  
                }
                else
                {
                    AutoMapper.Mapper.Map(lstModel, isExists);
                    isExists.ModifiedBy = userId;
                    isExists.ModifiedDate = DateTime.UtcNow;
                    isExists.RecordDeleted = lstModel.RecordDeleted;
                    actionItemResponsibleRepository.Update(isExists);
                }
            }

          //  AutoMapper.Mapper.Map(actionItemResponsibleModelList, listActionItemResponsible);
           // actionItemResponsibleRepository.InsertAll(listActionItemResponsible);


        }

        public List<ActionItemResponsibleModel> GetActionItemResponsibleByActionItem(int ActionItemId)
        {
            List<ActionItemResponsibleModel> actionItemResponsibleModelList = new List<ActionItemResponsibleModel>();
            List<ActionItemResponsible> listActionItemResponsible = actionItemResponsibleRepository.GetAll(x => x.ActionItemId == ActionItemId && x.RecordDeleted == false).ToList();
            AutoMapper.Mapper.Map(listActionItemResponsible, actionItemResponsibleModelList);
            return actionItemResponsibleModelList;

        }
    }
}
