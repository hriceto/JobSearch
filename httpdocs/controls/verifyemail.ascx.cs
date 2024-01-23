using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    /// <summary>
    /// Purpose of this control is to process a link that is sent to the user's email
    /// address and verify that the user received the link. Since emails are used for usernames
    /// we need to verify email for validity. A guid is sent with the email link
    /// and that guid is valid for only 24 hours or whatever the setting in web.config is. 
    /// If not clicked during that time period
    /// the user will need to request a password reset which will send a new guid and
    /// will reset the datetime stamp.
    /// </summary>
    public partial class verifyemail : GeneralControlBase, IFormValidation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlResetEmailVerification.Visible = false;

                //getting guid from querystring
                string strGuid = "";
                if (Request.QueryString["verificationid"] != null)
                {
                    strGuid = Request.QueryString["verificationid"].ToString();
                }

                //if both guid and email are present try to find the user in the 
                //database and activate the user account
                Guid guid = Guid.NewGuid();
                if (Guid.TryParse(strGuid, out guid))
                {
                    UserManager userManager = new UserManager();
                    if (userManager.ApproveMembership(guid, Int32.Parse(WebConfigurationManager.AppSettings["EMAIL_VERIFICATION_VALID_FOR_HOURS"].ToString())))
                    {
                        UrlManager urlManager = new UrlManager();
                        string loginUrl = urlManager.GetUrlRedirectAbsolute("/Login.aspx", null);
                        AddSystemMessage(String.Format(GetLocalResourceObject("strUserEmailVerified").ToString(), loginUrl),
                            GeneralMasterPageBase.SystemMessageTypes.OK,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                        return;
                    }
                    else
                    {
                        LogManager logManager = new LogManager();
                        logManager.AddLog("Invalid Attempt to verify user", -1, "guid=" + guid.ToString(), "");
                        pnlResetEmailVerification.Visible = true;
                    }
                }

                AddSystemMessage(String.Format(GetLocalResourceObject("strUserEmailNotVerified").ToString(),
                    WebConfigurationManager.AppSettings["EMAIL_VERIFICATION_VALID_FOR_HOURS"].ToString()),
                    GeneralMasterPageBase.SystemMessageTypes.Error,
                    GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
            }
        }

        protected void btnResetEmailVerification_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                bool resetEmailVerification = false;
                UserManager userManager = new UserManager();
                if (!userManager.IsMembershipUserApproved(txtResetEmailVerificationEmail.Text.Trim()))
                {
                    resetEmailVerification = userManager.ResetUserEmailVerificationToken(txtResetEmailVerificationEmail.Text.Trim(),
                        txtResetEmailVerificationFirstName.Text.Trim());
                }

                if (resetEmailVerification)
                {
                    //success
                    AddSystemMessage(String.Format(GetLocalResourceObject("strResetEmailVerificationSuccess").ToString(), txtResetEmailVerificationEmail.Text), 
                        GeneralMasterPageBase.SystemMessageTypes.OK, 
                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                    pnlResetEmailVerification.Visible = false;
                }
                else
                {
                    //fail to reset link
                    AddSystemMessage(GetLocalResourceObject("strResetEmailVerificationFailure").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.Error,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                }
            }
        }

        public bool ValidateForm()
        {
            StringValidation stringValidation = new StringValidation();
            txtResetEmailVerificationFirstName.Text = stringValidation.SanitizeUserInputString(txtResetEmailVerificationFirstName.Text, StringValidation.SanitizeEntityNames.Name);

            Page.Validate("ResetEmailVerification");
            return Page.IsValid && heFormValidator.ValidateForm();
        }
    }
}