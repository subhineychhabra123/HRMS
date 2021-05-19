using EMPMGMT.Repository;
using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;

using EMPMGMT.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business
{
    public class ProfilePermissionBusiness : IProfilePermissionBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ProfilePermissionRepository profilePermissionRepository;

        public ProfilePermissionBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            profilePermissionRepository = new ProfilePermissionRepository(unitOfWork);
        }

        public void UpdateProfilePermission(List<ProfilePermissionModel> profilePermissionModels, int modifiedBy)
        {
            int? profileId = profilePermissionModels[0].ProfileId;           
            List<ProfilePermission> profilePermissionList = profilePermissionRepository.GetAll(r => r.ProfileId == profileId).ToList();
            if (profilePermissionList != null)
            {
                foreach (var profilePermissionModel in profilePermissionModels)
                {
                    ProfilePermission profilePermission = profilePermissionList.Where(x => x.ProfilePermissionId == profilePermissionModel.ProfilePermissionId).SingleOrDefault();
                    profilePermission.HasAccess = profilePermissionModel.HasAccess;
                    profilePermission.ModifiedDate = DateTime.UtcNow;
                    profilePermission.ModifiedBy = modifiedBy;
                }
                profilePermissionRepository.UpdateAll(profilePermissionList);
            }

            profilePermissionRepository.CheckMainMenuConfiguration((int)profileId);
            profilePermissionRepository.CheckMainMenuManageMetricDashboard((int)profileId);
            
        }
    }
}
