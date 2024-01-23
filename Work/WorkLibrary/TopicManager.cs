using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class TopicManager
    {
        public List<Topic> GetTopics()
        {
            return GetTopics(false);
        }

        public List<Topic> GetTopics(bool refreshFromDatabase)
        {
            TopicDataAccess tda = new TopicDataAccess();
            return tda.GetTopics(refreshFromDatabase);
        }

        public List<Topic> GetTopicsActive()
        {
            return GetTopicsActive(false);
        }

        public List<Topic> GetTopicsActive(bool refreshFromDatabase)
        {
            TopicDataAccess tda = new TopicDataAccess();
            return tda.GetTopicsActive(refreshFromDatabase);
        }

        public List<TopicGroup> GetTopicGroupsActive(bool refreshFromDatabase)
        {
            TopicDataAccess tda = new TopicDataAccess();
            return tda.GetTopicGroupsActive(refreshFromDatabase);
        }

        public List<Topic> GetTopicsByGroup(string topicGroupName, bool refreshFromDatabase)
        {
            TopicDataAccess tda = new TopicDataAccess();
            return tda.GetTopicsByGroup(topicGroupName, refreshFromDatabase);
        }

        public List<Topic> GetTopicsByGroupActive(string topicGroupName, bool refreshFromDatabase)
        {
            TopicDataAccess tda = new TopicDataAccess();
            return tda.GetTopicsByGroupActive(topicGroupName, refreshFromDatabase);
        }

        public Topic GetTopic(int topicId)
        {
            TopicDataAccess tda = new TopicDataAccess();
            return tda.GetTopic(topicId);
        }

        public void UpdateTopicCounts()
        {
            try
            {
                TopicDataAccess tda = new TopicDataAccess();
                tda.UpdateTopicCounts();
            }
            catch (System.Exception ex)
            {
                ExceptionManager exceptionManager = new ExceptionManager();
                exceptionManager.AddException(ex);
            }
        }

        #region static
        public static string GetAbsoluteUrl(UrlManager urlManager, Topic topic)
        {
            return urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.BlogList,
                        new Dictionary<string, string>() { 
                            { "topicid", topic.TopicId.ToString() },
                            { "seurl", topic.SeUrl.Trim() }
                        });
        }
        #endregion
    }
}
