using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary.UserControls
{
    public class JobEditControlBase : GeneralControlBase
    {
        protected int GetEditJobId()
        {
            if (HttpContext.Current.Request.QueryString["jobpostid"] != null)
            {
                int jobPostId = -1;
                if (Int32.TryParse(HttpContext.Current.Request.QueryString["jobpostid"].ToString(), out jobPostId))
                {
                    return jobPostId;
                }
            }
            return -1;
        }

        protected JobPost GetEditJob()
        {
            int jobPostId = GetEditJobId();
            if (jobPostId > 0)
            {
                User currentUser = GetUser();

                JobManager jobManager = new JobManager();
                return jobManager.GetJobPostForEdit(currentUser.UserId, jobPostId);
            }
            return null;
        }
    }
}
