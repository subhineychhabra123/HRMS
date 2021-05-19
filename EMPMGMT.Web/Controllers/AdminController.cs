using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;
using EMPMGMT.Utility;
using EMPMGMT.Web.Infrastructure;
using EMPMGMT.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMPMGMT.Web.Controllers
{

    [CustomAuthorize(Roles = Constants.AUTHENTICATION_ROLE_ADMIN)]
    public class AdminController : Controller
    {
        private IUserBusiness userBusiness;
        private ICommentsnUnitBusiness commnetBusiness;
        public AdminController(IUserBusiness _userBusiness, ICommentsnUnitBusiness _commnetBusiness)
        {
            userBusiness = _userBusiness;
            commnetBusiness = _commnetBusiness;
        }
        public ActionResult AdminProfile()
        {
            ViewBag.successimport = false;
            return View(new EmployeeVM());
        }
        [HttpPost]
        public ActionResult ResetPassword(EmployeeVM userVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            EmployeeModel userModel = new EmployeeModel();
            AutoMapper.Mapper.Map(userVM, userModel);
            userModel.UserId = (int)SessionManagement.LoggedInUser.UserId;
            userModel.ActivationDate = null;
            userModel.DOB = null;
            string result = userBusiness.ResetPassword(userModel);
            return Json(result);
        }

        [HttpPost]
        public JsonResult UploadImage()
        {
            if (Request.Files.Count == 0)
            {
                return Json(new { statusCode = 500, status = "No image provided." });
            }
            var file = Request.Files[0];
            var fileExtension = Path.GetExtension(file.FileName);
            var userId = (int)SessionManagement.LoggedInUser.UserId;// Request.Form["UserId"].Decrypt();

            int imageWidth = ReadConfiguration.ProfileImageWidth;
            int imageHeight = ReadConfiguration.ProfileImageHieght;
            string imageName = Constants.PROFILE_IMAGE_NAME_PREFIX + userId + fileExtension;
            string imagePathAndName = Constants.PROFILE_IMAGE_PATH + imageName;

            if (SessionManagement.LoggedInUser.UserId == userId)
                SessionManagement.LoggedInUser.ProfileImageUrl = imageName;

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
            SessionManagement.LoggedInUser.ProfileImageUrl = Constants.PROFILE_IMAGE_PATH + imageName;
            // Return JSON
            return Json(new
            {
                statusCode = 200,
                status = "Image uploaded.",
                file = imagePathAndName,
            });

        }
        [HttpPost]
        public ActionResult UpdateUserProfile(EmployeeVM userVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            EmployeeModel userModel = new EmployeeModel();
            AutoMapper.Mapper.Map(userVM, userModel);
            userModel.UserId = (int)SessionManagement.LoggedInUser.UserId;
            userModel.Status = (int)Enums.UserStatus.Active;
            userModel.FirstName = userVM.FirstName;
            userModel.UserTypeId = (int)Enums.UserType.Admin;
            userModel.LastName = userVM.LastName;
            userModel.EmailId = userVM.EmailId;
            userBusiness.UpdateUserProfile(userModel);
            SessionManagement.LoggedInUser.UserName = userModel.FullName;
            return Json(response);
        }
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

        public ActionResult ViewAdminProfile()
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


        public ActionResult Home()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            SessionManagement.LoggedInUser = null;
            Session.Abandon();
            Session.RemoveAll();
            CommonFunctions.RemoveCookies();
            return RedirectToAction("Index", "Site");
        }
        public ActionResult RegisteredUsers()
        {
            return View();
        }

        public ActionResult GetUsers(ListingParameters listingParameters)
        {
            List<EmployeeVM> userVMList = new List<EmployeeVM>();
            List<EmployeeModel> userModelList = new List<EmployeeModel>();
            listingParameters.IsRegistered = true;
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


        public ActionResult GetUser(EmployeeVM userVM)
        {

            EmployeeModel userModel = new EmployeeModel();
            userModel = userBusiness.GetUser(userVM.UserId.Decrypt());
            AutoMapper.Mapper.Map(userModel, userVM);
            return Json(new
            {
                Data = userVM
            }
          , JsonRequestBehavior.AllowGet);

        }



        public ActionResult GetComments(CommentsVM userVM)
        {
            List<CommentsVM> commentListVM = new List<CommentsVM>();
            int userid = (int)userVM.CommentTo.Decrypt();
            int loginuserid = SessionManagement.LoggedInUser.UserId;
            List<CommentsModel> userModelList = new List<CommentsModel>();
            userModelList = commnetBusiness.GetCommentList(loginuserid, userid);
            AutoMapper.Mapper.Map(userModelList, commentListVM);
            return Json(new
            {

                commentdata = commentListVM

            }
          , JsonRequestBehavior.AllowGet);

        }
        public ActionResult SaveComments(CommentsVM commentVM)
        {
            string Message = "", status = "", responseMessage = "";
            if (commentVM.Comment.ToString() != "")
            {
                int loginuserid = SessionManagement.LoggedInUser.UserId;

                CommentsModel cm = new CommentsModel();
                cm.CommentTo = commentVM.CommentTo.Decrypt();
                cm.Comment = commentVM.Comment;
                cm.CommentBy = (int)loginuserid;
                cm.Isdelete = false;
                cm.CommentId = (int)commentVM.CommentId;

                bool succmsg = commnetBusiness.SaveComment(cm);
                if (succmsg)
                {
                    responseMessage = "Comment saved sucessfully";
                    status = "0";

                }
                else
                {
                    responseMessage = "There is something problem";
                    status = "1";

                }
            }
            else
            {
                responseMessage = "Comment can not be blank";
                status = "1";

            }



            return Json(new Response
            {
                Message = responseMessage,
                Status = status
            }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult DeleteComment(CommentsVM commentVM)
        {
            string responseMessage = "";

            CommentsModel cm = new CommentsModel();

            cm.CommentId = (int)commentVM.CommentId;

            bool succmsg = commnetBusiness.DeleteComment(cm);
            if (succmsg)
            {
                responseMessage = "Comment Deleted sucessfully";


            }
            else
            {
                responseMessage = "There is something problem";


            }


            return Json(new Response
            {
                Message = responseMessage,

            }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ResendMailToUser(EmployeeVM userVM)
        {
            string responseMessage = "";
            string Userguid = Guid.NewGuid().ToString();
            EmployeeModel userModel = new EmployeeModel();
            userVM.LinkSendDate = DateTime.UtcNow;
            userVM.UserGuid = Userguid;
            AutoMapper.Mapper.Map(userVM, userModel);
            userModel.FirstName = userVM.FullName;         
            SendMailToUser(userModel);
            bool isupdated = userBusiness.UpdateUserAfterMailSend(userModel.UserId, Userguid);
            if (isupdated)
            {
                responseMessage = "Resend Mail Successfully Send";

            }
            else
            {
                responseMessage = "Error";
            }
            return Json(new Response
            {
                Message = responseMessage

            });
        }
        private void SendMailToUser(EmployeeModel user)
        {
            var userid = user.UserId.Encrypt();
            user.IsLinkSend = (Boolean)true;
            MailHelper mailHelper = new MailHelper();
            MailBodyTemplate mailBodyTemplate = new MailBodyTemplate();
            string mailTemplateFolder = ReadConfiguration.MailTemplateFolder;
            string mailBody = string.Empty;
            mailHelper.Subject = Constants.RegistrationActivationMailSubject;
            mailHelper.ToEmail = user.EmailId;
            mailBody = CommonFunctions.ReadFile(Server.MapPath(mailTemplateFolder + Constants.InvitationActivationMailFileName));
            mailBodyTemplate.RegistrationUserName = user.FullName;
            mailBodyTemplate.MailBody = mailBody;
            mailBodyTemplate.AccountLoginUserId = user.EmailId;
            mailBodyTemplate.AccountLoginUrl = ReadConfiguration.WebsiteUrl + "ResetPassword?uid=" + user.UserGuid;
            mailBody = CommonFunctions.ConfigureActivationMailBody(mailBodyTemplate);
            mailHelper.Body = mailBody;
            mailHelper.SendEmail();
        }

        [HttpPost]
        public JsonResult RegisteredUserAccountAction(EmployeeVM requestParam)
        {
            string Userguid = Guid.NewGuid().ToString();
            EmployeeModel userModel = new EmployeeModel();
            userModel.UserId = requestParam.UserId.Decrypt();
            MailHelper mailHelper = new MailHelper();
            MailBodyTemplate mailBodyTemplate = new MailBodyTemplate();
            string mailTemplateFolder = ReadConfiguration.MailTemplateFolder;
            string mailBody = string.Empty;
            string responseMessage = string.Empty;
            //userModel.ModifiedBy = null;
            if (requestParam.ActionType == "Activate")
            {
                var userid = userModel.UserId.Encrypt();
                userModel.Status = (int)Enums.UserStatus.Active;
                //userModel.Password = Utility.CommonFunctions.GenerateUniqueNumber();
                userModel.Comments = requestParam.Comments;
               userModel = userBusiness.RegisteredUserActivate_Deactivate_Action(userModel);
                mailHelper.Subject = Constants.RegistrationActivationMailSubject;
                mailHelper.ToEmail = userModel.EmailId;
                mailBody = CommonFunctions.ReadFile(Server.MapPath(mailTemplateFolder + Constants.RegistrationActivationMailFileName));
                mailBodyTemplate.RegistrationUserName = userModel.FullName;
                if (!string.IsNullOrEmpty(requestParam.Comments))
                mailBodyTemplate.Comment_By_Admin = requestParam.Comments.Replace("\n", "<br/>");
                mailBodyTemplate.MailBody = mailBody;
                mailBodyTemplate.AccountLoginUserId = userModel.EmailId;
                mailBodyTemplate.AccountLoginPassowrd = userModel.Password;
                mailBodyTemplate.AccountLoginUrl = ReadConfiguration.WebsiteUrl + "ResetPassword?uid=" + Userguid;
                string module = string.Empty;
                string accessType = string.Empty;
                accessType = "Activate Account";
                mailBodyTemplate.AccessForDeploymentModule = module;
                mailBodyTemplate.ModulesAccessType = accessType;
                mailBody = CommonFunctions.ConfigureActivationMailBody(mailBodyTemplate);
                mailHelper.Body = mailBody;
                mailHelper.SendEmail();
                userBusiness.UpdateUserAfterMailSend((int)userModel.UserId, Userguid);
                if (requestParam.Comments != null)
                {
                    CommentsModel cm = new CommentsModel();
                    cm.CommentTo = userModel.UserId;
                    cm.Comment = requestParam.Comments;
                    cm.CommentBy = (int)SessionManagement.LoggedInUser.UserId;
                    cm.Isdelete = false;
                    bool succmsg = commnetBusiness.SaveComment(cm);

                }
                responseMessage = "Registered user activation is done successfully.User is now in  activation list";
                // user activation email
            }
            else if (requestParam.ActionType == "Denied")
            {
                userModel.Status = (int)Enums.UserStatus.Deactive;
                userModel.Comments = requestParam.Comments;
                userModel = userBusiness.RegisteredUserActivate_Deactivate_Action(userModel);
                // user Deactivation email
                mailHelper.Subject = Constants.RegistrationDeniedMailSubject;
                mailHelper.ToEmail = userModel.EmailId;
                mailBody = CommonFunctions.ReadFile(Server.MapPath(mailTemplateFolder + Constants.RegistrationDeniedMailFileName));
                mailBodyTemplate.RegistrationUserName = userModel.FullName;
                if (!string.IsNullOrEmpty(requestParam.Comments))
                    mailBodyTemplate.Comment_By_Admin = requestParam.Comments.Replace("\n", "<br/>");
                mailBodyTemplate.MailBody = mailBody;
                mailBody = CommonFunctions.ConfigureDeactivationMailBody(mailBodyTemplate);
                mailHelper.Body = mailBody;
                mailHelper.SendEmail();
                if (requestParam.Comments != null)
                {
                    CommentsModel cm = new CommentsModel();
                    cm.CommentTo = userModel.UserId;
                    cm.Comment = requestParam.Comments;
                    cm.CommentBy = (int)SessionManagement.LoggedInUser.UserId;
                    cm.Isdelete = false;
                    bool succmsg = commnetBusiness.SaveComment(cm);
                }
                responseMessage = "Registered user Deactivation is done successfully.User is now in  inactive users list";
            }
            else if (requestParam.ActionType == "MoreInfo")
            {
                //userModel.Active = false;
                userModel.Comments = requestParam.Comments;
                userModel = userBusiness.RegisteredUserActivate_Deactivate_Action(userModel);
                // user Deactivation email
                mailHelper.Subject = Constants.RegistrationMoreInfoMailSubject;
                mailHelper.ToEmail = userModel.EmailId;
                mailBody = CommonFunctions.ReadFile(Server.MapPath(mailTemplateFolder + Constants.RegistrationMoreInfoMailFileName));
                mailBodyTemplate.RegistrationUserName = userModel.FullName;
                if (!string.IsNullOrEmpty(requestParam.Comments))
                mailBodyTemplate.Comment_By_Admin = requestParam.Comments.Replace("\n", "<br/>");
                mailBodyTemplate.MailBody = mailBody;
                mailBody = CommonFunctions.ConfigureRegistrationMoreInfoMailBody(mailBodyTemplate);
                mailHelper.Body = mailBody;
                mailHelper.SendEmail();
                userModel.Status = (int)Enums.UserStatus.Moreinfo;

                userBusiness.updatemoreinfo_status(userModel);
                if (requestParam.Comments != null)
                {
                    CommentsModel cm = new CommentsModel();
                    cm.CommentTo = userModel.UserId;
                    cm.Comment = requestParam.Comments;
                    cm.CommentBy = (int)SessionManagement.LoggedInUser.UserId;
                    cm.Isdelete = false;
                    bool succmsg = commnetBusiness.SaveComment(cm);
                }

                responseMessage = "Request for more information about registered user is sent successfully?User is now in  on hold list";
            }

            return Json(new Response
            {
                Message = responseMessage

            });
        }




    }
}
