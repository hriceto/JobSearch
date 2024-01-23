using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HristoEvtimov.Websites.Work.WorkLibrary.UserControls
{
    public class JobSearchInput : UserControl
    {
        public TextBox txtSearch;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            UrlManager urlManager = new UrlManager();
            string term = HttpContext.Current.Server.UrlEncode(txtSearch.Text);
            HttpContext.Current.Response.Redirect(urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.JobSearch, new Dictionary<string, string>() { { "term", term } }));
        }
    }
}
