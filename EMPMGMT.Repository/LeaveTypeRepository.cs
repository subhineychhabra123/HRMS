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
   public class LeaveTypeRepository:BaseRepository<LeaveType>
    {
        Entities eintities;
        public LeaveTypeRepository(IUnitOfWork unit)
            : base(unit)
        {
            eintities = (Entities)this.UnitOfWork.Db;
        }
    }
}
