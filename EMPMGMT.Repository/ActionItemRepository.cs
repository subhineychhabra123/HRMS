using EMPMGMT.Repository;
using EMPMGMT.Repository.Infrastructure;
using EMPMGMT.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EMPMGMT.Repository
{
    public class ActionItemRepository : BaseRepository<ActionItem>
    {
        Entities entities;
        public ActionItemRepository(IUnitOfWork unit)
            : base(unit)
        {
            entities = (Entities)this.UnitOfWork.Db;
        }


        public void DeleteActionItem(int ActionItemId)
        {
            entities.SSP_DeleteActionItem(ActionItemId);
        }
        public List<SSP_GetActionItemDescription_Result> GetActionItemDescriptionOfActionItem(int ActionItemId)
        {
            List<SSP_GetActionItemDescription_Result> actionItemDescription = entities.SSP_GetActionItemDescription(ActionItemId).ToList();
            return actionItemDescription;
        }
        public void GetActionItemStatus(int ActionItemId, out int ActionItemStatus)
        {
            ObjectParameter Status = new ObjectParameter("ActionItemStatus", typeof(String));
         //   entities.SSP_GetActionItemStatus(ActionItemId,Status);
            ActionItemStatus = Convert.ToInt32(Status.Value);
        }


        public List<SSP_GetActionItemsFromActionListId_Result> GetActionItemsFromActionListId(int? ActionListId, bool OnTime, bool OverDue, bool BeforeDue, string SearchText, DateTime? StartDate, DateTime? DueDate, int StatusDrop, int ResponsibleUserId)//, int UserId
        {
            List<SSP_GetActionItemsFromActionListId_Result> listActionItemsOfActionListResult = entities.SSP_GetActionItemsFromActionListId(ActionListId, OnTime, OverDue, BeforeDue, SearchText, StartDate, DueDate, StatusDrop, ResponsibleUserId).ToList(); //, UserId
            return listActionItemsOfActionListResult;
        }
        public List<SSP_GetActionItemsforDropdwonforProjectId_Result> GetActionItemsForDropdownProjectId(int? ProjectId, int? UserId )
        {
            List<SSP_GetActionItemsforDropdwonforProjectId_Result> listActionItemsfordropdownProject = entities.SSP_GetActionItemsforDropdwonforProjectId(ProjectId.Value, UserId.Value).ToList();
            return listActionItemsfordropdownProject;
        }

        public List<SSP_GetActionItemsFromActionListId_Mtest_Result> GetActionItemsFromActionListId_Test(int? ActionListId, bool OnTime, bool OverDue, bool BeforeDue, string SearchText, DateTime? StartDate, DateTime? DueDate, int StatusDrop, int ResponsibleUserId)//, int UserId
        {
            List<SSP_GetActionItemsFromActionListId_Mtest_Result> listActionItemsOfActionListResult = entities.SSP_GetActionItemsFromActionListId_Test(ActionListId, OnTime, OverDue, BeforeDue, SearchText, StartDate, DueDate, StatusDrop, ResponsibleUserId).ToList(); //, UserId
            return listActionItemsOfActionListResult;
        }
       

    }
}