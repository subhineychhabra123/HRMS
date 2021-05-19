//Register name space
jQuery.namespace('EMPMGMT.User.Profiles');
var viewModel;
EMPMGMT.User.Profiles.pageLoad = function () {
    profileviewModel = new EMPMGMT.User.Profiles.pageViewModel();
    ko.applyBindings(profileviewModel);
}
// A view model that represent a Test report query model.
EMPMGMT.User.Profiles.profilesViewModel = function (data) {

    ko.validation.rules.pattern.message = 'Invalid.';
    ko.validation.configure({
        registerExtenders: true,
        messagesOnModified: true,
        insertMessages: true,
        parseInputAttributes: true,
        messageTemplate: null
    });
    var self = this;

    self.ProfileId = ko.observable(data.ProfileId);
    self.ProfileName = ko.observable(data.ProfileName);
    self.Description = ko.observable(data.Description);
    self.CompanyId = ko.observable(data.CompanyId);

    self.Edit = "EditProfileType/" + data.ProfileId;
    self.Detail = "ProfileDetail/" + data.ProfileId;
    self.Visible = data.IsDefaultForRegisterdUser == true || data.IsDefaultForStaffUser == true ? false : true;
    self.Delete = "Javascript:void()";

}



ko.validation.rules.pattern.message = 'Invalid.';
ko.validation.configure({
    registerExtenders: true,
    messagesOnModified: true,
    insertMessages: true,
    parseInputAttributes: true,
    messageTemplate: null
});

EMPMGMT.User.Profiles.tableHeaderViewModel = function (title, columnname, viewModel) {
    var self = this;
    self.ColumnText = ko.observable(title);
    self.ColumnName = ko.observable(columnname);
    self.SortOrder = ko.observable('');
    self.Sort = viewModel.Sort;
}

