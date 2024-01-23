using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class TopicDataAccess : DataAccess
    {
        private const string cacheKey = "Topic";
        private const string cacheKeyActive = "TopicActive";

        public List<Topic> GetTopics(bool refreshFromDatabase)
        {
            if (HttpContext.Current.Cache[cacheKey] != null && !refreshFromDatabase)
            {
                return (List<Topic>)HttpContext.Current.Cache[cacheKey];
            }
            else
            {
                lock (cacheKey)
                {
                    if (HttpContext.Current.Cache[cacheKey] != null && !refreshFromDatabase)
                    {
                        return (List<Topic>)HttpContext.Current.Cache[cacheKey];
                    }
                    else
                    {
                        using (WorkEntities context = GetContext())
                        {
                            var topicsQuery = from t in context.Topics.Include("TopicGroup") orderby t.TopicGroup.Name, t.Name ascending select t;
                            var topics = topicsQuery.ToList();
                            HttpContext.Current.Cache.Insert(cacheKey, topics, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 30, 0));
                            return topics;
                        }
                    }
                }
            }
        }

        public List<Topic> GetTopicsActive(bool refreshFromDatabase)
        {
            if (HttpContext.Current.Cache[cacheKeyActive] != null && !refreshFromDatabase)
            {
                return (List<Topic>)HttpContext.Current.Cache[cacheKeyActive];
            }
            else
            {
                lock (cacheKeyActive)
                {
                    if (HttpContext.Current.Cache[cacheKeyActive] != null && !refreshFromDatabase)
                    {
                        return (List<Topic>)HttpContext.Current.Cache[cacheKeyActive];
                    }
                    else
                    {
                        using (WorkEntities context = GetContext())
                        {
                            var topicsQuery = from t in context.Topics.Include("TopicGroup") 
                                              where t.NumberOfBlogs > 0
                                              orderby t.TopicGroup.Name, t.Name ascending select t;
                            var topics = topicsQuery.ToList();
                            HttpContext.Current.Cache.Insert(cacheKeyActive, topics, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 30, 0));
                            return topics;
                        }
                    }
                }
            }
        }

        public List<Topic> GetTopicsByGroup(string topicGroupName, bool refreshFromDatabase)
        {
            List<Topic> topics = GetTopics(refreshFromDatabase);
            return topics.Where(n => n.TopicGroup.Name == topicGroupName).ToList();
        }

        public List<Topic> GetTopicsByGroupActive(string topicGroupName, bool refreshFromDatabase)
        {
            List<Topic> topics = GetTopicsActive(refreshFromDatabase);
            return topics.Where(n => n.TopicGroup.Name == topicGroupName).ToList();
        }

        public List<TopicGroup> GetTopicGroupsActive(bool refreshFromDatabase)
        {
            List<Topic> topics = GetTopicsActive(refreshFromDatabase);
            return topics.Select(n => n.TopicGroup).ToList();
        }

        public Topic GetTopic(int topicId)
        {
            using (WorkEntities context = GetContext())
            {
                var topicQuery = from t in context.Topics where t.TopicId == topicId select t;
                return topicQuery.FirstOrDefault();
            }
        }

        public bool UpdateTopic(Topic topic)
        {
            bool result = false;
            using (WorkEntities context = GetContext())
            {
                if (topic != null)
                {
                    context.Topics.Attach(topic);
                    context.ObjectStateManager.ChangeObjectState(topic, System.Data.EntityState.Modified);
                    result = (context.SaveChanges() == 1);
                }
            }
            return result;
        }

        public void UpdateTopicCounts()
        {
            List<Topic> topics = GetTopics(true);
            using (WorkEntities context = GetContext())
            {
                foreach (Topic topic in topics)
                {
                    if (topic != null)
                    {
                        var blogCountQuery = from bt in context.BlogTopics where bt.TopicId == topic.TopicId select bt.BlogId;
                        topic.NumberOfBlogs = blogCountQuery.Distinct().Count(); 

                        context.Topics.Attach(topic);
                        context.ObjectStateManager.ChangeObjectState(topic, System.Data.EntityState.Modified);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
