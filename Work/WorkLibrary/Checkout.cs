using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Security;
using System.Security.Cryptography;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary.Security;
using HristoEvtimov.Websites.Work.WorkLibrary.BusinessObjects;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class Checkout
    {
        public PricingOptions GetAllowedPriceOptions(User currentUser, Company currentUserCompany, int jobPostId)
        {
            PricingOptions result = new PricingOptions();

            result.NextFreeJob = TimeSpan.MinValue;
            result.AllowFreeAds = false;
            result.AllowPaidAds = false;
            result.AllowPaidAnonymousAds = true;
            result.DisplayCompanyNotReviewed = false;

            if (currentUser.CompanyId == null)
            {
                result.AllowPaidAnonymousAds = false;
                return result;
            }

            if (currentUserCompany != null)
            {
                if (!currentUserCompany.IsRecruiter)
                {
                    result.AllowPaidAds = true;
                }
                
                //unlimited free ads
                if (currentUserCompany.AllowUnlimitedFreeJobPosts.HasValue && currentUserCompany.AllowUnlimitedFreeJobPosts.Value)
                {
                    result.AllowFreeAds = true;
                    return result;
                }

                //figure out if free ad is allowed. need to consider both 
                //adds that are in the current shopping cart and adds that have been posted
                if (!currentUserCompany.AllowFreeJobPosts)
                {
                    result.DisplayCompanyNotReviewed = !currentUserCompany.Reviewed;
                    return result;
                }
                int numberOfFreeAdsAllowed = -1;
                if (WebConfigurationManager.AppSettings["NUMBER_OF_FREE_ADS_ALLOWED"] != null)
                {
                    Int32.TryParse(WebConfigurationManager.AppSettings["NUMBER_OF_FREE_ADS_ALLOWED"], out numberOfFreeAdsAllowed);
                }
                int freeAdsRefreshIntervalDays = -1;
                if (WebConfigurationManager.AppSettings["FREE_ADS_REFRESH_INTERVAL_DAYS"] != null)
                {
                    Int32.TryParse(WebConfigurationManager.AppSettings["FREE_ADS_REFRESH_INTERVAL_DAYS"], out freeAdsRefreshIntervalDays);
                }
                JobManager jobManager = new JobManager();
                int numberOfFreeAdsPosted = jobManager.GetJobPostsByCompanyPublishedFreeCount((int)currentUser.CompanyId, freeAdsRefreshIntervalDays);
                if (numberOfFreeAdsPosted < numberOfFreeAdsAllowed)
                {
                    //so far allow free ads. check items that are marked for checkout. If
                    //there are already free ads configured for checkout maybe we won't allow any more.
                    List<JobPost> shoppingCartJobs = jobManager.GetJobPostsByCompanyInShoppingCart((int)currentUser.CompanyId);
                    int numberOfFreeAdsInCheckout = 0;

                    if (shoppingCartJobs != null)
                    {
                        foreach (JobPost shoppingCartJob in shoppingCartJobs)
                        {
                            if (shoppingCartJob.JobPostId != jobPostId)
                            {
                                if (shoppingCartJob.IsFreeAd)
                                {
                                    numberOfFreeAdsInCheckout++;
                                }
                            }
                        }
                    }

                    if (numberOfFreeAdsInCheckout < numberOfFreeAdsAllowed)
                    {
                        result.AllowFreeAds = true;
                    }
                }
                else
                {
                    DateTime? oldestJobStartDate = jobManager.GetPublishedFreeJobLatestStartDate((int)currentUser.CompanyId, freeAdsRefreshIntervalDays);
                    if (oldestJobStartDate.HasValue)
                    {
                        result.NextFreeJob = (new TimeSpan(freeAdsRefreshIntervalDays, 0, 0, 0, 0)).Subtract(DateTime.Now - (DateTime)oldestJobStartDate);
                    }
                }
            }

            return result;
        }

        public Prices GetPrices(Company currentUserCompany)
        {
            Prices result = new Prices();

            result.FreeAdDuration = Int32.Parse(WebConfigurationManager.AppSettings["FREE_AD_DURATION"].ToString());
            result.PaidAdDuration = Int32.Parse(WebConfigurationManager.AppSettings["PAID_AD_DURATION"].ToString());

            Decimal basicAdPrice = 0;
            Decimal.TryParse(WebConfigurationManager.AppSettings["BASIC_AD_PRICE"].ToString(), out basicAdPrice);
            result.BasicAdPrice = basicAdPrice;

            Decimal anonymousAdPrice = 0;
            Decimal.TryParse(WebConfigurationManager.AppSettings["ANONYMOUS_AD_PRICE"].ToString(), out anonymousAdPrice);
            result.AnonymousAdPrice = anonymousAdPrice + basicAdPrice;

            if (currentUserCompany.AllowUnlimitedFreeJobPosts.HasValue && currentUserCompany.AllowUnlimitedFreeJobPosts.Value)
            {
                result.BasicAdPrice = 0;
                result.AnonymousAdPrice = 0;
            }

            return result;
        }

        public CheckoutTotals GetTotals(List<JobPost> shoppingCartJobs, Coupon coupon)
        {
            CheckoutTotals result = new CheckoutTotals();
            result.Subtotal = 0;
            result.Tax = 0;
            result.Total = 0;
            result.CouponDiscount = 0;
            result.CouponUses = 0;

            foreach (JobPost shoppingCartJob in shoppingCartJobs)
            {
                if (shoppingCartJob.PriceTotal != null)
                {
                    decimal jobPrice = (Decimal)shoppingCartJob.PriceTotal;
                    result.Subtotal += jobPrice;
                    if (coupon != null)
                    {
                        if ((coupon.NumberOfUsesLimit > (coupon.NumberOfUses + result.CouponUses)) && jobPrice > 0)
                        {
                            decimal jobCouponDiscount = GetCouponDiscount(coupon, jobPrice);
                            result.CouponDiscount += jobCouponDiscount;
                            result.CouponUses++;
                        }
                    }
                }
            }

            result.Total = result.Tax + result.Subtotal - result.CouponDiscount;
            return result;
        }

        public int PublishShoppingCart(User currentUser, 
                                int billingAddressId, bool saveNewBillingAddress, string billingFirstName,
                                string billingLastName, string billingAddress1, string billingAddress2,
                                string billingCity, string billingState, string billingZip, string billingCountry,
                                SecureString creditCardNumber, SecureString creditCardExpirationMonth,
                                SecureString creditCardExpirationYear, SecureString creditCardCVV)
        {
            if (IsCheckoutDisabled())
            {
                return -1;
            }
            JobManager jobManager = new JobManager();
            List<JobPost> shoppingCartJobs = jobManager.GetJobPostsByUserInShoppingCart(currentUser.UserId);

            CouponManager couponManager = new CouponManager();
            Coupon coupon = null;
            Nullable<int> couponId = null;
            if(currentUser.CouponId.HasValue)
            {
                coupon = couponManager.GetActiveCoupon(currentUser.CouponId.Value, currentUser.CompanyId.Value, currentUser.UserId);
                if (coupon != null)
                {
                    couponId = coupon.CouponId;
                }
                else
                {
                    UserManager userManager = new UserManager();
                    userManager.UpdateUserCoupon(currentUser, null);
                }
            }
            var totals = GetTotals(shoppingCartJobs, coupon);

            int orderId = -1;
            if (totals.Total > 0)
            {
                BillingAddress billingAddress = null;
                BillingAddressManager billingAddressManager = new BillingAddressManager();

                if (billingAddressId == -1)
                {
                    if (saveNewBillingAddress)
                    {
                        billingAddress = billingAddressManager.CreateBillingAddress(currentUser.UserId, billingFirstName,
                            billingLastName, billingAddress1, billingAddress2, billingCity,
                            billingState, billingZip, billingCountry);
                    }
                    else
                    {
                        billingAddress = billingAddressManager.CreateBillingAddressObject(currentUser.UserId, billingFirstName,
                            billingLastName, billingAddress1, billingAddress2, billingCity,
                            billingState, billingZip, billingCountry);
                    }
                }
                else
                {
                    billingAddress = billingAddressManager.GetBillingAddress(currentUser.UserId, billingAddressId);
                }
                orderId = PublishShoppingCartPaid(currentUser, shoppingCartJobs, billingAddress, totals.Subtotal, totals.Tax, totals.CouponDiscount, 
                    totals.Total, couponId, creditCardNumber, creditCardExpirationMonth, creditCardExpirationYear, creditCardCVV);
            }
            else
            {
                orderId = PublishShoppingCartFree(currentUser, shoppingCartJobs, totals.Subtotal, totals.Tax, totals.CouponDiscount, totals.Total, couponId);
            }

            if (orderId > 0)
            {
                Order newOrder = GetOrderJobData(currentUser.UserId, orderId);
                SendOrderConfirmationEmail(currentUser, newOrder);
                if(newOrder.Coupon != null)
                {
                    couponManager.UpdateCouponUses(newOrder.Coupon.CouponId, totals.CouponUses);
                    if (newOrder.Coupon.NumberOfUsesLimit == newOrder.Coupon.NumberOfUses + totals.CouponUses)
                    {
                        UserManager userManager = new UserManager();
                        userManager.UpdateUserCoupon(currentUser, null);
                    }
                }
                SendEmailToAdmin(currentUser, shoppingCartJobs, newOrder);
                jobManager.SaveShoppingCartCountInCookie(currentUser.UserId);
            }

            return orderId;
        }

        /// <summary>
        /// Publish the shopping cart for the current user if all jobs in there are free
        /// </summary>
        /// <returns></returns>
        private int PublishShoppingCartFree(User currentUser, List<JobPost> shoppingCartJobs, 
            Decimal subtotal, Decimal tax, Decimal couponDiscount, Decimal total, Nullable<int> couponId)
        {
            int result = -1;

            //create a new order
            int newOrderId = CreateOrder(currentUser.UserId, true);

            if (PublishShoppingCartJobs(currentUser, shoppingCartJobs, newOrderId))
            {
                UpdateOrder(currentUser.UserId, newOrderId, subtotal, tax, couponDiscount, total, couponId,
                            "", "", "", "", "", "", true, "", "",
                            "", "", new SecureString());
                result = newOrderId;
            }

            return result;
        }

        private int PublishShoppingCartPaid(User currentUser, List<JobPost> shoppingCartJobs, BillingAddress billingAddress,
                                Decimal subtotal, Decimal tax, Decimal couponDiscount, Decimal total, Nullable<int> couponId,
                                SecureString creditCardNumber, SecureString creditCardExpirationMonth,
                                SecureString creditCardExpirationYear, SecureString creditCardCVV)
        {
            int result = -1;
            AuthorizeNet.IGatewayResponse response = null;
            int newOrderId = -1;
            
            int invalidCheckoutAttempts = GetNumberOfInvalidCheckoutAttempts(currentUser.UserId);
            int maxInvalidCheckoutAttempts = MaxNumberOfCheckoutAttempts();

            if (invalidCheckoutAttempts < maxInvalidCheckoutAttempts)
            {
                //create a new order
                newOrderId = CreateOrder(currentUser.UserId, false);

                AuthorizeNet.AuthorizationRequest request;
                try
                {
                    request = new AuthorizeNet.AuthorizationRequest(creditCardNumber.ConvertToUnsecureString(), creditCardExpirationMonth.ConvertToUnsecureString() + creditCardExpirationYear.ConvertToUnsecureString(), total, "Job postings");
                }
                catch (System.Exception ex)
                {
                    ExceptionManager exceptionManager = new ExceptionManager();
                    exceptionManager.AddException(ex);
                    return result;
                }

                //add cvv code
                try
                {
                    request.AddCardCode(creditCardCVV.ConvertToUnsecureString());
                }
                catch (System.Exception ex)
                {
                    ExceptionManager exceptionManager = new ExceptionManager();
                    exceptionManager.AddException(ex);
                    return result;
                }

                request.AddCustomer(currentUser.UserId.ToString(), billingAddress.FirstName, billingAddress.LastName,
                    (billingAddress.Address1 + " " + billingAddress.Address2).Trim(), billingAddress.State,
                    billingAddress.Zip);
                request.Country = billingAddress.Country;
                request.AddInvoice(newOrderId.ToString());

                //add user ip address
                UserManager userManager = new UserManager();
                request.AddFraudCheck(userManager.GetUserIPAddress());

                //add line items
                foreach (JobPost shoppingCartJob in shoppingCartJobs)
                {
                    request.AddLineItem(shoppingCartJob.JobPostId.ToString(), "job posting", "job posting", 1, Math.Round((Decimal)shoppingCartJob.PriceTotal, 2), false);
                }

                CompanyManager companyManager = new CompanyManager();
                Company currentUserCompany = companyManager.GetCompany(currentUser.UserId);
                if (currentUserCompany == null)
                {
                    return -1;
                }
                //add more parameters. company name, email, and city
                if (currentUserCompany != null)
                {
                    request.Company = currentUserCompany.Name;
                }
                request.City = billingAddress.City;
                request.Email = currentUser.Email;

                bool gatewayTestMode = false;
                if (WebConfigurationManager.AppSettings["GATEWAY_IS_TEST_MODE"] != null)
                {
                    Boolean.TryParse(WebConfigurationManager.AppSettings["GATEWAY_IS_TEST_MODE"], out gatewayTestMode);
                }
                string gatewayApiLogin = "";
                if (WebConfigurationManager.AppSettings["GATEWAY_API_LOGIN"] != null)
                {
                    gatewayApiLogin = WebConfigurationManager.AppSettings["GATEWAY_API_LOGIN"];
                }
                string gatewayTransactionKey = "";
                if (WebConfigurationManager.AppSettings["GATEWAY_TRANSACTION_KEY"] != null)
                {
                    gatewayTransactionKey = WebConfigurationManager.AppSettings["GATEWAY_TRANSACTION_KEY"];
                }

                var gateway = new AuthorizeNet.Gateway(gatewayApiLogin, gatewayTransactionKey, gatewayTestMode);

                try
                {
                    response = gateway.Send(request);
                }
                catch (System.Exception ex)
                {
                    ExceptionManager exceptionManager = new ExceptionManager();
                    exceptionManager.AddException(ex);
                }


                bool invalidCheckout = true;
                if (response != null)
                {
                    if (response.Approved)
                    {
                        UpdateOrder(currentUser.UserId, newOrderId, subtotal, tax, couponDiscount, total, couponId,
                            billingAddress.Address1, billingAddress.Address2, billingAddress.City,
                            billingAddress.State, billingAddress.Zip, billingAddress.Country,
                            true, response.AuthorizationCode, response.ResponseCode,
                            response.Message, response.TransactionID, creditCardNumber);

                        if (PublishShoppingCartJobs(currentUser, shoppingCartJobs, newOrderId))
                        {
                            result = newOrderId;
                            invalidCheckout = false;
                        }
                    }
                }

                if (invalidCheckout)
                {
                    //Order was not successful. We leave the order record in. Then we can count how many order
                    //attempts there were in a period of time and deny further checkouts 
                    LogManager logManager = new LogManager();
                    logManager.AddLog("Invalid checkout attempt", currentUser.UserId, "orderId=" + newOrderId.ToString(), "");

                    string authorizationCode = "";
                    string responseCode = "";
                    string message = "";
                    string transactionId = "";
                    if (response != null)
                    {
                        authorizationCode = response.AuthorizationCode;
                        responseCode = response.ResponseCode;
                        message = response.Message;
                        transactionId = response.TransactionID;
                    }

                    UpdateOrder(currentUser.UserId, newOrderId, subtotal, tax, couponDiscount, total, couponId,
                        billingAddress.Address1, billingAddress.Address2, billingAddress.City,
                        billingAddress.State, billingAddress.Zip, billingAddress.Country,
                        false, authorizationCode, responseCode,
                        message, transactionId, creditCardNumber);
                    result = -1;
                }
            }

            return result;
        }

        private bool PublishShoppingCartJobs(User currentUser, List<JobPost> shoppingCartJobs, int newOrderId)
        {
            UrlManager urlManager = new UrlManager();
            foreach (JobPost shoppingCartJob in shoppingCartJobs)
            {
                shoppingCartJob.OrderId = newOrderId;
                shoppingCartJob.Published = true;
                if (shoppingCartJob.StartDate < DateTime.Now)
                {
                    shoppingCartJob.StartDate = DateTime.Now;
                }
                shoppingCartJob.Duration = Checkout.GetAdDuration(shoppingCartJob);
                shoppingCartJob.EndDate = ((DateTime)shoppingCartJob.StartDate).AddDays(shoppingCartJob.Duration.Value);
            }

            JobDataAccess jda = new JobDataAccess();

            return jda.UpdateJobs(shoppingCartJobs);
        }

        private int CreateOrder(int userId, bool orderComplete)
        {
            int result = -1;

            Order newOrder = Order.CreateOrder(-1, userId, DateTime.Now, 0, 0, 0, 0, "", "", "", "", "", orderComplete);
            newOrder.BillingAddress2 = "";

            OrderDataAccess oda = new OrderDataAccess();
            result = oda.AddOrder(newOrder);

            return result;
        }


        private bool UpdateOrder(int userId, int orderId, Decimal subtotal, Decimal tax, Decimal couponDiscount, Decimal total, Nullable<int> couponId, String billingAddress1,
            string billingAddress2, string billingCity, string billingState, string billingZip, string billingCountry,
            bool orderComplete, string gwAuthorizationCode, string gwResponseCode, string gwMessage, string gwTransactionId,
            SecureString creditCardNumber)
        {
            bool result = false;
            Order updateOrder = GetOrder(userId, orderId);
            if (updateOrder != null)
            {
                OrderDataAccess oda = new OrderDataAccess();
                updateOrder.Subtotal = subtotal;
                updateOrder.Tax = tax;
                updateOrder.CouponDiscount = couponDiscount;
                if (couponId.HasValue)
                {
                    updateOrder.CouponId = couponId.Value;
                }
                updateOrder.Total = total;
                updateOrder.BillingAddress1 = billingAddress1;
                updateOrder.BillingAddress2 = billingAddress2;
                updateOrder.BillingCity = billingCity;
                updateOrder.BillingState = billingState;
                updateOrder.BillingZip = billingZip;
                updateOrder.BillingCountry = billingCountry;
                updateOrder.OrderComplete = orderComplete;
                updateOrder.GWAuthorizationCode = gwAuthorizationCode;
                updateOrder.GWResponseCode = gwResponseCode;
                updateOrder.GWMessage = gwMessage;
                updateOrder.GWTransactionID = gwTransactionId;

                EncryptCardholderData(creditCardNumber, updateOrder);
                
                result = oda.UpdateOrder(updateOrder);
            }

            return result;
        }

        /// <summary>
        /// Get a specific order for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Order GetOrder(int userId, int orderId)
        {
            OrderDataAccess oda = new OrderDataAccess();
            return oda.GetOrder(userId, orderId);
        }

        public Order GetOrderJobData(int userId, int orderId)
        {
            OrderDataAccess oda = new OrderDataAccess();
            return oda.GetOrderJobData(userId, orderId);
        }

        /// <summary>
        /// Gets the number of invalid checkout attempts in the past few hours.
        /// The number of hours is controlled by an app config setting.
        /// If these are over the allowed limit then a checkout will be denied.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int GetNumberOfInvalidCheckoutAttempts(int userId)
        {
            int invalidAttemptWindow = 12;
            if (WebConfigurationManager.AppSettings["MAX_NUMBER_OF_INVALID_CHECKOUT_ATTEMPTS_TIME_WINDOW"] != null)
            {
                invalidAttemptWindow = Int32.Parse(WebConfigurationManager.AppSettings["MAX_NUMBER_OF_INVALID_CHECKOUT_ATTEMPTS_TIME_WINDOW"]);
            }
            OrderDataAccess oda = new OrderDataAccess();
            return oda.GetNumberOfInvalidCheckoutAttempts(userId, invalidAttemptWindow);
        }

        /// <summary>
        /// gets an app config setting for the maximum number of invalid checkout 
        /// attempts before denying the user any further checkout attempts.
        /// </summary>
        /// <returns></returns>
        public int MaxNumberOfCheckoutAttempts()
        {
            int result = 5;
            if (WebConfigurationManager.AppSettings["MAX_NUMBER_OF_INVALID_CHECKOUT_ATTEMPTS"] != null)
            {
                result = Int32.Parse(WebConfigurationManager.AppSettings["MAX_NUMBER_OF_INVALID_CHECKOUT_ATTEMPTS"]);
            }
            return result;
        }

        public void SendOrderConfirmationEmail(User currentUser, Order newOrder)
        {
            Email email = new Email();
            DecryptCardholderData(newOrder);

            Coupon coupon = new Coupon();
            if (newOrder.CouponId.HasValue)
            {
                CouponManager couponManager = new CouponManager();
                coupon = couponManager.GetCouponForAdmin(newOrder.CouponId.Value);
            }
            if (newOrder != null)
            {
                UrlManager urlManager = new UrlManager();
                string jobsPage = urlManager.GetUrlRedirectAbsolute("/Employer/MyJobs.aspx", null);

                email.SendEmail(currentUser.Email, Email.EmailTemplates.OrderConfirmation, new Dictionary<string, string>() { {"JobsPage", jobsPage} }, currentUser, newOrder, newOrder.JobPosts, coupon);
            }
        }

        public bool IsCheckoutDisabled()
        {
            bool result = false;
            if (WebConfigurationManager.AppSettings["CHECKOUT_DISABLED"] != null)
            {
                result = Boolean.Parse(WebConfigurationManager.AppSettings["CHECKOUT_DISABLED"].ToString());
            }
            return result;
        }

        public void EncryptCardholderData(SecureString creditCardNumber, Order order)
        {
            //encrypt credit card last four and card type
            try
            {
                string cardNumberUnsecure = creditCardNumber.ConvertToUnsecureString();

                if (!String.IsNullOrEmpty(cardNumberUnsecure))
                {
                    //create salt
                    RNGCryptoServiceProvider rngCrypto = new RNGCryptoServiceProvider();
                    byte[] saltBytes = new byte[32];
                    rngCrypto.GetNonZeroBytes(saltBytes);
                    order.Salt = Convert.ToBase64String(saltBytes);

                    //encrypt last four
                    Encryption encyption = new Encryption();
                    string cardLastFour = cardNumberUnsecure.Substring(cardNumberUnsecure.Length - 4, 4);
                    cardLastFour = cardLastFour.PadLeft(cardNumberUnsecure.Length, '*');
                    order.CardLastFour = encyption.Encrypt(cardLastFour, order.Salt);

                    Validation.CreditCardValidation creditCardValidation = new Validation.CreditCardValidation();
                    order.CardType = encyption.Encrypt(creditCardValidation.GetCreditCardType(cardNumberUnsecure.Substring(0, 4), cardNumberUnsecure.Length).ToString(), order.Salt);
                }
            }
            catch (System.Exception ex)
            {
                ExceptionManager exceptionManager = new ExceptionManager();
                exceptionManager.AddException(ex);
            }
        }

        public void DecryptCardholderData(Order order)
        {
            try
            {
                if (!String.IsNullOrEmpty(order.Salt))
                {
                    Encryption encryption = new Encryption();
                    order.CardLastFour = encryption.Decrypt(order.CardLastFour, order.Salt);
                    order.CardType = encryption.Decrypt(order.CardType, order.Salt);
                }
            }
            catch (System.Exception ex)
            {
                ExceptionManager exceptionManager = new ExceptionManager();
                exceptionManager.AddException(ex);
            }
        }

        public static int GetAdDuration(JobPost jobPost)
        {
            int duration = 30;
            if (jobPost.Duration.HasValue)
            {
                duration = jobPost.Duration.Value;
            }
            else
            {
                string durationSettingName = "";
                if (jobPost.IsFreeAd)
                {
                    durationSettingName = "FREE_AD_DURATION";
                }
                else if (jobPost.IsPaidAd || jobPost.IsAnonymousAd)
                {
                    durationSettingName = "PAID_AD_DURATION";
                }

                if (!Int32.TryParse(WebConfigurationManager.AppSettings[durationSettingName], out duration))
                {
                    duration = 30;
                }
            }
            return duration;
        }

        private decimal GetCouponDiscount(Coupon coupon, decimal subtotal)
        {
            decimal discount = 0;

            if (coupon.DiscountAmount.HasValue)
            {
                discount = coupon.DiscountAmount.Value;
            }
            if (coupon.DiscountPercentage.HasValue)
            {
                decimal tmpSubtotal = subtotal;
                if (coupon.DiscountAmount.HasValue)
                {
                    tmpSubtotal = tmpSubtotal - coupon.DiscountAmount.Value;
                }
                decimal percentDiscount = (tmpSubtotal) * ((decimal)coupon.DiscountPercentage.Value / 100);
                discount += percentDiscount;
            }

            if(discount > subtotal)
            {
                discount = subtotal;
            }
            if(discount < 0)
            {
                discount = 0;
            }

            discount = Decimal.Round(discount, 2, MidpointRounding.AwayFromZero);

            return discount;
        }

        private void SendEmailToAdmin(User user, List<JobPost> jobs, Order order)
        {
            try
            {
                Email emailManager = new Email();
                emailManager.SendEmail(WebConfigurationManager.AppSettings["EMAIL_TO"], Email.EmailTemplates.AdminAlertJobPosted, null, user, jobs, order);
            }
            catch (System.Exception ex)
            {

            }
        }
    }
}
