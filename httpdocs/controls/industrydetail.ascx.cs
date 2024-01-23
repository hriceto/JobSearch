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
    public partial class industrydetail : System.Web.UI.UserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request.QueryString["categoryid"] != null)
            {
                int categoryId = 0;
                if (Int32.TryParse(Request.QueryString["categoryid"].ToString(), out categoryId))
                {
                    heLatestJobs.CategoryId = categoryId;
                    heLatestJobs.NumberOfItems = 50;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["categoryid"] != null)
            {
                int categoryId = 0;
                if (Int32.TryParse(Request.QueryString["categoryid"].ToString(), out categoryId))
                {
                    DisplayIndustry(categoryId);
                }
            }
        }

        private void DisplayIndustry(int categoryId)
        {
            CategoryManager categoryManager = new CategoryManager();
            Category category = categoryManager.GetCategory(categoryId);
            UrlManager urlManager = new UrlManager();

            if (category != null)
            {
                string qsSeUrl = "";
                if (Request.QueryString["seurl"] != null)
                {
                    qsSeUrl = Request.QueryString["seurl"].ToString();
                }

                if (!String.IsNullOrEmpty(category.SeUrl))
                {
                    if (!category.SeUrl.Equals(qsSeUrl))
                    {
                        Response.RedirectPermanent(urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.Industries,
                            new Dictionary<string, string>() {{"categoryid", categoryId.ToString()}, 
                                {"seurl", category.SeUrl}}
                            ), true);
                    }
                }

                litTitle.Text = String.Format(GetLocalResourceObject("litTitle").ToString(), 
                    urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.Industries, null),
                    category.SeTitle);

                this.Page.Title = Server.HtmlEncode(String.Format(GetLocalResourceObject("strTitle").ToString(), category.SeTitle));
                if (String.IsNullOrEmpty(category.SeDescription))
                {
                    //this.Page.MetaDescription = category.SeDescription;
                }
            }
        }
    }
}