jQuery.namespace('EMPMGMT.User.ManageTimeSheet');
var controllerUrl = "/Employee/";
var viewModelTimeSheet = '';
var CurrentPage = 1;
var Message = {
    Failure: 'Failure',
    Success: 'Success'
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

EMPMGMT.User.ManageTimeSheet.pageLoad = function () {

    CurrentPage = 1;
    viewModelTimeSheet = new EMPMGMT.User.ManageTimeSheet.pageViewModel();
    ko.applyBindings(viewModelTimeSheet);

}
var PagingMethodTimeSheet = "viewModelTimeSheet.GetTimeList";

EMPMGMT.User.ManageTimeSheet.TimeSheetViewModel = function (data) {
    var self = this;
    //debugger;
    var monthList = '', year = '', totalTime = '', userId = '', dates = '', timeTaken = '', months = '', actionItemId = '', workHours = '', itemName = '', projectName = '', projectId = '', fulldate = '', timesheetComment = '', title = '', actionListId='';
    var goToViewProjectDetail = '', goToActionItemDetail = '', timeSheetId = '', gotoEditTimsheet = '', selectDate = '', timeTaken1 = '', gotoDeleteTimsheet = '', runningMonth='';
    if (data != undefined) {

        monthList = data.MonthList;
        year = data.Years;
        totalTime = data.TotalTime;
        userid = data.LoggedInUser;
        dates = data.Dates;
        timeTaken = data.TimeTaken;
        months = data.Months;
        actionItemId = data.ActionItemId;
        workHours = data.WorkHours;
        itemName = data.ItemName;
        projectName = data.ProjectName;
        projectId = data.ProjectId;
        fulldate = data.FullDate;
        timesheetComment = data.TimeSheetComment;
        title = data.Title;
        actionListId = data.ActionListId ;
        goToViewProjectDetail = "/Employee/ProjectDetail/" + data.ProjectId;
        goToActionItemDetail = "/Employee/ActionItemDescription/" + data.ActionItemId;
     // gotoDeleteTimsheet = "/Employee/DeleteTimeSheet/" + data.ActionItemId;
     // gotoEditTimsheet = "/Employee/TimeSheet/" + data.TimeSheetId;
        timeSheetId = data.TimeSheetId;
        selectDate = data.SelectDate;
        timeTaken1 = data.TimeTaken1;

        runningMonth = data.RunningMonth;
    }

    self.MonthList = ko.observable(monthList);
    self.Years = ko.observable(year);
    self.Months = ko.observable(months);
    self.TotalTime = ko.observable(totalTime);
    self.UserId = ko.observable(userId);
    self.TimeTaken = ko.observable(timeTaken);
    self.Dates = ko.observable(dates);
    self.ActionItemId = ko.observable(actionItemId);
    self.WorkHours = ko.observable(workHours);
    self.ItemName = ko.observable(itemName);
    self.ProjectName = ko.observable(projectName);
    self.ProjectId = ko.observable(projectId);
    self.FullDate = ko.observable(fulldate);
    self.TimeSheetComment = ko.observable(timesheetComment);
    self.ProjectDetail = goToViewProjectDetail;
    self.Title = ko.observable(title);
    self.ActionListId = ko.observable(actionListId);
    self.ActionItemDetails=ko.observable(goToActionItemDetail);
   
    self.TimeSheetId = ko.observable(timeSheetId);
    self.SelectDate = ko.observable(selectDate);
    self.ManageTimeSheetDetials = ko.observableArray([]);
    self.TimeDetials = ko.observableArray([]);
    self.IsDetailEnable = ko.observable(false);
    self.IsActionItemDetailEnable = ko.observable(false);
    self.IsInitialLoading = ko.observable(true);
    self.currentmonth = ko.observable();
    self.currentYear = ko.observable();
    self.EditTimesheet = ko.observable(gotoEditTimsheet);
    self.TimeTaken1 = ko.observable(timeTaken1);
    self.DeleteTimesheet = ko.observable(gotoDeleteTimsheet);

    self.RunningMonth = ko.observable(runningMonth);

    self.Visble = ko.observable(false);
    var curDate = new Date();
    var dtcheck = new Date(curDate.getFullYear(), (curDate.getMonth()), curDate.getDate());
    if (self.SelectDate() != undefined && self.SelectDate() != null) {
        var dtArr = self.SelectDate().split('/');
        var dbDate = new Date();
        if (dtArr.length > 0) {
            dbDate = new Date(parseInt(dtArr[2]), parseInt(dtArr[0] - 1), parseInt(dtArr[1]) + 3);
            if (dtcheck > dbDate) {
                self.Visble(false)
            }
            else {
                self.Visble(true)
            }
        }
    }
   
    self.DetailTImeSheet = function (obj) {
        var objParam = new Object();
        objParam.Years = self.Years();
        objParam.Months = self.Months();
        objParam.UserId = self.UserId();
        if (!self.IsDetailEnable()) {
            EMPMGMT.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetTimeDetails", objParam,
                 function onSuccess(response) {
                 
                     self.IsDetailEnable(true);
                     self.IsInitialLoading(false);
                     self.ManageTimeSheetDetials.removeAll();
                     ko.utils.arrayForEach(response.MonthDateList, function (getManageTimeSheetDetails) {
                        
                         getManageTimeSheetDetails.Years = self.Years();
                         getManageTimeSheetDetails.Months = self.Months();
                                       

                         self.ManageTimeSheetDetials.push(new EMPMGMT.User.ManageTimeSheet.TimeSheetViewModel(getManageTimeSheetDetails));
                        


                     });
                     //EMPMGMT.Framework.Common.ApplyPermission();
                 },
             function onError(err) {
                 EMPMGMT.Framework.Core.ShowMessage(err.Message, true);
             });
        }

       
        //slide up down
        self.SileTo(obj.MonthList());
        //
    }

  
    self.TimeSheetWholeDetail = function (obj) {
        //debugger;
         var objParam = new Object();
        objParam.Years = self.Years();
        objParam.Months = self.Months();
        objParam.UserId = self.UserId();
        objParam.Date = self.Dates();
        if (!self.IsActionItemDetailEnable()) {
            EMPMGMT.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetDateActionItemDetails", objParam,
             function onSuccess(response) {
                 self.IsActionItemDetailEnable(true);
                 self.IsInitialLoading(false);
                 self.TimeDetials.removeAll();
                 ko.utils.arrayForEach(response.ActionItemList, function (getTimeDetails) {
                     self.TimeDetials.push(new EMPMGMT.User.ManageTimeSheet.TimeSheetViewModel(getTimeDetails));
                   
                 });
             },
             function onError(err) {
                 EMPMGMT.Framework.Core.ShowMessage(err.Message, true);
             });
        }
    
      
        //if (self.TimeDetials().length == 0) { $('._NoSubchild').show(); } else { { $('._NoSubchild').hide(); } }

        //slide up down
        self.ChildSileTo(obj.Dates());
        //
    }

    self.SileTo = function (parent) {        
        $('div._child[parentid !="' + parent + '"]').slideUp();        
        $('div[parentid="' + parent + '"]').slideToggle();
    }


    self.ChildSileTo = function (parent) {     
        $('div._Subchild[parentid !="' + parent + '"]').slideUp();
        $('div._Subchild[parentid ="' + parent + '"]').slideToggle();
      
    }


  


    return self;
}


