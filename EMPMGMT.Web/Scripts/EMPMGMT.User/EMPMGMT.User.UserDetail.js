//Register name space
jQuery.namespace('EMPMGMT.User.UserDetail');
var logedInUser = '';

if ($('#hdnUserId').val() != "") {
    $('#EditAddHeading').text('Edit Employee');
    $('._EditUserBtn, ._EditCancelBtn').show();
    $('._AddUserBtn, ._AddCancelBtn').hide();
    $('#Documents').show();
}
else {
    $('#EditAddHeading').text('Create Employee');
    $('._EditUserBtn, ._EditCancelBtn').hide();
    $('._AddUserBtn, ._AddCancelBtn').show();
    $('._emailidTxt').removeAttr('readonly');

}
var config = EMPMGMT.Framework.Core.Config;
var viewModelUserDetail;
var controllerUrl = "/Employee/";
var Message = {
    Failure: 'Failure',
    Success: 'Success'
}
EMPMGMT.User.UserDetail.pageLoad = function () {

    viewModelUserDetail = new EMPMGMT.User.UserDetail.pageViewModel();
    ko.applyBindings(viewModelUserDetail);
}

//***************************Validation ************************//

ko.validation.rules.pattern.message = 'Invalid.';
ko.validation.configure({
    registerExtenders: true,
    messagesOnModified: true,
    insertMessages: true,
    parseInputAttributes: true,
    messageTemplate: null
});
//*************************** End ********************************//


EMPMGMT.User.UserDetail.UserDetailViewModel = function (user) {
    var self = this;
    var firstName = '', lastName = '', emailId = '', reportTo = '', profileName = '', organisationUnit = '', dateOfJoining = '', dob = '', fatherName = '', motherName = '', phone = '', refferName = '', designation = '', technology = '', active = '', pan = '', remark = '', photograph = '', corrAddress = '', permanentAddress = '', imgImageURL = '', reportToFullName = '', empCode='';
    debugger;
    if (user != undefined) {
        firstName = user.FirstName;
        lastName = user.LastName;
        emailId = user.EmailId;
        reportTo = user.ReportTo;
        profileName = user.ProfileName;
        organisationUnit = user.OrgUnitName;
        dateOfJoining = user.DateOfJoining;
        dob = user.DOB;
        fatherName = user.FatherName;
        motherName = user.MotherName;
        phone = user.Phone;
        refferName = user.ReferrerName;
        designation = user.DesignationName;
        technology = user.TechnologyName;
        active = user.Active;
        pan = user.PAN;
        remark = user.Remark;
        photograph = user.Photograph;
        corrAddress = user.CorrespondenceAddr;
        permanentAddress = user.PermanentAddr;
        reportToFullName = user.ReportToFullName;
        empCode = user.EmpCode;
    }

    self.FirstName = ko.observable(firstName);
    self.LastName = ko.observable(lastName);
    self.EmailId = ko.observable(emailId);
    self.ProfileName = ko.observable(profileName);
    self.OrganisationUnit = ko.observable(organisationUnit);
    self.DateOfJoining = ko.observable(dateOfJoining);
    self.DOB = ko.observable(dob);
    self.FatherName = ko.observable(fatherName);
    self.MotherName = ko.observable(motherName);
    self.Phone = ko.observable(phone);
    self.ReferrerName = ko.observable(refferName);
    self.ReferrerId = ko.observable('');
    self.UserId = ko.observable('');
    
    self.ReportTo = ko.observable(reportTo);
    self.FullName = ko.observable(reportToFullName);
    self.DesignationName = ko.observable(designation);
    self.TechnologyName = ko.observable(technology);
    self.Active = ko.observable(active);
    self.PAN = ko.observable(pan);
    self.Remarks = ko.observable(remark);
    self.Photograph = ko.observable(photograph);
    self.CorrespondenceAddr = ko.observable(corrAddress);
    self.PermanentAddr = ko.observable(permanentAddress);
    self.ImageURL = ko.observable(imgImageURL);
    self.EmpCode = ko.observable(empCode);

    return self;
}
//***********************************************************************//

