jQuery.namespace('EMPMGMT.User.ManageActionItem');
var controllerUrl = "/Employee/";
var viewModelActionItem;
var gblSelectedParentActionItem = '';
var autocompleteObj = '';
var gblAction = '';
var CurrentPage = 1;
var orderbycolumn = '', orderby = '';
$("#startDatesearch, #dueDatesearch").datepicker({
    weekStart: 1,
    startDate: '01/01/2012',
    autoclose: true
});

var ProgressDateDivWidth = 170;
var gblResponsibleUserIds = '';
var gblSelectedActionItem = '';
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
//*************************** End ********************************//
var common = EMPMGMT.Framework.Common;
EMPMGMT.User.ManageActionItem.pageLoad = function () {  
    CurrentPage = 1;
    viewModelActionItem = new EMPMGMT.User.ManageActionItem.pageViewModel();
    ko.applyBindings(viewModelActionItem), document.getElementById('ManageActionItemContainer');
}
var PagingMethodForActionItem = "viewModelActionItem.GetActionItemFromActionList";

EMPMGMT.User.ManageActionItem.DocumentViewModel = function (data, ImagePath) {
  //  debugger;
    var self = this;
    var documentId = '', documentName = '', deleteDocumentVisible = false, attachedBy = '', attachedDate = '', removeDocument = '', documentPath = '', downloadDocument = '', deleteDocument = '', documentMouseOver = '', documnetMouseLeave = '', documentNameextn='', date;
    if (data != undefined) {
        documentId = data.DocumentId;
        documentName = data.DocumentName;
        documentPath = data.DocumentPath;
        downloadDocument = ImagePath + documentPath;// data.ActionItemPath;
        documentMouseOver = viewModelActionItem.DocumentMouseOverAction;
        documentMouseLeave = viewModelActionItem.DocumentMouseLeaveAction;
        deleteDocument = viewModelActionItem.DeleteDocumentAction;
        date = data.CreatedDate;
        if (date == '' || date == undefined || date == null)
            date = '';
        else
        { date = dateFormat(common.ParsedJsonDate(date), "mm/dd/yyyy") }
        if (data.DocumentPath != undefined) {
            var pathext = data.DocumentPath.split('.');
            if (pathext.length>0)
                documentNameextn =documentName+'.'+pathext[pathext.length-1];
        }
    }

    self.DeleteDocumentVisible = ko.observable(deleteDocumentVisible);
    self.DeleteDocument = deleteDocument;
    self.DocumentMouseOver = documentMouseOver;
    self.DocumentMouseLeave = documentMouseLeave;
    self.DocumentName = ko.observable(documentName);
    self.DocumentId = ko.observable(documentId);
    self.DocumentPath = ko.observable(documentPath);
    self.DownloadDocument = ko.observable(downloadDocument);

    self.DocumentNameextn = ko.observable(documentNameextn);

    self.Date = ko.observable(date);
    self.Visible = ko.observable(false);

    return true;
}
EMPMGMT.User.ManageActionItem.ResponsibleViewModel = function (data) {// debugger
    var self = this;
    var responsibleUserId = '', responsibleUserName = '', removeActionItemResponsible = '', statusdrop='';

    if (data != undefined) {

        responsibleUserId = data.ResponsibleUserId;
        responsibleUserName = data.ResponsibleUserName;
        statusdrop = data.StatusDrop;
    }
   
    self.RemoveActionItemResponsible = viewModelActionItem.RemoveActionItemResponsibleAction;
    self.ResponsibleUserId = ko.observable(responsibleUserId);
    self.ResponsibleUserName = ko.observable(responsibleUserName);
    self.StatusDrop = ko.observable(statusdrop);
    self.ResponsibleOnMouseLeave = viewModelActionItem.ResponsibleOnMouseLeaveAction;
    self.ResponsibleMouseOver = viewModelActionItem.ResponsibleMouseOverAction;
    if (data.StatusDrop != undefined || data.StatusDrop != null) {
        $('._ddlStatus').val(data.StatusDrop);
    }
   
}
EMPMGMT.User.ManageActionItem.ActionItemViewModel_N = function (data, viewModelObject, isExpandable, textpadding) {
    debugger;
    var self = this;
    var imagepadding = 0, progressDateHtml = "&nbsp";
    var collapseExpandImage = '';
    var dueDate = '', priority = '', isSendEmailNotification = false, isArchived = false, itemName = '', responsibleUserId = '', description = '', status = '', responsibleUser = '', actionItemId = '', text = '', maxMinutes = '', applyCssColortext = '';
    var startDate = '', eTA = '', rCAId = '', rCATitle = '', applyProgressSatusCss = '', applyProgressStatusCurrenDateCss = '', actionItemChildCurrentDate = '', progressCurrentDateLeftMargin = 0, progreeStartDate = '', progressDueDate = '', progressWidth = '', progressLeftMargin = 0, progressRightMargin = 0, actionItemRedirection = '';
    var enableProgressCurrentDate = false, statusdrop = '', actualTime='';
    var itemstartDate = '', itemdueDate = '';
    if (data != "" && data != null && data != undefined) {
        itemName = data.ItemName;
        description = data.Description;
        itemstartDate = data.StartDate;
       startDate = data.MinStartDate;
        dueDate = data.MaxDueDate;
         itemdueDate = data.DueDate;
        eTA = data.ETA;
        isSendEmailNotification = data.IsSendEmailNotification;
        isArchived = data.IsArchived;
        actionItemId = data.ActionItemId;
        responsibleUserId = data.ResponsibleUserId;
        status = data.ActionItemStatus;
        priority = data.Priority;
        responsibleUser = data.ResponsibleUserName == "" ? "&nbsp;" : data.ResponsibleUserName;
        rCAId = data.RCAId;
        rCATitle = data.RCATitle;
        text = data.Minutes;
        maxMinutes = data.MaxMinutes
  
        statusdrop = data.StatusDrop;
        actualTime=data.ActualTime;

    }
     self.ItemstartDate = ko.observable(itemstartDate);
     self.ItemDueDate = ko.observable(itemdueDate);

    self.Collapsed = ko.observable(false);
    self.ItemName = ko.observable(itemName).extend({
        required: {
            message: 'Item Name is required'
        }
    });
    debugger;
    self.Description = ko.observable(description);
    self.ActionItemId = ko.observable(actionItemId);
    self.ResponsibleUserName = ko.observable(responsibleUser);
  //  if (startDate.indexOf('-')>0)  { startDate = ''; }
    self.StartDate = (startDate == null || startDate == undefined || startDate == "" || startDate.indexOf('-') > 0) ? ko.observable('').extend({
        required: {
            message: 'Start Date is required'
        }
    })
        :ko.observable(dateFormat(common.ParsedJsonDate(startDate), "mm/dd/yyyy")).extend({
        required: {
            message: 'Start Date is required'
        }
   
    
    });
    //if (dueDate.indexOf('-')>0) { dueDate = ''; }
    self.DueDate = (dueDate != "" && dueDate != null && dueDate != undefined && dueDate.indexOf('-') < 1) ? ko.observable(dateFormat(common.ParsedJsonDate(dueDate), "mm/dd/yyyy")).extend({
        required: {
            message: 'Due Date is required'
        }
    })
    : ko.observable('').extend({
        required: {
            message: 'Due Date is required'
        }
    });
    
    self.ETA = (eTA != "" && eTA != null && eTA != undefined) ? ko.observable(dateFormat(common.ParsedJsonDate(eTA), "mm/dd/yyyy")) : ko.observable(eTA);
    self.IsSendEmailNotification = ko.observable(isSendEmailNotification);
    self.IsArchived = ko.observable(isArchived);
    self.ResponsibleUserId = ko.observable(responsibleUserId);
    self.UserDetailHref = controllerUrl + "UserDetails/" + responsibleUserId;
    var actionItemStatusDiv = actionItemId;
    self.StatusElementId = CreateId(actionItemStatusDiv);
    self.RCAId = ko.observable(rCAId);
    self.RCATitle = ko.observable(rCATitle);
    self.Status = ko.observable(status);
    self.Priority = ko.observable(priority);
    self.Minutes = ko.observable(text);
    self.MaxMinutes = ko.observable(maxMinutes);
    self.StatusDrop = ko.observable(statusdrop);
    self.ActualTime = ko.observable(actualTime);

  
    if (data != null) {
     
        self.Children = viewModelObject.RenderChildren(data.Children, textpadding + 20);
    }
    else
        self.Children = ko.observableArray([]);

    if (self.Children().length == 0) {
        expandableImageVisiblity = false;
        applyProgressSatusCss = 'actionItemChildProgress';
        applyCssColortext = 'textcolorchild';
    }
    else {
        // enableParentNode = false;
        collapseExpandImage = "fa fa-minus-square";
        expandableImageVisiblity = true;
        applyProgressSatusCss = 'actionItemHeaderProgress';
        applyCssColortext = 'textcolorParent';
        //expandableImage = true;
    }
    var list = ko.utils.arrayFilter(self.Children(), function (ai) {

        return ai.IsArchived() === false;
    });


       // alert(obj.length);
    self.IsChildrenArchived = ko.observable(list.length == 0 ? false : true);
    self.EnableParentNode = ko.observable(false)
    self.ExpandableImageVisiblity = ko.observable(expandableImageVisiblity);
    self.CollapseExpandImage = ko.observable(self.Children().length > 0 ? "fa fa-minus-square" : "fa fa-minus-square");  //fa fa-plus-square" : "fa fa-minus-square"
    self.CreatehierachyActionItem = viewModelObject.CreatehierachyActionItemAction;
    self.EditActionItem = viewModelObject.EditActionItemAction;
    self.DeleteActionItem = viewModelObject.DeleteActionItemAction;
    self.ActionItemCollapseExpandClick = viewModelObject.ActionItemCollapseExpandClickAction;

    self.IsExpandable = ko.observable(isExpandable);

    if (data != null) {

        self.ActionItemRedirection = "/Employee/ActionItemDescription/" + data.ActionItemId;

        if (data.ActionListMaxDueDate != "" && data.ActionListMaxDueDate != null && data.ActionListMaxDueDate != undefined && data.MaxDueDate != "" && data.MaxDueDate != null && data.MaxDueDate != undefined && data.ActionListMaxDueDate.indexOf('-')<0 && data.MaxDueDate.indexOf('-')<0) {
            progressDueDate = data.ActionListMaxDueDate;// ko.observable(dateFormat(common.ParsedJsonDate(data.ActionListMaxDueDate), "mm/dd/yyyy"));
            progreeStartDate = data.ActionListMaxDueDate;//new Date(ko.observable(dateFormat(common.ParsedJsonDate(data.ActionListMaxDueDate), "mm/dd/yyyy")) - ko.observable(dateFormat(common.ParsedJsonDate(data.MaxDueDate), "mm/dd/yyyy")));


            var LeftMargin = 0;
            var RightMargin = 0;



            var DatediffPercentage = ProgressDateDivWidth / data.ActionListMaxMinDatesTotalDiff;//calculate the 1 pecentage of total day

            var StartDateDiff = data.MinStartDatesDiff;
            var DueDateDiff = data.MaxDueDatesDiff;
            //if (StartDateDiff == 0) {

            //LeftMargin = 0;
            //progressLeftMargin = 0;
            // progressWidth = ProgressDateDivWidth;

            //}
            // else {          
            if (data.StartDate == data.MaxDueDate) {

                progressLeftMargin = ((data.ActionListMaxMinDatesTotalDiff - ((DueDateDiff) + data.ActionListMaxMinDatesTotalDiff - (StartDateDiff + DueDateDiff))) * DatediffPercentage);
                progressWidth = 1 * DatediffPercentage;

            }
            else {

                progressLeftMargin = ((data.ActionListMaxMinDatesTotalDiff - ((DueDateDiff) + data.ActionListMaxMinDatesTotalDiff - (StartDateDiff + DueDateDiff))) * DatediffPercentage);
                progressWidth = Math.ceil((data.ActionListMaxMinDatesTotalDiff - (StartDateDiff + DueDateDiff))) * DatediffPercentage;
            }

            //}

        }

        if (data.ActionListMaxMinDatesTotalDiff < data.ActionListCurrentDateDiff) {

            progressCurrentDateLeftMargin = ProgressDateDivWidth;
            enableProgressCurrentDate = false;

        }
        else {
            if (data.ActionListMaxMinDatesTotalDiff < (data.ActionListMaxMinDatesTotalDiff - data.ActionListCurrentDateDiff)) {

                enableProgressCurrentDate = false;
            }
            else {
                enableProgressCurrentDate = true;
                progressCurrentDateLeftMargin = (data.ActionListMaxMinDatesTotalDiff - (data.ActionListMaxMinDatesTotalDiff - data.ActionListCurrentDateDiff)) * DatediffPercentage;

            }
        }
    }
    self.ApplyCssColortext = ko.observable(applyCssColortext);
    self.ApplyProgressSatusCss = ko.observable(applyProgressSatusCss);
    self.Textpadding = ko.observable(textpadding);
    self.ProgressLeftMargin = ko.observable(progressLeftMargin);
    self.ProgressRightMargin = ko.observable(progressRightMargin);
    self.ProgressWidth = ko.observable(progressWidth);
    self.Imagepadding = ko.observable(imagepadding);
    self.ProgressDateHtml = ko.observable(progressDateHtml);
    self.EnableProgressCurrentDate = ko.observable(enableProgressCurrentDate);
    self.ProgressCurrentDateLeftMargin = ko.observable(progressCurrentDateLeftMargin);
}

