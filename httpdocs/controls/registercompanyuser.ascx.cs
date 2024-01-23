using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkLibrary.Security;
using System.Web.Configuration;
using System.Text;
using System.Security;

namespace HristoEvtimov.Websites.Work.Web.Controls
{   
    /// <summary>
    /// This control will allow a user to register as an employer/company user.
    /// </summary>
    public partial class registercompanyuser : GeneralControlBase, IFormValidation
    {
        public SecureString Password
        {
            get
            {
                if (Session["Password"] != null)
                {
                    return (SecureString)Session["Password"];
                }
                return null;
            }
            set { Session["Password"] = value; }
        }

        #region "Event Handlers"

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User currentUser = GetUser();
                if (currentUser != null)
                {
                    //registration form and user already exists
                    RedirectToHomeAndError("strUserAlreadyRegistered");
                }

                BindStateAndCountry();
                pnlCompanyApplication.Attributes.Add("style", "display:none;");
            }
        }

        protected void BindStateAndCountry()
        {
            CountryManager countryManager = new CountryManager();
            ddlCompanyCountry.DataSource = countryManager.GetCountries();
            ddlCompanyCountry.DataBind();
            
            ddlCompanyState.DataSource = countryManager.GetStates(ddlCompanyCountry.SelectedItem.Value);
            ddlCompanyState.DataBind();
            ddlCompanyState.SelectedValue = WebConfigurationManager.AppSettings["DEFAULT_STATE"].ToString();
        }

        protected void btnStep1Next_Command(object sender, CommandEventArgs e)
        {
            Password = txtUserPassword.Text.ConvertToSecureString();

            UserManager userManager = new UserManager();
            //check if user with this email already exists
            bool userExists = userManager.UserExists(txtUserEmail.Text);
            if (userExists)
            {
                UrlManager urlManager = new UrlManager();
                string message = String.Format(GetLocalResourceObject("strUserExists").ToString(),
                    urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.Login, null),
                    urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.ResetPassword, null));
                AddSystemMessage(message,
                    GeneralMasterPageBase.SystemMessageTypes.Error,
                    GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                return;
            }

            //check if a company with the same email domain already exists
            bool companyExists = false;
            CompanyManager companyManager = new CompanyManager();
            Company company = companyManager.GetCompanyByUserEmail(txtUserEmail.Text);
            if (company != null)
            {
                if (company.CompanyId > 0)
                {
                    companyExists = true;
                }
            }

            pnlStep2a.Visible = false;
            pnlStep2b.Visible = false;
            if (companyExists)
            {
                pnlStep2b.Visible = true;
                lblExistingCompanyDomain.Text = company.CompanyDomain;
                lblExistingCompanyName.Text = company.Name;
            }
            else
            {
                pnlStep2a.Visible = true;
            }

            SwitchSteps(e.CommandArgument.ToString(), false);
        }

        protected void btnNextPrevious_Command(object sender, CommandEventArgs e)
        {
            SwitchSteps(e.CommandArgument.ToString(), !((Button)sender).CausesValidation);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                LogManager logManager = new LogManager();

                string email = txtUserEmail.Text.ToLower();
                string password = "";
                if (Password != null)
                {
                    password = Password.ConvertToUnsecureString();
                }
                string firstName = txtUserFirstName.Text;
                string lastName = txtUserLastName.Text;

                UserManager userManager = new UserManager();
                CompanyManager companyManager = new CompanyManager();
                MembershipCreateStatus membershipCreateStatus = userManager.CreateMembershipUser(email, password, firstName, UserManager.UserRoles.Employer.ToString());
                if (membershipCreateStatus == MembershipCreateStatus.Success)
                {
                    WorkDal.User user = userManager.CreateUser(email, firstName, lastName, chkbOkToEmail.Checked);
                    if (user == null)
                    {
                        AddSystemMessage(GetLocalResourceObject("strErrorUserNotCreated").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                        logManager.AddLog("User record was not created", -1, "email=" + email, "firstName=" + firstName + ";lastName=" + lastName);
                        return;
                    }

                    if (user.UserId <= 0)
                    {
                        AddSystemMessage(GetLocalResourceObject("strErrorUserNotCreated").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                        logManager.AddLog("User record was not created", -1, "email=" + email, "firstName=" + firstName + ";lastName=" + lastName);
                        return;
                    }

                    //if company already exist then we are not a company admin
                    bool companyExists = false;
                    bool isCompanyAdmin = true;
                    int companyId = -1;
                    Company company = companyManager.GetCompanyByUserEmail(txtUserEmail.Text);
                    if (company != null)
                    {
                        if (company.CompanyId > 0)
                        {
                            companyExists = true;
                            isCompanyAdmin = false;
                            companyId = company.CompanyId;
                        }
                    }

                    //if company does not exist then creat it.
                    if (!companyExists)
                    {
                        string companyName = txtCompanyName.Text;
                        string companyAddress1 = txtCompanyAddress1.Text;
                        string companyAddress2 = txtCompanyAddress2.Text;
                        string companyCity = txtCompanyCity.Text;
                        string companyState = ddlCompanyState.SelectedValue;
                        string companyZip = txtCompanyZip.Text;
                        string companyCountry = ddlCompanyCountry.SelectedValue;
                        string companyPhone = txtCompanyPhoneNumber.Text;
                        string companyWebsite = txtCompanyWebsite.Text;
                        bool isRecruiter = Boolean.Parse(rbtnlCompanyIsRecruiter.SelectedValue);
                        
                        companyId = companyManager.CreateCompany(companyName,
                            companyAddress1, companyAddress2, companyCity, companyState,
                            companyZip, companyCountry, companyPhone, companyWebsite, isRecruiter, user.UserId, email);
                    }

                    if (companyId > 0)
                    {
                        if (!userManager.UpdateUserCompany(user, companyId, isCompanyAdmin))
                        {
                            AddSystemMessage(GetLocalResourceObject("strErrorCompanyUserAssociation").ToString(),
                                GeneralMasterPageBase.SystemMessageTypes.Error,
                                GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                            logManager.AddLog("User was not assigned to company", user.UserId, "companyId=" + companyId.ToString(), "isCompanyAdmin=" + isCompanyAdmin.ToString());
                        }
                    }
                    else
                    {
                        AddSystemMessage(GetLocalResourceObject("strErrorCompanyNotCreated").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                        logManager.AddLog("Company not created", user.UserId, "", "");
                        return;
                    }

                    bool createEmailVerification = userManager.CreateEmailVerification(user);
                    if (!createEmailVerification)
                    {
                        AddSystemMessage(GetLocalResourceObject("strErrorEmailNotSent").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                        logManager.AddLog("Email not sent", user.UserId, "companyId=" + companyId, "");
                        return;
                    }

                    if (chkbCompanyApplication.Checked)
                    {
                        CompanyApplicationManager companyApplicationManager = new CompanyApplicationManager();
                        companyApplicationManager.CreateCompanyApplication(companyId,
                            ddlCompanyNumberOfEmployees.SelectedValue, ddlCompanyPostingsPerYear.SelectedValue);
                    }
                    SwitchSteps("3", false);
                }
                else
                {
                    switch (membershipCreateStatus)
                    {
                        case MembershipCreateStatus.DuplicateEmail: 
                        case MembershipCreateStatus.DuplicateUserName:
                            UrlManager urlManager = new UrlManager();
                            string message = String.Format(GetLocalResourceObject("strErrorUserCreationUserExists").ToString(), 
                                urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.Login, null),
                                urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.ResetPassword, null));
                            AddSystemMessage(message,
                                 GeneralMasterPageBase.SystemMessageTypes.Error,
                                 GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                            break;
                        case MembershipCreateStatus.InvalidEmail :
                            AddSystemMessage(GetLocalResourceObject("strErrorUserCreationInvalidEmail").ToString(),
                                GeneralMasterPageBase.SystemMessageTypes.Error,
                                GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                            break;
                        case MembershipCreateStatus.InvalidPassword:
                            AddSystemMessage(GetLocalResourceObject("strErrorUserCreationInvalidPassword").ToString(),
                                GeneralMasterPageBase.SystemMessageTypes.Error,
                                GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                            break;
                        default:
                            AddSystemMessage(GetLocalResourceObject("strErrorUserNotCreated").ToString(),
                                GeneralMasterPageBase.SystemMessageTypes.Error,
                                GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                            break;
                    }
                }
            }
            else
            {
                //use standard error
                AddSystemMessage(GetGlobalResourceObject("GlobalResources", "strFormNotFilledOutProperly").ToString(),
                    GeneralMasterPageBase.SystemMessageTypes.Error,
                    GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
            }
        }

        #endregion

        #region "Private Methods"

        

        private void SwitchSteps(string step, bool skipValidation)
        {
            if (skipValidation || ValidateForm())
            {
                //Switching between pages depending on comman argument
                pnlStep1.Visible = false;
                pnlStep2.Visible = false;
                pnlStep3.Visible = false;
                
                switch (step)
                {
                    case "1": pnlStep1.Visible = true; break;
                    case "2": pnlStep2.Visible = true; break;
                    case "3": pnlStep3.Visible = true; break;
                }
            }
            else
            {
                AddSystemMessage(GetGlobalResourceObject("GlobalResources", "strFormNotFilledOutProperly").ToString(),
                    GeneralMasterPageBase.SystemMessageTypes.Error,
                    GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
            }
        }

        /// <summary>
        /// Validates all input. Also validates agains bots.
        /// </summary>
        /// <returns></returns>
        public bool ValidateForm()
        {
            StringValidation validation = new StringValidation();
            txtCompanyName.Text = validation.SanitizeUserInputString(txtCompanyName.Text, StringValidation.SanitizeEntityNames.CompanyName);
            txtCompanyAddress1.Text = validation.SanitizeUserInputString(txtCompanyAddress1.Text, StringValidation.SanitizeEntityNames.Address);
            txtCompanyAddress2.Text = validation.SanitizeUserInputString(txtCompanyAddress2.Text, StringValidation.SanitizeEntityNames.Address);
            txtCompanyCity.Text = validation.SanitizeUserInputString(txtCompanyCity.Text, StringValidation.SanitizeEntityNames.City);
            txtCompanyPhoneNumber.Text = validation.SanitizeUserInputString(txtCompanyPhoneNumber.Text, StringValidation.SanitizeEntityNames.Phone);
            txtCompanyWebsite.Text = validation.SanitizeUserInputString(txtCompanyWebsite.Text, StringValidation.SanitizeEntityNames.Phone);
            if (pnlStep2a.Visible)
            {
                Page.Validate("Company");
            }

            txtUserFirstName.Text = validation.SanitizeUserInputString(txtUserFirstName.Text, StringValidation.SanitizeEntityNames.Name);
            txtUserLastName.Text = validation.SanitizeUserInputString(txtUserLastName.Text, StringValidation.SanitizeEntityNames.Name);
            if (pnlStep1.Visible)
            {
                Page.Validate("User");
            }

            return Page.IsValid && heFormValidator.ValidateForm();
        }

        #endregion
    }
}