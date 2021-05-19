using EMPMGMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business.Interfaces
{
  public interface IReffererBusiness
    {
      List<ReffererModel> GetUsersListForAutocomplete(string searchString);
    }
}
