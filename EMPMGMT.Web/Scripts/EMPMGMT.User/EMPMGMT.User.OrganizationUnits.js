//Register name space
jQuery.namespace('EMPMGMT.User.OrganizationUnits');
var viewModel;
var draggedUnitId = '';
var draggedUnitName = '';
var draggedParentUnitId = '';
var parent = '';
EMPMGMT.User.OrganizationUnits.pageLoad = function () {
    $(".rolelistcontainer").width($(window).width() - 75);
    //$(".rolelistcontainer").height($(window).height() - 200);
    $(".rolelistcontainer").css("overflow", "auto");
    viewModel = new EMPMGMT.User.OrganizationUnits.pageViewModel();
    ko.applyBindings(viewModel);
}
// A view model that represent a Test report query model.
EMPMGMT.User.OrganizationUnits.rolesViewModel = function (data) {

    var self = this;
    self.OrgUnitId = data.OrgUnitId;
    self.ParentOrgUnitId = data.ParentOrgUnitId;
    self.OrgUnitName = data.OrgUnitName;
    self.IsDefaultUnit = data.IsDefaultUnit;
    self.CompanyId = data.CompanyId;
    //self.Url = "EditProfileType/" + data.ProfileId;
    self.Visible = data.IsDefaultForRegisterdUser == true ? false : true;
}



