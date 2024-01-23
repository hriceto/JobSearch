using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.Web
{
    public partial class Unsubscribe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = GetGlobalResourceObject("PageTitles", "strUnsubscribe").ToString();
            if (!IsPostBack)
            {
                User user = VerifyUnsubscribeCodeAndGetUser();

                if (user != null)
                {
                    lblUnsubscribe.Text = String.Format(GetLocalResourceObject("strUnsubscribe").ToString(), user.Email);
                }
            }
        }

        protected void btnUnsubscribe_Click(object sender, EventArgs e)
        {
            lblSuccess.Visible = false;
            lblFailure.Visible = false;

            User user = VerifyUnsubscribeCodeAndGetUser();
            UserManager userManger = new UserManager();
            if (userManger.UpdateUserUnsubscribe(user))
            {
                lblSuccess.Visible = true;
            }
            else
            {
                lblFailure.Visible = true;
            }
        }

        public User VerifyUnsubscribeCodeAndGetUser()
        {
            User user = null;

            if (Request.QueryString["unsubscribeid"] != null)
            {
                Guid unsubscribeId;
                if (Guid.TryParse(Request.QueryString["unsubscribeid"].ToString(), out unsubscribeId))
                {
                    UserManager userManager = new UserManager();
                    user = userManager.GetUnsubscribeUser(unsubscribeId);
                    if (user == null)
                    {
                        DisplayError();
                        return null;
                    }

                    string email = "";
                    if (Request.QueryString["email"] != null)
                    {
                        email = Server.UrlDecode(Request.QueryString["email"].ToString());
                    }

                    if (String.IsNullOrEmpty(email))
                    {
                        DisplayError();
                        return null;
                    }

                    if (email != user.Email)
                    {
                        DisplayError();
                        return null;
                    }
                }
            }
            return user;
        }

        public void DisplayError()
        {
            lblUnsubscribe.Text = GetLocalResourceObject("strUnsubscribeFail").ToString();
            btnUnsubscribe.Visible = false;
        }
    }
}