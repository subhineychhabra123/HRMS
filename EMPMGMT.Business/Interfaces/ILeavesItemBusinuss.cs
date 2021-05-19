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
    public interface ILeavesItemBusinuss
    {
        int SaveLeaveItemRecord(LeavesItemModel LeavesItemModel);
        //void DeleteLeavesItem(LeavesItemModel LeavesItemModel);

        //int UpdateLeavesItemRecord(LeavesItemModel LeavesItemModel);
        //List<LeavesItemModel> LeavesItemList();
        List<LeaveTypeModel> GetAllLeaveId();
        List<LeavesItemModel> GetAllLeavesDetail(ListingParameters listingParameters);
        void ApproveLeaveItemRecord(LeavesItemModel leaveModel);

    }
}
