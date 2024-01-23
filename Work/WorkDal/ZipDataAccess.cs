using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class ZipDataAccess : DataAccess 
    {
        /// <summary>
        /// Get zip code details. lat, lon, city, etc.
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public ZipCode GetZipCode(int zipCode)
        {
            using (WorkEntities context = GetContext())
            {
                var result = from z in context.ZipCodes where z.Zip == zipCode select z;
                return result.FirstOrDefault();
            }
        }
    }
}