EMPMGMT.User.UserDetail.userViewModel = function (user) {
    var self = this;
    var password = '', confirmPassword = '';
    if (user != undefined) {
        password = user.Password;
        confirmPassword = user.ConfirmPassword;

    }
    self.Password = ko.observable(password);
    self.ConfirmPassword = ko.observable(confirmPassword);

    return self;
}
EMPMGMT.User.UserDetail.DocumentViewModel = function (data, ImagePath) {
  
    var self = this;
    var documentId = '', documentName = '', deleteDocumentVisible = false, attachedDate = '', removeDocument = '', documentPath = '', downloadDocument = '', deleteDocument = '', documnetMouseLeave = '', userId = '', documentMouseOver = '', documentNameextn = ''
       
    if (data != undefined) {
    
        documentId = data.DocumentId;
        documentName = data.DocumentName;
        documentPath = data.DocumentPath;
        downloadDocument = ImagePath + documentPath;
        documentMouseOver = viewModelUserDetail.DocumentMouseOverAction;
        documentMouseLeave = viewModelUserDetail.DocumentMouseLeaveAction;
        deleteDocument = viewModelUserDetail.DeleteDocumentAction;
        if (data.DocumentPath != undefined) {
            var extn = data.DocumentPath.split('.');
            documentNameextn = data.DocumentName + '.' + extn[extn.length - 1]
        }

    }
    self.DeleteDocumentVisible = ko.observable(deleteDocumentVisible);
    self.DeleteDocument = deleteDocument;
    self.DocumentMouseOver = documentMouseOver;
    self.DocumentMouseLeave = documentMouseLeave;
    self.DocumentName = ko.observable(documentName);
    self.DocumentId = ko.observable(documentId);
    self.DocumentPath = ko.observable(documentPath);
    self.DownloadDocument = ko.observable(downloadDocument);
    self.DocumentNameextn = ko.observable(documentNameextn);

    return true;
}
//Page view model
EMPMGMT.User.UserDetail.pageViewModel = function () {
    //Class variables


    var self = this;

    self.RefferNameId = ko.observable('');
    self.RefferName = ko.observable('');

    var userDocumentsPopupHandler = {
        open: function () {
            userDocumentsPopupObj.methods.open();
        },
        close: function () {
            userDocumentsPopupObj.methods.close();
        }
    }
    //******************Reset Password PopUp functionality *************************************//
    $("._resetPasswordPopbox").popbox({
        open: '._resetPasswordOpen',
        arrow: '.arrow',
        arrow_border: '.arrow-border',
        close: '._resetPasswordClose,._resetConfigurationClose',
        overlay: '._popup_overlay'
    });
    var userDocumentsPopupObj = $("._UserDocumentsPopbox").popbox({
        arrow: '.arrow',
        arrow_border: '.arrow-border',
        close: '._UserDetailsClose,._resetConfigurationClose',
        overlay: '._popup_overlay'
    });



    //*****************************End*********************************************//
    self.ResetPasswordVisibility = ko.observable(false);
    self.editUserVisibility = ko.observable(false);
    self.User = new EMPMGMT.User.UserDetail.UserDetailViewModel();
    //self.viewUser = new EMPMGMT.User.UserDetail.UserDetailViewModel();
    self.User.FirstName = ko.observable('').extend({  // custom message
        required: {
            message: 'First name is required'
        }
    });
    self.User.LastName = ko.observable('');
    self.User.EmailId = ko.observable('').extend({  // custom message
        required: {
            message: 'Email Address is required'
        },
        email: {
            message: 'Please enter valid email address'
        }

        //email: true
    });

    self.Password = ko.observable('').extend({  // custom message
        required: {
            message: 'Password is required'
        }
    });
    self.ConfirmPassword = ko.observable('').extend({  // custom message
        required: {
            message: 'Confirm Password is required'
        }

    });
    self.User.DateOfJoining = ko.observable('').extend({  // custom message
        required: {
            message: 'Date of Joining is required'
        }

    });
  
    self.User.Phone = ko.observable().extend({  // custom message
        pattern: {
            message: 'Invalid phone number',
            params: '^\\D?(\\d{3})\\D?\\D?(\\d{3})\\D?(\\d{4})$'
        }

    });
    self.User.PAN = ko.observable().extend({  // custom message
        pattern: {
            message: 'Invalid Permanent Account Number',
            params:  '^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$'
        }

    });
    self.User.FullName = ko.observable('').extend({  // custom message
        required: {
            message: "Senior's Name is required"
        }

    });

    //****************************Reset PassWord Click functionality ********************//
    self.ResetYourPassword = function (data) {
        if (self.pswerrors().length == 0) {
            if (self.Password().length < 6) {

                EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Password.PasswordLengthMessage, true);
            }
            else if (self.Password() != self.ConfirmPassword()) {
                EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Password.PasswordMismatch, true);
            }

            else {
                var PostData = new Object();
                PostData.Password = data.Password();
                PostData.UserId = $('#hdnUserId').val();
                EMPMGMT.Framework.Core.doPostOperation
                       (
                           controllerUrl + "ResetUserPassword",
                           PostData,
                           function onSuccess(response) {
                               if (response == "Successful") {
                                   $('._Password').val('');
                                   $('._CPassword').val('');
                                   $("._resetPasswordClose").click();
                                   EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Password.ResetPasswordSuccess, false);
                                   $("#passwordPopUpClose").click();
                               }
                               else {
                                   EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Password.InvalidUser, true);
                               }

                           },
                           function onError(err) {
                               self.status(err.Message);
                           }
                       );
            }
        } else {
            self.pswerrors.showAllMessages();
        }
    }
    //******************************End ******************************************//

    //*******************Add/Create New User******************************//
    self.AddUserDetail = function () {
        debugger;
        if (self.EditUserErrors().length == 0) {
            var PostData = new Object();
            PostData.FirstName = self.User.FirstName();
            PostData.LastName = self.User.LastName();
            PostData.EmailId = self.User.EmailId();
            PostData.ProfileId = $("#profilelstdropdown option:selected").val();// PostData.ProfileId = 2;
            PostData.OrgUnitId = $("#ddlOrganisationUnit option:selected").val();
            PostData.DateOfJoining = self.User.DateOfJoining();
            PostData.DOB = self.User.DOB();
            PostData.FatherName = self.User.FatherName();
            PostData.MotherName = self.User.MotherName();
            PostData.Phone = self.User.Phone();
            PostData.ReferrerId = self.User.ReferrerId();
            PostData.ReportTo = $("#hdnReportTo").val();
            PostData.FullName = self.User.FullName();
            PostData.Active = self.User.Active();
            PostData.PAN = self.User.PAN();
            PostData.Remarks = self.User.Remarks();
            PostData.Photograph = self.User.Photograph();
            PostData.ImageURL = $('#hdnUserImage').val();
            PostData.CorrespondenceAddr = self.User.CorrespondenceAddr();
            PostData.PermanentAddr = self.User.PermanentAddr();
            PostData.DesignationId = $("#ddlDesignationId option:selected").val();// PostData.ProfileId = 2;
            PostData.TechnologyId = $("#ddlTechnologyId option:selected").val();
            EMPMGMT.Framework.Core.doPostOperation
                   (
                       controllerUrl + "AddUser",
                       PostData,
                       function onSuccess(response) {
                           if (response == "Successful") {

                               EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.User.UserAddedSuccess, false);
                               window.location.href = "/Employee/ManageUser";
                           }
                           else {

                               EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.User.UserAlreadyExist, true);
                           }
                       },
                       function onError(err) {
                           self.status(err.Message);
                       }
                   );
        }
        else {
            self.EditUserErrors.showAllMessages();
        }
    }
    //**********************************END*********************************************//

    //*********************Edit User Detailo *******************************************//

    self.EditUserDetail = function () {
        debugger;
        if (self.EditUserErrors().length == 0) {
            var PostData = new Object();
            PostData.FirstName = self.User.FirstName();
            PostData.LastName = self.User.LastName();
            PostData.EmailId = self.User.EmailId();
            PostData.UserId = $('#hdnUserId').val();
            PostData.ProfileId = $("#profilelstdropdown option:selected").val();
            PostData.OrgUnitId = $("#ddlOrganisationUnit option:selected").val();
            PostData.DateOfJoining = self.User.DateOfJoining();
            PostData.DOB = self.User.DOB();
            PostData.FatherName = self.User.FatherName();
            PostData.MotherName = self.User.MotherName();
            PostData.Phone = self.User.Phone();
            PostData.ReferrerId = self.User.ReferrerId();
            PostData.ReportTo = $("#hdnReportTo").val();
            PostData.FullName = self.User.FullName();
            PostData.Active = self.User.Active();
            PostData.PAN = self.User.PAN();
            PostData.Remarks = self.User.Remarks();
            PostData.Photograph = self.User.Photograph();
            PostData.CorrespondenceAddr = self.User.CorrespondenceAddr();
            PostData.PermanentAddr = self.User.PermanentAddr();
            PostData.DesignationId = $("#ddlDesignationId option:selected").val();// PostData.ProfileId = 2;
            PostData.TechnologyId = $("#ddlTechnologyId option:selected").val();
            
            EMPMGMT.Framework.Core.doPostOperation
                   (
                       controllerUrl + "EditUserDetail",
                       PostData,
                       function onSuccess(response) {
                           EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.User.UserUpdatedSuccess, false);
                           var userid = $('#hdnUserId').val();
                           window.location.href = $(".redirectBack").attr('href');
                       },
                       function onError(err) {
                           self.status(err.Message);
                       }
                   );

        }
        else {
            self.EditUserErrors.showAllMessages();
        }
    }
    //****************************END***************************************************//
    //*******************Fetch Profile List ***************************************//
    self.GetProfileList = function () {
        EMPMGMT.Framework.Core.getJSONData(controllerUrl + "ProfileTypeList",
            function onSuccess(response) {
                debugger;
                self.BindProfileList(response);
            }, function onError(err) {
                self.status(err.Message);
            });
    }
    //**********************************END*********************************************//

    //***********************Fetch Organisation List ************************************//
    self.GetOrganisationList = function () {

        EMPMGMT.Framework.Core.getJSONData(controllerUrl + "OrganizationUnitsList",
            function onSuccess(response) {
                self.BindOrganisationList(response.OrganizationUnits);
            }, function onError(err) {
                self.status(err.Message);
            });
    }
    //**********************************END************************************************//


    //*******************Fetch Technology List ***************************************//
    self.GetTechnologies = function () {

        EMPMGMT.Framework.Core.getJSONData(controllerUrl + "Technologies",
            function onSuccess(response) {
                self.BindTechnologies(response);
            }, function onError(err) {
                self.status(err.Message);
            });
    }
    //**********************************END*********************************************//


    //*******************Fetch Designation List ***************************************//
    self.DesignationList = function () {
        EMPMGMT.Framework.Core.getJSONData(controllerUrl + "DesignationList",
            function onSuccess(response) {
                self.BindDesignationList(response);
            }, function onError(err) {
                self.status(err.Message);
            });
    }
    //**********************************END*********************************************//


    //***********************Bind Organisation  Dropdown List **************************************//
    self.BindOrganisationList = function (data) {

        $.each(data, function (index, element) {

            $('._OrganisationDDL').append("<option value='" + element.OrgUnitId + "'>" + element.OrgUnitName + "</option>");
        });
    }
    //**********************************END*********************************************//

    //***********************Bind Profile Dropdown List **************************************//
    self.BindProfileList = function (data) {
        debugger;
        $.each(data, function (index, element) {
            $('._ProfileDDL').append("<option value='" + element.ProfileId + "'>" + element.ProfileName + "</option>");
        });
    }
    //**********************************END*********************************************//

    //*********************************Bind Technologies***********************//
    self.BindTechnologies = function (data) {
        $.each(data, function (index, element) {
            $('#ddlTechnologyId').append("<option value='" + element.TechnologyId + "'>" + element.TechnologyName + "</option>");

        });
    }

    //**********************************End************************************//

    //*********************************Bind Designation List***********************//
    self.BindDesignationList = function (data) {
        $.each(data, function (index, element) {
            $('#ddlDesignationId').append("<option value='" + element.DesignationId + "'>" + element.DesignationName + "</option>");

        });
    }

    //**********************************End************************************//

    //****************get user detail. *****************************//
    self.GetUserDetail = function (applyBindings) {
        debugger;
        var PostData = new Object();
        PostData.UserId = $('#hdnUserId').val();
        EMPMGMT.Framework.Core.doPostOperation(
            controllerUrl + "GetUserDetail", PostData,
            function onSuccess(response) {
                debugger;
                if (response != undefined) {
                    var d = new Date();
                    var imagePath = "/Uploads/Employee/" + response.userVM.ImageURL + "?" + d.getTime();
                    self.User.FirstName(response.userVM.FirstName);
                    self.User.LastName(response.userVM.LastName);
                    self.User.EmailId(response.userVM.EmailId);
                    self.User.OrganisationUnit(response.userVM.OrgUnitName);
                    self.User.ProfileName(response.userVM.ProfileName);
                    if (response.userVM.DateOfJoining == "" || response.userVM.DateOfJoining == undefined || response.userVM.DateOfJoining == null) {
                        self.User.DateOfJoining('');
                    }
                    else { self.User.DateOfJoining(dateFormat(EMPMGMT.Framework.Common.ParsedJsonDate(response.userVM.DateOfJoining), "mm/dd/yyyy")); }
                    if (response.userVM.DOB == "" || response.userVM.DOB == undefined || response.userVM.DOB == null) {
                        self.User.DOB('');
                    }
                    else  {
                        self.User.DOB(dateFormat(EMPMGMT.Framework.Common.ParsedJsonDate(response.userVM.DOB), "mm/dd/yyyy"));
                    }
                    self.User.FatherName(response.userVM.FatherName);
                    self.User.MotherName(response.userVM.MotherName);
                    self.User.Phone(response.userVM.Phone);
                    self.User.ReferrerName(response.userVM.ReferrerName);
                   // self.User.FullName(response.userVM.ReportTo);
                    self.User.FullName(response.userVM.ReportToFullName);
                    self.User.ReferrerId(response.userVM.ReferrerId);                    
                    self.User.Active(response.userVM.Active);
                    self.User.PAN(response.userVM.PAN);
                    self.User.Remarks(response.userVM.Remarks);
                    self.User.CorrespondenceAddr(response.userVM.CorrespondenceAddr);
                    self.User.PermanentAddr(response.userVM.PermanentAddr);
                    self.User.DesignationName(response.userVM.DesignationName);
                    self.User.TechnologyName(response.userVM.TechnologyName);
                    self.User.EmpCode(response.userVM.EmpCode);
                    self.User.ImageURL(imagePath == null ? "/Uploads/Employee/no_image.gif" : imagePath);
                    logedInUser = response.LoggedInUser;
                    if (logedInUser == response.userVM.UserId) {
                        self.ResetPasswordVisibility(false);
                        self.editUserVisibility(false);
                    }
                    else {
                        if (response.userVM.UserGuid == null) {

                            self.ResetPasswordVisibility(false);
                        }
                        else {
                            self.ResetPasswordVisibility(true);
                        }
                        self.editUserVisibility(true);
                    }
                    window.setTimeout(function () {
                        $("#profilelstdropdown").val(response.userVM.ProfileId);
                        $("#ddlOrganisationUnit").val(response.userVM.OrgUnitId);
                        $("#ddlDesignationId").val(response.userVM.DesignationId);
                        $("#ddlTechnologyId").val(response.userVM.TechnologyId);
                    }, 1000);

                }
            }, function onError(err) {
                self.status(err.Message);
            });
    }
    //*****************End ***************************************************//


    //***********************************************Edit View SCREEN Click **************//
    self.EditViewScreen = function () {
        window.location.href = "/Employee/EditUserView/" + $('#hdnUserId').val();
    }

    //***************************************END******************************************//

    self.EditUserDetailCancel = function () {
        var userid = $('#hdnUserId').val();
        //window.location.href = " /Employee/UserDetails/" + userid;

        history.go(-1);
    }
    self.AddUserDetailCancel = function () {

        window.location.href = "/Employee/ManageUser";
    }


    //***********************************Function Calling **************************//
    self.GetOrganisationList();
    self.GetProfileList();
    self.GetTechnologies();
    self.DesignationList();
    if ($('#hdnUserId').val() != "") {

        self.GetUserDetail();


    }
    //***********************************Popup**************************//

    self.OpenPopupToUploadDocuments = function () {

        userDocumentsPopupHandler.open();

    }
    //}


    //******************************************End**********************************//
    self.EditUserErrors = ko.validation.group([self.User.FirstName, self.User.EmailId, self.User.DateOfJoining, self.User.PAN]); //self.User.FullName , self.User.Phone,
    self.pswerrors = ko.validation.group([self.Password, self.ConfirmPassword]);

    $('#RefferUserName').autocomplete({
        serviceUrl: '/Employee/GetReffer',
        displayProperty: 'ReferrerName',
        onChnage: function () {

        },
        params: {},
        lookup: null,
        addButtonId: null,
        onSelect: function (suggestion) {
            $("#hdnReferrerId").val(suggestion.data.ReferrerId);
            self.User.ReferrerId(suggestion.data.ReferrerId);
            self.User.ReferrerName(suggestion.value);
        }
    });
    //debugger;
    $('#ReportingToEmployee').autocomplete({
        serviceUrl: '/Employee/GetDesignation?UserId=' + $('#hdnUserId').val(),
        displayProperty: 'FullName',
        onChnage: function () {

        },
        params: {},
        lookup: null,
        addButtonId: null,
        onSelect: function (suggestion) {
            //debugger;
            $("#hdnReportTo").val(suggestion.data.UserId);
            self.User.UserId(suggestion.data.UserId);
            self.User.FullName(suggestion.value);
        }
    });



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
           self.Document.push(new EMPMGMT.User.UserDetail.DocumentViewModel(documentData, DocumentsList.ImagePath));
        });
        if (self.Document().length == 0) {
            $("._DocumentsNoRecord").show();
            $('#hor-zebra').hide();
            $('#divpopboxView').hide();
        }
        else {
            $("._DocumentsNoRecord").hide();
            $('#hor-zebra').show();
            $('#divpopboxView').show();
        }

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
        debugger;

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


    //Uplaod Document 



    self.CancelUserDocument = function () {
        window.location.href = "/Employee/ManageUserDocument";
    }


    //End
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
            postData.DocumentPath = data.DataList.DocumentPath;

            self.Document.push(new EMPMGMT.User.UserDetail.DocumentViewModel(postData, data.ImagePath));
        
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




    return self;



}

