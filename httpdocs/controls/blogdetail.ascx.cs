using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using System.Text;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    /// <summary>
    /// This control will display the blog details.
    /// </summary>
    public partial class blogdetail : System.Web.UI.UserControl
    {
        UrlManager urlManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            urlManager = new UrlManager();

            if (Request.QueryString["blogid"] != null)
            {
                int blogId = 0;
                if (Int32.TryParse(Request.QueryString["blogid"].ToString(), out blogId))
                {
                    DisplayBlog(blogId);
                    LoadTopics();
                }
            }
        }

        public void DisplayBlog(int blogId)
        {
            BlogManager blogManager = new BlogManager();
            Blog blog = blogManager.GetBlogForDetailPage(blogId);

            if (blog != null)
            {
                string qsSeUrl = "";
                if (Request.QueryString["seurl"] != null)
                {
                    qsSeUrl = Request.QueryString["seurl"].ToString();
                }

                if (!String.IsNullOrEmpty(blog.SeUrl))
                {
                    if (!blog.SeUrl.Equals(qsSeUrl))
                    {
                        Response.RedirectPermanent(BlogManager.GetAbsoluteUrl(urlManager, blog), true);
                    }
                }

                lblTitle.Text = blog.Title;
                if (blog.PublishDate.HasValue) { lblPublishDate.Text = blog.PublishDate.Value.ToString("MM/dd/yyyy"); }
                lblSubTitle.Text = blog.SubTitle;
                lblSummary.Text = blog.Summary;
                lblContent.Text = blog.Content;

                StringBuilder sbTopics = new StringBuilder();
                string separator = "";
                foreach (BlogTopic blogTopic in blog.BlogTopics)
                {
                    sbTopics.Append(separator);
                    sbTopics.AppendFormat("<a href=\"{0}\">{1}</a>", TopicManager.GetAbsoluteUrl(urlManager, blogTopic.Topic), 
                        blogTopic.Topic.Name);
                    separator = ", ";
                }
                lblAuthor.Text = String.Format(GetLocalResourceObject("strAuthorBy").ToString(), blog.User.FirstName,
                    blog.User.LastName, sbTopics.ToString());

                this.Page.Title = Server.HtmlEncode(GetGlobalResourceObject("PageTitles", "strBlogDetailPrefix").ToString() + " " + blog.Title);
                this.Page.MetaDescription = blog.SeDescription;

                
            }
            else
            {
                plhDetail.Visible = false;
                plhError.Visible = true;
                this.Page.Title = GetGlobalResourceObject("PageTitles", "strJobDetail").ToString();
                this.Page.MetaDescription = "";

                Response.TrySkipIisCustomErrors = true;
                Response.Status = "404 Not Found";
                Response.StatusCode = 404;            
            }
        }

        private void LoadTopics()
        {
            heTopicList.LoadTopics();
        }
    }
}