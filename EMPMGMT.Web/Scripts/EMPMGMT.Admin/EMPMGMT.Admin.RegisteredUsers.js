jQuery.namespace('EMPMGMT.Admin.RegisteredUsers');

var viewModelRegisteredUsers;
var UserDetail;
var registeredUsersCurrentPage = 1;
var controllerUrl = "/admin/";
var Message = {
    Failure: 'Failure',
    Success: 'Success'
}
var common = EMPMGMT.Framework.Common;
var config = EMPMGMT.Framework.Core.Config;
var UserFilters = {
    NewUsers: "newusers",
    ActiveUsers: "activeusers",
    DeactiveUsers: "deactiveusers",
    Moreinfo: "Moreinfo"
}

var UserComment = {
    Detail: "detail",
    Comments: "comments",
       
}
var UserDetail = {
    FirstName: "",
    LastName: "",
    Message:""

}
$(function () {
    //EMPMGMT.Framework.Core.SetGlobalDatepicker("PlanDateFrom");
    EMPMGMT.Framework.Core.SetGlobalDatepicker("PlanDateTo");
})
var PagingMethodForRegisteredUsers = "viewModelRegisteredUsers.GetRegisteredUsersList";
EMPMGMT.Admin.RegisteredUsers.pageLoad = function () {
    registeredUsersCurrentPage = 1;
    viewModelRegisteredUsers = new EMPMGMT.Admin.RegisteredUsers.pageViewModel();
    ko.applyBindings(viewModelRegisteredUsers);

}
EMPMGMT.Admin.RegisteredUsers.CommentViewModel = function (data) {
   

    var self = this;
 
    for (member in data) {
        if (data[member] != null) {
            self.Comments = ko.observable(data.Comment);
            var value = data.CommentDate;
            if (String(value).indexOf('/Date(') == 0) {
                value = new Date(parseInt(value.replace(/\/Date\((.*?)\)\//gi, "$1")));
            }
            date = new Date(value).format("ddMMyyyyDate");
            var newdate = new Array();
            newdate = date.toString().split(' ');
            self.CommentDate = ko.observable(newdate[0]);
            self.CommentId = ko.observable(data.CommentId);
            self.CommentBy = ko.observable(data.CommentBy);
            self.CommentTo = ko.observable(data.CommentTo);
            self.CommentByName = ko.observable(data.CommentByName);
        }
}
}

ko.validation.rules.pattern.message = 'Invalid.';
ko.validation.configure({
    registerExtenders: true,
    messagesOnModified: true,
    insertMessages: true,
    parseInputAttributes: true,
    messageTemplate: null
});


EMPMGMT.Admin.RegisteredUsers.filterButtonsViewModel = function (displayText, isActive, filterValue, title) {
    var self = this;
    self.DisplayText = ko.observable(displayText);
    self.IsActive = ko.observable(isActive);
    self.FilterBy = ko.observable(filterValue);
    self.Title = ko.observable(title);
    self.ActiveClass = ko.computed(function () {
        return self.IsActive() == true ? "activefilter" : '';
    });
}
EMPMGMT.Admin.RegisteredUsers.filterButtonsViewModelComment = function (displayText, isActive, filterValue, title) {
    var self = this;
    self.DisplayText = ko.observable(displayText);
    self.IsActive = ko.observable(isActive);
    self.FilterBy = ko.observable(filterValue);
    self.Title = ko.observable(title);
    self.ActiveClass = ko.computed(function () {
        return self.IsActive() == true ? "activefilter" : '';
    });
}


EMPMGMT.Admin.RegisteredUsers.registeredUsersViewModel = function (data,UserStatus) {
    var self = this;
    var actionType = '';
    var methodName = '';
    var userId, firstName = '', lastName = '', emailId = '', isActive = false, date = null, userNote = '',CompanyName='', comments = '', commento = '', commentid = '', accessForKPIModule = false, accessForGoalModule = false, accessIsTrialBased = false, accessPeriodFrom = new Date(), accessPeriodTo = new Date(), showAllActionButtons = true, status = config.UserStatus.Pending;
    var showActionTd = false, showActionTh = false, HasTrue = false, btnvalue = 'Submit', CommentByName = '', SearchText = '', IsResend = false;

    if (data != undefined) {
       
        userId = data.UserId;
        firstName = data.FirstName;
        lastName = data.LastName;
        emailId = data.EmailId;
        CompanyName = data.CompanyName;
        
        if (data.Status == config.UserStatus.Active) {
            isActive = true;
            actionType = "Deactivate"
            methodName = viewModelRegisteredUsers.DeactivateUser;
            showActionTd = true;
            HasTrue = false;
            if (data.IsPassword == false) {
                IsResend = true;
            }
            else {

                IsResend = false;
            }
           
        }
        else if (data.Status == config.UserStatus.Deactive) {
           
            actionType = "Activate"
            methodName = viewModelRegisteredUsers.OpenConfigurationPopUp;
            isActive = false;
            showActionTd = true;
            HasTrue = false;
            IsResend = false;
          //  showActionTh = true;
        }
        else if (data.Status == config.UserStatus.Moreinfo) {

            
            showActionTd = false;
            IsResend = false
           
            //  showActionTh = true;
        }
        else {
           
            actionType = ""
            isActive = false;
            showActionTd = false;
            HasTrue = true;
            IsResend = false
          //  showActionTh = false;
        }
        var value = data.CreatedDate;
        if (String(value).indexOf('/Date(') == 0) {
            value = new Date(parseInt(value.replace(/\/Date\((.*?)\)\//gi, "$1")));
        }
        date= dateFormat(new Date(value), "mm/dd/yyyy hh:mm:ss tt");
        //date = new Date(value).format('M/d/yyyy hh:mm:ss tt');
        //date = new Date(Date(data.CreatedDate)).format("default");
        userNote = data.UserNote;
        comments = "";
        CompanyName = data.CompanyName;
        commento = data.CommentTo;
        accessForGoalModule = data.AccessForGoalModule == null ? false : data.AccessForGoalModule;
        accessForKPIModule = data.AccessForKPIModule == null ? false : data.AccessForKPIModule;
        accessIsTrialBased = data.AccessIsTrialBased;
        accessPeriodFrom = data.AccessPeriodFrom;
        accessPeriodTo = data.AccessPeriodTo;
        status = data.Status;
    }
   
  //  self.ShowActionTh = ko.observable();
    self.ShowActionTd = ko.observable(showActionTd);
    self.UserId = ko.observable(userId);
    self.FirstName = ko.observable(firstName);
    self.btnvalue = ko.observable(btnvalue);
    self.LastName = ko.observable(lastName);
    self.EmailId = ko.observable(emailId);
    self.Active = ko.observable(isActive);
    self.Status = ko.observable(status);
    self.CommentByName = ko.observable(CommentByName);
    self.RegistrationDate = ko.observable(date);
    self.Comments = ko.observable('');
   
    self.HasTrue = ko.observable(HasTrue);
   
    self.CommentTo = ko.observable(commento);
    self.CommentId = ko.observable(commentid);
    self.ShowAllActionButtons = ko.observable(showAllActionButtons);
    self.ShowDeniedButton = ko.observable(true);
    self.View = 'javascript:void(0)';
    self.FullName = ko.computed({
        read: function () {
            var name = '';
            if (self.FirstName() != null && self.LastName() == null) { name = self.FirstName(); }
            else if (self.FirstName() == null && self.LastName() != null) { name = self.LastName(); }
            else {
                name = self.FirstName() + " " + self.LastName();
            }
            return name;

        }
    });
    self.UserNote = ko.observable(userNote);
    self.CompanyName = ko.observable(CompanyName);
    self.AccessForKPIModule = ko.observable(accessForKPIModule);
    self.AccessForGoalModule = ko.observable(accessForGoalModule);
    self.AccessIsTrialBased = ko.observable(accessIsTrialBased);
    self.AccessPeriodFrom = ko.observable(accessPeriodFrom);
    self.AccessPeriodTo = ko.observable(accessPeriodTo);
    self.AccessIsTrialBased.ForEditing = ko.computed({
        read: function () {
            return self.AccessIsTrialBased().toString();
        },
        write: function (newValue) {
            self.AccessIsTrialBased(newValue === "true");
        },
        owner: this
    });
    self.ActionType = ko.observable(actionType);
    self.MethodName = methodName;
    self.ShowAllActionButtons = ko.observable(true);
    self.ShowDeniedButton = ko.observable(true);
    //alert(self.FirstName + ":Firstname");
    if (UserStatus == config.UserStatus.Expired) {
        self.IsResend = ko.observable(false);
        self.IsActiveDeactive = ko.observable(false);
        self.IsEditLink = ko.observable(true);
    }
    else
    if (status == config.UserStatus.Active) {
        self.IsResend = ko.observable(IsResend);
        self.IsActiveDeactive = ko.observable(true);
        self.IsEditLink = ko.observable(true);
    }
    else {
        self.IsResend = ko.observable(IsResend);
        self.IsActiveDeactive = ko.observable(true);
        self.IsEditLink = ko.observable(false);
    }
    //alert(self.IsResend + ":IsResend");


    return self;
}

EMPMGMT.Admin.RegisteredUsers.tableHeaderViewModel = function (title, columnname, visibility, viewModel) {
    var self = this;
    self.ColumnText = ko.observable(title);
    self.ColumnName = ko.observable(columnname);
    self.SortOrder = ko.observable('');
    self.Sort = viewModel.Sort;
    self.Visibility = ko.observable(visibility);
    EMPMGMT.Framework.Common.ApplyPermission();
}


EMPMGMT.Admin.RegisteredUsers.pageViewModel = function () {
   
    var self = this;
    var orderbycolumn = '', orderby = '';
    HasTrue = false;
    btnvalue = 'Submit';
    CommentByName = '';
    self.TableHeaders = ko.observableArray([]);
    self.SearchText = ko.observable('');
    self.RegisteredUsercomments = new EMPMGMT.Admin.RegisteredUsers.CommentViewModel();
    self.RegisteredUser = new EMPMGMT.Admin.RegisteredUsers.registeredUsersViewModel();
    self.btnvalue = ko.observable(btnvalue);
    self.ShowActionTh = ko.observable(false);
    self.CompanyName = ko.observable('');

    self.RegisteredUser.AccessPeriodFrom = ko.observable('').extend({
        required: {
            message: "Form date is  required"
        }
    })

    self.RegisteredUser.AccessPeriodTo = ko.observable('').extend({

        required: {


            message: "To date is reuired"

        }

    })


    self.ResendMail = function (user) {
        var resendmail = confirm("Are you want to Resend the mail?");
        if (resendmail) {
            var objPost = new Object();
            objPost.UserId = JSON.parse(ko.toJSON(user.UserId()));
            objPost.FullName = user.FullName();
            objPost.EmailId = user.EmailId();
            objPost.FirstName = user.FirstName();
            objPost.LastName = user.LastName();
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
   
    self.ClickedOnActiveLink = ko.observable(false);
    self.Core = EMPMGMT.Framework.Core;
    self.RegisteredUsersList = ko.observableArray([]);
    self.RegisteredUsersCommentList = ko.observableArray([]);
    
    self.UsersCommentList = ko.observableArray();
    self.UsersCommentList.push(new EMPMGMT.Admin.RegisteredUsers.filterButtonsViewModelComment("Detail", true, config.UserStatus.Pending, ""));
    self.UsersCommentList.push(new EMPMGMT.Admin.RegisteredUsers.filterButtonsViewModelComment("Comments", false, config.UserStatus.Active, ""));
  
   


    self.RegisteredUsersFilterList = ko.observableArray();
    self.RegisteredUsersFilterList.push(new EMPMGMT.Admin.RegisteredUsers.filterButtonsViewModel("New Users", true, config.UserStatus.Pending,""));
    self.RegisteredUsersFilterList.push(new EMPMGMT.Admin.RegisteredUsers.filterButtonsViewModel("Active Users", false, config.UserStatus.Active,""));
    self.RegisteredUsersFilterList.push(new EMPMGMT.Admin.RegisteredUsers.filterButtonsViewModel("Inactive Users", false, config.UserStatus.Deactive,""));
    self.RegisteredUsersFilterList.push(new EMPMGMT.Admin.RegisteredUsers.filterButtonsViewModel("On Hold   (?)", false, config.UserStatus.Moreinfo, " List of users requested to provide more information."));
    self.RegisteredUsersFilterList.push(new EMPMGMT.Admin.RegisteredUsers.filterButtonsViewModel("replevy Users", false, config.UserStatus.Expired, ""));

    self.SelectedFilter = EMPMGMT.Framework.Core.Config.UserStatus.Pending;
    self.GetRegisteredUsersList = function (currentPageNo) {
        var objParam = new Object();
        objParam.CurrentPage = currentPageNo;
        objParam.UserStatus = self.SelectedFilter;
        objParam.SearchText = self.SearchText;
        objParam.OrderByColumn = orderbycolumn;
        objParam.OrderBy = orderby;
        
        if (self.SelectedFilter !=3 && self.SelectedFilter != 4) {

            //self.ShowActionTh(false);
            self.TableHeaders()[3].Visibility(true);
        }
        else {
            self.TableHeaders()[3].Visibility(false);
            //self.ShowActionTh(true);
        }
        EMPMGMT.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetUsers", objParam, function onSuccess(response) {
            self.RenderRegisteredUsers(response, objParam.UserStatus);
            $("._RegisteredUsersPager").html(self.GetPaging(response.TotalRecords, currentPageNo, PagingMethodForRegisteredUsers, "RegisteredUsersPager"));
        }, function onError(err) {
            EMPMGMT.Framework.Core.ShowMessage(err.Message, true);
        });
    }

    self.GetPaging = function (Rowcount, currentPage, methodName, uniqueMethodName) {
        return EMPMGMT.Framework.Core.GetPagger(Rowcount, currentPage, methodName, uniqueMethodName);

    }

    self.RenderTableHeaders = function () {
        self.TableHeaders.push(new EMPMGMT.Admin.RegisteredUsers.tableHeaderViewModel("Name", "Name", true, self));
        self.TableHeaders.push(new EMPMGMT.Admin.RegisteredUsers.tableHeaderViewModel("Email Id", "EmailId", true, self));
        self.TableHeaders.push(new EMPMGMT.Admin.RegisteredUsers.tableHeaderViewModel("Registration Date", "RegistrationDate", true, self));
        self.TableHeaders.push(new EMPMGMT.Admin.RegisteredUsers.tableHeaderViewModel("Action", "Action", false, self));
    }

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

    self.FilteredData = function () {

        self.GetRegisteredUsersList(1);

    }

    self.Sort = function (col) {
        if (col.ColumnName() != "Action") {
            ko.utils.arrayFirst(self.TableHeaders(), function (item) {
                if (item.ColumnName() != col.ColumnName()) {
                    item.SortOrder('');
                }
            });
            if (col.SortOrder() == 'asc') {
                col.SortOrder('desc');
            }
            else {
                col.SortOrder('asc');
            }
            orderbycolumn = col.ColumnName();
            orderby = col.SortOrder();
            self.GetRegisteredUsersList(1);
        }
    }
    self.RenderTableHeaders();
    self.RenderRegisteredUsers = function (registeredUsers,UserStatus) {
        
        $("._RegisteredUsersNoRecord").hide();
        self.RegisteredUsersList.removeAll();
        if (registeredUsers.DataList.length == 0) {
            //$("._RegisteredUsersNoRecord").html("<b>No record found.</b>");
            $("._RegisteredUsersNoRecord").show();
        }

        ko.utils.arrayForEach(registeredUsers.DataList, function (registeredUser) {

          
            self.RegisteredUsersList.push(new EMPMGMT.Admin.RegisteredUsers.registeredUsersViewModel(registeredUser, UserStatus));


        });
        
      
    };
    self.ViewRegisteredUserDetail = function (userDetail) {
        self.RegisteredUser.HasTrue = ko.observable('');
        self.RegisteredUser.UserId(userDetail.UserId);
        self.RegisteredUser.FirstName(userDetail.FirstName());
        self.RegisteredUser.LastName(userDetail.LastName());
        self.RegisteredUser.EmailId(userDetail.EmailId());
        self.RegisteredUser.Active(userDetail.Active());
        self.RegisteredUser.UserNote(userDetail.UserNote());
        self.RegisteredUser.CompanyName(userDetail.CompanyName());
        self.RegisteredUser.Comments(userDetail.Comments());
        self.RegisteredUser.RegistrationDate(userDetail.RegistrationDate());
        self.RegisteredUser.HasTrue = ko.observable(true);
        self.RegisteredUser.AccessForKPIModule(userDetail.AccessForKPIModule());
        self.RegisteredUser.AccessForGoalModule(userDetail.AccessForGoalModule())
        self.RegisteredUser.AccessIsTrialBased(userDetail.AccessIsTrialBased());
        var value = new Date();
        date = value;
        if (userDetail.AccessPeriodFrom() == null) {
            self.RegisteredUser.AccessPeriodFrom(date);
        }

        else {
            self.RegisteredUser.AccessPeriodFrom(userDetail.AccessPeriodFrom());
            
        }


        if (userDetail.AccessPeriodTo() == null) {

            self.RegisteredUser.AccessPeriodTo(date);

        }

        else {
            self.RegisteredUser.AccessPeriodTo(userDetail.AccessPeriodTo());

        }

       

       
       
        if (userDetail.Status() == "3" || userDetail.Status() == "2" || userDetail.Status() == "4") {

            $("#dvadmincomment").show();
            //self.RegisteredUser.HasTrue = ko.observable('true');

        }
        else {

            $("#dvadmincomment").hide();
            //self.RegisteredUser.HasTrue = ko.observable('false');
           
        }
     
        if (userDetail.Status() == config.UserStatus.Active || userDetail.Status() == config.UserStatus.Deactive) {


            self.RegisteredUser.ShowAllActionButtons(userDetail.Active() == false);
            self.RegisteredUser.ShowDeniedButton(userDetail.Active());


        }
        else {
            self.RegisteredUser.ShowAllActionButtons(true);
            self.RegisteredUser.ShowDeniedButton(true);
        }
        self.RegisteredUser.AccessIsTrialBased.ForEditing = ko.computed({
            read: function () {
                return self.RegisteredUser.AccessIsTrialBased().toString();
            },
            write: function (newValue) {
                self.RegisteredUser.AccessIsTrialBased(newValue === "true");
            },
            owner: this
        });

        
        self.GetCommentsData(userDetail.UserId());
        btnvalue = "Submit";
        self.RegisteredUser.btnvalue(btnvalue);
        self.RegisteredUser.Status = ko.observable(userDetail.Status());

    }



    self.GetCommentsData = function (UserId) {
        
        var postObj = new Object();

        postObj.CommentTo = UserId;
        self.RegisteredUser.CommentTo(UserId);

        EMPMGMT.Framework.Core.doPostOperation
                (
                    controllerUrl + "GetComments", postObj,


                    function onSuccess(response) {
                       
                        self.RenderCommentList(response);


                    },
                    function onError(err) {


                        EMPMGMT.Framework.Core.ShowMessage(response.Message, false);
                    }
                );

    }
    self.OpenConfigurationPopUp = function (user) {
       
        self.ClickedOnActiveLink(true);
        self.ViewRegisteredUserDetail(user);
        $("._userActivation").click();
    }
    self.DeactivateUser = function (user) {
        var deactivate = confirm("Are you want to deactivate this user?");
        if (deactivate) {
            var objPost = new Object();
            objPost.ActionType = "Denied"
            objPost.UserId = JSON.parse(ko.toJSON(user.UserId()));
            objPost.Comments = JSON.parse(ko.toJSON(user.Comments()));
            self.RegisteredUserAccountAction(objPost);
            self.UpdateRegisteredUserInList(objPost, false);
        }
    }

   

    self.ActiveRegisteredUser = function (data) {
        if (self.ActiveErrors().length == 0) { 
            if (dateFormat(data.RegisteredUser.AccessPeriodFrom(), "dd/mm/yyyy") > dateFormat(data.RegisteredUser.AccessPeriodTo(), "dd/mm/yyyy")) {
                var msg = "From date can not greater then to date";
                EMPMGMT.Framework.Core.ShowMessage(msg, true);
            }
            //if (JSON.parse(ko.toJSON(data.RegisteredUser.AccessPeriodFrom())) > JSON.parse(ko.toJSON(data.RegisteredUser.AccessPeriodTo()))) {
            //    var msg = "From date can not greater then to date";
            //    EMPMGMT.Framework.Core.ShowMessage(msg, true);
            //}

            else {
                //if (data.RegisteredUser.AccessForGoalModule() == false && data.RegisteredUser.AccessIsTrialBased() == false) {
                if (data.RegisteredUser.AccessForGoalModule() == false && data.RegisteredUser.AccessForKPIModule() == false) {
                    //var msg = "You have select atleast one from Goal Management and KPI Management";
                    var msg = "Grant Access to atleast one module";
                    EMPMGMT.Framework.Core.ShowMessage(msg, true);
                }
                else {

                    var objPost = new Object();
                    objPost.ActionType = "Activate"
                    objPost.UserId = JSON.parse(ko.toJSON(data.RegisteredUser.UserId()));
                    objPost.Comments = JSON.parse(ko.toJSON(data.RegisteredUser.Comments()));
                    objPost.AccessForGoalModule = JSON.parse(ko.toJSON(data.RegisteredUser.AccessForGoalModule()));
                    objPost.AccessForKPIModule = JSON.parse(ko.toJSON(data.RegisteredUser.AccessForKPIModule()));
                    objPost.AccessIsTrialBased = JSON.parse(ko.toJSON(data.RegisteredUser.AccessIsTrialBased()));
                    objPost.AccessPeriodFrom = JSON.parse(ko.toJSON(data.RegisteredUser.AccessPeriodFrom()));
                    objPost.AccessPeriodTo = JSON.parse(ko.toJSON(data.RegisteredUser.AccessPeriodTo()));
                    self.RegisteredUserAccountAction(objPost);
                    self.CloseUserDetailPopup();
                    self.UpdateRegisteredUserInList(objPost, false);
                    if (self.ClickedOnActiveLink() == true)
                    {
                        self.UpdateRegisteredUserInList(objPost, false);
                        self.ClickedOnActiveLink(false);
                    }
                    else {
                        self.UpdateRegisteredUserInList(objPost, true);
                    }
                }
               
            }

        }
        else {

            self.ActiveErrors.showAllMessages();
            
        }
    }
   
    
    self.DeActivateRegisterdUser = function (data) {

        var deactivate = confirm("Are you want to Denied this user?");
        if (deactivate) {
            var objPost = new Object();
            objPost.ActionType = "Denied"
            objPost.UserId = JSON.parse(ko.toJSON(data.RegisteredUser.UserId()));
            objPost.Comments = JSON.parse(ko.toJSON(data.RegisteredUser.Comments()));
            self.RegisteredUserAccountAction(objPost);
            self.CloseUserDetailPopup();
            self.UpdateRegisteredUserInList(objPost, false);
        }
    }
    self.RequestForMoreInfoAboutRegisteredUser = function (data) {
        var deactivate = confirm("Are you want to more information for this user?");
        if (deactivate) {
            var objPost = new Object();
            objPost.ActionType = "MoreInfo"
            objPost.UserId = JSON.parse(ko.toJSON(data.RegisteredUser.UserId()));
            objPost.Comments = JSON.parse(ko.toJSON(data.RegisteredUser.Comments()));
            self.RegisteredUserAccountAction(objPost);
            self.CloseUserDetailPopup();
            if (self.RegisteredUser.Status() != 4) {
                self.UpdateRegisteredUserInList(objPost, false);
            }
            else {


                EMPMGMT.Framework.Core.ShowMessage('Request for more information about registered user is sent successfully?User is now in  on hold list', false);
            }

        }
    }
    self.CloseUserDetailPopup = function () {
        $("#UserDetailBoxClose").click();
        $("._userConfigurationClose").click();
    }
    self.RegisteredUserAccountAction = function (requestParam) {
        EMPMGMT.Framework.Core.doPostOperation(controllerUrl + "RegisteredUserAccountAction", requestParam, function onSuccess(response) {
            EMPMGMT.Framework.Core.ShowMessage(response.Message, false);
        }, function onError(err) {
            EMPMGMT.Framework.Core.ShowMessage(err.Message, true);
        });
    }
    self.UpdateRegisteredUserInList = function (objUser, active) {

        var registeredUser = ko.utils.arrayFirst(self.RegisteredUsersList(), function (item) {
            return objUser.UserId == item.UserId();
        });
        if (active == true) {
            registeredUser.AccessForGoalModule(objUser.AccessForGoalModule);
            registeredUser.AccessForKPIModule(objUser.AccessForKPIModule);
            registeredUser.AccessIsTrialBased(objUser.AccessIsTrialBased);
            registeredUser.AccessPeriodFrom(new Date(objUser.AccessPeriodFrom));
            registeredUser.AccessPeriodTo(new Date(objUser.AccessPeriodTo));
            registeredUser.Comments(objUser.Comments);

        } else {
            if (self.SelectedFilter != config.UserStatus.Expired)
                self.RegisteredUsersList.remove(registeredUser);
            else
                self.GetRegisteredUsersList(1);

            if (self.RegisteredUsersList().length == 0) {
                //$("._RegisteredUsersNoRecord").html("<b>No record found.</b>");
                $("._RegisteredUsersNoRecord").show();
            }
        }
    }




    //***Delete comment***//


    self.DeleteComment = function (data) {

        var deletecomment = confirm("Are you want to delete this comment?");

        if (deletecomment) {


          
            var objPost = new Object();
         
            objPost.CommentId = data.CommentId();
            self.RegisteredUser.CommentTo();
            EMPMGMT.Framework.Core.doPostOperation
               (
                   controllerUrl + "DeleteComment",
                   objPost,

                   function onSuccess(response) {

                       var msg = "Comment deleted  sucessfiullly";

                       EMPMGMT.Framework.Core.ShowMessage(msg, false);
                       self.GetCommentsData(self.RegisteredUser.CommentTo());
                       btnvalue = "Submit";
                       self.RegisteredUser.btnvalue(btnvalue);

                   },
                   function onError(err) {
                       EMPMGMT.Framework.Core.ShowMessage(response.Message, false);
                   }
               );

        }

    }




    //***End***//


    //******Save Comment//*****//


    self.CancelComment = function () {

        self.RegisteredUser.CommentId("")
        self.RegisteredUser.Comments("");
        btnvalue = "Submit";
        self.RegisteredUser.btnvalue(btnvalue);

    }
    self.SaveComment = function () {
      
        if (self.RegisteredUser.Comments() == '') {
            EMPMGMT.Framework.Core.ShowMessage('Comment box can not be blank', false);
        }
        else {

            var userid = "", commentid = "";
            userid = self.RegisteredUser.CommentTo();
            var postObj = new Object();
            if (self.RegisteredUser.CommentId() == null || self.RegisteredUser.CommentId().toString() == "") {

                commentid = 0

            }
            else {

                commentid = self.RegisteredUser.CommentId();
            }
            postObj.Comment = self.RegisteredUser.Comments();

            postObj.CommentTo = userid;
            postObj.CommentId = commentid;

            EMPMGMT.Framework.Core.doPostOperation
                (
                    controllerUrl + "SaveComments",
                    postObj,
                    function onSuccess(response) {
                        var msg = "Comments saved  sucessfiullly";
                        EMPMGMT.Framework.Core.ShowMessage(msg, false);
                        self.RegisteredUser.CommentId("")
                        self.GetCommentsData(userid);
                        self.RegisteredUser.Comments("");
                        btnvalue = "Submit";
                        self.RegisteredUser.btnvalue(btnvalue);
                        EMPMGMT.Framework.Core.ShowMessage(response.Message, false);
                    },
                    function onError(err) {
                        EMPMGMT.Framework.Core.ShowMessage(response.Message, false);
                    }
                );

        }
    }


    ////********End******/////



    ////*****Bind Edit comment*****///

    self.BindEditComment = function (data) {
       
        self.RegisteredUser.Comments(data.Comments());
        self.RegisteredUser.CommentTo(data.CommentTo());
        self.RegisteredUser.CommentId(data.CommentId());
        btnvalue = "Update";
        self.RegisteredUser.btnvalue(btnvalue);
    }

    ////****End***////

    //Bind Comment

    



    self.RenderCommentList = function (CommentListData) {
        self.RegisteredUsersCommentList.removeAll();
        if (CommentListData.commentdata.length == 0) {

            $("._RegisteredUsersCommentNoRecord").show();
            $("._RegisteredUsersCommentNoRecord").html("<b>No Comment found.</b>");

        }
        else {
            $("._RegisteredUsersCommentNoRecord").hide();
            $("._RegisteredUsersCommentNoRecord").html("");

        }
        ko.utils.arrayForEach(CommentListData.commentdata, function (Comments) {
           
            self.RegisteredUsersCommentList.push(new EMPMGMT.Admin.RegisteredUsers.CommentViewModel(Comments));

        });
    };

   

    self.FilterUsers = function (data) {
        ko.utils.arrayForEach(self.RegisteredUsersFilterList(), function (item) {
            item.IsActive(false);
        });

        data.IsActive(true);
        
        self.SelectedFilter = data.FilterBy();
        self.RegisteredUsersList.removeAll();
        self.GetRegisteredUsersList(1);
    }

    self.UserComment = function (data) {
        ko.utils.arrayForEach(self.UsersCommentList(), function (item) {
            item.IsActive(false);
        });

        data.IsActive(true);
        self.SelectedFilter = data.FilterBy();
       
      
        if (self.SelectedFilter == "1") {
            $("#userdiv").hide();
            $("#usercomment").show();
            self.RegisteredUser.Comments("");
            btnvalue = "Submit";
            self.RegisteredUser.btnvalue(btnvalue);
           
        }
        else {
            
            $("#userdiv").show();
            $("#usercomment").hide();
            
            self.RegisteredUser.Comments("");
            btnvalue = "Submit";
            self.RegisteredUser.btnvalue(btnvalue);
        }
        
    }

    self.GetRegisteredUsersList(registeredUsersCurrentPage);
    $("._configurationpopbox").popbox({
        open: '._userActivation',
        arrow: '.arrow',
        arrow_border: '.arrow-border',
        close: '._userConfigurationClose',
        overlay: '._configurationpopup_overlay',
        zindex: '800'
    });

    $("._userDetailPopbox").popbox({
        open: '._userDetailOpen',
        arrow: '.arrow',
        arrow_border: '.arrow-border',
        close: '._userDetailClose',
        overlay: '._popup_overlay',
        zindex:'700'
    });


    //Comment poppup


    //$("._configurationpopboxComment").popbox({
    //    open: '._AdminComments',
    //    arrow: '.arrow',
    //    arrow_border: '.arrow-border',
    //    close: '._userConfigurationCloseComment',
    //    overlay: '._configurationpopup_overlayComment'
    //});

    self.ActiveErrors = ko.validation.group([self.RegisteredUser.AccessPeriodFrom, self.RegisteredUser.AccessPeriodTo]);
    //self.CommentError = ko.validation.group([self.RegisteredUser.AccessPeriodFrom, self.RegisteredUser.AccessPeriodTo]);
    return self;
}