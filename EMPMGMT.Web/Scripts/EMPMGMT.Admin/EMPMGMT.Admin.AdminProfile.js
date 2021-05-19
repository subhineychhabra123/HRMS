//Register name space
jQuery.namespace('EMPMGMT.Admin.AdminProfile');
var viewModelUserProfile;
var controllerUrl = "/Admin/";
var Message = {
    Failure: 'Failure',
    Success: 'Success'
}

var jqXHRData;

EMPMGMT.Admin.AdminProfile.pageLoad = function () {
    viewModelUserProfile = new EMPMGMT.Admin.AdminProfile.pageViewModel();
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
EMPMGMT.Admin.AdminProfile.pageViewModel = function () {
    //Class variables
    var self = this;
    self.txtFirstName = ko.observable('').extend({  // custom message
        required: {
            message: 'First name is required'
        }
    });
    self.txtLastName = ko.observable('');
    self.txtEmailId = ko.observable('');
    self.imgImageURL = ko.observable('');
    self.ProfileImage = ko.observable('');
    //*****************************End*********************************************//


    self.CancelEdit = function () {
        window.location.href = "/Admin/ViewAdminProfile";
    }
    //****************get the loged-in user detail. *****************************//
    self.GetUserDetail = function (applyBindings) {
   
        EMPMGMT.Framework.Core.getJSONData(
            controllerUrl + "GetUser",
            function onSuccess(response) {
                if (response != undefined) {
                    var d = new Date();
                    var imagePath = "/Uploads/users/" + response.ImageURL + "?" + d.getTime();
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
        if (self.errors().length == 0) {
            var PostData = new Object();
            PostData.FirstName = data.txtFirstName();
            PostData.LastName = data.txtLastName();
            PostData.EmailId = data.txtEmailId();
            EMPMGMT.Framework.Core.doPostOperation
                   (
                       controllerUrl + "UpdateUserProfile",
                       PostData,
                       function onSuccess(response) {
                           EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.User.UserUpdatedSuccess, false);
                           window.setTimeout(function () {

                               window.location.href = "/Admin/ViewAdminProfile";
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
        url: '/Admin/UploadImage',
        allowedTypes: "png,gif,jpg,jpeg,tiff",
        dataType: 'json',
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


            $('.ajax-file-upload-statusbar').hide();


        },
        fail: function (event, data) {
            if (data.files[0].error) {
                alert(data.files[0].error);
            }
        },
        afterUploadAll: function () {

        }
    });
}



//*******************************End*************************************************//



