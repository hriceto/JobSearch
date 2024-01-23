using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary.BusinessObjects;

namespace HristoEvtimov.Websites.Work.Web.Employer.Controls
{
    public partial class addeditjob : JobEditControlBase, IFormValidation
    {
        private enum AddUpdateAction { GoToMyJobs, GoToPublish }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnAddUpdate.CommandArgument = AddUpdateAction.GoToMyJobs.ToString();
                btnAddUpdatePublish.CommandArgument = AddUpdateAction.GoToPublish.ToString();

                EmploymentTypeManager employmentTypeManager = new EmploymentTypeManager();
                ddlEmploymentType.DataSource = employmentTypeManager.GetEmploymentTypes();
                ddlEmploymentType.DataBind();

                CategoryManager categoryManager = new CategoryManager();
                lstCategories.DataSource = categoryManager.GetCategories();
                lstCategories.DataBind();

                divJobBenefits.Attributes.Add("style", "display:none;");
                divJobRequirements.Attributes.Add("style", "display:none;");

                bool isUpdate = (GetEditJobId() > 0);
                if (isUpdate)
                {
                    JobPost jobPost = GetEditJob();
                    if (jobPost != null)
                    {
                        PresetJob(jobPost);
                    }
                    else
                    {
                        this.AddSystemMessage(GetGlobalResourceObject("GlobalResources", "strEditJobNotPossible").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.NextLoad);
                        UrlManager urlManager = new UrlManager();
                        Response.Redirect(urlManager.GetUrlRedirectAbsolute("/Employer/MyJobs.aspx", null));
                    }
                }
                else
                {
                    //only allow add if there are less than 5 unpublished jobs already
                    User currentUser = GetUser();
                    
                    JobManager jobManager = new JobManager();
                    if (jobManager.GetJobPostsByUserUnpublishedCount(currentUser.UserId) >= jobManager.MaxNumberOfUnpublishedJobs())
                    {
                        UrlManager urlManager = new UrlManager();
                        Response.Redirect(urlManager.GetUrlRedirectAbsolute("/Employer/MyJobs.aspx", null));
                    }

                    //preload if republishing an old expired job
                    if (Request.QueryString["republishjobpostid"] != null)
                    {
                        int republishJobPostId = Int32.Parse(Request.QueryString["republishjobpostid"].ToString());
                        if (republishJobPostId > 0)
                        {
                            JobPost republishJobPost = jobManager.GetJobPostExpired(currentUser.UserId, republishJobPostId);
                            if (republishJobPost != null)
                            {
                                PresetJob(republishJobPost);
                            }
                        }
                    }
                }
            }

