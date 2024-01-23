using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class CountryManager
    {
        public List<Country> GetCountries()
        {
            CountryDataAccess cda = new CountryDataAccess();
            return cda.GetCountries();
        }

        public List<State> GetStates(string countryCode)
        {
            CountryDataAccess cda = new CountryDataAccess();
            return cda.GetStates(countryCode);
        }

        public ZipCode GetZipCode(int zipCode)
        {
            ZipDataAccess zipDataAccess = new ZipDataAccess();
            return zipDataAccess.GetZipCode(zipCode);
        }
    }
}
