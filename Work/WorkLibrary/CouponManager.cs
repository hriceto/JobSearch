using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class CouponManager
    {
        public enum CouponStatus { Active, Expired, Used, Pending };
        private CouponDataAccess cda = null;

        public CouponManager()
        {
            cda = new CouponDataAccess();
        }

        public int AddCoupon(string userId, string companyId, string couponCode,
            int numberOfUsesLimit, string discountPercentage, string discountAmount,
            string startDate, string endDate, int createdByUserId)
        {
            return AddUpdateCoupon(-1, userId, companyId, couponCode, numberOfUsesLimit, discountPercentage,
                discountAmount, startDate, endDate, DateTime.Now, createdByUserId, DateTime.Now, createdByUserId);
        }

        public int UpdateCoupon(int couponId, string userId, string companyId, string couponCode,
            int numberOfUsesLimit, string discountPercentage, string discountAmount,
            string startDate, string endDate, int lastUpdatedByUserId)
        {
            return AddUpdateCoupon(couponId, userId, companyId, couponCode, numberOfUsesLimit, discountPercentage,
                discountAmount, startDate, endDate, DateTime.Now, lastUpdatedByUserId, DateTime.Now, lastUpdatedByUserId);
        }

        private int AddUpdateCoupon(int couponId, string userId, string companyId, string couponCode,
            int numberOfUsesLimit, string discountPercentage, string discountAmount,
            string startDate, string endDate, DateTime createdDate, int createdByUserId, 
            DateTime lastUpdatedDate, int lastUpdatedByUserId)
        {
            int result = -1;

            try
            {
                Coupon coupon = null;
                if (couponId > 0)
                {
                    coupon = cda.GetCouponForAdmin(couponId);
                }
                else
                {
                    coupon = new Coupon();
                    coupon.NumberOfUses = 0;
                    coupon.Active = true;
                    coupon.CreatedDate = createdDate;
                    coupon.CreatedByUserId = createdByUserId;
                }

                coupon.UserId = null;
                coupon.CompanyId = null;
                coupon.DiscountPercentage = null;
                coupon.DiscountAmount = null;
                coupon.StartDate = null;
                coupon.EndDate = null;
                coupon.CouponCode = couponCode;
                coupon.NumberOfUsesLimit = numberOfUsesLimit;
                coupon.LastUpdatedDate = lastUpdatedDate;
                coupon.LastUpdatedByUserId = lastUpdatedByUserId;

                if (!String.IsNullOrEmpty(userId))
                {
                    coupon.UserId = Int32.Parse(userId);
                }
                if (!String.IsNullOrEmpty(companyId))
                {
                    coupon.CompanyId = Int32.Parse(companyId);
                }
                if (!String.IsNullOrEmpty(discountPercentage))
                {
                    coupon.DiscountPercentage = Decimal.Parse(discountPercentage);
                }
                if (!String.IsNullOrEmpty(discountAmount))
                {
                    coupon.DiscountAmount = Decimal.Parse(discountAmount);
                }
                if (!String.IsNullOrEmpty(startDate))
                {
                    coupon.StartDate = DateTime.Parse(startDate);
                }
                if (!String.IsNullOrEmpty(endDate))
                {
                    coupon.EndDate = DateTime.Parse(endDate);
                }

                if (couponId > 0)
                {
                    result = cda.UpdateCoupon(coupon);
                }
                else
                {
                    result = cda.AddCoupon(coupon);
                    if (result > 0)
                    {
                        coupon.CouponId = result;
                        SendNewCouponEmail(coupon);
                    }
                }
            }
            catch (System.Exception ex)
            {
                result = -1;
                //
            }

            return result;
        }

        public int UpdateCouponUses(int couponId, int newUses)
        {
            int result = -1;

            try
            {
                Coupon coupon = null;
                if (couponId > 0)
                {
                    coupon = cda.GetCouponForAdmin(couponId);
                    coupon.NumberOfUses += newUses;
                    result = cda.UpdateCoupon(coupon);
                }
            }
            catch (System.Exception ex)
            {
                result = -1;
                //
            }

            return result;
        }

        private void SendNewCouponEmail(Coupon coupon)
        {
            Email email = new Email();
            UrlManager urlManager = new UrlManager();
            CouponManager couponManager = new CouponManager();

            string myCouponsPage = urlManager.GetUrlRedirectAbsolute("/Employer/MyCoupons.aspx", null);
            string addJobPage = urlManager.GetUrlRedirectAbsolute("/Employer/AddEditJob.aspx", null);
            string checkoutPage = urlManager.GetUrlRedirectAbsolute("/Employer/Checkout.aspx", null);

            List<User> userList = new List<User>();
            if (coupon.UserId.HasValue)
            {
                //if user is specified email them even though they might not have opted into marketing emails.
                UserManager userManager = new UserManager();
                userList.Add(userManager.GetUser(coupon.UserId.Value));
            }
            else if (coupon.CompanyId.HasValue)
            {
                CompanyManager companyManager = new CompanyManager();
                Company company = companyManager.GetCompanyForReview(coupon.CompanyId.Value);
                foreach (User user in company.Users)
                {
                    //if company id then email only users who have opted into marketting communication
                    if (user.OkToEmail)
                    {
                        userList.Add(user);
                    }
                }
            }

            foreach (User user in userList)
            {
                email.SendEmail(user.Email, Email.EmailTemplates.CouponCreation, 
                    new Dictionary<string, string>() { 
                        { "MyCouponsPage", myCouponsPage }, 
                        { "CouponDiscount", couponManager.GetCouponDiscount(coupon) },
                        { "AddJobPage", addJobPage },
                        { "CheckoutPage", checkoutPage },
                        { "unsubscribeUrl", 
                                            urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.Unsubscribe, 
                                                new Dictionary<string,string>(){
                                                {"unsubscribeid", user.OkToEmailGuid.ToString()},
                                                {"email", user.Email}
                                                }) }
                    }, user, coupon);
            }
        }

        public Coupon GetCouponForAdmin(int couponId)
        {
            return cda.GetCouponForAdmin(couponId);
        }

        public bool DeactivateCoupon(int couponId, int lastUpdatedByUserId)
        {
            return cda.DeactivateCoupon(couponId, lastUpdatedByUserId);
        }

        public List<Coupon> GetCouponsForAdmin(Nullable<int> companyId, Nullable<int> userId, int page, int pageSize, out int totalNumberOfResults)
        {
            return cda.GetCouponsForAdmin(companyId, userId, page, pageSize, out totalNumberOfResults);
        }

        public List<Coupon> GetMyCoupons(int companyId, int userId)
        {
            return cda.GetMyCoupons(companyId, userId);
        }

        public List<Coupon> GetSiteCoupons()
        {
            return cda.GetSiteCoupons();
        }

        public Coupon GetActiveCoupon(int couponId, int companyId, int userId)
        {
            return cda.GetActiveCoupon(couponId, "", companyId, userId);
        }

        public Coupon GetActiveCoupon(string couponCode, int companyId, int userId)
        {
            return cda.GetActiveCoupon(-1, couponCode, companyId, userId);
        }

        public CouponStatus GetCouponStatus(Coupon coupon)
        {
            CouponStatus status = CouponStatus.Active;

            if (coupon.NumberOfUsesLimit == coupon.NumberOfUses)
            {
                status = CouponStatus.Used;
            }
            else if (coupon.EndDate.HasValue)
            {
                if (DateTime.Now > coupon.EndDate.Value)
                {
                    status = CouponStatus.Expired;
                }
            }
            else if (coupon.StartDate.HasValue)
            {
                if (DateTime.Now < coupon.StartDate.Value)
                {
                    status = CouponStatus.Pending;
                }
            }

            return status;
        }

        public string GetCouponDiscount(Coupon coupon)
        {
            string result = "";
            if (coupon.DiscountPercentage.HasValue && coupon.DiscountAmount.HasValue)
            {
                result = String.Format(System.Web.HttpContext.GetGlobalResourceObject("GlobalResources", "strDiscountPercentAndValue").ToString(), coupon.DiscountPercentage.Value, String.Format("{0:C}", coupon.DiscountAmount.Value));
            }
            else if (coupon.DiscountPercentage.HasValue)
            {
                result = String.Format(System.Web.HttpContext.GetGlobalResourceObject("GlobalResources", "strDiscountPercent").ToString(), coupon.DiscountPercentage.Value);
            }
            else if (coupon.DiscountAmount.HasValue)
            {
                result = String.Format(System.Web.HttpContext.GetGlobalResourceObject("GlobalResources", "strDiscountValue").ToString(), coupon.DiscountAmount.Value);
            }
            return result;
        }
    }
}
