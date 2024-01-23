using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;

namespace HristoEvtimov.Websites.Work.Web
{
    public partial class Pricing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UrlManager urlManager = new UrlManager();
            string redirectUrl = urlManager.GetWwwUrlRedirectAbsolute(UrlManager.PageLink.Pricing, null);
            Response.Status = "301 Moved Permanently";
            Response.AddHeader("Location", redirectUrl);
            Response.End();
        }
    }
}