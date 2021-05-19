using EMPMGMT.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPMGMT.Domain
{
    public class EmployeeModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        private CompanyModel _companyModel;
        public virtual CompanyModel CompanyModel
        {
            get
            {
                if (this._companyModel == null)
                    this._companyModel = new CompanyModel();
                return this._companyModel;
            }
            set { this._companyModel = value; }
        }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public bool IsRegisteredUser { get; set; }
        public bool RecordDeleted { get; set; }
        private OrganizationUnitModel _role;
        public OrganizationUnitModel organizationUnitModel
        {
            get
            {
                if (this._role == null)
                    this._role = new OrganizationUnitModel();
                return this._role;
            }
            set { this._role = value; }
        }
        public int RoleId { get; set; }
        private ProfileModel _profile;
        public ProfileModel ProfileModel
        {
            get
            {
                if (this._profile == null)
                    this._profile = new ProfileModel();
                return this._profile;
            }
            set { this._profile = value; }
        }
        public string ImageURL { get; set; }

        public int UserTypeId { get; set; }
        public string ProfileName { get; set; }
        public int ProfileId { get; set; }
        public int CompanyId { get; set; }
        public int OrgUnitId { get; set; }
        private string _fullName;
        public string FullName
        {
            get
            {
                _fullName = CommonFunctions.ConcatenateStrings(this.FirstName, this.LastName);
                return _fullName;
            }
            set { _fullName = value; }
        }



        private string _reportName;
        public string ReportName
        {
            get
            {
                _reportName = CommonFunctions.ConcatenateStrings(this.FirstName, this.LastName);
                return _reportName;
            }
            set { _reportName = value; }
        }



        public string tokenId { get; set; }
        public string UserNote { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ActivationDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public int ModifiedBy { get; set; }
        public string Comments { get; set; }
        public string Phone { get; set; }    
        public string NewPassword { get; set; }
        public bool IsLinkSend { get; set; }
        public Nullable<System.DateTime> LinkSendDate { get; set; }
        public int CommentId { get; set; }
        public string Comment { get; set; }
        public string UserGuid { get; set; }
        public bool IsPassword { get; set; }

        public string LoginStatus { get; set; }

        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string CorrespondenceAddr { get; set; }
        public string PermanentAddr { get; set; }
        public Nullable<int> DesignationId { get; set; }
        public string PAN { get; set; }
        public Nullable<int> ReportTo { get; set; }
       
        public Nullable<bool> IsSuperAdmin { get; set; }
        public Nullable<int> TechnologyId { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> ReferrerId { get; set; }
        public Nullable<System.DateTime> DateOfJoining { get; set; }
        public Nullable<int> CandidateId { get; set; }
        public Nullable<bool> IsActive { get; set; }    
        //Rekha  Refferrer
        private ReffererModel _reffererModel;
        public ReffererModel reffererModel
        {
            get
            {
                if (this._reffererModel == null)
                    this._reffererModel = new ReffererModel();
                return this._reffererModel;
            }
            set { this._reffererModel = value; }
        }


        //Technologies
        private TechnologyModel _technologyModel;
        public TechnologyModel technologyModel
        {
            get
            {
                if (this._technologyModel == null)
                    this._technologyModel = new TechnologyModel();
                return this._technologyModel;
            }
            set { this._technologyModel = value; }
        }

        //getting Designation model here for mapping
        private DesignationModel _designationModel;
        public DesignationModel designationModel
        {
            get
            {
                if (this._designationModel == null)
                    this._designationModel = new DesignationModel();
                return this._designationModel;
            }
            set { this._designationModel = value; }
        }

        public string ReportToFullName { get; set; }

        public string TotalWorkHours { get; set; }

        public string EmpCode { get; set; }

    }
}
