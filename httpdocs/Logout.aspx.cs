using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;

namespace HristoEvtimov.Websites.Work.Web
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = GetGlobalResourceObject("PageTitles", "strLogout").ToString();

            UrlManager urlManager = new UrlManager();
            string loginLink = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.Login, null);
            litLogin.Text = String.Format(GetLocalResourceObject("strLogin").ToString(), loginLink);
        }
    }
}