//********************************Upload file Functionality **********//

$("#hl-start-upload-with-size").on('click', function () {
    if (jqXHRData) {
        var isStartUpload = true;
        var uploadFile = jqXHRData.files[0];

        if (!(/\.(gif|jpg|jpeg|tiff|png)$/i).test(uploadFile.name)) {
            alert('You must select an image file only');
            isStartUpload = false;
        } else if (uploadFile.size > 4000000) { // 4mb
            alert('Please upload a smaller image, max size is 4 MB');
            isStartUpload = false;
        }
        if (isStartUpload) {
            jqXHRData.submit();
        }
    }
    return false;
});
function IsValidImageAndSize(jqXHRData) {

    var isStartUpload = true;
    var uploadFile = jqXHRData.files[0];
    if (!(/\.(gif|jpg|jpeg|tiff|png)$/i).test(uploadFile.name)) {
        alert('You must select an image file only');
        isStartUpload = false;
    } else if (uploadFile.size > 4000000) { // 4mb
        alert('Please upload a smaller image, max size is 4 MB');
        isStartUpload = false;
    }
    return isStartUpload;
}

$('#fileupload').uploadFile({
    autoSubmit: true,
    url: '/Employee/UploadImage',
    allowedTypes: "png,gif,jpg,jpeg,tiff",
    dataType: 'json',

    dynamicFormData: function () {
        var PostData = new Object();
        PostData.UserId_encrypted = $('#hdnUserId').val();
        return PostData;
    },

    add: function (e, data) {
        if (IsValidImageAndSize(data)) {
            var jqXHR = data.submit()
                .success(function (data, textStatus, jqXHR) {
                    if (data.statusCode == 200) {
                       
                    }
                    else {
                        $('#hdnUserImage').val('');
                    }
                })
                .error(function (data, textStatus, errorThrown) {
                    if (typeof (data) != 'undefined' || typeof (textStatus) != 'undefined' || typeof (errorThrown) != 'undefined') {
                        alert(textStatus + errorThrown + data);
                        $('#hdnUserImage').val('');
                    }
                });
        }
    },
    showStatusAfterSuccess: false,
    multiple: false,
    onSuccess: function (files, data, xhr) {

        var d = new Date();
        $("#ProfileImage1").attr("src", data.file + "?" + d.getTime());
        $(".profileimg").attr("src", data.file + "?" + d.getTime());
        $("#imglogout").attr("src", data.file + "?" + d.getTime());
        $("#photograph").html("<span style='font-weight: bold;padding-left:50px; data-bind='value: Photograph''>'" + files + "' file selected</span>");
        $('#hdnUserImage').val(data.imageName);
      //  self.User.ImageURL(data.file)
    },
    onSubmit: function (files) {

    },
    fail: function (event, data) {
        if (data.files[0].error) {
            alert(data.files[0].error);
        }
    },
    afterUploadAll: function () {
        //$('#inactiveScreen').removeClass('');
    }
});




//*******************************End***************************************//



//******************************StartDate*************************//
var startDate = new Date('01/01/1980');
var FromEndDate = new Date();
var ToEndDate = new Date();

ToEndDate.setDate(ToEndDate.getDate() + 365);

$('#JoiningDate').datepicker({

    weekStart: 1,
    startDate: '01/01/2012',
    //endDate: FromEndDate,
    autoclose: true
});
$('#DOB').datepicker({

    weekStart: 1,
    startDate: startDate,
    endDate: ToEndDate,
    autoclose: true
});
//*****************End Date*************************************//



