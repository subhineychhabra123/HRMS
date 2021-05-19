jQuery.namespace('EMPMGMT.User.ManageActionList');
var controllerUrl = "/Employee/";
var viewModelActionList;
var CurrentPage = 1;
var Message = {
    Failure: 'Failure',
    Success: 'Success'
}

var StatusCode = {
    Red: 1,
    Yellow: 2,
    Green: 3,
}
EMPMGMT.User.ManageActionList.pageLoad = function () {
    CurrentPage = 1;
    viewModelActionList= new EMPMGMT.User.ManageActionList.pageViewModel();
    ko.applyBindings(viewModelActionList, document.getElementById("ManageActionItem"));
}
var PagingMethodForActionList = "viewModelActionList.GetActionList";



EMPMGMT.User.ManageActionList.ActionItemViewModel = function (data) {
    var self = this;
    var dueDate = '', responsibleUserId = '', itemName = '', description = '', status = '', objective = '', responsibleUser = '', actionItemId = '', deleteActionItem = '', goToEditActionItem = '', statusImage = '', projectName='';
    if (data != undefined) {
        itemName = data.ItemName;
        description = data.Description;
        if (data.Status == StatusCode.Red) {
            statusImage = data.ImagePath + "/bullet-red.png";
        }
        else if (data.Status == StatusCode.Yellow) {
            statusImage = data.ImagePath + "/bullet-yellow.png";
        }
        else if (data.Status == StatusCode.Green) {
            statusImage = data.ImagePath + "/bullet-green.png";
        }
        status = data.StatusName;
        responsibleUserId = data.ResponsibleUserId;
        dueDate = data.DueDate;
        actionItemId = data.ActionItemId;
        projectName = data.ProjectName;
        responsibleUser = data.ResponsibleUserName;
        status = data.Status;
        goToEditActionItem = "/Employee/ActionList/" + data.ActionItemId;
        actionList = "/user/ManageActionItem/" + data.ActionListId;
        deleteActionItem = viewModelActionItem.DeleteActionItemAction;
        addActionItemClick = viewModelActionList.AddActionItemClickAction;
        
    }
    // self.StatusImage = statusImage;
    if (dueDate != "") {
        self.DueDate = ko.observable(dateFormat(common.ParsedJsonDate(dueDate), "mm/dd/yyyy"));
    }
    else
        self.DueDate = ko.observable(dueDate);
    self.DeleteActionItem = deleteActionItem;
    self.ResponsibleUserId = ko.observable(responsibleUserId);
    self.ItemName = ko.observable(itemName);
    self.Status = ko.observable(status);
    //self.PeriodView = ko.observable(periodView);
    self.ResponsibleUserName = ko.observable(responsibleUser);
    //self.DisplayPeriod = ko.observable(displayPeriod);
    self.Description = ko.observable(description);
    //self.GoToMainMetricDasbord = goToMainMetricDasbord;
    self.GoToEditActionItem = goToEditActionItem;
    self.ActionItemId = ko.observable(actionItemId);
    self.ProjectName = ko.observable(projectName);
    //self.GoToViewUserDetail = goToViewUserDetail;
 
    return self;
}

