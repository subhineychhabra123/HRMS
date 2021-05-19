using EMPMGMT.Repository.Infrastructure;
using EMPMGMT.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Repository
{
    public class TechnologyRepository : BaseRepository<Technology>
    {
        Entities entities;
        public TechnologyRepository(IUnitOfWork unit)
            : base(unit)
        {
            entities = (Entities)this.UnitOfWork.Db;
        }

    }
}
