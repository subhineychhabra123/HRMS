using EMPMGMT.Repository.Infrastructure;
using EMPMGMT.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Repository
{
    public class DesignationRepository : BaseRepository<Designation>
    {
        public DesignationRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
    }
}