EMPMGMT.User.ManageTimeSheet.tableHeaderViewModel = function (title, columnname, viewModel) {

    var self = this;
    self.ColumnText = ko.observable(title);
    self.ColumnName = ko.observable(columnname);
    self.SortOrder = ko.observable('');
    self.Sort = viewModel.Sort;
}

EMPMGMT.User.ManageTimeSheet.pageViewModel = function (data) {
   // self.TimeSheetDetails = new EMPMGMT.User.ManageTimeSheet.TimeSheetViewModel(null, self, true, 0);
    var self = this;
    var createTimeSheet = '', editTimeSheet='', orderbycolumn = '', orderby = '', timeSheetPopupMessageHelper = EMPMGMT.Messages.TimeSheet, frameworkHelper = EMPMGMT.Framework.Core;
    self.TimeSheetTitle = ko.observable(timeSheetPopupMessageHelper.AddTimeSheetHeader);
    if (data != undefined) {
        createTimeSheet = viewModelTimeSheet.CreateTimeSheet;
        editTimeSheet = viewModelTimeSheet.EditTimeSheet;
    }

    timeSheetPopupMessageHelper = EMPMGMT.Messages.TimeSheet,

    self.CreateTimeSheet = createTimeSheet;
    self.EditTimeSheet = editTimeSheet;

    self.SearchText = ko.observable('');

    self.TimeSheetPopupTitle = ko.observable();
    self.AddTimeSheet = function (TimeSheet) {
       // $("#ddlProjectId").val(0); $('#ddlTimeId').val(0); $('#hdnActionidtemId').val(''); $('#ddlActionItemId').val(0);
        /* $("#ddlProjectId").attr('disabled', false);  $("#ddlActionItemId").attr('disabled', false); $('#EntryDate').attr('disabled', false); //$('#ddlTimeId').attr('disabled', false);*/
        self.TimeSheetTitle(timeSheetPopupMessageHelper.AddTimeSheetHeader);
        self.TimeSheetPopupTitle();
    }
    self.DeleteTimesheet = function (TimeSheet) {
        var ret = confirm(EMPMGMT.Messages.TimeSheet.ConfirmDeleteAnnualObjective);
        if (ret) {
            TimeSheet.TimeSheetId();
            var PostData = new Object();
            PostData.TimeSheetId = TimeSheet.TimeSheetId();
            PostData.UserId = TimeSheet.UserId();
            EMPMGMT.Framework.Core.doPostOperation(controllerUrl + "DeleteTimeSheet",
                 PostData, function onSuccess(response) {
                   var TimeSheetId = $('#TimeSheetId').val();
                   EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.TimeSheet.TimeSheetDeleteSuccessMsg, true);
                   window.location.href = window.location.href;
                  
                 }, function onError(err) { self.status(err.Message); });

        }
    }
    self.EditTimesheet = function (TimeSheet) {
        //debugger;        
        self.TimeSheetTitle(timeSheetPopupMessageHelper.EditTimeSheetHeader);
        TimeSheet.WorkHours();
        var workHour = TimeSheet.WorkHours().replace('Hour(s)', '').replace('Minute(s)', '').replace(' ', '.').replace(' ', '').replace('30', '5');
        $("#ddlProjectId").val(TimeSheet.ProjectId());
        $('#hdnTimeSheetId').val(TimeSheet.TimeSheetId());      
        $('#ddlTimeId').val(TimeSheet.TimeTaken1());
       // $('#ddlTimeId').val(parseFloat(workHour));
        $('#txtComment').val(TimeSheet.TimeSheetComment());
        self.TimeSheetPopupTitle();
        $('#hdnActionidtemId').val(TimeSheet.ActionItemId());
        $('#EntryDate').val(TimeSheet.SelectDate());
      //  $("#ddlProjectId").attr('disabled', true); $("#ddlActionItemId").attr('disabled', true);  $('#EntryDate').attr('disabled', true);         
        ActionItemOnProject(TimeSheet.ProjectId());
      
    }
  
 

    self.FilteredData = function () {

        self.RenderTableHeaders();
        self.GetTimeList(1);

    }

    self.IsInitialLoading = ko.observable(true);

    self.TableHeaders = ko.observableArray([]);


    self.RenderTableHeaders = function () {
        var ColumnHeaderText = '<table style="width:100%"><tr><td style="width:50%;">Month, Year</td><td>Duration</td></tr> </table>';
        //self.TableHeaders.push(new EMPMGMT.User.ManageTimeSheet.tableHeaderViewModel("Month Year", "Month", self));
        self.TableHeaders.push(new EMPMGMT.User.ManageTimeSheet.tableHeaderViewModel(ColumnHeaderText, "Month", self));

        // self.TableHeaders.push(new EMPMGMT.User.ManageTimeSheet.tableHeaderViewModel("Time Taken", "TimeTaken", self));

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
            // debugger
            // self.GetTimeList(1);
            if (orderby == 'asc')
                self.ManageTimeSheet.sort(function (left, right) { return left.RunningMonth() == right.RunningMonth() ? 0 : (left.RunningMonth() < right.RunningMonth() ? 1 : -1) })
            else
                self.ManageTimeSheet.sort(function (left, right) { return left.RunningMonth() == right.RunningMonth() ? 0 : (left.RunningMonth() > right.RunningMonth() ? 1 : -1) })

        }
    }


    self.ManageTimeSheet = ko.observableArray([]);


    self.GetTimeList = function (currentPageNo) {
       // debugger;
        var objParam = new Object();
        objParam.UserStatus = self.SelectedFilter;
        objParam.SearchText = self.SearchText();//$('._SearchTxt').val();
        objParam.OrderByColumn = orderbycolumn;
        objParam.OrderBy = orderby;
        EMPMGMT.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetTimeSheet", objParam,
            function onSuccess(response) {
              //  debugger;
                self.RenderTimeSheet(response);
                self.IsInitialLoading(false);
              //  $("._TimeSheetPager").html(self.GetPaging(response.TotalRecords, currentPageNo, PagingMethodTimeSheet, "ActionListingPager"));
                EMPMGMT.Framework.Common.ApplyPermission();
            },
        function onError(err) {
            EMPMGMT.Framework.Core.ShowMessage(err.Message, true);

        });
    }

    self.GetPaging = function (Rowcount, currentPage, methodName, uniqueMethodName) {
        return EMPMGMT.Framework.Core.GetPagger(Rowcount, currentPage, methodName, uniqueMethodName);

    }

    self.RenderTimeSheet = function (ManageTimeSheet) {
        self.ManageTimeSheet.removeAll();
        ko.utils.arrayForEach(ManageTimeSheet.MonthList, function (getManageTimeSheet) {
            self.ManageTimeSheet.push(new EMPMGMT.User.ManageTimeSheet.TimeSheetViewModel(getManageTimeSheet));
        });

    };

    self.ActionItemList = ko.observableArray([]);

   


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


    self.RenderTableHeaders();
    self.GetTimeList(1);


    // $('#ScriptDeatils')

  


    return true;


}


