using EMPMGMT.Domain;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business.Interfaces
{
    public interface IProfileBusiness
    {
        List<ProfileModel> Profiles(ListingParameters listingParameters);
        List<ProfileModel> GetProfileDDL(int companyId);
        bool AddProfile(ProfileModel profileModel);
        bool UpdateProfile(ProfileModel profileModel);
        ProfileModel GetProfileTypeById(int profileId);
        ProfileModel GetProfileDetail(int profileId, int? companyId);
        string GetPipeSepratedPermission(int profileId, int? companyId);

    }
}
