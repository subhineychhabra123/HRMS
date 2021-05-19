jQuery.namespace('EMPMGMT.User.ActionList');
var controllerUrl = "/Employee/";
var viewModelActionList;

var Message = {
    Failure: 'Failure',
    Success: 'Success'
}

EMPMGMT.User.ActionList.pageLoad = function () {
    viewModelActionList = new EMPMGMT.User.ActionList.pageViewModel();
    ko.applyBindings(viewModelActionList);
}


EMPMGMT.User.ActionList.DocumentViewModel = function (data,ImagePath) {
   
    var self = this;
    var documentId = '', documentName = '', deleteDocumentVisible = false, attachedBy = '', attachedDate = '', removeDocument = '', documentPath = '', downloadDocument = '', deleteDocument = '', documentMouseOver = '', documnetMouseLeave = '';;
    if (data != undefined) {
        documentId = data.DocumentId;
        documentName = data.DocumentName;
        //attachedBy = data.AttachmentByName;
        //attachedDate = data.CreatedDate;
        documentPath = data.DocumentPath;
    
        downloadDocument = ImagePath + documentPath
        documentMouseOver = viewModelActionList.DocumentMouseOverAction;
        documentMouseLeave = viewModelActionList.DocumentMouseLeaveAction;
        deleteDocument = viewModelActionList.DeleteDocumentAction;
       
    }
    self.DeleteDocumentVisible = ko.observable(deleteDocumentVisible);
    self.DeleteDocument = deleteDocument;
    self.DocumentMouseOver = documentMouseOver;
    self.DocumentMouseLeave = documentMouseLeave;
    self.DocumentName = ko.observable(documentName);
    self.DocumentId = ko.observable(documentId);
    //self.CreatedDate = ko.observable(dateFormat(common.ParsedJsonDate(attachedDate), "mm/dd/yyyy"));
    //self.AttachedByName = ko.observable(attachedBy);
    self.DocumentPath = ko.observable(documentPath);
    self.DownloadDocument = ko.observable(downloadDocument);

    return true;
}
EMPMGMT.User.ActionList.pageViewModel = function (data) {
    var self = this;
   
    self.ResponsibleUserId = ko.observable('');
    self.ResponsibleUserName = ko.observable('');
    self.DocumentDivVisible = ko.observable(false);




    //Related Document Functionality................................
    self.DashboardDocumentsList = ko.observableArray([]);
    self.GetDocumentsList = function () {
        var objParam = new Object();
        objParam.ActionListId = $('#hdnActionListId').val();

        EMPMGMT.Framework.Core.doPostOperation
                (
                    controllerUrl + "GetActionListDocuments",
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
        self.DashboardDocumentsList.removeAll();
        ko.utils.arrayForEach(DocumentsList.DataList, function (documentData) {
            //
            self.DashboardDocumentsList.push(new EMPMGMT.User.ActionList.DocumentViewModel(documentData, DocumentsList.ImagePath));
        });
        if (self.DashboardDocumentsList().length == 0)
            $("._DocumentsNoRecord").show();
        else
            $("._DocumentsNoRecord").hide();

       
    };


    self.DocumentMouseOverAction = function (data) {
        data.DeleteDocumentVisible(true);
    }
    self.DocumentMouseLeaveAction = function (data) {
        data.DeleteDocumentVisible(false);
        //  data.NoOrgResponsibleHoverClass('');
    }


    self.DeleteDocumentAction = function (data) {
        //debugger;
        var ret = confirm(EMPMGMT.Messages.ActionList.DeleteDocument);
        if (ret) {
            var PostData = new Object();
            PostData.DocumentId = data.DocumentId();

            EMPMGMT.Framework.Core.doPostOperation
                    (
                        controllerUrl + "DeleteDocument",
                        PostData,
                        function onSuccess(response) {
                            self.DashboardDocumentsList.remove(data);
                            EMPMGMT.Framework.Common.ApplyPermission();
                            if (self.DashboardDocumentsList().length == 0)
                                $("._DocumentsNoRecord").show();
                            else
                                $("._DocumentsNoRecord").hide();
                        },
                        function onError(err) {
                            self.status(err.Message);
                        }
                    );

        }
    }


    //End Of related document Functionality................................

    //Upload Documents...................................


    var uploadObj = $("#fileupload").uploadFile({
        url: '/Employee/UploadActionListDocument',
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


            //  var profileId1 = ko.utils.unwrapObservable($('._proflelstdropdown').val())
         
            var PostData = new Object();
            PostData.ActionListId_encrypted = $('#hdnActionListId').val();

            return PostData;

        },
        showStatusAfterSuccess: false,
        onSubmit: function (files) {

        },
        onSuccess: function (files, data, xhr) {
    
            var postData = new Object();
            postData.DocumentName = data.DataList.DocumentName;
            postData.DocumentId = data.DataList.DocumentId;
            postData.DocumentPath = data.DataList.DocumentPath; 
            postData.ActionListPath = data.DataList.ActionListPath;

            self.DashboardDocumentsList.push(new EMPMGMT.User.ActionList.DocumentViewModel(postData,data.ImagePath));
            if (self.DashboardDocumentsList().length == 0)
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


    //End Of Upload Documents......................................





    self.Init = function () {
        if ($('#hdnActionListId').val() != "") {
            self.DocumentDivVisible = ko.observable(true);
            self.GetDocumentsList();
        }
        else {
          self.DocumentDivVisible = ko.observable(false);
        }
   
        $('._ResponsibleUserAutoComplete').autocomplete({
            
            data: "{'ProjectId':'" + $('#hdnProjectId').val() + "'}",
            serviceUrl: '/Employee/GetProjectResponsibles/' + $('#hdnProjectId').val(), //GetResponsibles
            displayProperty: 'FullName',
            onChnage: function () {
                //debugger;
                var PrId = $('#hdnProjectId').val();
               // self.ResponsibleUserId('');
              //  self.ResponsibleUserName('');
                //$('#hdnResponsibleUserId').val('');              

            },
            params: {},
            lookup: null,
            onSelect: function (suggestion) {
                self.ResponsibleUserId(suggestion.data.UserId);
                $('#hdnResponsibleUserId').val(suggestion.data.UserId);
                self.ResponsibleUserName(suggestion.value);
            },
        });


        //Slider Sataus Script.......................................................
            $("#slider2").slider({
                value: 0,
                min: 0,
                max: 100,
                step:5,
                slide: function (event, ui) {
                    $("#amount2").val( ui.value +"%");
                }
            });
            $("#amount2").val( $("#slider2").slider("value") +"%");
        //End Slider Status Script...................................................
    }

    self.Init();

    self.CancelActionList = function () {
        window.location.href = "/Employee/ManageActionList";
    }
   

    //ko.bindingHandlers.slider = {
    //    init: function (element, valueAccessor, allBindingsAccessor) {
    //        var options = allBindingsAccessor().sliderOptions || {};
    //        $(element).slider(options);
    //        ko.utils.registerEventHandler(element, "slidechange", function (event, ui) {
    //            var observable = valueAccessor();
                
  
    //            $('#hdnSliderValue').val(ui.value);
               
    //            observable(ui.value);
    //        });
    //        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
    //            $(element).slider("destroy");
    //        });
    //        ko.utils.registerEventHandler(element, "slide", function (event, ui) {
    //            var observable = valueAccessor();
    //            observable(ui.value);
    //        });
    //    },
    //    update: function (element, valueAccessor) {
    //        var value = ko.utils.unwrapObservable(valueAccessor());
    //        if (isNaN(value)) value = 0;
    //        $(element).slider("value", value);
    //    }
    //};
    //var ss = new EMPMGMT.User.ActionList.SliderViewModel();

    //var ViewModel = function () {
    //    var self = this;

    //    self.savings = ko.observable(10);
    //    self.spent = ko.observable(5);
    //    self.net = ko.computed(function () {
    //        return self.savings() - self.spent();
    //    });
    //}

    //ko.applyBindings(new ViewModel());
}