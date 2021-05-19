//Register name space
jQuery.namespace('EMPMGMT.User.uploaduser');

var config = EMPMGMT.Framework.Core.Config;
var viewModelUserDetail;
var controllerUrl = "/Employee/";
var Message = {
    Failure: 'Failure',
    Success: 'Success'
}
EMPMGMT.User.uploaduser.pageLoad = function () {
    viewModelUploadUsers = new EMPMGMT.User.uploaduser.pageViewModel();
    ko.applyBindings(viewModelUploadUsers);
}

EMPMGMT.User.uploaduser.pageViewModel = function () {
    //Class variables
    var self = this;
    var controllerUrl = "/Employee/";
    var Message = {

        Failure: 'Failure',
        Success: 'Success'
    }
    var Userprofileid = $('._proflelstdropdown').val();

    var uploadObj = $("#fileupload").uploadFile({
        url: '/Employee/UploadUsers',
        maxFileCount: 1,
        multiple: false,
        autoSubmit: false,
        fileName: "docs",
        maxFileSize: 1024 * 10000,
        fileCounter: 0,
        onCancel: function (e) { uploadObj.selectedFiles = 0; e.perventDefault(); return true; },
        dynamicFormData: function () {


            var profileId1 = ko.utils.unwrapObservable($('._proflelstdropdown').val())


            var PostData = new Object();
            PostData.Id_encrypted = profileId1;

            return PostData;
        },
        showStatusAfterSuccess: false,
        onSubmit: function (files) {




        },
        onSuccess: function (files, data, xhr) {

            self.SendmailToUplodedusers();


        },
        afterUploadAll: function () {

        },
        onError: function (files, status, errMsg) {

        }
    });

    self.Profiles = ko.observableArray([]);

    self.getProfileTypeList = function () {

        EMPMGMT.Framework.Core.getJSONData(controllerUrl + "ProfileTypeList", function onSuccess(response) {



            self.BindProfileList(response);



        }, function onError(err) {
            self.status(err.Message);
        });
    }


    self.getProfileTypeList();
    self.CurrentProfileData = ko.observable();
    self.SuccessfullyRetrievedModelsFromAjax = function (profileTypeList) {
        ko.utils.arrayForEach(profileTypeList, function (profile) {

        });
        $("._ProfilesNoRecord").hide();
        if (self.Profiles().lenght == 0) {
            $("._ProfilesNoRecord").show();
        }

    };
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

    self.SendmailToUplodedusers = function () {


        EMPMGMT.Framework.Core.getJSONData(
                      controllerUrl + "SendMail_UploadUsers",
                   function onSuccess(response) {

                       EMPMGMT.Framework.Core.ShowMessage_uploaduser(response.Message, false);

                   }, function onError(err) {


                       var msg = "Mail has ben sent to Registered user";
                       EMPMGMT.Framework.Core.ShowMessage(msg, false);
                   });

    }
    self.BindProfileList = function (data) {


        $.each(data, function (index, element) {

            $('._proflelstdropdown').append("<option value='" + element.ProfileId + "'>" + element.ProfileName + "</option>");

        });

    }



    //  $("div.ajax-file-upload").text("Upload");

}







