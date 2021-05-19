jQuery.namespace('EMPMGMT.Admin.ViewAdminProfile');
var viewModelUserProfile;
var controllerUrl = "/Admin/";
var Message = {
    Failure: 'Failure',
    Success: 'Success'
}
EMPMGMT.Admin.ViewAdminProfile.pageLoad = function () {
    viewModelUserProfile = new EMPMGMT.Admin.ViewAdminProfile.pageViewModel();
    ko.applyBindings(viewModelUserProfile);
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
EMPMGMT.Admin.ViewAdminProfile.userProfileViewModel = function (user) {
    var self = this;
    var password = '', confirmPassword = '', newPassword = '';
    if (user != undefined) {
        password = user.Password;
        confirmPassword = user.ConfirmPassword;
        newPassword = user.NewPassword;
    }
    self.Password = ko.observable(password);
    self.ConfirmPassword = ko.observable(confirmPassword);
    self.NewPassword = ko.observable(newPassword);
    return self;
}
EMPMGMT.Admin.ViewAdminProfile.pageViewModel = function () {
    //Class variables
    var self = this;
    //******************Reset Password PopUp functionality *************************************//
    self.Password = ko.observable('').extend({  // custom message
        required: {
            message: 'Password is required'
        }
    });

    self.NewPassword = ko.observable('').extend({  // custom message
        required: {
            message: 'New password is required'
        }
    });
    self.ConfirmPassword = ko.observable('').extend({  // custom message
        required: {
            message: 'Confirm password is required'
        }
    });

    $("._resetPasswordPopbox").popbox({
        open: '._resetPasswordOpen',
        arrow: '.arrow',
        arrow_border: '.arrow-border',
        close: '._resetPasswordClose,._resetConfigurationClose',
        overlay: '._popup_overlay'
    });



    //*****************************End*********************************************//
    self.CancelResetPassword = function () {
        $('.statusMessage ').hide();
    }

    //****************************Reset Passord Click functionality ********************//
    self.ResetYourPassword = function () {
        if (self.pswerrors().length == 0) {
            if (self.NewPassword().length < 6) {
                EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Password.PasswordLengthMessage, true);
                //self.NewPassword('');
                //self.ConfirmPassword('');
            }
          else  if (self.NewPassword() != self.ConfirmPassword()) {
                self.ConfirmPassword('');
                self.NewPassword('');
                EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Password.PasswordMismatch, true);

            }
            else {
                var PostData = new Object();
                PostData.Password = self.Password();
                PostData.NewPassword = self.NewPassword();
                EMPMGMT.Framework.Core.doPostOperation
                       (
                           controllerUrl + "ResetPassword",
                           PostData,
                           function onSuccess(response) {
                               if (response == "Successful") {
                                   $("._resetPasswordClose").click();

                                   EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Password.ResetPasswordSuccess, false);
                                   self.Password('');
                                   self.ConfirmPassword('');
                                   self.NewPassword('');
                               }
                               else if (response == "IncorrectPassword") {
                                   EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Password.IncorrectPassword, true);
                               }
                               else {
                                   EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Password.InvalidUser, true);
                               }


                           },
                           function onError(err) {
                               self.status(err.Message);
                           }
                       );
            }
        } else {
            self.pswerrors.showAllMessages();
        }

    }


    //******************************End ******************************************//
    self.ResetPasswordScreen = function () {
       
        self.Password('');
        self.ConfirmPassword('');
        self.NewPassword('');
    }

    self.errors = ko.validation.group([self.txtFirstName]);
    self.pswerrors = ko.validation.group([self.NewPassword, self.Password, self.ConfirmPassword]);
}