//Page view model
EMPMGMT.User.Profiles.pageViewModel = function (emailId) {
    //Class variables

    var self = this;
    var orderbycolumn = '', orderby = '';

    self.TableHeaders = ko.observableArray([]);
    self.ProfileName = ko.observable('').extend({
        required: {
            message: "Profile Name required"
        }
    })




    self.ProfileNameSave = ko.observable('').extend({
        required: {
            message: "Profile Name required"
        }
    })



    self.DescriptionSave = ko.observable('')
    self.Description = ko.observable('')




    self.ProfileId = ko.observable('');
    var controllerUrl = "/Employee/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }
    self.HasFocusOnProfileName = ko.observable(true);
    self.profileTypeViewModels = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    self.Profiles = ko.observableArray([]);
    self.getProfileTypeList = function () {
        var objParam = new Object();
        objParam.OrderByColumn = orderbycolumn;
        objParam.OrderBy = orderby;
        //hhh
        EMPMGMT.Framework.Core.getJSONDataBySearchParam(controllerUrl + "ProfileTypeList", objParam,
            function onSuccess(response) {
                self.Profiles.removeAll();
                self.SuccessfullyRetrievedModelsFromAjax(response);
            },
        function onError(err) {
            self.status(err.Message);
        });

    }
    self.RenderTableHeaders = function () {

        self.TableHeaders.push(new EMPMGMT.User.Profiles.tableHeaderViewModel("Profile Name", "ProfileName", self));
        self.TableHeaders.push(new EMPMGMT.User.Profiles.tableHeaderViewModel("Description", "Description", self));
        //self.TableHeaders.push(new EMPMGMT.User.Profiles.tableHeaderViewModel("Action", "Action", self));

    }
    self.Sort = function (col) {
        if (col.ColumnName() != "Action") {
            ko.utils.arrayFirst(self.TableHeaders(), function (item) {
                if (item.ColumnName() != col.ColumnName()) {
                    item.SortOrder('');
                }
            });
            if (col.SortOrder() == 'sorting_asc') {
                col.SortOrder('sorting_desc');
            }
            else {
                col.SortOrder('sorting_asc');
            }
            orderbycolumn = col.ColumnName();
            orderby = col.SortOrder() == 'sorting_asc' ? 'asc' : 'desc';
            self.getProfileTypeList();
        }
    }
    self.RenderTableHeaders();
    self.getProfileTypeList();
    self.CurrentProfileData = ko.observable();
    self.SuccessfullyRetrievedModelsFromAjax = function (profileTypeList) {
        ko.utils.arrayForEach(profileTypeList, function (profile) {
            self.Profiles.push(new EMPMGMT.User.Profiles.profilesViewModel(profile));
        });
        EMPMGMT.Framework.Common.ApplyPermission();
        $("._ProfilesNoRecord").hide();
        if (self.Profiles().lenght == 0) {
            $("._ProfilesNoRecord").show();
        }

    };

    self.deletedprofileId = ko.observable();

    self.OpenProfilePopup_Create = function () {
        self.ProfileNameSave('');
        self.DescriptionSave('');
        self.SaveProfileErrors.showAllMessages(false);
        //profilePopBoxCreate.methods.open();
        self.HasFocusOnProfileName(true);
    }
    //***************Call Bind  Function******************//

    self.OpenReassignProfilePopup = function (data) {
        var ask = confirm('Are you sure you want to delete this profile?')

        if (ask == true) {

            $('._proflelstdropdown').focus();
            self.CurrentProfileData = data;
            var deletedprofileId = ko.utils.unwrapObservable(data.ProfileId)
            $('._proflelstdropdown').empty();
            $('._proflelstdropdown').append("<option value='0'>" + EMPMGMT.Messages.DropDowns.SelectOption + "</option>");
            self.BindProfileList(self.Profiles(), deletedprofileId);
            $('#ReAssignProfile').modal('show');
        }
    }


    //***************END******************//


    self.deletedprofileId = ko.observable();


    //***************Call Bind EditProfile******************//
    self.BindEditProfile = function (data) {

        var deletedprofileId = data.ProfileId()
        self.ProfileName(data.ProfileName());

        self.Description(data.Description());

        self.ProfileId(deletedprofileId);      
       // profilePopBoxEdit.methods.open();
        $('._editTitleText').focus();

    }

    //***************END******************//


    self.BindProfileList = function (data, deletedprofileId) {


        $.each(data, function (index, element) {

            if (element.ProfileId() != deletedprofileId) {

                $('._proflelstdropdown').append("<option value='" + element.ProfileId() + "'>" + element.ProfileName() + "</option>");
            }
        });

    }







    //***************Save Profile******************//

    self.SaveProfile = function () {

       

        if (self.SaveProfileErrors().length == 0) {


            var postObjSave = new Object();

            postObjSave.ProfileName = self.ProfileNameSave();
            postObjSave.Description = self.DescriptionSave();

            EMPMGMT.Framework.Core.doPostOperation
                    (
                        controllerUrl + "CreateProfileType",
                        postObjSave,

                        function onSuccess(response) {

                            if (response.Status == "0") {
                                $('.close').click();
                                self.getProfileTypeList();

                                $('._clearProfile').val('');
                               
                                EMPMGMT.Framework.Core.ShowMessage(response.Message, false);
                                self.ProfileName('')
                                self.Description('')
                            }
                            else {

                                EMPMGMT.Framework.Core.ShowMessage(response.Message, true);
                            }
                        },
                        function onError(err) {


                            EMPMGMT.Framework.Core.ShowMessage(response.Message, false);
                        }
                    );


        }
        else {
            self.SaveProfileErrors.showAllMessages();
        }



    }

    //***************END******************//

    //***************Edit Profile******************//
    self._editprofile = function (data) {



        if (self.EditProfileErrors().length == 0) {

            var postObj = new Object();

            postObj.ProfileName = JSON.parse(ko.toJSON(data.ProfileName()));
            postObj.Description = JSON.parse(ko.toJSON(data.Description()));
            postObj.ProfileId = JSON.parse(ko.toJSON(data.ProfileId()));

            EMPMGMT.Framework.Core.doPostOperation
                    (
                        controllerUrl + "Save_EditProfile",
                        postObj,
                        function onSuccess(response) {

                            if (response.Status == "0") {

                                $('.close').click();
                                self.getProfileTypeList();

                                EMPMGMT.Framework.Core.ShowMessage(response.Message, false);
                            }

                            else {

                                EMPMGMT.Framework.Core.ShowMessage(response.Message, true);

                            }

                        },
                        function onError(err) {

                            EMPMGMT.Framework.Core.ShowMessage(response.Message, false);
                        }
                    );

        }
        else {
            self.EditProfileErrors.showAllMessages();
        }




    }

    //***************END******************//



    //***************initialization Reassign******************//

    $('._reassignrolebtn').click(function () {

        var reassignedId = $('._proflelstdropdown').val();
        if (reassignedId != "0") {


            self.DeleteProfile(self.CurrentProfileData, reassignedId);
            self.ProfileName('');
            self.Description('');
            $('.close').click();
        }
        else {

            var msg = "Please select profile first";
            EMPMGMT.Framework.Core.ShowMessage(msg, true);

        }
    });
    self.Cancelprofile = function () {
        self.ProfileName('');
        self.Description('');
        self.ProfileNameSave('');
        self.DescriptionSave('');
        $('.close').click();
        //$('._profileDetailClose_Edit').click();
        //$('._profileDetailClose').click();
        self.SaveProfileErrors.showAllMessages(false);


    };

    //***************END******************//


    //***************Reassign Profile to user******************//

    self.DeleteProfile = function (data, reassignProfileId) {


        var profileId = ko.utils.unwrapObservable(data.ProfileId)
        var PostData = new Object();
        PostData.profileId_encrypted = profileId;
        PostData.reassignProfileId_encrypted = reassignProfileId;
        EMPMGMT.Framework.Core.doPostOperation(controllerUrl + "DeleteProfile", PostData,
                     function SuccessCallBack(response) {
                         if (response.Status == true) {

                             var msg = "Profile deleted successfully";

                             EMPMGMT.Framework.Core.ShowMessage(msg, false);
                             self.Profiles.remove(function (item) { return item.ProfileId == data.ProfileId });
                             $('._profileDetailClose').click();

                         }
                         else {
                             EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Profile.ProfileDeletedFailure, true);
                         }
                     }, function FailureCallback() { })

    }

    //***************END******************//

    // Popup initialization---------------------------------
    var profilePopbox = $("._profilePopbox").popbox({

        arrow: '.arrow',
        arrow_border: '.arrow-border',
        close: '._profileDetailClose',
        overlay: '._popup_overlay'
    });

    var profilePopBoxCreate = $("._profilePopboxCreate").popbox({
        open: '._profileDetailOpen_create',
        arrow: '.arrow',
        arrow_border: '.arrow-border',
        close: '._profileDetailClose_create',
        overlay: '._popup_overlay'
    });

    var profilePopBoxEdit = $("._profilePopboxEdit").popbox({      
        
        close: '._profileDetailClose_Edit'       
    });

    //***************END******************//



    //***************Validation Error******************//

    self.SaveProfileErrors = ko.validation.group([self.ProfileNameSave, self.DescriptionSave]);
    self.EditProfileErrors = ko.validation.group([self.ProfileName, self.Description]);

    //***************END******************//

}