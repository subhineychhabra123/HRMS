jQuery.namespace('EMPMGMT.User.UserDocument');
var controllerUrl = "/Employee/";
var viewModelUserDocument;

var Message = {
    Failure: 'Failure',
    Success: 'Success'
}

EMPMGMT.User.UserDocument.pageLoad = function () {
    viewModelUserDocument = new EMPMGMT.User.UserDocument.pageViewModel();
    ko.applyBindings(viewModelUserDocument, document.getElementById("AddDocumentPopup"));
}

EMPMGMT.User.UserDocument.DocumentViewModel = function (data) {
    var self = this;
    var documentId = '', documentName = '', deleteDocumentVisible = false, attachedDate = '', removeDocument = '', documentPath = '', downloadDocument = '', deleteDocument = '', documnetMouseLeave = '', userId = '', documentMouseOver = '';
    if (data != undefined) {
        documentId = data.DocumentId;
        documentName = data.DocumentName;
        documentPath = data.DocumentPath;
        downloadDocument = data.UserDocumentPath;
        documentMouseOver = viewModelUserDocument.DocumentMouseOverAction;
        documentMouseLeave = viewModelUserDocument.DocumentMouseLeaveAction;
        deleteDocument = viewModelUserDocument.DeleteDocumentAction;

    }
    self.DeleteDocumentVisible = ko.observable(deleteDocumentVisible);
    self.DeleteDocument = deleteDocument;
    self.DocumentMouseOver = documentMouseOver;
    self.DocumentMouseLeave = documentMouseLeave;
    self.DocumentName = ko.observable(documentName);
    self.DocumentId = ko.observable(documentId);
    self.DocumentPath = ko.observable(documentPath);
    self.DownloadDocument = ko.observable(downloadDocument);

    return true;
}
EMPMGMT.User.UserDocument.pageViewModel = function (data) {
    var self = this;
    //Related Document Functionality................................
    self.Document = ko.observableArray([]);
    self.GetDocumentsList = function () {
        var objParam = new Object();
        objParam.UserId_encrypted = $('#hdnUserId').val();
        EMPMGMT.Framework.Core.doPostOperation
               (
                   controllerUrl + "GetUserDocuments",
                   objParam,
                   function onSuccess(response) {
                       self.RenderDocumentsList(response);
                       EMPMGMT.Framework.Common.ApplyPermission();
                   },
                   function onError(err) {
                       self.status(err.Message);
                   }
               );
    }

    self.RenderDocumentsList = function (DocumentsList) {
        self.Document.removeAll();
        ko.utils.arrayForEach(DocumentsList.DataList, function (documentData) {
            self.Document.push(new EMPMGMT.User.UserDocument.DocumentViewModel(documentData));
        });
        if (self.Document().length == 0)
            $("._DocumentsNoRecord").show();
        else
            $("._DocumentsNoRecord").hide();

        //EnableDisableMetric();
    };
    self.DocumentMouseOverAction = function (data) {
        data.DeleteDocumentVisible(true);
    }
    self.DocumentMouseLeaveAction = function (data) {
        data.DeleteDocumentVisible(false);
        //  data.NoOrgResponsibleHoverClass('');
    }


    self.DeleteDocumentAction = function (data) {

        var ret = confirm(EMPMGMT.Messages.UserDocument.DeleteDocument);
        if (ret) {
            var PostData = new Object();
            PostData.DocumentId = data.DocumentId();

            EMPMGMT.Framework.Core.doPostOperation
                    (
                        controllerUrl + "DeleteDocument",
                        PostData,
                        function onSuccess(response) {
                            self.GetDocumentsList();
                        },
                        function onError(err) {
                            self.status(err.Message);
                        }
                    );

        }
    }


    //End Of related document Functionality................................



    self.Init = function () {

        if ($('#hdnUserId').val() != "") {
        self.DocumentDivVisible = ko.observable(true);
            self.GetDocumentsList();
        }
        else {
            self.DocumentDivVisible = ko.observable(false);
        }
    }

    self.Init();

    self.CancelUserDocument = function () {
        window.location.href = "/Employee/ManageUserDocument";
    }



    var uploadObj = $("#fileuploadDocoments").uploadFile({
        url: '/Employee/UploadUserDocument',
        // maxFileCount: 1,
        multiple: false,
        autoSubmit: false,
        // fileName: "docs",
        maxFileSize: 1024 * 10000,
        fileCounter: 0,

        onCancel: function (e) {
            uploadObj.selectedFiles = 0;
            e.perventDefault();
            return true;
        },

        dynamicFormData: function () {
         
            var PostData = new Object();
            PostData.UserId_encrypted = $('#hdnUserId').val();
            return PostData;
        },

        showStatusAfterSuccess: false,
        multiple: false,
        onSubmit: function (files) {

        },

        onSuccess: function (files, data, xhr) {
           
            var postData = new Object();
            postData.DocumentName = data.DataList.DocumentName;
            postData.DocumentId = data.DataList.DocumentId;
            postData.DocumentPath = data.Documentpath;
            //postData.UserDocumentPath = data.DataList.UserDocumentPath;

            self.Document.push(new EMPMGMT.User.UserDocument.DocumentViewModel(postData));

            if (self.Document().length == 0)
                $("._DocumentsNoRecord").show();
            else
                $("._DocumentsNoRecord").hide();
        },
        afterUploadAll: function () {

        },

        onError: function (files, status, errMsg) {

        }
    });
    uploadObj.selectedFiles = 0;
    $("#ButtonUploadUser").bind("click", function () {
        if (uploadObj == null || uploadObj.selectedFiles == 0) {
            alert("Please select any file")
        }
        else {
            uploadObj.startUpload();
            uploadObj.selectedFiles = 0;
        }
    });

}
