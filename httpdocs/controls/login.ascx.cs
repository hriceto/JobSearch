using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    /// <summary>
    /// Login control.
    /// </summary>
    public partial class login : GeneralControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pnlMessage.Visible = false;
            if (!IsPostBack)
            {
                if (Membership.GetUser() != null)
                {
                    this.Visible = false;
                    RedirectToHomeAndError("");
                }
                else
                {
                    UrlManager urlManager = new UrlManager();
                    hypRegisterEmployer.NavigateUrl = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.RegisterCompanyUser, null);
                    hypRegisterJobSeeker.NavigateUrl = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.RegisterUser, null);
                    hypForgotPassword.NavigateUrl = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.ResetPassword, null);
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                bool valid = Membership.ValidateUser(txtUserName.Text, txtPassword.Text);
                if (valid)
                {
                    UserManager userManager = new UserManager();
                    userManager.SignIn(txtUserName.Text);
                }
                else
                {
                    //if invalid check if maybe the account is locked 
                    MembershipUser membershipUser = Membership.GetUser(txtUserName.Text);
                    string errorMessage = GetLocalResourceObject("strErrorLogin").ToString();
                    if (membershipUser != null)
                    {
                        if (membershipUser.IsLockedOut)
                        {
                            int lockoutDurationMinutes = Int32.Parse(WebConfigurationManager.AppSettings["LOCKOUT_DURATION"]);
                            DateTime autoUnlockDate = membershipUser.LastLockoutDate.AddMinutes(lockoutDurationMinutes);
                            if (autoUnlockDate > DateTime.Now)
                            {
                                //if still locked show an error message
                                TimeSpan lockout = autoUnlockDate.Subtract(DateTime.Now);
                                errorMessage = String.Format(GetLocalResourceObject("strErrorLockedOut").ToString(), lockout.Hours, lockout.Minutes);
                            }
                            else
                            {
                                //if account was locked but the lock time period has expired unlock
                                //the account and try to log them in again.
                                errorMessage = GetLocalResourceObject("strErrorLogin").ToString();
                                membershipUser.UnlockUser();
                                valid = Membership.ValidateUser(txtUserName.Text, txtPassword.Text);
                                if (valid)
                                {
                                    UserManager userManager = new UserManager();
                                    userManager.SignIn(txtUserName.Text);
                                }
                            }
                        }
                    }
                    pnlMessage.Visible = true;
                    lblMessage.Text = errorMessage;
                }
            }
        }
    }
}