using EMPMGMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business.Interfaces
{
  public  interface IResourcesBusiness
    {
      void SaveResources(List<ResourcesModel> listResourcesModel);
      void DeleteResource(ResourcesModel resourcesModel);
    }
}
