using Recaptcha.Web;
using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;
using EMPMGMT.Utility;
using EMPMGMT.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Recaptcha.Web.Mvc;
namespace EMPMGMT.Web.Controllers
{
    public class SiteController : Controller
    {
        private IUserBusiness userBusiness;
        public static string uid = "";
        public SiteController(IUserBusiness _userBusiness)
        {
            userBusiness = _userBusiness;
        }
        public ActionResult Index()
        {
            HttpCookie rememberMe = Request.Cookies["RememberMe"];
            LoginVM loginVM = new LoginVM();
            if (rememberMe != null && !string.IsNullOrEmpty(rememberMe.Values["emailId"]) && !string.IsNullOrEmpty(rememberMe.Values["password"]))
            {
                loginVM.EmailId = rememberMe.Values["emailId"];
                loginVM.Password = rememberMe.Values["password"];
                loginVM.isChecked = true;
                loginVM.usertype = Convert.ToString((int)Enums.UserType.User);
                int userTypeId = ValidateUser(loginVM);

                if (userTypeId == 2)
                {
                    return RedirectToAction("Home", "Employee");
                }
            }
            return View();
        }


        public ActionResult AdminLogin()
        {



            LoginVM loginModel = new LoginVM();
            loginModel.usertype = "1";
            ViewBag.success = false;
            if (Request.Cookies["Permissions"] != null)
            {
                HttpCookie myCookie = new HttpCookie("Permissions");
                HttpCookie myCookie1 = new HttpCookie("type");
                myCookie1.Expires = DateTime.Now.AddDays(-1d);
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);

            }
            return View(loginModel);
        }
        [HttpPost]
        public ActionResult AdminLogin(LoginVM loginVM)
        {


            if (ModelState.IsValid)
            {
                //UserModel usermodel = userBusiness.ValidateUser(loginVM.EmailId, loginVM.Password, loginVM.isChecked);

                int userTypeId = ValidateUser(loginVM);
                if (userTypeId == -1)
                {
                    loginVM = new LoginVM();

                    ViewBag.LoginError = "Invalid Username or Password";
                    return View(loginVM);

                }
                else if (userTypeId == (int)Enums.UserType.Admin)
                {

                    return RedirectToAction("RegisteredUsers", "Admin");

                }
                else
                {
                    loginVM = new LoginVM();
                    ViewBag.LoginError = "Invalid Username or Password";
                    return View(loginVM);
                }


            }
            else
            {
                loginVM = new LoginVM();
                string sitePageName = Constants.PUBLIC_PAGE_LOGIN;
                return View(loginVM);
            }

        }
        public ActionResult Login()
        {
            LoginVM loginModel = new LoginVM();
            loginModel.usertype = "2";
            ViewBag.success = false;
            SessionManagement.LoggedInUser = null;
            Session.Abandon();
            Session.RemoveAll();
            CommonFunctions.RemoveCookies();
            if (Request.Cookies["Permissions"] == null)
            {
                HttpCookie myCookie = new HttpCookie("Permissions");
                HttpCookie myCookie1 = new HttpCookie("type");
                myCookie1.Expires = DateTime.Now.AddDays(-1d);
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
            return View(loginModel);
        }
        [HttpPost]
        public ActionResult Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                //UserModel usermodel = userBusiness.ValidateUser(loginVM.EmailId, loginVM.Password, loginVM.isChecked);

                EmployeeModel userModel = ValidateUserLogin(loginVM);
                if (userModel.LoginStatus == Utility.Enums.LoginStatus.ExpiredLogin.ToString())
                {
                    loginVM = new LoginVM();
                    //string sitePageName = Constants.PUBLIC_PAGE_LOGIN;
                    ViewBag.LoginError = "Expired Account";
                    return View(loginVM);
                }
                if (userModel.LoginStatus == Utility.Enums.LoginStatus.InvalidLogin.ToString())
                {
                    loginVM = new LoginVM();
                    //string sitePageName = Constants.PUBLIC_PAGE_LOGIN;
                    ViewBag.LoginError = "Invalid Username or Password";
                    return View(loginVM);
                }

                else if (userModel.UserTypeId == (int)Enums.UserType.Admin)
                {
                    loginVM = new LoginVM();
                    //string sitePageName = Constants.PUBLIC_PAGE_LOGIN;
                    ViewBag.LoginError = "Invalid Username or Password";
                    return View(loginVM);

                }
                else
                {
                    return RedirectToAction("Home", "Employee");
                }


            }
            else
            {
                loginVM = new LoginVM();
                //string sitePageName = Constants.PUBLIC_PAGE_LOGIN;
                return View(loginVM);
            }

        }
        //var userSubDomain = CommonFunctions.GetSubDomain();
        //       HttpCookie cookie = new HttpCookie("UserSubDomain", userSubDomain);
        //       cookie.Expires = DateTime.Now.AddDays(1);
        //       HttpCookie encodedCookie = HttpSecureCookie.Encode(cookie);
        //       Response.Cookies.Add(encodedCookie);

        public ActionResult Register()
        {
            RegistrationVM registrationVM = new RegistrationVM();
            ViewBag.success = false;
            return View(registrationVM);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationVM registration)
        {
            if (ModelState.IsValid)
            {
                Boolean IsModelErrorExist = false;
                if (ModelState["Profile.ProfileName"] != null)
                    ModelState["Profile.ProfileName"].Errors.Clear();

                if (ModelState["Role.RoleName"] != null)
                    ModelState["Role.RoleName"].Errors.Clear();


                EmployeeModel userModel;
                if (IsModelErrorExist == false)
                {
                    userModel = userBusiness.GetUserByEmailId(registration.EmailId);
                    if (userModel != null)
                    {
                        ModelState.AddModelError("", "Email Already Exists");
                        IsModelErrorExist = true;
                    }
                }

                if (IsModelErrorExist == true)
                {
                    ViewBag.success = false;
                    ViewBag.StatusMessage = "Email Already Exists";
                    TempData["View"] = "Register";
                    return View(registration);
                }
                userModel = new EmployeeModel();

                AutoMapper.Mapper.Map(registration, userModel);
                userModel = userBusiness.RegisterUser(userModel);
                userBusiness.AddDefaultOrganisationUnit(userModel.CompanyModel.CompanyName, userModel.CompanyId, (int)Enums.UserType.Admin);
                MailHelper mailHelper = new MailHelper();
                MailBodyTemplate mailBodyTemplate = new MailBodyTemplate();
                string mailTemplateFolder = ReadConfiguration.MailTemplateFolder;
                string mailBody = string.Empty;
                mailHelper.Subject = Constants.RegistrationMailSubject;
                mailHelper.ToEmail = userModel.EmailId;
                mailBody = CommonFunctions.ReadFile(Server.MapPath(mailTemplateFolder + Constants.RegistrationMailFileName));
                mailBodyTemplate.RegistrationUserName = userModel.FullName;
                mailBodyTemplate.MailBody = mailBody;
                mailBody = CommonFunctions.ConfigureRegistrationMailBody(mailBodyTemplate);
                mailHelper.Body = mailBody;
                mailHelper.SendEmail();
                ViewBag.success = true;
                ViewBag.RegistrationSuccessWelComeMessage = "Thank you for registering with us! Your request has been sent to Admin. We'll contact you very soon. A confirmation about your request is sent on your registered email.";
                //userBusiness.SendRegistrationEmail(userModel);               
                ModelState.Clear();
                //return RedirectToAction("Index");
            }
            TempData["View"] = "Register";

            return View(registration);




        }
        private EmployeeModel ValidateUserLogin(LoginVM loginVM)
        {
            EmployeeModel usermodel = userBusiness.ValidateUser(loginVM.EmailId, loginVM.Password, loginVM.isChecked);
            if (usermodel.LoginStatus == Utility.Enums.LoginStatus.ValidLogin.ToString())
            {
                SessionManagement.LoggedInUser.UserId = usermodel.UserId;
                SessionManagement.LoggedInUser.UserIdEncrypted = usermodel.UserId.Encrypt();
                SessionManagement.LoggedInUser.UserName = (usermodel.FirstName + " " + usermodel.LastName);
                SessionManagement.LoggedInUser.Password = usermodel.Password;
                SessionManagement.LoggedInUser.EmailId = usermodel.EmailId;
                //SessionManagement.LoggedInUser.Offset = usermodel.TimeZoneModel.offset;
                SessionManagement.LoggedInUser.FullName = usermodel.FirstName + " " + usermodel.LastName;
                SessionManagement.LoggedInUser.Role = (Enums.UserType)usermodel.UserTypeId;
                SessionManagement.LoggedInUser.CompanyId = usermodel.CompanyModel.CompanyId;
                SessionManagement.LoggedInUser.CompanyName = usermodel.CompanyModel.CompanyName;
                SessionManagement.LoggedInUser.ProfileImageUrl = Constants.PROFILE_IMAGE_PATH + (usermodel.ImageURL != null ? usermodel.ImageURL : "no_image.gif");
                Response.Cookies["username"].Value = (usermodel.FirstName + " " + usermodel.LastName);
                Response.Cookies["EmailId"].Value = usermodel.EmailId;

                // SessionManagement.LoggedInUser.CurrentCulture = usermodel.CultureInformationModel.CultureName;
                // SessionManagement.LoggedInUser.TimeZoneOffSet = usermodel.TimeZoneModel.offset;
                string pipeSeperatedPermissions = String.Join("|", usermodel.ProfileModel.ProfilePermissionModels.Where(x => x.HasAccess == true).Select(x => x.ModulePermission.Module.ModuleCONSTANT + x.ModulePermission.Permission.PermissionCONSTANT).ToList().ToArray());
                pipeSeperatedPermissions += "|" + CommonFunctions.GetSubDomain();
                CommonFunctions.SetCookie(SessionManagement.LoggedInUser, pipeSeperatedPermissions, loginVM.isChecked);
            }
            return usermodel;
        }
        #region ForgotPassword view
        private int ValidateUser(LoginVM loginVM)
        {

            EmployeeModel usermodel = userBusiness.ValidateUser(loginVM.EmailId, loginVM.Password, loginVM.isChecked);
            //if (usermodel.UserId != 0)
            if (usermodel.UserTypeId == Convert.ToInt32(Utility.Enums.UserType.Admin))
            {
                SessionManagement.LoggedInUser.UserId = usermodel.UserId;
                SessionManagement.LoggedInUser.UserIdEncrypted = usermodel.UserId.Encrypt();
                SessionManagement.LoggedInUser.UserName = (usermodel.FirstName + " " + usermodel.LastName);
                SessionManagement.LoggedInUser.Password = usermodel.Password;
                SessionManagement.LoggedInUser.EmailId = usermodel.EmailId;
                //SessionManagement.LoggedInUser.Offset = usermodel.TimeZoneModel.offset;
                SessionManagement.LoggedInUser.FullName = usermodel.FirstName + " " + usermodel.LastName;
                SessionManagement.LoggedInUser.Role = (Enums.UserType)usermodel.UserTypeId;
                SessionManagement.LoggedInUser.CompanyId = usermodel.CompanyModel.CompanyId;
                SessionManagement.LoggedInUser.CompanyName = usermodel.CompanyModel.CompanyName;
                SessionManagement.LoggedInUser.ProfileImageUrl = Constants.PROFILE_IMAGE_PATH + (usermodel.ImageURL != null ? usermodel.ImageURL : "no_image.gif");
                Response.Cookies["username"].Value = (usermodel.FirstName + " " + usermodel.LastName);
                Response.Cookies["EmailId"].Value = usermodel.EmailId;

                // SessionManagement.LoggedInUser.CurrentCulture = usermodel.CultureInformationModel.CultureName;
                // SessionManagement.LoggedInUser.TimeZoneOffSet = usermodel.TimeZoneModel.offset;
                string pipeSeperatedPermissions = String.Join("|", usermodel.ProfileModel.ProfilePermissionModels.Where(x => x.HasAccess == true).Select(x => x.ModulePermission.Module.ModuleCONSTANT + x.ModulePermission.Permission.PermissionCONSTANT).ToList().ToArray());
                pipeSeperatedPermissions += "|" + CommonFunctions.GetSubDomain();
                CommonFunctions.SetCookie(SessionManagement.LoggedInUser, pipeSeperatedPermissions, loginVM.isChecked);
                return usermodel.UserTypeId;
            }
            else return -1;

        }
        #endregion
        #region ForgotPassword view
        public ActionResult ForgotPassword(string pagename)
        {


            string sitePageName = Constants.PUBLIC_PAGE_FORGOTPASSWORD;


            if (pagename != null)
            {
                sitePageName = pagename;
            }



            ForgotPasswordVM forgotPasswordVM = new ViewModels.ForgotPasswordVM();

            return View(forgotPasswordVM);
        }
        #endregion
        #region ForgotPassword
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordVM forgotPassword)
        {
            Response response = new Response();
            response.Status = "Failure";
            response.StatusCode = 500;
            if (ModelState.IsValid)
            {
                EmployeeModel userModel;
                bool isMailSent = true;
                userModel = userBusiness.GetUserByEmailId(forgotPassword.EmailId);
                if (userModel != null)
                {
                    if (userModel.Status == (int)Enums.UserStatus.Active)
                    {

                        // isMailSent = userBusiness.SendPasswordRecoveryMail(userModel);
                        MailHelper mailHelper = new MailHelper();
                        MailBodyTemplate mailBodyTemplate = new MailBodyTemplate();
                        string mailTemplateFolder = ReadConfiguration.MailTemplateFolder;
                        string mailBody = string.Empty;
                        mailHelper.Subject = Constants.PasswordRecoveryMailSubject;
                        mailHelper.ToEmail = userModel.EmailId;
                        mailBody = CommonFunctions.ReadFile(Server.MapPath(mailTemplateFolder + Constants.PasswordRecoveryMailFileName));
                        mailBodyTemplate.RegistrationUserName = userModel.FullName;
                        mailBodyTemplate.MailBody = mailBody;
                        mailBodyTemplate.AccountLoginUserId = userModel.EmailId;
                        mailBodyTemplate.AccountLoginPassowrd = userModel.Password;
                        mailBodyTemplate.AccountLoginUrl = ReadConfiguration.WebsiteLogoPath;
                        mailBody = CommonFunctions.ConfigurePasswordRecoveryMailBody(mailBodyTemplate);
                        mailHelper.Body = mailBody;
                        isMailSent = mailHelper.SendEmail();
                        if (isMailSent)
                        {

                            response.Message = "Password for your account is sent. ";
                            response.Status = "Success";

                        }
                        else
                        {
                            response.Message = "Error while sending mail. Please try again.";
                        }
                    }
                    else
                    {
                        response.Message = "Your account is deactivated. Please contact your administrator";
                    }

                }
                else
                {
                    response.Message = "No user exists with the given Email Address.";
                }
            }
            else
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                        response.Message += error.ErrorMessage;
                }

            }
            return Json(response);
        }

        #endregion


        public ActionResult Contactus()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Contactus(ForgotPasswordVM forgotPassword)
        {
            return View();
        }


        public ActionResult Features()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Features(ForgotPasswordVM forgotPassword)
        {
            return View();
        }

        public ActionResult ResetPassword()
        {
            uid = "";

            if (Request.QueryString["uid"] is object)
            {
                SessionManagement.LoggedInUser = null;
                Session.Abandon();
                Session.RemoveAll();
                CommonFunctions.RemoveCookies();
                uid = Request.QueryString["uid"].ToString();
                string userid = uid;
                bool chk = (bool)userBusiness.Checkinvite_Validuser_Guid(userid);
                if (chk == true)
                {

                }
                else
                {
                    return Redirect("Login");
                }

            }
            else
            {

                return Redirect("Login");

            }

            ResetPasswordVM forgotPassword = new ResetPasswordVM();
            forgotPassword.UserId = uid;
            return View(forgotPassword);

        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordVM forgotPassword)
        {
            EmployeeModel um = new EmployeeModel();
            string userid = forgotPassword.UserId.ToString();
            bool result = userBusiness.UpdateUser_ResetPassword_Guid(userid, forgotPassword.password);
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

    }


}
