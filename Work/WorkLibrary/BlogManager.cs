using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class BlogManager
    {
        public int AddBlog(string blogAuthor, string blogTitle, string blogSubTitle, string blogSummary, string blogContent, string blogSeUrl, string blogSeTitle, string blogSeDescription, string blogKeywords, List<int> topicAssignments, bool published, string publishDate)
        {
            return AddUpateBlog(-1, blogAuthor, blogTitle, blogSubTitle, blogSummary, blogContent, blogSeUrl, blogSeTitle, blogSeDescription, blogKeywords, topicAssignments, published, publishDate);
        }

        public int UpdateBlog(int blogId, string blogAuthor, string blogTitle, string blogSubTitle, string blogSummary, string blogContent, string blogSeUrl, string blogSeTitle, string blogSeDescription, string blogKeywords, List<int> topicAssignments, bool published, string publishDate)
        {
            return AddUpateBlog(blogId, blogAuthor, blogTitle, blogSubTitle, blogSummary, blogContent, blogSeUrl, blogSeTitle, blogSeDescription, blogKeywords, topicAssignments, published, publishDate);
        }

        private int AddUpateBlog(int blogId, string blogAuthor, string blogTitle, string blogSubTitle, string blogSummary, string blogContent, 
            string blogSeUrl, string blogSeTitle, string blogSeDescription, string blogKeywords, List<int> topicAssignments, bool published, string publishDate)
        {
            int result = -1;
            //Do lots of validation here. This data could be anything as request validation is false to allow some html input.
            HtmlParser htmlParserAllowSomeHtml = new HtmlParser(HtmlParser.ParseRules.AllowSomeHtml);
            HtmlParser htmlParserNoHtml = new HtmlParser(HtmlParser.ParseRules.FilterOutHtml);

            BlogDataAccess bda = new BlogDataAccess();

            string originalBlogText = String.Format("TITLE:|{0}|SUBTITLE:|{1}|SUMMARY:|{2}|CONTENT:|{3}|SEURL:|{4}|SETITLE:|{5}|SEDESCRIPTION:|{6}|KEYWORDS:|{7}|",
                blogTitle.Trim(), blogSubTitle.Trim(), blogSummary.Trim(), blogContent.Trim(), 
                blogSeUrl.Trim(), blogSeTitle.Trim(), blogSeDescription.Trim(), blogKeywords.Trim());

            //filter out html
            blogTitle = htmlParserNoHtml.FilterHtml(blogTitle.Trim());
            blogSubTitle = htmlParserNoHtml.FilterHtml(blogSubTitle.Trim());
            blogSummary = htmlParserAllowSomeHtml.FilterHtml(blogSummary.Trim());
            blogContent = htmlParserAllowSomeHtml.FilterHtml(blogContent.Trim());
            blogSeUrl = htmlParserNoHtml.FilterHtml(blogSeUrl.Trim());
            blogSeTitle = htmlParserNoHtml.FilterHtml(blogSeTitle.Trim());
            blogSeDescription = htmlParserNoHtml.FilterHtml(blogSeDescription.Trim());
            blogKeywords = htmlParserNoHtml.FilterHtml(blogKeywords.Trim());

            UserManager userManager = new UserManager();
            User currentUser = userManager.GetUser();

            Blog blog = null;
            bool isUpdate = (blogId > 0);
            if (isUpdate)
            {
                if (userManager.GetMembershipUserRole() == UserManager.UserRoles.Admin)
                {
                    blog = bda.GetBlogAdmin(blogId);
                }
                else
                {
                    blog = bda.GetBlogAdmin(blogId, currentUser.UserId);
                }
                
                if (blog == null)
                {
                    return -1;
                }
                blog.Title = blogTitle;
                blog.Content = blogContent;
                blog.SeUrl = blogSeUrl;
                blog.SeTitle = blogSeTitle;
                
                if (!blog.Published && published)
                {
                    blog.PublishByUserId = currentUser.UserId;
                    DateTime dPublishDate;
                    if (DateTime.TryParse(publishDate, out dPublishDate))
                    {
                        blog.PublishDate = dPublishDate;
                    }
                }
            }
            else
            {
                blog = Blog.CreateBlog(-1,
                   currentUser.UserId,
                   blogTitle,
                   blogContent,
                   blogSeUrl,
                   blogSeTitle,
                   0,
                   DateTime.Now,
                   DateTime.Now,
                   published,
                   currentUser.UserId
                   );
                blog.PublishByUserId = currentUser.UserId;
                
                DateTime dPublishDate;
                if (DateTime.TryParse(publishDate, out dPublishDate))
                {
                    blog.PublishDate = dPublishDate;
                }
            }

            blog.SubTitle = blogSubTitle;
            blog.Summary = blogSummary;
            blog.SeDescription = blogSeDescription;
            blog.Keywords = blogKeywords;
            blog.UpdatedDate = DateTime.Now;
            blog.Published = published;

            if (userManager.GetMembershipUserRole() == UserManager.UserRoles.Admin)
            {
                if (!String.IsNullOrEmpty(blogAuthor))
                {
                    blog.UserId = Int32.Parse(blogAuthor);
                }
            }

            //Some html was stripped out. Add a log entry.
            if (htmlParserAllowSomeHtml.InvalidHtmlWasPresent || htmlParserNoHtml.InvalidHtmlWasPresent)
            {
                LogManager logManager = new LogManager();
                logManager.AddLog("HTML stripped from BLOG", currentUser.UserId, "blogid=" + blog.BlogId.ToString(), originalBlogText);
            }

            //deal with topic assignments
            List<BlogTopic> topicsToAdd = new List<BlogTopic>();
            List<BlogTopic> topicsToRemove = new List<BlogTopic>();
            if (isUpdate)
            {
                //figure out which topics to remove
                foreach (BlogTopic blogTopic in blog.BlogTopics)
                {
                    if (!topicAssignments.Contains(blogTopic.TopicId))
                    {
                        topicsToRemove.Add(blogTopic);
                    }
                }

                //new topic assignments to add
                foreach (int topicAssignment in topicAssignments)
                {
                    bool assignmentAlreadyExists = false;
                    foreach (BlogTopic blogTopic in blog.BlogTopics)
                    {
                        if (blogTopic.TopicId == topicAssignment)
                        {
                            assignmentAlreadyExists = true;
                            break;
                        }
                    }
                    if (!assignmentAlreadyExists)
                    {
                        topicsToAdd.Add(BlogTopic.CreateBlogTopic(-1, -1, topicAssignment));
                    }
                }
            }
            else
            {
                //add new category assignments
                foreach (int topicAssignment in topicAssignments)
                {
                    blog.BlogTopics.Add(BlogTopic.CreateBlogTopic(-1, -1, topicAssignment));
                }
            }

            //perform update or delete
            if (isUpdate)
            {
                bool updateOK = bda.UpdateBlog(blog, topicsToRemove, topicsToAdd);
                if (updateOK)
                {
                    result = blog.BlogId;
                }
            }
            else
            {
                result = bda.AddBlog(blog);
            }

            TopicManager topicManager = new TopicManager();
            topicManager.UpdateTopicCounts();

            return result;
        }

        public Blog GetBlogAdmin(int blogId)
        {
            BlogDataAccess bda = new BlogDataAccess();
            UserManager userManager = new UserManager();
            User currentUser = userManager.GetUser();
            if (userManager.GetMembershipUserRole() == UserManager.UserRoles.Admin)
            {
                return bda.GetBlogAdmin(blogId);
            }
            else
            {
                return bda.GetBlogAdmin(blogId, currentUser.UserId);
            }
        }

        public Blog GetBlogForDetailPage(int blogId)
        {
            BlogDataAccess bda = new BlogDataAccess();
            UserManager userManager = new UserManager();
            User currentUser = userManager.GetUser();
            if (userManager.GetMembershipUserRole() == UserManager.UserRoles.Admin)
            {
                return bda.GetBlogAdmin(blogId);
            }
            else
            {
                return bda.GetBlogForDetailPage(blogId);
            }
        }

        public List<Blog> GetBlogsAdmin(int page, int pageSize, out int totalNumberOfResults)
        {
            BlogDataAccess bda = new BlogDataAccess();
            UserManager userManager = new UserManager();
            User currentUser = userManager.GetUser();
            if (userManager.GetMembershipUserRole() == UserManager.UserRoles.Admin)
            {
                return bda.GetBlogsAdmin(page, pageSize, out totalNumberOfResults);
            }
            else
            {
                return bda.GetBlogsAdmin(currentUser.UserId, page, pageSize, out totalNumberOfResults);
            }
        }

        public List<Blog> GetBlogsForList(int topicId, int page, int pageSize, out int totalNumberOfResults)
        {
            BlogDataAccess bda = new BlogDataAccess();
            if (topicId > 0)
            {
                return bda.GetBlogsForListByTopic(topicId, page, pageSize, out totalNumberOfResults);
            }
            else
            {
                return bda.GetBlogsForList(page, pageSize, out totalNumberOfResults);
            }
        }

        #region static
        public static string GetAbsoluteUrl(UrlManager urlManager, Blog blog)
        {
            return urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.BlogDetail,
                        new Dictionary<string, string>() { { "blogid", blog.BlogId.ToString() }, { "seurl", blog.SeUrl.Trim() } });
        }
        #endregion

    }
}
