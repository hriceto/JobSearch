using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;

namespace HristoEvtimov.Websites.Work.Web.Admin.Controls
{
    public partial class blogedit : GeneralControlBase, IFormValidation
    {
        public int BlogId
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["blogid"] != null)
                {
                    int blogId = -1;
                    if (Int32.TryParse(HttpContext.Current.Request.QueryString["blogid"].ToString(), out blogId))
                    {
                        return blogId;
                    }
                }
                return -1;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTopics();
                LoadAuthors();

                bool isUpdate = (BlogId > 0);
                if (isUpdate)
                {
                    BlogManager blogManager = new BlogManager();
                    Blog blog = blogManager.GetBlogAdmin(BlogId);
                    if (blog != null)
                    {
                        PresetBlog(blog);
                    }
                    else
                    {
                        this.AddSystemMessage(GetLocalResourceObject("strEditNotPossible").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.NextLoad);
                        UrlManager urlManager = new UrlManager();
                        Response.Redirect(urlManager.GetUrlRedirectAbsolute("/Admin/BlogAdmin.aspx", null));
                    }
                }
            }

            string script = "";
            script += "CKEDITOR.replace( '" + txtBlogSummary.ClientID + "', {allowedContent:'" + WebConfigurationManager.AppSettings["ALLOWED_HTML_TAGS"] + "'});\n\t";
            script += "CKEDITOR.replace( '" + txtBlogContent.ClientID + "', {allowedContent:'" + WebConfigurationManager.AppSettings["ALLOWED_HTML_TAGS"] + "'});\n\t";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ckeditor", script, true);
        }

        private void PresetBlog(Blog blog)
        {
            if (blog != null)
            {
                txtBlogTitle.Text = blog.Title;
                txtBlogSubTitle.Text = blog.SubTitle;
                txtBlogSummary.Text = blog.Summary;
                txtBlogContent.Text = blog.Content;
                txtBlogSeUrl.Text = blog.SeUrl;
                txtBlogSeTitle.Text = blog.SeTitle;
                txtBlogSeDescription.Text = blog.SeDescription;
                txtBlogKeywords.Text = blog.Keywords;
                chkbBlogPublished.Checked = blog.Published;

                if (blog.PublishDate.HasValue)
                {
                    txtBlogPublishDate.Text = blog.PublishDate.Value.ToString("MM/dd/yyyy");
                }
                else
                {
                    txtBlogPublishDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }

                foreach (BlogTopic blogTopic in blog.BlogTopics)
                {
                    foreach (ListItem topicItem in lstTopics.Items)
                    {
                        if (topicItem.Value == blogTopic.TopicId.ToString())
                        {
                            topicItem.Selected = true;
                        }
                    }
                }
            }
        }

        private void LoadTopics()
        {
            TopicManager topicManager = new TopicManager();
            lstTopics.DataSource = topicManager.GetTopics();
            lstTopics.DataBind();
        }

        private void LoadAuthors()
        {
            UserManager userManager = new UserManager();
            divBlogAuthor.Visible = false;
            if (userManager.GetMembershipUserRole() == UserManager.UserRoles.Admin)
            {
                divBlogAuthor.Visible = true;
                ddlBlogAuthor.DataSource = userManager.GetContributorUsers();
                ddlBlogAuthor.DataBind();
                ddlBlogAuthor.Items.Insert(0, new ListItem(GetLocalResourceObject("ddlBlogAuthor_Item1").ToString(), ""));
            }
        }

        protected void btnAddUpdateBlog_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                List<int> topicAssignments = new List<int>();
                foreach (ListItem topicItem in lstTopics.Items)
                {
                    if (topicItem.Selected)
                    {
                        topicAssignments.Add(Int32.Parse(topicItem.Value));
                    }
                }

                BlogManager blogManager = new BlogManager();
                bool success = false;
                if (BlogId > 0)
                {
                    //update
                    int updatedBlogId = blogManager.UpdateBlog(BlogId, ddlBlogAuthor.SelectedValue, txtBlogTitle.Text,
                        txtBlogSubTitle.Text, txtBlogSummary.Text, txtBlogContent.Text, txtBlogSeUrl.Text,
                        txtBlogSeTitle.Text, txtBlogSeDescription.Text, txtBlogKeywords.Text, topicAssignments, chkbBlogPublished.Checked,
                        txtBlogPublishDate.Text);
                    success = (updatedBlogId == BlogId);
                    if(success)
                    {
                        AddSystemMessage(GetLocalResourceObject("strUpdateBlogSuccess").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.OK,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                    }
                }
                else
                {
                    //insert
                    int newBlogId = blogManager.AddBlog(ddlBlogAuthor.SelectedValue, txtBlogTitle.Text, txtBlogSubTitle.Text,
                        txtBlogSummary.Text, txtBlogContent.Text, txtBlogSeUrl.Text, txtBlogSeTitle.Text, txtBlogSeDescription.Text,
                        txtBlogKeywords.Text, topicAssignments, chkbBlogPublished.Checked, txtBlogPublishDate.Text);
                    success = (newBlogId > 0);
                    if (success)
                    {
                        AddSystemMessage(GetLocalResourceObject("strInsertBlogSuccess").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.OK,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.NextLoad);

                        UrlManager urlManager = new UrlManager();
                        Response.Redirect(urlManager.GetUrlRedirectAbsolute("/Admin/BlogEdit.aspx", new Dictionary<string, string>() { { "blogid", newBlogId.ToString() } }));
                    }
                }

                if(!success)
                {
                    this.AddSystemMessage(GetLocalResourceObject("strBlogSaveError").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.Error,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                }
            }
            else
            {
                AddSystemMessage(GetGlobalResourceObject("GlobalResources", "strFormNotFilledOutProperly").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
            }
        }

        public bool ValidateForm()
        {
            StringValidation stringValidation = new StringValidation();
            txtBlogSeUrl.Text = stringValidation.SanitizeUserInputString(txtBlogSeUrl.Text.Trim(), StringValidation.SanitizeEntityNames.SeUrl).Trim();
            txtBlogKeywords.Text = stringValidation.SanitizeUserInputString(txtBlogKeywords.Text.Trim(), StringValidation.SanitizeEntityNames.Keywords).Trim();
            
            Page.Validate("AddUpdateBlog");            

            return Page.IsValid && heFormValidator.ValidateForm();
        }
    }
}