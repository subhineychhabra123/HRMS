// include jQuery framework first.

// Namespace resolution
var PermissionArray = [];

var innerPageArray = [["OrganizationUnits"], ["profiles"], ["ManageUser"], ["ManageCategory"], ["MetricDashboard"], ["Home"], ["RegisteredUsers"], ["Metric"], ["ManageMetricDashboard"], ["Goals"],["Configuration"]]
var outerPageArray = [["Home"], ["home"], ["Register"], ["Features"], ["features"], ["Aboutus"], ["aboutus"], ["Contactus"], ["contactus"], ["Login"], ["MobileCRM"]]
jQuery.namespace = function () {
    var a = arguments, o = null, i, j, d;
    for (i = 0; i < a.length; i = i + 1) {
        d = a[i].split(".");
        o = window;
        for (j = 0; j < d.length; j = j + 1) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
};
//Register name space
jQuery.namespace('EMPMGMT.Framework.Core');
var PermissionArray = [];
var CultureSpecificDateFormat = "";
$(document).ready(function () {

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

    EMPMGMT.Framework.Core.Config.CultureSpecificDateFormat = "";
    $.ajaxSetup(
    {
        beforeSend: function () {
            EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Framework.Core.Config.ajaxProcessingText, false);
        },
        complete: function (jqXHR) {
            var errmessage = "";
            if (jqXHR.status == 200) {
                errmessage = EMPMGMT.Framework.Core.Config.ajaxProcessedText;
                //EMPMGMT.Framework.Core.ShowMessage(errmessage, false);
                setTimeout('$("._messagediv").css({ "height": "25px" }).slideUp("slow",function(){$(this).remove()})',1000);
                return;
            } else if (jqXHR.status == 404) {
                errmessage = "oops! Something went wrong. (Requested URL not found).";
            } else if (jqXHR.status == 500) {
                var response = $.parseJSON(jqXHR.responseText);
                errmessage = "oops! Something went wrong. (Internel Server Error Occurred).";
                //alert(response.ExceptionMessage);
            } else {
                errmessage = "oops! Something went wrong. (Unknown Error Occurred).";
            }
            EMPMGMT.Framework.Core.ShowMessage(errmessage, true);
            setTimeout('$("._messagediv").css({ "height": "25px" }).slideUp("slow",function(){$(this).remove()})', 6000);
        }
    });

    EMPMGMT.Framework.Core.ApplyCultureValidations = function () {

        $("[data-val]").each(function (i, e) {
            var $e = $(e);
            if ($("[data-valmsg-for='" + $e.attr("name") + "']").length == 0) {
                $($e.parent()).append('<span data-valmsg-replace="true" data-valmsg-for="' + $e.attr("name").replace(/_/g, ".") + '" class="field-validation-valid"></span>');
            }

            if (EMPMGMT.Messages != undefined) {
                // Set the validation messages specific to culture from culture specific JSON
                $(this).each(function (index, element) {

                    //loop through all type of model  validations like validation for  required, datatype specific etc.
                    $.each(element.attributes, function (inx, att) {
                        //if attribute 'data-val' found then it mean its for model validation
                        if (this.specified && this.name.indexOf('data-val') != -1) {
                            var jsonExpression;

                            if (this.name.indexOf('data-val-equalto-other') != -1) {
                                jsonExpression = "EMPMGMT.Messages.PasswordConfirmPasswordDoesNotMatches"

                            }
                            else if (this.name.indexOf('data-val-number') != -1) {

                                jsonExpression = "EMPMGMT.Messages.Lead.AmountNotValid"
                            }
                            else {
                                jsonExpression = "EMPMGMT.Messages." + this.value;

                            }




                            //skip the attribute having spaces in value
                            if (jsonExpression.indexOf(' ') < 0) {
                                var CultureSpecificValidationMessage = eval(jsonExpression);

                                //set the cultureSpecific  message in attribute 
                                if (CultureSpecificValidationMessage != undefined)
                                    $(element).attr(this.name, CultureSpecificValidationMessage);
                            }
                        }
                    });
                })
            }
        });
    }

    // EMPMGMT.Framework.Core.ApplyCultureValidations();
    var returnUrl = EMPMGMT.Framework.Common.GetUrlVars()["returnurl"];
    returnUrl = returnUrl == undefined || returnUrl == '' ? 'javascript:window.history.back()' : returnUrl;
    $("._back").attr("href", returnUrl);
    $("._backtohere").mouseenter(function () {
        var $this = $(this);
        if ($this.attr("href").contains('?')) {
            var href = $this.attr("href") + "&returnurl=" + window.location.href;
        } else {
            var href = $this.attr("href") + "?returnurl=" + window.location.href;
        }
        $this.attr("href", href);
        $this.unbind("mouseenter");
    });

    var inner = SelectMainMenu();
    if (inner != undefined) {
        $('ul li').removeClass('active');

        $('ul li a[menu=' + inner + ']').parent().addClass('active');
    }
    else {
        var outer = SelectOuterMenu();
        $('ul li a').removeClass('active');

        $('ul li a[menu=' + outer + ']').addClass('active');
        if (outer != undefined) {
            var imageUrl = "/content/images/Public_" + outer.toLowerCase() + ".png";
        }
        else { var imageUrl = ''; }
        $("#LayoutHome_Outer").css('background-image', 'url(' + imageUrl + ')');
    }
    if (EMPMGMT.Framework.Common) {
        var d = new Date();
        var v = d.getTime() + "|" + EMPMGMT.Framework.Common.GetCookie('Permissions');
        PermissionArray = v.split('|')
        for (var i = 0; i < PermissionArray.length; i++) {
            {
                PermissionArray[i] = PermissionArray[i].toString().toLowerCase();
            }
        }
        EMPMGMT.Framework.Common.ApplyPermission();
    }


    function OneClickSubmitButton() {
        $('._one-click-submit-button').each(function () {
            var buttonType = $(this).prop('type');
            if (buttonType.toLowerCase() != 'submit') return;
            var $theButton = $(this);
            var $theForm = $theButton.closest('form');

            //disabled the button and submit the form
            function tieButtonToForm() {
                $theButton.one('click', function () {
                    $theButton.prop('disabled', true);
                    $theForm.submit();
                });
            }

            tieButtonToForm();

            // This handler will re-wire the event when the form is invalid.
            $theForm.submit(function (event) {
                if (!$(this).valid()) {
                    $theButton.prop('disabled', false);
                    event.preventDefault();
                    tieButtonToForm();
                }
            });
        });
    }

    $(document).click(function (e) {
        if ($(e.target).closest('#SideMenu-Configuration').length != 0) return false;
        $('._dropdownMenu').hide(500);

    });
    function GetMenuStatus() {
        var menuOpen = EMPMGMT.Framework.Common.GetCookie("MenuStatus");
        if (menuOpen == "")
            EMPMGMT.Framework.Common.SetCookie("MenuStatus", EMPMGMT.Framework.Core.Config.MenuStatus.Close, 365);
        menuOpen = EMPMGMT.Framework.Common.GetCookie("MenuStatus");
        if (menuOpen == EMPMGMT.Framework.Core.Config.MenuStatus.Open) {          
            $("html").removeClass("sidebar-left-collapsed");
           // $("html").removeClass("nav-parent nav-expanded");
        }
        else {
            $("html").addClass("sidebar-left-collapsed");
          //  $("html").addClass("nav-parent nav-expanded");
        }
    }
    $(".sidebar-toggle").click(function () {
     
        if ($("html").hasClass("sidebar-left-collapsed"))
            EMPMGMT.Framework.Common.SetCookie("MenuStatus", EMPMGMT.Framework.Core.Config.MenuStatus.Open, 365);
        else
            EMPMGMT.Framework.Common.SetCookie("MenuStatus", EMPMGMT.Framework.Core.Config.MenuStatus.Close, 365);
    });

   
    GetMenuStatus();
    OneClickSubmitButton();



  
});



