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
    public class CommentsRepository : BaseRepository<Comments>
    {
        public CommentsRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
    }
}
