using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    /// <summary>
    /// This control will display a list of topics.
    /// </summary>
    public partial class topiclist : System.Web.UI.UserControl
    {
        UrlManager urlManager = null;
        private string CurrentGroupName = "";

        public topiclist()
        {
            urlManager = new UrlManager();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void LoadTopics()
        {
            TopicManager topicManager = new TopicManager();
            rptrTopics.DataSource = topicManager.GetTopicsActive();
            rptrTopics.DataBind();
        }

        protected void rptrTopics_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Topic topic = (Topic)e.Item.DataItem;
                
                Label lblTopicGroup = e.Item.FindControl("lblTopicGroup") as Label;
                HtmlGenericControl divTopicGroup = e.Item.FindControl("divTopicGroup") as HtmlGenericControl;
                if (lblTopicGroup != null && divTopicGroup != null)
                {
                    if (CurrentGroupName != topic.TopicGroup.Name)
                    {
                        CurrentGroupName = lblTopicGroup.Text = topic.TopicGroup.Name;
                        divTopicGroup.Visible = true;
                    }
                }

                HyperLink hypTopicName = e.Item.FindControl("hypTopicName") as HyperLink;
                if (hypTopicName != null)
                {
                    hypTopicName.Text = topic.Name;
                    hypTopicName.NavigateUrl = TopicManager.GetAbsoluteUrl(urlManager, topic);
                }
            }
        }

    }
}