EMPMGMT.User.ManageActionList.ActionListViewModel = function (data, viewmodel) {
    var self = this;
    var riskIssues = '', addActionItemClick = '', actionList = '', title = '', description = '', status = '', objective = '', responsibleUser = '', actionListId = '', deleteActionList = '', goToCreatActionList = '', statusImage = '', addActionItem = '', responsibleUserId = '', projectId = '', projectName = '', resourceList = '', totalWorkTime='';
    if (data != undefined) {
        riskIssues = data.RiskIssues;
        title = data.Title;
        projectId = data.ProjectId;
        projectName = data.ProjectName;
        description = data.Description;
        if (data.Status == StatusCode.Red) {
            statusImage = data.ImagePath + "/bullet-red.png";
        }
        else if (data.Status == StatusCode.Yellow) {
            statusImage = data.ImagePath + "/bullet-yellow.png";
        }
        else if (data.Status == StatusCode.Green) {
            statusImage = data.ImagePath + "/bullet-green.png";
        }
        actionList = "/Project/ManageActionItem/" + data.ActionListId;
        status = data.StatusName;
        objective = data.Objective;
        actionListId = data.ActionListId;
        responsibleUser = data.ResponsibleUserName;
        responsibleUserId = data.ResponsibleUserId;
        //goToMainMetricDasbord = "/Employee/MetricDashboardData/" + data.MetricDashboardId;
        //editMetricDasbord = "/Employee/MetricDashboard/" + data.MetricDashboardId;
        goToEditActionList = "/Project/EditActionList/" + data.ActionListId + "/" + data.ProjectId;
        addActionItem = "/Project/ManageActionItem/" + data.ActionListId;
       deleteActionList = viewmodel.DeleteActionListAction;
        // addActionItemClick = viewModelActionList.AddActionItemClickAction;
       resourceList = "/Project/ManageActionItem/" + data.projectId;

       totalWorkTime = data.TotalWorkTime;
    }
    self.ActionList = actionList;
 
    self.AddActionItemClick = addActionItemClick;
    self.StatusImage = statusImage;
    self.DeleteActionListAction = deleteActionList;
    self.Title = ko.observable(title);
    self.Status = ko.observable(status);
    self.ProjectId = ko.observable(projectId);
    self.AddActionItem = addActionItem;
    //self.PeriodView = ko.observable(periodView);
    self.ResponsibleUserName = ko.observable(responsibleUser);
    self.UserDetailHref = controllerUrl + "UserDetails/" + responsibleUserId;
    //self.DisplayPeriod = ko.observable(displayPeriod);
    self.Description = ko.observable(description);
    //self.GoToMainMetricDasbord = goToMainMetricDasbord;
    self.GoToEditActionList = goToEditActionList;
    self.AddActionItemClickAction = addActionItemClick;
    self.ActionListId = ko.observable(actionListId);
    //self.GoToViewUserDetail = goToViewUserDetail;
    self.TotalWorkTime = ko.observable(totalWorkTime);

    viewmodel.ProjectName(projectName);

    ResponsibleTempList = resourceList;

    return self;
}

EMPMGMT.User.ManageActionList.tableHeaderViewModel = function (title, columnname, viewModel) {

    var self = this;
    self.ColumnText = ko.observable(title);
    self.ColumnName = ko.observable(columnname);
    self.SortOrder = ko.observable('');
    self.Sort = viewModel.Sort;
}


