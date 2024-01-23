using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class CouponDataAccess : DataAccess
    {
        public int AddCoupon(Coupon coupon)
        {
            int result = -1;
            using (WorkEntities context = GetContext())
            {
                context.Coupons.AddObject(coupon);
                if (context.SaveChanges() == 1)
                {
                    result = coupon.CouponId;
                }
            }
            return result;
        }

        public int UpdateCoupon(Coupon coupon)
        {
            int result = -1;
            using (WorkEntities context = GetContext())
            {
                if (coupon != null)
                {
                    context.Coupons.Attach(coupon);
                    context.ObjectStateManager.ChangeObjectState(coupon, System.Data.EntityState.Modified);
                    if (context.SaveChanges() == 1)
                    {
                        result = coupon.CouponId;
                    }
                }
            }
            return result;
        }

        public Coupon GetCouponForAdmin(int couponId)
        {
            Coupon result = null;
            using (WorkEntities context = GetContext())
            {
                result = (from c in context.Coupons.Include("Company").Include("User")
                          where c.CouponId == couponId && c.Active == true 
                          select c).FirstOrDefault();
            }
            return result;
        }

        public bool DeactivateCoupon(int couponId, int lastUpdatedByUserId)
        {
            bool result = false;
            using (WorkEntities context = GetContext())
            {
                var coupon = (from c in context.Coupons
                              where c.CouponId == couponId && c.Active == true
                              select c).FirstOrDefault();
                if (coupon != null)
                {
                    coupon.Active = false;
                    coupon.LastUpdatedByUserId = lastUpdatedByUserId;
                    coupon.LastUpdatedDate = DateTime.Now;

                    result = (context.SaveChanges() == 1);
                }
            }
            return result;
        }

        public List<Coupon> GetCouponsForAdmin(Nullable<int> companyId, Nullable<int> userId, int page, int pageSize, out int totalNumberOfResults)
        {
            using (WorkEntities context = GetContext())
            {
                var results = from c in context.Coupons.Include("Company").Include("User")
                              where c.Active == true &&
                              ((c.UserId.HasValue && userId.HasValue && c.UserId == userId) || !userId.HasValue) &&
                              ((c.CompanyId.HasValue && companyId.HasValue && c.CompanyId == companyId) || !companyId.HasValue)
                              orderby c.StartDate descending
                              select c;
                totalNumberOfResults = results.Count();

                return results.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public List<Coupon> GetMyCoupons(int companyId, int userId)
        {
            using (WorkEntities context = GetContext())
            {
                var results = from c in context.Coupons
                              where c.Active == true &&
                              (c.UserId == userId || (!c.UserId.HasValue && c.CompanyId == companyId))
                              orderby c.StartDate descending
                              select c;

                return results.ToList();
            }
        }

        public List<Coupon> GetSiteCoupons()
        {
            using (WorkEntities context = GetContext())
            {
                var results = from c in context.Coupons
                              where c.Active == true &&
                              (!c.UserId.HasValue && !c.CompanyId.HasValue) &&
                              c.NumberOfUsesLimit > c.NumberOfUses
                              orderby c.StartDate descending
                              select c;

                return results.ToList();
            }
        }

        public Coupon GetActiveCoupon(int couponId, string couponCode, int companyId, int userId)
        {
            using (WorkEntities context = GetContext())
            {
                var todayDate = DateTime.Now.Date;
                var todayDateTime = DateTime.Now;
                var results = from c in context.Coupons
                              where c.Active == true &&
                              (c.UserId == userId || (!c.UserId.HasValue && c.CompanyId == companyId) || (c.UserId == null && c.CompanyId == null)) &&
                              (((c.CouponId == couponId) && (couponId > 0)) || ((c.CouponCode == couponCode) && (!String.IsNullOrEmpty(couponCode)))) &&
                              c.NumberOfUses < c.NumberOfUsesLimit &&
                              (c.StartDate == null || (c.StartDate != null && c.StartDate <= todayDateTime)) &&
                              (c.EndDate == null || (c.EndDate != null && c.EndDate >= todayDate))
                              select c;

                return results.FirstOrDefault();
            }
        }
    }
}
