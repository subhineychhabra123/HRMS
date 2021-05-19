﻿jQuery.namespace('EMPMGMT.Messages');
EMPMGMT.Messages = {
    OrgUnit: { "CreateUnit": "Create New Designation", "UnitCreated": " Designation created successfully.", "UnitRequired": "Designation is required", "ReplyToRequired": "Reply to is required", "ReAssignToRequired": "Assign to is required", "UnitDeleted": " Designation Deleted Successfully.", "UnitNotDeleted": "Designation not Deleted.", "EditUnit": "Edit Designation", "ConfirmUnitDeleteAction": "Are you sure you want to delete this Designation?", "ReAssignUnitHeader": "Re-Assign Designation", "EditUnitHeader": "Edit Designation Unit", "CreateUnitHeader": "Create Designation", "UnitAlreadyExists": " Designation Already Exists.", "UnitSaved": "Designation saved successfully", "RestrictDragMessage": "You cannot drag the Designation at your lower level", "ConfirmDropMessage": "Are you sure, you want to change the Designation structure?" },
    DropDowns: { SelectOption: "Select" },
    User: { "ResendMailSucessFully": "Mail send successfully", "UserAlreadyExist": "User Already Exist", "AddUser": "Create New User", "UserAddedSuccess": "User created successfully.", "UserUpdatedSuccess": "User updated successfully.", "EditUser": "Edit User", "ConfirmUserDeleteAction": "Are you sure you want to delete this user?", "AddUserHeader": "Create User", "DeleteEmployee": "Record deleted Successfully" },

    Password: { "PasswordLengthMessage": "Password should be minimum of 6 characters", "ResetPasswordSuccess": "Reset password successfully.", "IncorrectPassword": "Enter Correct Password", "InvalidUser": "Invalid User", "PasswordMismatch": "Password Does not match", "ResetPasswordLength": "Password should be minimum of 6 characters" },
    Category: { "CategoryDeleted": "Category deleted successfully", "CategorySaved": "Category saved successfully", "CategoryAlreadyExist": "Category name already exist", "CategoryCreated": "Category created successfully", "DeletConfirmationMessage": "Are you sure, you want to delete this category?\nNOTE: Deleting a category will delete all the Metrics associated with it.", "AddCategoryHeader": "Add Category", "EditCategoryHeader": "Edit Category", "CategoryListingHeader": "Category", "ErrorOccured": "Error Occured" },
    Goal: { 'AddGoalHeader': 'Create Goal', 'EditGoalHeaer': 'Edit Goal', 'GoalSavedSuccessMsg': 'Saved successfully.', 'GoalDeleteSuccessMsg': 'Deleted successfully.', 'AddBreakThrough': 'Add Break Through', 'AddAnnualObjective': 'Add Annual Objective', 'AddTargetToImprove': 'Add Target to improve', 'AddKeyImprovement': 'Add Key improvement', 'AddGoalResponsible': 'Add Responsible', 'AddGoalResponsible': 'Add Responsible', 'UpdateGoalResponsible': 'Update Responsible', 'UpdateBreakThrough': 'Update Break Through', 'UpdateAnnualObjective': 'Update Annual Objective', 'UpdateTargetToImprove': 'Update Target to improve', 'UpdateKeyImprovement': 'Update Key improvement', 'ConfirmDeleteBreakThrough': 'Are you sure you want to delete this breakthrough?', 'ConfirmDeleteAnnualObjective': 'Are you sure you want to delete this annual objective?', 'ConfirmDeleteKeyImprovement': 'Are you sure you want to delete this key improvement?', 'ConfirmDeleteTargetToImprove': 'Are you sure you want to delete this target?', 'ConfirmDeleteResponsible': 'Are you sure you want to delete this user?', 'ConfirmDeleteGoalDocumentFile': 'Are you sure you want to delete this document?', 'ConfirmDeleteGoal': 'Are you sure you want to delete this goal?', 'AddKeyImprovement2': 'Add 2nd Level Key Improvement', 'AddKeyImprovement3': 'Add 3rd Level Key Improvement', 'UpdateKeyImprovement2': 'Update 2nd Level Key improvement', 'UpdateKeyImprovement3': 'Update 3rd Level Key improvement' },
    MetricDashboard: { "DeletDocument": "Are you sure, you want to delete attachment", "UpdateSucessfully": "Data saved successfully", "Addsucessfully": "Data saved successfully", "InvalidResponsibleUser": "Please enter valid Responsible user", "AlreadyExist": "Title already exist", "ErrorOccured": "Error Occured", "DashboardDeleted": "Dashboard deleted successfully", },
    MertricDashboardAssociation: { 'DeletDashboardOrganization': 'Are you sure, you want to delete this Organization?', 'DeletDashboardMetric': 'Are you sure, you want to delete this Metric?', 'DeleteDashboardMetricResponsible': 'Are you sure, you want to delete this Responsible?', 'DeleteDashboardResponsible': 'Are you sure, you want to delete this Responsible?', 'DeleteDashboardOrgResponsible': 'Are you sure, you want to delete this Responsible?' },
    MertricDashboardData: { "DeleteDocument": "Are you sure, you want to delete this Metric Dascshboard?" },
    ActionList: { "DeleteActionList": "Are you sure, you want to delete this Action List?", "DeleteDocument": "Are you sure, you want to delete this Action List Document?" },
    ActionItem: { "DeleteActionItem": "Are you sure, you want to delete this Action Item?", "DeleteDocument": "Are you sure, you want to delete this Action Item Document?" },
    RootCauseAction: { "DeleteMetric": "Are you sure, you want to delete this metric?", "DeleteCategory": "Are you sure, you want to delete this category?", "SaveSuccessfully": "Saved successfully", "DeleteSuccessfully": "Deleted successfully", "DeleteDocument": "Are you sure, you want to delete this document?", "DeleteRCA": "Are you sure, you want to delete this Root Cause?" },
    KPI: { "DeleteKPI": "Are you sure, you want to delete this KPI?", "SaveSuccessfully": "Saved successfully", "DeleteSuccessfully": "Deleted successfully" },
    KPILevel: { "EditKPILevel": "Edit", "UpdateKPILevel": "Update", "CreateKPILevel": "Create", "SaveKPILevel": "Save", "DeleteConfirmation": "Are you sure, you want to delete this KPI Level?" },

    UserDocument: { "DeleteDocument": "Are you sure, you want to delete this User's Document?" },

    Project: { "ProjectDeleted": "Project delete successfully!", "DeleteProject": "Are you sure, you want to delete this project?", "ProjectEdit": "Updated successfully", "ProjectAdd": "Data saved successfully" },

    TimeSheet: {'AddTimeSheetHeader': 'Create Time Sheet ', 'EditTimeSheetHeader': 'Edit Time Sheet', 'TimeSheetSavedSuccessMsg': 'Saved successfully.', 'TimeSheetDeleteSuccessMsg': 'Deleted successfully.', 'AddBreakThrough': 'Add Break Through', 'AddAnnualObjective': 'Add Annual Objective', 'AddTargetToImprove': 'Add Target to improve', 'AddKeyImprovement': 'Add Key improvement', 'AddGoalResponsible': 'Add Responsible', 'AddTimeSheetResponsible': 'Add Responsible', 'UpdateTimeSheetResponsible': 'Update Responsible', 'UpdateBreakThrough': 'Update Break Through', 'UpdateAnnualObjective': 'Update Annual Objective', 'UpdateTargetToImprove': 'Update Target to improve', 'UpdateKeyImprovement': 'Update Key improvement', 'ConfirmDeleteBreakThrough': 'Are you sure you want to delete this breakthrough?', 'ConfirmDeleteAnnualObjective': 'Are you sure you want to delete this TimeSheet?', 'ConfirmDeleteKeyImprovement': 'Are you sure you want to delete this key improvement?', 'ConfirmDeleteTargetToImprove': 'Are you sure you want to delete this target?', 'ConfirmDeleteResponsible': 'Are you sure you want to delete this user?', 'ConfirmDeleteTimeSheetDocumentFile': 'Are you sure you want to delete this document?', 'ConfirmDeleteTimeSheet': 'Are you sure you want to delete this goal?', 'AddKeyImprovement2': 'Add 2nd Level Key Improvement', 'AddKeyImprovement3': 'Add 3rd Level Key Improvement', 'UpdateKeyImprovement2': 'Update 2nd Level Key improvement', 'UpdateKeyImprovement3': 'Update 3rd Level Key improvement'}
}