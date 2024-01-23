using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class BillingAddressManager
    {
        public List<BillingAddress> GetBillingAddresses(int userId)
        {
            BillingAddressDataAccess bada = new BillingAddressDataAccess();
            return bada.GetBillingAddresses(userId);
        }

        public BillingAddress CreateBillingAddress(int userId, string firstName, string lastName, string address1,
            string address2, string city, string state, string zip, string country)
        {
            BillingAddress newBillingAddress = CreateBillingAddressObject(userId, firstName, lastName,
                address1, address2, city, state, zip, country);
            BillingAddressDataAccess bada = new BillingAddressDataAccess();
            bada.AddBillingAddress(newBillingAddress);
            return newBillingAddress;
        }

        public BillingAddress CreateBillingAddressObject(int userId, string firstName, string lastName, string address1,
            string address2, string city, string state, string zip, string country)
        {
            BillingAddress newBillingAddress = BillingAddress.CreateBillingAddress(-1, userId, firstName, lastName,
                address1, city, state, zip, country, DateTime.Now);
            newBillingAddress.Address2 = address2;
            return newBillingAddress;
        }

        public BillingAddress GetBillingAddress(int userId, int billingAddressId)
        {
            BillingAddressDataAccess bada = new BillingAddressDataAccess();
            return bada.GetBillingAddress(userId, billingAddressId);
        }
    }
}