var SelectMainMenu = function () {
    var pathArray
    if (window.location.href.toString().indexOf("?") > 0) {
        pathArray = window.location.href.split(/[\/?]+/)

    }
    else {
        pathArray = window.location.href.split('/')

    }
    var returnvalue = false;

    for (i = 0; i < innerPageArray.length; i++) {

        var v = innerPageArray[i];

        for (j = 0; j < v.length; j++) {

            if (jQuery.inArray(v[j], pathArray) > 0) {
                returnvalue = true;

                return v[0];

            }
        }
    }
}

var SelectOuterMenu = function () {

    var pathArray = window.location.href.split('/')

    if (pathArray[3] == "") {
        return "Home";
    }
    for (i = 0; i < outerPageArray.length; i++) {
        var v = outerPageArray[i];

        for (j = 0; j < v.length; j++) {

            if (jQuery.inArray(v[j], pathArray) > 0) {

                return v[0];

            }
        }
    }

}
var CheckStatusCodeToPerformAction = function (response) {
    //"StatusCode":401
    var statusCode = response.StatusCode;
    if (statusCode != undefined && statusCode != null) {
        if (statusCode.toString() == "401") {
            location.href = "/login";
        }

    }

}
EMPMGMT.Framework.Core.Config = {
    APIBaseUrl: "",
    PageSize: 10,
    ajaxProcessingText: "Processing....",
    ajaxProcessedText: "Loaded",
    CultureSpecificDateFormat: "",
    UserStatus: {
        Active: 1,
        Deactive: 2,
        Pending: 3,
        Moreinfo: 4,
        Invited: 5,
        Expired: 6,
        AllUsers: 25
    },
    MenuStatus: {
        Close: 0,
        Open: 1
    },
    SubMenuStatus: {
        Close: 0,
        Open: 1
    }




    //$("#" + dateElement).datepicker({ dateFormat: '@Html.jQueryDatePickerFormat()' });

}



