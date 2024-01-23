using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;

namespace HristoEvtimov.Websites.Work.Web.Employer.Controls
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

                txtUserFirstName.Text = currentUser.FirstName;
                txtUserLastName.Text = currentUser.LastName;
                chkbOkToEmail.Checked = currentUser.OkToEmail;
            }
        }

        protected void btnUpdateUser_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                UserManager userManager = new UserManager();
                User currentUser = userManager.GetUser();

                if (currentUser == null)
                {
                    RedirectToHomeAndError("strUserNotLoggedIn");
                }

                bool updateSuccess = userManager.UpdateUser(currentUser,
                    currentUser.Phone, currentUser.CoverLetter, currentUser.Resume, currentUser.ShareResumeWithEmployers,
                    txtUserFirstName.Text, txtUserLastName.Text, chkbOkToEmail.Checked, null);

                if (updateSuccess)
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
            txtUserFirstName.Text = stringValidation.SanitizeUserInputString(txtUserFirstName.Text, StringValidation.SanitizeEntityNames.Name);
            txtUserLastName.Text = stringValidation.SanitizeUserInputString(txtUserLastName.Text, StringValidation.SanitizeEntityNames.Name);
            Page.Validate("UpdateUser");
            
            return Page.IsValid && heFormValidator.ValidateForm();
        }
    }
}