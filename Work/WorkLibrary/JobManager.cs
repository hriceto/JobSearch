using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkLibrary.BusinessObjects;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class JobManager
    {
        public AddUpdateJobResult AddJob(User currentUser, string jobTitle, string jobDescription, string jobRequirements, string jobBenefits, string position, string jobZip, string location, int employmentTypeId, List<int> categoryAssignments)
        {
            return AddUpateJob(currentUser, -1, jobTitle, jobDescription, jobRequirements, jobBenefits, position, jobZip, location, employmentTypeId, categoryAssignments);
        }

        public AddUpdateJobResult UpdateJob(User currentUser, int jobPostId, string jobTitle, string jobDescription, string jobRequirements, string jobBenefits, string position, string jobZip, string location, int employmentTypeId, List<int> categoryAssignments)
        {
            return AddUpateJob(currentUser, jobPostId, jobTitle, jobDescription, jobRequirements, jobBenefits, position, jobZip, location, employmentTypeId, categoryAssignments);
        }

        private AddUpdateJobResult AddUpateJob(User currentUser, int jobPostId, string jobTitle, string jobDescription, string jobRequirements, string jobBenefits, string position, string jobZip, string location, int employmentTypeId, List<int> categoryAssignments)
        {
            AddUpdateJobResult result = new AddUpdateJobResult();
            result.Success = false;
            //Do lots of validation here. This data could be anything as request validation is false to allow some html input.
            HtmlParser htmlParserAllowSomeHtml = new HtmlParser(HtmlParser.ParseRules.AllowSomeHtml);
            HtmlParser htmlParserNoHtml = new HtmlParser(HtmlParser.ParseRules.FilterOutHtml);
            
            CompanyManager companyManager = new CompanyManager();
            Company company = companyManager.GetCompany(currentUser.UserId);

            JobDataAccess jda = new JobDataAccess();
            
            jobZip = jobZip.Trim();
            int iJobZip = 0;
            if (jobZip.Length > 5)
            {
                Int32.TryParse(jobZip.Remove(5), out iJobZip);
            }
            else
            {
                Int32.TryParse(jobZip, out iJobZip);
            }

            string originalJobText = String.Format("DESCRIPTION:|{0}|REQUIREMENTS:|{1}|BENEFITS:|{2}|",
                jobDescription.Trim(), jobRequirements.Trim(), jobBenefits.Trim());

            //filter out html
            jobTitle = htmlParserNoHtml.FilterHtml(jobTitle.Trim());
            jobDescription = htmlParserAllowSomeHtml.FilterHtml(jobDescription.Trim());
            position = htmlParserNoHtml.FilterHtml(position.Trim());
            location = htmlParserNoHtml.FilterHtml(location.Trim());
            jobRequirements = htmlParserAllowSomeHtml.FilterHtml(jobRequirements.Trim());
            jobBenefits = htmlParserAllowSomeHtml.FilterHtml(jobBenefits.Trim());

            int jobAdvertisementLength = jobDescription.Length + jobRequirements.Length + jobBenefits.Length;
            if (jobAdvertisementLength < Int32.Parse(WebConfigurationManager.AppSettings["MINIMUM_JOB_LENGTH"].ToString()))
            {
                result.JobTooShort = true;
                return result;
            }

            //todo: filter out sql injection attempts.

            //todo: filter out spam attempts.

            JobPost jobPost = null;
            bool isUpdate = (jobPostId > 0);
            if (isUpdate)
            {
                jobPost = jda.GetJobPostForEdit(currentUser.UserId, jobPostId);
                if (jobPost == null)
                {
                    return result;
                }
                jobPost.Title = jobTitle;
                jobPost.Description = jobDescription;
                jobPost.Position = position;
                jobPost.Location = location;
                jobPost.Zip = jobZip;
                jobPost.ZipInt = iJobZip;
                jobPost.EmploymentTypeId = employmentTypeId;

            }
            else
            {
                jobPost = JobPost.CreateJobPost(-1,
                   currentUser.UserId,
                   company.CompanyId,
                   jobTitle,
                   jobDescription,
                   position,
                   location,
                   jobZip,
                   iJobZip,
                   employmentTypeId,
                   DateTime.Now,
                   false,
                   false,
                   false,
                   false,
                   false,
                   false,
                   false,
                   false
                   );
            }

            jobPost.Requirements = jobRequirements;
            jobPost.Benefits = jobBenefits;
            

            //have to review tahis posting before it can be posted. invalid html was present and parsed out.
            jobPost.ReviewRequired = false;
            if (htmlParserAllowSomeHtml.InvalidHtmlWasPresent || htmlParserNoHtml.InvalidHtmlWasPresent)
            {
                jobPost.ReviewRequired = true;
                LogManager logManager = new LogManager();
                logManager.AddLog("HTML stripped from job", currentUser.UserId, "jobpostid=" + jobPost.JobPostId.ToString(), originalJobText);
            }

            //deal with category assignments
            List<JobPostCategory> categoriesToAdd = new List<JobPostCategory>();
            List<JobPostCategory> categoriesToRemove = new List<JobPostCategory>();
            if (isUpdate)
            {
                //figure out which category assignments to remove
                foreach (JobPostCategory jobPostCategory in jobPost.JobPostCategories)
                {
                    if (!categoryAssignments.Contains(jobPostCategory.CategoryId))
                    {
                        categoriesToRemove.Add(jobPostCategory);
                    }
                }

                //new category assignments to add
                foreach (int categoryAssignment in categoryAssignments)
                {
                    bool assignmentAlreadyExists = false;
                    foreach (JobPostCategory jobPostCategory in jobPost.JobPostCategories)
                    {
                        if (jobPostCategory.CategoryId == categoryAssignment)
                        {
                            assignmentAlreadyExists = true;
                            break;
                        }
                    }
                    if (!assignmentAlreadyExists)
                    {
                        categoriesToAdd.Add(JobPostCategory.CreateJobPostCategory(-1, -1, categoryAssignment));
                    }
                }
            }
            else
            {
                //add new category assignments
                foreach (int categoryAssignment in categoryAssignments)
                {
                    jobPost.JobPostCategories.Add(JobPostCategory.CreateJobPostCategory(-1, -1, categoryAssignment));
                }
            }

            //se info
            try
            {
                UrlManager urlManager = new UrlManager();
                jobPost.SeUrl = urlManager.GetSeUrl(jobPost);
                jobPost.SeDescription = urlManager.GetSeDescription(
                    String.Format("{0} {1} {2}", jobDescription.Trim(), jobRequirements.Trim(), jobBenefits.Trim())
                );
            }
            catch(System.Exception ex)
            {
                ExceptionManager exceptionManager = new ExceptionManager();
                exceptionManager.AddException(ex);
            }

            //perform update or delete
            if (isUpdate)
            {
                bool updateOK = jda.UpdateJob(jobPost, categoriesToRemove, categoriesToAdd);
                if (updateOK)
                {
                    result.JobPostId = jobPost.JobPostId;
                    result.Success = (jobPost.JobPostId > 0);
                }
            }
            else
            {
                int newJobPostId = jda.AddJob(jobPost);
                result.JobPostId = newJobPostId;
                result.Success = (newJobPostId > 0);
            }
            return result;
        }

        public bool UpdateJobPostPublishInfo(int currentUserId, int jobPostId, bool isFree, bool isPaid, bool isPaidAnonymous, 
            string keywords, string replyEmail, string replyUrl, DateTime startDate)
        {
            bool result = false;
            
            JobDataAccess jda = new JobDataAccess();
            JobPost jobPost = null;
            bool isUpdate = (jobPostId > 0);
            if (isUpdate)
            {
                jobPost = jda.GetJobPostForEdit(currentUserId, jobPostId);
                if (jobPost == null)
                {
                    return false;
                }

                jobPost.Keywords = "";
                jobPost.ReplyEmail = "";
                jobPost.ReplyUrl = "";
                jobPost.StartDate = DateTime.Now;
                jobPost.IsFreeAd = isFree;
                jobPost.IsPaidAd = false;
                jobPost.IsAnonymousAd = false;
                jobPost.PricePaidAd = 0;
                jobPost.PriceAnonymousAd = 0;
                jobPost.PriceTotal = 0;

                CompanyManager companyManager = new CompanyManager();
                Company currentUserCompany = companyManager.GetCompany(currentUserId);
                if (currentUserCompany == null)
                {
                    return false;
                }
                Checkout checkout = new Checkout();
                Prices prices = checkout.GetPrices(currentUserCompany);

                if (isPaid || isPaidAnonymous)
                {
                    jobPost.Keywords = keywords;
                    jobPost.ReplyEmail = replyEmail;
                    jobPost.ReplyUrl = replyUrl;
                    jobPost.StartDate = startDate;
                    jobPost.IsPaidAd = true;

                    jobPost.PricePaidAd = prices.BasicAdPrice;
                    jobPost.PriceTotal = prices.BasicAdPrice;
                }
                if (isPaidAnonymous)
                {
                    jobPost.IsAnonymousAd = true;
                    jobPost.PriceAnonymousAd = prices.AnonymousAdPrice - prices.BasicAdPrice;
                    jobPost.PriceTotal = prices.AnonymousAdPrice;
                }

                if (jobPost.StartDate < DateTime.Now)
                {
                    jobPost.StartDate = DateTime.Now;
                }
                jobPost.AddedToCart = true;
                jobPost.AddedToCartDate = DateTime.Now;
                jobPost.Duration = Checkout.GetAdDuration(jobPost);

                result = jda.UpdateJob(jobPost);
                if (result)
                {
                    SaveShoppingCartCountInCookie(currentUserId);
                    return true;
                }
            }
            return result;
        }

        public int SaveShoppingCartCountInCookie(int userId)
        {
            int count = GetJobPostsByUserInShoppingCartCount(userId);
            CookieManager cookieManager = new CookieManager();
            cookieManager.SetShoppingCartCountCookie(count);
            return count;
        }

        /// <summary>
        /// Remove job from cart
        /// </summary>
        /// <param name="jobPostId"></param>
        /// <returns></returns>
        public bool UpdateJobPostRemoveFromCart(int currentUserId, int jobPostId)
        {
            bool result = false;
            
            JobDataAccess jda = new JobDataAccess();
            JobPost removeJobPostForCart = jda.GetJobPostForEdit(currentUserId, jobPostId);
            if (removeJobPostForCart != null)
            {
                removeJobPostForCart.AddedToCart = false;
                result = jda.UpdateJob(removeJobPostForCart);
                if (result)
                {
                    SaveShoppingCartCountInCookie(currentUserId);
                }
            }
            return result;
        }

        /// <summary>
        /// Update the reviewed and suspended flags for a job.
        /// </summary>
        /// <param name="jobPostId"></param>
        /// <param name="adminUserId"></param>
        /// <param name="suspend"></param>
        /// <returns></returns>
        public bool UpdateJobPostReviewed(int jobPostId, int adminUserId, bool suspend, string suspendReason)
        {
            bool result = false;
            bool reviewRequired = false;
            bool dateUpdated = false;
            JobPost jobPost = GetJobPostForAdmin(jobPostId);

            if (jobPost != null)
            {
                reviewRequired = jobPost.ReviewRequired;
                //if this job required review then that means it was not showing on the front end
                //update the start and end date
                if (jobPost.ReviewRequired && jobPost.StartDate < DateTime.Now)
                {
                    jobPost.StartDate = DateTime.Now;
                    if (jobPost.Duration != null)
                    {
                        jobPost.EndDate = DateTime.Now.AddDays((int)jobPost.Duration);
                    }
                    dateUpdated = true;
                }
                

                //update reviewed
                jobPost.Reviewed = true;
                jobPost.ReviewedByUserId = adminUserId;
                jobPost.ReviewedDate = DateTime.Now;
                jobPost.ReviewRequired = false;

                //updated suspeanded
                if (suspend)
                {
                    jobPost.Suspended = true;
                    jobPost.SuspendedByUserId = adminUserId;
                    jobPost.SuspendedOn = DateTime.Now;
                }

                JobDataAccess jda = new JobDataAccess();
                result = jda.UpdateJob(jobPost);
                //if suspended send out an email to the user
                if (result && suspend)
                {
                    Email email = new Email();
                    UserManager userManager = new UserManager();
                    User employerUser = userManager.GetUser(jobPost.UserId);
                    email.SendEmail(employerUser.Email, Email.EmailTemplates.AdminJobSuspended, new Dictionary<string, string>() { { "ReasonText", suspendReason } }, employerUser, jobPost);
                }
                else if (result && reviewRequired)
                {
                    Email email = new Email();
                    UserManager userManager = new UserManager();
                    User employerUser = userManager.GetUser(jobPost.UserId);
                    email.SendEmail(employerUser.Email, Email.EmailTemplates.AdminJobReviewed, new Dictionary<string, string>() { { "DateUpdated", dateUpdated.ToString().ToLower() } }, employerUser, jobPost);
                }
            }
            return result;
        }

        public bool UpdateJobPostSuspend(int jobPostId, User user, bool suspend)
        {
            bool result = false;
            JobPost jobPost = GetJobPostForEdit(user.UserId, jobPostId);

            if (jobPost != null)
            {
                //updated suspeanded
                if (suspend)
                {
                    jobPost.Suspended = true;
                    jobPost.SuspendedByUserId = user.UserId;
                    jobPost.SuspendedOn = DateTime.Now;
                }

                JobDataAccess jda = new JobDataAccess();
                result = jda.UpdateJob(jobPost);
                //if suspended send out an email to the user
                if (result && suspend)
                {
                    UrlManager urlManager = new UrlManager();
                    string jobsPage = urlManager.GetUrlRedirectAbsolute("/Employer/MyJobs.aspx", null);

                    //suspend by user email
                    Email email = new Email();
                    email.SendEmail(user.Email, Email.EmailTemplates.JobSuspended, new Dictionary<string, string>() { {"JobsPage", jobsPage} }, user, jobPost);
                }
            }
            return result;
        }

        /// <summary>
        /// get job post for edit purposes.
        /// </summary>
        /// <param name="jobPostId"></param>
        /// <returns></returns>
        public JobPost GetJobPostForEdit(int currentUserId, int jobPostId)
        {
            JobDataAccess jda = new JobDataAccess();
            JobPost jobPost = jda.GetJobPostForEdit(currentUserId, jobPostId);
            return jobPost;
        }

        /// <summary>
        /// get expired job post for republish screen
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="jobPostId"></param>
        /// <returns></returns>
        public JobPost GetJobPostExpired(int currentUserId, int jobPostId)
        {
            JobDataAccess jda = new JobDataAccess();
            JobPost jobPost = jda.GetJobPostExpired(currentUserId, jobPostId);
            return jobPost;
        }

        /// <summary>
        /// Get a single job post for detail page
        /// </summary>
        /// <param name="jobPostId"></param>
        /// <returns></returns>
        public JobPost GetJobPostForDetail(int jobPostId, bool isPreview, bool updateNumberOfViews)
        {
            JobDataAccess jda = new JobDataAccess();
            if (isPreview)
            {
                //get job post bu id and user id
                UserManager userManager = new UserManager();
                User currentUser = userManager.GetUser();
                if (currentUser != null)
                {
                    return jda.GetJobPostForPreview(jobPostId, currentUser.UserId);
                }
            }
            else
            {
                //get published job post
                return jda.GetJobPostForDetail(jobPostId, updateNumberOfViews);
            }
            return null;
        }

        /// <summary>
        /// get job post by user and jobpostid. 
        /// </summary>
        /// <param name="jobPostId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JobPost GetJobPostForEmployerAdmin(int jobPostId, int userId)
        {
            JobDataAccess jda = new JobDataAccess();
            JobPost jobPost = jda.GetJobPostForEmployerAdmin(jobPostId, userId);
            return jobPost;
        }

        /// <summary>
        /// get job post by user and jobpostid. 
        /// </summary>
        /// <param name="jobPostId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public JobPost GetJobPostForAdmin(int jobPostId)
        {
            JobDataAccess jda = new JobDataAccess();
            JobPost jobPost = jda.GetJobPostForAdmin(jobPostId);
            return jobPost;
        }

        /// <summary>
        /// get published jobs for a given user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsByUserPublished(int userId, int page, int pageSize, out int totalNumberOfResults)
        {
            JobDataAccess jda = new JobDataAccess();
            return jda.GetJobPostsByUserPublished(userId, page, pageSize, out totalNumberOfResults);
        }

        /// <summary>
        /// get published jobs for a given user witha future starting date
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsByUserPublishedFuture(int userId, int page, int pageSize, out int totalNumberOfResults)
        {
            JobDataAccess jda = new JobDataAccess();
            return jda.GetJobPostsByUserPublishedFuture(userId, page, pageSize, out totalNumberOfResults);
        }

        /// <summary>
        /// get published jobs for a given user that are pending a review
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsByUserPublishedPendingReview(int userId)
        {
            JobDataAccess jda = new JobDataAccess();
            return jda.GetJobPostsByUserPublishedPendingReview(userId);
        }

        /// <summary>
        /// get all published jobs that have not been reviewed by an administrator. Only get top 10.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsPublishedForReview()
        {
            JobDataAccess jda = new JobDataAccess();
            return jda.GetJobPostsPublishedForReview();
        }

        /// <summary>
        /// Get jobs that have not been published yet for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsByUserUnpublished(int userId)
        {
            JobDataAccess jda = new JobDataAccess();
            return jda.GetJobPostsByUserUnpublished(userId);
        }

        /// <summary>
        /// Get the number of unpublished jobs for the current user
        /// </summary>
        /// <returns></returns>
        public int GetJobPostsByUserUnpublishedCount(int currentUserId)
        {
            JobDataAccess jda = new JobDataAccess();
            return jda.GetJobPostsByUserUnpublishedCount(currentUserId);
        }

        /// <summary>
        /// get expired jobs for user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsByUserPublishedExpired(int userId, int page, int pageSize, out int totalNumberOfResults)
        {
            JobDataAccess jda = new JobDataAccess();
            return jda.GetJobPostsByUserPublishedExpired(userId, page, pageSize, out totalNumberOfResults);
        }

        /// <summary>
        /// Get all unpublished jobs that are currently in the shopping cart
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsByUserInShoppingCart(int userId)
        {
            JobDataAccess jda = new JobDataAccess();
            return jda.GetJobPostsByUserInShoppingCart(userId);
        }

        /// <summary>
        /// Get all unpublished jobs that are currently in the shopping cart for the company
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<JobPost> GetJobPostsByCompanyInShoppingCart(int companyId)
        {
            JobDataAccess jda = new JobDataAccess();
            return jda.GetJobPostsByCompanyInShoppingCart(companyId);
        }

        /// <summary>
        /// Get count of unpublished jobs that are currently in the shopping cart. Private.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private int GetJobPostsByUserInShoppingCartCount(int userId)
        {
            JobDataAccess jda = new JobDataAccess();
            return jda.GetJobPostsByUserInShoppingCartCount(userId);
        }

        //the issue here is that userid is only available on authenticated pages.
        //so we cannot rely on it for displaying the count on every page.
        //setting the value to a cookie is another option but it would be a good idea if that cookie was encrypted.
        public int GetShoppingCartCountForDisplay()
        {
            int result = -1;

            CookieManager cookieManager = new CookieManager();
            UserManager.UserRoles userRole = cookieManager.GetNavigationCookie();

            if (userRole == UserManager.UserRoles.Employer)
            {
                //try and read the cookie value.
                result = cookieManager.GetShoppingCartCountCookie();
                
                //if there is no cookie check whether the user is logged in. If yes grab the value from
                //database and store in cookie.
                if (result < 0)
                {
                    int userId = -1;
                    UserManager userManager = new UserManager();
                    User currentUser = userManager.GetUser();
                    if (currentUser != null)
                    {
                        userId = currentUser.UserId;
                    }
                
                    if (userId > 0)
                    {
                        result = SaveShoppingCartCountInCookie(currentUser.UserId);
                    }
                }

                //otherwise return 0. User is not logged in and there is no cookie
                if (result < 0)
                {
                    result = 0;
                }
            }
            return result;
        }

        /// <summary>
        /// Get the number of free job posts made by a company that are currently published 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetJobPostsByCompanyPublishedFreeCount(int companyId, int timeIntervalDays)
        {
            JobDataAccess jda = new JobDataAccess();
            return jda.GetJobPostsByCompanyPublishedFreeCount(companyId, timeIntervalDays);
        }

        public DateTime? GetPublishedFreeJobLatestStartDate(int companyId, int timeIntervalDays)
        {
            JobDataAccess jda = new JobDataAccess();
            return jda.GetPublishedFreeJobLatestStartDate(companyId, timeIntervalDays);
        }

        public List<JobSearchIndex> GetJobPostsForSearch()
        {
            JobDataAccess jda = new JobDataAccess();
            return jda.GetJobPostsForSearch();
        }

        public List<JobPost> AdminSearchForJobs(string companyName)
        {
            JobDataAccess jda = new JobDataAccess();
            return jda.AdminSearchForJobs(companyName);
        }

        /// <summary>
        /// read the max number of jobs that can be created before publishing
        /// </summary>
        /// <returns></returns>
        public int MaxNumberOfUnpublishedJobs()
        {
            int result = 5;
            if (WebConfigurationManager.AppSettings["MAX_NUMBER_OF_UNPUBLISHED_JOBS"] != null)
            {
                result = Int32.Parse(WebConfigurationManager.AppSettings["MAX_NUMBER_OF_UNPUBLISHED_JOBS"]);
            }
            return result;
        }

        public bool UserCanEdit(JobPost jobPost)
        {
            return ((!jobPost.Published) || (jobPost.Published && jobPost.IsPaidAd && (jobPost.EndDate >= DateTime.Now)));
        }

        public bool UserCanPublish(JobPost jobPost)
        {
            return !jobPost.Published;
        }

        public string GetJobHeading(string companyName, string jobPosition, bool isRecruiter, bool isAnonymous,
            string employerResource, string recruiterResource, string anonymousResource)
        {
            string result = "";

            jobPosition = jobPosition.Trim();
            if (!String.IsNullOrEmpty(companyName) && !String.IsNullOrEmpty(jobPosition))
            {
                string article = "a";

                char firstChar = jobPosition.ToLower()[0];
                if (firstChar == 'a' || firstChar == 'e' || firstChar == 'i' ||
                    firstChar == 'o' || firstChar == 'u' || firstChar == 'y')
                {
                    article = "an ";
                }

                string text = employerResource;
                if (isRecruiter)
                {
                    text = recruiterResource;
                }
                else if (isAnonymous)
                {
                    text = anonymousResource;
                }

                result = String.Format(text, companyName,
                        article, jobPosition);
            }

            return result;
        }

#region static
        public static string GetAbsoluteUrl(UrlManager urlManager, JobPost jobPost)
        {
            Dictionary<string, string> jobDetailDict = new Dictionary<string, string>() { { "jobpostid", jobPost.JobPostId.ToString() } };
            if (!String.IsNullOrEmpty(jobPost.SeUrl))
            {
                jobDetailDict.Add("seurl", jobPost.SeUrl);
            }

            return urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.JobDetail, jobDetailDict);
        }

        public static string GetAbsoluteUrl(UrlManager urlManager, JobSearchResult jobSearchResult)
        {
            Dictionary<string, string> jobDetailDict = new Dictionary<string, string>() { { "jobpostid", jobSearchResult.JobPostId.ToString() } };
            if (!String.IsNullOrEmpty(jobSearchResult.SeUrl))
            {
                jobDetailDict.Add("seurl", jobSearchResult.SeUrl);
            }

            return urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.JobDetail, jobDetailDict);
        }
#endregion
    }
}
