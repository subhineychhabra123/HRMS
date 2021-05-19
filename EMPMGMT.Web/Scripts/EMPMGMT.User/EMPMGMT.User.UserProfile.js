//Register name space
jQuery.namespace('EMPMGMT.User.UserProfile');
var viewModelUserProfile;
var controllerUrl = "/Employee/";
var Message = {
    Failure: 'Failure',
    Success: 'Success'
}

var jqXHRData;

EMPMGMT.User.UserProfile.pageLoad = function () {
    viewModelUserProfile = new EMPMGMT.User.UserProfile.pageViewModel();
    ko.applyBindings(viewModelUserProfile);
}

//*************************** validation ********************************//
ko.validation.rules.pattern.message = 'Invalid.';
ko.validation.configure({
    registerExtenders: true,
    messagesOnModified: true,
    insertMessages: true,
    parseInputAttributes: true,
    messageTemplate: null
});
//*************************** End ********************************//
//Page view model
EMPMGMT.User.UserProfile.pageViewModel = function () {
    //Class variables
    var self = this;
    self.txtFirstName = ko.observable('').extend({  // custom message
        required: {
            message: 'First name is required'
        }
    });
    self.UserId = ko.observable('');
    self.txtLastName = ko.observable('');
    self.txtEmailId = ko.observable('');
    self.imgImageURL = ko.observable('');
    self.ProfileImage = ko.observable('');
    //*****************************End*********************************************//


    self.CancelEdit = function () {
        window.location.href = "/Employee/ViewUserProfile";
    }
    //****************get the loged-in user detail. *****************************//
    self.GetUserDetail = function (applyBindings) {
      
        EMPMGMT.Framework.Core.getJSONData(
            controllerUrl + "GetUser",
            function onSuccess(response) {                
                if (response != undefined) {
                    var d = new Date();
                    var imagePath = "/Uploads/Employee/" + response.ImageURL + "?" + d.getTime();
                    self.UserId(response.UserId);
                    self.txtFirstName(response.FirstName);
                    self.txtLastName(response.LastName);
                    self.txtEmailId(response.EmailId);
                    self.imgImageURL('');
                    self.imgImageURL(response.ImageURL == null ? "/Uploads/users/no_image.gif" : imagePath);

                }
            }, function onError(err) {
                self.status(err.Message);
            });
    }
    //*****************End ***************************************************//

    self.GetUserDetail();
    //Edit profile click event
    //***************************Edit loged-in user Detail*********************//

    self.EditUserProfile = function (data) {
       // debugger;
        if (self.errors().length == 0) {
            var PostData = new Object();
            PostData.FirstName = data.txtFirstName();
            PostData.LastName = data.txtLastName();
            PostData.EmailId = data.txtEmailId();
            PostData.ImageURL = data.imgImageURL();
            EMPMGMT.Framework.Core.doPostOperation
                   (
                       controllerUrl + "UpdateUserProfile",
                       PostData,
                       function onSuccess(response) {
                           EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.User.UserUpdatedSuccess, false);
                           window.setTimeout(function () {
                              
                               window.location.href = "/Employee/ViewUserProfile";
                           }, 3000);
                       },
                       function onError(err) {
                           self.status(err.Message);
                       }
                   );
        }
        else {
            self.errors.showAllMessages();
        }
    }

    self.errors = ko.validation.group([self.txtFirstName]);

    // $(".ajax-file-upload").html("Upload");
}


//********************************Upload file Functionality ****************************//
initAutoFileUpload();
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
function initAutoFileUpload() {

    'use strict';

    $('#fileupload').uploadFile({
        autoUpload: true,
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

                        }
                    })
                    .error(function (data, textStatus, errorThrown) {
                        if (typeof (data) != 'undefined' || typeof (textStatus) != 'undefined' || typeof (errorThrown) != 'undefined') {
                            alert(textStatus + errorThrown + data);
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
        },
        onSubmit: function (files) {
            //$('.ajax-file-upload-statusbar').hide();
           // $('.ajax-file-upload-statusbar').css('z-index', '1000');
           // $('.wrapper').prepend("<div id='inactiveScreen' style='z-index:999;width:100%;height:100%;background-color: rgba(0, 0, 0, 0.6); position: fixed;'></div>");
            
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
}
//*******************************End*************************************************//


//******************************StartDate*************************//
var startDate = new Date('01/01/1980');
var FromEndDate = new Date();
var ToEndDate = new Date();

ToEndDate.setDate(ToEndDate.getDate() + 365);

$('#StartDate').datepicker({

    weekStart: 1,
    startDate: '01/01/2012',
    //endDate: FromEndDate,
    autoclose: true
});
$('#EndDate').datepicker({

    weekStart: 1,
    startDate: startDate,
    endDate: ToEndDate,
    autoclose: true
});
//*****************End Date*************************************//



