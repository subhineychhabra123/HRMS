using EMPMGMT.Repository;
using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;

using EMPMGMT.Repository.Infrastructure.Contract;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business
{
    public class ProfileBusiness : IProfileBusiness
    {
        private readonly ProfileRepository profileRepository;
        private readonly ProfilePermissionRepository profilePermissionRepository;
        public ProfileBusiness(IUnitOfWork _unitOfWork)
        {
            profileRepository = new ProfileRepository(_unitOfWork);
            profilePermissionRepository = new ProfilePermissionRepository(_unitOfWork);
        }
        public List<ProfileModel> Profiles(ListingParameters listingParameters)
        {

            List<ProfileModel> listProfileModel = new List<ProfileModel>();
            List<Profile> listProfile = new List<Profile>();
            switch (listingParameters.OrderByColumn)
            {
                case "ProfileName":
                    if (listingParameters.OrderBy == "asc")
                    {
                        listProfile = profileRepository.GetAll(p =>  p.RecordDeleted == false || p.CompanyId.HasValue == false).OrderBy(x => x.ProfileName).ToList();//p.CompanyId == listingParameters.CompanyId &&
                    }
                    else
                    {
                        listProfile = profileRepository.GetAll(p => p.RecordDeleted == false || p.CompanyId.HasValue == false).OrderByDescending(x => x.ProfileName).ToList();//p.CompanyId == listingParameters.CompanyId && 
                    }
                    break;
                case "Description":
                    if (listingParameters.OrderBy == "asc")
                    {
                        listProfile = profileRepository.GetAll(p => p.RecordDeleted == false || p.CompanyId.HasValue == false).OrderBy(x => x.Description).ToList();//p.CompanyId == listingParameters.CompanyId &&
                    }
                    else
                    {
                        listProfile = profileRepository.GetAll(p => p.RecordDeleted == false || p.CompanyId.HasValue == false).OrderByDescending(x => x.Description).ToList();//p.CompanyId == listingParameters.CompanyId &&
                    }
                    break;
                default:
                    listProfile = profileRepository.GetAll(p =>  p.RecordDeleted == false || p.CompanyId.HasValue == false).ToList();//p.CompanyId == listingParameters.CompanyId &&
                    break;
            }
            AutoMapper.Mapper.Map(listProfile, listProfileModel);
            return listProfileModel;
        }

        public bool AddProfile(ProfileModel profileModel)
        {
            Profile profile = new Profile();
            AutoMapper.Mapper.Map(profileModel, profile);
            bool isExists = profileRepository.Exists(r => r.ProfileName == profileModel.ProfileName && (profileModel.CompanyId == null) && r.RecordDeleted == false);//r.CompanyId == profileModel.CompanyId ||

            if (!isExists)
            {

                List<ProfilePermission> defaultPermissions = profilePermissionRepository.GetAll(x => x.Profile.IsDefaultForRegisterdUser == true).ToList();
                foreach (ProfilePermission profilePermission in defaultPermissions)
                {
                    profile.ProfilePermission.Add(new ProfilePermission()
                    {
                        HasAccess = profilePermission.HasAccess,
                        CreatedDate = DateTime.UtcNow,
                        ModulePermissionId = profilePermission.ModulePermissionId,
                        CreatedBy = profileModel.CreatedBy
                    });
                }
                profileRepository.Insert(profile);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateProfile(ProfileModel profileModel)
        {
            bool isExists = profileRepository.Exists(r => r.ProfileName == profileModel.ProfileName && r.ProfileId != profileModel.ProfileId);

            if (!isExists)
            {
                Profile profile = profileRepository.SingleOrDefault(p => p.ProfileId == profileModel.ProfileId && p.RecordDeleted == false);
                if (profile != null)
                {
                    profile.ProfileName = profileModel.ProfileName;
                    profile.Description = profileModel.Description;
                    profile.ModifiedDate = profileModel.ModifiedDate;
                    profile.ModifiedBy = profileModel.ModifiedBy;
                    profileRepository.Update(profile);

                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public ProfileModel GetProfileTypeById(int profileId)
        {

            ProfileModel profileModel = new ProfileModel();
            Profile profile = profileRepository.SingleOrDefault(p => p.ProfileId == profileId && p.RecordDeleted == false);
            AutoMapper.Mapper.Map(profile, profileModel);
            return profileModel;
        }
        public ProfileModel GetProfileDetail(int profileId, int? companyId)
        {
            ProfileModel profileModel = new ProfileModel();
            Profile profile = profileRepository.SingleOrDefault(p => p.ProfileId == profileId && p.RecordDeleted == false);
            if (profile != null)
                if (profile.IsDefaultForRegisterdUser != true)
                {
                    if (companyId == null)
                    {
                        profile = profileRepository.SingleOrDefault(p => p.ProfileId == profileId && p.RecordDeleted == false);
                    }
                    else
                    {
                        profile = profileRepository.SingleOrDefault(p => p.ProfileId == profileId && p.RecordDeleted == false);//&& p.CompanyId == (int)companyId
                    }

                }
            AutoMapper.Mapper.Map(profile, profileModel);
            AutoMapper.Mapper.Map(profile.ProfilePermission, profileModel.ProfilePermissionModels);
            return profileModel;
        }
        public string GetPipeSepratedPermission(int profileId, int? companyId)
        {
            String pipesepratedPermissions = string.Empty;
            ProfileModel profileModel = this.GetProfileDetail(profileId, companyId);
            // profileModel.ProfilePermissions = profileModel.ProfilePermissions.OrderBy(x => x.ModulePermission.Module.SortOrder).ToList();
            var permissionArray = profileModel.ProfilePermissionModels.Where(x => x.HasAccess == true).Select(x => x.ModulePermission.Module.ModuleCONSTANT + x.ModulePermission.Permission.PermissionCONSTANT).ToList();
            pipesepratedPermissions = String.Join("|", permissionArray.ToArray());
            return pipesepratedPermissions;
        }
        public List<ProfileModel> GetProfileDDL(int companyId)
        {
            List<ProfileModel> listProfileModel = new List<ProfileModel>();
            listProfileModel = profileRepository.GetAll(p => p.RecordDeleted == false || p.CompanyId.HasValue == false).Select(x => new ProfileModel { ProfileId = x.ProfileId, ProfileName = x.ProfileName }).ToList();//p.CompanyId == companyId &&
            if (listProfileModel == null) { new List<ProfileModel>(); }
            listProfileModel.Insert(0, new ProfileModel
            {
                ProfileId = Constants.DropDownListDefualtValue,
                ProfileName = Constants.DropDownListDefaultText
            });
            return listProfileModel;

        }
    }
}
