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
    public class ActionListBussiness : IActionListBussiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ActionListRepository actionListRepository;
        private readonly ActionItemRepository actionItemRepository;
        private readonly ActionItemResponsibleRepository actionItemResponsibleRepository;
        private readonly UserRepository userRepository;

        public ActionListBussiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            actionListRepository = new ActionListRepository(unitOfWork);
            userRepository = new UserRepository(unitOfWork);
            actionItemRepository = new Repository.ActionItemRepository(unitOfWork);
            actionItemResponsibleRepository = new Repository.ActionItemResponsibleRepository(unitOfWork);
        }

        public List<ActionListModel> GetActionListing(ListingParameters listingParameters, out int totalRecords)
        {
            List<ActionListModel> actionListingModel = new List<ActionListModel>();
            List<ActionList> actionListing = new List<ActionList>();
            Expression<Func<ActionList, bool>> whereCondition = x => x.Title.Contains(listingParameters.SearchText == null ? x.Title : listingParameters.SearchText) && x.RecordDeleted == false; //&& x.CompanyId == listingParameters.CompanyId
            //Expression<Func<ActionList, bool>> whereCondition = x => x.Title.Contains(listingParameters.SearchText == null ? x.Title : listingParameters.SearchText);
            totalRecords = actionListRepository.Count(x => x.Title.Contains(listingParameters.SearchText == null ? x.Title : listingParameters.SearchText) && x.ProjectId == listingParameters.ProjectId.Decrypt() && x.RecordDeleted == false); //x.CompanyId == listingParameters.CompanyId &&
            //totalRecords = actionListRepository.Count(x => x.Title.Contains(listingParameters.SearchText == null ? x.Title : listingParameters.SearchText));
            
            switch (listingParameters.OrderByColumn)
            {
                case "Title":
                    if (listingParameters.OrderBy == "asc")
                    {
                        actionListing = actionListRepository.GetPagedRecords(whereCondition, y => y.Title, listingParameters.CurrentPage, listingParameters.PageSize).ToList();
                    }
                    else
                    {
                        actionListing = actionListRepository.GetPagedRecordsDecending(whereCondition, y => y.Title, listingParameters.CurrentPage, listingParameters.PageSize).ToList();
                    }
                    break;
                case "Objective":
                    if (listingParameters.OrderBy == "asc")
                    {
                        actionListing = actionListRepository.GetPagedRecords(whereCondition, y => y.Objective, listingParameters.CurrentPage, listingParameters.PageSize).ToList();
                    }
                    else
                    {
                        actionListing = actionListRepository.GetPagedRecordsDecending(whereCondition, y => y.Objective, listingParameters.CurrentPage, listingParameters.PageSize).ToList();
                    }
                    break;
                case "ResponsibleUserName":
                    if (listingParameters.OrderBy == "asc")
                    {
                        actionListing = actionListRepository.GetPagedRecords(whereCondition, y => y.Employee.FirstName, y => y.Employee.LastName, listingParameters.CurrentPage, listingParameters.PageSize).ToList();
                    }
                    else
                    {
                        actionListing = actionListRepository.GetPagedRecordsDecending(whereCondition, y => y.Employee.FirstName, listingParameters.CurrentPage, listingParameters.PageSize).ToList();
                    }
                    break;
                default:
                         actionListing = actionListRepository.GetPagedRecords(whereCondition, y => y.ActionListId, listingParameters.CurrentPage, listingParameters.PageSize).ToList();
                    break;
            }

            AutoMapper.Mapper.Map(actionListing, actionListingModel);
            return actionListingModel;
        }
        public List<ActionListModel> GetActionListForAutoComplete(int CompanyId, string searchText)
        {
            List<ActionListModel> actionListModel = new List<ActionListModel>();
            List<ActionList> actionList = actionListRepository.GetAll(x => x.Title.Contains(searchText) && x.RecordDeleted != true).ToList();//x.CompanyId == CompanyId &&
            AutoMapper.Mapper.Map(actionList, actionListModel);
            return actionListModel;
        }
        public ActionListModel GetActionListDetail(ActionListModel actionListModel)
        {
            ActionList objActionList = actionListRepository.SingleOrDefault(u => u.ActionListId == actionListModel.ActionListId &&  u.RecordDeleted == false);
            ActionListModel objActionListModel = new ActionListModel();
            if (objActionList == null)
            {
                ActionItem objActionItem = actionItemRepository.SingleOrDefault(x => x.ActionItemId == actionListModel.ActionListId && x.RecordDeleted == false);
                if (objActionItem != null)
                {
                    objActionList = actionListRepository.SingleOrDefault(u => u.ActionListId == objActionItem.ActionListId && u.RecordDeleted == false);
                }
            }
            AutoMapper.Mapper.Map(objActionList, objActionListModel);
        
           // objActionList.FileAttachments = objActionList.FileAttachments.Where(x => x.RecordDeleted == false).ToList();
          
            
            return objActionListModel;
        }
        public int SaveActionList(ActionListModel actionListModel)
        {
            ActionList actionList = new ActionList();
            bool isExists = actionListRepository.Exists(r => r.Title == actionListModel.Title && r.ProjectId == actionListModel.ProjectId  && r.RecordDeleted == false); //r.CompanyId == actionListModel.CompanyId &&
            if (!isExists)
            {
                actionList = new ActionList();
               
                AutoMapper.Mapper.Map(actionListModel, actionList);
                actionList.ModifiedDate = DateTime.UtcNow;
                actionList.CreatedDate = DateTime.UtcNow;
                actionListRepository.Insert(actionList);
                return actionList.ActionListId;
            }
            else
            {
                return 0;
            }

        }
        public string UpdateActionList(ActionListModel actionListModel)
        {

            var isExists = actionListRepository.Exists(r => r.Title == actionListModel.Title && r.ActionListId != actionListModel.ActionListId && r.ProjectId == actionListModel.ProjectId && r.RecordDeleted == false); // r.CompanyId == actionListModel.CompanyId &&
            if (isExists)
            {
                return "AlreadyExist";
            }
            else
            {
                ActionList actionListObj = actionListRepository.SingleOrDefault(r => r.ActionListId == actionListModel.ActionListId && r.ProjectId == actionListModel.ProjectId && r.RecordDeleted == false); //r.CompanyId == actionListModel.CompanyId &&
                if (actionListObj != null)
                {
                    actionListObj.Title = actionListModel.Title;
                    actionListObj.Description = actionListModel.Description;
                    actionListObj.Objective = actionListModel.Objective;
                    actionListObj.ModifiedBy = actionListModel.ModifiedBy;
                    actionListObj.ModifiedDate =  DateTime.UtcNow;
                    actionListObj.Status = actionListModel.Status;
                    actionListObj.RiskIssues = actionListModel.RiskIssues;
                    actionListObj.ResponsibleUserId = actionListModel.ResponsibleUserId;
                    actionListRepository.Update(actionListObj);
                    return "Success";
                }
                return "Error";

            }
        }
        public bool DeleteActionList(ActionListModel actionListModel)
        {
            ActionList objMetricDashboard = actionListRepository.SingleOrDefault(r => r.ActionListId == actionListModel.ActionListId && r.RecordDeleted == false); // r.CompanyId == actionListModel.CompanyId &&
            if (objMetricDashboard != null)
            {
                objMetricDashboard.RecordDeleted = actionListModel.RecordDeleted;
                objMetricDashboard.ModifiedBy = actionListModel.ModifiedBy;
                objMetricDashboard.ModifiedDate = actionListModel.ModifiedDate;
                objMetricDashboard.ActionItem.ToList().ForEach(x => { x.RecordDeleted = true; });
             
                actionListRepository.Update(objMetricDashboard);
                return true;
            }
            return false;

        }

        public List<ActionListModel> GetActionList(ListingParameters listingParameters, int LogedInUserId, ref int totalRecords)
        {
            int currentPage = listingParameters.CurrentPage;
            int pageSize = listingParameters.PageSize;
            List<ActionListModel> listActionListModel = new List<ActionListModel>();
            List<ActionList> listActionList = new List<ActionList>();
            ActionListModel objUserModel = new ActionListModel();
            Employee objUsers = new Employee();
            int projectId = listingParameters.ProjectId.Decrypt();

            List<SSP_GetActionListDetails_Result> getactionList = new List<Repository.SSP_GetActionListDetails_Result>();

           getactionList=actionListRepository.GetActionListDetails(projectId, listingParameters.SearchText, LogedInUserId, currentPage, pageSize, listingParameters.OrderByColumn, listingParameters.OrderBy);
           if (getactionList.Count() > 0)
               totalRecords = getactionList[0].TotalCount.Value;

           AutoMapper.Mapper.Map(getactionList, listActionListModel);
            //Testing and Confirm Purpose/////
        /*   List<int> objItemList = actionItemResponsibleRepository.GetAll(x=>x.ResponsibleUserId==LogedInUserId && x.RecordDeleted==false).Select(x=>x.ActionItemId).ToList();
            List<int> objActionList = actionItemRepository.GetAll(x => objItemList.Contains(x.ActionItemId) && x.RecordDeleted==false).Select(x=>x.ActionListId.Value).ToList();
            ////////////////////////////////////////////////

            Expression<Func<ActionList, bool>> whereCondition = x => (x.Title.Contains(listingParameters.SearchText == null ? x.Title : listingParameters.SearchText)) && x.ProjectId == projectId && x.RecordDeleted == false && (objActionList.Contains(x.ActionListId) || x.CreatedBy == LogedInUserId || x.ModifiedBy == LogedInUserId || x.ResponsibleUserId == LogedInUserId);//x.CompanyId == listingParameters.CompanyId &&
            totalRecords = actionListRepository.Count(whereCondition);

            switch (listingParameters.OrderByColumn)
            {
                case "Title":
                    if (listingParameters.OrderBy == "desc")
                    {
                        listActionList = actionListRepository.GetPagedRecordsDecending(whereCondition, y => y.Title, currentPage, pageSize).ToList();
                    }
                    else
                    {
                        listActionList = actionListRepository.GetPagedRecords(whereCondition, y => y.Title, currentPage, pageSize).ToList();
                    }
                    break;
                case "Description":
                    if (listingParameters.OrderBy == "desc")
                    {
                        listActionList = actionListRepository.GetPagedRecordsDecending(whereCondition, y => y.Description, currentPage, pageSize).ToList();
                    }
                    else
                    {
                        listActionList = actionListRepository.GetPagedRecords(whereCondition, y => y.Description, currentPage, pageSize).ToList();
                    }
                    break;
                case "ResponsibleUserName":
                    if (listingParameters.OrderBy == "desc")
                    {
                        listActionList = actionListRepository.GetPagedRecordsDecending(whereCondition, y => y.Employee.FirstName, x => x.Employee.LastName, currentPage, pageSize).ToList();
                    }
                    else
                    {
                        listActionList = actionListRepository.GetPagedRecords(whereCondition, y => y.Employee.FirstName, x => x.Employee.LastName, currentPage, pageSize).ToList();
                    }
                    break;
                case "Status":
                    if (listingParameters.OrderBy == "desc")
                    {
                        listActionList = actionListRepository.GetPagedRecords(whereCondition, y => y.Status.ToString(), currentPage, pageSize).ToList();
                    }
                    else
                    {
                        listActionList = actionListRepository.GetPagedRecordsDecending(whereCondition, y => y.Status.ToString(), currentPage, pageSize).ToList();
                    }
                    break;
                default:
                    listActionList = actionListRepository.GetPagedRecordsDecending(whereCondition, y => y.CreatedDate, currentPage, pageSize).ToList();
                    break;
            }
            AutoMapper.Mapper.Map(listActionList, listActionListModel);*/
            return listActionListModel;
        }


        public List<ActionListModel> GetProjectActionListing(int ProjectId)
        {
            List<ActionListModel> actionListingModel = new List<ActionListModel>();
            List<ActionList> actionListing = new List<ActionList>();

            List<ActionList> actionList = actionListRepository.GetAll(x => x.ProjectId == ProjectId && x.RecordDeleted != true).ToList();

            AutoMapper.Mapper.Map(actionList, actionListingModel);
           
            return actionListingModel;
        }
        public ActionListModel ProjectActionList(int ProjectId, int UserId)
        {
            ActionListModel actionListModel=new ActionListModel();
            ActionList actionList = actionListRepository.SingleOrDefault(x => x.ProjectId == ProjectId && x.ResponsibleUserId== UserId && x.RecordDeleted == false);
            AutoMapper.Mapper.Map(actionList, actionListModel);
            return actionListModel;

        }
    }

}
