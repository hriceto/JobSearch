using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;

namespace HristoEvtimov.Websites.Work.Web.JobSeeker.Controls
{
    public partial class updateuser : GeneralControlBase, IFormValidation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User currentUser = GetUser();

                if (currentUser == null)
                {
                    RedirectToHomeAndError("strUserNotLoggedIn");
                }

                txtFirstName.Text = currentUser.FirstName;
                txtLastName.Text = currentUser.LastName;
                txtCoverLetter.Text = currentUser.CoverLetter;
                txtResume.Text = currentUser.Resume;
                txtPhone.Text = currentUser.Phone;
                chkbPublicResume.Checked = currentUser.ShareResumeWithEmployers;
                chkbOkToEmail.Checked = currentUser.OkToEmail;

                CategoryManager categoryManager = new CategoryManager();
                lstbUserCategories.DataSource = categoryManager.GetCategories();
                lstbUserCategories.DataBind();

                foreach (UserCategory userCategory in currentUser.UserCategories)
                {
                    foreach (ListItem userCategoryItem in lstbUserCategories.Items)
                    {
                        if (userCategoryItem.Value == userCategory.CategoryId.ToString())
                        {
                            userCategoryItem.Selected = true;
                        }
                    }
                }
            }
            string script = "";
            script += "CKEDITOR.replace( '" + txtCoverLetter.ClientID + "', {allowedContent:'" + WebConfigurationManager.AppSettings["ALLOWED_HTML_TAGS"] + "'});\n\t";
            script += "CKEDITOR.replace( '" + txtResume.ClientID + "', {allowedContent:'" + WebConfigurationManager.AppSettings["ALLOWED_HTML_TAGS"] + "'});\n\t";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ckeditor", script, true);
        }

        protected void btnUpdateUser_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                UserManager userManger = new UserManager();
                User currentUser = userManger.GetUser();

                if (currentUser == null)
                {
                    RedirectToHomeAndError("strUserNotLoggedIn");
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

                //update user & user categories
                bool success = userManger.UpdateUser(currentUser, txtPhone.Text, txtCoverLetter.Text, txtResume.Text, chkbPublicResume.Checked,
                    txtFirstName.Text, txtLastName.Text, chkbOkToEmail.Checked, categoryAssignments);

                if (success)
                {
                    AddSystemMessage(GetLocalResourceObject("strUpdateUserSuccess").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.OK,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                }
                else
                {
                    AddSystemMessage(GetLocalResourceObject("strUpdateUserFailure").ToString(),
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

        public bool ValidateForm()
        {
            StringValidation stringValidation = new StringValidation();
            txtPhone.Text = stringValidation.SanitizeUserInputString(txtPhone.Text.Trim(), StringValidation.SanitizeEntityNames.Phone).Trim();
            txtFirstName.Text = stringValidation.SanitizeUserInputString(txtFirstName.Text.Trim(), StringValidation.SanitizeEntityNames.Name).Trim();
            txtLastName.Text = stringValidation.SanitizeUserInputString(txtLastName.Text.Trim(), StringValidation.SanitizeEntityNames.Name).Trim();
            Page.Validate("UpdateUser");

            return Page.IsValid && heFormValidator.ValidateForm();
        }
    }
}