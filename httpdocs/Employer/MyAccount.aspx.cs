using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkLibrary;

namespace HristoEvtimov.Websites.Work.Web.Employer
{
    public partial class MyAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = GetGlobalResourceObject("PageTitles", "strEmployerMyAccount").ToString();
            UrlManager urlManager = new UrlManager();
            hypMyCoupons.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Employer/MyCoupons.aspx", null);
        }
    }
}