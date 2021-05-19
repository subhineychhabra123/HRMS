jQuery.namespace('EMPMGMT.User.ActionItemDescription');

var controllerUrl = "/Employee/";
 var ViewModel;
 var common = EMPMGMT.Framework.Common;
 EMPMGMT.User.ActionItemDescription.pageLoad = function () {
    CurrentPage = 1;
    ViewModel = new EMPMGMT.User.ActionItemDescription.pageViewModel();
    ko.applyBindings(ViewModel);
}

 EMPMGMT.User.ActionItemDescription.pageViewModel = function () {
     self.DashboardDocumentsList = ko.observableArray([]);
     self.GetDocumentsList = function () {
       //  debugger;
         var objParam = new Object();
         objParam.ActionItemId = $('#hdnActionItemId').val();

         EMPMGMT.Framework.Core.doPostOperation
                 (
                     controllerUrl + "GetActionItemDocuments",
                     objParam,
                     function onSuccess(response) {
                        // debugger;
                         if (response.DataList.length > 0) {
                             //   self.Visible(true);
                             $('#divdocuments').show();
                         }
                         else {
                             //self.Visible(false);
                             $('#divdocuments').hide();
                         }


                         self.RenderDocumentsList(response);
                         EMPMGMT.Framework.Common.ApplyPermission();
                     },
                     function onError(err) {
                         self.status(err.Message);
                     }
                 );
     }
     self.RenderDocumentsList = function (DocumentsList) {
       //  debugger;
         self.DashboardDocumentsList.removeAll();
         ko.utils.arrayForEach(DocumentsList.DataList, function (documentData) {
           //  debugger;
             self.DashboardDocumentsList.push(new EMPMGMT.User.ActionItemDescription.DocumentViewModel(documentData, DocumentsList.ImagePath));
         });

     };

     self.GetDocumentsList();

 }

 EMPMGMT.User.ActionItemDescription.DocumentViewModel = function (data, ImagePath) {
   // debugger;
    var self = this;
    var documentId = '', documentName = '', deleteDocumentVisible = false, attachedBy = '', attachedDate = '', removeDocument = '', documentPath = '', downloadDocument = '', deleteDocument = '', documentMouseOver = '', documnetMouseLeave = '', documentNameextn = '', date;
    if (data != undefined) {
        documentId = data.DocumentId;
        documentName = data.DocumentName;
        documentPath = data.DocumentPath;
        downloadDocument = ImagePath + data.DocumentPath;// data.ActionItemPath;
        date = data.CreatedDate;
        if (date == '' || date == undefined || date == null)
            date = '';
        else
        { date = dateFormat(common.ParsedJsonDate(date), "mm/dd/yyyy") }
        if (data.DocumentPath != undefined) {
            var pathext = data.DocumentPath.split('.');
            if (pathext.length > 0)
                documentNameextn = documentName + '.' + pathext[pathext.length - 1];
        }
    }

    self.DeleteDocumentVisible = ko.observable(deleteDocumentVisible);
  
      self.DocumentName = ko.observable(documentName);
    self.DocumentId = ko.observable(documentId);
    self.DocumentPath = ko.observable(documentPath);
    self.DownloadDocument = ko.observable(downloadDocument);

    self.DocumentNameextn = ko.observable(documentNameextn);

    self.Date = ko.observable(date);
    self.Visible = ko.observable(false);

    return true;
}


