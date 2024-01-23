using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Security;

namespace HristoEvtimov.Websites.Work.Web
{
    public partial class Employers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = GetGlobalResourceObject("PageTitles", "strEmployers").ToString();

            UrlManager urlManager = new UrlManager();
            string registerLink = urlManager.GetUrlRedirectAbsolute("/RegisterCompanyUser.aspx", null);

            hypRegister2.NavigateUrl = hypRegister.NavigateUrl = registerLink;
            lblAcceptingApplications.Text = String.Format(GetLocalResourceObject("strAcceptingApplications2").ToString(), registerLink);

            lblPublishWithUs.Text = String.Format(GetLocalResourceObject("strPublishWithUs").ToString(), 
                urlManager.GetWwwUrlRedirectAbsolute(UrlManager.PageLink.Pricing, null));
        }
    }
}