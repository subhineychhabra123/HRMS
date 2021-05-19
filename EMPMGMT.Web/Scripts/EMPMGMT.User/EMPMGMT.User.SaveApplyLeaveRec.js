jQuery.namespace('EMPMGMT.User.SaveApplyLeaveRec');
var controllerUrl = "/Employee/";

var common = EMPMGMT.Framework.Common;
var config = EMPMGMT.Framework.Core.Config;
EMPMGMT.User.SaveApplyLeaveRec.pageLoad = function () {
    var viewModelSaveApplyLeaveRec = new EMPMGMT.User.SaveApplyLeaveRec.pageViewModel();

    ko.applyBindings(viewModelSaveApplyLeaveRec);
}
var  SaveLeaveItemRecord = function () {

    $.ajax({
        url: '/Employee/SaveLeaveItemRecord',
        type: 'post',
        dataType: 'json',
        data: ko.toJSON(this),
        contentType: 'application/json',
        success: function (result) {
        },
        error: function (err) {

        },
        complete: function () {

        }
    });
};

EMPMGMT.User.SaveApplyLeaveRec.pageViewModel = function () {
    
    var self = this;
   
    self.FetchLeaveType = ko.observableArray();
    self.FullDay = ko.observable();
    self.FromDate = ko.observable();
    self.ToDate = ko.observable();
    self.Reason = ko.observable();
    self.FirstName = ko.observable();
    self.UserName = ko.observable();
    self.UserEmail = ko.observable();
    self.HalfDay = ko.observable(false);

    self.FirstHalf = ko.observable(false);
    self.FromDateForFirstHalf = ko.observable();
    self.ReasonForFirstHalf = ko.observable();

    self.SecondHalf = ko.observable(false);
    self.FromDateForSecondHalf = ko.observable();
    self.ReasonForSecondHalf = ko.observable();

    self.shortleave = ko.observable(false);
    self.FromDateForshortleave = ko.observable();
    self.ReasonForshortleave = ko.observable();

    self.getleaveType = function () {

        $.ajax({
            url: '/Employee/GetAllLeaveType',
            type: 'Get',
            dataType: 'json',
            contentType: 'application/json',
            success: function (result) {
                debugger

                self.FetchLeaveType = result;

            },
            error: function (err) {

            },
            complete: function () {

            }
        });
    };
    
   


   
    self.getleaveType();
    
}