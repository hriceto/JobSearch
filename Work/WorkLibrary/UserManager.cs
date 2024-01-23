using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class UserManager
    {
        public enum UserRoles { None, Admin, Employer, JobSeeker, SuperAdmin, Unknown, Contributor };

        public bool UpdatePassword(string currentPassword, string newPassword)
        {
            bool result = false;
            MembershipUser membershipUser = Membership.GetUser();
            if (membershipUser != null)
            {
                result = membershipUser.ChangePassword(currentPassword, newPassword);
                //if account is locked out. then log out user
                if (!result)
                {
                    membershipUser = Membership.GetUser();
                    if (membershipUser.IsLockedOut)
                    {
                        UserManager userManager = new UserManager();
                        userManager.SignOut();

                        CookieManager cookieManager = new CookieManager();
                        cookieManager.ExpireLoggedInCookie();
                    }
                }
                else
                {
                    //send success email
                    User currentUser = GetUser();
                    Email email = new Email();
                    email.SendEmail(membershipUser.Email, Email.EmailTemplates.UpdatePassword,
                        null, 
                        currentUser);
                }
            }
            return result;
        }

        public MembershipCreateStatus CreateMembershipUser(string email, string password, string firstName, string initialRole)
        {
            //create asp.net membership user
            MembershipCreateStatus membershipCreateStatus;
            MembershipUser membershipUser = Membership.CreateUser(email, password, email, "First Name", firstName, false, out membershipCreateStatus);
            if (membershipCreateStatus == MembershipCreateStatus.Success && !String.IsNullOrEmpty(initialRole))
            {
                //add asp.net membership role
                Roles.AddUserToRole(email, initialRole);
            }
            return membershipCreateStatus;
        }

        public WorkDal.User CreateUser(string email, string firstName, string lastName, bool okToEmail)
        {
            //add an additional user record to attach more information
            WorkDal.User user = WorkDal.User.CreateUser(-1, email, firstName, lastName, okToEmail, false, Guid.NewGuid(), DateTime.Now, DateTime.Now, false, Guid.NewGuid());
            UserDataAccess uda = new UserDataAccess();
            if (!uda.AddUser(user))
            {
                user = null;
            }
            return user;
        }

        public bool UpdateUser(User user, string phone, string coverLetter, string resume, bool publicResume, string firstName, string lastName, bool okToEmail, List<int> categoryAssignments)
        {
            HtmlParser htmlParserAllowSomeHtml = new HtmlParser(HtmlParser.ParseRules.AllowSomeHtml);
            htmlParserAllowSomeHtml.RemoveAllowedTag("a");
            if (!String.IsNullOrEmpty(coverLetter)) { coverLetter = coverLetter.Trim(); }
            if (!String.IsNullOrEmpty(resume)) { resume = resume.Trim(); }

            user.Phone = phone;
            user.CoverLetter = htmlParserAllowSomeHtml.FilterHtml(coverLetter).Trim();
            user.Resume = htmlParserAllowSomeHtml.FilterHtml(resume).Trim();
            user.ShareResumeWithEmployers = publicResume;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.OkToEmail = okToEmail;
            
            List<UserCategory> categoriesToAdd = null;
            List<UserCategory> categoriesToRemove = null;

            if (categoryAssignments != null)
            {
                categoriesToAdd = new List<UserCategory>();
                categoriesToRemove = new List<UserCategory>();

                //figure out which category assignments to remove
                if (user.UserCategories.IsLoaded)
                {
                    foreach (UserCategory userCategory in user.UserCategories)
                    {
                        if (!categoryAssignments.Contains(userCategory.CategoryId))
                        {
                            categoriesToRemove.Add(userCategory);
                        }
                    }
                }

                //new category assignments to add
                foreach (int categoryAssignment in categoryAssignments)
                {
                    bool assignmentAlreadyExists = false;
                    if (user.UserCategories.IsLoaded)
                    {
                        foreach (UserCategory userCategory in user.UserCategories)
                        {
                            if (userCategory.CategoryId == categoryAssignment)
                            {
                                assignmentAlreadyExists = true;
                                break;
                            }
                        }

                    }
                    if (!assignmentAlreadyExists)
                    {
                        categoriesToAdd.Add(UserCategory.CreateUserCategory(-1, -1, categoryAssignment));
                    }
                }
            }
            
            UserDataAccess uda = new UserDataAccess();

            return uda.UpdateUser(user, categoriesToAdd, categoriesToRemove);
        }

        public bool UpdateUserCompany(User user, int companyId, bool isCompanyAdmin)
        {
            user.CompanyId = companyId;
            user.IsCompanyAdmin = isCompanyAdmin;
            
            UserDataAccess uda = new UserDataAccess();
            return uda.UpdateUser(user, null, null);
        }

        public bool UpdateUserResume(User user, string coverLetter, string resume, bool updateCoverLetter, bool updateResume)
        {
            HtmlParser htmlParserAllowSomeHtml = new HtmlParser(HtmlParser.ParseRules.AllowSomeHtml);
            htmlParserAllowSomeHtml.RemoveAllowedTag("a");

            if (updateCoverLetter)
            {
                user.CoverLetter = htmlParserAllowSomeHtml.FilterHtml(coverLetter.Trim()).Trim();
            }

            if (updateResume)
            {
                user.Resume = htmlParserAllowSomeHtml.FilterHtml(resume.Trim()).Trim();
            }

            UserDataAccess uda = new UserDataAccess();
            return uda.UpdateUser(user, null, null);
        }

        public bool UpdateUserCoupon(User user, Nullable<int> couponId)
        {
            if (couponId.HasValue)
            {
                user.CouponId = couponId.Value;
            }
            else
            {
                user.CouponId = null;
            }
            UserDataAccess uda = new UserDataAccess();
            return uda.UpdateUser(user, null, null);
        }

        public bool CreateEmailVerification(User user)
        {
            bool result = false;

            //create link. user needs to click this to verify their account
            UrlManager urlManager = new UrlManager();
            Dictionary<string,string> queryStrings = new Dictionary<string,string>();
            queryStrings.Add("verificationid", user.EmailVerificationGuid.ToString());
            string url = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.VerifyEmail, queryStrings);

            //add the link to the email as a string parameter. also pass in th user object.
            Email emailController = new Email();
            Dictionary<string, string> stringParameters = new Dictionary<string, string>();
            stringParameters.Add("VerificationLink", url);
            result = emailController.SendEmail(user.Email, Email.EmailTemplates.VerifyEmail, stringParameters, user);

            return result;
        }

        public bool CreateEmailResetPassword(string email, string firstName)
        {
            bool result = false;

            MembershipUser membershipUser = Membership.GetUser(email);
            if (membershipUser != null)
            {
                if (membershipUser.IsApproved == true)
                {
                    UserDataAccess uda = new UserDataAccess();
                    User user = uda.CreateUserResetPasswordGuid(email, firstName);
                    if (user != null)
                    {
                        if (user.UserId > 0)
                        {
                            //create link. user needs to click this to verify their email and be able to reset their password
                            UrlManager urlManager = new UrlManager();
                            Dictionary<string, string> queryStrings = new Dictionary<string, string>();
                            queryStrings.Add("verificationid", user.ResetPasswordGuid.ToString());
                            string url = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.ResetPassword, queryStrings);

                            //add the link to the email as a string parameter. also pass in th user object.
                            Email emailController = new Email();
                            Dictionary<string, string> stringParameters = new Dictionary<string, string>();
                            stringParameters.Add("ResetPasswordLink", url);
                            result = emailController.SendEmail(user.Email, Email.EmailTemplates.ResetPasswordEmail, 
                                stringParameters, user);
                        }
                    }
                }
            }
            
            return result;
        }

        public bool UserResetPasswordTokenIsValid(Guid resetPasswordGuid, int resetPasswordValidInMinutes)
        {
            bool result = false;

            UserDataAccess uda = new UserDataAccess();
            User user = uda.GetUserByResetPasswordGuid(resetPasswordGuid, resetPasswordValidInMinutes);
            if (user != null)
            {
                MembershipUser membershipUser = Membership.GetUser(user.Email);
                if (membershipUser.IsApproved == true)
                {
                    result = true;
                }
            }

            return result;
        }

        public bool ValidatePasswordResetTokenAndChangePassword(Guid resetPasswordGuid, int resetPasswordValidInMinutes, string newPassword, string email, string firstName)
        {
            bool result = false;

            UserDataAccess uda = new UserDataAccess();
            User user = uda.GetUserByResetPasswordGuid(resetPasswordGuid, resetPasswordValidInMinutes);
            if (user != null)
            {
                if (user.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                    user.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase))
                {
                    MembershipUser membershipUser = Membership.GetUser(user.Email);
                    if (membershipUser.IsApproved == true)
                    {
                        result = membershipUser.ChangePassword(membershipUser.ResetPassword(), newPassword);
                        if (result)
                        {
                            uda.ClearUserResetPasswordGuid(user.UserId);
                            //send success email
                            Email emailer = new Email();
                            emailer.SendEmail(membershipUser.Email, Email.EmailTemplates.UpdatePassword,
                                null,
                                user);
                        }
                    }
                }
            }

            return result;
        }

        public bool ApproveMembership(Guid guid, int VerificationValidPeriodInHours)
        {
            bool result = false;
            UserDataAccess uda = new UserDataAccess();
            User user = uda.GetUserByGuid(guid, VerificationValidPeriodInHours);
            if (user != null)
            {
                MembershipUser membershipUser = Membership.GetUser(user.Email);
                if (membershipUser.IsApproved == false)
                {
                    membershipUser.IsApproved = true;
                    Membership.UpdateUser(membershipUser);

                    //send approved email
                    UrlManager urlManager = new UrlManager();
                    Email email = new Email();
                    
                    //send employer role assignment email
                    if (Roles.IsUserInRole(membershipUser.UserName, UserRoles.Employer.ToString()))
                    {
                        CompanyManager companyManager = new CompanyManager();
                        var company = companyManager.GetCompany(user.UserId);

                        CompanyApplicationManager companyApplicationManager = new CompanyApplicationManager();
                        List<CompanyApplication> companyApplications = companyApplicationManager.GetCompanyApplication(company.CompanyId);

                        if (!companyApplications.Any())
                        {
                            email.Send_EmployerRoleAssignment_Email(user, company);
                        }
                    }
                    else
                    {
                        string loginLink = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.JobSearch, null);

                        //send email to job seeker.
                        email.SendEmail(user.Email, Email.EmailTemplates.EmailVerified,
                        new Dictionary<string, string>() { { "LoginLink", loginLink } },
                        user);
                    }
                }
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Get the first role for the currently logged in user. Users should generally have only one role.
        /// Contributor roles are disregarded.
        /// </summary>
        /// <returns></returns>
        public UserRoles GetMembershipUserRole()
        {
            UserRoles result = UserRoles.None;
            string[] membershipUserRoles = Roles.GetRolesForUser();
            foreach (string membershipUserRole in membershipUserRoles)
            {
                if (!membershipUserRole.ToLower().Contains("contributor"))
                {
                    result = (UserRoles)Enum.Parse(typeof(UserRoles), membershipUserRole);
                }
            }
            return result;
        }

        /// <summary>
        /// get first role for user
        /// </summary>
        /// <returns></returns>
        public UserRoles GetMembershipUserRole(string username)
        {
            UserRoles result = UserRoles.None;
            string[] membershipUserRoles = Roles.GetRolesForUser(username);
            foreach (string membershipUserRole in membershipUserRoles)
            {
                if (!membershipUserRole.ToLower().Contains("contributor"))
                {
                    result = (UserRoles)Enum.Parse(typeof(UserRoles), membershipUserRole);
                }
            }
            return result;
        }

        /// <summary>
        /// get currently logged in user.
        /// </summary>
        /// <returns></returns>
        public User GetUser()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                UserDataAccess uda = new UserDataAccess();
                return uda.GetUserByEmail(HttpContext.Current.User.Identity.Name);
            }
            return null;
        }

        /// <summary>
        /// get user detail by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUser(int userId)
        {
            UserDataAccess uda = new UserDataAccess();
            return uda.GetUser(userId);
        }

        /// <summary>
        /// Find if a user exists by email. Prevent multiple registration attempts.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool UserExists(string email)
        {
            bool result = false;
            UserDataAccess uda = new UserDataAccess();
            User user = uda.GetUserByEmail(email);
            if (user != null)
            {
                if (user.UserId > 0)
                {
                    result = true;
                }
            }
            
            return result;
        }

        /// <summary>
        /// Get users in the contributor role
        /// </summary>
        /// <returns></returns>
        public List<string> GetContributorUsers()
        {
            return Roles.GetUsersInRole(UserRoles.Contributor.ToString()).ToList();
        }

        public void SignIn(string username)
        {
            FormsAuthentication.RedirectFromLoginPage(username, false, FormsAuthentication.FormsCookiePath);

            CookieManager cookieManager = new CookieManager();
            cookieManager.SetNavigationCookie(GetMembershipUserRole(username));
            cookieManager.SetLoggedInCookie(true);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
            CookieManager cookieManager = new CookieManager();
            cookieManager.SetLoggedInCookie(false);
            string logoutPage = System.Web.Configuration.WebConfigurationManager.AppSettings["LOGOUT_REDIRECT_PAGE"].ToString();
            UrlManager urlManager = new UrlManager();
            HttpContext.Current.Response.Redirect(urlManager.GetUrlRedirectAbsolute(logoutPage, null));
        }

        public string GetUserIPAddress()
        {
            string VisitorsIPAddr = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
            }
            return VisitorsIPAddr;
        }

        public List<User> GetUsersWithUnusedFreeJobPostForEmailReminder()
        {
            int numberOfFreeAdsAllowed = -1;
            if (WebConfigurationManager.AppSettings["NUMBER_OF_FREE_ADS_ALLOWED"] != null)
            {
                Int32.TryParse(WebConfigurationManager.AppSettings["NUMBER_OF_FREE_ADS_ALLOWED"], out numberOfFreeAdsAllowed);
            }
            int freeAdsRefreshIntervalDays = -1;
            if (WebConfigurationManager.AppSettings["FREE_ADS_REFRESH_INTERVAL_DAYS"] != null)
            {
                Int32.TryParse(WebConfigurationManager.AppSettings["FREE_ADS_REFRESH_INTERVAL_DAYS"], out freeAdsRefreshIntervalDays);
            }

            UserDataAccess uda = new UserDataAccess();
            return uda.GetUsersWithUnusedFreeJobPostForEmailReminder(numberOfFreeAdsAllowed, freeAdsRefreshIntervalDays);
        }

        public User GetUnsubscribeUser(Guid unsubscribeId)
        {
            UserDataAccess uda = new UserDataAccess();
            return uda.GetUnsubscribeUser(unsubscribeId);
        }

        public bool UpdateUserUnsubscribe(User user)
        {
            UserDataAccess uda = new UserDataAccess();
            return uda.UpdateUserUnsubscribe(user);
        }

        public bool ResetUserEmailVerificationToken(string email, string firstName)
        {
            bool result = false;
            MembershipUser membershipUser = Membership.GetUser(email);
            if (membershipUser != null)
            {
                if (membershipUser.IsApproved == false)
                {
                    UserDataAccess uda = new UserDataAccess();
                    User user = uda.GetUserByEmail(email);
                    if (user != null)
                    {
                        if (user.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase))
                        {
                            user.EmailVerificationGuid = Guid.NewGuid();
                            user.EmailVerificationGuidDate = DateTime.Now;
                            uda.UpdateUser(user, null, null);
                            CreateEmailVerification(user);
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        public bool IsMembershipUserApproved(string email)
        {
            bool result = false;
            MembershipUser membershipUser = Membership.GetUser(email);
            if (membershipUser != null)
            {
                result = membershipUser.IsApproved;
            }
            return result;
        }

        public void ProcessResumeHelpRequest(string firstName, string lastName, string email, string phone, string resume)
        {
            try
            {
                HtmlParser htmlParserAllowSomeHtml = new HtmlParser(HtmlParser.ParseRules.AllowSomeHtml);
                htmlParserAllowSomeHtml.RemoveAllowedTag("a");
                if (!String.IsNullOrEmpty(resume)) { resume = resume.Trim(); }

                resume = htmlParserAllowSomeHtml.FilterHtml(resume).Trim();

                Email emailManager = new Email();
                emailManager.SendEmail(WebConfigurationManager.AppSettings["RESUME_HELP"].ToString(),
                    Email.EmailTemplates.ResumeHelp, new Dictionary<string, string>()
                            {
                                {"FirstName", firstName},
                                {"LastName", lastName},
                                {"Email", email},
                                {"Phone", phone},
                                {"Resume", resume}
                            }, null);

            }
            catch (System.Exception ex)
            {
                ExceptionManager exceptionManager = new ExceptionManager();
                exceptionManager.AddException(ex);
            }
        }
    }
}
