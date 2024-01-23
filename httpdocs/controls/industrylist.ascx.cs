using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    public partial class industrylist : System.Web.UI.UserControl
    {
        private UrlManager urlManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            urlManager = new UrlManager();
            LoadIndustries();
        }

        private void LoadIndustries()
        {
            CategoryManager categoryManager = new CategoryManager();
            rptrIndustries.DataSource = categoryManager.GetCategories();
            rptrIndustries.DataBind();
        }

        protected void rptrIndustries_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Category category = (Category)e.Item.DataItem;

                HyperLink hypIndustry = e.Item.FindControl("hypIndustry") as HyperLink;
                if (hypIndustry != null)
                {
                    hypIndustry.Text = String.Format("{0} ({1})", category.Name, category.NumberOfActiveJobs.ToString());
                    hypIndustry.NavigateUrl = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.Industries,
                        new Dictionary<string, string>() { 
                            {"categoryid", category.CategoryId.ToString()},
                            {"seurl", category.SeUrl}
                        });
                }
            }
        }
    }
}