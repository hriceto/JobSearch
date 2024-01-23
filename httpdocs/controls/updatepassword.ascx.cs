using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    public partial class updatepassword : GeneralControlBase, IFormValidation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            User user = GetUser();
            if (user == null)
            {
                RedirectToHomeAndError("strUserNotLoggedIn");
            }
        }

        protected void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                UserManager userManager = new UserManager();
                bool passwordUpdated = userManager.UpdatePassword(txtCurrentPassword.Text, txtNewPassword.Text);
                if (passwordUpdated)
                {
                    this.AddSystemMessage(GetLocalResourceObject("strPasswordUpdatedSuccess").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.OK,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                }
                else
                {
                    this.AddSystemMessage(GetLocalResourceObject("strPasswordUpdatedFailure").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.Error,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                }
            }
        }

        public bool ValidateForm()
        {
            return Page.IsValid && heFormValidator.ValidateForm();
        }
    }
}