//Register name space
jQuery.namespace('EMPMGMT.Site.ForgotPassword');
var viewModel;
var controllerUrl = "/";
var Message = {
    Failure: 'Failure',
    Success: 'Success'
}
EMPMGMT.Site.ForgotPassword.pageLoad = function () {
    viewModel = new EMPMGMT.Site.ForgotPassword.pageViewModel();
    ko.applyBindings(viewModel);
}
ko.validation.rules.pattern.message = 'Invalid.';
ko.validation.configure({
    registerExtenders: true,
    messagesOnModified: true,
    insertMessages: true,
    parseInputAttributes: true,
    messageTemplate: null
});
EMPMGMT.Site.ForgotPassword.pageViewModel = function () {
    var self = this;
    self.EmailAddress = ko.observable().extend({  // custom message
        required: { message: 'Please enter your email address.' },
        email: { message: 'Please enter valid email address' }
    });
    self.submit = function (data) {
       
       
        if (self.errors().length == 0) {
            var postObj = new Object();
            postObj.EmailId = JSON.parse(ko.toJSON(data.EmailAddress()));
            EMPMGMT.Framework.Core.doPostOperation
                 (
                     controllerUrl + "ForgotPassword",
                     postObj,
                     function onSuccess(response) {                        
                        
                         if (response.Status == Message.Failure) {
                             EMPMGMT.Framework.Core.ShowMessage(response.Message, true);
                         }
                         else if (response.Status == Message.Success) {
                            
                             EMPMGMT.Framework.Core.ShowMessage(response.Message, false);
                             setTimeout(function () {

                                 location.href = "/login"

                             }, 1000);
                         }
                        
                     },
                     function onError(err) {

                        // self.status(err.Message);
                     }
                 );
        } else {           
            self.errors.showAllMessages();
        }
    };
    self.errors = ko.validation.group(self);
}






