//Register name space
jQuery.namespace('STRATEGY.User.Roles');
var viewModel;
STRATEGY.User.Roles.pageLoad = function () {
    viewModel = new STRATEGY.User.Roles.pageViewModel();
    ko.applyBindings(viewModel);

}
// A view model that represent a Test report query model.
STRATEGY.User.Roles.rolesViewModel = function (data) {
    var self = this;
    self.RoleId = data.RoleId;
    self.ParentRoleId = data.ParentRoleId;
    self.RoleName = data.RoleName;
    self.CompanyId = data.CompanyId;
    //self.Url = "EditProfileType/" + data.ProfileId;
    self.Visible = data.IsDefaultForRegisterdUser == true ? false : true;
}

//Page view model
STRATEGY.User.Roles.pageViewModel = function (emailId) {
    //Class variables
    var self = this;
    var controllerUrl = "/Employee/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }
    // self.actionDataArray = ko.observableArray([]);
    self.profileTypeViewModels = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    self.roles = ko.observable();//= ko.observableArray([]);
    self.designations = ko.observable(false);
    self.designationHierarchy = ko.observable(false);

    self.getRolesList = function (applyBindings) {
        STRATEGY.Framework.Core.getJSONData(
            controllerUrl + "RolesList",
            function onSuccess(response) {
                self.roles = new Object();
                self.roles.items = new Array();

                self.roles.items = self.designationHierarchy = self.convertIntoHirarchicalArray(response);

                self.designations = new self.TreeViewModel(self.roles);
                ko.cleanNode(document.getElementById('parent'));
                ko.applyBindings(self.designations, document.getElementById('parent'));
                $("#parent ul:not(:has(li))").parent().addClass("nochild");
                $("._openclose").click(function () {
                    if ($(this).hasClass('open')) {
                        $(this).removeClass('open');
                    }
                    else {
                        $(this).addClass('open');
                    }
                    self.Toggle(this);
                });
                STRATEGY.Framework.Common.ApplyPermission('._roleselector');
            }, function onError(err) {
                self.status(err.Message);
            });
    }
    self.Toggle = function (element) {
        $(element).parent().find("ul:first").toggle();


    }

    self.TreeViewModel = function (data) {
        var mapping = {
            items: {
                create: function (options) {
                    return new self.TreeViewModel(options.data);
                }
            }
        };

        this.items = ko.observableArray();

        ko.mapping.fromJS(data, mapping, this);
    };

    self.TreeViewModel.prototype = {
        constructor: self.TreeViewModel
        //onChecked: function (checked) {
        //    ko.utils.arrayForEach(this.items(), function (item) {
        //        item.checked(checked);
        //    });
        //}
    };
    self.BindRoleListDropDown = function (roleId, parentRoleId) {
        $('._roleListdropdown').empty();
        $('._roleListdropdown').append("<option value='0'>" + STRATEGY.Messages.DropDowns.SelectOption + "</option>");
        var data = self.designationHierarchy;
        self.AddRolesToDropDown(data, roleId, parentRoleId);
    }

    self.AddRolesToDropDown = function (data, roleId, parentRoleId) {
        $.each(data, function (index, element) {
            if (element.RoleId != roleId && element.ParentRoleId != parentRoleId) {
                $('._roleListdropdown').append("<option value='" + element.RoleId + "'>" + element.RoleName + "</option>");
                if (element.items != undefined && element.items != null && element.items.length > 0) {
                    self.AddRolesToDropDown(element.items, roleId, parentRoleId);
                }
            }
        });
    }

    self.convertIntoHirarchicalArray = function (array) {
        var map = {};
        var main_object = array.filter(function (v) {
            if (v.ParentRoleId == null) return true;
            return false;
        });
        var obj = main_object[0];
        obj.items = [];

        var parent = '-';
        map[parent] = {
            items: []
        };

        obj = self.bindAllChildreen(array, obj);
        map[parent].items.push(obj);
        return map['-'].items;

    }

    self.bindAllChildreen = function (array, obj) {
        var allChild_objects = array.filter(function (v) {
            var returnVal = false;
            if (obj.RoleId == v.ParentRoleId && v.ParentRoleId != null) {
                returnVal = true;
            }
            return returnVal;
        });

        for (var i = 0; i < allChild_objects.length; i++) {
            var objnew = allChild_objects[i];
            objnew.items = [];
            objnew = self.bindAllChildreen(array, objnew);
            obj.items.push(objnew);
        }

        return obj;
    }

    $('._newUnit').click(function () {
        $('._roleHeader').text("New Role");
        $('#hdnRoleId').val(0);
        $('._roleName').val('');
        $('._error').html('');
        $('._roleListdropdown').val(0);
        self.BindRoleListDropDown(0, 0);
    });
    $('._submitbtn').click(function () {
        var actionName = "EditRole";
        var successMessage = STRATEGY.Messages.Role.RoleSaved;
        self.RoleName = $.trim($("._roleName").val());
        if (self.RoleName == '') {

            $("._error").text(STRATEGY.Messages.Role.RoleRequired).removeClass("msgssuccess"); return;
        }
        self.RoleId = $('#hdnRoleId').val();

        var parentRoleId = $('._roleListdropdown').val();

        if (parentRoleId == 0)
        { $("._error").text(STRATEGY.Messages.Role.ReplyToRequired).removeClass("msgssuccess"); return; }
        else { self.ParentRoleId = parentRoleId; }
        if (self.RoleId == 0) {
            actionName = "CreateRole";
            successMessage = STRATEGY.Messages.Role.RoleCreated;
        }
        self.SaveRole(actionName, successMessage);
        unitPopop.methods.close()
    });
    $('._reassignrolebtn').click(function () {
        var PostData = new Object();
        if ($('._rolelstdropdown').val() > "0") {
            self.DeleteRole(self.DeletedRoleId, $('._rolelstdropdown').val());
        }
        else {
            $("._error").text(STRATEGY.Messages.Role.ReplyToRequired);
        }

    });
    self.DeleteRole = function (deletedRoleId, reassignedId) {
        var PostData = new Object();
        PostData.roleId_encrypted = deletedRoleId;
        PostData.reassignedId_encrypted = reassignedId;
        STRATEGY.Framework.Core.doDeleteOperation("DeleteRole", PostData,
            function SuccessCallBack(response) {
                if (response.Status == true) {
                    self.getRolesList();
                    STRATEGY.Framework.Core.ShowMessage(STRATEGY.Messages.Role.RoleDeleted, false);
                    $('._close').click();
                }
                else {
                    STRATEGY.Framework.Core.ShowMessage(STRATEGY.Messages.Role.RoleNotDeleted, true);
                }
            }, function FailureCallback() { })
    }
    self.SaveRole = function (actionName, successMessage) {
        var PostData = new Object();
        PostData.RoleId = self.RoleId;
        PostData.ParentRoleId = self.ParentRoleId;
        PostData.RoleName = self.RoleName;
        STRATEGY.Framework.Core.doPostOperation
                (
                    controllerUrl + actionName,
                    PostData,
                    function onSuccess(response) {
                        if (response.Status == Message.Failure) { STRATEGY.Framework.Core.ShowMessage(STRATEGY.Messages.Role.RoleAlreadyExists, true); }
                        else if (response.Status == Message.Success) {
                            STRATEGY.Framework.Core.ShowMessage(successMessage, false);
                            self.getRolesList();
                            $('._close').click();
                        }
                    },
                    function onError(err) {

                        self.status(err.Message);
                    }
                );
    }
    self.RolePopup = function (data) {
   
        $('._roleHeader').text(STRATEGY.Messages.Role.EditRoleHeader);
        var roleName = ko.utils.unwrapObservable(data.RoleName);
        var RoleId = ko.utils.unwrapObservable(data.RoleId);
        var parentRoleId = ko.utils.unwrapObservable(data.ParentRoleId);
        self.BindRoleListDropDown(RoleId, parentRoleId);
        $('#hdnRoleId').val(RoleId);
        $('._roleName').val(roleName);
        if (parentRoleId != undefined && parentRoleId != null) {
            $('._roleListdropdown').val(parentRoleId);
        }
        else {
            $('._roleListdropdown').val(0);
        }
        $('.error-role').html("");
        unitPopop.methods.open()
    }

    self.RolePopUpActionDelete = function (data) {
        if (confirm(STRATEGY.Messages.Role.ConfirmRoleDeleteAction)) {
            var roleName = ko.utils.unwrapObservable(data.RoleName);
            var roleId = ko.utils.unwrapObservable(data.RoleId);
            var parentRoleId = ko.utils.unwrapObservable(data.ParentRoleId);
            self.DeletedRoleId = roleId;
            if (true) {
                self.BindRoleListDropDownForDeleteOperation(roleId, parentRoleId);
                STRATEGY.Framework.Common.OpenPopup('div.assignRole');
                $('._error').html("");
            }
            else {
                DeleteRole(roleId, 0);
            }
        }
    }
    self.getRolesList(true);
    self.DeletedRoleId = ko.observable();
    self.BindRoleListDropDownForDeleteOperation = function (roleId, parentRoleId) {
        var data = self.designationHierarchy;
        $('._rolelstdropdown').empty();
        $('._rolelstdropdown').append("<option value='0'>" + STRATEGY.Messages.DropDowns.SelectOption + "</option>");
        self.AddRolesToDropDownForReAssigning(data, roleId, parentRoleId);
    }

    self.AddRolesToDropDownForReAssigning = function (data, roleId, parentRoleId) {
        $.each(data, function (index, element) {
            if (element.RoleId != roleId) {
                $('._rolelstdropdown').append("<option value='" + element.RoleId + "'>" + element.RoleName + "</option>");
                if (element.items != undefined && element.items != null && element.items.length > 0) {
                    self.AddRolesToDropDownForReAssigning(element.items, roleId, parentRoleId);
                }
            }
        });
    }
    var unitPopop = $("._addNewUnitPopbox").popbox({
        open: '._newUnit',
        arrow: '.arrow',
        arrow_border: '.arrow-border',
        close: '._addNewUnitClose',
        overlay: '._popup_overlay'
    });

    $("._assignUnitPopbox").popbox({
        open: '._editUnit',
        arrow: '.arrow',
        arrow_border: '.arrow-border',
        close: '._assignUnitClose',
        overlay: '._popup_overlay'
    });
}
function showActionField(v) {
    $('._actions').each(function () { $(this).hide(); })
    var data = ko.dataFor(v);
    if (data.IsDefaultForStaffUser() != true && data.IsDefaultForRegisterdUser() != true) {
        
        $(v).find('._actions').css("display","Inline-block");
    }
}

function hideActionField() {

    $('._actions').each(function () { $(this).hide(); })
}

