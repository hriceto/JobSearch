using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HristoEvtimov.Websites.Work.Web
{
    public partial class Industries : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            int categoryId = 0;

            if (Request.QueryString["categoryid"] != null)
            {
                Int32.TryParse(Request.QueryString["categoryid"].ToString(), out categoryId);
            }

            if (categoryId <= 0)
            {
                this.Title = GetGlobalResourceObject("PageTitles", "strIndustries").ToString();
            }

            LoadCategoryControl(categoryId);
        }

        private void LoadCategoryControl(int categoryId)
        {
            string controlToLoad = "~/controls/industrydetail.ascx";
            if (categoryId <= 0)
            {
                controlToLoad = "~/controls/industrylist.ascx";
            }

            Control industryControl = LoadControl(controlToLoad);
            plhIndustryControl.Controls.Add(industryControl);
        }
    }
}