EMPMGMT.User.ManageActionList.pageViewModel = function () {
    //Class variables
    var self = this;
    self.ProjectName = ko.observable();
    var orderbycolumn = '', orderby = '';
    self.ActionItem = new EMPMGMT.User.ManageActionList.ActionItemViewModel();

    var ActionItemPopup, ActionItemHelper = {
        Open: function () { ActionItemPopup.methods.open(); },
        Close: function () { ActionItemPopup.methods.close(); }
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

    self.AddActionItemClickAction = function (data) {

        ActionItemHelper.Open();
    }

    self.Init = function () {
        ActionItemPopup = $("._ActionItemPopbox").popbox({
            close: '._ActionItemClose'
        });
       

        $('#ResponsibleUserAutoComplete').autocomplete({          
            serviceUrl: '/Employee/GetProjectResponsibles/'+ $('#hdnProjectId').val(),//  GetResponsibles',
            displayProperty: 'FullName',
            onChnage: function () {
                
               // self.ActionItem.ResponsibleUserId('');
               // self.ActionItem.ResponsibleUserName('');
               // $('#hdnResponsibleUserId').val('');

            },
            params: {},
            lookup: null,
            onSelect: function (suggestion) {
               
                self.ActionItem.ResponsibleUserId(suggestion.data.UserId);
                $('#hdnResponsibleUserId').val(suggestion.data.UserId);
                self.ActionItem.ResponsibleUserName(suggestion.value);
                alert("select" + suggestion.value + suggestion.data.UserId);
            },
        });
    }


    self.Init();
    //metric dashboard Header
    self.TableHeaders = ko.observableArray([]);
    self.RenderTableHeaders = function () {

        self.TableHeaders.push(new EMPMGMT.User.ManageActionList.tableHeaderViewModel("Title", "Title", self));
        self.TableHeaders.push(new EMPMGMT.User.ManageActionList.tableHeaderViewModel("Responsible", "ResponsibleUserName", self));
        self.TableHeaders.push(new EMPMGMT.User.ManageActionList.tableHeaderViewModel("Worked (Hour(s))", "TotalWorkTime", self));
        self.TableHeaders.push(new EMPMGMT.User.ManageActionList.tableHeaderViewModel("Status", "Status", self));
    }
    self.Sort = function (col) {
        //if (col.ColumnName() != "Status")
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
            self.GetActionList(CurrentPage);
        }
    }
    self.ResponsibleTempList = ko.observableArray([]);
    self.ActionList = ko.observableArray([]);
    self.SearchText = ko.observable('');
    self.GetActionList = function (currentPageNo) {
            var objParam = new Object();
        objParam.CurrentPage = currentPageNo;
        objParam.UserStatus = self.SelectedFilter;
        objParam.SearchText = self.SearchText();//$('._SearchTxt').val();
        objParam.OrderByColumn = orderbycolumn;
        objParam.OrderBy = orderby;
        objParam.ProjectId = $('#hdnProjectId').val();
        objParam.ProjectId = $('#hdnProjectId').val();
        $("._ActionListPager").html('');
        EMPMGMT.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetActionListItem", objParam,
            function onSuccess(response) {

                //currentUser = response.LoggedInUser;
                self.RenderActionList(response);
                $("._ActionListPager").html(self.GetPaging(response.TotalRecords, currentPageNo, PagingMethodForActionList, "ActionListPager"));
                EMPMGMT.Framework.Common.ApplyPermission();
            },
        function onError(err) {
            EMPMGMT.Framework.Core.ShowMessage(err.Message, true);

        });
    }

    self.FilteredData = function () {

        self.GetActionList(CurrentPage);

    }

    self.GetPaging = function (Rowcount, currentPage, methodName, uniqueMethodName) {
        return EMPMGMT.Framework.Core.GetPagger(Rowcount, currentPage, methodName, uniqueMethodName);

    }

    self.RenderActionList = function (acionList) {       
        $("._ActionListRecord").hide();
        self.ActionList.removeAll();
        if (acionList.DataList.length == 0) {
            $("._ActionListRecord").show();
        }
        ko.utils.arrayForEach(acionList.DataList, function (actionListData) {

            self.ActionList.push(new EMPMGMT.User.ManageActionList.ActionListViewModel(actionListData,self));

        });


    };
    self.RenderTableHeaders();
    self.GetActionList(CurrentPage);
    self.DeleteActionListAction = function (data) {

        var ret = confirm(EMPMGMT.Messages.ActionList.DeleteActionList);
        if (ret) {
            var PostData = new Object();
            PostData.ActionListId = data.ActionListId();

            EMPMGMT.Framework.Core.doPostOperation
                    (
                        controllerUrl + "DeleteActionList",
                        PostData,
                        function onSuccess(response) {
                            // self.DashboardDocumentsList.remove(data);
                            //EMPMGMT.Framework.Common.ApplyPermission();
                            self.GetActionList(CurrentPage);
                        },
                        function onError(err) {
                            self.status(err.Message);
                        }
                    );
        }
    }


    ProjectId = $('#hdnProjectId').val();
}