EMPMGMT.Framework.Core.SetGlobalDatepicker = function (dateElement) {

    $("#" + dateElement).datepicker();


}
EMPMGMT.Framework.Core.SetCulture = function (cultureName) {



    //document.cookie = "Culture=" + $(cultureName).attr("Culture") + ";{ expires: 7 }";
    document.cookie = "Culture=" + cultureName + ";{ expires: 7 }";
    window.location.reload()
    // window.location.href = window.location.href + "?reload=true";

}
//Web api - Http get operation - Data fetch
EMPMGMT.Framework.Core.getJSONData = function (url, successCallBack, failureCallBack) {
    // alert('Ho');
    $.ajaxSetup({ cache: false });
    $.getJSON(this.Config.APIBaseUrl + url)

    .success(function (data) {
        CheckStatusCodeToPerformAction(data);
        successCallBack(data);

    })
        .error(function OnError(xhr, textStatus, err) {

            if (failureCallBack != null) {
                var obj = jQuery.parseJSON(xhr.responseText);
                var errObj = new Object();
                errObj.Message = obj.Message;
                errObj.status = xhr.status;
                errObj.statusText = xhr.statusText;
                failureCallBack(errObj)

            };
        });
};

//Web api - Http get operation - Data fetch
EMPMGMT.Framework.Core.getJSONDataBySearchParam = function (url, object, successCallBack, failureCallBack, beforeSendCallBack, onCompleteCallBack) {



    $.ajax({
        url: this.Config.APIBaseUrl + url,
        cache: false,
        type: 'GET',
        data: object,
        beforeSend: beforeSendCallBack == undefined ? undefined : beforeSendCallBack,
        complete: onCompleteCallBack == undefined ? undefined : onCompleteCallBack
    })
     .success(function (data) {
         CheckStatusCodeToPerformAction(data); successCallBack(data);


     })
     .error(function OnError(xhr, textStatus, err) {
         if (failureCallBack != null) {
             var obj = jQuery.parseJSON(xhr.responseText);
             var errObj = new Object();
             errObj.Message = obj.Message;

             if (obj.ModelState != null)
                 errObj.ModelState = obj.ModelState;

             errObj.status = xhr.status;
             errObj.statusText = xhr.statusText;
             failureCallBack(errObj)
         }
     });
};

// Web api - Http put operation - record update
EMPMGMT.Framework.Core.doPutOperation = function (url, object, successCallBack, failureCallBack) {
    $.ajax({
        url: this.Config.APIBaseUrl + url,
        cache: false,
        type: 'PUT',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(object)
    })
    .success(function (data) { CheckStatusCodeToPerformAction(data); successCallBack(data); })
    .error(function OnError(xhr, textStatus, err) {

        if (failureCallBack != null) {
            var obj = jQuery.parseJSON(xhr.responseText);
            var errObj = new Object();
            errObj.Message = obj.Message;

            if (obj.ModelState != null)
                errObj.ModelState = obj.ModelState;

            errObj.status = xhr.status;
            errObj.statusText = xhr.statusText;
            failureCallBack(errObj)
        }
    });
};

