jQuery.namespace('EMPMGMT.User.ProjectList');
var viewModelProjectList;
var ResourcesViewModel;
var CurrentPage = 1;
var controllerUrl = "/Employee/";

var gblprojectLead = '';
var config = EMPMGMT.Framework.Core.Config;
var Message = {
    Failure: 'Failure',
    Success: 'Success'
}

EMPMGMT.User.ProjectList.pageLoad = function () {

    CurrentPage = 1;
    viewModelProjectList = new EMPMGMT.User.ProjectList.pageViewModel();

    ko.applyBindings(viewModelProjectList);
}
var PagingMethodForRegisteredUsers = "viewModelProjectList.GetProjectList";

EMPMGMT.User.ProjectList.ProjectListviewMode = function (data) {
    debugger;
    var self = this;
    var projectId = '';
    var projectName = '';
    var projectCode = '';
    var projectDescription = '';
    var startDate = '';
    var endDate = '';
    var communicationEmailPassword = '';
    var sourceControlDetail = '';
    var status = '';
    var createdDate = '';
    var createdBy = '';
    var modifiedDate = '';
    var modifiedBy = '';
    var editProject = '';
    var projectLead = '';
    var fullName = '';
    var date = '';
    var goToViewUserDetail = '';
    //var userId = '';
    var deleteProject = '';
    var goToProjectList = '';
    var manageActionList = "";
    var projectLeadUrl = "";
  
    var loginUsserId = '';
    var timeCONSUMED = '';
    if (data != undefined) {
        loginUsserId = data.UserId;
        projectId = data.ProjectId;
        projectName = data.ProjectName;
        projectCode = data.ProjectCode;
        projectLead = data.ProjectLead;
        fullName = data.FullName;
        timeCONSUMED = data.TIMECONSUMED;
        projectDescription = data.ProjectDescription;
        if (data.StartDate == null || data.StartDate == "" || data.StartDate == undefined) {
            startDate = '';
        }
        else { startDate = dateFormat(EMPMGMT.Framework.Common.ParsedJsonDate(data.StartDate), "mm/dd/yyyy"); }
        if (data.EndDate == null || data.EndDate == "" || data.EndDate == undefined) {
            endDate = '';
        }
        else {            
                endDate = dateFormat(EMPMGMT.Framework.Common.ParsedJsonDate(data.EndDate), "mm/dd/yyyy");            
        }
        communicationEmailPassword = data.CommunicationEmailPassword;
        sourceControlDetail = data.SourceControlDetail;
        status = data.Status;
        createdDate = data.CreatedDate;
        createdBy = data.CreatedBy;
        modifiedDate = data.ModifiedDate;
        modifiedBy = data.ModifiedBy;
        goToProjectList = "/Project/GetProjectList/" + data.ProjectId;
        editProject = "/Project/EditProject/" + data.ProjectId;
        goToViewUserDetail = "/Project/ProjectDetail/" + data.ProjectId;
        manageActionList = "/Project/ManageActionList/" + data.ProjectId;
        projectLeadUrl = "/Employee/UserDetails/" + data.ProjectLead;

    }

    self.ProjectId = ko.observable(projectId);
    self.ProjectName = ko.observable(projectName);
    self.ProjectCode = ko.observable(projectCode);
    self.ProjectDescription = ko.observable(projectDescription);
    self.StartDate = ko.observable(startDate);
    self.EndDate = ko.observable(endDate);
    self.CommunicationEmailPassword = ko.observable(communicationEmailPassword);
    self.SourceControlDetail = ko.observable(sourceControlDetail);
    self.Status = ko.observable(status);
    self.CreatedDate = ko.observable(createdDate);
    self.CreatedBy = ko.observable(createdBy);
    self.ProjectLead = ko.observable(projectLead);
    self.FullName = ko.observable(fullName);
    self.ModifiedDate = ko.observable(modifiedDate);
    self.ModifiedBy = ko.observable(modifiedBy);
    self.GoToProjectList = goToProjectList;
    self.EditProject = editProject;
    self.DeleteProject = deleteProject;
    //self.MetricDashboardId = ko.observable(metricDashboardId);
    //  self.GoToViewUserDetail = goToViewUserDetail;
    //debugger;
    self.ProjectDetail = goToViewUserDetail;
    self.ManageActionList = manageActionList;
    self.ProjectLeadUrl = projectLeadUrl;
    self.UserId = ko.observable(loginUsserId);
    self.TIMECONSUMED = ko.observable(timeCONSUMED);
    return self;
}
EMPMGMT.User.ProjectList.tableHeaderViewModel = function (title, columnname, viewModel) {

    var self = this;
    self.ColumnText = ko.observable(title);
    self.ColumnName = ko.observable(columnname);
    self.SortOrder = ko.observable('');
    self.Sort = viewModel.Sort;
}
EMPMGMT.User.ProjectList.ResourcesViewModel = function (data, viewmodel) {
    var self = this;

    var userId = '', fullName = '', removeResourcesAction = '', projectAssigneeUrl = '';

    if (data != undefined) {

        userId = data.UserId;
        fullName = data.FullName;
        projectAssigneeUrl =  "/Employee/UserDetails/" +data.UserId;
    }
    self.RemoveResourcesAction = viewmodel.RemoveResourcesAction;
    self.UserId = ko.observable(userId);
    self.FullName = ko.observable(fullName);
    self.ProjectAssigneeUrl = ko.observable(projectAssigneeUrl);

}

