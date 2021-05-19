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
    public interface IFileAttachmentsBusiness
    {
        FileAttachmentsModel SaveDocuments(FileAttachmentsModel fileAttachmentModel);
        FileAttachmentsModel SaveGoalDocuments(FileAttachmentsModel fileAttachmentModel);
        List<FileAttachmentsModel> GetDocumentsList(int? UserId);
        List<FileAttachmentsModel> GetDocumentsListByGoal(int goalId, ListingParameters listingParameters, ref int toatalRecords);

        void DeleteDocument(FileAttachmentsModel fileAttachmentModel);
        FileAttachmentsModel GetDocumentsListByDocumentId(int DocumentId);
        List<FileAttachmentsModel> GetDocumentsListByActionList(int? ActionListId);
        FileAttachmentsModel GetDocumentsByActionList(int? ActionListId);
        List<FileAttachmentsModel> GetDocumentsListByActionItem(int? ActionItemId);
        List<FileAttachmentsModel> GetUserDocumentsList(int? UserId);
        List<FileAttachmentsModel> GetDocumentsListByRCAId(ListingParameters listingParameters, ref int totalRecords); //int rcaId,
        FileAttachmentsModel SaveActionListDashboardDocuments(FileAttachmentsModel fileAttachmentsModel);
        FileAttachmentsModel GetDocumentsListByActionListId(int DocumentId);
        FileAttachmentsModel SaveMetricDashboardDocuments(FileAttachmentsModel fileAttachmentsModel);
    }
}
