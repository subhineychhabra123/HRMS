using EMPMGMT.Domain;
using EMPMGMT.Repository;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business.Interfaces
{
    public interface IUserBusiness
    {
        EmployeeModel GetUserByEmailId(string emailId);
        void AddUser(EmployeeModel user);
        EmployeeModel RegisterUser(EmployeeModel user);
        List<EmployeeModel> GetUsers(ListingParameters listingParameters, ref int totalRecords);
        EmployeeModel RegisteredUserActivate_Deactivate_Action(EmployeeModel userModel);
        EmployeeModel ValidateUser(string email, string password, bool isChecked);
        EmployeeModel GetUser(int userId);
        EmployeeModel GetUsercomments(int loginbuserId, int userid);
        string ResetPassword(EmployeeModel user);
        void UpdateUser(EmployeeModel userModel);
        void UpdateUserProfile(EmployeeModel userModel);
        List<EmployeeModel> GetAllUsers(int companyId, int userStatus, int exceptThisId = 0);
        void ChangeImage(int userId, string fileName);
        bool DeleteProfile(int profileId, int reassignProfileId, int? companyId, int ModifiedBy);
        string GenerateRandomAlphaNumericCode(int length);
        void updatemoreinfo_status(EmployeeModel userModel);
        void AddUsers(List<EmployeeModel> userModelList);
        bool UpdateUser_ResetPassword(int uid, string password);
        bool Checkinvite_Validuser(int userid);
        bool UpdateUserAfterMailSend(int uid, string guid);
        List<EmployeeModel> GetAllUsers_Import(int companyId);
        EmployeeModel UpdateUserStatus(EmployeeModel userModel);
        List<EmployeeModel> GetCompanyUsers(ListingParameters listingParameters, ref int totalRecords);
        bool EmailIsValid(string emailAddress);
        List<EmployeeModel> GetUsersListForAutocomplete(string searchString, int userStatus);
        void AddDefaultOrganisationUnit(string OrgUnitName, int CompanyId, int ProfileId);
        bool Checkinvite_Validuser_Guid(string guid);
        bool UpdateUser_ResetPassword_Guid(string guid, string password);
        List<EmployeeModel> GetUsersListForAutocomplete(int companyId, int userStatus, string searchString);
        List<EmployeeModel> GetProjectUsersListForAutocomplete(int companyId, int userStatus, string searchString, int ProjectId);

        List<EmployeeModel> GetUsersListForAutocompleteById(string searchString, int userId);

        bool DeleteEmployee(EmployeeModel employeeModel);

        
    }
}
