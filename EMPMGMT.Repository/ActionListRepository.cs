using EMPMGMT.Repository;
using EMPMGMT.Repository.Infrastructure;
using EMPMGMT.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Repository
{
    public class ActionListRepository : BaseRepository<ActionList>
    {
        public ActionListRepository(IUnitOfWork unit)
            : base(unit)
        {

        }

        public List<SSP_GetActionListDetails_Result> GetActionListDetails(int ProjectId, string SearchText, int UserId, int currentPage, int pageSize, string SortbyColumn, string sortOrder)
        {

            Entities entities = (Entities)this.UnitOfWork.Db;
            List<SSP_GetActionListDetails_Result> actionListDetails = entities.SSP_GetActionListsDetails(ProjectId, SearchText, UserId, currentPage, pageSize, SortbyColumn, sortOrder).ToList();
            return actionListDetails;
        }
    }
}
