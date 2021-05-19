using AutoMapper;
using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;
using EMPMGMT.Utility;
using EMPMGMT.Web.Infrastructure;
using EMPMGMT.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Net;
using EMPMGMT.Business;
using System.Web.Script.Serialization;

namespace EMPMGMT.Web.Controllers
{
    [CustomAuthorize(Roles = Constants.AUTHENTICATION_ROLE_USER)]
    public partial class EmployeeController : Controller
    {
        private int CurrentCompanyId
        {
            get
            {
                return (int)SessionManagement.LoggedInUser.CompanyId;
            }
        }
        private int CurrentUserId
        {
            get { return SessionManagement.LoggedInUser.UserId; }
        }
        private IUserBusiness userBusiness;
        private ITechnologyBusiness technologyBusiness;
        private IDesignationBusiness designationBusiness;
        private IProfileBusiness profileBusiness;
        private IOrganizationUnitBusiness roleBusiness;
        private IProfilePermissionBusiness profilePermissionBusiness;
        private IReffererBusiness reffererBusiness;
        private IFileAttachmentsBusiness fileAttachmentsBusiness;
        private IProjectBusiness projectBusiness;
        private IActionListBussiness actionListBussiness;
        private IActionItemResponsibleBusiness actionItemResponsibleBusiness;
        private IResourcesBusiness resourcesBusiness;
        private ITimeSheetBusiness timeSheetBusiness;
        public IActionItemBusiness actionItemBusiness;
        public ILeavesItemBusinuss LeavesItemBusiness;
        public IActionItemCommentBusiness actionItemCommentBusiness;
        public ICategoryBusiness categoryBusiness;