EMPMGMT.User.ProjectList.pageViewModel = function () {
    //Class variables
    var self = this;
    self.User = new EMPMGMT.User.ProjectList.ProjectListviewMode(null);
    //ResourcesViewModel = new EMPMGMT.User.ProjectList.ResourcesViewModel();
    var orderbycolumn = '', orderby = '', frameworkHelper = EMPMGMT.Framework.Core;
    self.ResponsibleTempList = ko.observableArray([]);

    self.IsInitialLoading = ko.observable(true);
    //metric dashboard Header
    self.ResponsibleOnMouseOver = function (data) {

    }
    self.TableHeaders = ko.observableArray([]);
    self.RenderTableHeaders = function () {
        self.TableHeaders.push(new EMPMGMT.User.ProjectList.tableHeaderViewModel("Project Name", "ProjectName", self));
        self.TableHeaders.push(new EMPMGMT.User.ProjectList.tableHeaderViewModel("Project Code", "ProjectCode", self));

        self.TableHeaders.push(new EMPMGMT.User.ProjectList.tableHeaderViewModel("StartDate", "StartDate", self));
        self.TableHeaders.push(new EMPMGMT.User.ProjectList.tableHeaderViewModel("EndDate", "EndDate", self));
        self.TableHeaders.push(new EMPMGMT.User.ProjectList.tableHeaderViewModel("ActualWork (Hour(s)", "ActualWork (Hour(s))", self));
        self.TableHeaders.push(new EMPMGMT.User.ProjectList.tableHeaderViewModel("Responsible", "ProjectLead", self));


        //self.TableHeaders.push(new EMPMGMT.User.ProjectList.tableHeaderViewModel("Action", "Action", self));
    }
    self.Sort = function (col) {
        //debugger;
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
            if (orderby == 'asc') {
                if (orderbycolumn == "ProjectName") {
                    self.ProjectListBind.sort(function (left, right) { return left.ProjectName() == right.ProjectName() ? 0 : (left.ProjectName() < right.ProjectName() ? 1 : -1) })
                }
                else if (orderbycolumn == "ProjectCode") {
                    self.ProjectListBind.sort(function (left, right) { return left.ProjectCode() == right.ProjectCode() ? 0 : (left.ProjectCode() < right.ProjectCode() ? 1 : -1) })
                }
                else if (orderbycolumn == "StartDate") {
                    self.ProjectListBind.sort(function (left, right) { return left.StartDate() == right.StartDate() ? 0 : (left.StartDate() < right.StartDate() ? 1 : -1) })
                }
                else if (orderbycolumn == "EndDate") {
                    self.ProjectListBind.sort(function (left, right) { return left.EndDate() == right.EndDate() ? 0 : (left.EndDate() < right.EndDate() ? 1 : -1) })
                }
                else {
                    self.ProjectListBind.sort(function (left, right) { return left.ProjectLead() == right.ProjectLead() ? 0 : (left.ProjectLead() < right.ProjectLead() ? 1 : -1) })
                }

            }
            else {
                if (orderbycolumn == "ProjectName") {
                    self.ProjectListBind.sort(function (left, right) { return left.ProjectName() == right.ProjectName() ? 0 : (left.ProjectName() > right.ProjectName() ? 1 : -1) })
                }
                else if (orderbycolumn == "ProjectCode") {
                    self.ProjectListBind.sort(function (left, right) { return left.ProjectCode() == right.ProjectCode() ? 0 : (left.ProjectCode() > right.ProjectCode() ? 1 : -1) })
                }
                else if (orderbycolumn == "StartDate") {
                    self.ProjectListBind.sort(function (left, right) { return left.StartDate() == right.StartDate() ? 0 : (left.StartDate() > right.StartDate() ? 1 : -1) })
                }
                else if (orderbycolumn == "EndDate") {
                    self.ProjectListBind.sort(function (left, right) { return left.EndDate() == right.EndDate() ? 0 : (left.EndDate() > right.EndDate() ? 1 : -1) })
                }
                else  {
                    self.ProjectListBind.sort(function (left, right) { return left.ProjectLead() == right.ProjectLead() ? 0 : (left.ProjectLead() > right.ProjectLead() ? 1 : -1) })
                }
            }
//            self.GetProjectList(1);
        }
    }


    self.ProjectListBind = ko.observableArray([]);
    self.SearchText = ko.observable('');

    self.GetProjectList = function (currentPageNo) {
        CurrentPage = currentPageNo;
        var objParam = new Object();
        objParam.CurrentPage = currentPageNo;
        objParam.UserStatus = self.SelectedFilter;
        objParam.SearchText = self.SearchText();//$('._SearchTxt').val();
        objParam.OrderByColumn = orderbycolumn;
        objParam.OrderBy = orderby;
        EMPMGMT.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetProjectList", objParam,
            function onSuccess(response) {
                //currentUser = response.LoggedInUser;

                self.RenderProjectList(response);
                self.IsInitialLoading(false);
                $("._MetricDashboardPager").html(self.GetPaging(response.TotalRecords, currentPageNo, PagingMethodForRegisteredUsers, "ActionListingPager"));
                EMPMGMT.Framework.Common.ApplyPermission();
            },
        function onError(err) {
            EMPMGMT.Framework.Core.ShowMessage(err.Message, true);

        });
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
    //**********************************Reset selection ******************//





    self.GetPaging = function (Rowcount, currentPage, methodName, uniqueMethodName) {
        return EMPMGMT.Framework.Core.GetPagger(Rowcount, currentPage, methodName, uniqueMethodName);

    }
    self.FilteredData = function () {

        self.GetProjectList(1);

    }
    self.RenderProjectList = function (projectList) {

        self.ProjectListBind.removeAll();
        ko.utils.arrayForEach(projectList.DataList, function (getprojectList) {

            self.ProjectListBind.push(new EMPMGMT.User.ProjectList.ProjectListviewMode(getprojectList));

        });

    };

    self.DeleteProjectAction = function (data) {
     
        var ret = confirm(EMPMGMT.Messages.Project.DeleteProject);
        if (ret) {
            var PostData = new Object();
            PostData.ProjectId = data.ProjectId();
            EMPMGMT.Framework.Core.doPostOperation
                    (
                        controllerUrl + "DeleteProject",
                        PostData,
                        function onSuccess(response) {
                            self.GetProjectList(CurrentPage);
                        },
                        function onError(err) {
                            self.status(err.Message);
                        }
                    );
        }
    }


    $('._ResponsibleUserAutoComplete').autocomplete({
        serviceUrl: '/Employee/GetResponsible',
        displayProperty: 'FullName',
        onSelect: function (suggestion) {
            $("#ProjectLead").val(suggestion.data.UserId);
            self.User.ProjectLead(suggestion.data.UserId);

            $("#hdnUserId").val(suggestion.data.ProjectLead);
            self.User.ProjectLead(suggestion.data.ProjectLead);
            self.User.FullName(suggestion.value);
        }
    });
    self.RenderTableHeaders();
    self.GetProjectList(CurrentPage);


    ///Resources Auto Compalete
    function MapOptions() {

        var ArrArr = $.map(self.ResponsibleTempList(), function (n, i) {
            return n.UserId();
        });
        autocompleteObj.options.notToDisplayItems = ArrArr
    }


    autocompleteObj = $('#ResponsibleUserAutoComplete1').autocomplete({
        
        serviceUrl: '/Employee/GetResponsible',
        displayProperty: 'FullName',
        onSearchStart: function () {
            //debugger;
            MapOptions();
        },
        onChnage: function () {
            //debugger;
          
        },
        params: { "UserId": gblprojectLead },
        lookup: null,
        notToDisplayItemsComparisonkey: "UserId",
        onSelect: function (suggestion) {
            //debugger;

            gblprojectLead += suggestion.data.UserId + ",";
            var objUser = new Object();
            objUser.UserId = suggestion.data.UserId;
            $("#hdnResourcesId").val(suggestion.data.UserId);
            objUser.FullName = suggestion.value;
            self.ResponsibleTempList.push(new EMPMGMT.User.ProjectList.ResourcesViewModel(objUser, self));
            autocompleteObj.options.notToDisplayItems.push(suggestion.data.UserId);//
            //gblprojectLead
            if ($('#GblprojectLead').val().trim() != '') {
                $('#GblprojectLead').val($('#GblprojectLead').val() + ',' + suggestion.data.UserId);
            }
            else { $('#GblprojectLead').val(suggestion.data.UserId); }


        },
    });

    self.RemoveResourcesAction = function (data) {

        self.ResponsibleTempList.remove(data);
        //autocom;pleteObj.options.notToDisplayItems.splice($.inArray(data.UserId(), autocompleteObj.options.notToDisplayItems), 1);
        // self.RemoveResourcesAction.push(data);
        var users = '';
        $('#GblprojectLead').val('');
        ko.utils.arrayForEach(self.ResponsibleTempList(), function (projectobject) {

            users = users + projectobject.UserId() + ',';
        });

        $('#GblprojectLead').val(users.slice(0, -1));


        var PostData = new Object();
        PostData.UserId = data.UserId();
        PostData.ProjectId = $('#hdnProjectId').val();

        //EMPMGMT.Framework.Core.doPostOperation
        //        (
        //            controllerUrl + "DeleteResource",
        //            PostData,
        //            function onSuccess(response) {

        //            },
        //            function onError(err) {
        //                self.status(err.Message);
        //            }
        //        );
    }

    if ($('#hdnProjectId').val() != '') {
       

        var result = $('#values').val();
        if (typeof result != 'undefined') {
            var jsonresult = jQuery.parseJSON(result);
            var users = '';

            $.each(jsonresult.List, function (i, val) {
                //  $( "#" + val ).text( "Mine is " + val + "." ); 

                var userobject = new Object();
                userobject.UserId = val.UserId;
                userobject.FullName = val.FullName;
                self.ResponsibleTempList.push(new EMPMGMT.User.ProjectList.ResourcesViewModel(userobject, self));
                users = users + val.UserId + ',';
            });

            $('#GblprojectLead').val(users.slice(0, -1));
        }
    }
    self.ProjectDetail = function (applyBindings) {

        var PostData = new Object();
        EMPMGMT.Framework.Core.doPostOperation(
            controllerUrl + "ProjectDetail", PostData,
            function onSuccess(response) {
               // debugger;
                if (response != undefined) {
                    var d = new Date();

                    self.data.ProjectName(response.projectVM.ProjectName);
                    self.data.ProjectCode(response.projectVM.ProjectCode);
                    self.data.ProjectDescription(response.projectVM.ProjectDescription);

                    self.data.StartDate(dateFormat(EMPMGMT.Framework.Common.ParsedJsonDate(response.projectVM.StartDate), "mm/dd/yyyy"));

                    self.data.EndDate(dateFormat(EMPMGMT.Framework.Common.ParsedJsonDate(response.projectVM.EndDate), "mm/dd/yyyy"));
                    self.data.CommunicationEmailId(response.projectVM.CommunicationEmailId);
                    self.data.CommunicationEmailPassword(response.projectVM.CommunicationEmailPassword);
                    self.data.SourceControlDetail(response.projectVM.SourceControlDetail);
                    self.data.FullName(response.projectVM.FullName);
                    self.data.ProjectLead(response.projectVM.ProjectLead);
                    self.data.Status(response.projectVM.Status);

                }
            }, function onError(err) {
                self.status(err.Message);
            });
    }


    return self;

}


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