            string script = "";
            script += "CKEDITOR.replace( '" + txtJobBenefits.ClientID + "', {allowedContent:'" + WebConfigurationManager.AppSettings["ALLOWED_HTML_TAGS"] + "'});\n\t";
            script += "CKEDITOR.replace( '" + txtJobDescription.ClientID + "', {allowedContent:'" + WebConfigurationManager.AppSettings["ALLOWED_HTML_TAGS"] + "'});\n\t";
            script += "CKEDITOR.replace( '" + txtJobRequirements.ClientID + "', {allowedContent:'" + WebConfigurationManager.AppSettings["ALLOWED_HTML_TAGS"] + "'});\n\t";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ckeditor", script, true);
        }

        private void PresetJob(JobPost jobPost)
        {
            if (jobPost != null)
            {
                txtJobTitle.Text = jobPost.Title;
                txtJobDescription.Text = jobPost.Description;
                txtJobRequirements.Text = jobPost.Requirements;
                if (!String.IsNullOrEmpty(jobPost.Requirements))
                {
                    divJobRequirements.Attributes.Add("style", "display:block;");
                    divShowJobRequirements.Visible = false;
                }
                txtJobBenefits.Text = jobPost.Benefits;
                if (!String.IsNullOrEmpty(jobPost.Benefits))
                {
                    divJobBenefits.Attributes.Add("style", "display:block;");
                    divShowJobBenefits.Visible = false;
                }
                txtPosition.Text = jobPost.Position;
                txtJobZip.Text = jobPost.Zip;
                txtJobLocation.Text = jobPost.Location;
                ddlEmploymentType.SelectedValue = jobPost.EmploymentTypeId.ToString();
                foreach (JobPostCategory jobPostCategory in jobPost.JobPostCategories)
                {
                    foreach (ListItem categoryItem in lstCategories.Items)
                    {
                        if (categoryItem.Value == jobPostCategory.CategoryId.ToString())
                        {
                            categoryItem.Selected = true;
                        }
                    }
                }
            }
        }

        protected void btnAddUpdate_Command(object sender, CommandEventArgs e)
        {
            if (ValidateForm())
            {
                int jobPostId = GetEditJobId();

                int employmentType = Int32.Parse(ddlEmploymentType.SelectedValue);
                List<int> categoryAssignments = new List<int>();
                foreach (ListItem categoryItem in lstCategories.Items)
                {
                    if (categoryItem.Selected)
                    {
                        categoryAssignments.Add(Int32.Parse(categoryItem.Value));
                    }
                }

                User currentUser = GetUser();

                JobManager jobManager = new JobManager();
                AddUpdateJobResult saveResult = null;
                if (jobPostId > 0)
                {
                    //update
                    saveResult = jobManager.UpdateJob(currentUser, jobPostId, txtJobTitle.Text, txtJobDescription.Text,
                        txtJobRequirements.Text, txtJobBenefits.Text, txtPosition.Text, txtJobZip.Text,
                        txtJobLocation.Text, employmentType, categoryAssignments);
                }
                else
                {
                    //insert
                    saveResult = jobManager.AddJob(currentUser, txtJobTitle.Text, txtJobDescription.Text,
                        txtJobRequirements.Text, txtJobBenefits.Text, txtPosition.Text, txtJobZip.Text,
                        txtJobLocation.Text, employmentType, categoryAssignments);
                }

                if (saveResult.Success)
                {
                    jobPostId = saveResult.JobPostId;
                    UrlManager urlManager = new UrlManager();
                    if (e.CommandArgument.ToString() == AddUpdateAction.GoToPublish.ToString())
                    {
                        Response.Redirect(urlManager.GetUrlRedirectAbsolute("/Employer/PublishJob.aspx", new Dictionary<string, string>() { { "jobpostid", jobPostId.ToString() } }));
                    }
                    else if (e.CommandArgument.ToString() == AddUpdateAction.GoToMyJobs.ToString())
                    {
                        Response.Redirect(urlManager.GetUrlRedirectAbsolute("/Employer/MyJobs.aspx", null));
                    }
                }
                else
                {
                    if (saveResult.JobTooShort)
                    {
                        this.AddSystemMessage(GetGlobalResourceObject("GlobalResources", "strJobTooShortError").ToString(),
                                GeneralMasterPageBase.SystemMessageTypes.Error,
                                GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                    }
                    else
                    {
                        this.AddSystemMessage(GetGlobalResourceObject("GlobalResources", "strEditJobError").ToString(),
                                GeneralMasterPageBase.SystemMessageTypes.Error,
                                GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                    }
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
            txtPosition.Text = stringValidation.SanitizeUserInputString(txtPosition.Text.Trim(), StringValidation.SanitizeEntityNames.PositionTitle).Trim();
            txtJobLocation.Text = stringValidation.SanitizeUserInputString(txtJobLocation.Text.Trim(), StringValidation.SanitizeEntityNames.Location).Trim();
            txtJobTitle.Text = stringValidation.SanitizeUserInputString(txtJobTitle.Text.Trim(), StringValidation.SanitizeEntityNames.PositionTitle).Trim();
            
            Page.Validate("AddUpdateJob");
                
            return Page.IsValid && heFormValidator.ValidateForm();
        }
    }
}