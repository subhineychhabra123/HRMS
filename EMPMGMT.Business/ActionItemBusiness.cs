using EMPMGMT.Repository;
using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;
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
    public class ActionItemBusiness : IActionItemBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ActionItemRepository actionItemRepository;
        private readonly ActionListRepository actionlistRepository;
        private readonly ActionItemResponsibleRepository actionitemResponsibleRepository;

        public ActionItemBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            actionItemRepository = new ActionItemRepository(unitOfWork);
            actionlistRepository = new Repository.ActionListRepository(unitOfWork);
            actionitemResponsibleRepository= new Repository.ActionItemResponsibleRepository (unitOfWork);

        }
        public ActionItemModel GetActionItemDescriptionByActionItemId(int actionItemId)
        {
            ActionItemModel actionItemModel = new ActionItemModel();
            SSP_GetActionItemDescription_Result actionItemDescription = new SSP_GetActionItemDescription_Result();
            actionItemDescription = actionItemRepository.GetActionItemDescriptionOfActionItem(actionItemId).FirstOrDefault();
            AutoMapper.Mapper.Map(actionItemDescription, actionItemModel);
            return actionItemModel;
        }
        public int UpdateActionItemRecord(ActionItemModel actionItemModel)
        {
            var isExists = actionItemRepository.Exists(r => r.ItemName == actionItemModel.ItemName && r.ActionItemId != actionItemModel.ActionItemId && r.RecordDeleted == false);

            if (isExists)
            {
                return 0;
            }
            else
            {
                ActionItem actionItemObj = actionItemRepository.SingleOrDefault(r => r.ActionItemId == actionItemModel.ActionItemId && r.RecordDeleted == false);
                if (actionItemObj != null)
                {
                    actionItemObj.IsArchived = actionItemModel.IsArchived;
                    actionItemObj.IsSendEmailNotification = actionItemModel.IsSendEmailNotification;
                    actionItemObj.ItemName = actionItemModel.ItemName;
                    actionItemObj.Description = actionItemModel.Description;
                    actionItemObj.StartDate = actionItemModel.StartDate == Convert.ToDateTime("1/1/0001") ? null : actionItemModel.StartDate;
                    actionItemObj.ModifiedBy = actionItemModel.ModifiedBy;
                    actionItemObj.ModifiedDate = DateTime.UtcNow;
                    actionItemObj.Status = actionItemModel.Status;
                    actionItemObj.StatusDrop = actionItemModel.StatusDrop;
                    actionItemObj.DueDate = actionItemModel.DueDate == Convert.ToDateTime("1/1/0001") ? null : actionItemModel.DueDate; 
                    actionItemObj.ETA = actionItemModel.ETA;
                    actionItemObj.Priority = actionItemModel.Priority;
                    actionItemObj.Minutes = actionItemModel.Minutes;
                    if (actionItemObj.Status == 100)
                    {
                        actionItemObj.StatusDate = DateTime.UtcNow;
                    }
                    actionItemRepository.Update(actionItemObj);
                    return actionItemModel.ActionItemId;
                }
                return 0;//**

            }
        }

        public List<ActionItemModel> GetActionItemListFromActionId(int? ActionListId, bool OnTime, bool OverDue, bool BeforeDue, string SearchText, DateTime? StartDate, DateTime? DueDate, int StatusDrop, int ResponsibleUserId) //, int UserId
        {
            DateTime  maxActionListDate, minActionListDate;
            int actualTime;
            List<ActionItemModel> listActionItemModel = new List<ActionItemModel>();
            List<ActionItemModel> listActionItemModelCopy = new List<ActionItemModel>();
            List<ActionItem> listActionItem = new List<ActionItem>();
            List<SSP_GetActionItemsFromActionListId_Result> listGetActionItemsOfActionListResult = new List<SSP_GetActionItemsFromActionListId_Result>();
            listGetActionItemsOfActionListResult = actionItemRepository.GetActionItemsFromActionListId(ActionListId, OnTime, OverDue, BeforeDue, SearchText, StartDate, DueDate, StatusDrop, ResponsibleUserId).ToList(); //, UserId
         
           // List<SSP_GetActionItemsFromActionListId_Mtest_Result> listGetActionItemsOfActionListResultTest = new List<SSP_GetActionItemsFromActionListId_Mtest_Result>();
           // listGetActionItemsOfActionListResultTest = actionItemRepository.GetActionItemsFromActionListId_Test(ActionListId, OnTime, OverDue, BeforeDue, SearchText, StartDate, DueDate, StatusDrop, ResponsibleUserId).ToList(); //, UserId
            AutoMapper.Mapper.Map(listGetActionItemsOfActionListResult, listActionItemModel);
            
           // AutoMapper.Mapper.Map(listGetActionItemsOfActionListResultTest, listActionItemModel);
            minActionListDate = listActionItemModel.Count != 0 ? listActionItemModel.Min(x => x.MinStartDate) : default(DateTime);
            maxActionListDate = listActionItemModel.Count != 0 ? listActionItemModel.Max(x => x.MaxDueDate) : default(DateTime);
            
            listActionItemModel.ForEach(x => { x.ActionListMinStartDate = minActionListDate; x.ActionListMaxDueDate = maxActionListDate; });
            return listActionItemModel;
        }





        public void DeleteActionItem(ActionItemModel actionItemModel)
        {
            actionItemRepository.DeleteActionItem(actionItemModel.ActionItemId);
        }
        public int SaveActionItemRecord(ActionItemModel actionItemModel)
        {
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            var isExists = actionItemRepository.Exists(r => r.ItemName == actionItemModel.ItemName && r.ActionListId==actionItemModel.ActionListId && r.ActionList.RecordDeleted == false && r.RecordDeleted == false);//&& r.ActionList.CompanyId == companyId
            if (!isExists)
            {
                ActionItem actionItem = new ActionItem();
                AutoMapper.Mapper.Map(actionItemModel, actionItem);
                actionItem.ActionListId = actionItem.ActionListId == 0 ? null : actionItem.ActionListId;
                actionItem.StartDate = actionItemModel.StartDate == Convert.ToDateTime("1/1/0001") ? null : actionItem.StartDate;
                actionItem.DueDate = actionItemModel.DueDate == Convert.ToDateTime("1/1/0001") ? null : actionItem.DueDate; ;
                actionItemRepository.Insert(actionItem);
                return actionItem.ActionItemId;
            }
            else
            {
                return 0;
            }
        }
        public List<ActionItemModel> GetActionItemListByACtionListId(ActionItemModel actionItemModel)
        {
            List<ActionItemModel> listActionItemModel = new List<ActionItemModel>();
            List<ActionItem> listActionItem = actionItemRepository.GetAll(x => x.ActionListId == actionItemModel.ActionListId && x.RecordDeleted == false).ToList();
            AutoMapper.Mapper.Map(listActionItem, listActionItemModel);
            return listActionItemModel;
        }
        public List<ActionItemModel> GetActionItemList(ListingParameters listingParameters, int ActionListId, ref int totalRecords)
        {
            int currentPage = listingParameters.CurrentPage;
            int pageSize = listingParameters.PageSize;
            List<ActionItemModel> listActionItemModel = new List<ActionItemModel>();
            List<ActionItem> listActionItem = new List<ActionItem>();
            ActionItemModel objUserModel = new ActionItemModel();
            Employee objUsers = new Employee();

            Expression<Func<ActionItem, bool>> whereCondition = x => (x.ItemName.Contains(listingParameters.SearchText == null ? x.ItemName : listingParameters.SearchText)) && x.ActionListId == ActionListId && x.RecordDeleted == false;
            totalRecords = actionItemRepository.Count(whereCondition);

            switch (listingParameters.OrderByColumn)
            {
                case "Title":
                    if (listingParameters.OrderBy == "desc")
                    {
                        listActionItem = actionItemRepository.GetPagedRecordsDecending(whereCondition, y => y.ItemName, currentPage, pageSize).ToList();
                    }
                    else
                    {
                        listActionItem = actionItemRepository.GetPagedRecords(whereCondition, y => y.ItemName, currentPage, pageSize).ToList();
                    }
                    break;
                case "DueDate":
                    if (listingParameters.OrderBy == "desc")
                    {
                        listActionItem = actionItemRepository.GetPagedRecordsDecending(whereCondition, y => y.Description, currentPage, pageSize).ToList();
                    }
                    else
                    {
                        listActionItem = actionItemRepository.GetPagedRecords(whereCondition, y => y.Description, currentPage, pageSize).ToList();
                    }
                   break;
                //case "ResponsibleUserName":
                //    if (listingParameters.OrderBy == "desc")
                //    {
                //        listActionItem = actionItemRepository.GetPagedRecordsDecending(whereCondition, y => y.objUsers.FirstName, x => x.User.LastName, currentPage, pageSize).ToList();
                //    }
                //    else
                //    {
                //        listActionItem = actionItemRepository.GetPagedRecords(whereCondition, y => y.objUsers.FirstName, x => x.User.LastName, currentPage, pageSize).ToList();
                //    //}
                //    break;

                case "MaxMinutes":
                   if (listingParameters.OrderBy == "desc")
                   {
                       listActionItem = actionItemRepository.GetPagedRecordsDecending(whereCondition, y => y.Minutes, currentPage, pageSize).ToList();
                   }
                   else
                   {
                       listActionItem = actionItemRepository.GetPagedRecords(whereCondition, y => y.Description, currentPage, pageSize).ToList();
                   }
                   break;

                default:
                    listActionItem = actionItemRepository.GetPagedRecordsDecending(whereCondition, y => y.CreatedDate, currentPage, pageSize).ToList();
                    break;
            }

            AutoMapper.Mapper.Map(listActionItem, listActionItemModel);
            return listActionItemModel;
        }


        #region TimeSheetDropdown Fill

        public List<ActionItemModel> ActionItemList()
        {
            List<ActionItemModel> listActionItemModel = new List<ActionItemModel>();
            List<ActionItem> listActionItem = new List<ActionItem>();              
            listActionItem = actionItemRepository.GetAll().ToList();
            AutoMapper.Mapper.Map(listActionItem, listActionItemModel);
            return listActionItemModel;
        }


        public List<ActionItemModel> ActionItemList(int projectId, int UserId)
        {
            List<ActionItemModel> listActionItemModel = new List<ActionItemModel>();
            List<ActionItem> listActionItem = new List<ActionItem>();

            List<SSP_GetActionItemsforDropdwonforProjectId_Result> actionListforDropdown = new List<SSP_GetActionItemsforDropdwonforProjectId_Result>();

            actionListforDropdown= actionItemRepository.GetActionItemsForDropdownProjectId(projectId, UserId);

            AutoMapper.Mapper.Map(actionListforDropdown, listActionItemModel);


            //List<int> objActionListIds = actionlistRepository.GetAll(x => x.ProjectId == projectId && x.RecordDeleted == false).Select(a => a.ActionListId).ToList(); //&& x.ResponsibleUserId==UserId
            //List<int> listActionItemResponsible = actionitemResponsibleRepository.GetAll(a => a.ResponsibleUserId == UserId && a.RecordDeleted == false || (a.CreatedBy == UserId || a.ModifiedBy==UserId)).Select(a => a.ActionItemId).ToList();
            //if (listActionItemResponsible.Count() > 0 && objActionListIds.Count() > 0) //objActionListIds.Count() > 0 &&
            //{
            //    // listActionItem = actionItemRepository.GetAll().Where(a => objActionList.All(x=>x.ActionListId==a.ActionListId)).ToList();     //    ResponsibleUserId      CreatedBy for Checking and 
            //    listActionItem = actionItemRepository.GetAll().Where(a => listActionItemResponsible.Contains(a.ActionItemId) && objActionListIds.Contains(a.ActionListId.Value) && a.RecordDeleted == false).ToList(); //objActionListIds.Contains(a.ActionListId.Value) &&
            //}
           // AutoMapper.Mapper.Map(listActionItem, listActionItemModel);
            return listActionItemModel;
        }


        public ActionItemModel GetActionItemDetail(ActionItemModel actionItemModel)
        {
            ActionItem objActionList = actionItemRepository.SingleOrDefault(u => u.ActionItemId == actionItemModel.ActionItemId && u.RecordDeleted == false);
            // objActionList.FileAttachments = objActionList.FileAttachments.Where(x => x.RecordDeleted == false).ToList();
            ActionItemModel objActionListModel = new ActionItemModel();
            AutoMapper.Mapper.Map(objActionList, objActionListModel);
            return objActionListModel;
        }
        #endregion
    }
}