// Web api - Http post operation - create record
EMPMGMT.Framework.Core.doPostOperation = function (url, object, successCallBack, failureCallBack) {
    $.ajax({
        url: this.Config.APIBaseUrl + url,
        cache: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(object),
        statusCode: {
            200 /*Created*/: function (data) {
               

            }
        }
    })
        .success(function (data) {          

            if (data.StatusCode != undefined && data.StatusCode != null && data.StatusCode == 401) {
                location.href = "/login";
            }
            CheckStatusCodeToPerformAction(data);
            successCallBack(data, object)
        })
      .error(function OnError(xhr, textStatus, err) {

          if (failureCallBack != null) {
              var obj = jQuery.parseJSON(xhr.responseText);
              var errObj = new Object();
              errObj.Message = obj.Message;

              if (obj.ModelState != null)
                  errObj.ModelState = obj.ModelState;

              errObj.status = xhr.status;
              errObj.statusText = xhr.statusText;
              failureCallBack(errObj)
          }
      });


};

// Web api - Http delete operation - delete a record
EMPMGMT.Framework.Core.doDeleteOperation = function (url, object, successCallBack, failureCallBack) {


    $.ajax({


        url: this.Config.APIBaseUrl + url,
        cache: false,
        type: 'DELETE',
        data: JSON.stringify(object),
        contentType: 'application/json; charset=utf-8'
    })
    .success(function (data) { CheckStatusCodeToPerformAction(data); successCallBack(data); })
    .fail(
        function (xhr, textStatus, err) {
            if (failureCallBack != null)
                failureCallBack(xhr, textStatus, err);
        });
};

EMPMGMT.Framework.Core.doEditOperation = function (url, object, successCallBack, failureCallBack) {


    $.ajax({


        url: this.Config.APIBaseUrl + url,
        cache: false,
        type: 'DELETE',
        data: JSON.stringify(object),
        contentType: 'application/json; charset=utf-8'

    })
    .success(function (data) { CheckStatusCodeToPerformAction(data); successCallBack(data); })
    .fail(
        function (xhr, textStatus, err) {
            if (failureCallBack != null)
                failureCallBack(xhr, textStatus, err);
        });
};

// Validation message
EMPMGMT.Framework.Core.showErrors = function (submitForm, err) {
    var validator = $("#" + submitForm).validate();
    var iCnt = 0;
    errors = [];
    $.each(err.ModelState, function (key, value) {
        var pieces = key.split('.');
        key = pieces[pieces.length - 1];
        errors[key] = value[0];

    });
    //var validator = $("#frmUserCreate").validate();
    //validator.showErrors({
    //    "FirstName": "I know that your firstname is Pete, Pete!"
    //});
    validator.showErrors(errors);
}

EMPMGMT.Framework.Core.ShowMessage = function (msg, iserror, removeOtherMessages) {

  
    if (msg == EMPMGMT.Framework.Core.Config.ajaxProcessedText && $('div._messagediv:not(:contains(' + EMPMGMT.Framework.Core.Config.ajaxProcessingText + '))').length > 0) {
        return;
    }
    var messageBoxid = "messagediv" + Math.round(Math.random() * 1000);
    if ($("#" + messageBoxid).length == 0)
    {
        if (removeOtherMessages != false) {
            $("._messagediv").remove();
        }
        var message = $("<div id='" + messageBoxid + "' class='_messagediv messagediv displaynone'><div class='statusMessage rb-a-4 _status'><span class='_message'></span></div></div>");
        $(document.body).append(message);
    }
    if (iserror != undefined && iserror == false) {
        $("#" + messageBoxid + " ._status").removeClass("error").addClass("message");
    }
    else {
        $("#" + messageBoxid + " ._status").removeClass("message").addClass("error");
       
    }
    var width = $(window).width() - $("#" + messageBoxid + "").width();

    $("#" + messageBoxid + " ._message").html(msg);

    $("#" + messageBoxid).slideDown("slow", function () { $(this).css({ "height": "1px" }) });
    if (iserror) {

        setTimeout('$("#MessageDiv").css({ "height": "25px" }).slideUp("slow",function(){$(this).remove()})',8000);
       
    }

    else {
        setTimeout('$("#MessageDiv").css({ "height": "25px" }).slideUp("slow",function(){$(this).remove()})', 3000);
    }
}

EMPMGMT.Framework.Core.ShowMessage_uploaduser = function (msg, iserror, removeOtherMessages) {

    $('#sucessmsg').html('');
    $('#sucessmsg').show();
    $('#sucessmsg').html(msg);

}

