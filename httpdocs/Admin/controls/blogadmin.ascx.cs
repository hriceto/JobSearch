using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;

namespace HristoEvtimov.Websites.Work.Web.Admin.Controls
{
    public partial class blogadmin : GeneralControlBase, IFormValidation
    {
        UrlManager urlManager = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            urlManager = new UrlManager();
            hypAddNewBlog.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Admin/BlogEdit.aspx", null);

            if (!IsPostBack)
            {
                BindBlogs();
            }
        }

        private void BindBlogs()
        {
            BlogManager blogManager = new BlogManager();
            User currentUser = GetUser();
            int totalNumberOfResults = 0;
            
            rptrBlogs.DataSource = blogManager.GetBlogsAdmin(hePaging.CurrentPage, hePaging.PageSize, out totalNumberOfResults);
            rptrBlogs.DataBind();

            hePaging.TotalNumberOfItems = totalNumberOfResults;
            hePaging.BuildPaging();
        }

        protected void rptrBlogs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Blog blog = (Blog)e.Item.DataItem;

                HyperLink hypEditBlog = e.Item.FindControl("hypEditBlog") as HyperLink;
                if (hypEditBlog != null)
                {
                    hypEditBlog.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Admin/BlogEdit.aspx", 
                        new Dictionary<string, string>() { {"blogid", blog.BlogId.ToString()} });
                }

                HyperLink hypViewBlog = e.Item.FindControl("hypViewBlog") as HyperLink;
                if (hypViewBlog != null)
                {
                    hypViewBlog.NavigateUrl = BlogManager.GetAbsoluteUrl(urlManager, blog);
                    hypViewBlog.Text = blog.SeUrl;
                }
            }
        }

        public bool ValidateForm()
        {
            return Page.IsValid;
        }
    }
}