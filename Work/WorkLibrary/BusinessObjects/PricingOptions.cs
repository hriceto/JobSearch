using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HristoEvtimov.Websites.Work.WorkLibrary.BusinessObjects
{
    public class PricingOptions
    {
        public bool AllowFreeAds { get; set; }
        public bool AllowPaidAds { get; set; }
        public bool AllowPaidAnonymousAds { get; set; }
        public TimeSpan NextFreeJob { get; set; }
        public bool DisplayCompanyNotReviewed { get; set; }
    }
}
