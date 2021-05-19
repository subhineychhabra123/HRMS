using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;
using EMPMGMT.Repository;
using EMPMGMT.Repository.Infrastructure.Contract;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq.Expressions;



namespace EMPMGMT.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserRepository userRepository;
        private readonly ProfileRepository profileRepository;
        private RefferrerRepository reffererRepository;
        private readonly OrganizationUnitRepository organizationUnitRepository;
        private readonly ResourcesRepository resourceRepository;
        static Regex ValidEmailRegex = CreateValidEmailRegex();
        public UserBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            userRepository = new UserRepository(unitOfWork);
            profileRepository = new ProfileRepository(unitOfWork);
            organizationUnitRepository = new OrganizationUnitRepository(unitOfWork);
            reffererRepository = new RefferrerRepository(unitOfWork);
            resourceRepository = new ResourcesRepository(unitOfWork);


        }




        /// </summary>
        /// <returns></returns>
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }

        public bool EmailIsValid(string emailAddress)
        {
            bool isValid = ValidEmailRegex.IsMatch(emailAddress);

            return isValid;

        }
        public void AddUser(EmployeeModel userModel)
        {

            Employee user = new Employee();
            Employee userAdmin = new Employee();
            var Userids = userRepository.GetAll(x => x.RecordDeleted == false || x.RecordDeleted == false);
            var UserCode = Userids.Where(x => x.UserId == Userids.Max(y => y.UserId)).Select(x => x.EmpCode).FirstOrDefault();
         
            string[] Empvar = null;
            long EmployeeId = 1;
            string EmpCode = "EMP_";
            if (UserCode != null)
            {
                Empvar = Convert.ToString(UserCode).Split('_');
                EmployeeId = Convert.ToInt64(Empvar[1]) + 1;
                EmpCode = EmpCode + EmployeeId;
            }
            //Referrer referrerEntity = new Referrer();
            //referrerEntity = reffererRepository.SingleOrDefault(r => r.ReferrerId  == referrerEntity.ReferrerId);
            //if (referrerEntity != null)
            //{
            //    user.ReferrerId = referrerEntity.ReferrerId;
            //}
            //else
            //{
            //    if (!string.IsNullOrWhiteSpace(referrerEntity.ReferrerName))
            //    {
            //        Referrer newReferrerEntity = new Referrer { ReferrerName = referrerEntity.ReferrerName };
            //        newReferrerEntity = reffererRepository.Insert(newReferrerEntity);
            //        user.ReferrerId = newReferrerEntity.ReferrerId;
            //    }
            //    else
            //        user.ReferrerId = null;
            //}
            AutoMapper.Mapper.Map(userModel, user);
            user.CreatedDate = DateTime.UtcNow;
            //Employee.Status = (int)Enums.UserStatus.Active;
            user.UserTypeId = (int)Enums.UserType.User;
            //Employee.AddressId = null;
            user.Profile = null;
            user.RecordDeleted = false;
            user.IsRegisteredUser = false;
            //Employee.AccessForKPIModule = userAdmin.AccessForKPIModule;
            //Employee.AccessForGoalModule = userAdmin.AccessForGoalModule;
            user.TechnologyId = user.TechnologyId == 0 ? null : user.TechnologyId;
            user.DesignationId = user.DesignationId == 0 ? null : user.DesignationId;
            user.CandidateId = user.CandidateId == 0 ? null : user.CandidateId;
            user.ReferrerId = user.ReferrerId == 0 ? null : user.ReferrerId;
            user.OrgUnitId = user.OrgUnitId == 0 ? null : user.OrgUnitId;
            user.ReportTo = user.ReportTo == 0 ? null : user.ReportTo;
            user.ImageURL = userModel.ImageURL;
            user.EmpCode = EmpCode;
            userRepository.Insert(user);
            userModel.UserId = user.UserId;
        }

        public void AddUsers(List<EmployeeModel> userModelList)
        {
            List<Employee> users = new List<Employee>();
            var Userids = userRepository.GetAll(x => x.RecordDeleted == false || x.RecordDeleted == true);
            var UserCode = Userids.Where(x => x.UserId == Userids.Max(y => y.UserId)).Select(x => x.EmpCode).FirstOrDefault();
            string[] Empvar = null;
            long EmployeeId = 1;
            string EmpCode = "EMP_";
            List<string> EmpCodeList = new List<string>();
            if (UserCode != null)
            {
                Empvar = Convert.ToString(UserCode).Split('_');
              
                EmployeeId = Convert.ToInt64(Empvar[1]) + 1;
               // EmpCode = EmpCode + EmployeeId;
            }




            AutoMapper.Mapper.Map(userModelList, users);
            for (int i = 0; i < users.Count; i++)
            {                
                EmpCode = EmpCode + EmployeeId;
                users[i].EmpCode = EmpCode;
                EmpCode = "EMP_";
            EmployeeId++;
            }

            users.ForEach(x => { x.Company = null; x.IsLinkSend = false; x.LinkSendDate = DateTime.UtcNow; });
            userRepository.InsertAll(users);
        }
        /// <summary>
        /// Used to reset/Change password of the user.
        /// </summary>
        /// <param name="userModel"></param>

        public string ResetPassword(EmployeeModel userModel)
        {
            Employee user = userRepository.SingleOrDefault(x => x.UserId == userModel.UserId && x.RecordDeleted == false);
            if (user == null)
            {
                return null;

            }
            else if (user.Password != userModel.Password)
            {
                return "IncorrectPassword";
            }
            else
            {
                //  AutoMapper.Mapper.Map(userModel, user);
                user.ModifiedDate = DateTime.UtcNow;
                // user.Status = userModel.Status == 0 ? user.Status : (int)Enums.UserStatus.Active;
                user.UserTypeId = (int)Enums.UserType.User;
                user.Password = userModel.NewPassword;
                //Employee.AddressId = null;
                user.OrganizationUnit = null;
                user.RecordDeleted = false;
                // user.IsRegisteredUser = true;
                userRepository.Update(user);
                return "Successful";
            }
        }

        public void ChangeImage(int userId, string fileName)
        {
            EmployeeModel usermodel = null;
            Employee user = userRepository.SingleOrDefault(u => u.UserId == userId && u.RecordDeleted == false);
            if (user != null)
            {
                user.ModifiedDate = DateTime.UtcNow;
                user.UserId = userId;
                user.ImageURL = fileName;
                userRepository.Update(user);
            }

        }
        public void UpdateUserProfile(EmployeeModel userModel)
        {
            Employee user = userRepository.SingleOrDefault(x => x.UserId == userModel.UserId && x.RecordDeleted == false);
            if (user == null)
            {
                return;
            }
            //AutoMapper.Mapper.Map(userModel, user);
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.EmailId = userModel.EmailId;

            // user.OrgUnitId = userModel.OrgUnitId;
            user.ModifiedDate = DateTime.UtcNow;
            //Employee.Status = userModel.Status == 0 ? user.Status : (int)Enums.UserStatus.Invited;
            //Employee.UserTypeId = (int)Enums.UserType.Admin;
            //Employee.AddressId = null;
            user.Profile = null;
            user.OrganizationUnit = null;
            user.RecordDeleted = false;
            user.IsRegisteredUser = false;
            user.TechnologyId = user.TechnologyId == 0 ? null : user.TechnologyId;
            user.OrgUnitId = user.OrgUnitId == 0 ? null : user.OrgUnitId;
            user.DesignationId = user.DesignationId == 0 ? null : user.DesignationId;
            user.CandidateId = user.CandidateId == 0 ? null : user.CandidateId;
            user.ReferrerId = user.ReferrerId == 0 ? null : user.ReferrerId;
            user.ReportTo = user.ReportTo == 0 ? null : user.ReportTo;

            userRepository.Update(user);
        }
        public void UpdateUser(EmployeeModel userModel)
        {
            Employee user = userRepository.SingleOrDefault(x => x.UserId == userModel.UserId && x.RecordDeleted == false);
            if (user == null)
            {
                return;
            }
            AutoMapper.Mapper.Map(userModel, user);
            // user.OrgUnitId = userModel.OrgUnitId;
            user.ModifiedDate = DateTime.UtcNow;

            user.UserTypeId = (int)Enums.UserType.User;
            //Employee.AddressId = null;
            user.Profile = null;
            user.OrganizationUnit = null;
            user.Referrer = null;
            user.Technology = null;
            user.Designation = null;
            user.RecordDeleted = false;
            user.IsRegisteredUser = false;
            user.TechnologyId = user.TechnologyId == 0 ? null : user.TechnologyId;
            user.OrgUnitId = user.OrgUnitId == 0 ? null : user.OrgUnitId;
            user.DesignationId = user.DesignationId == 0 ? null : user.DesignationId;
            user.CandidateId = user.CandidateId == 0 ? null : user.CandidateId;
            user.ReferrerId = user.ReferrerId == 0 ? null : user.ReferrerId;

            user.ReportTo = user.ReportTo == 0 ? null : user.ReportTo;
            userRepository.Update(user);
        }

        public bool UpdateUser_ResetPassword(int uid, string password)
        {
            Employee user = userRepository.SingleOrDefault(x => x.UserId == uid);
            if (user == null)
            {
                return false;
            }
            user.ActivationDate = DateTime.UtcNow;
            user.IsLinkSend = true;
            user.LinkSendDate = DateTime.UtcNow;
            user.Password = password;
            user.Status = (int)Enums.UserStatus.Active;
            user.UserGuid = Guid.NewGuid().ToString();
            userRepository.Update(user);
            return true;
        }

        public bool UpdateUser_ResetPassword_Guid(string guid, string password)
        {
            Employee user = userRepository.SingleOrDefault(x => x.UserGuid == guid);
            if (user == null)
            {
                return false;
            }
            user.ActivationDate = DateTime.UtcNow;
            user.IsLinkSend = true;
            user.LinkSendDate = DateTime.UtcNow;
            user.Password = password;
            user.Status = (int)Enums.UserStatus.Active;
            user.UserGuid = guid.ToString();
            userRepository.Update(user);
            return true;
        }


        public bool UpdateUserAfterMailSend(int uid, string guid)
        {
            Employee user = userRepository.SingleOrDefault(x => x.UserId == uid);
            if (user == null)
            {
                return false;
            }
            user.IsLinkSend = true;
            user.LinkSendDate = DateTime.UtcNow;
            user.UserGuid = guid;
            userRepository.Update(user);
            return true;
        }

        public bool Checkinvite_Validuser(int uid)
        {
            DateTime currentdate = DateTime.Now;
            Employee user = userRepository.SingleOrDefault(x => x.UserId == uid);
            if (user == null)
            {
                return false;
            }

            //if (user.UserGuid == null || user.UserGuid.ToString() == "")
            //{
            if (user.LinkSendDate != null || user.LinkSendDate.ToString() != "")
            {
                DateTime linksenddate = (DateTime)user.LinkSendDate;

                TimeSpan difference = currentdate - linksenddate;


                int days = (int)difference.TotalDays;

                if (days <= 1)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }


        public bool Checkinvite_Validuser_Guid(string Guid)
        {
            DateTime currentdate = DateTime.Now;
            Employee user = userRepository.SingleOrDefault(x => x.UserGuid == Guid);
            if (user == null)
            {
                return false;

            }

            //if (user.UserGuid == null || user.UserGuid.ToString() == "")
            //{
            if (user.LinkSendDate != null || user.LinkSendDate.ToString() != "")
            {
                DateTime linksenddate = (DateTime)user.LinkSendDate;

                TimeSpan difference = currentdate - linksenddate;


                int days = (int)difference.TotalDays;

                if (days <= 1)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        public bool DeleteProfile(int ProfileId, int reassignedId, int? companyId, int ModifiedBy)
        {


            List<Employee> listUsers = new List<Employee>();
            Profile profile = new Profile();
            listUsers = userRepository.GetAll(x => x.ProfileId == ProfileId && x.RecordDeleted == false).ToList(); //x.CompanyId == companyId &&
            if (listUsers.Count > 0)
            {
                foreach (Employee userObj in listUsers)
                {
                    userObj.ProfileId = reassignedId;
                    userObj.ModifiedDate = DateTime.UtcNow;
                    userObj.ModifiedBy = ModifiedBy;


                }
                userRepository.UpdateAll(listUsers);
            }
            profile = profileRepository.SingleOrDefault(r => r.ProfileId == ProfileId && r.RecordDeleted == false); //r.CompanyId == companyId &&
            if (profile != null)
            {
                profile.RecordDeleted = true;
                profile.ModifiedDate = DateTime.UtcNow;
                profile.ModifiedBy = ModifiedBy;
                profileRepository.Update(profile);
                return true;
            }
            else
                return false;
        }
        public EmployeeModel RegisterUser(EmployeeModel userModel)
        {
            Employee user = new Employee();
            user.Company = new Company();

            if (userModel.ProfileModel != null)
                userModel.ProfileModel = null;
            if (userModel.organizationUnitModel != null)
                userModel.organizationUnitModel = null;
            AutoMapper.Mapper.Map(userModel, user);
            user.CreatedDate = DateTime.UtcNow;
            //Employee.ProfileId = GetDefaultRegistertedUserProfileId();//(int)Enums.Profile.Administrator;
            user.UserTypeId = (int)Enums.UserType.User;
            //Employee.OrganizationUnitId = OrganizationUnitRepository.SingleOrDefault(x => x.IsDefaultForRegisterdUser == true && x.RecordDeleted == false).OrganizationUnitId;           
            user.Profile = null;
            user.OrganizationUnit = null;
            user.Company.BreakThroughObjectiveYear = 3;
            user.Company.CreatedDate = DateTime.UtcNow;
            ////****************organisation detail ********************//
            //Employee.OrganizationUnit.OrgUnitName = userModel.CompanyModel.CompanyName;
            //Employee.OrganizationUnit.IsDefaultUnit = true;
            //Employee.OrganizationUnit.CreatedDate = DateTime.UtcNow;
            user.Status = (int)Enums.UserStatus.Pending;
            user.ReportTo = user.ReportTo == 0 ? null : user.ReportTo;
            user.RecordDeleted = false;
            user.IsRegisteredUser = true;
            user.ActivationDate = null;
            userRepository.Insert(user);
            userModel.CompanyId = user.Company.CompanyId;
            
            userModel.UserId = user.UserId;

            return userModel;
        }
        public EmployeeModel GetUserByEmailId(string emailId)
        {
            EmployeeModel usermodel = null;
            Employee user = userRepository.SingleOrDefault(u => u.EmailId == emailId && u.RecordDeleted == false);
            if (user != null)
            {
                usermodel = new EmployeeModel();
                AutoMapper.Mapper.Map(user, usermodel);
            }


            return usermodel;
        }
        public List<EmployeeModel> GetUsers(ListingParameters listingParameters, ref int totalRecords)
        {
            int currentPage = listingParameters.CurrentPage;
            int pageSize = listingParameters.PageSize;
            List<EmployeeModel> usermodelList = new List<EmployeeModel>();
            DateTime currentDate = DateTime.Now;
            if (listingParameters.SearchText != null)
            {
                Expression<Func<Employee, bool>> whereCondition;
                //whereCondition = x => (x.FirstName.Contains(listingParameters.SearchText) || x.LastName.Contains(listingParameters.SearchText) || x.EmailId.Contains(listingParameters.SearchText)) && x.Status == listingParameters.UserStatus && x.IsRegisteredUser == listingParameters.IsRegistered && x.RecordDeleted == false;
                if (listingParameters.UserStatus == Convert.ToInt32(Utility.Enums.UserStatus.Expired))
                {
                    whereCondition = x => (x.FirstName.Contains(listingParameters.SearchText) || x.LastName.Contains(listingParameters.SearchText) ||  x.EmpCode.Contains(listingParameters.SearchText)|| x.EmailId.Contains(listingParameters.SearchText)) && x.IsRegisteredUser == true && x.RecordDeleted == false;
                }
                else
                    if (listingParameters.UserStatus == Convert.ToInt32(Utility.Enums.UserStatus.Deactive))
                    {
                        whereCondition = x => (x.FirstName.Contains(listingParameters.SearchText) || x.LastName.Contains(listingParameters.SearchText) || x.EmpCode.Contains(listingParameters.SearchText) || x.EmailId.Contains(listingParameters.SearchText) && x.Status == listingParameters.UserStatus && x.IsRegisteredUser == listingParameters.IsRegistered && x.RecordDeleted == false);
                    }
                    else
                    {
                        whereCondition = x => (x.FirstName.Contains(listingParameters.SearchText) || x.LastName.Contains(listingParameters.SearchText) || x.EmpCode.Contains(listingParameters.SearchText) || x.EmailId.Contains(listingParameters.SearchText)) && x.Status == listingParameters.UserStatus && x.IsRegisteredUser == listingParameters.IsRegistered && x.RecordDeleted == false && ((listingParameters.UserStatus == (int)Utility.Enums.UserStatus.Pending));
                    }

                totalRecords = userRepository.Count(whereCondition);

                //listMetric = metricRepository.GetPagedRecordsDecending(x => x.Title.Contains(listingParameters.SearchText) && x.CompanyId == listingParameters.CompanyId && x.RecordDeleted == false, y => y.CreatedDate, currentPage, pageSize).OrderByDescending(c => c.CreatedDate).ToList();

                List<Employee> registeredUsers = new List<Employee>();
                switch (listingParameters.OrderByColumn)
                {
                    case "Name":
                        if (listingParameters.OrderBy == "asc")
                        {
                            registeredUsers = userRepository.GetPagedRecords(whereCondition, y => y.FirstName, currentPage, pageSize).ToList();
                        }
                        else
                        {
                            registeredUsers = userRepository.GetPagedRecordsDecending(whereCondition, y => y.FirstName, currentPage, pageSize).ToList();
                        }
                        break;
                    case "EmailId":
                        if (listingParameters.OrderBy == "asc")
                        {
                            registeredUsers = userRepository.GetPagedRecords(whereCondition, y => y.EmailId, currentPage, pageSize).ToList();
                        }
                        else
                        {
                            registeredUsers = userRepository.GetPagedRecordsDecending(whereCondition, y => y.EmailId, currentPage, pageSize).ToList();
                        }
                        break;
                    case "RegistrationDate":
                        if (listingParameters.OrderBy == "asc")
                        {
                            registeredUsers = userRepository.GetPagedRecords(whereCondition, y => y.CreatedDate, currentPage, pageSize).ToList();
                        }
                        else
                        {
                            registeredUsers = userRepository.GetPagedRecordsDecending(whereCondition, y => y.CreatedDate, currentPage, pageSize).ToList();
                        }
                        break;
                    default:
                        registeredUsers = userRepository.GetPagedRecordsDecending(whereCondition, y => y.CreatedDate, currentPage, pageSize).ToList();
                        break;

                }
                AutoMapper.Mapper.Map(registeredUsers, usermodelList);
                foreach (var user in usermodelList)
                {
                    if (user.Password != null)
                    {
                        user.IsPassword = true;
                        user.Password = null;
                    }
                    else
                    {
                        user.IsPassword = false;
                    }
                }
                return usermodelList;


            }
            else
            {
                Expression<Func<Employee, bool>> whereCondition;

                //!(currentDate.Date >= Convert.ToDateTime(userAdmin.AccessPeriodFrom).Date && currentDate.Date <= Convert.ToDateTime(userAdmin.AccessPeriodTo).Date)
                if (listingParameters.UserStatus == Convert.ToInt32(Utility.Enums.UserStatus.Expired))
                {
                    whereCondition = x => x.IsRegisteredUser == true && x.RecordDeleted == false;
                }
                else
                    if (listingParameters.UserStatus == Convert.ToInt32(Utility.Enums.UserStatus.Deactive))
                    {
                        whereCondition = x => x.Status == listingParameters.UserStatus && x.IsRegisteredUser == listingParameters.IsRegistered && x.RecordDeleted == false;
                    }
                    else
                    {
                        whereCondition = x => x.Status == listingParameters.UserStatus && x.IsRegisteredUser == listingParameters.IsRegistered && x.RecordDeleted == false && ((listingParameters.UserStatus == (int)Utility.Enums.UserStatus.Pending || listingParameters.UserStatus == (int)Utility.Enums.UserStatus.Moreinfo));
                    }
                totalRecords = userRepository.Count(whereCondition);
                List<Employee> registeredUsers = new List<Employee>();
                switch (listingParameters.OrderByColumn)
                {
                    case "Name":
                        if (listingParameters.OrderBy == "asc")
                        {
                            registeredUsers = userRepository.GetPagedRecords(whereCondition, y => y.FirstName, currentPage, pageSize).ToList();
                        }
                        else
                        {
                            registeredUsers = userRepository.GetPagedRecordsDecending(whereCondition, y => y.FirstName, currentPage, pageSize).ToList();
                        }
                        break;
                    case "EmpCode":
                        if (listingParameters.OrderBy == "asc")
                        {
                            registeredUsers = userRepository.GetPagedRecords(whereCondition, y => y.EmpCode, currentPage, pageSize).ToList();
                        }
                        else
                        {
                            registeredUsers = userRepository.GetPagedRecordsDecending(whereCondition, y => y.EmpCode, currentPage, pageSize).ToList();
                        }
                        break;
                    case "EmailId":
                        if (listingParameters.OrderBy == "asc")
                        {
                            registeredUsers = userRepository.GetPagedRecords(whereCondition, y => y.EmailId, currentPage, pageSize).ToList();
                        }
                        else
                        {
                            registeredUsers = userRepository.GetPagedRecordsDecending(whereCondition, y => y.EmailId, currentPage, pageSize).ToList();
                        }
                        break;
                    case "RegistrationDate":
                        if (listingParameters.OrderBy == "asc")
                        {
                            registeredUsers = userRepository.GetPagedRecords(whereCondition, y => y.CreatedDate, currentPage, pageSize).ToList();
                        }
                        else
                        {
                            registeredUsers = userRepository.GetPagedRecordsDecending(whereCondition, y => y.CreatedDate, currentPage, pageSize).ToList();
                        }
                        break;
                    default:
                        registeredUsers = userRepository.GetPagedRecordsDecending(whereCondition, y => y.CreatedDate, currentPage, pageSize).ToList();
                        break;

                }
                AutoMapper.Mapper.Map(registeredUsers, usermodelList);
                foreach (var user in usermodelList)
                {
                    if (user.Password != null)
                    {
                        user.IsPassword = true;
                        user.Password = null;
                    }
                    else
                    {
                        user.IsPassword = false;
                        user.Password = null;
                    }
                }
                return usermodelList;
            }

        }



        private List<Employee> GetUsersByOrganizationUnit(int OrganizationUnitId, int companyId)
        {
            List<Employee> userList = new List<Employee>();
            userList = userRepository.GetAll(r => r.OrgUnitId == OrganizationUnitId && r.Status == (int)Enums.UserStatus.Active).ToList(); //&& r.CompanyId == companyId
            return userList;
        }
        public EmployeeModel RegisteredUserActivate_Deactivate_Action(EmployeeModel userModel)
        {
            Employee user = userRepository.SingleOrDefault(x => x.UserId == userModel.UserId && x.RecordDeleted == false);
            if (user != null)
            {
                user.Status = userModel.Status == 0 ? user.Status : userModel.Status;
                user.Comments = userModel.Comments;

                if (userModel.Status == (int)Enums.UserStatus.Active)
                {
                    user.ActivationDate = DateTime.UtcNow;
                }
                user.ModifiedDate = DateTime.UtcNow;
                user.ModifiedBy = userModel.ModifiedBy;
                userRepository.Update(user);
                AutoMapper.Mapper.Map(user, userModel);
            }
            return userModel;
        }

        public void DeactivateRegisteredUser(int userId, string comment)
        {

        }
        public EmployeeModel ValidateUser(string email, string password, bool isChecked)
        {
            int status = (int)Enums.UserStatus.Active;
            Employee user = null;
            Employee userAdmin = null;
            EmployeeModel userModel = new EmployeeModel();
            //objuser = userRepository.SingleOrDefault(a => a.EmailId == email && a.RecordDeleted == false && a.Status == status);//objuser.AccessPeriodTo CHECK
            DateTime currentDate = DateTime.Now;
            user = userRepository.SingleOrDefault(a => a.EmailId == email && a.Password == password && a.Status == status && a.RecordDeleted == false);
            AutoMapper.Mapper.Map(user, userModel);

            //Get User Admin Rights
            userAdmin = userRepository.SingleOrDefault(x => (x.IsSuperAdmin == true ? true : x.CompanyId == userModel.CompanyId) && x.RecordDeleted == false && x.Status == status);//x.IsRegisteredUser == true && 


            if (user == null)//Login entries unmatched for user
            {
                userModel.LoginStatus = Utility.Enums.LoginStatus.InvalidLogin.ToString();
                return userModel;
            }
            if (user.IsSuperAdmin != true)
            {
                if (userAdmin == null)//Login entries unmatched for user
                {
                    userModel.LoginStatus = Utility.Enums.LoginStatus.InvalidLogin.ToString();
                    return userModel;
                }
                //if (!(currentDate.Date >= Convert.ToDateTime(userAdmin.AccessPeriodFrom).Date && currentDate.Date <= Convert.ToDateTime(userAdmin.AccessPeriodTo).Date))//Account Access Time Invalid
                //{
                //    userModel.LoginStatus = Utility.Enums.LoginStatus.ExpiredLogin.ToString();
                //    return userModel;
                //}
                if (userModel.UserId != 0)//Accesible Account
                {

                    userModel.ProfileModel = new ProfileModel();
                    AutoMapper.Mapper.Map(userModel, userModel);
                    AutoMapper.Mapper.Map(user.Company, userModel.CompanyModel);
                    AutoMapper.Mapper.Map(user.Profile2, userModel.ProfileModel);

                    List<ProfilePermissionModel> profilePermissionModels = new List<ProfilePermissionModel>();
                    if (user.Profile2 != null)
                    {
                        AutoMapper.Mapper.Map(user.Profile2.ProfilePermission, profilePermissionModels);
                        userModel.ProfileModel.ProfilePermissionModels = profilePermissionModels;

                        ModulePermissionModel modulePermissionModel = new ModulePermissionModel();
                        AutoMapper.Mapper.Map(user.Profile2.ProfilePermission, profilePermissionModels);
                        userModel.ProfileModel.ProfilePermissionModels = profilePermissionModels;
                    }
                    //AccessForGoalModule Permission check
                    //if (userAdmin.AccessForGoalModule == false)
                    //{
                    //    userModel.ProfileModel.ProfilePermissionModels.ToList().ForEach(x =>
                    //    {
                    //        var moduleName = x.ModulePermission.Module.ModuleCONSTANT;
                    //        if (moduleName == "Goal" || moduleName == "MetricDashboard" || moduleName == "Metric" || moduleName == "RCA" || moduleName == "ManageActionList" || moduleName == "ManageActionItem")
                    //        {
                    //            userModel.ProfileModel.ProfilePermissionModels.Remove(x);
                    //        }
                    //    });
                    //}
                    //AccessForKPIModule Permission check
                    //int count=0;
                    //if (userAdmin.AccessForKPIModule == false)
                    //{
                    //    userModel.ProfileModel.ProfilePermissionModels.ToList().ForEach(x =>
                    //    {
                    //        var moduleName = x.ModulePermission.Module.ModuleCONSTANT;
                    //        if (moduleName == "KPI")
                    //            count++;
                    //        if (moduleName == "KPI")
                    //        {
                    //            userModel.ProfileModel.ProfilePermissionModels.Remove(x);
                    //        }
                    //    });
                    //}
                    userModel.LoginStatus = Utility.Enums.LoginStatus.ValidLogin.ToString();
                    return userModel;
                }
            }
            else
            {
                userModel.LoginStatus = Utility.Enums.LoginStatus.ValidLogin.ToString();
            }
            return userModel;
        }
        public EmployeeModel GetUser(int userId)
        {
            EmployeeModel usermodel = null;

            Employee listUsers = new Employee();
            listUsers = userRepository.GetAll(a => a.UserId == userId).Select(y => new Employee() { ReportTo = y.ReportTo }).SingleOrDefault();
            List<string> ReportTOName = userRepository.GetAll(x => x.UserId == listUsers.ReportTo).Select(x => x.FirstName + ' ' + x.LastName).ToList();
            

            Employee user = userRepository.SingleOrDefault(u => u.UserId == userId && u.RecordDeleted == false);
            usermodel = new EmployeeModel();
            AutoMapper.Mapper.Map(user, usermodel);
            AutoMapper.Mapper.Map(user.Profile2, usermodel.ProfileModel);
            AutoMapper.Mapper.Map(user.OrganizationUnit, usermodel.organizationUnitModel);
            AutoMapper.Mapper.Map(user.Referrer, usermodel.reffererModel);
            AutoMapper.Mapper.Map(user.Technology, usermodel.technologyModel);
            AutoMapper.Mapper.Map(user.Designation, usermodel.designationModel);
            if (ReportTOName.Count() > 0)
            {
                usermodel.ReportToFullName = ReportTOName[0];
            }
            return usermodel;
        }

        public EmployeeModel GetUsercomments(int loginbuserId, int userid)
        {
            EmployeeModel usermodel = null;
            Employee user = userRepository.SingleOrDefault(u => u.UserId == loginbuserId && u.RecordDeleted == false);
            usermodel = new EmployeeModel();
            AutoMapper.Mapper.Map(user, usermodel);
            AutoMapper.Mapper.Map(user.Profile2, usermodel.ProfileModel);
            AutoMapper.Mapper.Map(user.OrganizationUnit, usermodel.organizationUnitModel);

            return usermodel;

        }
        public List<EmployeeModel> GetAllUsers(int companyId, int userStatus, int exceptThisId = 0)
        {
            List<EmployeeModel> listUserModel = new List<EmployeeModel>();
            List<Employee> listUsers = new List<Employee>();
            listUsers = userRepository.GetAll(x => x.RecordDeleted == false && x.Status == userStatus && x.UserId != exceptThisId).ToList(); //x.CompanyId == companyId && 
            AutoMapper.Mapper.Map(listUsers, listUserModel);
            return listUserModel;
        }

        public List<EmployeeModel> GetAllUsers_Import(int companyId)
        {

            List<EmployeeModel> listUserModel = new List<EmployeeModel>();
            List<Employee> listUsers = new List<Employee>();
            listUsers = userRepository.GetAll(x => x.RecordDeleted == false && x.IsLinkSend == false).ToList(); //x.CompanyId == companyId &&
            AutoMapper.Mapper.Map(listUsers, listUserModel);
            return listUserModel;
        }


        public void updatemoreinfo_status(EmployeeModel userModel)
        {
            Employee user = userRepository.SingleOrDefault(x => x.UserId == userModel.UserId);

            user.Status = userModel.Status;

            userRepository.Update(user);
        }


        public string GenerateRandomAlphaNumericCode(int length)
        {
            string characterSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();

            //The below code will select the random characters from the set
            //and then the array of these characters are passed to string 
            //constructor to make an alphanumeric string
            string randomCode = new string(
                Enumerable.Repeat(characterSet, length)
                    .Select(set => set[random.Next(set.Length)])
                    .ToArray());
            return randomCode;
        }


        public List<EmployeeModel> GetCompanyUsers(ListingParameters listingParameters, ref int totalRecords)
        {
            int currentPage = listingParameters.CurrentPage;
            int pageSize = listingParameters.PageSize;
            int status = (int)EMPMGMT.Utility.Enums.UserStatus.AllUsers;
            List<EmployeeModel> listUserModel = new List<EmployeeModel>();
            List<Employee> listUsers = new List<Employee>();
            EmployeeModel objUserModel = new EmployeeModel();
            Employee objUsers = new Employee();
            string FirstName = null;
            string LastName = null;
            if (!string.IsNullOrEmpty(listingParameters.SearchText))
            {
                if (listingParameters.SearchText.Contains(' '))
                {
                    string[] search = listingParameters.SearchText.Split(' ');
                    if (search.Length < 3)
                    {
                        FirstName = search[0];
                        LastName = search[1];
                    }
                }
            }
            Expression<Func<Employee, bool>> whereCondition = x =>  (x.FirstName.Contains(listingParameters.SearchText == null ? x.FirstName : listingParameters.SearchText) || (x.LastName.Contains(listingParameters.SearchText == null ? x.LastName : listingParameters.SearchText)) || (x.FirstName.Contains(FirstName) && x.LastName.Contains(LastName)) || x.EmailId.Contains(listingParameters.SearchText == null ? x.EmailId : listingParameters.SearchText) || (x.EmpCode.Contains(listingParameters.SearchText == null ? x.EmpCode : listingParameters.SearchText))) && x.Status == (listingParameters.UserStatus == status ? x.Status : listingParameters.UserStatus) && x.RecordDeleted == false; //&& x.CompanyId == listingParameters.CompanyId //
            
            totalRecords = userRepository.Count(whereCondition);

            switch (listingParameters.OrderByColumn)
            {
                case "Name":
                    if (listingParameters.OrderBy == "desc")
                    {
                        listUsers = userRepository.GetPagedRecordsDecending(whereCondition, y => y.FirstName, x => x.LastName, currentPage, pageSize).ToList();
                    }
                    else
                    {
                        listUsers = userRepository.GetPagedRecords(whereCondition, y => y.FirstName, x => x.LastName, currentPage, pageSize).ToList();
                    }
                    break;
                case "EmpCode":
                    if (listingParameters.OrderBy == "desc")
                    {
                        listUsers = userRepository.GetPagedRecordsDecending(whereCondition, y => y.EmpCode, currentPage, pageSize).ToList();
                    }
                    else
                    {
                        listUsers = userRepository.GetPagedRecords(whereCondition, y => y.EmpCode, currentPage, pageSize).ToList();
                    }
                    break;
                case "EmailId":
                    if (listingParameters.OrderBy == "desc")
                    {
                        listUsers = userRepository.GetPagedRecordsDecending(whereCondition, y => y.EmailId, currentPage, pageSize).ToList();
                    }
                    else
                    {
                        listUsers = userRepository.GetPagedRecords(whereCondition, y => y.EmailId, currentPage, pageSize).ToList();
                    }
                    break;
                case "Profile":
                    if (listingParameters.OrderBy == "desc")
                    {
                        listUsers = userRepository.GetPagedRecordsDecending(whereCondition, y => y.Profile2.ProfileName, currentPage, pageSize).ToList();
                    }
                    else
                    {
                        listUsers = userRepository.GetPagedRecords(whereCondition, y => y.Profile2.ProfileName, currentPage, pageSize).ToList();
                    }
                    break;
                default:
                    listUsers = userRepository.GetPagedRecordsDecending(whereCondition, y => y.CreatedDate, currentPage, pageSize).ToList();
                    break;
            }
            AutoMapper.Mapper.Map(listUsers, listUserModel);
            return listUserModel;
        }



        public EmployeeModel UpdateUserStatus(EmployeeModel userModel)
        {
            Employee user = userRepository.SingleOrDefault(x => x.UserId == userModel.UserId && x.RecordDeleted == false);
            var checkSenior = userRepository.GetAll(x => x.ReportTo == userModel.UserId && x.RecordDeleted == false).Select(x => x.UserId).ToList();
            if (user == null || checkSenior.Count()>0)
            {
                return null;
            }
            
            user.ModifiedDate = DateTime.UtcNow;
            user.Status = userModel.Status;
            userRepository.Update(user);
            return userModel;
        }
        private EmployeeModel UpdateCompanyUserStatus(EmployeeModel userModel)
        {
            Employee user = userRepository.SingleOrDefault(x => x.UserId == userModel.UserId && x.RecordDeleted == false);
            if (user == null)
            {
                return null;
            }
            user.ModifiedDate = DateTime.UtcNow;
            user.Status = userModel.Status;
            userRepository.Update(user);
            return userModel;

        }
        public List<EmployeeModel> GetUsersListForAutocomplete(string searchString, int userStatus)
        {
            List<EmployeeModel> listUserModel = new List<EmployeeModel>();
            List<Employee> listUsers = new List<Employee>();
            searchString=searchString==null ? "" : searchString;
            listUsers = userRepository.GetAll(x => (x.FirstName.Contains(searchString) || ((x.FirstName ?? "") + " " + (x.LastName ?? "")).Contains(searchString) || x.LastName.Contains(searchString)) && x.RecordDeleted == false && x.Status == userStatus).Select(y => new Employee() { FirstName = y.FirstName, LastName = y.LastName, UserId = y.UserId }).ToList(); // && x.CompanyId == companyId Was not. But required if Bind according to CompanyId
            AutoMapper.Mapper.Map(listUsers, listUserModel);
            return listUserModel;
        }

        public int GetDefaultMdOrgUnitID(int companyId, int metricDashboardId)
        {
            EmployeeModel orgUnitModel = null;
            int MdOrgUnit = userRepository.SingleOrDefault(u => u.CompanyId == companyId).Company.CompanyId;
            orgUnitModel = new EmployeeModel();
            // AutoMapper.Mapper.Map(MdOrgUnit, orgUnitModel);
            //return orgUnitModel;
            return MdOrgUnit;
        }


        public void AddDefaultOrganisationUnit(string OrgUnitName, int CompanyId, int ProfileID)
        {
            userRepository.AddDefaultOrganisationUnit(OrgUnitName, CompanyId, ProfileID);
        }
        #region Action List
        #region GetUsersListForAutocomplete  ActionList
        public List<EmployeeModel> GetUsersListForAutocomplete(int companyId, int userStatus, string searchString)
        {
            List<EmployeeModel> listEmployeeModel = new List<EmployeeModel>();
            List<Employee> listEmployee = new List<Employee>();
            listEmployee = userRepository.GetAll(x => (x.FirstName.Contains(searchString) || ((x.FirstName ?? "") + " " + (x.LastName ?? "")).Contains(searchString) || x.LastName.Contains(searchString)) && x.RecordDeleted == false && x.Status == userStatus).ToList(); //&& x.CompanyId == companyId
            AutoMapper.Mapper.Map(listEmployee, listEmployeeModel);
            return listEmployeeModel;
        }
        public List<EmployeeModel> GetProjectUsersListForAutocomplete(int companyId, int userStatus, string searchString, int ProjectId)
        {
            List<EmployeeModel> listEmployeeModel = new List<EmployeeModel>();
            List<Employee> listEmployee = new List<Employee>();
            List<int> ProjectUsers = resourceRepository.GetAll(x => x.ProjectId == ProjectId && x.RecordDeleted == false).Select(x => x.UserId.Value).ToList();
            listEmployee = userRepository.GetAll(x => (x.FirstName.Contains(searchString) || ((x.FirstName ?? "") + " " + (x.LastName ?? "")).Contains(searchString) || x.LastName.Contains(searchString)) && x.RecordDeleted == false && x.Status == userStatus && ProjectUsers.Contains(x.UserId)).ToList(); //&& x.CompanyId == companyId
            AutoMapper.Mapper.Map(listEmployee, listEmployeeModel);
            return listEmployeeModel;
        }
        #endregion





        #endregion


        #region Delete Employee Record
        public bool DeleteEmployee(EmployeeModel employeeModel)
        {
            var checkSenior = userRepository.GetAll(x => x.ReportTo == employeeModel.UserId && x.RecordDeleted == false).Select(x=>x.UserId).ToList();
            Employee employee = userRepository.SingleOrDefault(r => r.UserId == employeeModel.UserId && r.RecordDeleted == false); //r.CompanyId == employeeModel.CompanyId &&             
         
            if (employee != null)
            {
                if (checkSenior.Count() > 0)
                { return false; }

                employee.RecordDeleted = employeeModel.RecordDeleted;
                employee.ModifiedBy = employeeModel.ModifiedBy;
                employee.ModifiedDate = employeeModel.ModifiedDate;
                employee.RecordDeleted = true;

                userRepository.Update(employee);
                return true;
            }
            return false;

        }
        #endregion



        public List<EmployeeModel> GetUsersListForAutocompleteById(string searchString, int userId)
        {
            List<EmployeeModel> listUserModel = new List<EmployeeModel>();
            List<Employee> listUsers = new List<Employee>();
            listUsers = userRepository.GetAll(a => a.UserId != userId && a.RecordDeleted == false && a.Status == 1).Select(y => new Employee() { FirstName = y.FirstName + " " + y.LastName, UserId = y.UserId }).ToList();
            AutoMapper.Mapper.Map(listUsers, listUserModel);
            return listUserModel;
        }
    }
}
