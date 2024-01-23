using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HristoEvtimov.Websites.Work.Web.Employer
{
    public partial class MyCoupons : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = GetGlobalResourceObject("PageTitles", "strEmployerMyCoupons").ToString();
        }
    }
}