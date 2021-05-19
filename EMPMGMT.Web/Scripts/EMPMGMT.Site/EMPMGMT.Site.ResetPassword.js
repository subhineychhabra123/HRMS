//Register name space
jQuery.namespace('EMPMGMT.Site.ResetPassword');
var viewModel;
var controllerUrl = "/";
var Message = {
    Failure: 'Failure',
    Success: 'Success'
}
EMPMGMT.Site.ResetPassword.pageLoad = function () {
    viewModel = new EMPMGMT.Site.ResetPassword.pageViewModel();
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
EMPMGMT.Site.ResetPassword.pageViewModel = function () {
    var self = this;
    var userId = $('#UserId').val();
    self.password = ko.observable().extend({  // custom message
        required: { message: 'Please enter your Password.' }
    });

    self.cpassword = ko.observable().extend({  // custom message
        required: { message: 'Please enter  confirm Password.' }
    });

    //***************On Submit******************//

    self.submit = function (data)
    {
        if (self.errors().length == 0)
        {
            var postObj = new Object();
            postObj.password = JSON.parse(ko.toJSON(data.password()));
            postObj.cpassword = JSON.parse(ko.toJSON(data.cpassword()));
            postObj.UserId = userId;
            if (postObj.password != postObj.cpassword)
            {
                var msg="Password does not matched";
                EMPMGMT.Framework.Core.ShowMessage(msg, true);
            }

            else
            {
                if (postObj.password.length < 6) {
                    EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Password.ResetPasswordLength, true);

                    }
            else
                {
                EMPMGMT.Framework.Core.doPostOperation
                     (
                         controllerUrl + "ResetPassword",
                         postObj,
                         function onSuccess(response) {
                             if (response == "Successful") {
                                 EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Password.ResetPasswordSuccess,false);

                                 setTimeout(function () {

                                     location.href="/login"

                                 },2000);
                            
                             }
                             else {
                            
                                 EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Password.InvalidUser,false);
                             }
                         },
                         function onError(err) {

                             EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Password.InvalidUser, false);

                         }
                     );
            }

        }
        }



        else {

            EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Password.InvalidUser, false);
        }
    };

    //***************END******************//

    //***************Validation Error******************//

    self.errors = ko.validation.group([self.password, self.cpassword]);

    //***************END******************//



}