ActionItemOnProject = function (ProjectId) {  
   // alert('actionItem');
    var objproject = new Object();
    objproject.ProjectId = $('#ddlProjectId').val();
    objproject.ProjectId = ProjectId;
    var url = controllerUrl + "GetActionItemListforDropdown";
    $('#ddlActionItemId').empty();
    $('#ddlActionItemId').append("<option value=0> Select a Action Item </option>")
    $.ajax({
        type: "POST",
        //data:objproject,// "{'timeSheet':'"+objproject.ProjectId+"'}",
        data: "{'timeSheet':'"+objproject.ProjectId+"'}",
        url: url,
        contentType: "application/json;",
        dataType: "json",
        success: function (Data) {           
            var dt = Data.ActionItemListForProject;
            $.each(dt, function (key, value) {
                if (value.child > 0)
                {
                    $('#ddlActionItemId').append('<option disabled="disabled" value=' + value.ActionItemId + '>' + value.ItemName + '</option>');
                }
                else {
                    $('#ddlActionItemId').append('<option value=' + value.ActionItemId + '>' + value.ItemName + '</option>');//.val($(value.ActionItemId).html(value.ItemName)));
                }
            });
            if ($('#hdnActionidtemId').val() != '') {
                $('#ddlActionItemId').val($('#hdnActionidtemId').val());
            }
        },
        error: function (Error) { }
    });

  /*  EMPMGMT.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetActionItemListforDropdown", objproject,
            function onSuccess(response) {
                debugger;
                // self.ActionItemList.push();

            });*/
}




var startDate = new Date();
var FromEndDate = new Date();
var ToEndDate = new Date();

ToEndDate.setDate(ToEndDate.getDate() + 365);

$('#EntryDate').datepicker({

    weekStart: 1,
    startDate: '11/01/2014',
    //endDate: FromEndDate,
    autoclose: true
});

