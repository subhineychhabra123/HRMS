using EMPMGMT.Domain;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business.Interfaces
{
    public interface IActionListBussiness
    {
        List<ActionListModel> GetActionListing(ListingParameters listingParameters, out int totalRecords);
        List<ActionListModel> GetActionListForAutoComplete(int CompanyId, string searchText);
        List<ActionListModel>GetActionList(ListingParameters listingParameters,int LogedInUserId, ref int totalRecords);
        bool DeleteActionList(ActionListModel actionListModel);
        string UpdateActionList(ActionListModel actionListModel);
        int SaveActionList(ActionListModel actionListModel);
        ActionListModel GetActionListDetail(ActionListModel actionListModel);
        List<ActionListModel> GetProjectActionListing(int ProjectId);
        ActionListModel ProjectActionList(int ProjectId, int UserId);
    }
}
