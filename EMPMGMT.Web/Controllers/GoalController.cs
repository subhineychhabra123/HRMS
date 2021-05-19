using AutoMapper;
using EMPMGMT.Business.Interfaces;
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
using EMPMGMT.Domain;

namespace EMPMGMT.Web.Controllers
{
    //public partial class UserController : Controller
    //{
    //    private int CurrentCompanyId
    //    {
    //        get
    //        {
    //            return (int)SessionManagement.LoggedInUser.CompanyId;
    //        }
    //    }
    //    private int CurrentUserId
    //    {
    //        get { return SessionManagement.LoggedInUser.UserId; }
    //    }
    //    public UserController()
    //    {

    //    }

      
        
    //    public JsonResult GetResponsibles(string query, string ResponsibleusersId)
    //    {
    //        int companyId;
    //        companyId = (int)SessionManagement.LoggedInUser.CompanyId;
    //        List<EmployeeVM> userVMList = new List<EmployeeVM>();
    //        int active = (int)Enums.UserStatus.Active;
    //        List<EmployeeModel> userModelList = userBusiness.GetUsersListForAutocomplete(companyId, active, query);
    //        AutoMapper.Mapper.Map(userModelList, userVMList);
    //        return Json(userVMList, JsonRequestBehavior.AllowGet);

    //    }

    //    public JsonResult GetGoalResponsibles(string query, string goalId)
    //    {
    //        int companyId;
    //        int GoalId = goalId.Decrypt();
    //        companyId = (int)SessionManagement.LoggedInUser.CompanyId;
    //        List<EmployeeVM> userVMList = new List<EmployeeVM>();
    //        List<EmployeeModel> userModelList = userBusiness.GetUsersListForAutocomplete(GoalId, companyId, query);
    //        AutoMapper.Mapper.Map(userModelList, userVMList);
    //        return Json(userVMList, JsonRequestBehavior.AllowGet);

    //    }
        
        
       
       
      
       
       
       
       

        

       

       

        
      

       
        

       


       
      
      
        
       
       
       
       

    //    // Newly Added
       

    //    [EncryptedActionParameter]
    //    [HttpPost]
    //    public JsonResult UploadGoalDocument(String GoalId_encrypted)
    //    {
    //        if (Request.Files.Count == 0)
    //        {
    //            return Json(new { statusCode = 500, status = "No file uploaded." });
    //        }
    //        var file = Request.Files[0];
    //        var fileExtension = Path.GetExtension(file.FileName);
    //        var userId = (int)SessionManagement.LoggedInUser.UserId;// Request.Form["UserId"].Decrypt();
    //        var goalID = GoalId_encrypted;
    //        //int imageWidth = ReadConfiguration.ProfileImageWidth;
    //        //int imageHeight = ReadConfiguration.ProfileImageHieght;
    //        var DocumentName = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileExtension;
    //        string imagePathAndName = Constants.GOAL_DOCUMENTS_PATH + DocumentName;
    //        string ImageSavingPath = Server.MapPath(@"~" + imagePathAndName);

    //        try
    //        {
    //            CommonFunctions.UploadFile(file, ImageSavingPath, 0, 0);
    //        }
    //        catch (Exception ex)
    //        {
    //            return Json(new
    //            {
    //                statusCode = 500,
    //                status = "Error uploading image.",
    //                file = string.Empty
    //            });
    //        }
    //        FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();
    //        fileAttachmentsModel.AttachedBy = userId;
    //        fileAttachmentsModel.CreatedBy = userId;
    //        fileAttachmentsModel.CreatedDate = DateTime.Now;
    //        fileAttachmentsModel.ModifiedBy = userId;
    //        fileAttachmentsModel.ModifiedDate = DateTime.Now;
    //        fileAttachmentsModel.RecordDeleted = false;
    //        fileAttachmentsModel.DocumentName = Path.GetFileNameWithoutExtension(file.FileName);
    //        fileAttachmentsModel.DocumentPath = DocumentName;
    //        fileAttachmentsModel = fileAttachmentsBusiness.SaveGoalDocuments(fileAttachmentsModel);
    //        FileAttachmentsVM FileAttachmentsVM = new FileAttachmentsVM();
    //        AutoMapper.Mapper.Map(fileAttachmentsModel, FileAttachmentsVM);
    //       // FileAttachmentsVM.AttachBy = SessionManagement.LoggedInUser.FullName;
    //        // Return JSON
    //        return Json(new
    //        {
    //            statusCode = 200,
    //            status = "Image uploaded.",
    //            file = imagePathAndName,
    //            data = FileAttachmentsVM
    //        });

    //    }

    //    public ActionResult GetGoalDocumentsList(ListingParameters listingParameters)
    //    {
    //        List<FileAttachmentsVM> listFileAttachmentsVM = new List<FileAttachmentsVM>();
    //        List<FileAttachmentsModel> listFileAttachmentsModel = new List<FileAttachmentsModel>();
    //        FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();
    //        int totalRecords = 0;
    //        listingParameters.PageSize = Convert.ToInt16(EMPMGMT.Utility.ReadConfiguration.PageSize);

    //        listFileAttachmentsModel = fileAttachmentsBusiness.GetDocumentsListByGoal(listingParameters.GoalId.Decrypt(), listingParameters, ref totalRecords);
    //        AutoMapper.Mapper.Map(listFileAttachmentsModel, listFileAttachmentsVM);
    //        return Json(new
    //        {
    //            TotalRecords = totalRecords,
    //            DataList = listFileAttachmentsVM
    //        }
    //      , JsonRequestBehavior.AllowGet);
    //    }

    //    [HttpPost]
    //    public JsonResult DeleteGoalDocumentFile(FileAttachmentsVM fileAttachmentVM)
    //    {
    //        FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();
    //        AutoMapper.Mapper.Map(fileAttachmentVM, fileAttachmentsModel);
    //        fileAttachmentsModel.ModifiedBy = CurrentUserId;
    //        fileAttachmentsBusiness.DeleteDocument(fileAttachmentsModel);
    //        AutoMapper.Mapper.Map(fileAttachmentsModel, fileAttachmentVM);
    //        return Json(new { Data = fileAttachmentVM });
    //    }

       
       

      
      

     

    //    public JsonResult GetActionList(string query)
    //    {
    //        int companyId;
    //        companyId = (int)SessionManagement.LoggedInUser.CompanyId;
    //        List<ActionListVM> actionListVM = new List<ActionListVM>();
    //        List<ActionListModel> actionListModel = actionListBussiness.GetActionListForAutoComplete(companyId, query);
    //        AutoMapper.Mapper.Map(actionListModel, actionListVM);
    //        return Json(actionListVM, JsonRequestBehavior.AllowGet);

    //    }

      


       

    //    //public JsonResult GetMetricsAsTTI(ListingParameters listingParam)
    //    //{
    //    //    List<MetricVM> metricVM = new List<MetricVM>();
    //    //    List<MetricModel> metricModel = metricDashboardMetricsBusiness.GetMetricsAsTTIByMetricDashboard(listingParam.MetricDashboardId.Decrypt(), listingParam.GoalId.Decrypt());
    //    //    AutoMapper.Mapper.Map(metricModel, metricVM);
    //    //    return Json(metricVM, JsonRequestBehavior.AllowGet);
    //    //}

       
    //}
}