EMPMGMT.Framework.Core.GetPagger = function (TotalRecords, currentPage, methodName, uniquePagerId) {
    currentPage = currentPage == undefined || parseInt(currentPage) <= 0 ? 1 : currentPage;
    var totalRecords = TotalRecords;
    var pageSize = EMPMGMT.Framework.Core.Config.PageSize;

    if (methodName.indexOf('.') > 0) {// check for if already passed (view model + methodname) example MyViewModel.MyMethodName (By Rakesh Rana)
        methodName = methodName;
    }
    else {
        methodName = "viewModel." + methodName;
    }
    var totalPages = parseInt(totalRecords / pageSize);

    if (totalRecords % pageSize > 0) {
        totalPages = totalPages + 1;
    }
    if (uniquePagerId == undefined || uniquePagerId == null) {
        uniquePagerId = "divPager";
    }
    if (totalPages <= 1) {
        $("#" + uniquePagerId).html('');
        $('#showallLess').hide();
        return false;
    }

    var sbPagger = '';
    sbPagger = sbPagger + "<div id='" + uniquePagerId + "' class='divPager pagination'>";
    if (totalPages > 5) {
        if (currentPage == 1) {
            sbPagger = sbPagger + "<li class='active'><a class='_selectedPage selectedGridPage'>First</a></li>";
        }
        else {
            sbPagger = sbPagger + "<li><a pageno='1' StartRowIndex='1' href='javascript:" + methodName + "(1);'>First</a></li>";
        }
    }
    var index = currentPage - 3;
    var tmppage = 0;
    if (index < 0) {
        tmppage = index;
        index = 0;
    }
    var tmptotalPages = currentPage + 2 - (tmppage);
    if (tmptotalPages < 5) {
        tmptotalPages = 5;
    }
    if (tmptotalPages > totalPages) {
        index -= (tmptotalPages - totalPages);
        tmptotalPages = totalPages;
    }
    if (index < 0) {
        index = 0;
    }
    for (; index < tmptotalPages; index++) {
        var pageNo = index + 1;

        if (currentPage == pageNo)
            sbPagger = sbPagger + "<li class='active'><a class='_selectedPage selectedGridPage'>" + pageNo + "</a></li>";
        else {
            sbPagger = sbPagger + "<li><a pageno='" + pageNo + "' StartRowIndex='" + ((pageSize * index) + 1)
            + "'href='javascript:" + methodName + "(" + pageNo + ");' " + (currentPage == pageNo ? "class='divpager'" : "") + ">" + pageNo + "</a></li>&nbsp;";
        }
    }
    if (totalPages > 5) {

        if (currentPage == tmptotalPages) {
            sbPagger = sbPagger + "<li class='active'><a class='_selectedPage selectedGridPage' >Last</a></li>;";
        }
        else {
            sbPagger = sbPagger + "<li><a pageno='" + tmptotalPages + "' StartRowIndex='" + ((pageSize * (tmptotalPages - 1)) + 1) + "' href='javascript:" + methodName + "(" + parseInt(totalPages) + ");'>Last</a></li>";
        }
    }
    sbPagger = sbPagger + "</div>";

    return sbPagger;
}

EMPMGMT.Framework.Core.OpenRolePopup = function (opendivSelector) {

    var hiddenSection = $(opendivSelector);
    $(opendivSelector + " .error").text('').removeClass("msgssuccess");
    hiddenSection.fadeIn("fast")
        .css({ 'display': 'block' })
        .css({ width: $(window).width() + 'px', height: $(window).height() + 'px' })
        .css({
            top: ($(window).height() - hiddenSection.height()) / 2 + 'px',
            left: ($(window).width() - hiddenSection.width()) / 2 + 'px'
        })
        .css({ 'background-color': 'rgba(0,0,0,0.5)' })
        .appendTo('body');
    $(opendivSelector + ' ._close').click(function () { $(hiddenSection).fadeOut("fast"); });

}
ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var $el = $(element);

        //initialize datepicker with some optional options
        var options = allBindingsAccessor().datepickerOptions || {};
        $el.datepicker(options);
        $.datepicker.setDefaults({ dateFormat: 'dd/mm/yy' });
        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            observable($el.datepicker("getDate"));

        });

        //handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $el.datepicker("destroy");
        });

    },
    update: function (element, valueAccessor) {

        var value = ko.utils.unwrapObservable(valueAccessor()),
            $el = $(element),
            current = $el.datepicker("getDate");
        if (String(value).indexOf('/Date(') == 0) {
            value = new Date(parseInt(value.replace(/\/Date\((.*?)\)\//gi, "$1")));
            var observable = valueAccessor();
            observable(value);
        }

        if (value - current !== 0) {
            $el.datepicker("setDate", value);
            $("#ui-datepicker-div").css({ "display": "none" });
            //$el.val(value);
        }
    }
};