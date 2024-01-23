using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class JobApplicationDataAccess : DataAccess 
    {
        public int AddJobApplication(JobApplication newJobApplicaiton)
        {
            int result = -1;

            using (WorkEntities context = GetContext())
            {
                context.JobApplications.AddObject(newJobApplicaiton);
                if (context.SaveChanges() >= 1)
                {
                    result = newJobApplicaiton.JobPostId;
                }

                return result;
            }
        }

        public List<JobApplication> GetJobApplications(int jobPostId, int currentUserId, int page, int pageSize, out int totalNumberOfResults)
        {
            using (WorkEntities context = GetContext())
            {
                var applicaitons = from ja in context.JobApplications
                        where ja.JobPostId == jobPostId && 
                        ja.JobPost.UserId == currentUserId                        
                        orderby ja.DateCreated descending
                        select ja;
                totalNumberOfResults = applicaitons.Count();

                return applicaitons.Skip((page-1) * pageSize).Take(pageSize).ToList();
            }
        }

        public JobApplication GetJobApplication(int jobApplicationId, int userId)
        {
            using (WorkEntities context = GetContext())
            {
                var application = from ja in context.JobApplications.Include("JobPost")
                                   where ja.JobApplicationId == jobApplicationId &&
                                   ja.JobPost.UserId == userId
                                   select ja;

                return application.FirstOrDefault();
            }
        }
    }
}