//Page view model
EMPMGMT.User.OrganizationUnits.pageViewModel = function (emailId) {

    //Class variables
    var self = this;

    var controllerUrl = "/Employee/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }
    // self.actionDataArray = ko.observableArray([]);
    self.profileTypeViewModels = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    self.organizationUnits = ko.observable();
    self.designations = ko.observable(false);
    self.designationHierarchy = ko.observable(false);
    self.validReplyToOrgUnits = ko.observableArray();

    $("input").bind("keydown", function (event) {
        var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
        if (keycode == 13) {
            $('._submitbtn').click();
            return false;
        } else {
            return true;
        }
    });
    $("select").bind("keydown", function (event) {
        var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
        if (keycode == 13) {
            $('._submitbtn').click();
            return false;
        } else {
            return true;
        }
    });

    self.RenderOrganzationUnits = function (organizationUnits) {
     
        self.organizationUnits = new Object();
        self.organizationUnits.items = new Array();
  
        self.organizationUnits.items = self.designationHierarchy = self.convertIntoHirarchicalArray(organizationUnits);

        self.designations = new self.TreeViewModel(self.organizationUnits);
        ko.cleanNode(document.getElementById('parent'));
        ko.applyBindings(self.designations, document.getElementById('parent'));
        $("#parent ul:not(:has(li))").parent().addClass("nochild");


        $("._openclose").on('click', function () {
            self.Toggle(this);
            if ($(this).hasClass('expand')) {
                $(this).removeClass('expand');
            }
            else {
                $(this).addClass('expand');
            }
        });
        EMPMGMT.Framework.Common.ApplyPermission('._unitselector');

        dragableAction();
        dropableAction();
        self.SetCss();
    }
    self.GetUnitsList = function (applyBindings) {
     
        EMPMGMT.Framework.Core.getJSONData(
            controllerUrl + "OrganizationUnitsList",
            function onSuccess(response) {
                self.RenderOrganzationUnits(response.OrganizationUnits);
                EMPMGMT.Framework.Common.ApplyPermission();
            }, function onError(err) {
                self.status(err.Message);
            });
    }


    self.Toggle = function (element) {
        //alert($(element).parent().parent().siblings().children('ul').length);
        var ul = $(element).parent().parent().find("ul:first");
        if (ul.css("visibility") == "visible") { ul.css("visibility", "hidden").find('ul').css("visibility", "hidden"); }
        else {
            ul.find('ul').css("visibility", "hidden");
            ul.css("visibility", "visible");//.find('ul').css("visibility", "visible"); 
            $(ul).children('li').each(function () {
                $(this).find('._openclose:first').addClass('expand');

            });
        }

    }

    self.TreeViewModel = function (data) {
        var mapping = {
            items: {
                create: function (options) {
                    return new self.TreeViewModel(options.data);
                }
            }
        };

        this.items = ko.observableArray();

        ko.mapping.fromJS(data, mapping, this);
    };

    self.TreeViewModel.prototype = {
        constructor: self.TreeViewModel
        //onChecked: function (checked) {
        //    ko.utils.arrayForEach(this.items(), function (item) {
        //        item.checked(checked);
        //    });
        //}
    };
    self.BindUnitListDropDown = function (orgUnitId, parentOrgUnitId) {

        $('._unitListdropdown').empty();
        $('._unitListdropdown').append("<option value='0'>" + EMPMGMT.Messages.DropDowns.SelectOption + "</option>");
        self.GetReplyToOrgUnits(orgUnitId, parentOrgUnitId);
        $.each(self.validReplyToOrgUnits(), function (index, element) {
            $('._unitListdropdown').append("<option value='" + element.OrgUnitId + "'>" + element.OrgUnitName + "</option>");
        });
    }

    self.AddUnitsToDropDown = function (data, orgUnitId, parentOrgUnitId) {

        $.each(data, function (index, element) {
            if (element.OrgUnitId != orgUnitId && element.ParentOrgUnitId != parentOrgUnitId) {
                $('._unitListdropdown').append("<option value='" + element.OrgUnitId + "'>" + element.OrgUnitName + "</option>");
                if (element.items != undefined && element.items != null && element.items.length > 0) {
                    self.AddUnitsToDropDown(element.items, orgUnitId, parentOrgUnitId);
                }
            }
        });
    }
    self.GetReplyToOrgUnits = function (orgUnitId, parentOrgUnitId) {

        self.validReplyToOrgUnits.removeAll();
        var data = self.designationHierarchy;

        self.FindValidOrgUnitsToReplyUnit(data, orgUnitId, parentOrgUnitId);
    }
    self.FindValidOrgUnitsToReplyUnit = function (data, orgUnitId, parentOrgUnitId) {

        $.each(data, function (index, element) {
            if (element.OrgUnitId != orgUnitId && element.ParentOrgUnitId != parentOrgUnitId) {
                var obj = new Object();
                obj.OrgUnitId = element.OrgUnitId;
                obj.OrgUnitName = element.OrgUnitName;
                self.validReplyToOrgUnits.push(obj);
                if (element.items != undefined && element.items != null && element.items.length > 0) {
                    self.FindValidOrgUnitsToReplyUnit(element.items, orgUnitId, parentOrgUnitId);
                }
            }
        });
    }
    self.convertIntoHirarchicalArray = function (array) {
       
        var map = {};
        var main_object = array.filter(function (v) {
            if (v.ParentOrgUnitId == null) return true;
            return false;
        });
        var obj = main_object[0];
        obj.items = [];
        var parent = '-';
        map[parent] = {
            items: []
        };
        obj = self.bindAllChildreen(array, obj);
        map[parent].items.push(obj);
        return map['-'].items;
    }

    self.bindAllChildreen = function (array, obj) {
        var allChild_objects = array.filter(function (v) {
            var returnVal = false;
            if (obj.OrgUnitId == v.ParentOrgUnitId && v.ParentOrgUnitId != null) {
                returnVal = true;
            }
            return returnVal;
        });
        for (var i = 0; i < allChild_objects.length; i++) {
            var objnew = allChild_objects[i];
            objnew.items = [];
            objnew = self.bindAllChildreen(array, objnew);
            obj.items.push(objnew);
        }

        return obj;
    }

    $('._newUnit').click(function () {
        $('._unitName').focus();
        $(".replyToHide").show();
        self.IsDefaultUnit = false;
        $("._unitNameError span").text('');
        $("._replyToError span").text('');
        $('._unitHeader').text("Create Designation");
        $('#hdnOrgUnitId').val(0);
        $('._unitName').val('');
        $('._error').html('');
        $('._unitListdropdown').val(0);
        self.BindUnitListDropDown(0, 0);
    });
    $('._submitbtn').click(function () {
        var errCount = 0;
        var actionName = "EditOrgUnit";
        var successMessage = EMPMGMT.Messages.OrgUnit.UnitSaved;
        self.OrgUnitName = $.trim($("._unitName").val());

        if (self.OrgUnitName == '') {
            $("._unitNameError span").text(EMPMGMT.Messages.OrgUnit.UnitRequired).removeClass("msgssuccess");
            errCount++;//return;
        }
        else {
            $("._unitNameError span").text('');
        }
        self.OrgUnitId = $('#hdnOrgUnitId').val();

        var parentOrgUnitId = $('._unitListdropdown').val();

        if (!self.IsDefaultUnit) {
            if (parentOrgUnitId == 0) {
                $("._replyToError span").text(EMPMGMT.Messages.OrgUnit.ReplyToRequired);// return;
                errCount++;
            }
            else {
                self.ParentOrgUnitId = parentOrgUnitId;
                $("._replyToError span").html();
            }
        }
        if (errCount == 0) {
            if (self.OrgUnitId == 0) {
                actionName = "CreateOrgUnit";
                successMessage = EMPMGMT.Messages.OrgUnit.UnitCreated;
            }
            self.SaveUnit(actionName, successMessage);
            unitPopop.methods.close();
        }
    });
    $('._reassignunitbtn').click(function () {
        var PostData = new Object();
        if ($('._unitlstdropdown').val() > "0") {
            $("._reAssignToError span").text('');
            self.DeleteUnit(self.DeletedOrgUnitId, $('._unitlstdropdown').val());
        }
        else {
            $("._reAssignToError span").text(EMPMGMT.Messages.OrgUnit.ReAssignToRequired);
        }

    });
    self.DeleteUnit = function (deletedOrgUnitId, reassignedId) {
       // debugger;

        var PostData = new Object();
     //   PostData.orgUnitId_encrypted = deletedOrgUnitId;
      //  PostData.reassignedId_encrypted = reassignedId;
        PostData.OrgUnitId = deletedOrgUnitId;
        PostData.ParentOrgUnitId = reassignedId;
        EMPMGMT.Framework.Core.doPostOperation(controllerUrl + "DeleteOrganizationUnit", PostData,
            function SuccessCallBack(response) {
               // debugger;
                if (response.Response.Message == "True") {
                    self.RenderOrganzationUnits(response.OrganizationUnits)
                    EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.OrgUnit.UnitDeleted, false);
                    $('._assignUnitClose').click();
                }
                else {
                    EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.OrgUnit.UnitNotDeleted, true);
                }
            }, function FailureCallback() { })
    }
    self.SaveUnit = function (actionName, successMessage) {

        var PostData = new Object();
        PostData.OrgUnitId = self.OrgUnitId;
        PostData.ParentOrgUnitId = self.ParentOrgUnitId;
        PostData.OrgUnitName = self.OrgUnitName;
        PostData.IsDefaultUnit = self.IsDefaultUnit;
        EMPMGMT.Framework.Core.doPostOperation
                (
                    controllerUrl + actionName,
                    PostData,
                    function onSuccess(response) {
                        if (response.Response.Message == "AlreadyExist") {
                            EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.OrgUnit.UnitAlreadyExists, true);
                        }
                        else if (response.Response.Message == "Success") {

                            EMPMGMT.Framework.Core.ShowMessage(successMessage, false);
                            self.RenderOrganzationUnits(response.OrganizationUnits);
                            window.setTimeout(function () {

                                $('._close').click();
                            }, 5000);
                        }

                        //if (response.Status == Message.Failure) { EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.OrgUnit.UnitAlreadyExists, true); }
                        //else if (response.Status == Message.Success) {
                        //    EMPMGMT.Framework.Core.ShowMessage(successMessage, false);
                        //    self.GetUnitsList();
                        //    $('._close').click();
                        //}
                    },
                    function onError(err) {

                        self.status(err.Message);
                    }
                );
    }
    self.UnitPopup = function (data) {

        $('._unitHeader').text(EMPMGMT.Messages.OrgUnit.EditUnitHeader);
        var unitName = ko.utils.unwrapObservable(data.OrgUnitName);
        var OrgUnitId = ko.utils.unwrapObservable(data.OrgUnitId);
        var parentOrgUnitId = ko.utils.unwrapObservable(data.ParentOrgUnitId);
        self.IsDefaultUnit = ko.utils.unwrapObservable(data.IsDefaultUnit);
        if (self.IsDefaultUnit) {
            $(".replyToHide").hide();
        }
        else {
            $(".replyToHide").show();
        }

        self.BindUnitListDropDown(OrgUnitId, parentOrgUnitId);
        $('#hdnOrgUnitId').val(OrgUnitId);

        $('._unitName').val(unitName);
        if (parentOrgUnitId != undefined && parentOrgUnitId != null) {
            $('._unitListdropdown').val(parentOrgUnitId);
        }
        else {
            $('._unitListdropdown').val(0);
        }
        $('.error-unit').html("");
        $("._unitNameError span").text('');
        $("._replyToError span").text('');
        unitPopop.methods.open()
    }
    $('#unitListdropdown').change(function () {

        var selectedValue = $(this).val();
        if (selectedValue.toString() == '0') { $("._replyToError span").text(EMPMGMT.Messages.OrgUnit.ReplyToRequired); }
        else { $("._replyToError span").text(''); }

    })
    $('#unitlstdropdown').change(function () {

        var selectedValue = $(this).val();
        if (selectedValue.toString() == '0') { $("._reAssignToError span").text(EMPMGMT.Messages.OrgUnit.ReAssignToRequired); }
        else { $("._reAssignToError span").text(''); }

    })
    $('._unitName').keyup(function () {

        var word = $(this).val();
        if (word == null || $.trim(word) == '') { $("._unitNameError span").text(EMPMGMT.Messages.OrgUnit.UnitRequired).removeClass("msgssuccess"); }
        else { $("._unitNameError span").text(''); }
    })
    self.UnitPopupAddScreen = function (data) {
        self.IsDefaultUnit = false;
        $(".replyToHide").show();
        $("._replyToError span").text('');
        $("._unitNameError span").text('');
        $('._unitHeader').text(EMPMGMT.Messages.OrgUnit.CreateUnitHeader);
        var OrgUnitId = ko.utils.unwrapObservable(data.OrgUnitId);
        //$('#hdnOrgUnitId').val(OrgUnitId);

        var parentOrgUnitId = ko.utils.unwrapObservable(data.ParentOrgUnitId);
        $('#hdnParentOrgUnitId').val(parentOrgUnitId);
        $('#hdnOrgUnitId').val(0);
        $('._unitName').val('');

        self.BindUnitListDropDown(0, 0);

        if (OrgUnitId != undefined && OrgUnitId != null) {
            $('._unitListdropdown').val(OrgUnitId);
        }
        else {
            $('._unitListdropdown').val(0);
        }
        unitPopop.methods.open()
    }
    self.UnitPopUpActionDelete = function (data) {
      //  debugger;

        $("._reAssignToError span").text('');
        if (confirm(EMPMGMT.Messages.OrgUnit.ConfirmUnitDeleteAction)) {
            var unitName = ko.utils.unwrapObservable(data.OrgUnitName);
            var orgUnitId = ko.utils.unwrapObservable(data.OrgUnitId);
            var parentOrgUnitId = ko.utils.unwrapObservable(data.ParentOrgUnitId);
            self.DeletedOrgUnitId = orgUnitId;
            if (true) {

                $('._unitHeader').text(EMPMGMT.Messages.OrgUnit.ReAssignUnitHeader);
                self.BindUnitListDropDownForDeleteOperation(orgUnitId, parentOrgUnitId);
                //EMPMGMT.Framework.Com n.OpenPopup('div.assignUnit');
                assignUnitPopup.methods.open()
                $('._error').html("");
            }
            else {
                DeleteUnit(orgUnitId, 0);
            }
        }
    }
    self.GetUnitsList(true);
    self.DeletedOrgUnitId = ko.observable();
    self.BindUnitListDropDownForDeleteOperation = function (orgUnitId, parentOrgUnitId) {

        var data = self.designationHierarchy;
        $('._unitlstdropdown').empty();
        $('._unitlstdropdown').append("<option value='0'>" + EMPMGMT.Messages.DropDowns.SelectOption + "</option>");
        self.AddUnitsToDropDownForReAssigning(data, orgUnitId, parentOrgUnitId);
    }
    self.AddUnitsToDropDownForReAssigning = function (data, orgUnitId, parentOrgUnitId) {
        $.each(data, function (index, element) {
            if (element.OrgUnitId != orgUnitId) {
                $('._unitlstdropdown').append("<option value='" + element.OrgUnitId + "'>" + element.OrgUnitName + "</option>");
                if (element.items != undefined && element.items != null && element.items.length > 0) {
                    self.AddUnitsToDropDownForReAssigning(element.items, orgUnitId, parentOrgUnitId);
                }
            }
        });

    }

    self.SetCss = function () {
    
        //setTimeout(function () {
        var treeWidth = $(".tree").width();
        var totalWidth = 0;
        var arrWidth = [];
        $(".tree ul").each(function () {
          
            totalWidth = 0;
            $(this).children("li").each(function () {
             
                if ($(this).children("ul").children("li").length > 0) {
                    $(this).children("ul").children("li").each(function () {
                   
                        if ($(this).children("ul").children("li").length > 0) {
                            $(this).children("ul").children("li").each(function () {
                             
                                totalWidth += $(this).width() + 150;
                                //totalWidth += $(this).width() + 13;
                            });
                        }
                        else {
                            totalWidth += $(this).width() + 150;
                          //  totalWidth += $(this).width() + 13;
                        }
                    });
                }
                else {
                   // totalWidth += $(this).width() + 13;
                    totalWidth += $(this).width() + 150;
                }
                arrWidth.push(totalWidth);
            });

        });
        var maxValueInArray = Math.max.apply(Math, arrWidth);
        //if (treeWidth < maxValueInArray) {
        $(".tree").css("width", maxValueInArray + "PX");

        //$(".tree ul li:first").css("width", maxValueInArray + "px");
        //$(".tree ul li:first").css("overflow", "auto");
        //}
        //}, 1000);
    }

    var unitPopop = $("._addNewUnitPopbox").popbox({
        open: '._newUnit',
        arrow: '.arrow',
        arrow_border: '.arrow-border',
        close: '._addNewUnitClose',
        overlay: '._popup_overlay'
    });
    var assignUnitPopup = $("._assignUnitPopbox").popbox({
        open: '._editUnit',
        arrow: '.arrow',
        arrow_border: '.arrow-border',
        close: '._assignUnitClose',
        overlay: '._popup_overlay'
    });
    //Drag And Drop Scrript//
    function dragableAction() {
        $('._draggable').draggable({

            revert: true, helper: 'clone',
            appendTo: "body",
            start: function (event, ui) {
                //ui.helper.css("width", "220px");
                ui.helper.css("postion", "relative");
                //$(this).css("border", "1px solid gray");
                //$(this).css("height", "50px");
                //$(this).css("width", "100px");
                $(ui.helper).addClass("ui-draggable-helper");

                var draggedObj = ko.dataFor(this);
                draggedUnitId = draggedObj.OrgUnitId();
                draggedUnitName = draggedObj.OrgUnitName();
                draggedParentUnitId = draggedObj.ParentOrgUnitId();
                self.GetReplyToOrgUnits(draggedUnitId, draggedParentUnitId);
            },
            tolerance: 'fit'
        });
    }
    self.IsValidToDrop = function (orgUnitId) {

        var match = ko.utils.arrayFirst(self.validReplyToOrgUnits(), function (item) {
            return orgUnitId === item.OrgUnitId;
        });
        return match;
    }
    function dropableAction() {
        $("._droppable").droppable({
            //hoverClass: "drop-hover",
            over: function (event, ui) {
                var dropableObj = ko.dataFor(this);
                var dropableOrgUnitId = dropableObj.OrgUnitId();
                var match = self.IsValidToDrop(dropableOrgUnitId);
                if (!match || draggedParentUnitId == dropableOrgUnitId) {
                    $(this).addClass("drop-hover-notallowed");
                    $(ui.helper).css("cursor", "not-allowed");
                }
                else {
                    $(this).addClass("drop-hover");
                    $(ui.helper).css("cursor", "pointer");
                }
            },
            out: function (event, ui) {
                $(this).removeClass("drop-hover-notallowed");
                $(this).removeClass("drop-hover");
            },
            drop: function (event, ui) {
                $(this).removeClass("drop-hover-notallowed");
                $(this).removeClass("drop-hover");

                var dropableObj = ko.dataFor(this);
                var dropableOrgUnitId = dropableObj.OrgUnitId();
                var dropableParentOrgUnitId = dropableObj.ParentOrgUnitId();
                var match = self.IsValidToDrop(dropableOrgUnitId);
                if (!match || draggedParentUnitId == dropableOrgUnitId) {
                    //  alert("Can't drop to below and same Organization Unit!");
                    return false;
                }
                //  ko.dataFor(this).GetParentsUnits(draggedUnitId, ko.dataFor(this).OrgUnitId());            
                //if (parent == draggedUnitId || parent == draggedParentUnitId || dropableParentOrgUnitId == draggedParentUnitId) {
                //    return false;
                //}
                var confirmation = confirm(EMPMGMT.Messages.OrgUnit.ConfirmDropMessage);

                if (confirmation) {
                    var PostData = new Object();
                    PostData.OrgUnitId = draggedUnitId;
                    PostData.ParentOrgUnitId = dropableOrgUnitId;
                    PostData.OrgUnitName = draggedUnitName;
                    EMPMGMT.Framework.Core.doPostOperation
                            (
                                controllerUrl + "EditOrgUnit",
                                PostData,
                                function onSuccess(response) {
                                    var successMessage = EMPMGMT.Messages.OrgUnit.UnitSaved;
                                    if (response.Status == "Failure") { EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.OrgUnit.UnitAlreadyExists, true); }
                                    else if (response.Response.Status == "Success") {
                                        $(ui.draggable).remove();
                                        self.RenderOrganzationUnits(response.OrganizationUnits);
                                        EMPMGMT.Framework.Core.ShowMessage(successMessage, false);
                                    }
                                },
                                function onError(err) {

                                    self.status(err.Message);
                                }
                            );

                }
                return true;
            }


        });
    }



    return self;
}

function showActionField(v) {
    $('._actions').each(function () {
        $(this).hide();
    })
    var data = ko.dataFor(v);
    if (data.IsDefaultForStaffUser) {
        if (data.IsDefaultUnit() == false) {
            $(v).find('._actions').css("display", "Inline-block");
        }
        else {
            $(v).find('._actions').css("display", "Inline-block");
            $(v).find('._actions').find("img:last").css("display", "none");
        }

    }
}

function hideActionField() {

    $('._actions').each(function () {
        $(this).hide();

    })
}
