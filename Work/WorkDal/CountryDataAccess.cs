using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class CountryDataAccess: DataAccess
    {
        public List<Country> GetCountries()
        {
            using (WorkEntities context = GetContext())
            {
                var countryQuery = from c in context.Countries
                              where c.CountryCode == "US"
                              select c;
                return countryQuery.ToList();
            }
        }

        public List<State> GetStates(string countryCode)
        {
            using (WorkEntities context = GetContext())
            {
                var stateQuery = from s in context.States
                                   where s.CountryCode == countryCode
                                   select s;
                return stateQuery.ToList();
            }
        }
    }
}
