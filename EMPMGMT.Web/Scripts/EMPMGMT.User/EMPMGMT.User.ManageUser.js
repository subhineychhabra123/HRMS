jQuery.namespace('EMPMGMT.User.ManageUser');
var viewModelManageUser;
var InvitedSearch = false;
var registeredUsersCurrentPage = 1;
var currentUser = "";
var controllerUrl = "/Employee/";
var Message = {
    Failure: 'Failure',
    Success: 'Success'
}
var config = EMPMGMT.Framework.Core.Config;
EMPMGMT.User.ManageUser.pageLoad = function () {
    registeredUsersCurrentPage = 1;
    viewModelManageUser = new EMPMGMT.User.ManageUser.pageViewModel();
    ko.applyBindings(viewModelManageUser);
    EMPMGMT.Framework.Common.ApplyPermission();
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

var PagingMethodForRegisteredUsers = "viewModelManageUser.GetRegisteredUsersList";

//*****************************Tab Functionality *******************************//
EMPMGMT.User.ManageUser.filterButtonsViewModel = function (displayText, isActive, filterValue, title) {
    var self = this;
    self.DisplayText = ko.observable(displayText);
    self.IsActive = ko.observable(isActive);
    self.FilterBy = ko.observable(filterValue);
    self.Title = ko.observable(title);


    self.ActiveClass = ko.computed(function () {
        return self.IsActive() == true ? "active" : '';
    });

}



EMPMGMT.User.ManageUser.ComapnyUsersViewModel = function (data, viewModel) {
    debugger;
    var self = this;
    var deleteAction = '';
    var editAction = '';
    var actionType = '';
    var methodName = '';
    // var viewUserDetailAction = '';
    var viewUserDetail = '';
    var editUserDetail = '';
    var SearchText = '';
    var editUserDetailAction = '';
    var userId, firstName = '', lastName = '', emailId = '', isActive = false, profileName = '', status = '', profileid = '', passwordSet = false, resendLinkAction = '', imageUrl = '', deleteEmployee = '', isSuperAdmin, userTypeId, empCode='';
  
    if (data != undefined) {
        userId = data.UserId;
        passwordSet = data.PasswordSet;
        firstName = data.FirstName;
        lastName = data.LastName;
        emailId = data.EmailId;
        status = data.Status;
        profileName = data.ProfileName;
        profileid = data.ProfileId;
        imageUrl = data.ImageURL;

        isSuperAdmin =data.IsSuperAdmin;
        userTypeId=data.UserTypeId;
        empCode = data.EmpCode;

        if (currentUser == userId) {
            actionType = "";
            methodName = "";
            isActive = false;
            //alert(1);
            //$('._editHide').css("display", "none");
            editAction = "Edit";
            deleteAction = "";
            
           // $('._delete').css('display', 'none');
                      
        }
        else {
            //$('._editHide').attr("display", "block");
            editAction = "Edit";
         //   $('._delete').css('display', 'block');
            deleteAction = "Delete";
           

            //if (InvitedSearch) {
            //    actionType = "Resend"
            //    methodName = viewModelManageUser.ResendMail;
            //}
            //else {
            if (data.Status == config.UserStatus.Active) {
                isActive = true;
                actionType = "Deactivate"
                methodName = viewModelManageUser.DeactivateUser;
            }
            else if (data.Status == config.UserStatus.Deactive) {
                actionType = "Activate";
                methodName = viewModelManageUser.ActivateUser;
                isActive = false;
            }


                //else if (data.Status == config.UserStatus.Invited) {
                //    actionType = "Resend"
                //    methodName = viewModelManageUser.ResendMail;

                //}
            else {
                actionType = ""
                isActive = false;
            }
            //}
            if (data.IsSuperAdmin == true)
            {
                deleteAction = "";
                actionType = "";
                editAction = "";
            }
        }
    }

    self.ImageURL = ko.observable(imageUrl);
    self.UserId = ko.observable(userId);
    self.FirstName = ko.observable(firstName);
    self.LastName = ko.observable(lastName);
    self.EmailId = ko.observable(emailId);
    self.Status = ko.observable(status);
    self.ProfileName = ko.observable(profileName);
    self.UpdateStatusHref = 'javascript:void(0)';
    self.ResendLink = ko.observable(!passwordSet);

    self.ResendLinkAction = viewModel.ResendMail;
    // self.ViewUserDetailAction = viewUserDetailAction;
    // self.EditUserDetailAction = editUserDetailAction;
    self.MethodName = methodName;
    self.DeleteEmployee = viewModelManageUser.deleteEmployee;
    self.ViewUserDetail = "/Employee/UserDetails/" + userId;
    self.EditUserDetail = "/Employee/EditUserView/" + userId;// editUserDetail;
    self.ViewProfileDetail = "/Employee/ProfileDetail/" + profileid;
    self.ActionType = ko.observable(actionType);
    self.EditAction = ko.observable(editAction);
    self.DeleteAction = ko.observable(deleteAction);
    self.FullName = ko.computed({
        read: function () {
            var name = '';
            if (self.FirstName() != null && self.LastName() == null) { name = self.FirstName(); }
            else if (self.FirstName() == null && self.LastName() != null) { name = self.LastName(); }
            else if (self.FirstName() == null && self.LastName() == null) { name = ''; }

            else {
                name = self.FirstName() + " " + self.LastName();
            }
            return name;
        }

    });

    self.PhotoPath = ko.computed(function () {
        return (self.ImageURL() == null ? null : "/Uploads/Employee/" + self.ImageURL());
    });
  
    self.EmpCode = ko.observable(empCode);

    EMPMGMT.Framework.Common.ApplyPermission();
    return self;
}

EMPMGMT.User.ManageUser.tableHeaderViewModel = function (title, columnname, viewModel) {
    var self = this;
    self.ColumnText = ko.observable(title);
    self.ColumnName = ko.observable(columnname);
    self.SortOrder = ko.observable('');
    self.Sort = viewModel.Sort;
}

EMPMGMT.User.ManageUser.pageViewModel = function () {
    //Class variables
    var self = this;
    var orderbycolumn = '', orderby = '';
    //****************** Invited User PopUp functionality *************************************//
    $("._inviteUserPopbox").popbox({
        open: '._InviteUserOpen',
        arrow: '.arrow',
        arrow_border: '.arrow-border',
        close: '._InviteUserClose,._resetConfigurationClose',
        overlay: '._popup_overlay'
    });


    //*****************************End*********************************************//
    self.EmailId = ko.observable('').extend({  // custom message
        required: {
            message: 'Email Id is required'

        },

        email: {
            message: 'Please enter valid email address'

        },
    });
    self.TableHeaders = ko.observableArray([]);
    self.SearchText = ko.observable('');
    self.CompanyUsersList = ko.observableArray([]);
    //self.CompanyUser = new EMPMGMT.User.ManageUser.ComapnyUsersViewModel();
    self.RegisteredUsersList = ko.observableArray([]);
    self.GetRegisteredUsersList = function (currentPageNo) {

        registeredUsersCurrentPage = currentPageNo;
        $("._CompanyUsersPager").html('');
        var objParam = new Object();
        objParam.CurrentPage = currentPageNo;
        objParam.UserStatus = self.SelectedFilter;
        objParam.SearchText = self.SearchText();//$('._SearchTxt').val();
        objParam.OrderByColumn = orderbycolumn;
        objParam.OrderBy = orderby;
       
        EMPMGMT.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetCompanyUsers", objParam,
            function onSuccess(response) {
                currentUser = response.LoggedInUser;
                self.RenderCompanyUsers(response);
                $("._CompanyUsersPager").html(self.GetPaging(response.TotalRecords, currentPageNo, PagingMethodForRegisteredUsers, "CompanyUsersPager"));
                EMPMGMT.Framework.Common.ApplyPermission();
            },
        function onError(err) {
            EMPMGMT.Framework.Core.ShowMessage(err.Message, true);

        });
    }
    self.GetPaging = function (Rowcount, currentPage, methodName, uniqueMethodName) {
        return EMPMGMT.Framework.Core.GetPagger(Rowcount, currentPage, methodName, uniqueMethodName);

    }
    //**********************************Tabs Functionality ********************************************//
    debugger;
    self.RegisteredUsersFilterList = ko.observableArray();
    self.RegisteredUsersFilterList.push(new EMPMGMT.User.ManageUser.filterButtonsViewModel("All Employee", true, config.UserStatus.AllUsers, ""));
    self.RegisteredUsersFilterList.push(new EMPMGMT.User.ManageUser.filterButtonsViewModel("Active Employee", false, config.UserStatus.Active, ""));
    self.RegisteredUsersFilterList.push(new EMPMGMT.User.ManageUser.filterButtonsViewModel("Inactive Employee", false, config.UserStatus.Deactive, ""));
    //self.RegisteredUsersFilterList.push(new EMPMGMT.User.ManageUser.filterButtonsViewModel("Pending", false, config.UserStatus.Invited, ""));
    //self.RegisteredUser = new EMPMGMT.User.ManageUser.ComapnyUsersViewModel();
    self.SelectedFilter = EMPMGMT.Framework.Core.Config.UserStatus.AllUsers;
    self.FilterUsers = function (data) {
        
        if (data.FilterBy() == EMPMGMT.Framework.Core.Config.UserStatus.Invited) {
            InvitedSearch = true;


        }
        else {
            InvitedSearch = false;
        }

        ko.utils.arrayForEach(self.RegisteredUsersFilterList(), function (item) {
            item.IsActive(false);
        });

        data.IsActive(true);

        self.SelectedFilter = data.FilterBy();
        self.CompanyUsersList.removeAll();
        self.GetRegisteredUsersList(1);
        EMPMGMT.Framework.Common.ApplyPermission();

    }
    //****************************************************End *****************************************//
    $("input").bind("keydown", function (event) {
        var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
        if (keycode == 13) {
            var search = $('#txtsearch').val();
            self.SearchText = ko.observable(search);
            self.FilteredData();
            return false;
        } else {
            return true;
        }
    });
    self.RenderTableHeaders = function () {
        self.TableHeaders.push(new EMPMGMT.User.ManageUser.tableHeaderViewModel("Image", "Image", self));
        self.TableHeaders.push(new EMPMGMT.User.ManageUser.tableHeaderViewModel("Name", "Name", self));
        self.TableHeaders.push(new EMPMGMT.User.ManageUser.tableHeaderViewModel("Employee Code", "EmpCode", self));
        self.TableHeaders.push(new EMPMGMT.User.ManageUser.tableHeaderViewModel("Email Address", "EmailId", self));
        self.TableHeaders.push(new EMPMGMT.User.ManageUser.tableHeaderViewModel("Profile", "Profile", self));
        //self.TableHeaders.push(new EMPMGMT.User.ManageUser.tableHeaderViewModel("Action", "Action", self));
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
            self.GetRegisteredUsersList(1);
        }
    }
    //self.ViewUserDetailAction = function () {
    //}
    self.EditUserDetailAction = function () {

    }
    self.FilteredData = function () {
        self.GetRegisteredUsersList(1);

    }
    self.RenderCompanyUsers = function (companyUsers) {
        $("._CompanyUsersNoRecord").hide();
       
        self.CompanyUsersList.removeAll();
        if (companyUsers.DataList.length == 0) {
            $("._CompanyUsersNoRecord").show();
        }


        ko.utils.arrayForEach(companyUsers.DataList, function (RegisteredUser) {
            debugger;
            self.CompanyUsersList.push(new EMPMGMT.User.ManageUser.ComapnyUsersViewModel(RegisteredUser, self));

        });




    };
    //*******************Fetch Profile List ***************************************//
    self.GetProfileList = function () {
        EMPMGMT.Framework.Core.getJSONData(controllerUrl + "ProfileTypeList",
            function onSuccess(response) {
                self.BindProfileList(response);
            }, function onError(err) {
                self.status(err.Message);
            });
    }
    //**********************************END*********************************************//
    //***********************Bind Profile Dropdown List **************************************//
    self.BindProfileList = function (data) {
        $.each(data, function (index, element) {
            $('._ProfileDDL').append("<option value='" + element.ProfileId + "'>" + element.ProfileName + "</option>");
        });
    }
    //**********************************END*********************************************//
    //************************Add User Screen ****************************//
    self.AddUserScreen = function (data) {
        window.location.href = "/Employee/AddUserView";
    }
    //************************END****************************************//
    //*******************************Reset Password Functionality ******************//
    self.ResendMail = function (user) {
        var resendmail = confirm("Are you want to Resend the mail?");
        if (resendmail) {
            var objPost = new Object();
            objPost.UserId = JSON.parse(ko.toJSON(user.UserId()));
            objPost.FullName = user.FullName();
            objPost.EmailId = user.EmailId();
            EMPMGMT.Framework.Core.doPostOperation
                    (
                        controllerUrl + "ResendMailToUser",
                        objPost,
                        function onSuccess(response) {
                            if (response == "Successful") {

                                EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.User.ResendMailSucessFully, false);

                            }

                        },
                        function onError(err) {
                            self.status(err.Message);
                        }
                    );
        }

    }
    //********************************************End*******************************************//
    //**********************Invite user Screen ************************************************//
    self.InviteUserScreen = function () {
        //var self = this;
        //  self.EmailId = '';
    }
    //*******************************End *****************************************************//
    self.InviteUserCancel = function () {

        self.EmailId('');
    }
    //**************************************Invite User Functionality ***************************//
    self.InviteUser = function () {

        if (self.InvitedUserError().length == 0) {
            var PostData = new Object();
            //PostData.FirstName = data.User.FirstName();
            //PostData.LastName = data.User.LastName();
            ////PostData.UserNote = data.AddUser.Message();
            PostData.EmailId = self.EmailId();
            PostData.ProfileId = $("#profilelstdropdown option:selected").val();// PostData.ProfileId = 2;

            EMPMGMT.Framework.Core.doPostOperation
                   (
                       controllerUrl + "AddUser",
                       PostData,
                       function onSuccess(response) {
                           if (response == "Successful") {

                               EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.User.UserAddedSuccess, false);

                               self.EmailId('');
                               //  window.location.href = "/Employee/ManageUser";
                               viewModelManageUser.GetRegisteredUsersList(1);

                               //  self.
                               $("._InviteUserClose").click();
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
            self.InvitedUserError.showAllMessages();
        }
    }
    //&***********************************************END **************************************//

    //**********************************Reset selection ******************//
    //self.ResetData = function () {
    //    self.SearchText('');
    //    window.setTimeout(function () {
    //        viewModelManageUser.GetRegisteredUsersList(1);
    //    }, 1000);
    //}
    //***********************Deactivate conformation **********************//
    self.DeactivateUser = function (user) {
        //debugger;
        var deactivate = confirm("Do you want to Deactivate this user?");
        if (deactivate) {
            var objPost = new Object();
            objPost.ActionType = "Active"
            objPost.UserId = JSON.parse(ko.toJSON(user.UserId()));
            self.UpdateUserStatus(objPost, config.UserStatus.Deactive);
        }
    }
    //**************************End*****************************************//
    self.ActivateUser = function (user) {
        var activate = confirm("Do you want to Activate this user?");
        if (activate) {
            var objPost = new Object();
            objPost.ActionType = "Deactive"
            objPost.UserId = JSON.parse(ko.toJSON(user.UserId()));
            self.UpdateUserStatus(objPost, config.UserStatus.Active);
        }
    }
    self.GotoUserDetailPage = function (user) {

        var userid = user.UserId();
        window.location.href = "/Employee/UserDetails/" + userid;
    }
    self.GotoEditDetailPage = function (user) {
        var userid = user.UserId();
        window.location.href = "/Employee/EditUserView/" + userid;
    }
    //************************Update Company User Status *******************//
    self.UpdateUserStatus = function (objUser, userStatus) {
        var PostData = new Object();
        PostData.UserId = objUser.UserId;
        PostData.Status = userStatus;
        EMPMGMT.Framework.Core.doPostOperation
               (
                   controllerUrl + "UpdateUserStatus",
                   PostData,
                   function onSuccess(response) {
                      // debugger;
                       if (response.CompanyId == null) {
                           alert("This User can't Deactivated, because this contains many child Users")
                       }
                       EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.User.UserUpdatedSuccess, false);
                       viewModelManageUser.GetRegisteredUsersList(1);
                   },
                   function onError(err) {
                       self.status(err.Message);
                   }
               );

    }
    self.RenderTableHeaders();
    self.GetProfileList();
    //******************************End*************************************//
    self.GetRegisteredUsersList(registeredUsersCurrentPage);
    self.InvitedUserError = ko.validation.group([self.EmailId]);

    //******************AddDocuments*********************//



    //  Delete Employee
    self.DeleteEmployee = function (data) {
        debugger;
        alert(data);
        var ret = confirm(EMPMGMT.Messages.User.ConfirmUserDeleteAction);// DeleteEmployee
        if (ret) {
            var PostData = new Object();
            PostData.UserId = data.UserId();

            EMPMGMT.Framework.Core.doPostOperation
                    (
                        controllerUrl + "DeleteEmployee",
                        PostData,
                        function onSuccess(response) {
                            debugger;
                            if (response.Message == "Record Deleted Successfully") {
                                EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.User.DeleteEmployee, true);
                            }
                            else {
                                EMPMGMT.Framework.Core.ShowMessage(response.Message, true);// alert(response.Message);
                            }
                           self.GetRegisteredUsersList(1);
                        },
                        function onError(err) {
                            self.status(err.Message);
                        }
                    );

        }
    }
}


