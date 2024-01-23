using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class BlogDataAccess : DataAccess
    {
        public int AddBlog(Blog blog)
        {
            int result = -1;

            using (WorkEntities context = GetContext())
            {
                context.Blogs.AddObject(blog);
                if (context.SaveChanges() >= 1)
                {
                    result = blog.BlogId;
                }
            }

            return result;
        }

        public bool UpdateBlog(Blog updateBlog)
        {
            return UpdateBlog(updateBlog, null, null);
        }

        public bool UpdateBlog(Blog updateBlog, List<BlogTopic> topicsToRemove, List<BlogTopic> topicsToAdd)
        {
            bool result = false;

            using (WorkEntities context = GetContext())
            {
                if (updateBlog != null)
                {
                    context.Blogs.Attach(updateBlog);
                    context.ObjectStateManager.ChangeObjectState(updateBlog, System.Data.EntityState.Modified);

                    if (topicsToRemove != null)
                    {
                        foreach (BlogTopic topicToRemove in topicsToRemove)
                        {
                            context.DeleteObject(topicToRemove);
                        }
                    }

                    if (topicsToAdd != null)
                    {
                        foreach (BlogTopic topicToAdd in topicsToAdd)
                        {
                            updateBlog.BlogTopics.Add(topicToAdd);
                        }
                    }

                    result = (context.SaveChanges() >= 1);
                }
            }
            return result;
        }

        public Blog GetBlogAdmin(int blogId)
        {
            using (WorkEntities context = GetContext())
            {
                var blogQuery = from b in context.Blogs.Include("BlogTopics").Include("BlogTopics.Topic") where b.BlogId == blogId select b;
                return blogQuery.FirstOrDefault();
            }
        }

        public Blog GetBlogAdmin(int blogId, int currentUserId)
        {
            using (WorkEntities context = GetContext())
            {
                var blogQuery = from b in context.Blogs.Include("BlogTopics")
                                where b.BlogId == blogId && 
                                b.UserId == currentUserId select b;
                return blogQuery.FirstOrDefault();
            }
        }

        public Blog GetBlogForDetailPage(int blogId)
        {
            using (WorkEntities context = GetContext())
            {
                var blogQuery = from b in context.Blogs.Include("BlogTopics").Include("User").Include("BlogTopics.Topic") 
                                where b.BlogId == blogId && b.Published  == true && b.PublishDate <= DateTime.Now
                                select b;
                return blogQuery.FirstOrDefault();
            }
        }

        public List<Blog> GetBlogsAdmin(int page, int pageSize, out int totalNumberOfResults)
        {
            using (WorkEntities context = GetContext())
            {
                var blogQuery = from b in context.Blogs.Include("BlogTopics")
                                orderby b.CreatedDate descending
                                select b;

                totalNumberOfResults = blogQuery.Count();
                return blogQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public List<Blog> GetBlogsAdmin(int currentUserId, int page, int pageSize, out int totalNumberOfResults)
        {
            using (WorkEntities context = GetContext())
            {
                var blogQuery = from b in context.Blogs.Include("BlogTopics")
                                where b.UserId == currentUserId
                                orderby b.CreatedDate descending
                                select b;

                totalNumberOfResults = blogQuery.Count();
                return blogQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public List<Blog> GetBlogsForList(int page, int pageSize, out int totalNumberOfResults)
        {
            using (WorkEntities context = GetContext())
            {
                var blogQuery = from b in context.Blogs.Include("BlogTopics").Include("User")
                                where b.Published == true && b.PublishDate <= DateTime.Now
                                orderby b.CreatedDate descending
                                select b;

                totalNumberOfResults = blogQuery.Count();
                return blogQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public List<Blog> GetBlogsForListByTopic(int topicId, int page, int pageSize, out int totalNumberOfResults)
        {
            using (WorkEntities context = GetContext())
            {
                var blogQuery = from b in context.Blogs.Include("BlogTopics").Include("User")
                                where b.Published == true && b.PublishDate <= DateTime.Now && 
                                b.BlogTopics.Any(bt => bt.TopicId == topicId)
                                orderby b.CreatedDate descending
                                select b;

                totalNumberOfResults = blogQuery.Count();
                return blogQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }
    }
}
