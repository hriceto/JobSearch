using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;
    
namespace HristoEvtimov.Websites.Work.Web.Admin.Controls
{
    public partial class jobadmin : GeneralControlBase, IFormValidation
    {
        UrlManager urlManager = new UrlManager();
            
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindJobs();
                if (CompanyId > 0)
                {
                    SearchForJobs(CompanyId);
                }
            }
        }

        public int CompanyId
        {
            get
            {
                if (Request.QueryString["companyid"] != null)
                {
                    return Int32.Parse(Request.QueryString["companyid"]);
                }
                return -1;
            }
        }

        private void BindJobs()
        {
            JobManager jobManager = new JobManager();

            rptrJobsForReview.DataSource = jobManager.GetJobPostsPublishedForReview();
            rptrJobsForReview.DataBind();
        }

        protected void rptrJobs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                JobPost jobPost = (JobPost)e.Item.DataItem;

                HyperLink hypReview = e.Item.FindControl("hypReview") as HyperLink;
                if (hypReview != null)
                {
                    hypReview.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/Admin/JobEdit.aspx", new Dictionary<string, string>() { { "jobpostid", jobPost.JobPostId.ToString() } }); 
                }
            }
        }

        protected void btnJobSearch_Click(object sender, EventArgs e)
        {
            SearchForJobs(-1);
        }

        private void SearchForJobs(int companyId)
        {
            JobManager jobManager = new JobManager();

            string companyName = txtCompanyName.Text;
            if(companyId > 0)
            {
                CompanyManager companyManager = new CompanyManager();
                Company company = companyManager.GetCompanyForReview(companyId);
                if (company != null)
                {
                    companyName = company.Name;
                }
            }
            rptrJobSearch.DataSource = jobManager.AdminSearchForJobs(companyName);
            rptrJobSearch.DataBind();
        }

        public bool ValidateForm()
        {
            return true;
        }
    }
}