using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    public partial class contactus : GeneralControlBase, IFormValidation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            litContactUsDescription.Text = String.Format(GetLocalResourceObject("strContactUsDescription").ToString(), WebConfigurationManager.AppSettings["CUSTOMER_SERVICE_PHONE"]);

            if (!IsPostBack)
            {
                User currentUser = GetUser();
                if (currentUser != null)
                {
                    txtName.Text = currentUser.FirstName + " " + currentUser.LastName;
                    txtEmail.Text = currentUser.Email;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Email mail = new Email();
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Name", txtName.Text);
                parameters.Add("Email", txtEmail.Text);
                parameters.Add("Message", txtMessage.Text);

                User currentUser = GetUser();
                if (currentUser != null)
                {
                    parameters.Add("UserId", currentUser.UserId.ToString());
                }
                else
                {
                    parameters.Add("UserId", "");
                }

                mail.SendEmail(WebConfigurationManager.AppSettings["EMAIL_TO"], Email.EmailTemplates.ContactUs, 
                    parameters, null);

                txtEmail.Text = "";
                txtName.Text = "";
                txtMessage.Text = "";
                AddSystemMessage(GetLocalResourceObject("strContactUsSubmitted").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.OK,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
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
            txtName.Text = stringValidation.SanitizeUserInputString(txtName.Text, StringValidation.SanitizeEntityNames.Name);
            
            HtmlParser htmlParser = new HtmlParser(HtmlParser.ParseRules.FilterOutHtml);
            txtMessage.Text = htmlParser.FilterHtml(txtMessage.Text);
            Page.Validate("ContactUs");

            return Page.IsValid && heFormValidator.ValidateForm();
        }
    }
}