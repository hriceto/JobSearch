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
    /// <summary>
    /// This control will display the blog details.
    /// </summary>
    public partial class bloglist : System.Web.UI.UserControl
    {
        UrlManager urlManager = null;

        public int TopicId
        {
            get
            {
                if (Request.QueryString["topicid"] != null)
                {
                    int _topicId = 0;
                    if (Int32.TryParse(Request.QueryString["topicid"].ToString(), out _topicId))
                    {
                        return _topicId;
                    }
                }
                return -1;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            urlManager = new UrlManager();
            
            LoadBlogs();
            LoadTopics();
            LoadPageInfo();
        }

        private void LoadBlogs()
        {
            BlogManager blogManager = new BlogManager();
            int totalNumberOfResults = 0;

            rptrBlogs.DataSource = blogManager.GetBlogsForList(TopicId, hePaging.CurrentPage, hePaging.PageSize, out totalNumberOfResults);
            rptrBlogs.DataBind();

            hePaging.TotalNumberOfItems = totalNumberOfResults;
            hePaging.BuildPaging();
        }

        private void LoadTopics()
        {
            heTopicList.LoadTopics();
        }

        private void LoadPageInfo()
        {
            if (TopicId > 0)
            {
                TopicManager topicManager = new TopicManager();
                Topic topic = topicManager.GetTopic(TopicId);

                string qsSeUrl = "";
                if (Request.QueryString["seurl"] != null)
                {
                    qsSeUrl = Request.QueryString["seurl"].ToString();
                }

                if (!String.IsNullOrEmpty(topic.SeUrl) && !topic.SeUrl.Equals(qsSeUrl))
                {
                    UrlManager urlManager = new UrlManager();
                    Response.RedirectPermanent(TopicManager.GetAbsoluteUrl(urlManager, topic), true);
                }
                lblBlogHeading.Text = topic.SeTitle;
                this.Page.Title = Server.HtmlEncode(GetGlobalResourceObject("PageTitles", "strBlogListPrefix").ToString() + " " + topic.SeTitle);
                this.Page.MetaDescription = topic.SeDescription;
                if (!String.IsNullOrEmpty(topic.Description))
                {
                    h3Description.Visible = true;
                    lblBlogDescription.Text = topic.Description;
                }
            }
            else
            {
                lblBlogHeading.Text = GetLocalResourceObject("strBlog").ToString();
                this.Page.Title = GetGlobalResourceObject("PageTitles", "strBlogList").ToString();
                this.Page.MetaDescription = GetGlobalResourceObject("PageDescriptions", "strBlogList").ToString();
            }
        }

        protected void rptrBlogs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Blog blog = (Blog)e.Item.DataItem;

                HyperLink hypBlogTitle = e.Item.FindControl("hypBlogTitle") as HyperLink;
                if (hypBlogTitle != null)
                {
                    hypBlogTitle.Text = blog.Title;
                    hypBlogTitle.NavigateUrl = BlogManager.GetAbsoluteUrl(urlManager, blog);
                }
            }
        }
    }
}