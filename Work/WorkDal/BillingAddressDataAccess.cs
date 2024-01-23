using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class BillingAddressDataAccess : DataAccess
    {
        public List<BillingAddress> GetBillingAddresses(int userId)
        {
            using (WorkEntities context = GetContext())
            {
                var addresses = from ba in context.BillingAddresses 
                                where ba.UserId == userId
                                select ba;
                return addresses.ToList();
            }
        }

        public int AddBillingAddress(BillingAddress newBillingAddress)
        {
            int result = -1;

            using (WorkEntities context = GetContext())
            {
                context.BillingAddresses.AddObject(newBillingAddress);
                if (context.SaveChanges() >= 1)
                {
                    result = newBillingAddress.BillingAddressId;
                }
            }

            return result;
        }

        public BillingAddress GetBillingAddress(int userId, int billingAddressId)
        {
            using (WorkEntities context = GetContext())
            {
                var billingAddress = (from ba in context.BillingAddresses
                                      where ba.UserId == userId &&
                                      ba.BillingAddressId == billingAddressId
                                      select ba).FirstOrDefault();
                return billingAddress;
            }
        }
    }
}
