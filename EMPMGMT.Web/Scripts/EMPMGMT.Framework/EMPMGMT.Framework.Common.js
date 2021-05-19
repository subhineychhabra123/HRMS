// include jQuery framework first.

// Namespace resolution
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
jQuery.namespace('EMPMGMT.Framework.Common');

//var didScroll;
//var lastScrollTop = 0;
//var delta = 5;
//var navbarHeight = $('header').outerHeight();

//$(window).scroll(function(event){
//    didScroll = true;
//});

//setInterval(function() {
//    if (didScroll) {
//        hasScrolled();
//        didScroll = false;
//    }
//}, 250);

//function hasScrolled() {
//    var st = $(this).scrollTop();

//    // Make sure they scroll more than delta
//    if(Math.abs(lastScrollTop - st) <= delta)
//        return;

//    // If they scrolled down and are past the navbar, add class .nav-up.
//    // This is necessary so you never see what is "behind" the navbar.
//    if (st > navbarHeight){
//        // Scroll Down
//        $('header').removeClass('nav-up').addClass('nav-down');
//    } else {
//        // Scroll Up
//        if(st + $(window).height() < $(document).height()) {
//            $('header').removeClass('nav-down').addClass('nav-up');
//        }
//    }

//    lastScrollTop = st;
//}

//EMPMGMT.Framework.Common.OpenPopup = function (opendivSelector) {
//    var hiddenSection = $(opendivSelector);

//    $(opendivSelector + " .error").text('').removeClass("msgssuccess");
//    hiddenSection.fadeIn("fast")
//        .css({ 'display': 'block' })
//        .css({ width: $(window).width() + 'px', height: $(window).height() + 'px' })
//        .css({
//            top: ($(window).height() - hiddenSection.height()) / 2 + 'px',
//            left: ($(window).width() - hiddenSection.width()) / 2 + 'px'
//        })
//        //.css({ "width": "100%", "height": "100%", "position": "fixed", "overflow": "hidden","top":"0" })
//        .css({ 'background-color': 'rgba(0,0,0,0.5)' })
//        .appendTo('body');
//    $(opendivSelector + ' ._close').click(function () { $(hiddenSection).fadeOut("fast"); });
//}

EMPMGMT.Framework.Common.Split = function (val) {
    return val.split(/,\s*/);
}

EMPMGMT.Framework.Common.ExtractLast = function (term) {
    return EMPMGMT.Framework.Common.Split(term).pop();
}

if (!('filter' in Array.prototype)) {
    Array.prototype.filter = function (filter, that /*opt*/) {
        var other = [], v;
        for (var i = 0, n = this.length; i < n; i++)
            if (i in this && filter.call(that, v = this[i], i, this))
                other.push(v);
        return other;
    };
}

EMPMGMT.Framework.Common.ActionColumnVisibility = function () {
    if ($('._tablelist .actionLinks').has(":visible").length == 0) {
        $(".actionLink,.actionLinks").addClass('displaynone');
    }
    else { $(".actionLink,.actionLinks").removeClass('displaynone'); }
}

EMPMGMT.Framework.Common.ApplyPermission = function (selector) {
    if (selector != undefined) {
        $(selector + ' [data-permission]').each(function () {

            if ($.inArray($(this).attr('data-permission').toLowerCase(), PermissionArray) >= 0) {



                $(this).removeClass('permissionbased');
               var title= $(this).hasAttr('data-permissionbasedtext');
                if (title) {
                    $(this).addClass('inlinepermissiongranted')
                    $(this).attr('href', "javascript:void(0)");
                    $(this).attr('click', "javascript:void(0)");
                }
                //$(this).addClass('permissiongranted')
            }
        });
    }
    else {
        $('[data-permission]').each(function () {
            if ($.inArray($(this).attr('data-permission').toLowerCase(), PermissionArray) >= 0) {
                $(this).removeClass('permissionbased');
                //$(this).addClass('permissiongranted')
            }
            var title = $(this).hasAttr('data-permissionbasedtext');
            if (title) {
                $(this).addClass('inlinepermissiongranted')
                $(this).attr('href', "javascript:void(0)");
                $(this).attr('click', "javascript:void(0)");
            }

        });

        $(".menuOption").each(function () {
            if ($(this).find("li:visible").length == 0) {
                $(this).hide();
            }
        });
        
        for (var trackLevel = 3; trackLevel != 0; trackLevel--) {
            $(".menuOption" + trackLevel).each(function () {
                if ($(this).find("li:visible").length == 0) {
                    $(this).hide();
                }
            });
        }
        //if ($(".permissionConfiguration").find("li:visible").length == 0) {
        //    $(".permissionConfiguration").hide();
        //}

    }
    EMPMGMT.Framework.Common.ActionColumnVisibility();
}


//Code added for implementing permission on ui based on cookie
EMPMGMT.Framework.Common.GetCookie = function (cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].split(' ').join('');
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}
//Set Cookies
EMPMGMT.Framework.Common.SetCookie = function (cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}

EMPMGMT.Framework.Common.CookieExists = function (permissionname) {
     if ($.inArray(permissionname.toLowerCase(), PermissionArray) >= 0) {
        return true;
    }
    else
        return false;
}


EMPMGMT.Framework.Common.ConvertMonthsToCultureSpecific = function (monthArray) {
    var dashboard = "EMPMGMT.Messages.DashBoard."
    for (i = 0; i < monthArray.length; i++) {
        monthArray[i] = eval(dashboard + monthArray[i]);

    }
    return monthArray;

}


EMPMGMT.Framework.Common.ConvertWeekToCultureSpecific = function (weekArray) {
    var dashboard = "EMPMGMT.Messages.DashBoard."
    for (i = 0; i < weekArray.length; i++) {
        weekArray[i] = eval(dashboard + weekArray[i]);

    }

    return weekArray;

}

EMPMGMT.Framework.Common.GetUrlVars = function () {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {

        hash = hashes[i].split('=');
        vars.push(hash[0]);
        if (hash.length % 3 == 0) {
            vars[hash[0]] = hash[1] + '=';;
        }
        else { vars[hash[0]] = hash[1]; }
    }
    return vars;
}



function CreateId(elementId) {
    var prefix = "el";
    elementId = elementId.replace("=", "e");
    elementId = elementId.replace("^", "st");
    elementId = elementId.replace(/["~!@#$%^&*\(\)_+=`{}\[\]\|\\:;'<>,.\/?"\- \t\r\n]+/g, '-');
    return prefix + elementId;
}


$.fn.hasAttr = function (name) {
    return this.attr(name) !== undefined;
};


EMPMGMT.Framework.Common.ParsedJsonDate = function (date) {
return new Date(parseInt(date.substr(6)))
}
