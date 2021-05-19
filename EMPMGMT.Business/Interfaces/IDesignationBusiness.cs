using EMPMGMT.Domain;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business.Interfaces
{
    public interface IDesignationBusiness
    {
        List<DesignationModel> DesignationList(ListingParameters listingParameters);       
    }
}
