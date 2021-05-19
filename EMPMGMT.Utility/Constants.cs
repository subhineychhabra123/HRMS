using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Utility
{
    public static class Constants
    {
        public const string AUTHENTICATION_ROLE_ADMIN = "Admin";
        public const string AUTHENTICATION_ROLE_USER = "User";
        public const string PROFILE_IMAGE_PATH = "/Uploads/Employee/";
        public const string ACTION_ITEM_DOCUMENTS_PATH = "/Uploads/Project/ActionItem/";
        public const string USER_DOCUMENTS_PATH = "/Uploads/Employee/";
        public const string CONTENT_IMAGES_PATH = "/Content/images/";
        public const string METRIC_DASHBOARD_DOCUMENTS_PATH = "/Uploads/Metric Dashboard/";
        public const string GOAL_DOCUMENTS_PATH = "/Uploads/Goal/";
        public const string ACTION_LIST_DOCUMENTS_PATH = "/Uploads/Project/ActionList/";
        public const string RCA_DOCUMENTS_PATH = "/Uploads/RCA/";
        public const string PROFILE_IMAGE_NAME_PREFIX = "ProfileImage_";
        public const string PERMISSION_VIEW = "View";
        public const string PERMISSION_CREATE = "Create";
        public const string PERMISSION_CREATEDOCUMENT = "CreateDocument";
        public const string PERMISSION_VIEWDOCUMENT = "ViewDocument";
        public const string PERMISSION_EDITDOCUMENT = "EditDocument";
        public const string PERMISSION_EDIT = "Edit";
        public const string PERMISSION_DELETE = "Delete";

        public const string MODULE_Projects = "Projects";
        public const string MODULE_Documents = "Docs";
        public const string PERMISSION_DELETEDOCUMENT = "DeleteDocument";
        
        public const string MODULE_USER = "User";
        public const string MODULE_PROFILE = "P";
        public const string MODULE_ORGANIZATIONAL_UNITS = "OU";
        public const string MODULE_METRIC = "Metric";
        public const string MODULE_GOAL = "Goal";
        public const string MODULE_METRIC_DASHBOARD="MetricDashboard";
        public const string MODULE_CATEGORY = "Cat";
        public const string MODULE_CONFIGURATION = "Conf";
        public const string MODULE_MANAGEMETRICDASHBOARD = "ManageMetricDashboard";
        public const string MODULE_MANAGE_ACTION_ITEM = "MAI";
        public const string MODULE_MANAGE_ACTION_LIST = "MAL";

        public const string MODULE_RCA = "RCA";
        public const string MODULE_KPI = "KPI";
        public const string MODULE_KPILevels = "KPILevels";
        public const string EXCEL_FILE_EXTENSION = ".xlsx";
        public const string JS_FILE_EXTENSION = ".js";
        public const string DEFAULT_CULTURE_NAME = "English(en-US)";

        //public const string MODULE_Project = "Project";

       

        //public pages name

        public const string PUBLIC_PAGE_HOME = "Home";
        public const string PUBLIC_PAGE_LOGIN = "Login";
        public const string PUBLIC_PAGE_REGISTRATION = "Registration";
        public const string PUBLIC_PAGE_FORGOTPASSWORD = "ForgotPassword";
        public const string PUBLIC_PAGE_FEATURE = "Feature";
        public const string PUBLIC_PAGE_CONTACTUS = "Contactus";
        public const string PUBLIC_PAGE_ABOUTUS = "Aboutus";


        //Email Template Constants

        public const string Registration_User_Name = "{Registration_User_Name}";
        public const string Account_Login_UserId = "{Account_Login_UserId}";
        public const string Account_Login_Passowrd = "{Account_Login_Passowrd}";
        public const string Account_Login_URL = "{Account_Login_URL}";
        public const string Access_For_Deployment_Module = "{Access_For_Deployment_Module}";
        public const string Modules_Access_Type = "{Modules_Access_Type}";
        public const string Module_Access_Valid_From = "{Module_Access_Valid_From}";
        public const string Module_Access_Valid_Till = "{Module_Access_Valid_Till}";
        public const string Comment_By_Admin = "{Comment_By_Admin}";



        public const string RegistrationActivationMailFileName = "RegistrationActivationMail.txt";
        public const string InvitationActivationMailFileName = "InvitationMail.txt";
        public const string UserInvitationActivationMailFileName = "InvitationMailToUser.txt";


        
        public const string RegistrationDeniedMailFileName = "RegistrationDeniedMail.txt";
        public const string RegistrationMoreInfoMailFileName = "RegistrationMoreInfoMail.txt";
        public const string RegistrationMailFileName = "RegistrationMail.txt";

        public const string RegistrationActivationMailSubject = "Please activate your account.";
        public const string RegistrationUserActivationMailSubject = "Welcome to EMPMGMT Deployment.";
        public const string RegistrationDeniedMailSubject = "Rejected Registration for EMPMGMT DEPLOYMENT";
        public const string RegistrationMoreInfoMailSubject = "More Information Registration";
        public const string RegistrationMailSubject = "Welcome to EMPMGMT DEPLOYMENT";

        public const string PasswordRecoveryMailSubject = "[EMPMGMT] Password Recovery mail";
        public const string PasswordRecoveryMailFileName = "PasswordRecoveryMail.txt";
        //End Email Templates Constants

        // Dropdown Constants

        public const int DropDownListDefualtValue = 0;
        public const string DropDownListDefaultText = "Select";

        //End Dropdown Constans
    }
}
