jQuery.namespace('EMPMGMT.User.LeaveDetail');
var controllerUrl = "/Employee/";
var Message = {
    Failure: 'Failure',
    Success: 'Success'
}
var common = EMPMGMT.Framework.Common;
var config = EMPMGMT.Framework.Core.Config;
EMPMGMT.User.LeaveDetail.pageLoad = function () {
    var viewModelLeaveDetail = new EMPMGMT.User.LeaveDetail.pageViewModel();
   
     ko.applyBindings(viewModelLeaveDetail);
}
EMPMGMT.User.LeaveDetail.filterButtonsViewModel = function (displayText, isActive, filterValue, title) {
    var self = this;
    debugger;
    self.DisplayText = ko.observable(displayText);
    self.IsActive = ko.observable(isActive);
    self.FilterBy = ko.observable(filterValue);
    self.Title = ko.observable(title);

    self.ActiveClass = ko.computed(function () {
        return self.IsActive() == true ? "active" : '';
    });
    
}


EMPMGMT.User.LeaveDetail.tableHeaderViewModel = function (title, columnname, viewModel) {
    var self = this;
    self.ColumnText = ko.observable(title);
    self.ColumnName = ko.observable(columnname);
   
}
EMPMGMT.User.LeaveDetail.ComapnyLeaveViewModel = function (data, viewModel) {
    debugger;
    var self = this;
    var currentUser = '';
    var deleteAction = 'DisApproved';
    var editAction = 'Approved';
    
    var EmpId = '', FromDate = '', ToDate = '', Reason = '', UserName = '', Status = '', LeaveId = '', ApprovedStatus='';
    if (data != undefined)
    {
        if( data.ApprovedStatus == null)
        {
        
            ApprovedStatus = 'Pending';
        
        }
        if (data.ApprovedStatus == true) {
            ApprovedStatus = 'Approve';

        }
        if (data.ApprovedStatus == false)
        {
            ApprovedStatus = 'DisApprove';

        }
        
        UserName = data.UserName;
        EmpId = data.EmpId;
        LeaveId = data.LeaveId;
        FromDate = data.FromDate;
        if (FromDate == '' || FromDate == undefined || FromDate == null)
            FromDate = '';
        else
        { FromDate = dateFormat(common.ParsedJsonDate(FromDate), "mm/dd/yyyy") }
       
    
        ToDate = data.ToDate;
        if (ToDate == '' || ToDate == undefined || ToDate == null)
            ToDate = '';
        else
        { ToDate = dateFormat(common.ParsedJsonDate(ToDate), "mm/dd/yyyy") }
        Reason = data.Reason;
    }
    self.ApprovedStatus = ko.observable(ApprovedStatus);
    self.LeaveId = ko.observable(LeaveId);
    self.UserName = ko.observable(UserName);
    self.EmpId = ko.observable(EmpId);
    self.FromDate = ko.observable(FromDate);
    self.ToDate = ko.observable(ToDate);
    self.Reason = ko.observable(Reason);
    self.Status = ko.observable(data.ApprovedStatus);
    self.editAction = ko.observable(editAction);
    self.deleteAction = ko.observable(deleteAction);
    self.GetAllLeavesDetailArray = ko.observableArray([]);

   

    self.Approve = function (data) {


       
        var objParam = new Object();
        objParam.LeaveId = data.LeaveId;
        debugger;

        EMPMGMT.Framework.Core.getJSONDataBySearchParam(controllerUrl + "ApproveLeave", objParam,

           function onSuccess(response) {
               debugger;
            
               if (response != false) {
                   viewModel.GetAllLeavesDetail();
               }
               



           },

               function onError(err) {
                  

               });

    }

    self.DisApproveLeave = function (data) {


       
        var objParam = new Object();
        objParam.LeaveId = data.LeaveId;
        debugger;

        EMPMGMT.Framework.Core.getJSONDataBySearchParam(controllerUrl + "DisApproveLeave", objParam,

           function onSuccess(response) {
             
               
               currentUser = response.LoggedInUser;
               viewModel.GetAllLeavesDetail();

              


           },

               function onError(err) {
                  

               });

    }
 
    return self;
}

EMPMGMT.User.LeaveDetail.pageViewModel = function () {

    var self = this;
    var deleteAction = 'DisApproved';
    var editAction = 'Approved';
    var actionType = '';
    var methodName = '';
    var currentUser = '';
    var viewUserDetail = '';
    var editUserDetail = '';
    self.editAction = ko.observable(editAction);
    self.deleteAction = ko.observable(deleteAction);
    self.GetAllLeavesDetailArray = ko.observableArray([]);
    self.ResponseLeaveDetail = ko.observableArray();
    self.TableHeaders = ko.observableArray([]);
   

    //////////////////GetAllLeaveDeatil//////////////////////////////////////////////

    self.GetAllLeavesDetail = function () {
        debugger;
        var objParam = new Object();
        objParam.UserStatus = self.SelectedFilter;
        debugger;
        EMPMGMT.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetAllLeavesDetail", objParam,

            function onSuccess(response) {
                currentUser = response.LoggedInUser;
                self.RenderCompanyLeaves(response);
                
                EMPMGMT.Framework.Common.ApplyPermission();
            },

                function onError(err) {
                    EMPMGMT.Framework.Core.ShowMessage(err.Message, true);
                    self.RenderCompanyLeaves(result);

                });
    }






   
    self.RenderCompanyLeaves = function (companyUsers) {
        debugger;
        
        $("._CompanyUsersNoRecord").hide();

        self.GetAllLeavesDetailArray.removeAll();
        if (companyUsers.leaveVMList.length == 0) {
            $("._CompanyUsersNoRecord").show();
        }
       

        ko.utils.arrayForEach(companyUsers.leaveVMList, function (leaves) {
           

            self.GetAllLeavesDetailArray.push(new EMPMGMT.User.LeaveDetail.ComapnyLeaveViewModel(leaves, self));
            
        });
    }
    //..................Apply Leave................
    self.ApplyLeave = function (data) {
        
        window.location.href = "/Employee/ApplyLeave";
    }


    // ------------Tab Functionality----------------


    self.RegisteredUsersFilterList = ko.observableArray();
    self.RegisteredUsersFilterList.push(new EMPMGMT.User.LeaveDetail.filterButtonsViewModel("My Leave", true, config.UserStatus.AllUsers, ""));
    self.RegisteredUsersFilterList.push(new EMPMGMT.User.LeaveDetail.filterButtonsViewModel("Leave For Approval", false, config.UserStatus.active, ""));

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
        self.GetAllLeavesDetail();

    }
              
      
        
        self.GetAllLeavesDetail();
      
         

    }

