jQuery.namespace('EMPMGMT.User.ManageCategory');
var CategoryID = '';
var viewModelManageCategory;
var controllerUrl = "/Employee/";
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
EMPMGMT.User.ManageCategory.pageLoad = function () {

    viewModelManageCategory = new EMPMGMT.User.ManageCategory.pageViewModel();
    ko.applyBindings(viewModelManageCategory);
}

EMPMGMT.User.ManageCategory.tableHeaderViewModel = function (title, columnname, viewModel) {
    var self = this;
    self.ColumnText = ko.observable(title);
    self.ColumnName = ko.observable(columnname);
    self.SortOrder = ko.observable('');
    self.Sort = viewModel.Sort;
}

EMPMGMT.User.ManageCategory.ManageCategoryViewModel = function (data) {
    var self = this;
    var categoryName = '';
    var editCategoryAction = '';
    var categoryId = '';
    var deleteCategoryAction = '';
    if (data != undefined) {
        categoryName = data.CategoryName;
        categoryId = data.CategoryId;
        editCategoryAction = viewModelManageCategory.EditCategoryAction;
        deleteCategoryAction=  viewModelManageCategory.DeleteCategoryAction;
    }
    self.CategoryName = ko.observable(categoryName);
    // self.Category.CategoryName = ko.observable(categoryName);
    self.CategoryId = ko.observable(categoryId);
    self.EditCategoryScreen = editCategoryAction;
    self.DeleteCategory = deleteCategoryAction;
    return self;
}
//Page view model
EMPMGMT.User.ManageCategory.pageViewModel = function () {
    //Class variables
    var self = this;
    var orderbycolumn = '', orderby = '';


    //****************** Invited User PopUp functionality *************************************//
    $("._CategoryPopbox").popbox({
        open: '._categoryOpen',
        arrow: '.arrow',
        arrow_border: '.arrow-border',
        close: '._categoryClose',
        overlay: '._popup_overlay'
    });


    self.TableHeaders = ko.observableArray([]);
    self.Category = new EMPMGMT.User.ManageCategory.ManageCategoryViewModel();
    self.CategoryList = ko.observableArray([]);
    self.categories = new EMPMGMT.User.ManageCategory.ManageCategoryViewModel();
    self.RegisteredUsersList = ko.observableArray([]);
   
    self.GatCategoryList = function () {
        var objParam = new Object();
        objParam.OrderByColumn = orderbycolumn;
        objParam.OrderBy = orderby;

        EMPMGMT.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GatCategoryList", objParam,
           function onSuccess(response) {
               self.RenderCategory(response);
               // $("._CompanyUsersPager").html(self.GetPaging(response.TotalRecords, currentPageNo, PagingMethodForRegisteredUsers, "CompanyUsersPager"));
               EMPMGMT.Framework.Common.ApplyPermission();
           },
       function onError(err) {
           EMPMGMT.Framework.Core.ShowMessage(err.Message, true);
       });

    }

    self.RenderTableHeaders = function () {

        self.TableHeaders.push(new EMPMGMT.User.ManageCategory.tableHeaderViewModel("Title", "Title", self));
        //self.TableHeaders.push(new EMPMGMT.User.ManageCategory.tableHeaderViewModel("Action", "Action", self));

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
            self.GatCategoryList();
        }
    }

    self.Category.CategoryName = ko.observable('').extend({  // custom message
        required: {
            message: 'Title is required'

        }
    });

    self.CategoryId = ko.observable();
    ////***************************************************************************************//
 
    self.EditCategoryAction = function (data) {
        self.Category.CategoryName(data.CategoryName());
        self.Errors.showAllMessages(false);
        $('._categoryHeader').text(EMPMGMT.Messages.Category.EditCategoryHeader);
        CategoryID = data.CategoryId();
    }

    self.DeleteCategoryAction = function (data) {
        var confirmDeletion = confirm(EMPMGMT.Messages.Category.DeletConfirmationMessage);
        if (confirmDeletion) {
            var PostData = new Object();
            PostData.CategoryId = data.CategoryId();
            EMPMGMT.Framework.Core.doPostOperation(controllerUrl + "DeleteCategory", PostData,
                function SuccessCallBack(response) {
                    if (response.Status == "Success") {
                        self.GatCategoryList();
                        EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Category.CategoryDeleted, false);

                    }
                    else {
                        EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Category.ErrorOccured, true);
                    }
                }, function FailureCallback() { })
        }

    }
    self.CreateCategoryAction = function () {
        $('._categoryHeader').text(EMPMGMT.Messages.Category.AddCategoryHeader);      
        CategoryID = "";
        self.Category.CategoryName('');
        self.Errors.showAllMessages(false);
    }
    self.SaveCategory = function (data) {
        var self = this;
        //add category //
        if (self.Errors().length == 0) {
            var PostData = new Object();
            PostData.CategoryName = self.Category.CategoryName();
            if (CategoryID != "") {
                PostData.CategoryId = CategoryID;
            }
            EMPMGMT.Framework.Core.doPostOperation(controllerUrl + "UpdateCategory", PostData,
                function SuccessCallBack(response) {
                  
                    if (response.Message == "CreateSucessfully") {
                        $('._categoryClose').click();
                        self.GatCategoryList();
                        EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Category.CategoryCreated, false);
                      
                    }
                    else if (response.Message == "UpdateSucessfully") {
                        $('._categoryClose').click();
                        self.GatCategoryList();
                        EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Category.CategorySaved, false);
                        
                    }
                    else if (response.Message == "AlreadyExist") {
                        EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Category.CategoryAlreadyExist, true);
                       
                    }
                    else {
                        EMPMGMT.Framework.Core.ShowMessage(EMPMGMT.Messages.Category.ErrorOccured, true);
                       
                    }
                }, function FailureCallback() { })

        }
        else {
            self.Errors.showAllMessages();
        }

    }

    self.RenderCategory = function (categories) {
        $("._CategoryNoRecord").hide();
        self.CategoryList.removeAll();
        if (categories.DataList.length == 0) {
            $("._CategoryNoRecord").show();
        }
        ko.utils.arrayForEach(categories.DataList, function (category) {

            self.CategoryList.push(new EMPMGMT.User.ManageCategory.ManageCategoryViewModel(category));
        });
    };

    self.RenderTableHeaders();

    self.GatCategoryList();


    self.Errors = ko.validation.group([self.Category.CategoryName]);
}

