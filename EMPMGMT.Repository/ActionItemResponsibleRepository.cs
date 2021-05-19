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
  public  class ActionItemResponsibleRepository: BaseRepository<ActionItemResponsible>
    {
        Entities entities;
        public ActionItemResponsibleRepository(IUnitOfWork unit)
            : base(unit)
        {
            entities = (Entities)this.UnitOfWork.Db;
        }
    }
}