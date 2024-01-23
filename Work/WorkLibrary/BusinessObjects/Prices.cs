using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HristoEvtimov.Websites.Work.WorkLibrary.BusinessObjects
{
    public class Prices
    {
        public int FreeAdDuration { get; set; }
        public int PaidAdDuration { get; set; }
        public Decimal BasicAdPrice { get; set; }
        public Decimal AnonymousAdPrice { get; set; }
    }
}
