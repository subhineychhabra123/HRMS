using EMPMGMT.Repository.Infrastructure;
using EMPMGMT.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Repository
{
   public class RefferrerRepository: BaseRepository<Referrer>
    {
       public RefferrerRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
       //public List<Referrer> GetUsersListAutocomplete(string query)
       //{
       //    Entities entities = (Entities)this.UnitOfWork.Db;
       //    List<Referrer> listReferrer = entities.Function_SSP_GetRefferrerForAutoComplete(query).ToList();
       //    List<Referrer> listReferrer = new List<Referrer>();
       //    return listReferrer;
       //}
    }
}
