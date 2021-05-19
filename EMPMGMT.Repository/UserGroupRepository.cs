using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMPMGMT.Repository;
using EMPMGMT.Repository.Infrastructure;
using EMPMGMT.Repository.Infrastructure.Contract;

using System.Data.Entity.Core.Objects;

namespace EMPMGMT.Repository
{
  public  class UserGroupRepository:BaseRepository<UserGroup>
    {
         Entities entities;
         public UserGroupRepository(IUnitOfWork unit)
            : base(unit)
        {
            entities = (Entities)this.UnitOfWork.Db;
        }
    }
}