        public static string erroremil, msg = "", MetricDashboardid = "";
        public static int flag = 0, metricid = 0;
        public string ReturnUrl { get { return SessionManagement.LoggedInUser.ReturnUrl; } set { SessionManagement.LoggedInUser.ReturnUrl = value; ; } }
        public EmployeeController(IActionItemResponsibleBusiness _actionItemResponsibleBusiness, IActionItemBusiness _actionItemBusiness, IActionItemCommentBusiness _actionItemCommentBusiness, IFileAttachmentsBusiness _fileAttachmentsBusiness, IUserBusiness _userBusiness, IProfileBusiness _iprofileBusiness, IOrganizationUnitBusiness _roleBusiness, IProfilePermissionBusiness _profilePermissionBusiness, IActionListBussiness _actionListBussiness, ITechnologyBusiness _technologyBusiness, IDesignationBusiness _designationBusiness, IReffererBusiness _reffererBusiness, IProjectBusiness _projectBusiness, IResourcesBusiness _resourcesBusiness, ITimeSheetBusiness _timeSheetBusiness, ICategoryBusiness _categoryBusiness, ILeavesItemBusinuss _LeavesItemBusiness)
        {
            actionItemResponsibleBusiness = _actionItemResponsibleBusiness;
            actionItemBusiness = _actionItemBusiness;
            LeavesItemBusiness = _LeavesItemBusiness;
            actionItemCommentBusiness = _actionItemCommentBusiness;

            fileAttachmentsBusiness = _fileAttachmentsBusiness;
            userBusiness = _userBusiness;
            profileBusiness = _iprofileBusiness;
            roleBusiness = _roleBusiness;
            profilePermissionBusiness = _profilePermissionBusiness;
            projectBusiness = _projectBusiness;
            actionListBussiness = _actionListBussiness;
            technologyBusiness = _technologyBusiness;
            designationBusiness = _designationBusiness;
            reffererBusiness = _reffererBusiness;
            resourcesBusiness = _resourcesBusiness;
            timeSheetBusiness = _timeSheetBusiness;
            categoryBusiness = _categoryBusiness;
        }
        public ActionResult Home()
        {
            if (SessionManagement.LoggedInUser.UserId != 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Site");
            }
        }
        public ActionResult LogOut()
        {
            SessionManagement.LoggedInUser = null;
            Session.Abandon();
            Session.RemoveAll();
            CommonFunctions.RemoveCookies();
            return RedirectToAction("Login", "Site");
        }

        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_VIEW)]
        public ActionResult Users(ListingParameters listingParameters)
        {

            List<EmployeeVM> userVMList = new List<EmployeeVM>();
            List<EmployeeModel> userModelList = new List<EmployeeModel>();
            int totalRecords = 0;
            listingParameters.PageSize = Convert.ToInt16(EMPMGMT.Utility.ReadConfiguration.PageSize);
            userModelList = userBusiness.GetUsers(listingParameters, ref totalRecords);
            AutoMapper.Mapper.Map(userModelList, userVMList);
            return Json(new
            {
                TotalRecords = totalRecords,
                DataList = userVMList
            }
          , JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_CREATE)]
        public ActionResult User(EmployeeVM userVM)
        {
            return View();
        }

        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_EDIT)]
        public ActionResult User_Form(String userId_Encrypted)
        {
            EmployeeVM profile = new EmployeeVM();
            if (!string.IsNullOrEmpty(userId_Encrypted))
            {
                int userId = userId_Encrypted.Decrypt();
                int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
                ViewBag.ErrorMessage = "";

                EmployeeModel userModel = userBusiness.GetUser(userId);
                AutoMapper.Mapper.Map(userModel, profile);
                ViewBag.LoggedInUser = (int)SessionManagement.LoggedInUser.UserId;
                if (userId != (int)SessionManagement.LoggedInUser.UserId)
                {
                    ViewBag.RolesList = roleBusiness.GetOrganizationUnitDDL(companyId);
                    ViewBag.ProfileList = profileBusiness.GetProfileDDL(companyId);
                }
                return View(profile);
            }
            return View(profile);
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_CREATE + "," + Constants.MODULE_USER + Constants.PERMISSION_EDIT)]
        public ActionResult User_Form(EmployeeVM userVM)
        {
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            if (ModelState.IsValid)
            {
                int userId = (int)SessionManagement.LoggedInUser.UserId;

                EmployeeModel userModel = new EmployeeModel();
                AutoMapper.Mapper.Map(userVM, userModel);
                userModel.CompanyId = companyId;


                if (!string.IsNullOrEmpty(userVM.UserId))
                {
                    userModel.ModifiedBy = userId;
                    userBusiness.UpdateUser(userModel);
                }
                else
                {
                    //userModel.CreatedbY = userId;
                    userBusiness.AddUser(userModel);

                }
                return RedirectToAction("UserList", "Employee");

            }
            ViewBag.RolesList = roleBusiness.GetOrganizationUnitDDL(companyId);
            ViewBag.ProfileList = profileBusiness.GetProfileDDL(companyId);
            return View(userVM);
        }

        [CustomAuthorize(Roles = Constants.MODULE_ORGANIZATIONAL_UNITS + Constants.PERMISSION_VIEW)]
        public ActionResult Designation()
        {
            ViewBag.ModuleName = "Unit";
            return View("OrganizationUnits");
        }

        [CustomAuthorize(Roles = Constants.MODULE_ORGANIZATIONAL_UNITS + Constants.PERMISSION_VIEW)]
        public ActionResult OrganizationUnits()
        {
            ViewBag.ModuleName = "Unit";
            return View();
        }

        private List<OrganizationUnitVM> GetOrnizationUnits()
        {

            List<OrganizationUnitVM> listOrgUnitVM = new List<OrganizationUnitVM>();
            List<OrganizationUnitModel> listOrgUnitModel = new List<OrganizationUnitModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            listOrgUnitModel = roleBusiness.GetOrganizationUnitByCompanyId(companyId);

            AutoMapper.Mapper.Map(listOrgUnitModel, listOrgUnitVM);
            //      listOrgUnitVM = listOrgUnitVM.BuildTree<OrganizationUnitVM>();
            return listOrgUnitVM;

        }

        public ActionResult OrganizationUnitsList()
        {
            List<OrganizationUnitVM> listOrgUnitVM = GetOrnizationUnits();
            return Json(new { OrganizationUnits = listOrgUnitVM }
                   , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_ORGANIZATIONAL_UNITS + Constants.PERMISSION_CREATE)]
        public ActionResult CreateOrgUnit(OrganizationUnitVM roleVM)
        {
            List<OrganizationUnitVM> listOrgUnitVM = new List<OrganizationUnitVM>();
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            bool _isAdded;
            if (ModelState.IsValid)
            {
                OrganizationUnitModel roleModel = new OrganizationUnitModel();
                AutoMapper.Mapper.Map(roleVM, roleModel);
                roleModel.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
                roleModel.CreatedBy = (int)SessionManagement.LoggedInUser.UserId;
                roleModel.CreatedDate = DateTime.UtcNow;
                roleModel.ModifiedDate = DateTime.UtcNow;
                _isAdded = roleBusiness.AddOrganizationUnit(roleModel);
                if (_isAdded == true)
                {
                    listOrgUnitVM = GetOrnizationUnits();
                    response.Status = Enums.ResponseResult.Success.ToString();
                    response.Message = "Success";
                }
                else
                {
                    response.Status = Enums.ResponseResult.Failure.ToString();
                    response.Message = "AlreadyExist";

                }
            }
            else
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                        response.Message += error.ErrorMessage;
                }                //response.Message = ModelState.err;

            }
            return Json(new { Response = response, OrganizationUnits = listOrgUnitVM });
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_ORGANIZATIONAL_UNITS + Constants.PERMISSION_EDIT)]
        public ActionResult EditOrgUnit(OrganizationUnitVM roleVM)
        {
            List<OrganizationUnitVM> listOrgUnitVM = new List<OrganizationUnitVM>();
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            if (ModelState.IsValid)
            {

                OrganizationUnitModel roleModel = new OrganizationUnitModel();
                roleVM.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
                AutoMapper.Mapper.Map(roleVM, roleModel);

                //   bool IsOrgNameAlreadyExist=roleBusiness.
                roleModel.ModifiedBy = (int)SessionManagement.LoggedInUser.UserId;
                roleModel.ModifiedDate = DateTime.UtcNow;
                string result = roleBusiness.UpdateOrganizationUnit(roleModel);
                response.Status = Enums.ResponseResult.Success.ToString();
                response.Message = result;
                listOrgUnitVM = GetOrnizationUnits();
            }
            else
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                        response.Message += error.ErrorMessage;
                }                //response.Message = ModelState.err;

            }
            return Json(new { Response = response, OrganizationUnits = listOrgUnitVM });
        }

        //**** Delete Organisation Unit... Now Working on this..
        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_ORGANIZATIONAL_UNITS + Constants.PERMISSION_DELETE)]
        public ActionResult DeleteOrganizationUnit(OrganizationUnitVM roleVM)
        {
            List<OrganizationUnitVM> listOrgUnitVM = new List<OrganizationUnitVM>();
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;


            OrganizationUnitModel roleModel = new OrganizationUnitModel();
            roleVM.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
            AutoMapper.Mapper.Map(roleVM, roleModel);

            //   bool IsOrgNameAlreadyExist=roleBusiness.
            roleModel.ModifiedBy = (int)SessionManagement.LoggedInUser.UserId;
            roleModel.ModifiedDate = DateTime.UtcNow;
            bool result = roleBusiness.DeleteOrganizationUnit(roleModel.OrgUnitId, roleModel.ParentOrgUnitId.Value, (int)SessionManagement.LoggedInUser.CompanyId);
            response.Status = Enums.ResponseResult.Success.ToString();
            response.Message = Convert.ToString(result);
            listOrgUnitVM = GetOrnizationUnits();

            return Json(new { Response = response, OrganizationUnits = listOrgUnitVM });
        }

        //*******Above Code*******************************//


        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_VIEW)]
        public ActionResult Profiles()
        {
            return View("Profiles", new ProfileVM());
        }

        public ActionResult ProfileTypeList(ListingParameters listingParameters)
        {


            List<ProfileVM> listProfileVM = new List<ProfileVM>();
            List<ProfileModel> listProfileModel = new List<ProfileModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            listingParameters.CompanyId = companyId;
            listProfileModel = profileBusiness.Profiles(listingParameters);
            AutoMapper.Mapper.Map(listProfileModel, listProfileVM);
            return Json(listProfileVM
               , JsonRequestBehavior.AllowGet);
        }


        public ActionResult Technologies(ListingParameters listingParameters)
        {

            List<TechnologyVM> technologyVM = new List<TechnologyVM>();
            List<TechnologyModel> technologyModel = new List<TechnologyModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            listingParameters.CompanyId = companyId;
            technologyModel = technologyBusiness.Technologies(listingParameters);
            AutoMapper.Mapper.Map(technologyModel, technologyVM);
            return Json(technologyVM
               , JsonRequestBehavior.AllowGet);

        }

        public ActionResult DesignationList(ListingParameters listingParameters)
        {
            List<DesignationVM> listDesignationVM = new List<DesignationVM>();
            List<DesignationModel> listDesignationModel = new List<DesignationModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            listingParameters.CompanyId = companyId;
            listDesignationModel = designationBusiness.DesignationList(listingParameters);
            AutoMapper.Mapper.Map(listDesignationModel, listDesignationVM);
            return Json(listDesignationVM
               , JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_CREATE)]
        public ActionResult CreateProfile()
        {
            return View(new ProfileVM());
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_CREATE)]
        public ActionResult CreateProfile(ProfileVM profileVM)
        {
            bool _isAdded;
            string msg = "";
            ProfileModel profileModel = new ProfileModel();
            AutoMapper.Mapper.Map(profileVM, profileModel);
            profileModel.CompanyId = SessionManagement.LoggedInUser.CompanyId;
            profileModel.CreatedBy = SessionManagement.LoggedInUser.UserId;
            profileModel.CreatedDate = DateTime.UtcNow;
            _isAdded = profileBusiness.AddProfile(profileModel);


            return Json(new

            {

                Message = "Profile Updated Successfully"

            }, JsonRequestBehavior.AllowGet);

            //if (_isAdded)
            //{
            //    return RedirectToAction("Profiles");
            //}
            //else
            //{
            //    // ViewBag.StatusMessage = "Profile Already Exists";
            //    ModelState.AddModelError("ProfileName", "Profile Already Exists.");
            //    return View(new ProfileVM());

            //}
        }

        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_EDIT)]
        [EncryptedActionParameter]
        public ActionResult EditProfile(string Id_encrypted)
        {
            ProfileModel profileModel = new ProfileModel();
            ProfileVM profileVM = new ProfileVM();
            profileModel = profileBusiness.GetProfileTypeById(Convert.ToInt32(Id_encrypted));
            AutoMapper.Mapper.Map(profileModel, profileVM);
            profileModel.ProfileId = (int)profileModel.ProfileId;


            return Json(new
            {
                Data = profileModel,
                Status = true
            }
          , JsonRequestBehavior.AllowGet);


        }

        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_EDIT)]
        public ActionResult Save_EditProfile(ProfileVM profileVM)
        {
            bool _isUpdate;
            string status = "";
            ProfileModel profileModel = new ProfileModel();
            AutoMapper.Mapper.Map(profileVM, profileModel);
            profileModel.ModifiedBy = SessionManagement.LoggedInUser.UserId;
            profileModel.ModifiedDate = DateTime.UtcNow;
            _isUpdate = profileBusiness.UpdateProfile(profileModel);
            if (_isUpdate)
            {
                return Json(new

                {

                    Message = "Profile Edited Successfully",
                    Status = "0"

                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new

                {

                    Message = "Profile Already Exist",
                    Status = "1"

                }, JsonRequestBehavior.AllowGet);


            }
        }

        [EncryptedActionParameter]
        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_VIEW)]
        public ActionResult ProfileDetail(string id_encrypted)
        {
            ProfileVM profileVm = new ProfileVM();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            ProfileModel profileModel = profileBusiness.GetProfileDetail(Convert.ToInt32(id_encrypted), null);
            profileModel.ProfilePermissionModels = profileModel.ProfilePermissionModels.Where(m => m.ModulePermission.Module.RecordDeleted == false && m.ModulePermissionId != 103 && m.ModulePermissionId != 108).OrderBy(x => x.ModulePermission.Module.SortOrder.GetValueOrDefault()).ToList();
            AutoMapper.Mapper.Map(profileModel, profileVm);
            return View(profileVm);

        }
        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_EDIT)]
        public ActionResult UpdateProfilePermissionAccess(List<ProfilePermissionModel> profilePermissionModel)
        {
            Response response = new Response();
            response.StatusCode = 500;
            int modifiedBy = SessionManagement.LoggedInUser.UserId;
            profilePermissionBusiness.UpdateProfilePermission(profilePermissionModel, modifiedBy);
            response.Status = Enums.ResponseResult.Success.ToString();
            return Json(response);
        }
        public ActionResult RoleAssignment()
        {
            List<EmployeeVM> userVMList = new List<EmployeeVM>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int exceptThisId = SessionManagement.LoggedInUser.UserId;
            int userStatus = (int)Enums.UserStatus.Active;
            List<EmployeeModel> listUserModel = userBusiness.GetAllUsers(companyId, userStatus, exceptThisId);
            AutoMapper.Mapper.Map(listUserModel, userVMList);
            ViewBag.RolesList = roleBusiness.GetOrganizationUnitDDL(companyId);
            return View(userVMList);
        }

        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_VIEW)]
        public ActionResult CreateProfileType()
        {
            return View(new ProfileVM());

        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_CREATE)]
        public ActionResult CreateProfileType(ProfileVM profileVM)
        {

            bool _isAdded;
            string status = "";
            ProfileModel profileModel = new ProfileModel();
            Mapper.Map(profileVM, profileModel);
            profileModel.CompanyId = SessionManagement.LoggedInUser.CompanyId;
            profileModel.CreatedBy = SessionManagement.LoggedInUser.UserId;
            profileModel.CreatedDate = DateTime.UtcNow;
            _isAdded = profileBusiness.AddProfile(profileModel);
            if (_isAdded)
            {
                msg = "Profile Saved Successfully";
                status = "0";
            }
            else
            {
                msg = "Profile Already Exists.";
                status = "1";

            }

            return Json(new


            {
                Message = msg,
                Status = status
            }, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_EDIT)]
        public ActionResult EditProfileType(string Id_encrypted)
        {
            ProfileModel profileModel = new ProfileModel();
            ProfileVM profileVM = new ProfileVM();
            profileModel = profileBusiness.GetProfileTypeById(Convert.ToInt32(Id_encrypted));
            Mapper.Map(profileModel, profileVM);
            return View(profileVM);

        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_EDIT)]
        public ActionResult EditProfileType(ProfileVM profileVM)
        {
            ProfileModel profileModel = new ProfileModel();
            Mapper.Map(profileVM, profileModel);
            profileModel.ModifiedBy = SessionManagement.LoggedInUser.UserId;
            profileModel.ModifiedDate = DateTime.UtcNow;
            profileBusiness.UpdateProfile(profileModel);
            return RedirectToAction("Profiles");

        }


        [EncryptedActionParameter]
        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_DELETE)]
        public ActionResult DeleteProfile(string profileId_encrypted, string reassignProfileId_encrypted)
        {
            bool result;
            result = userBusiness.DeleteProfile(Convert.ToInt32(profileId_encrypted), Convert.ToInt32(reassignProfileId_encrypted), SessionManagement.LoggedInUser.CompanyId.Value, SessionManagement.LoggedInUser.UserId);
            return Json(new
            {
                Status = result,
                JsonRequestBehavior.AllowGet
            });
        }

        [CustomAuthorize(Roles = Constants.MODULE_CATEGORY + Constants.PERMISSION_VIEW)]
        public ActionResult ManageCategory()
        {
            return View(new CategoryVM());
        }

        public JsonResult GetReffer(string query)
        {
            List<RefferrerVM> ReferrerVMList = new List<RefferrerVM>();
            List<ReffererModel> reffererModelList = reffererBusiness.GetUsersListForAutocomplete(query);
            AutoMapper.Mapper.Map(reffererModelList, ReferrerVMList);
            return Json(ReferrerVMList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetDesignation(string query)
        {
            List<EmployeeVM> DesignationVMList = new List<EmployeeVM>();
            int currentUser = SessionManagement.LoggedInUser.UserId;
            var selecteduser = Request.QueryString["UserId"];
            EmployeeVM userVm = new EmployeeVM();
            userVm.UserId = selecteduser;
            EmployeeModel employeeModel = new EmployeeModel();
            employeeModel.UserId = userVm.UserId.Decrypt();
            //AutoMapper.Mapper.Map(userVm, employeeModel);

            List<EmployeeModel> DesignationModelList = userBusiness.GetUsersListForAutocompleteById(query, employeeModel.UserId);
            AutoMapper.Mapper.Map(DesignationModelList, DesignationVMList);
            return Json(DesignationVMList, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// Used to Add the user/Employee Detail for particular company.
        /// Created By:  Sheffy
        /// Created On: 10june2014
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>
        #region  AddUser
        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_CREATE)]
        public ActionResult AddUser(EmployeeVM userVM)
        {
            string Userguid = Guid.NewGuid().ToString();
            EmployeeModel userModel = new EmployeeModel();
            EmployeeModel userModelExit = new EmployeeModel();
            userModel.ProfileId = userVM.ProfileId.Decrypt();
            userModel.ReferrerId = userVM.ReferrerId.Decrypt();
            userModel.TechnologyId = userVM.TechnologyId.Decrypt();
            userModel.DesignationId = userVM.DesignationId.Decrypt();
            userModel.ReportTo = userVM.ReportTo.Decrypt();
            AutoMapper.Mapper.Map(userVM, userModel);
            userModel.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
            userModel.CreatedBy = (int)SessionManagement.LoggedInUser.UserId;
            userModel.CreatedDate = DateTime.UtcNow;
            //   userModel.Password = CommonFunctions.GenerateUniqueNumber();
            string result = "";
            userModelExit = userBusiness.GetUserByEmailId(userVM.EmailId);
            if (userModelExit == null)
            {
                userModel.Status = (int)Enums.UserStatus.Deactive;
                userModel.UserGuid = Userguid;
                userBusiness.AddUser(userModel);
                SendMailToUser(userModel);
                userModel.IsLinkSend = (Boolean)true;
                userModel.LinkSendDate = DateTime.UtcNow;
                bool check = userBusiness.UpdateUserAfterMailSend(userModel.UserId, Userguid);
                if (check)
                {
                    result = "Successful";
                    return Json(result);
                }
                else
                {
                    result = "Failure";
                    return Json(result);
                }
            }
            else
            {
                result = "UserAlreadyExist";
                return Json(result);
            }
        }
        #endregion
        #region UserProfile
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_VIEW)]
        public ActionResult UserProfile()
        {
            ViewBag.successimport = false;
            return View(new EmployeeVM());

        }
        #endregion
        /// <summary>
        /// Used to Update the particular user/Employee Detail .
        /// Created By:  Sheffy
        /// Created On: 11june2014
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_EDIT)]
        public ActionResult UpdateUserProfile(EmployeeVM userVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            EmployeeModel userModel = new EmployeeModel();
            AutoMapper.Mapper.Map(userVM, userModel);
            userModel.UserId = (int)SessionManagement.LoggedInUser.UserId;
            userModel.Status = (int)Enums.UserStatus.Active;
            userModel.UserTypeId = (int)Enums.UserType.User;
            userModel.FirstName = userVM.FirstName;
            userModel.LastName = userVM.LastName;
            userModel.EmailId = userVM.EmailId;
            userModel.ImageURL = userVM.ImageURL;
            userBusiness.UpdateUserProfile(userModel);

            SessionManagement.LoggedInUser.UserName = userModel.FullName;
            return Json(response);
        }

        /// <summary>
        /// Used to Update the particular user/Employee Detail .
        /// Created By:  Sheffy
        /// Created On: 11june2014
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_EDIT)]
        public ActionResult UpdateUser(EmployeeVM userVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            EmployeeModel userModel = new EmployeeModel();
            AutoMapper.Mapper.Map(userVM, userModel);
            userModel.UserId = (int)SessionManagement.LoggedInUser.UserId;
            userModel.Status = (int)Enums.UserStatus.Deactive;

            userBusiness.UpdateUser(userModel);
            return Json(response);
        }
        /// <summary>
        /// Used to change/reset the password
        /// Created By:  Sheffy
        /// Created On: 11june2014
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_EDIT)]
        public ActionResult ResetPassword(EmployeeVM userVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            EmployeeModel userModel = new EmployeeModel();

            AutoMapper.Mapper.Map(userVM, userModel);
            userModel.UserId = (int)SessionManagement.LoggedInUser.UserId;
            userModel.DOB = null;
            userModel.ProfileId = (int)Enums.UserType.User;
            string result = userBusiness.ResetPassword(userModel);
            return Json(result);
        }
        public ActionResult Importuser()
        {

            return View(new EmployeeModel());

        }
        /// <summary>
        /// Fetch the loged-in user detail.
        /// Created By:  Sheffy
        /// Created On: 11june2014
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns></returns>
        /// 
        #region ViewUserProfile
        [HttpGet]
        public ActionResult GetUser()
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            EmployeeVM userVM = new EmployeeVM();
            EmployeeModel userModel = new EmployeeModel();
            AutoMapper.Mapper.Map(userVM, userModel);
            userModel.UserId = (int)SessionManagement.LoggedInUser.UserId;

            userModel = userBusiness.GetUser(userModel.UserId);
            AutoMapper.Mapper.Map(userModel, userVM);
            return Json(userVM
              , JsonRequestBehavior.AllowGet);
        }
        #endregion
        /// <summary>
        /// Upload profile Image 
        /// Created By:  Sheffy
        /// Created On: 12june2014
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns></returns>
        #region UploadImage
        [HttpPost]
        public JsonResult UploadImage()
        {
            if (Request.Files.Count == 0)
            {
                return Json(new { statusCode = 500, status = "No image provided." });
            }
            var file = Request.Files[0];
            var fileExtension = Path.GetExtension(file.FileName);
            var userId = Request.Form["UserId_encrypted"].Decrypt();

            int imageWidth = ReadConfiguration.ProfileImageWidth;
            int imageHeight = ReadConfiguration.ProfileImageHieght;
            if (userId == 0)
            {
                userId = Convert.ToInt32(CommonFunctions.GenerateUniqueNumberNumeric());
            }
            string imageName = Constants.PROFILE_IMAGE_NAME_PREFIX + userId + fileExtension;
            string imagePathAndName = Constants.PROFILE_IMAGE_PATH + imageName;
            string ImageSavingPath = Server.MapPath(@"~" + imagePathAndName);

            try
            {
                CommonFunctions.UploadFile(file, ImageSavingPath, imageWidth, imageHeight);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    statusCode = 500,
                    status = "Error uploading image.",
                    file = string.Empty
                });
            }
            userBusiness.ChangeImage(userId, imageName);
            if (SessionManagement.LoggedInUser.UserId == userId)
                SessionManagement.LoggedInUser.ProfileImageUrl = Constants.PROFILE_IMAGE_PATH + imageName + "?" + DateTime.Now.Millisecond;
            // Return JSON
            return Json(new
            {
                statusCode = 200,
                status = "Image uploaded.",
                file = imagePathAndName,
                imageName = imageName
            });

        }
        #endregion
        #region ViewUserProfile

        public ActionResult ViewUserProfile()
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            EmployeeVM userVM = new EmployeeVM();
            EmployeeModel userModel = new EmployeeModel();
            AutoMapper.Mapper.Map(userVM, userModel);
            userModel.UserId = (int)SessionManagement.LoggedInUser.UserId;

            userModel = userBusiness.GetUser(userModel.UserId);
            AutoMapper.Mapper.Map(userModel, userVM);
            return View(userVM);
        }
        #endregion
        #region ManageUser
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_VIEW)]
        public ActionResult ManageUser()
        {
            return View(new EmployeeVM());
        }
        #endregion

        #region GetCompanyUsers
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_VIEW)]
        public ActionResult GetCompanyUsers(ListingParameters listingParameters)
        {
            List<EmployeeVM> userVMList = new List<EmployeeVM>();
            List<EmployeeModel> userModelList = new List<EmployeeModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            string LogedInUserId = ((int)SessionManagement.LoggedInUser.UserId).Encrypt();
            int totalRecords = 0;
            listingParameters.CompanyId = companyId;
            listingParameters.PageSize = Convert.ToInt16(EMPMGMT.Utility.ReadConfiguration.PageSize);
            userModelList = userBusiness.GetCompanyUsers(listingParameters, ref totalRecords);
            AutoMapper.Mapper.Map(userModelList, userVMList);
            return Json(new
            {
                TotalRecords = totalRecords,
                DataList = userVMList,
                LoggedInUser = LogedInUserId
            }
          , JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region UpdateUserStatus
        public ActionResult UpdateUserStatus(EmployeeVM userVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            EmployeeModel userModel = new EmployeeModel();

            AutoMapper.Mapper.Map(userVM, userModel);

            userModel.Status = userVM.Status;
            userModel.ActivationDate = null;
            userModel.DOB = null;
            userModel.LinkSendDate = null;
            userModel.IsRegisteredUser = true;
            userModel = userBusiness.UpdateUserStatus(userModel);
            AutoMapper.Mapper.Map(userModel, userVM);
            return Json(userVM);
        }

        #endregion
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_VIEW)]
        public ActionResult uploaduser()
        {
            msg = "";
            return View();
        }
        #region UploadUsers
        [EncryptedActionParameter]

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_CREATE)]
        public ActionResult UploadUsers(string Id_encrypted)
        {
            msg = "";
            erroremil = "";
            DataSet dsobject;
            int flag = 0;
            var file = Request.Files[0];
            if (Request.Files.Count > 0)
            {
                string extension = System.IO.Path.GetExtension(file.FileName);

                if (extension == ".xlsx" || extension == ".xls")
                {


                }
                else
                {
                    msg = "File Not Supported.Format should be in .xlsx or .xls";
                    return Json(new { Status = "0", Message = msg });
                }


                string path1 = string.Format("{0}/{1}", Server.MapPath("~/Uploads"), file.FileName);
                if (System.IO.File.Exists(path1))
                    System.IO.File.Delete(path1);
                file.SaveAs(path1);
                dsobject = CommonFunctions.ImportExceltoDataset(path1);
                int rowcount = dsobject.Tables[0].Rows.Count;
                if (rowcount == 0)
                {

                    msg = "There is no data in excel file to upload";

                    return Json(new { Status = "0", Message = msg });

                }

                List<EmployeeModel> userModelList = new List<EmployeeModel>();


                for (int i = 0; i <= dsobject.Tables[0].Rows.Count - 1; i++)
                {
                    EmployeeModel userModel = new EmployeeModel();
                    userModel = userBusiness.GetUserByEmailId(dsobject.Tables[0].Rows[i][2].ToString());


                    if (dsobject.Tables[0].Rows[i][2].ToString() != "")
                    {
                        bool Chkemailformat = userBusiness.EmailIsValid(dsobject.Tables[0].Rows[i][2].ToString());

                        if (Chkemailformat)
                        {

                            if (userModel != null)
                            {
                                if (erroremil.ToString() == "")
                                {
                                    erroremil = dsobject.Tables[0].Rows[i][2].ToString();
                                }

                                else
                                {

                                    erroremil = erroremil + " </br>" + dsobject.Tables[0].Rows[i][2].ToString();
                                }
                            }
                            else
                            {
                                int checkflag = 0;
                                foreach (var checkusermail in userModelList)
                                {
                                    if (checkusermail.EmailId == dsobject.Tables[0].Rows[i][2].ToString())
                                    {
                                        if (erroremil.ToString() == "")
                                        {
                                            erroremil = dsobject.Tables[0].Rows[i][2].ToString();
                                        }

                                        else
                                        {

                                            erroremil = erroremil + " </br>" + dsobject.Tables[0].Rows[i][2].ToString();
                                        }
                                        checkflag = 1;
                                    }
                                }
                                userModel = new EmployeeModel();
                                userModel.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
                                userModel.CreatedBy = (int)SessionManagement.LoggedInUser.UserId;
                                userModel.CreatedDate = DateTime.UtcNow;
                                userModel.FirstName = dsobject.Tables[0].Rows[i][0].ToString();
                                userModel.LastName = dsobject.Tables[0].Rows[i][1].ToString();
                                userModel.EmailId = dsobject.Tables[0].Rows[i][2].ToString();
                                userModel.RecordDeleted = (Boolean)false;
                                userModel.CreatedDate = DateTime.UtcNow;
                                userModel.UserTypeId = (int)Enums.UserType.User;
                                userModel.Status = (int)Enums.UserStatus.Invited;
                                userModel.IsLinkSend = (Boolean)false;
                                userModel.LinkSendDate = DateTime.UtcNow;
                                userModel.ProfileId = Convert.ToInt32(Id_encrypted);
                                userModel.Status = (int)Enums.UserStatus.Active;
                                if (checkflag == 0)
                                {
                                    userModelList.Add(userModel);
                                }
                                checkflag = 0;
                                flag = 1;
                            }
                        }
                        else
                        {
                            if (erroremil.ToString() == "")
                            {
                                erroremil = "Wrong email : " + dsobject.Tables[0].Rows[i][2].ToString();
                            }

                            else
                            {

                                erroremil = erroremil + " </br>" + "Wrong email : " + dsobject.Tables[0].Rows[i][2].ToString();
                            }

                        }
                    }
                    else
                    {


                    }
                }
                userBusiness.AddUsers(userModelList);
                if (erroremil.ToString() == "")
                {
                    if (flag == 1)
                    {
                        msg = "Users uploaded successfully";
                    }
                    else
                    {
                        msg = "There is no data in excel file to upload";
                    }
                }
                else
                {
                    msg = "Users uploaded successfully.These below emails not registered with us due to duplicacy : </br>" + erroremil;
                }
            }
            else
            {
                msg = "Please upload user file first";
            }

            return Json(new { Status = "0", Message = msg });
        }
        #endregion
        #region ResendMailToUser
        [HttpPost]
        public ActionResult ResendMailToUser(EmployeeVM userVM)
        {
            string Userguid = Guid.NewGuid().ToString();
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;

            EmployeeModel userModel = new EmployeeModel();

            AutoMapper.Mapper.Map(userVM, userModel);

            string result = "";
            userModel.IsLinkSend = (Boolean)true;
            userModel.LinkSendDate = DateTime.UtcNow;
            SendMailToUser(userModel);


            bool isupdated = userBusiness.UpdateUserAfterMailSend(userModel.UserId, Userguid);
            if (isupdated)
            {
                result = "Successful";
                response.Status = Enums.ResponseResult.Success.ToString();
                response.StatusCode = 200;
            }
            else
            {
                result = "Error";
            }
            return Json(result);

        }
        #endregion

        #region SendMailToUser
        private void SendMailToUser(EmployeeModel user)
        {
            var userid = user.UserId.Encrypt();
            MailHelper mailHelper = new MailHelper();
            MailBodyTemplate mailBodyTemplate = new MailBodyTemplate();
            string mailTemplateFolder = ReadConfiguration.MailTemplateFolder;
            string mailBody = string.Empty;
            EmployeeModel userModel = new EmployeeModel();
            userModel.UserId = user.UserId;
            userModel.Status = (int)Enums.UserStatus.Active;
            userModel.Password = user.Password;
            userModel.Comments = user.Comments;
            mailHelper.Subject = Constants.RegistrationUserActivationMailSubject;
            mailHelper.ToEmail = user.EmailId;
            mailBody = CommonFunctions.ReadFile(Server.MapPath(mailTemplateFolder + Constants.UserInvitationActivationMailFileName));
            mailBodyTemplate.RegistrationUserName = user.FullName;
            mailBodyTemplate.MailBody = mailBody;
            mailBodyTemplate.AccountLoginUserId = user.EmailId;
            mailBodyTemplate.AccountLoginUrl = ReadConfiguration.WebsiteUrl + "ResetPassword?uid=" + user.UserGuid;
            mailBody = CommonFunctions.ConfigureActivationMailBody(mailBodyTemplate);
            mailHelper.Body = mailBody;
            mailHelper.SendEmail();
        }
        #endregion
        #region SendMail_UploadUsers
        public ActionResult SendMail_UploadUsers()
        {
            string responseMessage = string.Empty;
            string Userguid = Guid.NewGuid().ToString();
            List<EmployeeVM> userVMList = new List<EmployeeVM>();
            List<EmployeeModel> userModelList = new List<EmployeeModel>();
            string euserid = "";
            userModelList = userBusiness.GetAllUsers_Import((int)SessionManagement.LoggedInUser.CompanyId);
            AutoMapper.Mapper.Map(userModelList, userVMList);
            foreach (var user in userModelList)
            {
                euserid = "";
                euserid = user.UserId.Encrypt();
                user.UserGuid = Userguid;
                SendMailToUser(user);
                int UserId = (int)user.UserId;
                bool check = userBusiness.UpdateUserAfterMailSend(UserId, Userguid);
                if (check == false)
                {
                    msg = "User uploded sucessfull but user status not updated yet";

                }
            }
            return Json(new { Status = "0", Message = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region AddUserView
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_CREATE)]
        public ActionResult AddUserView()
        {
            ViewBag.UserDetailMode = "EditView";
            return View("UserDetails");
        }
        #endregion
        #region EditUserView
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_EDIT)]
        public ActionResult EditUserView(string id_Encrypted)
        {
            ViewBag.UserDetailMode = "EditView";
            EmployeeVM userVm = new EmployeeVM();
            userVm.UserId = id_Encrypted;
            //EmployeeModel employeeModel = new EmployeeModel();
            //employeeModel.UserId = userVm.UserId.Decrypt();
            //AutoMapper.Mapper.Map(userVm, employeeModel);
            return View("UserDetails", userVm);
        }
        #endregion
        #region PopUpInitialize
        [HttpPost]
        public PartialViewResult PopUpInitialize()
        {
            return PartialView("_FileUploaded");
        }
        #endregion
        #region UserDetails

        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_VIEW)]
        public ActionResult UserDetails(string id_Encrypted)
        {
            ViewBag.UserDetailMode = "View";
            EmployeeVM userVm = new EmployeeVM();
            userVm.UserId = id_Encrypted;
            userVm.Status = (int)Enums.UserStatus.Invited;
            return View(userVm);
        }
        #endregion
        /// <summary>
        /// Fetch the particular user detail.
        /// Created By:  Sheffy
        /// Created On: 16june2014
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <returns></returns>
        #region GetUserDetail

        public ActionResult GetUserDetail(EmployeeVM userVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            string LogrdInUser = ((int)SessionManagement.LoggedInUser.UserId).Encrypt();
            response.StatusCode = 500;
            EmployeeModel userModel = new EmployeeModel();
            // userModel. = (int)SessionManagement.LoggedInUser.CompanyId;

            AutoMapper.Mapper.Map(userVM, userModel);
            userModel = userBusiness.GetUser(userModel.UserId);
            AutoMapper.Mapper.Map(userModel, userVM);


            return Json(new
            {
                userVM = userVM,
                LoggedInUser = LogrdInUser,

            }
        , JsonRequestBehavior.AllowGet);

            //return Json(userVM
            //  , JsonRequestBehavior.AllowGet);

        }
        #endregion
        /// <summary>
        /// Reset Password by Company Admin for  users
        /// Created By:  Sheffy
        /// Created On: 17june2014
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>

        #region ResetUserPassword
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_EDIT)]
        [HttpPost]
        public ActionResult ResetUserPassword(EmployeeVM userVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            EmployeeModel userModel = new EmployeeModel();
            AutoMapper.Mapper.Map(userVM, userModel);
            userModel.UserId = userVM.UserId.Decrypt();
            userModel.ActivationDate = null;
            userModel.DOB = null;
            bool result = userBusiness.UpdateUser_ResetPassword(userModel.UserId, userVM.Password);

            string resultData = "";
            if (result)
            {
                resultData = "Successful";
            }
            else
            {
                resultData = "InvalidUser";
            }
            return Json(resultData);

        }

        #endregion

        /// <summary>
        /// Used to Update the particular user/Employee Detail .
        /// Created By:  Sheffy
        /// Created On: 11june2014
        /// Modified By:
        /// Modified On:
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>       
        #region EditUserDetail
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_EDIT)]
        [HttpPost]
        public ActionResult EditUserDetail(EmployeeVM userVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            //UserVM userVM = new UserVM();
            EmployeeModel userModel = new EmployeeModel();
            userModel.UserId = userVM.UserId.Decrypt();
            userModel.ProfileId = userVM.ProfileId.Decrypt();
            userModel.ReferrerId = userVM.ReferrerId.Decrypt();
            userModel.TechnologyId = userVM.TechnologyId.Decrypt();
            userModel.DesignationId = userVM.DesignationId.Decrypt();
            userModel.ReportTo = userVM.ReportTo.Decrypt();
            if (userVM.OrgUnitId != "0")
            {
                userModel.OrgUnitId = userVM.OrgUnitId.Decrypt();
            }
            if (userModel.ReferrerId == 0)
            {
                userModel.ReferrerId = null;
            }
            if (userModel.TechnologyId == 0)
            { userModel.TechnologyId = null; }


            if (userModel.DesignationId == 0)
            { userModel.DesignationId = null; }



            AutoMapper.Mapper.Map(userVM, userModel);

            //userModel.Status = (int)Enums.UserStatus.Active;
            userModel.ActivationDate = null;
            userModel.LinkSendDate = null;
            userModel.CreatedDate = null;
            userBusiness.UpdateUser(userModel);
            return Json(response);
        }

        #endregion
        public ActionResult AutoComplete()
        {
            return View();
        }


        #region AutoCompleteList
        public JsonResult AutoCompleteList(int UserId, string query)
        {
            //string[] names = new string[3] {"Matt", "Joanne", "Robert"};
            List<EmployeeModel> userList = new List<EmployeeModel>();
            userList.Add(new EmployeeModel() { FirstName = "Rakesh", LastName = "Rana" });
            userList.Add(new EmployeeModel() { FirstName = "Mukesh", LastName = "Kumar" });
            userList.Add(new EmployeeModel() { FirstName = "Gobind", LastName = "Singh" });
            return Json(userList, JsonRequestBehavior.AllowGet);

        }
        #endregion
        [CustomAuthorize(Roles = Constants.MODULE_METRIC + Constants.PERMISSION_CREATE + "," + Constants.MODULE_METRIC + Constants.PERMISSION_EDIT)]

        #region Metric Dashboard

        public ActionResult ManageMetricDashboard()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_ORGANIZATIONAL_UNITS + Constants.PERMISSION_EDIT)]


        public ActionResult GetDocumentsList(FileAttachmentsVM fileAttachmentVM)
        {

            List<FileAttachmentsVM> listFileAttachmentsVM = new List<FileAttachmentsVM>();
            List<FileAttachmentsModel> listFileAttachmentsModel = new List<FileAttachmentsModel>();
            FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();
            AutoMapper.Mapper.Map(fileAttachmentVM, fileAttachmentsModel);
            listFileAttachmentsModel = fileAttachmentsBusiness.GetDocumentsList(fileAttachmentsModel.UserId);
            AutoMapper.Mapper.Map(listFileAttachmentsModel, listFileAttachmentsVM);
            return Json(new
            {
                DataList = listFileAttachmentsVM
            }
          , JsonRequestBehavior.AllowGet);
        }

        [EncryptedActionParameter]
        [HttpPost]


        #endregion

        #region Metric Dashboard Values



        public ActionResult DeleteMetricDasboardDocument(FileAttachmentsVM fileAttachmentsVM)
        {
            int userId = SessionManagement.LoggedInUser.UserId;
            List<FileAttachmentsModel> listFileAttachmentsModel = new List<FileAttachmentsModel>();

            Response response = new Response();
            response.Status = Enums.ResponseResult.Success.ToString();
            response.StatusCode = 200;
            int UserId = SessionManagement.LoggedInUser.UserId;
            fileAttachmentsVM.ModifiedBy = UserId;
            fileAttachmentsVM.ModifiedDate = DateTime.UtcNow;
            FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();
            AutoMapper.Mapper.Map(fileAttachmentsVM, fileAttachmentsModel);
            fileAttachmentsBusiness.DeleteDocument(fileAttachmentsModel);
            return Json(response);
        }

        public ActionResult GetActionListDocuments(FileAttachmentsVM fileAttachmentVM)
        {

            List<FileAttachmentsVM> listFileAttachmentsVM = new List<FileAttachmentsVM>();
            List<FileAttachmentsModel> listFileAttachmentsModel = new List<FileAttachmentsModel>();
            FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();
            AutoMapper.Mapper.Map(fileAttachmentVM, fileAttachmentsModel);
            listFileAttachmentsModel = fileAttachmentsBusiness.GetDocumentsListByActionList(fileAttachmentsModel.ActionListId);
            AutoMapper.Mapper.Map(listFileAttachmentsModel, listFileAttachmentsVM);
            return Json(new
            {
                DataList = listFileAttachmentsVM.Where(x => x.RecordDeleted == false).ToList(),
                ImagePath = Constants.ACTION_LIST_DOCUMENTS_PATH
            }
          , JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public ActionResult SaveMetricDashboardWeekData(MetricDashboardWeekDataVM metricDashboardWeekDataVM)
        //{
        //    Response response = new Response();
        //    response.Status = Enums.ResponseResult.Failure.ToString();
        //    response.StatusCode = 500;
        //    bool _isSaved;

        //    MetricDashboardWeekDataModel metricDashboardWeekDataModel = new MetricDashboardWeekDataModel();
        //    AutoMapper.Mapper.Map(metricDashboardWeekDataVM, metricDashboardWeekDataModel);

        //    _isSaved = metricDashboardWeekDataBusiness.SaveMetricDashboardWeekData(metricDashboardWeekDataModel);
        //    if (_isSaved == true)
        //    {
        //        response.Status = Enums.ResponseResult.Success.ToString();
        //        response.Message = "Success";
        //        response.StatusCode = 200;
        //    }
        //    else
        //    {
        //        response.Status = Enums.ResponseResult.Failure.ToString();
        //        response.Message = "Error";
        //        response.StatusCode = 500;

        //    }

        //    return Json(response);
        //}

        //[HttpPost]
        //public ActionResult SaveMetricDashboardMonthlyData(MetricDashboardMonthlyDataVM metricDashboardMonthlyDataVM)
        //{
        //    Response response = new Response();
        //    response.Status = Enums.ResponseResult.Failure.ToString();
        //    response.StatusCode = 500;
        //    bool _isSaved;

        //    MetricDashboardMonthlyDataModel metricDashboardMonthlyDataModel = new MetricDashboardMonthlyDataModel();
        //    AutoMapper.Mapper.Map(metricDashboardMonthlyDataVM, metricDashboardMonthlyDataModel);

        //    _isSaved = metricDashboardMonthlyDataBusiness.SaveMetricDashboardMonthlyData(metricDashboardMonthlyDataModel);
        //    if (_isSaved == true)
        //    {
        //        response.Status = Enums.ResponseResult.Success.ToString();
        //        response.Message = "Success";
        //        response.StatusCode = 200;
        //    }
        //    else
        //    {
        //        response.Status = Enums.ResponseResult.Failure.ToString();
        //        response.Message = "Error";
        //        response.StatusCode = 500;

        //    }

        //    return Json(response);
        //}

        //[HttpPost]
        //public ActionResult SaveMetricDashboardDailyData(MetricDashboardDailyDataVM metricDashboardDailyDataVM)
        //{
        //    Response response = new Response();
        //    response.Status = Enums.ResponseResult.Failure.ToString();
        //    response.StatusCode = 500;
        //    bool _isSaved;

        //    MetricDashboardDailyDataModel metricDashboardDailyDataModel = new MetricDashboardDailyDataModel();
        //    AutoMapper.Mapper.Map(metricDashboardDailyDataVM, metricDashboardDailyDataModel);

        //    _isSaved = metricDashboardDailyDataBusiness.SaveMetricDashboardDailyData(metricDashboardDailyDataModel);
        //    if (_isSaved == true)
        //    {
        //        response.Status = Enums.ResponseResult.Success.ToString();
        //        response.Message = "Success";
        //        response.StatusCode = 200;

        //    }
        //    else
        //    {
        //        response.Status = Enums.ResponseResult.Failure.ToString();
        //        response.Message = "Error";
        //        response.StatusCode = 500;

        //    }

        //    return Json(response);
        //}
        #endregion

        #region GetFirstDateOfWeek
        public static DateTime GetFirstDateOfWeek(int year, int weekOfYear)
        {

            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)(jan1.DayOfWeek);

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = jan1.AddDays((weekNum * 7));
            return result;
        }
        #endregion


        #region Action List ManageActionList

        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_VIEW)]
        public ActionResult ManageActionList(string id_Encrypted)
        {
            ActionListVM actionListVM = new ActionListVM();
            actionListVM.ProjectId = id_Encrypted;
            int LoggInUserId = SessionManagement.LoggedInUser.UserId;
            int ProjectId = actionListVM.ProjectId.Decrypt();
            ActionListModel actionListModel = actionListBussiness.ProjectActionList(ProjectId, LoggInUserId);
            AutoMapper.Mapper.Map(actionListModel, actionListVM);
            return View(actionListVM);
        }
        #endregion
        #region UploadUserDocument
        [EncryptedActionParameter]
        [HttpPost]

        public JsonResult UploadUserDocument(string UserId_encrypted)
        {
            if (Request.Files.Count == 0)
            {
                return Json(new { statusCode = 500, status = "No file uploaded." });
            }
            var file = Request.Files[0];
            var fileExtension = Path.GetExtension(file.FileName);
            var userId = (int)SessionManagement.LoggedInUser.UserId;
            var cryptedusedId = UserId_encrypted;

            var DocumentName = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileExtension;
            string imagePathAndName = Constants.USER_DOCUMENTS_PATH + DocumentName;
            string ImageSavingPath = Server.MapPath(@"~" + imagePathAndName);

            try
            {
                CommonFunctions.UploadFile(file, ImageSavingPath);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    statusCode = 500,
                    status = "Error uploading Document.",
                    file = string.Empty
                });
            }
            FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();
            fileAttachmentsModel.AttachedBy = userId;
            fileAttachmentsModel.CreatedBy = userId;
            fileAttachmentsModel.CreatedDate = DateTime.Now;
            fileAttachmentsModel.ModifiedBy = userId;
            fileAttachmentsModel.ModifiedDate = DateTime.Now;
            fileAttachmentsModel.RecordDeleted = false;
            fileAttachmentsModel.UserId = Convert.ToInt32(cryptedusedId);
            fileAttachmentsModel.DocumentName = Path.GetFileNameWithoutExtension(file.FileName);
            fileAttachmentsModel.DocumentPath = DocumentName;
            fileAttachmentsModel = fileAttachmentsBusiness.SaveDocuments(fileAttachmentsModel);
            FileAttachmentsModel fileAttachmentsModelNew = new FileAttachmentsModel();
            fileAttachmentsModelNew = fileAttachmentsBusiness.GetDocumentsListByDocumentId(fileAttachmentsModel.DocumentId);
            FileAttachmentsVM fileAttachmentsVM = new FileAttachmentsVM();
            AutoMapper.Mapper.Map(fileAttachmentsModelNew, fileAttachmentsVM);
            // Return JSON
            return Json(new
            {
                statusCode = 200,
                status = "Image uploaded.",
                DataList = fileAttachmentsVM,
                AttachedByName = SessionManagement.LoggedInUser.UserName,
                ImagePath = Constants.USER_DOCUMENTS_PATH
            });
        }
        #endregion

        #region Delete Employee
        [CustomAuthorize(Roles = Constants.MODULE_Documents + Constants.PERMISSION_DELETEDOCUMENT)]
        public ActionResult DeleteEmployee(EmployeeVM EmployeeVM)
        {
            Response response = new Response();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int Userid = (int)SessionManagement.LoggedInUser.UserId;
            EmployeeModel employeeModel = new EmployeeModel();

            AutoMapper.Mapper.Map(EmployeeVM, employeeModel);
            employeeModel.CompanyId = companyId;
            employeeModel.RecordDeleted = true;
            employeeModel.ModifiedDate = DateTime.UtcNow;
            employeeModel.ModifiedBy = Userid;
            try
            {
                bool _IsDeleted = userBusiness.DeleteEmployee(employeeModel);
                if (_IsDeleted == true)
                {
                    response.Status = Enums.ResponseResult.Success.ToString();
                    response.StatusCode = 200;
                    response.Message = "Record Deleted Successfully";
                }
                else
                {
                    response.Status = Enums.ResponseResult.StageOrderExist.ToString();
                    response.StatusCode = 200;
                    response.Message = "This Record can't Deleted, because this contains many child Users ";
                }
            }
            catch
            {
                response.Status = Enums.ResponseResult.Failure.ToString();
                response.StatusCode = 500;
            }

            return Json(response);
        }
        #endregion
        #region GetActionListItem
        [CustomAuthorize(Roles = Constants.MODULE_MANAGE_ACTION_LIST + Constants.PERMISSION_VIEW)]
        public ActionResult GetActionListItem(ListingParameters listingParameters)
        {
            List<ActionListVM> actionListVM = new List<ActionListVM>();
            List<ActionListModel> actionListModel = new List<ActionListModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int LogedInUserId = ((int)SessionManagement.LoggedInUser.UserId);
            int totalRecords = 0;
            listingParameters.CompanyId = companyId;

            listingParameters.PageSize = Convert.ToInt16(EMPMGMT.Utility.ReadConfiguration.PageSize);
            actionListModel = actionListBussiness.GetActionList(listingParameters, LogedInUserId, ref totalRecords);
            AutoMapper.Mapper.Map(actionListModel, actionListVM);
            return Json(new
            {
                TotalRecords = totalRecords,
                DataList = actionListVM
            }
          , JsonRequestBehavior.AllowGet);


        }
        #endregion
        #region DeleteDocument
        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_DELETE)]
        public ActionResult DeleteDocument(FileAttachmentsVM fileAttachmentsVM)
        {
            int userId = SessionManagement.LoggedInUser.UserId;
            List<FileAttachmentsModel> listFileAttachmentsModel = new List<FileAttachmentsModel>();

            Response response = new Response();
            response.Status = Enums.ResponseResult.Success.ToString();
            response.StatusCode = 200;
            int UserId = SessionManagement.LoggedInUser.UserId;
            fileAttachmentsVM.ModifiedBy = UserId;
            fileAttachmentsVM.ModifiedDate = DateTime.UtcNow;
            FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();
            AutoMapper.Mapper.Map(fileAttachmentsVM, fileAttachmentsModel);
            fileAttachmentsBusiness.DeleteDocument(fileAttachmentsModel);
            return Json(response);
        }
        #endregion

        #region ActionList Delete Edit Create
        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_MANAGE_ACTION_LIST + Constants.PERMISSION_DELETE)]
        public ActionResult DeleteActionList(ActionListVM actionListVM)
        {
            Response response = new Response();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int Userid = (int)SessionManagement.LoggedInUser.UserId;
            ActionListModel actionListModel = new ActionListModel();

            AutoMapper.Mapper.Map(actionListVM, actionListModel);
            actionListModel.CompanyId = companyId;
            actionListModel.RecordDeleted = true;
            actionListModel.ModifiedDate = DateTime.UtcNow;
            actionListModel.ModifiedBy = Userid;
            try
            {
                bool _IsDeleted = actionListBussiness.DeleteActionList(actionListModel);
                response.Status = Enums.ResponseResult.Success.ToString();
                response.StatusCode = 200;
            }
            catch
            {
                response.Status = Enums.ResponseResult.Failure.ToString();
                response.StatusCode = 500;
            }

            return Json(response);
        }

        [CustomAuthorize(Roles = Constants.MODULE_MANAGE_ACTION_LIST + Constants.PERMISSION_CREATE + "," + Constants.MODULE_MANAGE_ACTION_LIST + Constants.PERMISSION_EDIT)]
        public ActionResult ActionList(string id_Encrypted)
        {
            ReturnUrl = Request.UrlReferrer == null ? ReturnUrl : Request.UrlReferrer.ToString();
            ActionListVM actionListVM = new ActionListVM();
            actionListVM.ActionListId = null;
            actionListVM.ProjectId = id_Encrypted;
            ActionListModel actionListModel = new ActionListModel();
            AutoMapper.Mapper.Map(actionListVM, actionListModel);
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int Userid = (int)SessionManagement.LoggedInUser.UserId;
            actionListVM.Url = "/Project/ManageActionList/" + actionListVM.ProjectId;
            return View("ActionList", actionListVM);
        }

        [CustomAuthorize(Roles = Constants.MODULE_MANAGE_ACTION_LIST + Constants.PERMISSION_CREATE + "," + Constants.MODULE_MANAGE_ACTION_LIST + Constants.PERMISSION_EDIT)]
        public ActionResult EditActionList(string id_Encrypted)
        {
            ReturnUrl = Request.UrlReferrer == null ? ReturnUrl : Request.UrlReferrer.ToString();
            ActionListVM actionListVM = new ActionListVM();
            actionListVM.ActionListId = id_Encrypted;
            ActionListModel actionListModel = new ActionListModel();
            AutoMapper.Mapper.Map(actionListVM, actionListModel);
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int Userid = (int)SessionManagement.LoggedInUser.UserId;
            if (id_Encrypted is object)
            {
                actionListModel = actionListBussiness.GetActionListDetail(actionListModel);
                AutoMapper.Mapper.Map(actionListModel, actionListVM);

            }
            actionListVM.Url = "/Project/ManageActionList/" + actionListVM.ProjectId;
            return View("ActionList", actionListVM);
        }

        #region UploadActionListDocument


        [EncryptedActionParameter]
        [HttpPost]

        public JsonResult UploadActionListDocument(String ActionListId_encrypted)
        {
            if (Request.Files.Count == 0)
            {
                return Json(new { statusCode = 500, status = "No file uploaded." });
            }
            var file = Request.Files[0];
            var fileExtension = Path.GetExtension(file.FileName);
            var userId = (int)SessionManagement.LoggedInUser.UserId;// Request.Form["UserId"].Decrypt();
            var actionListId = ActionListId_encrypted;

            var DocumentName = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileExtension;
            string imagePathAndName = Constants.ACTION_LIST_DOCUMENTS_PATH + DocumentName;
            string ImageSavingPath = Server.MapPath(@"~" + imagePathAndName);

            try
            {
                CommonFunctions.UploadFile(file, ImageSavingPath);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    statusCode = 500,
                    status = "Error uploading image.",
                    file = string.Empty
                });
            }

            FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();
            fileAttachmentsModel.AttachedBy = userId;
            fileAttachmentsModel.CreatedBy = userId;
            fileAttachmentsModel.CreatedDate = DateTime.Now;
            fileAttachmentsModel.ModifiedBy = userId;
            fileAttachmentsModel.ModifiedDate = DateTime.Now;
            fileAttachmentsModel.RecordDeleted = false;
            fileAttachmentsModel.UserId = null;

            fileAttachmentsModel.ActionListId = Convert.ToInt32(actionListId);
            fileAttachmentsModel.DocumentName = file.FileName;
            fileAttachmentsModel.DocumentPath = DocumentName;
            fileAttachmentsModel = fileAttachmentsBusiness.SaveActionListDashboardDocuments(fileAttachmentsModel);
            FileAttachmentsModel fileAttachmentsModelNew = new FileAttachmentsModel();
            FileAttachmentsVM fileAttachmentsVM = new FileAttachmentsVM();
            fileAttachmentsModelNew = fileAttachmentsBusiness.GetDocumentsListByActionListId(fileAttachmentsModel.DocumentId);
            AutoMapper.Mapper.Map(fileAttachmentsModelNew, fileAttachmentsVM);
            // Return JSON
            return Json(new
            {
                statusCode = 200,
                status = "Image uploaded.",
                DataList = fileAttachmentsVM,
                AttachedByName = SessionManagement.LoggedInUser.UserName,
                ImagePath = Constants.ACTION_LIST_DOCUMENTS_PATH

            });
        }

        #endregion
        #endregion

        #region UploadActionItemDocument

        [EncryptedActionParameter]
        [HttpPost]
        public JsonResult UploadActionItemDocument(String ActionItemId_encrypted)
        {
            if (Request.Files.Count == 0)
            {
                return Json(new { statusCode = 500, status = "No file uploaded." });
            }
            var file = Request.Files[0];
            var fileExtension = Path.GetExtension(file.FileName);
            var userId = (int)SessionManagement.LoggedInUser.UserId;// Request.Form["UserId"].Decrypt();
            var actionItemId = (ActionItemId_encrypted == "0" || ActionItemId_encrypted == "" || ActionItemId_encrypted == null) ? 0 : Convert.ToInt32(ActionItemId_encrypted);
            if (actionItemId == 0)
            {
                actionItemId = CommonFunctions.GenerateUniqueNumberNumeric();
            }

            var DocumentName = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileExtension;
            string imagePathAndName = Constants.ACTION_ITEM_DOCUMENTS_PATH + DocumentName;
            string ImageSavingPath = Server.MapPath(@"~" + imagePathAndName);

            try
            {
                CommonFunctions.UploadFile(file, ImageSavingPath);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    statusCode = 500,
                    status = "Error uploading image.",
                    file = string.Empty
                });
            }
            FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();
            fileAttachmentsModel.AttachedBy = userId;
            fileAttachmentsModel.CreatedBy = userId;
            fileAttachmentsModel.CreatedDate = DateTime.Now;
            fileAttachmentsModel.ModifiedBy = userId;
            fileAttachmentsModel.ModifiedDate = DateTime.Now;
            fileAttachmentsModel.RecordDeleted = false;
            fileAttachmentsModel.ActionItemId = Convert.ToInt32(actionItemId);
            // fileAttachmentsModel.DocumentName = Path.GetFileNameWithoutExtension(file.FileName);
            fileAttachmentsModel.DocumentName = file.FileName;
            fileAttachmentsModel.DocumentPath = DocumentName;
            fileAttachmentsModel = fileAttachmentsBusiness.SaveMetricDashboardDocuments(fileAttachmentsModel);
            FileAttachmentsModel fileAttachmentsModelNew = new FileAttachmentsModel();
            fileAttachmentsModelNew = fileAttachmentsBusiness.GetDocumentsListByDocumentId(fileAttachmentsModel.DocumentId);
            FileAttachmentsVM fileAttachmentsVM = new FileAttachmentsVM();
            AutoMapper.Mapper.Map(fileAttachmentsModelNew, fileAttachmentsVM);
            // Return JSON
            return Json(new
            {
                statusCode = 200,
                status = "Image uploaded.",
                DataList = fileAttachmentsVM,
                AttachedByName = SessionManagement.LoggedInUser.UserName,
                ImagePath = Constants.ACTION_ITEM_DOCUMENTS_PATH,
                documentName = file.FileName,
                documentfileName = DocumentName

            });
        }

        #endregion


        #region GetUserDocuments
        public ActionResult GetUserDocuments(string UserId_encrypted)
        {
            var cryptedusedId = UserId_encrypted;
            List<FileAttachmentsVM> listFileAttachmentsVM = new List<FileAttachmentsVM>();
            List<FileAttachmentsModel> listFileAttachmentsModel = new List<FileAttachmentsModel>();
            FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();
            fileAttachmentsModel.UserId = Convert.ToInt32(cryptedusedId.Decrypt());
            listFileAttachmentsModel = fileAttachmentsBusiness.GetUserDocumentsList(fileAttachmentsModel.UserId);
            AutoMapper.Mapper.Map(listFileAttachmentsModel, listFileAttachmentsVM);
            return Json(new
            {
                DataList = listFileAttachmentsVM,
                ImagePath = Constants.USER_DOCUMENTS_PATH
            }
          , JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  GetActionListing
        public JsonResult GetResponsibles(string query, string ResponsibleusersId)
        {
            int companyId;
            companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            List<EmployeeVM> employeeVM = new List<EmployeeVM>();
            int active = (int)Enums.UserStatus.Active;
            List<EmployeeModel> employeeModel = userBusiness.GetUsersListForAutocomplete(companyId, active, query);
            AutoMapper.Mapper.Map(employeeModel, employeeVM);
            return Json(employeeVM, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetProjectResponsibles(string Id_encrypted)
        {
            int companyId;
            companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            List<EmployeeVM> employeeVM = new List<EmployeeVM>();
            int active = (int)Enums.UserStatus.Active;
            List<EmployeeModel> employeeModel = userBusiness.GetProjectUsersListForAutocomplete(companyId, active, "", Id_encrypted.Decrypt());
            AutoMapper.Mapper.Map(employeeModel, employeeVM);
            return Json(employeeVM, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[EncryptedActionParameter]
        [CustomAuthorize(Roles = Constants.MODULE_MANAGE_ACTION_LIST + Constants.PERMISSION_CREATE + "," + Constants.MODULE_MANAGE_ACTION_LIST + Constants.PERMISSION_EDIT)]
        public ActionResult ActionList(ActionListVM actionListVM, string id_Encrypted, string ProjectId)
        {
            if (ModelState.IsValid)
            {
                Response response = new Response();
                response.Status = Enums.ResponseResult.Failure.ToString();
                response.StatusCode = 500;
                //int actionListId = id_Encrypted.Decrypt();
                int actionListId = actionListVM.ActionListId.Decrypt();
                int projectId = ProjectId.Decrypt();
                ActionListModel actionListModel = new ActionListModel();
                AutoMapper.Mapper.Map(actionListVM, actionListModel);
                actionListModel.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
                actionListModel.CreatedBy = (int)SessionManagement.LoggedInUser.UserId;
                actionListModel.ModifiedBy = (int)SessionManagement.LoggedInUser.UserId;
                actionListModel.CreatedDate = DateTime.UtcNow;
                actionListModel.ModifiedDate = DateTime.UtcNow;


                if (actionListVM.ActionListId != null && actionListVM.ProjectId != null)
                {
                    actionListModel.ActionListId = actionListId;
                    actionListModel.ProjectId = projectId;
                    string _isUpdatedMsg = actionListBussiness.UpdateActionList(actionListModel);

                    if (_isUpdatedMsg == "AlreadyExist")
                    {

                        ViewBag.MetricDashboardId = id_Encrypted;
                        response.Status = Enums.ResponseResult.Success.ToString();
                        response.Message = "AlreadyExist";
                        response.StatusCode = 200;
                        ModelState.AddModelError("Title", "Action list already exists");
                        return View("ActionList", actionListVM);

                    }
                    else if (_isUpdatedMsg == "Success")
                    {
                        response.Status = Enums.ResponseResult.Success.ToString();
                        response.Message = "UpdateSuccessfully";
                        response.StatusCode = 200;
                    }
                    else
                    {
                        response.Message = "Failure";
                    }

                }
                else
                {
                    actionListModel.ProjectId = id_Encrypted.Decrypt();
                    actionListId = actionListBussiness.SaveActionList(actionListModel);
                    if (actionListId > 0)
                    {
                        response.Status = Enums.ResponseResult.Success.ToString();
                        response.Message = "Success";
                        response.StatusCode = 200;
                    }
                    else
                    {

                        response.Status = Enums.ResponseResult.Failure.ToString();
                        response.Message = "AlreadyExist";
                        response.StatusCode = 200;
                        ModelState.AddModelError("Title", "Action list already exists");

                    }

                }
                if (response.Message == "UpdateSuccessfully" || response.Message == "Success")
                {

                    if (!string.IsNullOrEmpty(ReturnUrl)) return Redirect(ReturnUrl);
                    else
                        return RedirectToAction("/ManageActionList");
                }
                else
                {
                    return View(new ActionListVM());
                }

            }
            else
            {
                return View(new ActionListVM());

            }
        }
        public ActionResult ActionListing()
        {
            return View();
        }

        [HttpGet]
        [CustomAuthorize(Roles = Constants.MODULE_MANAGE_ACTION_LIST + Constants.PERMISSION_VIEW)]
        public ActionResult GetActionListing(ListingParameters listingParameters, string id_Encrypted)
        {
            int totalRecords;
            listingParameters.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
            listingParameters.PageSize = ReadConfiguration.PageSize;
            int ProjectId = id_Encrypted.Decrypt();
            List<ActionListModel> actionListModel = actionListBussiness.GetActionListing(listingParameters, out totalRecords);
            List<ActionListVM> actionListVM = new List<ActionListVM>();
            AutoMapper.Mapper.Map(actionListModel, actionListVM);
            return Json(new
            {
                DataList = actionListVM,
                TotalRecords = totalRecords
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Action Item ActionItemDescription

        [EncryptedActionParameter]
        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_VIEW)]
        public ActionResult ActionItemDescription(string id_Encrypted)
        {
            ActionItemModel actionItemModel = new ActionItemModel();
            ActionItemVM actionItemVM = new ActionItemVM();
            List<FileAttachmentsVM> listFileAttachmentsVM = new List<FileAttachmentsVM>();
            List<FileAttachmentsModel> listFileAttachmentsModel = new List<FileAttachmentsModel>();
            actionItemModel = actionItemBusiness.GetActionItemDescriptionByActionItemId(Convert.ToInt32(id_Encrypted));
            listFileAttachmentsModel = fileAttachmentsBusiness.GetDocumentsListByActionItem(Convert.ToInt32(id_Encrypted));
            AutoMapper.Mapper.Map(listFileAttachmentsModel, listFileAttachmentsVM);

            AutoMapper.Mapper.Map(actionItemModel, actionItemVM);
            return View(actionItemVM);
        }



        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_MANAGE_ACTION_ITEM + Constants.PERMISSION_DELETE)]
        public ActionResult DeleteActionItem(ActionItemVM actionItemVM)
        {
            ActionItemModel actionItemModel = new ActionItemModel();
            AutoMapper.Mapper.Map(actionItemVM, actionItemModel);
            actionItemBusiness.DeleteActionItem(actionItemModel);

            return null;


        }

        [CustomAuthorize(Roles = Constants.MODULE_MANAGE_ACTION_ITEM + Constants.PERMISSION_VIEW)]
        public ActionResult GetActionItemListByActionListId(ActionItemVM actionItemVM)
        {
            ActionItemModel actionItemModel = new ActionItemModel();
            AutoMapper.Mapper.Map(actionItemVM, actionItemModel);
            List<ActionItemVM> listACtionItemVM = new List<ActionItemVM>();
            List<ActionItemModel> lstActionItemModel = actionItemBusiness.GetActionItemListByACtionListId(actionItemModel);
            AutoMapper.Mapper.Map(lstActionItemModel, listACtionItemVM);
            return Json(new
            {
                DataList = listACtionItemVM,

            }
        , JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = Constants.MODULE_MANAGE_ACTION_ITEM + Constants.PERMISSION_VIEW)]

        public ActionResult GetActionItemResponsibleByActionItem(ActionItemResponsibleVM actionItemResponsibleVM)
        {
            ActionItemResponsibleModel actionItemResponsibleModel = new ActionItemResponsibleModel();
            AutoMapper.Mapper.Map(actionItemResponsibleVM, actionItemResponsibleModel);
            List<ActionItemResponsibleVM> listACtionItemResponsibleVM = new List<ActionItemResponsibleVM>();
            List<ActionItemResponsibleModel> listACtionItemResponsibleModel = actionItemResponsibleBusiness.GetActionItemResponsibleByActionItem(actionItemResponsibleModel.ActionItemId);
            AutoMapper.Mapper.Map(listACtionItemResponsibleModel, listACtionItemResponsibleVM);
            return Json(new
            {
                DataList = listACtionItemResponsibleVM,

            }
        , JsonRequestBehavior.AllowGet);
        }

        [EncryptedActionParameter]
        [CustomAuthorize(Roles = Constants.MODULE_MANAGE_ACTION_ITEM + Constants.PERMISSION_VIEW)]
        public ActionResult ManageActionItem(string id_Encrypted)
        {
            ActionListVM actionListVM = new ActionListVM();
            ActionListModel actionListModel = new ActionListModel();
            actionListModel.ActionListId = Convert.ToInt32(id_Encrypted);
            actionListModel.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
            actionListModel = actionListBussiness.GetActionListDetail(actionListModel);
            AutoMapper.Mapper.Map(actionListModel, actionListVM);
            ViewBag.ActionListId = actionListVM.ActionListId;
            ViewBag.ProjectId = actionListVM.ProjectId;
            return View(actionListVM);

        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_MANAGE_ACTION_ITEM + Constants.PERMISSION_CREATE + "," + Constants.MODULE_MANAGE_ACTION_ITEM + Constants.PERMISSION_EDIT)]
        public ActionResult SaveActionItemRecord(ActionItemVM actionItemVM)
        {
            int userId = (int)SessionManagement.LoggedInUser.UserId;
            ActionItemModel actionItemModel = new ActionItemModel();
            AutoMapper.Mapper.Map(actionItemVM, actionItemModel);
            ActionItemResponsibleModel objActionItemResponsibleModel = new ActionItemResponsibleModel();
            List<ActionItemResponsibleModel> listActionItemResponsibleModel = new List<ActionItemResponsibleModel>();
            List<ActionItemResponsibleModel> listOfActionItemResponsibleModel = new List<ActionItemResponsibleModel>();
            AutoMapper.Mapper.Map(actionItemVM.ListActionItemResponsible, listActionItemResponsibleModel);
            int actionItemId = 0;
            actionItemModel.CreatedBy = userId;
            actionItemModel.CreatedDate = DateTime.UtcNow;



            if (actionItemVM.ActionItemId != null && actionItemVM.ActionItemId != "")
            {
                actionItemId = actionItemBusiness.UpdateActionItemRecord(actionItemModel);
            }
            else
            {
                actionItemId = actionItemBusiness.SaveActionItemRecord(actionItemModel);
            }

            foreach (ActionItemResponsibleModel lst in listActionItemResponsibleModel)
            {
                objActionItemResponsibleModel = new ActionItemResponsibleModel();
                objActionItemResponsibleModel.CreatedBy = userId;
                objActionItemResponsibleModel.CreatedDate = DateTime.UtcNow;
                objActionItemResponsibleModel.ActionItemId = actionItemId;
                objActionItemResponsibleModel.ResponsibleUserId = lst.ResponsibleUserId;
                objActionItemResponsibleModel.RecordDeleted = lst.RecordDeleted;
                listOfActionItemResponsibleModel.Add(objActionItemResponsibleModel);

            }
            if (actionItemId != 0)
            {
                actionItemResponsibleBusiness.SaveActionItemResponsible(listOfActionItemResponsibleModel);

                if ((actionItemVM.FileName == "" || actionItemVM.FileName == null) && (actionItemVM.DocumentName == "" || actionItemVM.DocumentName == null))
                { }
                else if ((actionItemVM.ActionItemId == null || actionItemVM.ActionItemId == "") && (actionItemVM.FileName != "") && (actionItemVM.DocumentName != ""))
                {
                    FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();
                    fileAttachmentsModel.AttachedBy = userId;
                    fileAttachmentsModel.CreatedBy = userId;
                    fileAttachmentsModel.CreatedDate = DateTime.Now;
                    fileAttachmentsModel.ModifiedBy = userId;
                    fileAttachmentsModel.ModifiedDate = DateTime.Now;
                    fileAttachmentsModel.RecordDeleted = false;
                    fileAttachmentsModel.ActionItemId = Convert.ToInt32(actionItemId);
                    // fileAttachmentsModel.DocumentName = Path.GetFileNameWithoutExtension(file.FileName);
                    fileAttachmentsModel.DocumentName = actionItemVM.DocumentName;
                    fileAttachmentsModel.DocumentPath = actionItemVM.FileName;
                    fileAttachmentsModel = fileAttachmentsBusiness.SaveMetricDashboardDocuments(fileAttachmentsModel);
                }
            }

            return Json(new
            {
                ActionItemId = actionItemId
                //ImagePath = Constants.ACTION_ITEM_DOCUMENTS_PATH
            }
          , JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetActionItemListFromActionId(ActionItemVM actionItemVM)
        {
            ActionItemModel actionItemModel = new ActionItemModel();
            List<ActionItemVM> listActionItemVM = new List<ActionItemVM>();
            List<ActionItemModel> listActionItemModel = new List<ActionItemModel>();

            AutoMapper.Mapper.Map(actionItemVM, actionItemModel);
            listActionItemModel = actionItemBusiness.GetActionItemListFromActionId(actionItemModel.ActionListId, actionItemVM.OnTime, actionItemVM.OverDue, actionItemVM.BeforeDue, actionItemVM.SearchText ?? "", actionItemModel.StartDate, actionItemModel.DueDate, actionItemModel.StatusDrop, actionItemModel.ResponsibleUserId);// == 0 ? (int)SessionManagement.LoggedInUser.UserId : actionItemModel.ResponsibleUserId);
            actionItemVM.StartDate = actionItemVM.StartDate;
            actionItemVM.DueDate = actionItemVM.DueDate;

            AutoMapper.Mapper.Map(listActionItemModel, listActionItemVM);
            listActionItemVM = listActionItemVM.BuildTree<ActionItemVM>();
            return Json(new
            {
                DataList = listActionItemVM,

            }
          , JsonRequestBehavior.AllowGet);

        }




        [EncryptedActionParameter]
        [CustomAuthorize(Roles = Constants.MODULE_MANAGE_ACTION_ITEM + Constants.PERMISSION_VIEW)]
        public ActionResult GetActionItemList(ListingParameters listingParameters)
        {
            ActionItemModel objActionItemModel = new ActionItemModel();
            ActionItemVM objActionItemVM = new ActionItemVM();
            objActionItemVM.ActionListId = listingParameters.ActionListId;
            AutoMapper.Mapper.Map(objActionItemVM, objActionItemModel);
            List<ActionItemVM> actionItemVM = new List<ActionItemVM>();
            List<ActionItemModel> actionItemModel = new List<ActionItemModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            //string LogedInUserId = ((int)SessionManagement.LoggedInUser.UserId).Encrypt();
            int totalRecords = 0;
            listingParameters.CompanyId = companyId;
            listingParameters.PageSize = Convert.ToInt16(EMPMGMT.Utility.ReadConfiguration.PageSize);
            actionItemModel = actionItemBusiness.GetActionItemList(listingParameters, objActionItemModel.ActionListId, ref totalRecords);
            AutoMapper.Mapper.Map(actionItemModel, actionItemVM);

            //List< ActionListVM> actionListVM = new  List< ActionListVM>();
            //List<ActionListModel> actionListModel = new List<ActionListModel>();
            // actionListModel = actionListBussiness.GetActionListForAutoComplete(companyId,"");
            // AutoMapper.Mapper.Map(actionListModel, actionListVM);



            return Json(new
            {
                TotalRecords = totalRecords,
                DataList = actionItemVM,
                // ActionList = actionListVM
            }
          , JsonRequestBehavior.AllowGet);


        }
        #endregion

        [CustomAuthorize(Roles = Constants.MODULE_RCA + Constants.PERMISSION_VIEW)]
        public ActionResult RootCauseActionList()
        {
            return View();
        }

        #region GetOrgUnitList
        public JsonResult GetOrgUnitList(string query)
        {
            int companyId;
            companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            List<OrganizationUnitVM> organizationUnitVMList = new List<OrganizationUnitVM>();
            List<OrganizationUnitModel> organizationUnitModelList = roleBusiness.GetOrgUnitListForAutoComplete(companyId, query);
            AutoMapper.Mapper.Map(organizationUnitModelList, organizationUnitVMList);
            return Json(organizationUnitVMList, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #region DeleteDocumentFile
        [HttpPost]
        public JsonResult DeleteDocumentFile(FileAttachmentsVM fileAttachmentVM)
        {
            FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();
            AutoMapper.Mapper.Map(fileAttachmentVM, fileAttachmentsModel);
            fileAttachmentsModel.ModifiedBy = CurrentUserId;
            fileAttachmentsBusiness.DeleteDocument(fileAttachmentsModel);
            AutoMapper.Mapper.Map(fileAttachmentsModel, fileAttachmentVM);
            return Json(new { Data = fileAttachmentVM });
        }
        #endregion

        public ActionResult KeyPerformanceIndicatorList()
        {
            return View();
        }



        public ActionResult KeyPerformanceIndicatorLevelList()
        {
            return View();
        }

        public ActionResult KPIHierarchy()
        {
            return View();
        }

        #region  GetActionItemDocuments
        [CustomAuthorize(Roles = Constants.MODULE_MANAGE_ACTION_ITEM + Constants.PERMISSION_VIEW)]
        public ActionResult GetActionItemDocuments(FileAttachmentsVM fileAttachmentVM)
        {

            List<FileAttachmentsVM> listFileAttachmentsVM = new List<FileAttachmentsVM>();
            List<FileAttachmentsModel> listFileAttachmentsModel = new List<FileAttachmentsModel>();
            FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();
            AutoMapper.Mapper.Map(fileAttachmentVM, fileAttachmentsModel);
            listFileAttachmentsModel = fileAttachmentsBusiness.GetDocumentsListByActionItem(fileAttachmentsModel.ActionItemId);
            AutoMapper.Mapper.Map(listFileAttachmentsModel, listFileAttachmentsVM);
            return Json(new
            {
                DataList = listFileAttachmentsVM,
                ImagePath = Constants.ACTION_ITEM_DOCUMENTS_PATH
            }
          , JsonRequestBehavior.AllowGet);
        }

        #endregion



        #region ProjectList

        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_VIEW)]
        public ActionResult ProjectList()
        {
            return View();
        }

        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_VIEW)]
        public ActionResult GetProjectList(ListingParameters listingParameters)
        {
            List<ProjectVM> projectVM = new List<ProjectVM>();
            List<ProjectModel> projectModel = new List<ProjectModel>();
            string LogedInUserId = ((int)SessionManagement.LoggedInUser.UserId).Encrypt();
            int totalRecords = 0;
            listingParameters.PageSize = Convert.ToInt16(EMPMGMT.Utility.ReadConfiguration.PageSize);
            projectModel = projectBusiness.GetProjectList(listingParameters, ref totalRecords, (int)SessionManagement.LoggedInUser.UserId);
            AutoMapper.Mapper.Map(projectModel, projectVM);
            return Json(new
            {
                TotalRecords = totalRecords,
                DataList = projectVM,
                LoggedInUser = LogedInUserId
            }
          , JsonRequestBehavior.AllowGet);
        }

        [EncryptedActionParameter]
        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_EDIT)]
        public ActionResult EditProject(string Id_Encrypted)
        {
            ProjectModel projectModel = new ProjectModel();
            ProjectVM projectVM = new ProjectVM();
            projectModel.ProjectId = Convert.ToInt32(Id_Encrypted);
            projectBusiness.ProjectDetails(projectModel);

            AutoMapper.Mapper.Map(projectModel, projectVM);

            var jsonSerialiser = new JavaScriptSerializer();
            ViewBag.json = jsonSerialiser.Serialize(new { List = projectVM.Resources.Where(a => a.RecordDeleted == false).Select(a => new { UserId = a.UserId, FullName = a.FullName, ProjectId = a.ProjectId }) });


            return View("AddEditProjectDetails", projectVM);

        }


        [EncryptedActionParameter]
        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_EDIT)]
        [HttpPost]
        public ActionResult EditProject(ProjectVM projectVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            int project = 0;

            ProjectModel projectModel = new ProjectModel();
            List<ResourcesModel> listResourcesModel = new List<ResourcesModel>();
            List<ResourcesModel> listofResourcesModel = new List<ResourcesModel>();
            int projectId = projectVM.ProjectId.Decrypt();
            string GblprojectList = projectVM.GblprojectLead;
            if (GblprojectList != null && GblprojectList != "undefined")
            {
                string[] GblprojectLead = GblprojectList.Split(new char[] { ',' });
                AutoMapper.Mapper.Map(projectVM.ListResources, listResourcesModel);
                projectModel.ProjectId = projectId;
                //string LogedInUserId = ((int)SessionManagement.LoggedInUser.UserId).Encrypt();
                projectVM.ModifiedBy = (int)SessionManagement.LoggedInUser.UserId;// LogedInUserId.Decrypt();
                projectVM.ModifiedDate = DateTime.UtcNow;
                projectVM.RecordDeleted = false;
                AutoMapper.Mapper.Map(projectVM, projectModel);
                //    projectBusiness.UpdateProject(projectModel);

                string _isUpdatedMsg = projectBusiness.UpdateProject(projectModel);
                if (_isUpdatedMsg == "ProjectCode")
                {

                    response.Status = Enums.ResponseResult.Success.ToString();
                    response.Message = "AlreadyExist";
                    response.StatusCode = 200;
                    ModelState.AddModelError("ProjectCode", "Project Code  already exists");
                }
                else if (_isUpdatedMsg == "ProjectName")
                {
                    response.Status = Enums.ResponseResult.Success.ToString();
                    response.Message = "ProjectName";
                    response.StatusCode = 200;
                    ModelState.AddModelError("ProjectName", "Project Name  already exists");
                }
                else if (_isUpdatedMsg == "Success")
                {

                    for (int i = 0; i < GblprojectLead.Count(); i++)
                    {


                        ResourcesModel resourcemodel = new ResourcesModel();
                        resourcemodel.ProjectId = projectId;
                        resourcemodel.UserId = GblprojectLead[i].Decrypt();
                        resourcemodel.CreatedBy = (int)SessionManagement.LoggedInUser.UserId;
                        resourcemodel.CreatedDate = DateTime.UtcNow;
                        resourcemodel.ModifiedBy = (int)SessionManagement.LoggedInUser.UserId;
                        resourcemodel.ModifiedDate = DateTime.UtcNow;
                        resourcemodel.RecordDeleted = false;
                        listResourcesModel.Add(resourcemodel);

                    }
                    resourcesBusiness.SaveResources(listResourcesModel);

                    response.Status = Enums.ResponseResult.Success.ToString();
                    response.Message = "UpdateSuccessfully";
                    response.StatusCode = 200;
                    return RedirectToAction("ProjectList");

                }
                //   return RedirectToAction("ProjectList");
            }
            projectModel.ProjectId = Convert.ToInt32(projectVM.ProjectId.Decrypt());
            projectBusiness.ProjectDetails(projectModel);

            AutoMapper.Mapper.Map(projectModel, projectVM);

            var jsonSerialiser = new JavaScriptSerializer();
            ViewBag.json = jsonSerialiser.Serialize(new { List = projectVM.Resources.Where(a => a.RecordDeleted == false).Select(a => new { UserId = a.UserId, FullName = a.FullName, ProjectId = a.ProjectId }) });

            return View("AddEditProjectDetails", projectVM);
        }

        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_CREATE)]
        public ActionResult AddProject(string Id_Encrypted)
        {
            ProjectVM projectVM = new ProjectVM();
            projectVM.ProjectId = Id_Encrypted;
            return View("AddEditProjectDetails", projectVM);

        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_CREATE)]
        public ActionResult AddProject(ProjectVM projectVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            ProjectModel projectModel = new ProjectModel();
            projectVM.CreatedBy = (int)SessionManagement.LoggedInUser.UserId;
            projectVM.CreatedDate = DateTime.UtcNow;
            projectVM.ModifiedDate = DateTime.UtcNow;
            projectVM.RecordDeleted = false;
            int projectLead = projectVM.ProjectLead.Decrypt();
            AutoMapper.Mapper.Map(projectVM, projectModel);
            projectBusiness.AddProject(projectModel);
            AutoMapper.Mapper.Map(projectModel, projectVM);

            if (projectModel.ErrorProjectName == true)
            {
                response.Status = Enums.ResponseResult.Success.ToString();
                response.StatusCode = 200;
                ModelState.AddModelError("ProjectName", "Project Name already exists.");
            }
            else if (projectModel.ErrorProjectCode == true)
            {
                response.Status = Enums.ResponseResult.Success.ToString();
                response.StatusCode = 200;
                ModelState.AddModelError("ProjectCode", "Project Code already exists.");
            }
            else
            {
                List<ResourcesModel> listResourcesModel = new List<ResourcesModel>();
                List<ResourcesModel> listofResourcesModel = new List<ResourcesModel>();

                int projectId = projectVM.ProjectId.Decrypt();
                string GblprojectList = projectVM.GblprojectLead;
                if (GblprojectList != null && GblprojectList != "undefined")
                {
                    string[] GblprojectLead = GblprojectList.Split(new char[] { ',' });
                    AutoMapper.Mapper.Map(projectVM.ListResources, listResourcesModel);
                    projectModel.ProjectId = projectId;

                    for (int i = 0; i < GblprojectLead.Count(); i++)
                    {

                        ResourcesModel resourcemodel = new ResourcesModel();
                        resourcemodel.ProjectId = projectId;
                        resourcemodel.UserId = GblprojectLead[i].Decrypt();
                        resourcemodel.CreatedBy = (int)SessionManagement.LoggedInUser.UserId;
                        resourcemodel.CreatedDate = DateTime.UtcNow;
                        resourcemodel.ModifiedBy = (int)SessionManagement.LoggedInUser.UserId;
                        resourcemodel.ModifiedDate = DateTime.UtcNow;
                        resourcemodel.RecordDeleted = false;
                        listResourcesModel.Add(resourcemodel);

                    }
                    resourcesBusiness.SaveResources(listResourcesModel);
                }
                return RedirectToAction("ProjectList");
            }
            return View("AddEditProjectDetails", projectVM);

        }


        //[CustomAuthorize(Roles = Constants.MODULE_METRIC_DASHBOARD + Constants.PERMISSION_DELETE)]
        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_DELETE)]
        public ActionResult DeleteProject(ProjectVM projectVM)
        {
            Response response = new Response();
            int Userid = (int)SessionManagement.LoggedInUser.UserId;
            ProjectModel projectModel = new ProjectModel();

            AutoMapper.Mapper.Map(projectVM, projectModel);
            projectModel.RecordDeleted = true;
            projectModel.ModifiedDate = DateTime.UtcNow;
            projectModel.ModifiedBy = Userid;
            try
            {
                bool _IsDeleted = projectBusiness.DeleteProject(projectModel);
                response.Status = Enums.ResponseResult.Success.ToString();
                response.StatusCode = 200;
            }
            catch
            {
                response.Status = Enums.ResponseResult.Failure.ToString();
                response.StatusCode = 500;
            }

            return Json(response);

        }

        public JsonResult GetResponsible(string query)
        {
            List<EmployeeVM> employeeVM = new List<EmployeeVM>();
            int active = (int)Enums.UserStatus.Active;
            List<EmployeeModel> employeeModel = userBusiness.GetUsersListForAutocomplete(query, active);
            AutoMapper.Mapper.Map(employeeModel, employeeVM);
            //employeeVM.Add(new EmployeeVM() { FullName = "Json" })
            return Json(employeeModel.Select(x => new { FullName = x.FullName, UserId = x.UserId.Encrypt() }), JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetResponsibleUser(string query)
        {
            List<EmployeeVM> employeeVM = new List<EmployeeVM>();
            int active = (int)Enums.UserStatus.Active;
            List<EmployeeModel> employeeModel = userBusiness.GetUsersListForAutocomplete(query, active);
            AutoMapper.Mapper.Map(employeeModel, employeeVM);
            //employeeVM.Add(new EmployeeVM() { FullName = "Json" })
            return Json(
               new { ResponsibleUser = employeeModel.Select(x => new { FullName = x.FullName, UserId = x.UserId.Encrypt() }) }, JsonRequestBehavior.AllowGet);

        }

        [EncryptedActionParameter]
        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_VIEW)]
        public ActionResult ProjectDetail(string Id_Encrypted)
        {
            ProjectModel projectModel = new ProjectModel();
            ProjectVM projectVM = new ProjectVM();
            projectModel = projectBusiness.GetProject(Convert.ToInt32(Id_Encrypted));//projectModel.ProjectId);
            projectModel.TIMECONSUMED = projectBusiness.ProjectWorkHours(projectModel.ProjectId);
            AutoMapper.Mapper.Map(projectModel, projectVM);

            var jsonSerialiser = new JavaScriptSerializer();
            ViewBag.json = jsonSerialiser.Serialize(new { List = projectVM.Resources.Where(a => a.RecordDeleted == false).Select(a => new { UserId = a.UserId, FullName = a.FullName, ProjectId = a.ProjectId }) });
            return View(projectVM);
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_DELETE)]
        public ActionResult DeleteResource(ResourcesVM resourcesVM)
        {
            List<ResourcesModel> listResourcesModel = new List<ResourcesModel>();
            Response response = new Response();
            response.Status = Enums.ResponseResult.Success.ToString();
            response.StatusCode = 200;
            int LoggInUserId = SessionManagement.LoggedInUser.UserId;
            resourcesVM.ModifiedBy = LoggInUserId;
            int projectId = resourcesVM.ProjectId.Decrypt();
            int userId = resourcesVM.UserId.Decrypt();
            resourcesVM.ModifiedDate = DateTime.UtcNow;
            ResourcesModel resourcesModel = new ResourcesModel();
            AutoMapper.Mapper.Map(resourcesVM, resourcesModel);
            resourcesBusiness.DeleteResource(resourcesModel);
            return Json(response);
        }



        #endregion

        #region  Time Sheet

        #region  Time Sheet View
        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_VIEW)]
        public ActionResult TimeSheet()
        {
            #region ActionItem dropdwon
            int userId = (int)SessionManagement.LoggedInUser.UserId;


            #endregion

            #region Project dropdwon
            List<ProjectVM> listProjectVM = new List<ProjectVM>();
            List<ProjectModel> listProjectModel = new List<ProjectModel>();
            listProjectModel = projectBusiness.ProjectList(userId);
            AutoMapper.Mapper.Map(listProjectModel, listProjectVM);

            ViewBag.Project = listProjectVM;



            List<ActionItemVM> listActionItemVM = new List<ActionItemVM>();
            List<ActionItemModel> listActionItemModel = new List<ActionItemModel>();
            //  listActionItemModel = actionItemBusiness.ActionItemList();
            //  AutoMapper.Mapper.Map(listActionItemModel, listActionItemVM);
            ViewBag.ActionItem = listActionItemVM;



            #endregion

            #region Time Count
            var listHours = new List<SelectListItem>();
            var listDecimals = new List<SelectListItem>();
            double n = 0.5;
            for (double i = n; i <= 23; i++)
            {
                listHours.Add(new SelectListItem { Text = n + "", Value = i.ToString() });
                n = n + 0.5;
            };

            ViewBag.Time = listHours;

            #endregion


            return View();
        }
        #endregion


        #region  Time Sheet Edit
        //[CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_VIEW)]
        //public ActionResult EditTimeSheet(TimeSheetVM timeSheetVM)
        //{
        //    //#region ActionItem dropdwon
        //    //int userId = (int)SessionManagement.LoggedInUser.UserId;
        //    //#endregion

        //    //#region Project dropdwon
        //    //List<ProjectVM> listProjectVM = new List<ProjectVM>();
        //    //List<ProjectModel> listProjectModel = new List<ProjectModel>();
        //    //listProjectModel = projectBusiness.ProjectList(userId);
        //    //AutoMapper.Mapper.Map(listProjectModel, listProjectVM);

        //    //ViewBag.Project = listProjectVM;



        //    //List<ActionItemVM> listActionItemVM = new List<ActionItemVM>();
        //    //List<ActionItemModel> listActionItemModel = new List<ActionItemModel>();
        //    ////  listActionItemModel = actionItemBusiness.ActionItemList();
        //    ////  AutoMapper.Mapper.Map(listActionItemModel, listActionItemVM);
        //    //ViewBag.ActionItem = listActionItemVM;
        //    //#endregion

        //    //#region Time Count
        //    //var listHours = new List<SelectListItem>();
        //    //var listDecimals = new List<SelectListItem>();
        //    //double n = 0.5;
        //    //for (double i = n; i <= 23; i++)
        //    //{
        //    //    listHours.Add(new SelectListItem { Text = n + "", Value = i.ToString() });
        //    //    n = n + 0.5;
        //    //};

        //    //ViewBag.Time = listHours;

        //    //#endregion
        //     //List<TimeSheetVM> timesheet= new List<TimeSheetVM> ();
        //     //List<TimeSheetModel> timesheetModel= new List<TimeSheetModel> ();
        //     // timesheetModel = timeSheetBusiness.GetCurrentTimeSheetDetails(timeSheetVM.TimeSheetId.Decrypt());
        //     // if (timesheetModel.Count() > 0)
        //     // {
        //     //     List<ActionItemVM> listNewActionItemVM = new List<ActionItemVM>();
        //     //     List<ActionItemModel> listNewActionItemModel = new List<ActionItemModel>();
        //     //     listActionItemModel = actionItemBusiness.ActionItemList(timesheetModel[0].ProjectId, Convert.ToInt32(SessionManagement.LoggedInUser.UserId));
        //     //     AutoMapper.Mapper.Map(listNewActionItemModel, listNewActionItemVM);
        //     //     ViewBag.ActionItem = listNewActionItemVM;
        //     // }

        //    TimeSheetVM timesheetVM = new TimeSheetVM();
        //    TimeSheetModel timeSheetModel = new TimeSheetModel();
        //    int timesheet = 0;
        //    timesheet = timeSheetVM.TimeSheetId.Decrypt();
        //    timeSheetModel = timeSheetBusiness.GetCurrentTimeSheetbyId(timesheet);
        //    AutoMapper.Mapper.Map(timeSheetModel, timesheetVM);


        //    return View("TimeSheet",timesheetVM);
        //}
        #endregion

        #region  Time Sheet Save
        [HttpPost]
        // [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_CREATE)]
        public ActionResult TimeSheet(TimeSheetVM timeSheetVM)
        {

            if (ModelState.IsValid)
            {
                Response response = new Response();
                int userId = (int)SessionManagement.LoggedInUser.UserId;
                TimeSheetModel timeSheetModel = new TimeSheetModel();
                AutoMapper.Mapper.Map(timeSheetVM, timeSheetModel);
                int timeSheetId = 0;
                timeSheetModel.CreatedBy = userId;
                timeSheetModel.UserId = userId;
                timeSheetModel.CreatedDate = DateTime.UtcNow;
                timeSheetModel.DeletedRecord = false;
                timeSheetModel.TimeTaken = timeSheetVM.TimeTaken;

                if (timeSheetVM.TimeSheetId != null && timeSheetVM.TimeSheetId != "")
                {
                    timeSheetId = timeSheetBusiness.UpdateTimeSheetRecord(timeSheetModel);
                    return RedirectToAction("TimeSheet");
                }
                else
                {
                    timeSheetId = timeSheetBusiness.SaveTimeSheetRecord(timeSheetModel);
                    //if (timeSheetId == 1)
                    //{
                    //    response.Status = Enums.ResponseResult.Success.ToString();
                    //    response.StatusCode = 200;
                    //    ModelState.AddModelError("ActionItemId", "Action item hour's assign today to you.");
                    //    return View("TimeSheet",timeSheetVM);
                    //}

                    return RedirectToAction("TimeSheet");
                }
            }
            else { return View("TimeSheet"); }


        }
        #endregion

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_DELETE)]
        public ActionResult DeleteTimeSheet(TimeSheetVM timeSheetVM)
        {
            int userId = (int)SessionManagement.LoggedInUser.UserId;
            TimeSheetModel timeSheetModel = new TimeSheetModel();
            AutoMapper.Mapper.Map(timeSheetVM, timeSheetModel);
            int timeSheetId = 0;
            timeSheetModel.CreatedBy = userId;
            timeSheetModel.UserId = userId;
            timeSheetModel.DeletedRecord = true;

            if (timeSheetVM.TimeSheetId != null && timeSheetVM.TimeSheetId != "")
            {
                timeSheetId = timeSheetBusiness.DeleteCurrentTimeSheetbyId(timeSheetModel.TimeSheetId, userId);
                if (timeSheetId == 1)
                {

                }
                return RedirectToAction("TimeSheet");
            }

            return View("TimeSheet");
        }


        #region   GetTimeSheet

        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_VIEW)]
        public ActionResult GetTimeSheet(TimeSheetVM timeSheetVM)
        {
            List<TimeSheetVM> listtimeSheetVM = new List<TimeSheetVM>();
            List<TimeSheetModel> listtimeSheetModel = new List<TimeSheetModel>();
            string LogedInUserId = ((int)SessionManagement.LoggedInUser.UserId).Encrypt();
            int LoginUser = LogedInUserId.Decrypt();

            List<DateTimeSheetVM> ObjtimeSheetVM = new List<DateTimeSheetVM>();
            List<DateTimeSheetModel> ObjtimeSheetModel = new List<DateTimeSheetModel>();

            listtimeSheetModel = timeSheetBusiness.GetTimeSheetList(LoginUser);

            if (listtimeSheetModel.Count() > 0)
            {
                ObjtimeSheetModel = timeSheetBusiness.GetDateTimeSheetList(listtimeSheetModel[0].Months, Convert.ToInt32(listtimeSheetModel[0].Years), LoginUser);
            }

            AutoMapper.Mapper.Map(listtimeSheetModel, listtimeSheetVM);
            AutoMapper.Mapper.Map(ObjtimeSheetModel, ObjtimeSheetVM);
            return Json(new
            {
                MonthList = listtimeSheetVM,
                MonthDateList = ObjtimeSheetVM,
                LoggedInUser = LogedInUserId
            }
          , JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_VIEW)]
        public ActionResult GetTimeDetails(TimeSheetVM timeSheetVM)
        {
            string LogedInUserId = ((int)SessionManagement.LoggedInUser.UserId).Encrypt();
            int LoginUser = LogedInUserId.Decrypt();

            List<DateTimeSheetVM> ObjtimeSheetVM = new List<DateTimeSheetVM>();
            List<DateTimeSheetModel> ObjtimeSheetModel = new List<DateTimeSheetModel>();
            ObjtimeSheetModel = timeSheetBusiness.GetDateTimeSheetList(timeSheetVM.Months, Convert.ToInt32(timeSheetVM.Years), LoginUser);
            AutoMapper.Mapper.Map(ObjtimeSheetModel, ObjtimeSheetVM);
            return Json(new
            {
                MonthDateList = ObjtimeSheetVM,
                LoggedInUser = LogedInUserId
            }
          , JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_VIEW)]
        public ActionResult GetDateActionItemDetails(TimeSheetVM timeSheetVM)
        {
            string LogedInUserId = ((int)SessionManagement.LoggedInUser.UserId).Encrypt();
            int LoginUser = LogedInUserId.Decrypt();

            List<DailyActionItemVM> objActionItemVM = new List<DailyActionItemVM>();
            List<DailyActionItemsModel> objActionItemModel = new List<DailyActionItemsModel>();
            objActionItemModel = timeSheetBusiness.GetDailyActionItemList(timeSheetVM.Date, timeSheetVM.Months, Convert.ToInt32(timeSheetVM.Years), LoginUser);
            AutoMapper.Mapper.Map(objActionItemModel, objActionItemVM);
            return Json(new
            {
                ActionItemList = objActionItemVM,
                LoggedInUser = LogedInUserId
            }
          , JsonRequestBehavior.AllowGet);


        }



        #endregion
        #endregion
        #region GetActionItemListforDropdown
        public JsonResult GetActionItemListforDropdown(string timeSheet)
        {
            int projectId = timeSheet.Decrypt();
            List<ActionItemVM> listActionItemVM = new List<ActionItemVM>();
            List<ActionItemModel> listActionItemModel = new List<ActionItemModel>();
            listActionItemModel = actionItemBusiness.ActionItemList(projectId, Convert.ToInt32(SessionManagement.LoggedInUser.UserId));
            AutoMapper.Mapper.Map(listActionItemModel, listActionItemVM);
            ViewBag.ActionItem = listActionItemVM;
            return Json(new
            {
                ActionItemListForProject = listActionItemVM,
            }
         , JsonRequestBehavior.AllowGet);
        }

        #region Action Item Details
        [EncryptedActionParameter]
        public ActionResult ActionItemDetail(string id_Encrypted)
        {
            ActionItemVM actionItemVM = new ActionItemVM();
            ActionItemModel actionItemModel = new ActionItemModel();
            actionItemModel.ActionItemId = Convert.ToInt32(id_Encrypted);

            actionItemModel = actionItemBusiness.GetActionItemDetail(actionItemModel);
            AutoMapper.Mapper.Map(actionItemModel, actionItemVM);
            return View(actionItemVM);

        }
        #endregion
        #endregion
        #region GatCategoryList
        [CustomAuthorize(Roles = Constants.MODULE_CATEGORY + Constants.PERMISSION_VIEW)]
        public ActionResult GatCategoryList(ListingParameters listingParameters)
        {
            List<CategoryVM> listCategoryVM = new List<CategoryVM>();
            List<CategoryModel> listCategoryModel = new List<CategoryModel>();
            AutoMapper.Mapper.Map(listCategoryVM, listCategoryModel);
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            listingParameters.CompanyId = companyId;
            int totalRecords = 0;
            listCategoryModel = categoryBusiness.GetAllCategories(listingParameters, ref totalRecords);
            AutoMapper.Mapper.Map(listCategoryModel, listCategoryVM);
            return Json(new
            {
                TotalRecords = totalRecords,
                DataList = listCategoryVM,

            }
            , JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Bind Hous Actionitem
        public ActionResult HoursList()
        {

            var listHours = new List<SelectListItem>();
            var listDecimals = new List<SelectListItem>();
            double n = 0.5;
            for (double i = n; i <= 31; i++)
            {
                listHours.Add(new SelectListItem { Text = n + "", Value = i.ToString() });
                n = n + 0.5;
            };

            return Json(new { list = listHours }
           , JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region For Apply Leaves


        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_VIEW)]
        public ActionResult Leave()
        {
            return View();
        }
        public ActionResult ApplyLeave()
        {
            return View();
        }

        public ActionResult GetAllLeaveType()
        {
            List<LeaveTypeModel> leavelist = new List<LeaveTypeModel>();
            List<LeaveTypeVM> leavelistVM = new List<LeaveTypeVM>();
            leavelist = LeavesItemBusiness.GetAllLeaveId();
            AutoMapper.Mapper.Map(leavelist, leavelistVM);
            return Json(leavelistVM, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveLeaveItemRecord(LeavesItemVM LeavesItemVM)
        {

            int userId = (int)SessionManagement.LoggedInUser.UserId;
            LeavesItemVM.UserId = userId;
            LeavesItemModel LeavesItemModel = new LeavesItemModel();
            AutoMapper.Mapper.Map(LeavesItemVM, LeavesItemModel);

            var LeaveItemId = LeavesItemBusiness.SaveLeaveItemRecord(LeavesItemModel);


            return Json(new
            {
                LeaveItemId = LeaveItemId
                //ImagePath = Constants.ACTION_ITEM_DOCUMENTS_PATH
            }
          , JsonRequestBehavior.AllowGet);

            //ListingParameters listingParameters

        }
        [HttpGet]
        public ActionResult GetAllLeavesDetail(ListingParameters listingParameters)
        {
            List<LeavesItemVM> leaveVMList = new List<LeavesItemVM>();
            List<LeavesItemModel> leaveModelList = new List<LeavesItemModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            string LogedInUserId = ((int)SessionManagement.LoggedInUser.UserId).Encrypt();
            leaveModelList = LeavesItemBusiness.GetAllLeavesDetail(listingParameters);
            AutoMapper.Mapper.Map(leaveModelList, leaveVMList);
            return Json(new
            {
                leaveVMList = leaveVMList,
                LoggedInUser = LogedInUserId
            }
          , JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult ApproveLeave(ListingParameters listingParameters)
        {
            LeavesItemModel leaveModel = new LeavesItemModel();
            leaveModel.ApprovedStatus = true;
            leaveModel.ApprovedById = SessionManagement.LoggedInUser.UserId;
            leaveModel.LeaveId = listingParameters.LeaveId;
             LeavesItemBusiness.ApproveLeaveItemRecord(leaveModel);
            return Json(true,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DisApproveLeave(ListingParameters listingParameters)
        {
            LeavesItemModel leaveModel = new LeavesItemModel();
            leaveModel.ApprovedStatus = false;
            leaveModel.ApprovedById = SessionManagement.LoggedInUser.UserId;
            leaveModel.LeaveId = listingParameters.LeaveId;
            LeavesItemBusiness.ApproveLeaveItemRecord(leaveModel);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Category
        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_CATEGORY + Constants.PERMISSION_EDIT)]
        public ActionResult UpdateCategory(CategoryVM categoryVM)
        {
            Response response = new Response();
            response.StatusCode = 500;
            response.Status = Enums.ResponseResult.Failure.ToString();
            bool _isSaved = false;
            CategoryModel categoryModel = new CategoryModel();
            AutoMapper.Mapper.Map(categoryVM, categoryModel);
            categoryModel.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
            categoryModel.CreatedBy = (int)SessionManagement.LoggedInUser.UserId;
            categoryModel.CreatedDate = DateTime.UtcNow;
            categoryModel.ModifiedBy = (int)SessionManagement.LoggedInUser.UserId;
            categoryModel.ModifiedDate = DateTime.UtcNow;
            if (categoryVM.CategoryId != null)
            {
                string SavedStatus = categoryBusiness.UpdateCategory(categoryModel);
                if (SavedStatus == "AlreadyExist")
                {
                    response.StatusCode = 200;
                    response.Status = Enums.ResponseResult.Success.ToString();
                    response.Message = "AlreadyExist";
                }
                else if (SavedStatus == "Success")
                {
                    response.Message = "UpdateSucessfully";
                    response.StatusCode = 200;
                    response.Status = Enums.ResponseResult.Success.ToString();
                }
                else
                {
                    response.Message = "Error";
                }
            }
            else
            {
                _isSaved = categoryBusiness.AddCategory(categoryModel);
                if (_isSaved)
                {
                    response.Message = "CreateSucessfully";
                    response.Status = Enums.ResponseResult.Success.ToString();
                    response.StatusCode = 200;
                }
                else
                {
                    response.StatusCode = 200;
                    response.Status = Enums.ResponseResult.Success.ToString();
                    response.Message = "AlreadyExist";
                }
            }
            return Json(response);
        }


        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_CATEGORY + Constants.PERMISSION_DELETE)]
        public ActionResult DeleteCategory(CategoryVM categoryVM)
        {
            Response response = new Response();
            response.StatusCode = 500;
            response.Status = Enums.ResponseResult.Failure.ToString();
            bool _isDeleted = false;
            CategoryModel categoryModel = new CategoryModel();

            AutoMapper.Mapper.Map(categoryVM, categoryModel);
            categoryModel.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;

            _isDeleted = categoryBusiness.DeleteCategory(categoryModel.CategoryId, categoryModel.CompanyId);
            if (_isDeleted)
            {
                response.Status = Enums.ResponseResult.Success.ToString();
                response.StatusCode = 200;
            }
            return Json(response);
        }


        #endregion

        public ActionResult TestPage()
        {
            return View();
        }

        public JsonResult TimeSheetProjectList()
        {
            List<ProjectVM> listProjectVM = new List<ProjectVM>();
            List<ProjectModel> listProjectModel = new List<ProjectModel>();
            int userId = (int)SessionManagement.LoggedInUser.UserId;
            listProjectModel = projectBusiness.ProjectList(userId);
            AutoMapper.Mapper.Map(listProjectModel, listProjectVM);
            //
            ViewBag.Project = listProjectVM;
            return Json(new { projectList = listProjectVM, JsonRequestBehavior.AllowGet });
        }



        #region  Time Sheet View
        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_VIEW)]
        public ActionResult UserTimeSheet()
        {
            #region ActionItem dropdwon
            int userId = (int)SessionManagement.LoggedInUser.UserId;
            #endregion

            #region Project dropdwon
            List<ProjectVM> listProjectVM = new List<ProjectVM>();
            List<ProjectModel> listProjectModel = new List<ProjectModel>();
            listProjectModel = projectBusiness.ProjectList(userId);
            AutoMapper.Mapper.Map(listProjectModel, listProjectVM);

            ViewBag.Project = listProjectVM;

            List<ActionItemVM> listActionItemVM = new List<ActionItemVM>();
            List<ActionItemModel> listActionItemModel = new List<ActionItemModel>();
            ViewBag.ActionItem = listActionItemVM;



            #endregion

            #region Time Count
            var listHours = new List<SelectListItem>();
            var listDecimals = new List<SelectListItem>();
            double n = 0.5;
            for (double i = n; i <= 23; i++)
            {
                listHours.Add(new SelectListItem { Text = n + "", Value = i.ToString() });
                n = n + 0.5;
            };

            ViewBag.Time = listHours;
            #endregion
            return View();
        }
        #endregion

        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_VIEW)]
        public ActionResult GetUsersDetailsForTimeSheetView(TimeSheetVM timeSheetVM)
        {
            List<TimeSheetVM> listtimeSheetVM = new List<TimeSheetVM>();
            List<TimeSheetModel> listtimeSheetModel = new List<TimeSheetModel>();
            List<TimeSheetModel> listtimeSheetModelAll = new List<TimeSheetModel>();
            string LogedInUserId = ((int)SessionManagement.LoggedInUser.UserId).Encrypt();
            int LoginUser = LogedInUserId.Decrypt();

            //////////////////For User List////////////////////////
            List<EmployeeVM> listEmployeeVM = new List<EmployeeVM>();
            List<EmployeeModel> listEmployeeModel = new List<EmployeeModel>();

            listEmployeeModel = timeSheetBusiness.GetEmployeeWorkHours(LoginUser);

            ////////////////////////////////////////////////////////////
            List<DateTimeSheetVM> ObjtimeSheetVM = new List<DateTimeSheetVM>();
            List<DateTimeSheetModel> ObjtimeSheetModel = new List<DateTimeSheetModel>();
            List<DateTimeSheetModel> ObjtimeSheetModelAll = new List<DateTimeSheetModel>();
            if (listEmployeeModel.Count() > 0)
            {
                for (int i = 0; i < listEmployeeModel.Count; i++)
                {
                    listtimeSheetModel = timeSheetBusiness.GetTimeSheetList(listEmployeeModel[i].UserId);
                    if (listtimeSheetModel.Count() > 0)
                    {
                        ObjtimeSheetModel = timeSheetBusiness.GetDateTimeSheetList(listtimeSheetModel[0].Months, Convert.ToInt32(listtimeSheetModel[0].Years), listEmployeeModel[i].UserId);
                    }
                    listtimeSheetModelAll.AddRange(listtimeSheetModel);
                    ObjtimeSheetModelAll.AddRange(ObjtimeSheetModel);
                }
            }
            AutoMapper.Mapper.Map(listEmployeeModel, listEmployeeVM);

            AutoMapper.Mapper.Map(listtimeSheetModelAll, listtimeSheetVM);
            AutoMapper.Mapper.Map(ObjtimeSheetModelAll, ObjtimeSheetVM);


            return Json(new
            {
                EmployeeList = listEmployeeVM,
                MonthList = listtimeSheetVM,
                MonthDateList = ObjtimeSheetVM,

                LoggedInUser = LogedInUserId
            }
          , JsonRequestBehavior.AllowGet);
        }


        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_VIEW)]
        public JsonResult GetUsersTimeSheet(TimeSheetVM timeSheetVM)
        {
            List<TimeSheetVM> listtimeSheetVM = new List<TimeSheetVM>();
            List<TimeSheetModel> listtimeSheetModel = new List<TimeSheetModel>();
            string LogedInUserId = timeSheetVM.UserId;//==null?((int)SessionManagement.LoggedInUser.UserId).Encrypt():timeSheetVM.UserId;
            int LoginUser = LogedInUserId.Decrypt();



            ////////////////////////////////////////////////////////////
            List<DateTimeSheetVM> ObjtimeSheetVM = new List<DateTimeSheetVM>();
            List<DateTimeSheetModel> ObjtimeSheetModel = new List<DateTimeSheetModel>();



            listtimeSheetModel = timeSheetBusiness.GetTimeSheetList(LoginUser);
            if (listtimeSheetModel.Count() > 0)
            {
                ObjtimeSheetModel = timeSheetBusiness.GetDateTimeSheetList(listtimeSheetModel[0].Months, Convert.ToInt32(listtimeSheetModel[0].Years), LoginUser);
            }

            AutoMapper.Mapper.Map(listtimeSheetModel, listtimeSheetVM);
            AutoMapper.Mapper.Map(ObjtimeSheetModel, ObjtimeSheetVM);

            return Json(new
            {
                MonthList = listtimeSheetVM,
                MonthDateList = ObjtimeSheetVM,
                LoggedInUser = LogedInUserId
            }
          , JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_VIEW)]
        public ActionResult GetUsersTimeDetails(TimeSheetVM timeSheetVM)
        {
            string LogedInUserId = timeSheetVM.UserId;// == null ? ((int)SessionManagement.LoggedInUser.UserId).Encrypt() : timeSheetVM.UserId;
            int LoginUser = LogedInUserId.Decrypt();

            List<DateTimeSheetVM> ObjtimeSheetVM = new List<DateTimeSheetVM>();
            List<DateTimeSheetModel> ObjtimeSheetModel = new List<DateTimeSheetModel>();
            ObjtimeSheetModel = timeSheetBusiness.GetDateTimeSheetList(timeSheetVM.Months, Convert.ToInt32(timeSheetVM.Years), LoginUser);
            AutoMapper.Mapper.Map(ObjtimeSheetModel, ObjtimeSheetVM);
            return Json(new
            {
                MonthDateList = ObjtimeSheetVM,
                LoggedInUser = LogedInUserId
            }
          , JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = Constants.MODULE_Projects + Constants.PERMISSION_VIEW)]
        public ActionResult GetUserDateActionItemDetails(TimeSheetVM timeSheetVM)
        {
            string LogedInUserId = timeSheetVM.UserId;// == null ? ((int)SessionManagement.LoggedInUser.UserId).Encrypt() : timeSheetVM.UserId;
            int LoginUser = LogedInUserId.Decrypt();

            List<DailyActionItemVM> objActionItemVM = new List<DailyActionItemVM>();
            List<DailyActionItemsModel> objActionItemModel = new List<DailyActionItemsModel>();
            objActionItemModel = timeSheetBusiness.GetDailyActionItemList(timeSheetVM.Date, timeSheetVM.Months, Convert.ToInt32(timeSheetVM.Years), LoginUser);
            AutoMapper.Mapper.Map(objActionItemModel, objActionItemVM);
            return Json(new
            {
                ActionItemList = objActionItemVM,
                LoggedInUser = LogedInUserId
            }
          , JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillReview()
        {
            return View(new FillReviewVM());
        }
    }
}

