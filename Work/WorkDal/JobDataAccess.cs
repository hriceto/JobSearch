using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class JobDataAccess : DataAccess 
    {
        public int AddJob(JobPost newJobPost)
        {
            int result = -1;

            using (WorkEntities context = GetContext())
            {
                context.JobPosts.AddObject(newJobPost);
                if (context.SaveChanges() >= 1)
                {
                    result = newJobPost.JobPostId;
                }
            }

            return result;
        }

        public bool UpdateJob(JobPost updateJobPost)
        {
            return UpdateJob(updateJobPost, null, null);
        }

        /// <summary>
        /// Update a job.
        /// </summary>
        /// <param name="newJobPost"></param>
        /// <returns></returns>
        public bool UpdateJob(JobPost updateJobPost, List<JobPostCategory> categoriesToRemove, List<JobPostCategory> categoriesToAdd)
        {
            bool result = false;

            using (WorkEntities context = GetContext())
            {
                if (updateJobPost != null)
                {
                    context.JobPosts.Attach(updateJobPost);
                    context.ObjectStateManager.ChangeObjectState(updateJobPost, System.Data.EntityState.Modified);

                    if (categoriesToRemove != null)
                    {
                        foreach (JobPostCategory categoryToRemove in categoriesToRemove)
                        {
                            context.DeleteObject(categoryToRemove);
                        }
                    }

                    if (categoriesToAdd != null)
                    {
                        foreach (JobPostCategory categoryToAdd in categoriesToAdd)
                        {
                            updateJobPost.JobPostCategories.Add(categoryToAdd);
                        }
                    }

                    result = (context.SaveChanges() >= 1);
                }
            }
            return result;
        }

        public bool UpdateJobs(List<JobPost> updateJobPosts)
        {
            bool result = false;
            using (WorkEntities context = GetContext())
            {
                if (updateJobPosts != null)
                {
                    foreach (JobPost updateJobPost in updateJobPosts)
                    {
                        context.JobPosts.Attach(updateJobPost);
                        context.ObjectStateManager.ChangeObjectState(updateJobPost, System.Data.EntityState.Modified);
                    }
                    result = (context.SaveChanges() >= updateJobPosts.Count);
                }
            }
            return result;
        }

        /// <summary>
        /// Get a single job posting with all its details. Including Company & Employment. 
        /// </summary>
        /// <param name="jobPostId">The id of the add.</param>
        /// <returns>Job Post with a given Id</returns>
        public JobPost GetJobPostForDetail(int jobPostId, bool updateNumberOfViews)
        {
            JobPost result = null;

            using (WorkEntities context = GetContext())
            {

                result = (from a in context.JobPosts.Include("Company").Include("EmploymentType")
                          where a.JobPostId == jobPostId &&
                          a.StartDate != null &&
                          a.StartDate <= DateTime.Now &&
                          a.EndDate >= DateTime.Now &&
                          a.Published == true &&
                          a.Suspended == false &&
                          ((a.ReviewRequired && a.Reviewed) || (!a.ReviewRequired))
                          select a).FirstOrDefault();

                if (result != null && updateNumberOfViews)
                {
                    result.NumberOfViews = result.NumberOfViews.GetValueOrDefault() + 1;
                    context.SaveChanges();
                }
            }

            return result;
        }

        /// <summary>
        /// Only get a job post if it matches the userid
        /// </summary>
        /// <param name="jobPostId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JobPost GetJobPostForPreview(int jobPostId, int userId)
        {
            JobPost result = null;

            using (WorkEntities context = GetContext())
            {
                result = (from a in context.JobPosts.Include("Company").Include("EmploymentType")
                          where a.JobPostId == jobPostId &&
                          a.UserId == userId
                          select a).FirstOrDefault();
            }
            return result;
        }

        /// <summary>
        /// Get job posts to push into search index
        /// </summary>
        /// <returns></returns>
        public List<JobSearchIndex> GetJobPostsForSearch()
        {
            using (WorkEntities context = GetContext())
            {
                var resultsProjection = (from jp in context.JobPosts.Include("Company").Include("EmploymentType")
                              join zc in context.ZipCodes on jp.ZipInt equals zc.Zip into jpzc
                              where jp.StartDate != null &&
                                jp.StartDate <= DateTime.Now &&
                                jp.EndDate >= DateTime.Now &&
                                jp.Published == true &&
                                jp.Suspended == false &&
                                ((jp.ReviewRequired && jp.Reviewed) || (!jp.ReviewRequired))
                              from zc in jpzc.DefaultIfEmpty()
                              select new {
                                  Job = jp,
                                  Zip = zc,
                                  JobCompany = jp.Company,
                                  JobEmploymentType = jp.EmploymentType,
                                  JobPostCategories = jp.JobPostCategories
                              }).AsEnumerable();

                var results = (from rp in resultsProjection
                              select new JobSearchIndex
                              {
                                  Job = rp.Job,
                                  Zip = rp.Zip,
                                  JobCompany = rp.JobCompany,
                                  JobEmploymentType = rp.JobEmploymentType,
                                  JobPostCategories = rp.JobPostCategories.ToList()
                              });
                return results.ToList();
            }
        }

        /// <summary>   
        /// Get jobs that have been published are approved (if required), not suspended, and are still active as far as date range
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsByUserPublished(int userId, int page, int pageSize, out int totalNumberOfResults)
        {
            using (WorkEntities context = GetContext())
            {
                var results = from a in context.JobPosts
                              where a.Published == true &&
                              a.StartDate != null &&
                              a.StartDate <= DateTime.Now &&
                              a.EndDate >= DateTime.Now &&
                              a.Suspended == false &&
                              a.UserId == userId &&
                              ((a.ReviewRequired && a.Reviewed) || (!a.ReviewRequired))
                              orderby a.StartDate descending 
                              select a;
                totalNumberOfResults = results.Count();
                
                return results.Skip((page-1) * pageSize).Take(pageSize).ToList();
            }
        }

        /// <summary>   
        /// Get jobs that have been published but start in the future
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsByUserPublishedFuture(int userId, int page, int pageSize, out int totalNumberOfResults)
        {
            using (WorkEntities context = GetContext())
            {
                var results = from a in context.JobPosts
                              where a.Published == true &&
                              a.StartDate != null &&
                              a.StartDate > DateTime.Now &&
                              a.EndDate >= DateTime.Now &&
                              a.Suspended == false &&
                              a.UserId == userId &&
                              ((a.ReviewRequired && a.Reviewed) || (!a.ReviewRequired))
                              orderby a.StartDate descending
                              select a;
                totalNumberOfResults = results.Count();

                return results.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        /// <summary>
        /// Get job posts by user that are published but waiting to be reviewed.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsByUserPublishedPendingReview(int userId)
        {
            using (WorkEntities context = GetContext())
            {
                var results = from a in context.JobPosts
                              where a.Published == true &&
                              a.StartDate != null &&
                              a.StartDate <= DateTime.Now &&
                              a.EndDate >= DateTime.Now &&
                              a.Suspended == false &&
                              a.UserId == userId &&
                              a.ReviewRequired == true && 
                              a.Reviewed == false
                              select a;
                return results.ToList();
            }
        }

        /// <summary>
        /// Get all job posts that are published and have not been reviewed by an administrator. Only get top 10.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsPublishedForReview()
        {
            using (WorkEntities context = GetContext())
            {
                var results = from a in context.JobPosts
                              where a.Published == true &&
                              a.StartDate != null &&
                              a.StartDate <= DateTime.Now &&
                              a.EndDate >= DateTime.Now &&
                              a.Suspended == false &&
                              a.Reviewed == false
                              orderby a.ReviewRequired descending, a.StartDate ascending
                              select a;
                return results.Take(10).ToList();
            }
        }

        /// <summary>
        /// Get jobs that have not been published yet.
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsByUserUnpublished(int userId)
        {
            using (WorkEntities context = GetContext())
            {
                var results = from a in context.JobPosts
                              where a.Published == false &&
                              a.UserId == userId
                              select a;
                return results.ToList();
            }
        }

        /// <summary>
        /// Get the count of unpublished jobs currently in the system
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetJobPostsByUserUnpublishedCount(int userId)
        {
            using (WorkEntities context = GetContext())
            {
                var results = from a in context.JobPosts
                              where a.Published == false &&
                              a.UserId == userId
                              select a;
                return results.Count();
            }
        }

        /// <summary>
        /// Get unpublished jobs made by user that are in the cart
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsByUserInShoppingCart(int userId)
        {
            using (WorkEntities context = GetContext())
            {
                var results = from a in context.JobPosts
                              where a.Published == false &&
                              a.UserId == userId &&
                              a.AddedToCart == true
                              select a;
                return results.ToList();
            }
        }

        /// <summary>
        /// Get unpublished jobs made by a company that are in the cart
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsByCompanyInShoppingCart(int companyId)
        {
            using (WorkEntities context = GetContext())
            {
                var results = from a in context.JobPosts
                              where a.Published == false &&
                              a.CompanyId == companyId &&
                              a.AddedToCart == true
                              select a;
                return results.ToList();
            }
        }

        /// <summary>
        /// Get count of unpublished jobs made by user that are in the cart
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetJobPostsByUserInShoppingCartCount(int userId)
        {
            using (WorkEntities context = GetContext())
            {
                var results = from a in context.JobPosts
                              where a.Published == false &&
                              a.UserId == userId &&
                              a.AddedToCart == true
                              select a;
                return results.Count();
            }
        }

        /// <summary>
        /// Get jobs that by user that have expired or have been suspended
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsByUserPublishedExpired(int userId, int page, int pageSize, out int totalNumberOfResults)
        {
            using (WorkEntities context = GetContext())
            {
                var results = from a in context.JobPosts
                              where a.Published == true &&
                              a.UserId == userId &&
                              (a.EndDate < DateTime.Now || a.Suspended == true)
                              orderby a.StartDate descending
                              select a;
                totalNumberOfResults = results.Count();
                return results.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        /// <summary>
        /// Allowed to edit published ads that are not free or
        /// unpublished adds.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jobPostId"></param>
        /// <returns></returns>
        public JobPost GetJobPostForEdit(int userId, int jobPostId)
        {
            using (WorkEntities context = GetContext())
            {
                var result = from a in context.JobPosts.Include("JobPostCategories")
                             where a.UserId == userId &&
                             a.JobPostId == jobPostId &&
                             ((a.Published == true && a.IsFreeAd == false && (a.EndDate >= DateTime.Now)) || (a.Published == false))
                             select a;
                return result.FirstOrDefault();
            }
        }

        /// <summary>
        /// get expired job post. mainly for re-publish screen
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jobPostId"></param>
        /// <returns></returns>
        public JobPost GetJobPostExpired(int userId, int jobPostId)
        {
            using (WorkEntities context = GetContext())
            {
                var result = from a in context.JobPosts.Include("JobPostCategories")
                             where a.UserId == userId &&
                             a.JobPostId == jobPostId &&
                             (a.EndDate < DateTime.Now || a.Suspended == true)
                             select a;
                return result.FirstOrDefault();
            }
        }

        /// <summary>
        /// Get job post by id and user id
        /// </summary>
        /// <param name="jobPostId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JobPost GetJobPostForEmployerAdmin(int jobPostId, int userId)
        {
            using (WorkEntities context = GetContext())
            {
                var result = from jp in context.JobPosts
                             where jp.UserId == userId &&
                             jp.JobPostId == jobPostId 
                             select jp;
                return result.FirstOrDefault();
            }   
        }

        /// <summary>
        /// Get job post by id and user id
        /// </summary>
        /// <param name="jobPostId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JobPost GetJobPostForAdmin(int jobPostId)
        {
            using (WorkEntities context = GetContext())
            {
                var result = from jp in context.JobPosts
                             where jp.JobPostId == jobPostId
                             select jp;
                return result.FirstOrDefault();
            }
        }

        /// <summary>
        /// get a number of free ads already published by company.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetJobPostsByCompanyPublishedFreeCount(int companyId, int timeIntervalDays)
        {
            DateTime startDate = DateTime.Now.AddDays(-timeIntervalDays);

            using (WorkEntities context = GetContext())
            {
                var result = from jp in context.JobPosts
                             where jp.CompanyId == companyId &&
                             jp.Published == true &&
                             jp.IsFreeAd == true &&
                             jp.StartDate >= startDate
                             select jp;
                return result.Count();
            }
        }

        /// <summary>
        /// get the last free ad published by company.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DateTime? GetPublishedFreeJobLatestStartDate(int companyId, int timeIntervalDays)
        {
            DateTime startDate = DateTime.Now.AddDays(-timeIntervalDays);

            using (WorkEntities context = GetContext())
            {
                var result = from jp in context.JobPosts
                             where jp.CompanyId == companyId &&
                             jp.Published == true &&
                             jp.IsFreeAd == true &&
                             jp.StartDate >= startDate
                             orderby jp.StartDate descending
                             select jp.StartDate;
                return result.Take(1).FirstOrDefault();
            }
        }

        public User GetJobPostEmployerUser(int jobPostId)
        {
            using (WorkEntities context = GetContext())
            {
                var result = from a in context.JobPosts
                             where a.JobPostId == jobPostId
                             select a.User;
                return result.FirstOrDefault();
            }
        }

        public List<JobPost> AdminSearchForJobs(string companyName)
        {
            List<JobPost> result = null;
            using (WorkEntities context = GetContext())
            {
                result = (from c in context.JobPosts
                          where c.Company.Name.Contains(companyName)
                          orderby c.CreatedDate descending
                          select c).Take(10).ToList();
            }
            return result;
        }
    }
}
