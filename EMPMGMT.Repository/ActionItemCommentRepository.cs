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
   public class ActionItemCommentRepository: BaseRepository<ActionItemComment>
    {
        Entities entities;
        public ActionItemCommentRepository(IUnitOfWork unit)
            : base(unit)
        {
            entities = (Entities)this.UnitOfWork.Db;
        }
}
}
