using EMPMGMT.Repository;
using EMPMGMT.Business.Interfaces;
using EMPMGMT.Domain;

using EMPMGMT.Repository.Infrastructure.Contract;
using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Business
{
    public class FileAttachmentsBusiness : IFileAttachmentsBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly FileAttachmentsRepository fileAttachmentsRepository;
        private readonly ActionItemRepository actionItemRepository;

        public FileAttachmentsBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            fileAttachmentsRepository = new FileAttachmentsRepository(unitOfWork);
            actionItemRepository = new ActionItemRepository(unitOfWork);

        }
        public FileAttachmentsModel SaveDocuments(FileAttachmentsModel fileAttachmentsModel)
        {
            FileAttachments fileAttachments = new FileAttachments();
           
            AutoMapper.Mapper.Map(fileAttachmentsModel, fileAttachments);
            fileAttachmentsRepository.Insert(fileAttachments);
            AutoMapper.Mapper.Map(fileAttachments, fileAttachmentsModel);
            return fileAttachmentsModel;
        }
        public List<FileAttachmentsModel> GetDocumentsList(int? UserId)
        {
            List<FileAttachmentsModel> listFileAttachmentsModel = new List<FileAttachmentsModel>();
            List<FileAttachments> listFileAttachments = fileAttachmentsRepository.GetAll(p => p.RecordDeleted == false).ToList();
            AutoMapper.Mapper.Map(listFileAttachments, listFileAttachmentsModel);
            return listFileAttachmentsModel;
        }
        public FileAttachmentsModel SaveGoalDocuments(FileAttachmentsModel fileAttachmentsModel)
        {
            FileAttachments fileAttachments = new FileAttachments();
            AutoMapper.Mapper.Map(fileAttachmentsModel, fileAttachments);
            fileAttachmentsRepository.Insert(fileAttachments);
            AutoMapper.Mapper.Map(fileAttachments, fileAttachmentsModel);
            return fileAttachmentsModel;
        }
        public List<FileAttachmentsModel> GetDocumentsListByGoal(int goalId, ListingParameters listingParameters, ref int totalRecords)
        {
            int currentPage = listingParameters.CurrentPage;
            int pageSize = listingParameters.PageSize;
            List<FileAttachmentsModel> listFileAttachmentsModel = new List<FileAttachmentsModel>();
            totalRecords = fileAttachmentsRepository.Count(x => x.RecordDeleted == false);
            List<FileAttachments> listFileAttachments = fileAttachmentsRepository.GetPagedRecords(x => x.RecordDeleted == false, y => y.CreatedDate, currentPage, pageSize).OrderByDescending(c => c.CreatedDate).ToList();
            AutoMapper.Mapper.Map(listFileAttachments, listFileAttachmentsModel);
            return listFileAttachmentsModel;
        }
        public List<FileAttachmentsModel> GetDocumentsListByActionItem(int? ActionItemId)
        {
            List<FileAttachmentsModel> listFileAttachmentsModel = new List<FileAttachmentsModel>();

            List<FileAttachments> listFileAttachments = fileAttachmentsRepository.GetAll(x => x.ActionItemId == ActionItemId && x.RecordDeleted == false).ToList();
            AutoMapper.Mapper.Map(listFileAttachments, listFileAttachmentsModel);
            return listFileAttachmentsModel;
        }

        public  List<FileAttachmentsModel> GetDocumentsListByActionList(int? ActionListId)
        {
            List<FileAttachmentsModel> listFileAttachmentsModel = new List<FileAttachmentsModel>();

            List<FileAttachments> listFileAttachments = fileAttachmentsRepository.GetAll(x => x.ActionListId == ActionListId && x.RecordDeleted == false).ToList();
            AutoMapper.Mapper.Map(listFileAttachments, listFileAttachmentsModel);
            return listFileAttachmentsModel;
        }

        public FileAttachmentsModel GetDocumentsByActionList(int? ActionListId)
        {
            FileAttachmentsModel listFileAttachmentsModel = new FileAttachmentsModel();

            FileAttachments listFileAttachments = fileAttachmentsRepository.SingleOrDefault(x => x.ActionListId == ActionListId && x.RecordDeleted == false);
            AutoMapper.Mapper.Map(listFileAttachments, listFileAttachmentsModel);
            return listFileAttachmentsModel;
        }

        public FileAttachmentsModel GetDocumentsListByActionListId(int DocumentId)
        {
            FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();

            FileAttachments fileAttachments = fileAttachmentsRepository.SingleOrDefault(x => x.DocumentId == DocumentId && x.RecordDeleted == false);
            AutoMapper.Mapper.Map(fileAttachments, fileAttachmentsModel);
            return fileAttachmentsModel;
        }

    
        public List<FileAttachmentsModel> GetDocumentsListByMetricDataValueIDs(int? MetricDashboardDailyDataId, int? MetricDashboardWeeklyDataId, int? MetricDashboardMonthlyId)
        {
            List<FileAttachmentsModel> listFileAttachmentsModel = new List<FileAttachmentsModel>();

            List<FileAttachments> listFileAttachments = fileAttachmentsRepository.GetAll(x => x.RecordDeleted==false).ToList();
            AutoMapper.Mapper.Map(listFileAttachments, listFileAttachmentsModel);
            return listFileAttachmentsModel;
        }
        public FileAttachmentsModel GetDocumentsListByDocumentId(int DocumentId)
        {
          FileAttachmentsModel fileAttachmentsModel = new FileAttachmentsModel();

            FileAttachments fileAttachments = fileAttachmentsRepository.SingleOrDefault(x => x.DocumentId==DocumentId && x.RecordDeleted == false);
            AutoMapper.Mapper.Map(fileAttachments, fileAttachmentsModel);
            return fileAttachmentsModel;
        }

        public void DeleteDocument(FileAttachmentsModel fileAttachmentsModel)
        {
            FileAttachments fileAttachment = new FileAttachments();

            if (fileAttachmentsModel.DocumentId > 0)
            {
                fileAttachment = fileAttachmentsRepository.SingleOrDefault(x => x.DocumentId == fileAttachmentsModel.DocumentId && x.RecordDeleted != true);
                if (fileAttachment != null)
                {
                    fileAttachment.RecordDeleted = true;
                    fileAttachment.ModifiedBy = fileAttachmentsModel.ModifiedBy;
                    fileAttachment.ModifiedDate = DateTime.UtcNow;
                    fileAttachmentsRepository.Update(fileAttachment);
                }
            }
        }

        public List<FileAttachmentsModel> GetDocumentsListByRCAId(ListingParameters listingParameters, ref int totalRecords) //int rcaId,
        {
            int currentPage = listingParameters.CurrentPage;
            int pageSize = listingParameters.PageSize;
            List<FileAttachmentsModel> listFileAttachmentsModel = new List<FileAttachmentsModel>();
            totalRecords = fileAttachmentsRepository.Count(x => x.RecordDeleted == false);
            List<FileAttachments> listFileAttachments = fileAttachmentsRepository.GetPagedRecords(x => x.RecordDeleted == false, y => y.CreatedDate, currentPage, pageSize).OrderByDescending(c => c.CreatedDate).ToList();
            AutoMapper.Mapper.Map(listFileAttachments, listFileAttachmentsModel);
            return listFileAttachmentsModel;
        }

        #region ActionList Document

        public FileAttachmentsModel SaveActionListDashboardDocuments(FileAttachmentsModel fileAttachmentsModel)
        {
           
            FileAttachments fileAttachments = new FileAttachments();

            AutoMapper.Mapper.Map(fileAttachmentsModel, fileAttachments);
            fileAttachmentsRepository.Insert(fileAttachments);
            AutoMapper.Mapper.Map(fileAttachments, fileAttachmentsModel);
            return fileAttachmentsModel;
        }
        #endregion



   public  List<FileAttachmentsModel> GetUserDocumentsList(int? UserId)
        {
            List<FileAttachmentsModel> listFileAttachmentsModel = new List<FileAttachmentsModel>();

            List<FileAttachments> listFileAttachments = fileAttachmentsRepository.GetAll(x => x.UserId == UserId && x.RecordDeleted == false).ToList();
            AutoMapper.Mapper.Map(listFileAttachments, listFileAttachmentsModel);
            return listFileAttachmentsModel;
        }

   #region SaveDocuments actionItemlist
   public FileAttachmentsModel SaveMetricDashboardDocuments(FileAttachmentsModel fileAttachmentsModel)
   {
       FileAttachments fileAttachments = new FileAttachments();
       AutoMapper.Mapper.Map(fileAttachmentsModel, fileAttachments);
       var exists = actionItemRepository.Exists(x => x.ActionItemId == fileAttachmentsModel.ActionItemId && x.RecordDeleted == false);
       //var exitsdocument = fileAttachmentsRepository.Exists(x => x.ActionItemId == fileAttachmentsModel.ActionItemId && x.DocumentName == fileAttachmentsModel.DocumentName && x.RecordDeleted == false);
       if (exists)
       {
          // if (!exitsdocument)
               fileAttachmentsRepository.Insert(fileAttachments);
       }
       AutoMapper.Mapper.Map(fileAttachments, fileAttachmentsModel);
       return fileAttachmentsModel;
   }
        #endregion

    }
}
