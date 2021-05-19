jQuery.namespace('STRATEGY.User.ImportUsers');
var importUsersviewModel;
STRATEGY.User.ImportUsers.pageLoad = function () {
    importUsersviewModel = new STRATEGY.User.ImportUsers.pageViewModel();
    ko.applyBindings(importUsersviewModel);


}
STRATEGY.User.ImportUsers.pageViewModel = function () {
    //Class variables
    var self = this;
    var controllerUrl = "/Employee/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    var uploadObj = $("#fileupload").uploadFile({
        url: '/Employee/Importexcel',
        multiple: false,
        autoSubmit: false,
        fileName: "docs",
        maxFileSize: 1024 * 10000,
        dynamicFormData: function () {
            //var data = self.setPostData();
            //return data;
        },
        showStatusAfterSuccess: false,
        onSubmit: function (files) {

        },
        onSuccess: function (files, data, xhr) {

            alert();
        },
        afterUploadAll: function () {

        },
        onError: function (files, status, errMsg) {

        }
    });

    $("#ButtonImportUser").bind("click", function () {
        uploadObj.startUpload();
    });
}