using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    public partial class resetpassword : GeneralControlBase, IFormValidation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //getting guid from querystring
                string strGuid = "";
                if (Request.QueryString["verificationid"] != null)
                {
                    strGuid = Request.QueryString["verificationid"].ToString();
                }

                //if both guid is present try to find the user in the 
                //database and see if they have a valid reset password request
                Guid guid = Guid.NewGuid();
                if (Guid.TryParse(strGuid, out guid))
                {
                    UserManager userManager = new UserManager();
                    if (userManager.UserResetPasswordTokenIsValid(guid,
                        Int32.Parse(WebConfigurationManager.AppSettings["EMAIL_PASSWORD_RESET_VALID_FOR_MINUTES"].ToString())))
                    {
                        pnlChangePassword.Visible = true;
                        pnlRequestResetPassword.Visible = false;
                    }
                    else
                    {
                        AddSystemMessage(GetLocalResourceObject("strInvalidGuid").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                    }
                }
            }
        }

        protected void btnRequestResetPassword_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                UserManager userManager = new UserManager();
                if (userManager.CreateEmailResetPassword(txtRequestResetPasswordEmail.Text, txtRequestResetPasswordFirstName.Text))
                {
                    //email sent successfully
                    AddSystemMessage(string.Format(GetLocalResourceObject("strEmailSentSuccess").ToString(), 
                        WebConfigurationManager.AppSettings["EMAIL_PASSWORD_RESET_VALID_FOR_MINUTES"].ToString()),
                        GeneralMasterPageBase.SystemMessageTypes.OK,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                    pnlRequestResetPassword.Visible = false;
                }
                else
                {
                    //error message email was not sent
                    AddSystemMessage(GetLocalResourceObject("strEmailSentFailure").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.Error,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                }
            }
            else
            {
                AddSystemMessage(GetGlobalResourceObject("GlobalResources", "strFormNotFilledOutProperly").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                //getting guid from querystring
                string strGuid = "";
                if (Request.QueryString["verificationid"] != null)
                {
                    strGuid = Request.QueryString["verificationid"].ToString();
                }

                //if both guid is present try to find the user in the 
                //database and see if they have a valid reset password request
                Guid guid = Guid.NewGuid();
                if (Guid.TryParse(strGuid, out guid))
                {
                    UserManager userManager = new UserManager();
                    pnlChangePassword.Visible = false;
                    pnlRequestResetPassword.Visible = false;

                    if (userManager.ValidatePasswordResetTokenAndChangePassword(guid,
                        Int32.Parse(WebConfigurationManager.AppSettings["EMAIL_PASSWORD_RESET_VALID_FOR_MINUTES"].ToString()),
                        txtChangePasswordPassword.Text,
                        txtChangePasswordEmail.Text,
                        txtChangePasswordFirstName.Text))
                    {
                        UrlManager urlManager = new UrlManager();
                        string message = String.Format(GetLocalResourceObject("strPasswordSuccessfullyChanged").ToString(),
                            urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.Login, null));
                        AddSystemMessage(message,
                            GeneralMasterPageBase.SystemMessageTypes.OK,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                    }
                    else
                    {
                        AddSystemMessage(GetLocalResourceObject("strPasswordChangeError").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                    }
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
            StringValidation stringValidation = new StringValidation();
            txtRequestResetPasswordFirstName.Text = stringValidation.SanitizeUserInputString(txtRequestResetPasswordFirstName.Text, StringValidation.SanitizeEntityNames.Name);
            txtChangePasswordFirstName.Text = stringValidation.SanitizeUserInputString(txtChangePasswordFirstName.Text, StringValidation.SanitizeEntityNames.Name);

            Page.Validate("ResetPassword");
            Page.Validate("ChangePassword");
            return Page.IsValid && heFormValidator.ValidateForm();
        }
    }
}