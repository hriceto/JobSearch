using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    public partial class registeruser : GeneralControlBase, IFormValidation
    {
        public bool IsJobApplication { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string script = "";
            script += "CKEDITOR.replace( '" + txtCoverLetter.ClientID + "', {allowedContent:'" + WebConfigurationManager.AppSettings["ALLOWED_HTML_TAGS"] + "'});\n\t";
            script += "CKEDITOR.replace( '" + txtResume.ClientID + "', {allowedContent:'" + WebConfigurationManager.AppSettings["ALLOWED_HTML_TAGS"] + "'});\n\t";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ckeditor", script, true);

            if (!IsPostBack)
            {
                CategoryManager categoryManager = new CategoryManager();
                lstbUserCategories.DataSource = categoryManager.GetCategories();
                lstbUserCategories.DataBind();

                //preset panels
                pnlSaveCoverLetter.Visible = false;
                pnlSaveResume.Visible = false;
                pnlRegisterChoice.Visible = false;
                pnlRegister.Visible = true;

                lblHeading.Text = GetLocalResourceObject("strHeadingRegister").ToString();
                if (IsJobApplication)
                {
                    int jobPostId = -1;
                    if (Request.QueryString["jobpostid"] != null)
                    {
                        jobPostId = Int32.Parse(Request.QueryString["jobpostid"].ToString());
                    }
                    JobManager jobManager = new JobManager();
                    JobPost jobPost = jobManager.GetJobPostForDetail(jobPostId, false, true);
                    if (jobPost != null)
                    {
                        lblHeading.Text = String.Format(GetLocalResourceObject("strHeadingJobApplication").ToString(),
                            jobPost.Title);
                    }

                    rdbRegisterYes.Checked = true;
                    pnlRegisterChoice.Visible = true;

                    rdbRegisterYes.Attributes.Add("onclick", "javascript:ChangeRegisterOption(1);");
                    rdbRegisterNo.Attributes.Add("onclick", "javascript:ChangeRegisterOption(0);");
                    btnSubmit.Text = GetLocalResourceObject("strApply").ToString();
                }
                
                //if current user is already a job seeker then prepopulate application form.
                UserManager userManager = new UserManager();
                User currentUser = userManager.GetUser();
                if (currentUser != null)
                {
                    if (IsJobApplication)
                    {
                        UserManager.UserRoles userRole = userManager.GetMembershipUserRole(currentUser.Email);
                    
                        //prepopulate job seeker information
                        if (userRole == UserManager.UserRoles.JobSeeker)
                        {
                            txtFirstName.Text = currentUser.FirstName;
                            txtLastName.Text = currentUser.LastName;
                            txtCoverLetter.Text = currentUser.CoverLetter;
                            txtResume.Text = currentUser.Resume;
                            txtEmail.Text = currentUser.Email;
                            txtPhone.Text = currentUser.Phone;

                            pnlRegister.Visible = false;
                            pnlRegisterChoice.Visible = false;
                            pnlSaveCoverLetter.Visible = true;
                            pnlSaveResume.Visible = true;
                        }
                        else
                        {
                            //job application for non-job seeker is disabled
                            RedirectToHomeAndError("strUserNotJobSeeker");
                        }
                    }
                    else
                    {
                        //registration form and user already exists
                        RedirectToHomeAndError("strUserAlreadyRegistered");
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                bool submitSuccess = false;
                LogManager logManager = new LogManager();

                bool skipRegistration = false;
                if (IsJobApplication && rdbRegisterNo.Checked)
                {
                    skipRegistration = true;
                }

                UserManager userManager = new UserManager();
                User currentUser = userManager.GetUser();
                if (currentUser != null)
                {
                    skipRegistration = true;
                }

                if (!skipRegistration)
                {
                    CompanyManager companyManager = new CompanyManager();
                    MembershipCreateStatus membershipCreateStatus = userManager.CreateMembershipUser(txtEmail.Text.ToLower(),
                        txtUserPassword.Text, txtFirstName.Text, UserManager.UserRoles.JobSeeker.ToString());

                    bool registrationOK = (membershipCreateStatus == MembershipCreateStatus.Success);
                    if (registrationOK)
                    {
                        User newUser = userManager.CreateUser(txtEmail.Text.ToLower(), txtFirstName.Text,
                            txtLastName.Text, chkbOkToEmail.Checked);
                        if (newUser == null)
                        {
                            AddSystemMessage(GetLocalResourceObject("strErrorUserNotCreated").ToString(),
                                GeneralMasterPageBase.SystemMessageTypes.Error,
                                GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                            logManager.AddLog(GetLocalResourceObject("strErrorUserNotCreated").ToString(), -1, "Email=" + txtEmail.Text.ToLower(),
                                "FirstName=" + txtFirstName.Text + "&LastName=" + txtLastName.Text);
                            return;
                        }

                        //Send email verification email
                        bool createEmailVerification = userManager.CreateEmailVerification(newUser);
                        if (!createEmailVerification)
                        {
                            AddSystemMessage(GetLocalResourceObject("strErrorEmailNotSent").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                            logManager.AddLog("Email not sent", newUser.UserId, "email=" + newUser.Email, "");
                            return;
                        }

                        //get new user categories
                        List<int> categoryAssignments = new List<int>();
                        foreach (ListItem categoryItem in lstbUserCategories.Items)
                        {
                            if (categoryItem.Selected)
                            {
                                categoryAssignments.Add(Int32.Parse(categoryItem.Value));
                            }
                        }
                        registrationOK = userManager.UpdateUser(newUser,
                            txtPhone.Text, txtCoverLetter.Text, txtResume.Text, chkbPublicResume.Checked,
                            newUser.FirstName, newUser.LastName, newUser.OkToEmail, categoryAssignments);
                        currentUser = newUser;
                    }

                    if (registrationOK)
                    {
                        AddSystemMessage(GetLocalResourceObject("strRegistrationSuccess").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.OK,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                        if (!IsJobApplication)
                        {
                            submitSuccess = true;
                        }
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
                            case MembershipCreateStatus.InvalidEmail:
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
                                AddSystemMessage(GetLocalResourceObject("strRegistrationFailure").ToString(),
                                    GeneralMasterPageBase.SystemMessageTypes.Error,
                                    GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                                break;
                        }

                        logManager.AddLog("Job Seeker Registration Not OK", -1, "email=" + txtEmail.Text.ToLower() + ";phone=" +
                            txtPhone.Text + ";publicresume=" + chkbPublicResume.Checked + ";oktoemail=" +
                            chkbOkToEmail.Checked, "");
                    }
                }

                if (IsJobApplication)
                {
                    //todo: add ability to apply with a word doc resume file. forward the file to the 
                    //employer via email. Also virus scan the file before sending it. and maybe 
                    //save the file to disc as well.

                    int jobPostId = -1;
                    if (Request.QueryString["jobpostid"] != null)
                    {
                        jobPostId = Int32.Parse(Request.QueryString["jobpostid"].ToString());
                    }

                    //add application
                    JobApplicationManager jobApplicationManger = new JobApplicationManager();
                    int jobApplicaitonId = jobApplicationManger.AddJobApplication(jobPostId, currentUser, txtFirstName.Text,
                        txtLastName.Text, txtEmail.Text.ToLower(), txtCoverLetter.Text, txtResume.Text, txtPhone.Text);

                    //if there is a user then save their resume and cover letter to their profile if they chose to do so
                    if (currentUser != null && skipRegistration)
                    {
                        userManager.UpdateUserResume(currentUser, txtCoverLetter.Text, txtResume.Text, chkbSaveCoverLetter.Checked, chkbSaveResume.Checked);
                    }

                    if (jobApplicaitonId > 0)
                    {
                        AddSystemMessage(GetLocalResourceObject("strApplicationSuccess").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.OK,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                        submitSuccess = true;
                    }
                    else
                    {
                        AddSystemMessage(GetLocalResourceObject("strApplicationFailure").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                        logManager.AddLog("Job Application failure", -1, "email=" + txtEmail.Text.ToLower(), "jobpostid=" + jobPostId);
                    }
                }

                if (submitSuccess)
                {
                    pnlForm.Visible = false;
                }

                //send email to resume help partner
                if (chkbResumeHelp.Checked)
                {
                    userManager.ProcessResumeHelpRequest(txtFirstName.Text, txtLastName.Text, txtEmail.Text.ToLower(), txtPhone.Text, txtResume.Text);
                }
            }
            else
            {
                AddSystemMessage(GetGlobalResourceObject("GlobalResources", "strFormNotFilledOutProperly").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
            }
        }

        public bool ValidateForm()
        {
            StringValidation stringValidation = new StringValidation();
            txtFirstName.Text = stringValidation.SanitizeUserInputString(txtFirstName.Text, StringValidation.SanitizeEntityNames.Name);
            txtLastName.Text = stringValidation.SanitizeUserInputString(txtLastName.Text, StringValidation.SanitizeEntityNames.Name);
            txtPhone.Text = stringValidation.SanitizeUserInputString(txtPhone.Text, StringValidation.SanitizeEntityNames.Phone);
            Page.Validate("Application");
            if (!IsJobApplication || (IsJobApplication && !rdbRegisterNo.Checked))
            {
                Page.Validate("RegisterUser");
            }            

            return Page.IsValid && heFormValidator.ValidateForm();
        }
    }
}