EMPMGMT.User.ManageActionItem.tableHeaderViewModel = function (title, columnname, viewModel) {

    var self = this;
    self.ColumnText = ko.observable(title);
    self.ColumnName = ko.observable(columnname);
    self.SortOrder = ko.observable('');
    self.Sort = viewModel.Sort;
    if (columnname == "") {
        self.SortOrder('no-cursor');
        self.Sort = null;
    }
}

EMPMGMT.User.ManageActionItem.pageViewModel = function () {
    //Class variables
    var self = this;
    var chkOverDue = $('#chkOverDue'), chkOnTime = $('#chkOnTime'), chkBeforeDue = $('#chkBeforeDue')

    self.ArchivedData = ko.observable(false);
    //var ActionItem, ActionItemHelper = {
    //    Open: function () { ActionItem.methods.open(); },
    //    Close: function () { ActionItem.methods.close(); }
    //}
    //var ActionItemPopup, ActionItemHelper = {
    //    Open: function () { ActionItemPopup.methods.open(); },
    //    Close: function () { ActionItemPopup.methods.close(); }
    //}

    self.clickArchivedData = function (obj) {
        if (obj.ArchivedDataStatus == true) {
            if (obj.ArchivedData == true) {
                self.Recursive(self.ActionItemList(), true);
                $('tr.actiontitle').removeClass("displaynone");
            }
            else {
                self.Recursive(self.ActionItemList(), false);
                $('tr.actiontitle').addClass("displaynone");
            }
        }
        else {

            if (obj.ArchivedData() == true) {

                self.Recursive(self.ActionItemList(), true);
                $('tr.actiontitle').removeClass("displaynone");
            }
            else {
                self.Recursive(self.ActionItemList(), false);
                $('tr.actiontitle').addClass("displaynone");
            }
        }
        return true;
    }

   
    self.Recursive = function (data, IsVisible) {
        ko.utils.arrayForEach(data, function (ai) {

            if (ai.Children().length > 0) {

                var list = ko.utils.arrayFilter(ai.Children(), function (child) {
                    return child.IsArchived() === false;
                });
                if (list.length == 0 && IsVisible == false) {
                    ai.IsChildrenArchived(false);
                } else if (IsVisible == true) { ai.IsChildrenArchived(true); }
                self.Recursive(ai.Children(), IsVisible);
            }
        });
    }


   


    $("input").bind("keydown", function (event) {
        //debugger;
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


    /////
    self.GetResponsibleUsers = function () {
        //debugger;
        var obj = new Object();
       
         obj.Id_encrypted = $('#hdnProjectId').val();
        ///Employee/GetProjectResponsibles/' + $('#hdnProjectId').val(),
        $('._ddlResponsible').empty();
        $('._ddlResponsible').append("<option value=0>Select</option>");
        EMPMGMT.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetProjectResponsibles", obj, //GetResponsibleUser
            function onSuccess(response) {
                //debugger;
               
                $.each(response, function (index, element) {
                    $('._ddlResponsible').append("<option value='" + element.UserId + "'>" + element.FullName + "</option>");
                });
            },

            function onError(err) {
               // debugger;
                self.status(err.Message);
            });
    }
    


    



    var actionItemChildren = new Array();

    self.IsInitialLoading = ko.observable(true);
    self.ActionHeaderData = ko.observableArray([]);

    // self.ResponsibleSelectedImage = ko.observable(false);
    self.ProgressDate = ko.observable('');
    self.ProgressDateHeader = ko.observable('');
    self.ResponsibleOnMouseOver = function (data) {
        //  var self = this;
        //  self.ResponsibleHoverClass('myHoverClass');
        //alert(1);
        //  self.ResponsibleSelectedImage(true);
    }
    self.ResponsibleOnMouseLeave = function (data) {
        // alert(3);
    }
    self.RemoveActionItemResponsibleAction = function (data) {
        debugger;
        self.ResponsibleTempList.remove(data);
        autocompleteObj.options.notToDisplayItems.splice($.inArray(data.ResponsibleUserId(), autocompleteObj.options.notToDisplayItems), 1);
        self.RemoveResponsibleItemsList.push(data);

    }
    self.BindActionItemChildren = function (data) {
        ko.utils.arrayForEach(data, function (resp) {
            actionItemChildren.push(new EMPMGMT.User.ManageActionItem.ActionItemViewModel(resp, self, 0, 0));
        });

    }
    self.RootCauseAnalysisId = ko.observable('');
    self.RootCauseAnalysisTitle = ko.observable('');

    //Related Document Functionality................................
    self.DashboardDocumentsList = ko.observableArray([]);
    self.GetDocumentsList = function () {
      //  debugger;
        var objParam = new Object();
        objParam.ActionItemId = gblSelectedActionItem;

        EMPMGMT.Framework.Core.doPostOperation
                (
                    controllerUrl + "GetActionItemDocuments",
                    objParam,
                    function onSuccess(response) {
                        debugger;
                        if (response.DataList.length > 0) {
                         //   self.Visible(true);
                            $('#divdocuments').show();
                        }
                        else {
                            //self.Visible(false);
                            $('#divdocuments').hide();
                        }


                        self.RenderDocumentsList(response);
                        EMPMGMT.Framework.Common.ApplyPermission();
                    },
                    function onError(err) {
                        self.status(err.Message);
                    }
                );
    }
    self.RenderDocumentsList = function (DocumentsList) {
        debugger;
        self.DashboardDocumentsList.removeAll();
        ko.utils.arrayForEach(DocumentsList.DataList, function (documentData) {
            debugger;
            
            self.DashboardDocumentsList.push(new EMPMGMT.User.ManageActionItem.DocumentViewModel(documentData, DocumentsList.ImagePath));
        });

    };

    self.KOSort = function (acionitemlist, sortorder) {
        acionitemlist.sort(function (left, right) {
            switch (orderbycolumn) {
                case "ItemName":
                    if (sortorder == 'asc') {
                        return (left.ItemName().toLowerCase() < right.ItemName().toLowerCase() ? -1 : 1);
                    }
                    else {
                        return (left.ItemName().toLowerCase() > right.ItemName().toLowerCase() ? -1 : 1);
                    }
                    break;
                case "ResponsibleUserName":
                    if (sortorder == 'asc') {
                        return (left.ResponsibleUserName().toLowerCase() < right.ResponsibleUserName().toLowerCase() ? -1 : 1);
                    }
                    else {
                        return (left.ResponsibleUserName().toLowerCase() > right.ResponsibleUserName().toLowerCase() ? -1 : 1);
                    }
                    break;
                case "StartDate":
                    if (sortorder == 'asc') {
                        return (left.StartDate() < right.StartDate() ? -1 : 1);
                    }
                    else {
                        return (left.StartDate() > right.StartDate() ? -1 : 1);
                    }
                    break;
                case "Status":
                    if (sortorder == 'asc') {
                        return (left.Status() < right.Status() ? -1 : 1);
                    }
                    else {
                        return (left.Status() > right.Status() ? -1 : 1);
                    }
                    break;
                case "Minutes":
                    if (sortorder == 'asc') {
                        return (left.MaxMinutes() < right.MaxMinutes() ? -1 : 1);
                    }
                    else {
                        return (left.MaxMinutes() > right.MaxMinutes() ? -1 : 1);
                    }
                    break;
            }
        });
    }

    
    var currentdate = new Date();
    var date = currentdate.getDate();
    var dates = date < 10 ? "0" + date : date;
    var month = currentdate.getMonth() + 1;
    var year = currentdate.getFullYear();
    self.CreateParentActionItem = function () {
        
        gblSelectedActionItem = '';
        gblSelectedParentActionItem = '';
        self.ActionItem.Description('');
        self.ActionItem.ItemName('');
        $('._priorityDDL').val(2);
        self.ActionItem.StartDate('');//month + '/' + dates + '/' + year);
        self.ActionItem.DueDate(''); //month + '/' + dates + '/' + year
        self.ActionItem.ETA('');

        self.ActionItem.IsSendEmailNotification(false);
        self.ActionItem.IsArchived(false);
        self.ActionItem.ResponsibleUserId('');
        self.RootCauseAnalysisTitle('');
        self.ActionItem.EnableParentNode(true);
        self.RootCauseAnalysisId('');
        self.ActionItem.ResponsibleUserName('');
        self.ResponsibleTempList.removeAll();
        autocompleteObj.options.notToDisplayItems = [];
        $("#slider2").slider("option", "disabled", false);
        $('#ActioItemStatus').val(0 + " %");
        var sliderwidth = ($('#slider2').css('width').replace('px', ''));
        $('#slider2 a').css('left', (0 * sliderwidth) / 100);

       // $('#Documents').hide();
        $('#ddlHours').val(0);
        $('#divdocuments').hide();
        self.ActionItemErrorValidator.showAllMessages(false);
        //ActionItemHelper.Open();

        //  PopupCenter($("._ActionItemPopbox"));
    }
    self.CreatehierachyActionItemAction = function (data) {
        //var self = this;
        gblSelectedActionItem = '';
        gblSelectedParentActionItem = data.ActionItemId();
     //   $('#Documents').hide();
        self.ActionItem.Description('');
        self.ActionItem.ItemName('');
        $('._priorityDDL').val(0);
        self.ActionItem.StartDate('');//month + '/' + dates + '/' + year);
        self.ActionItem.DueDate('');//month + '/' + dates + '/' + year
        self.ActionItem.ETA('');
        self.ActionItem.IsSendEmailNotification(false);
        self.ActionItem.IsArchived(false);
        self.ActionItem.ResponsibleUserId('');
        self.RootCauseAnalysisTitle('');
        self.ActionItem.EnableParentNode(true);
        self.RootCauseAnalysisId('');
        self.ActionItem.ResponsibleUserName('');
        self.ResponsibleTempList.removeAll();
        autocompleteObj.options.notToDisplayItems = [];
        $("#slider2").slider("option", "disabled", false);
        $('#ActioItemStatus').val(0 + " %");
        var sliderwidth = ($('#slider2').css('width').replace('px', ''));
        $('#slider2 a').css('left', (0 * sliderwidth) / 100);

        //if (self.ResponsibleTempList().length == 0) {

        //}
        $('#ddlHours').val(0);
        $('#divdocuments').hide();
        self.ActionItemErrorValidator.showAllMessages(false);

        // ActionItemHelper.Open();
        PopupCenter($("._ActionItemPopbox"));

    }
    function MapOptions() {
        var ArrArr = $.map(self.ResponsibleTempList(), function (n, i) {
            return n.ResponsibleUserId();
        });
        autocompleteObj.options.notToDisplayItems = ArrArr
    }
    self.GetActionItemListByACtionListId = function (actionListId) {
        var objParam = new Object();
        objParam.ActionListId = actionListId;

        EMPMGMT.Framework.Core.doPostOperation
                (
                    controllerUrl + "GetActionItemListByACtionListId",
                    objParam,
                    function onSuccess(response) {
                        // self.DashboardDocumentsList.remove(data);
                        //EMPMGMT.Framework.Common.ApplyPermission();
                        //self.BindActionItem(response.DataList);
                    },
                    function onError(err) {
                        self.status(err.Message);
                    }
                );

    }
    self.CancelActionItemAction = function () {
        self.RemoveResponsibleItemsList.removeAll();
        //  ActionItemHelper.Close();
    }
    self.EditActionItemAction = function (data) {
        debugger;
        var ckEditor = $('#ckeditor').attr("id");
        self.ActionItemErrorValidator.showAllMessages(false);
        //self.ResponsibleCountStatus("");
        gblSelectedParentActionItem = data.ActionItemId();
        gblSelectedActionItem = data.ActionItemId();
        $('#Documents').show();
        self.GetActionItemResponsible();
        self.GetDocumentsList();
        self.ActionItem.ItemName(data.ItemName());
        self.ActionItem.Description(data.Description());
       // $('#ckeditor').setData(data.Description());
        
       // $('#ckeditor').setHtml(data.Description());
        
        CKEDITOR.instances.editor1.editable().setHtml(data.Description());
        //self.ActionItem.Description((CKEDITOR.instances.editorDescription.editable().setHtml(data.Description())));
      
     //   CKEDITOR.instances[ckEditor].setHtml(data.Description());

       // $('#ckeditor').setHtml(data.Description());
       // $('#editorDescription').CKEDITOR.instances.editable().setHtml(data.Description());
        self.ActionItem.StartDate(data.StartDate());
        $('._HourDDL').val(data.Minutes());
        // self.ActionItem.Minutes(data.Minutes());
        self.ActionItem.DueDate(data.DueDate());
        self.ActionItem.ETA(data.ETA());
        self.ActionItem.IsSendEmailNotification(data.IsSendEmailNotification());
        self.ActionItem.IsArchived(data.IsArchived());
        self.ActionItem.ResponsibleUserId(data.ResponsibleUserId());
        self.ActionItem.ResponsibleUserName(data.ResponsibleUserName());
        self.ActionItem.StatusDrop(data.StatusDrop());
       
        //if (data.Children() > 0) {
        if (data.Children().length > 0) {
            $("#slider2").slider("option", "disabled", true);
            self.ActionItem.EnableParentNode(false);
            $('._ddlStatus').attr("disabled", true);
        }
        else {
            $("#slider2").slider("option", "disabled", false);
            self.ActionItem.EnableParentNode(true);
            $('._ddlStatus').attr("disabled", false);
        }

        $('#ActioItemStatus').val(data.Status() + "%");
       // debugger;
        var valleft = (data.Status() * sliderwidth) / 100;
        var valright = 100 - (data.Status() * sliderwidth) / 100;


        var sliderwidth = ($('#slider2').css('width').replace('px', ''));
        //$('#slider2 a').css('left', (data.Status().replace("%", '') * sliderwidth) / 100);
        $('#slider2 a').css('left', (data.Status() * sliderwidth) / 100);
        self.ActionItem.Status(data.Status());
        //  $('._statusDDL').val(data.Status());
        $('._priorityDDL').val(data.Priority());
       // $('._ddlStatus').val(data.StatusDrop());
        // ActionItemHelper.Open();

        $("#dueDate,#EtaDate").datepicker("option", "minDate", data.StartDate());

        PopupCenter($("._ActionItemPopbox"));

    }
    self.DocumentMouseOverAction = function (data) {
        data.DeleteDocumentVisible(true);
    }
    self.DocumentMouseLeaveAction = function (data) {
        data.DeleteDocumentVisible(false);
    }
    self.DeleteDocumentAction = function (data) {
        debugger;

        var ret = confirm(EMPMGMT.Messages.ActionItem.DeleteDocument);
        if (ret) {
            var PostData = new Object();
            PostData.DocumentId = data.DocumentId();

            EMPMGMT.Framework.Core.doPostOperation
                    (
                        controllerUrl + "DeleteDocument",
                        PostData,
                        function onSuccess(response) {
                            self.DashboardDocumentsList.remove(data);
                            $('#hdnActionItemDocumentFile').val('');
                            $('#hdnActionItemDocumentName').val('');
                            EMPMGMT.Framework.Common.ApplyPermission();
                        },
                        function onError(err) {
                            self.status(err.Message);
                        }
                    );
        }
    }
    self.RemoveResponsibleItemsList = ko.observableArray([]);
    self.ResponsibleTempList = ko.observableArray([]);
    self.ResponsibleTempListError = ko.observable().extend({
        required: {
            message: 'Responsible is required',
            onlyIf: function () {
                return self.ResponsibleTempList().length === 0;
            }
        }
    });
    self.GetActionItemResponsible = function () {
        var objParam = new Object();
        objParam.ActionItemId = gblSelectedActionItem;

        EMPMGMT.Framework.Core.doPostOperation
                (
                    controllerUrl + "GetActionItemResponsibleByActionItem",
                    objParam,
                    function onSuccess(response) {
                      
                        self.RenderActionItemResponsibleList(response);                          
                        EMPMGMT.Framework.Common.ApplyPermission();
                    },
                    function onError(err) {
                        self.status(err.Message);
                    }
                );
    }
    self.RenderActionItemResponsibleList = function (RespList) {
        self.ResponsibleTempList.removeAll();
        for (i = autocompleteObj.options.notToDisplayItems.length; i-- > 0;) {
            autocompleteObj.options.notToDisplayItems.splice(autocompleteObj.options.notToDisplayItems[i], 1);
        }
        //  alert(autocompleteObj.options.notToDisplayItems.length);
        ko.utils.arrayForEach(RespList.DataList, function (RespData) {
            //debugger;
             //$('._ddlStatus').val(RespData.StatusDrop);
            self.ResponsibleTempList.push(new EMPMGMT.User.ManageActionItem.ResponsibleViewModel(RespData));
            // alert(autocompleteObj.options.notToDisplayItem.length);
            autocompleteObj.options.notToDisplayItems.push(RespData.ResponsibleUserId);
        });


    };

    //Action Item Header
    self.TableHeaders = ko.observableArray([]);
    self.SaveActionItemAction = function (data) {
        debugger;
        //   alert(self.ActionItemErrorValidator().length);
        // alert(gblSelectedParentActionItem);

        if (self.ActionItemErrorValidator().length == 0) {


            var actionItemResponsibleList = new Array();
            var RemoveactionItemResponsibleList = new Array();
            var MergedResponsible = new Array();
            var objParam = new Object();

            for (i = MergedResponsible.length; i-- > 0;) {
                MergedResponsible.splice(MergedResponsible[i], 1);
            }



            ko.utils.arrayForEach(viewModelActionItem.ResponsibleTempList(), function (resp) {
                var obj = new Object();
                obj.ResponsibleUserId = resp.ResponsibleUserId();
                obj.ResponsibleUserName = resp.ResponsibleUserName();
                obj.ActionItemId = gblSelectedActionItem;
                obj.RecordDeleted = false;
                actionItemResponsibleList.push(obj);

            });
            ko.utils.arrayForEach(viewModelActionItem.RemoveResponsibleItemsList(), function (Removeresp) {
                debugger
                var obj = new Object();
                obj.ResponsibleUserId = Removeresp.ResponsibleUserId();
                obj.ResponsibleUserName = Removeresp.ResponsibleUserName();
                obj.ActionItemId = gblSelectedActionItem;
                obj.RecordDeleted = true;
                RemoveactionItemResponsibleList.push(obj);

            });

            MergedResponsible = $.merge(actionItemResponsibleList, RemoveactionItemResponsibleList);
            objParam.ActionListId = $('#hdnActionListdId').val();
            objParam.ParentActionItemId = gblSelectedParentActionItem;
            objParam.ItemName = self.ActionItem.ItemName();
            
            objParam.Description = CKEDITOR.instances.editor1.getData();
         //   objParam.Description = self.ActionItem.Description();
            objParam.StartDate = $('#startDate').val();// == '1/1/0001 12:00:00 AM' ? null : $('#startDate').val();//self.ActionItem.StartDate();
            objParam.DueDate = self.ActionItem.DueDate();// == '1/1/0001 12:00:00 AM' ? null : self.ActionItem.DueDate();
            objParam.ETA = self.ActionItem.ETA();
            objParam.Status = $('#ActioItemStatus').val().replace('%', '');// $("._statusDDL option:selected").val();
            objParam.Priority = $("._priorityDDL option:selected").val();
            objParam.Minutes = $("#ddlHours option:selected").val();
            objParam.ActionItemId = gblSelectedActionItem;
            objParam.ResponsibleUserId = self.ActionItem.ResponsibleUserId();
            objParam.IsSendEmailNotification = self.ActionItem.IsSendEmailNotification();
            objParam.IsArchived = self.ActionItem.IsArchived();
            objParam.RCAId = $("#RCAId").val();
            objParam.StatusDrop = $("#ddlStatus option:selected").val();
            objParam.ListActionItemResponsible = MergedResponsible;
            objParam.DocumentName = $('#hdnActionItemDocumentName').val();
            objParam.FileName = $('#hdnActionItemDocumentFile').val();
           
            EMPMGMT.Framework.Core.doPostOperation
                    (
                        controllerUrl + "SaveActionItemRecord",
                        objParam,
                        function onSuccess(response) {
                            if (response.ActionItemId == 0) {
                                $("#action-name-exist").css('display', 'block').text("Action Name already exists");
                            }

                            else {
                                self.ActionItemList.removeAll();
                                CKEDITOR.instances.editor1.editable().setHtml('');
                                self.GetActionItemFromActionList(1);
                               

                                $("#ManageGoal").modal("hide");

                            }

                            EMPMGMT.Framework.Common.ApplyPermission();
                        },
                        function onError(err) {
                            self.status(err.Message);
                        }
                    );
        }
        else {
            self.ActionItemErrorValidator.showAllMessages();
        }

    }
    self.RenderTableHeaders = function (progressDates) {
        debugger;
        self.TableHeaders.removeAll();
        self.TableHeaders.push(new EMPMGMT.User.ManageActionItem.tableHeaderViewModel("Action Title", "ItemName", self));
        self.TableHeaders.push(new EMPMGMT.User.ManageActionItem.tableHeaderViewModel("Responsible", "ResponsibleUserName", self));
        self.TableHeaders.push(new EMPMGMT.User.ManageActionItem.tableHeaderViewModel("Start Date", "StartDate", self));
        self.TableHeaders.push(new EMPMGMT.User.ManageActionItem.tableHeaderViewModel("End Date", "", self)); //Due Date<br/>ETA
        self.TableHeaders.push(new EMPMGMT.User.ManageActionItem.tableHeaderViewModel("Estimated Time", "Minutes", self));
        self.TableHeaders.push(new EMPMGMT.User.ManageActionItem.tableHeaderViewModel("Actual Time", "ActualTime", self));
        self.TableHeaders.push(new EMPMGMT.User.ManageActionItem.tableHeaderViewModel("Status", "Status", self));
        self.TableHeaders.push(new EMPMGMT.User.ManageActionItem.tableHeaderViewModel(progressDates, "", self));
        //self.TableHeaders.push(new EMPMGMT.User.ManageActionItem.tableHeaderViewModel("Action", "Action", self));
    }
    self.ClearActionDetailData = function (data) {
        self.ActionItem.ItemName('');
        self.ActionItem.Description('');
    }

    var startDate = new Date('01/01/2012');
    var FromEndDate = new Date();
    var ToEndDate = new Date();

    ToEndDate.setDate(ToEndDate.getDate() + 365);

    $('#startDate').datepicker({

        weekStart: 1,
        startDate: '01/01/2012',
        autoclose: true
    })
        .on('changeDate', function (selected) {
            startDate = new Date(selected.date.valueOf());
            //   startDate.setDate(startDate.getDate(new Date(selectedResponsibleUserAutoComplete1.date.valueOf())));
            $('#dueDate').datepicker('setStartDate', startDate);
        });
    $('#dueDate')
        .datepicker({

            weekStart: 1,
            startDate: startDate,
            endDate: ToEndDate,
            autoclose: true
        })
        .on('changeDate', function (selected) {
            FromEndDate = new Date(selected.date.valueOf());
            FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
            $('#startDate').datepicker('setEndDate', FromEndDate);
        });

    $('#EtaDate')
      .datepicker({

          weekStart: 1,
          startDate: startDate,
          endDate: ToEndDate,
          autoclose: true
      })
      .on('changeDate', function (selected) {
          FromEndDate = new Date(selected.date.valueOf());
          FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
          $('#startDate').datepicker('setEndDate', FromEndDate);
      });

    self.Sort = function (col) {
        if (col.ColumnName() != "Action") {

            ko.utils.arrayFirst(self.TableHeaders(), function (item) {
                if (item.ColumnName() != col.ColumnName() && item.ColumnName() != "") {
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

            self.KOSort(self.ActionItemList, orderby);
            ChildSort(self.ActionItemList, orderby);
        }
    }

    var ChildSort = function (actionitemlist, orderby) {
        ko.utils.arrayForEach(actionitemlist(), function (ai) {
            self.KOSort(ai.Children, orderby);
            ChildSort(ai.Children, orderby);
        });
    }

    self.SortActionItemChildren = function (ActionItemList) {
        $.each(ActionItemList, function (i, actionItem) {
            actionItem.SortChildren();
            self.SortActionItemChildren(actionItem.Children());
        });
    }

    self.ChildActionItemList = ko.observableArray([]);

    self.ActionItemList = ko.observableArray([]);
    self.SearchText = ko.observable('');
    
   
    self.GetActionItemFromActionList = function (currentPageNo) {
        debugger;
        var textpadding = 0;
        var objParam = new Object();
        objParam.ActionListId = $('#hdnActionListdId').val();
        objParam.RCAId = $('#RCAId').val();
        objParam.FetchChild = false;
        objParam.OnTime = chkOnTime.is(':checked');
        objParam.OverDue = chkOverDue.is(':checked');
        objParam.BeforeDue = chkBeforeDue.is(':checked');
        objParam.SearchText = self.SearchText();
        objParam.ResponsibleUserId = $('._ddlResponsible').val();
        objParam.StartDate = $('#startDatesearch').val();
        objParam.DueDate = $('#dueDatesearch').val();
        objParam.StatusDrop = $('#ddlStatusSearch').val();
      
        EMPMGMT.Framework.Core.doPostOperation
              (
                   controllerUrl + "GetActionItemListFromActionId",
                  objParam,
                  function onSuccess(response) { debugger;
                      self.ActionItemList.removeAll();
                      if (response.DataList.length > 0) {

                          if (response.DataList.length > 0) {
                              //self.ProgressDateHeader(response.DataList[0].ActionListMinStartDate + " - " + response.DataList[0].ActionListMaxDueDate);
                              if (response.DataList[0].ActionListMinStartDate == '' || response.DataList[0].ActionListMinStartDate == null || response.DataList[0].ActionListMinStartDate == undefined || response.DataList[0].ActionListMinStartDate.indexOf('-') > 0 || response.DataList[0].ActionListMaxDueDate.indexOf('-') > 0 || response.DataList[0].ActionListMaxDueDate == '' || response.DataList[0].ActionListMaxDueDate == null || response.DataList[0].ActionListMaxDueDate == undefined)
                              { }
                              else
                              self.ProgressDateHeader(dateFormat(common.ParsedJsonDate(response.DataList[0].ActionListMinStartDate), "mmm yyyy") + "  -  " + dateFormat(common.ParsedJsonDate(response.DataList[0].ActionListMaxDueDate), "mmm yyyy"));
                          }
                          ko.utils.arrayForEach(response.DataList, function (ActionItemData) {

                              self.ActionItemList.push(new EMPMGMT.User.ManageActionItem.ActionItemViewModel_N(ActionItemData, self, true, 0));
                              BindActionItemStatus(ActionItemData.ActionItemId, ActionItemData.ActionItemStatus);
                              BindStatusForChildren(ActionItemData.Children);
                              //    alert(self.ActionItem.Childs().length);
                          });
                          $('._ActionItemPager').hide();
                          var obj = new Object();
                          obj.ArchivedData = self.ArchivedData();
                          obj.ArchivedDataStatus = true;
                          if (self.ArchivedData()) {

                              self.clickArchivedData(obj)

                          }
                          EMPMGMT.Framework.Common.ApplyPermission();
                      }
                      else {

                          $('._ActionItemPager').show();
                          self.ProgressDateHeader("Progress Dates");
                      }

                      self.RenderTableHeaders(self.ProgressDateHeader());
                      self.IsInitialLoading(false);
                  },
                  function onError(err) {
                      self.status(err.Message);
                  }
              );


    }

    BindStatusForChildren = function (childActionItems) {
        if (childActionItems) {
            $.each(childActionItems, function (i, actionItem) {
                BindActionItemStatus(actionItem.ActionItemId, actionItem.ActionItemStatus);
                if (actionItem.Children && actionItem.Children.length > 0)
                    BindStatusForChildren(actionItem.Children);
            });
        }
    }

    self.RenderChildren = function (actionItemchildren, textpadding) {
        var showChild = false;
        var childArrary = ko.observableArray([]);
        if (actionItemchildren) {
            $.each(actionItemchildren, function (i, actionItem) {
                childArrary.push(new EMPMGMT.User.ManageActionItem.ActionItemViewModel_N(actionItem, self, false, textpadding));

            });
        }
        return childArrary;
    }

    //Get Hours
    self.GetHoursList = function () {
        EMPMGMT.Framework.Core.getJSONData(controllerUrl + "HoursList",
            function onSuccess(data) {
                $.each(data.list, function (index, element) {
                    $('._HourDDL').append("<option value='" + element.Text + "'>" + element.Text + "</option>");
                });
            }, function onError(err) {
                self.status(err.Message);
            });
    }


    function BindActionItemStatus(elementId, status) {
        elementId = CreateId(elementId);
        $('#' + elementId.toString()).progressCircle({
            nPercent: status,
            showPercentText: true,
            thickness: 7,
            circleSize: 35
        });

    }
    self.FilteredData = function () {

        self.GetActionItemFromActionList(CurrentPage);

    }

    self.GetPaging = function (Rowcount, currentPage, methodName, uniqueMethodName) {
        return EMPMGMT.Framework.Core.GetPagger(Rowcount, currentPage, methodName, uniqueMethodName);

    }



    // self.GetActionItemList(CurrentPage);
    self.DeleteActionItemAction = function (data) {

        var ret = confirm(EMPMGMT.Messages.ActionItem.DeleteActionItem);
        if (ret) {
            var PostData = new Object();
            PostData.ActionItemId = data.ActionItemId();

            EMPMGMT.Framework.Core.doPostOperation
                    (
                        controllerUrl + "DeleteActionItem",
                        PostData,
                        function onSuccess(response) {
                            self.ActionItemList.removeAll();
                            self.GetActionItemFromActionList(CurrentPage);
                            EMPMGMT.Framework.Common.ApplyPermission();
                            var obj = new Object();
                            obj.ArchivedData = self.ArchivedData();
                            obj.ArchivedDataStatus = true;
                            if (self.ArchivedData()) {
                                self.clickArchivedData(obj);
                            }
                        },
                        function onError(err) {
                            self.status(err.Message);
                        }
                    );
        }
    }
    self.ActionItem = new EMPMGMT.User.ManageActionItem.ActionItemViewModel_N(null, self, true, 0);
    var ExpandAll = function (data, isExpand) {
        ko.utils.arrayForEach(data.Children(), function (resp) {
            resp.IsExpandable(isExpand);
            if (resp.Children().length > 0) {
                resp.CollapseExpandImage("fa fa-minus-square"); //fa fa-plus-square
                ExpandAll(resp, false);
                resp.Collapsed(false);
            }
        });
    }
    self.ActionItemCollapseExpandClickAction = function (data) {
        if (data.IsExpandable() && !data.Collapsed()) {
            data.CollapseExpandImage("fa fa-minus-square");
            ExpandAll(data, true);
            data.Collapsed(true);
        }
        else if (data.IsExpandable() && data.Collapsed()) {
            data.CollapseExpandImage("fa fa-minus-square"); //fa - plus - square
            ExpandAll(data, false);
            data.Collapsed(false);
        }

    }

    self.Init = new function () {


        //ActionItemPopup = $("._ActionItemPopbox").popbox({
        //    close: '._ActionItemClose'
        //});

       
        self.GetHoursList();

        $("#startDate").datepicker({
            onClose: function (dateText, inst) {
                var date = $.datepicker.parseDate($.datepicker._defaults.dateFormat, dateText);
                $("#dueDate,#EtaDate").datepicker("option", "minDate", date);
            }


        });
        $('#dueDate,#EtaDate').datepicker().trigger('change');
        //self. ($('#hdnActionItemdId').val());

        autocompleteObj = $('#ResponsibleUserAutoComplete1').autocomplete({            
            serviceUrl: '/Employee/GetProjectResponsibles/' + $('#hdnProjectId').val(), //GetResponsibles
            displayProperty: 'FullName',
            
            onSearchStart: function () {
             
                MapOptions();
            },
            onChnage: function () {
                //debugger;
            },
            params: { "ResponsibleusersId": gblResponsibleUserIds },
            lookup: null,
            notToDisplayItemsComparisonkey: "UserId",
            onSelect: function (suggestion) {
                gblResponsibleUserIds += suggestion.data.UserId + ",";
                var objParam = new Object();
                objParam.ResponsibleUserId = suggestion.data.UserId;
                objParam.ResponsibleUserName = suggestion.value;

                self.ResponsibleTempList.push(new EMPMGMT.User.ManageActionItem.ResponsibleViewModel(objParam));
                autocompleteObj.options.notToDisplayItems.push(suggestion.data.UserId);

            },
        });

        $('#ResponsibleUserAutoComplete').autocomplete({

            serviceUrl: '/Employee/GetProjectResponsibles/'+ $('#hdnProjectId').val(), //GetResponsibles
            displayProperty: 'FullName',
            onChnage: function () {
                //debugger;
              //  self.ActionItem.ResponsibleUserId('');
              //  self.ActionItem.ResponsibleUserName('');

            },
            params: {},
            lookup: null,
            onSelect: function (suggestion) {
                self.ActionItem.ResponsibleUserId(suggestion.data.UserId);

                self.ActionItem.ResponsibleUserName(suggestion.value);

            },
        });

        //Slider Sataus Script.......................................................
        $("#slider2").slider({
            value: 0,
            min: 0,
            max: 100,
            step: 5,
            slide: function (event, ui) {
                $("#ActioItemStatus").val(ui.value + "%");
            }
        });
        $("#ActioItemStatus").val($("#slider2").slider("value") + "%");
        //End Slider Status Script...................................................
        self.GetActionItemFromActionList();
        self.GetResponsibleUsers();
    }


    //Upload Documents...................................


    var uploadObj = $("#fileupload").uploadFile({
        url: '/Employee/UploadActionItemDocument',
        // maxFileCount: 1,
        multiple: false,
        autoSubmit: false,
        // fileName: "docs",
        maxFileSize: 1024 * 10000,
        fileCounter: 0,
        onCancel: function (e) {
            uploadObj.selectedFiles = 0;
            e.perventDefault();
            return true;
        },
        dynamicFormData: function () {
            var PostData = new Object();
            PostData.ActionItemId_encrypted = gblSelectedActionItem;

            return PostData;

        },
        showStatusAfterSuccess: false,
        onSubmit: function (files) {
        },
        onSuccess: function (files, data, xhr) {
            var postData = new Object();
            postData.DocumentName = data.DataList.DocumentName;
            postData.DocumentId = data.DataList.DocumentId;
            postData.DocumentPath = data.DataList.Documentpath;
            postData.ActionItemPath = data.DataList.ActionItemPath;

            self.DashboardDocumentsList.push(new EMPMGMT.User.ManageActionItem.DocumentViewModel(postData));
            EMPMGMT.Framework.Common.ApplyPermission();
            $('#hdnActionItemDocumentFile').val(data.documentfileName);
            $('#hdnActionItemDocumentName').val(data.documentName);
        },
        afterUploadAll: function () {

        },
        onError: function (files, status, errMsg) {

        }
    });


    uploadObj.selectedFiles = 0;
    $("#ButtonUploadUser").bind("click", function () {
        if (uploadObj == null || uploadObj.selectedFiles == 0) {
            bootbox.alert("Please select any file")
        }
        else {
            uploadObj.startUpload();
            uploadObj.selectedFiles = 0;
        }
    });


    //End Of Upload Documents......................................
     
    self.ActionItemErrorValidator = ko.validation.group([self.ActionItem.ItemName, self.ResponsibleTempListError]); //self.ActionItem.StartDate, self.ActionItem.DueDate, 
}


function SliderClick(control) {

    var sliderwidth = ($('#slider2').css('width').replace('px', ''));
    var status = ($(control).css("left").replace('px', '') * 100) / sliderwidth;
    var sliderPercentage = (status * sliderwidth) / 100;
    $('#slider2 a').css('left', sliderPercentage);
    $('#ActioItemStatus').val(status + " %");

}

var PopupCenter = function (ths) {
    ths.css("position", "absolute");
    ths.css("top", Math.max(0, (($(window).height() - $(ths).outerHeight()) / 2) +
                                                $(window).scrollTop()) + "px");
    ths.css("left", Math.max(0, (($(window).width() - $(ths).outerWidth()) / 2) +
                                                $(window).scrollLeft()) + "px");
    return this;
}