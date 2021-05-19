using System;
using EMPMGMT.Domain;
using EMPMGMT.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMPMGMT.Utility;


namespace EMPMGMT.Business.Interfaces
{
    public interface IActionItemBusiness
    {
        ActionItemModel GetActionItemDescriptionByActionItemId(int actionItemId);
        List<ActionItemModel> GetActionItemList(ListingParameters listingParameters, int ActionListId, ref int totalRecords);
        List<ActionItemModel> GetActionItemListByACtionListId(ActionItemModel actionItemModel);
        int SaveActionItemRecord(ActionItemModel actionItemModel);
        // bool DeleteActionItem(ActionItemModel actionItemModel);
        void DeleteActionItem(ActionItemModel actionItemModel);

        int UpdateActionItemRecord(ActionItemModel actionItemModel);

        List<ActionItemModel> GetActionItemListFromActionId(int? ActionListId, bool OnTime, bool OverDue, bool BeforeDue, string SearchText, DateTime? StartDate, DateTime? DueDate, int StatusDrop, int ResponsibleUserId); //, int UserId

        List<ActionItemModel> ActionItemList();
        List<ActionItemModel> ActionItemList(int projectId, int UserId);
        ActionItemModel GetActionItemDetail(ActionItemModel actionItemModel);
    }
}
