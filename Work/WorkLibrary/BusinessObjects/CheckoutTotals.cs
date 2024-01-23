using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HristoEvtimov.Websites.Work.WorkLibrary.BusinessObjects
{
    public class CheckoutTotals
    {
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public decimal CouponDiscount { get; set; }
        public int CouponUses { get; set; }
    }
}
