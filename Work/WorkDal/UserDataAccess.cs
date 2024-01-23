using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Web;
using System.Web.Caching;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class UserDataAccess : DataAccess
    {
        private const string cacheName = "User";
        private string cacheKey = "";

        /// <summary>
        /// add a user and return whether the add was successful
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool AddUser(User user)
        {
            using (WorkEntities context = GetContext())
            {
                context.Users.AddObject(user);
                return (context.SaveChanges() == 1);
            }
        }

        public bool UpdateUser(User user, List<UserCategory> categoriesToAdd, List<UserCategory> categoriesToRemove)
        {
            bool result = false;
            if (user != null)
            {
                using (WorkEntities context = GetContext())
                {
                    context.Users.Attach(user);
                    context.ObjectStateManager.ChangeObjectState(user, System.Data.EntityState.Modified);

                    if (categoriesToRemove != null)
                    {
                        foreach (UserCategory categoryToRemove in categoriesToRemove)
                        {
                            context.DeleteObject(categoryToRemove);
                        }
                    }

                    if (categoriesToAdd != null)
                    {
                        foreach (UserCategory categoryToAdd in categoriesToAdd)
                        {
                            user.UserCategories.Add(categoryToAdd);
                        }
                    }

                    result = (context.SaveChanges() >= 1);
                }
            }

            //reset the user's cache
            if (result)
            {
                ResetUserCache(user);
            }

            return result;
        }

        /// <summary>
        /// Get a user by their emails and email verification guid. mostly used for password reset or 
        /// account creation.
        /// </summary>
        /// <param name="guid">the user guid that was created in the last 24 hours</param>
        /// <returns>the user matching the mail</returns>
        public User GetUserByGuid(Guid guid, int VerificationValidPeriodInHours)
        {
            using (WorkEntities context = GetContext())
            {
                var user = from a in context.Users
                           where a.EmailVerificationGuid == guid &&
                            EntityFunctions.AddHours(a.EmailVerificationGuidDate, VerificationValidPeriodInHours) >= DateTime.Now
                           select a;
                return user.FirstOrDefault();
            }
        }

        /// <summary>
        /// Get user by username + firstname and create a reset password guid and set a datetime stamp
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="VerificationValidPeriodInHours"></param>
        /// <returns>User if found. Otherwise null</returns>
        public User CreateUserResetPasswordGuid(string email, string firstName)
        {
            using (WorkEntities context = GetContext())
            {
                var userEntity = from a in context.Users
                                 where a.Email == email &&
                                      a.FirstName == firstName
                                 select a;
                User user = userEntity.FirstOrDefault();

                if (user != null)
                {
                    if (user.UserId > 0)
                    {
                        user.ResetPasswordGuid = Guid.NewGuid();
                        user.ResetPasswordGuidDate = DateTime.Now;
                        ResetUserCache(user);
                        if (context.SaveChanges() != 1)
                        {
                            user = null;
                        }
                    }
                }

                return user;
            }
        }

        public User GetUserByResetPasswordGuid(Guid resetPasswordGuid, int resetPasswordValidInMinutes)
        {
            using (WorkEntities context = GetContext())
            {
                var userEntity = from a in context.Users
                                 where a.ResetPasswordGuid == resetPasswordGuid &&
                                      EntityFunctions.AddMinutes(a.ResetPasswordGuidDate, resetPasswordValidInMinutes) >= DateTime.Now
                                 select a;
                User user = userEntity.FirstOrDefault();
                return user;
            }
        }

        public bool ClearUserResetPasswordGuid(int userId)
        {
            bool result = false;
            using (WorkEntities context = GetContext())
            {
                var userEntity = from a in context.Users
                                 where a.UserId == userId
                                 select a;
                User user = userEntity.FirstOrDefault();
                if (user != null)
                {
                    user.ResetPasswordGuid = null;
                    user.ResetPasswordGuidDate = null;
                    result = (context.SaveChanges() == 1);
                    ResetUserCache(user);
                }
            }
            return result;
        }

        /// <summary>
        /// get user by userid for admin purposes
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUser(int userId)
        {
            using (WorkEntities context = GetContext())
            {
                var userEntity = from u in context.Users
                                 where u.UserId == userId
                                 select u;
                return userEntity.FirstOrDefault();
            }
        }

        public User GetUserByEmail(string email)
        {
            cacheKey = cacheName + "_" + email;

            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                return (User)HttpContext.Current.Cache[cacheKey];
            }
            else
            {
                lock (cacheKey)
                {
                    if (HttpContext.Current.Cache[cacheKey] != null)
                    {
                        return (User)HttpContext.Current.Cache[cacheKey];
                    }
                    else
                    {
                        using (WorkEntities context = GetContext())
                        {
                            var userEntity = from a in context.Users.Include("UserCategories")
                                             where a.Email == email
                                             select a;
                            User user = userEntity.FirstOrDefault();
                            if (user != null)
                            {
                                HttpContext.Current.Cache.Insert(cacheKey, user, null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 30, 0));
                            }
                            return user;
                        }
                    }
                }
            }
        }

        private void ResetUserCache(User user)
        {
            cacheKey = cacheName + "_" + user.Email;

            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                lock (cacheKey)
                {
                    if (HttpContext.Current.Cache[cacheKey] != null)
                    {
                        HttpContext.Current.Cache.Remove(cacheKey);
                    }
                }
            }
        }

        public List<User> GetUsersWithUnusedFreeJobPostForEmailReminder(int numberOfFreeAdsAllowed, int freeAdsRefreshIntervalDays)
        {
            using (WorkEntities context = GetContext())
            {
                DateTime freeAdsSince = DateTime.Now.AddDays(-freeAdsRefreshIntervalDays);

                var users = ((from u in context.Users
                              join aspnet in context.aspnet_Membership on u.Email equals aspnet.Email
                              where u.CompanyId > 0 &&
                              u.Company.AllowFreeJobPosts == true &&
                              u.OkToEmail == true &&
                              aspnet.IsApproved == true &&
                              u.Company.JobPosts.Where(jp => jp.IsFreeAd && jp.Published == true && jp.StartDate >= freeAdsSince).Count() < numberOfFreeAdsAllowed
                              select u) as ObjectQuery<User>).Include("Company").Include("Company.JobPosts");
                return users.ToList();
            }
        }

        public User GetUnsubscribeUser(Guid unsubscribeId)
        {
            using (WorkEntities context = GetContext())
            {
                var user = from u in context.Users 
                           where u.OkToEmailGuid == unsubscribeId && u.OkToEmail == true
                           select u;
                return user.FirstOrDefault();
            }
        }

        public bool UpdateUserUnsubscribe(User user)
        {
            bool result = false;
            if (user != null)
            {
                using (WorkEntities context = GetContext())
                {
                    user.OkToEmail = false;
                    context.Users.Attach(user);
                    context.ObjectStateManager.ChangeObjectState(user, System.Data.EntityState.Modified);
                    result = (context.SaveChanges() == 1);
                }
            }
            return result;
        }
    }
}
