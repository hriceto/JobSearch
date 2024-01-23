using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;
using HristoEvtimov.Websites.Work.WorkLibrary.Security;

namespace HristoEvtimov.Websites.Work.Web.Employer.Controls
{
    public partial class checkout : GeneralControlBase, IFormValidation
    {
        UrlManager urlManager;
        CouponManager couponManager;

        private bool IsPaidCheckout
        {
            get
            {
                if (Session["IsPaidCheckout"] != null)
                {
                    return Boolean.Parse(Session["IsPaidCheckout"].ToString());
                }
                return false;
            }
            set { Session["IsPaidCheckout"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            urlManager = new UrlManager();
            couponManager = new CouponManager();
                
            heShoppingCart.DeleteJob += new _shoppingcart.DeleteJobEventHandler(heShoppingCart_DeleteJob);

            if (!IsPostBack)
            {
                LoadCheckout();
                BindStateAndCountry();
            }
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {

                int billingAddressId = -1;
                Int32.TryParse(rdblBillingAddresses.SelectedValue, out billingAddressId);

                User currentUser = GetUser();
                Checkout checkout = new Checkout();

                int invalidCheckoutAttempts = checkout.GetNumberOfInvalidCheckoutAttempts(currentUser.UserId);
                int maxInvalidCheckoutAttempts = checkout.MaxNumberOfCheckoutAttempts();
                if (invalidCheckoutAttempts >= maxInvalidCheckoutAttempts)
                {
                    string errorMessage = GetGlobalResourceObject("GlobalResources", "strInvalidNumberOfCHeckoutAttempts").ToString();
                    AddSystemMessage(errorMessage,
                        GeneralMasterPageBase.SystemMessageTypes.Error,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                    return;
                }

                int orderId = -1;

                try
                {
                    orderId = checkout.PublishShoppingCart(currentUser,
                         billingAddressId, chkbSaveBillingAddress.Checked, txtBillingFirstName.Text,
                         txtBillingLastName.Text, txtBillingAddress1.Text, txtBillingAddress2.Text, txtBillingCity.Text,
                         ddlBillingState.SelectedValue, txtBillingZip.Text, ddlBillingCountry.SelectedValue,
                         txtCreditCardNumber.Text.ConvertToSecureString(), ddlCreditCardExpirationMonth.SelectedValue.ConvertToSecureString(),
                         ddlCreditCardExpirationYear.SelectedValue.ConvertToSecureString(), txtCreditCardCVV.Text.ConvertToSecureString());
                }
                catch (System.Exception ex)
                {
                    ExceptionManager exceptionManager = new ExceptionManager();
                    exceptionManager.AddException(ex);
                    orderId = -1;
                }

                if (orderId > 0)
                {
                    Response.Redirect(urlManager.GetUrlRedirectAbsolute("/Employer/CheckoutConfirmation.aspx", new Dictionary<string, string>() { { "orderid", orderId.ToString() } }));
                }
                else
                {
                    AddSystemMessage(GetGlobalResourceObject("GlobalResources", "strOrderCreationError").ToString(),
                        GeneralMasterPageBase.SystemMessageTypes.Error,
                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                }
            }
            else
            {
                AddSystemMessage(GetGlobalResourceObject("GlobalResources", "strFormNotFilledOutProperly").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
            }
        }

        public bool ValidateForm()
        {
            StringValidation stringValidation = new StringValidation();

            if (IsPaidCheckout)
            {
                if (rdblBillingAddresses.SelectedValue == "-1")
                {
                    txtBillingFirstName.Text = stringValidation.SanitizeUserInputString(txtBillingFirstName.Text, StringValidation.SanitizeEntityNames.Name);
                    txtBillingLastName.Text = stringValidation.SanitizeUserInputString(txtBillingLastName.Text, StringValidation.SanitizeEntityNames.Name);
                    txtBillingAddress1.Text = stringValidation.SanitizeUserInputString(txtBillingAddress1.Text, StringValidation.SanitizeEntityNames.Address);
                    txtBillingAddress2.Text = stringValidation.SanitizeUserInputString(txtBillingAddress2.Text, StringValidation.SanitizeEntityNames.Address);
                    txtBillingCity.Text = stringValidation.SanitizeUserInputString(txtBillingCity.Text, StringValidation.SanitizeEntityNames.City);

                    Page.Validate("NewBillingAddress");
                }

                txtCreditCardNumber.Text = stringValidation.SanitizeUserInputString(txtCreditCardNumber.Text, StringValidation.SanitizeEntityNames.CreditCardNumber);
                txtCreditCardCVV.Text = stringValidation.SanitizeUserInputString(txtCreditCardCVV.Text, StringValidation.SanitizeEntityNames.CreditCardCVV);
                Page.Validate("CheckoutCreditCardNumber");
                Page.Validate("Checkout");
            }

            return Page.IsValid && heFormValidator.ValidateForm();
        }

        protected void cvalCreditCardNumber_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CreditCardValidation creditCardValidation = new CreditCardValidation();
            bool isValid = creditCardValidation.ValidateCreditCardNumber(txtCreditCardNumber.Text);
            if(isValid)
            {
                isValid = creditCardValidation.ValidateCreditCardType(txtCreditCardNumber.Text.Substring(0, 4), txtCreditCardNumber.Text.Length);
            }

            args.IsValid = isValid;
        }

        protected void cvalCreditCardExpirationMonth_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime expirationDate = new DateTime(Int32.Parse(ddlCreditCardExpirationYear.Text),
                Int32.Parse(ddlCreditCardExpirationMonth.Text),
                1
                );
            expirationDate = expirationDate.AddMonths(1);
            args.IsValid = DateTime.Now < expirationDate;
        }
        
        private void LoadCheckout()
        {
            User currentUser = GetUser();

            JobManager jobManager = new JobManager();
            List<JobPost> shoppingCartJobs = jobManager.GetJobPostsByUserInShoppingCart(currentUser.UserId);
            string myJobsUrl = urlManager.GetUrlRedirectAbsolute("/Employer/MyJobs.aspx", null);

            lblTermsAccept.Text = String.Format(
                GetLocalResourceObject("strTermsAccept").ToString(),
                urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.TermsConditions, null)
                );

            if (shoppingCartJobs.Count == 0)
            {
                pnlEmptyCart.Visible = true;
                plhCheckout.Visible = false;
                lblEmptyCart.Text = String.Format(GetLocalResourceObject("strEmptyCart").ToString(),
                    myJobsUrl);
                return;
            }

            hypBackToMyJobs.NavigateUrl = myJobsUrl;

            Checkout checkout = new Checkout();
            CouponManager couponManager = new CouponManager();
            Coupon coupon = null;
            if (currentUser.CouponId.HasValue)
            {
                coupon = couponManager.GetActiveCoupon(currentUser.CouponId.Value, currentUser.CompanyId.Value, currentUser.UserId);
            }
            
            var totals = checkout.GetTotals(shoppingCartJobs, coupon);

            heShoppingCart.LoadCart(shoppingCartJobs, totals.Subtotal, totals.Tax, totals.Total, totals.CouponDiscount);

            pnlBillingAddress.Visible = false;
            pnlPayment.Visible = false;
            pnlFreeCheckout.Visible = false;
            pnlCheckout.Visible = false;
            pnlCoupon.Visible = false;

            //allow to checkout from this page. no review page needed.
            if (checkout.IsCheckoutDisabled())
            {
                AddSystemMessage(GetLocalResourceObject("strCheckoutDisabled").ToString(), 
                    GeneralMasterPageBase.SystemMessageTypes.Error,
                    GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
            }
            else if (totals.Total > 0)
            {
                pnlBillingAddress.Visible = true;
                pnlPayment.Visible = true;
                pnlCheckout.Visible = true;
                pnlCoupon.Visible = true;
                IsPaidCheckout = true;
                LoadBillingAddress(currentUser);
                LoadCreditCardForm();
                LoadCouponForm(currentUser);
            }
            else
            {
                pnlFreeCheckout.Visible = true;
                pnlCheckout.Visible = true;
                if (totals.CouponDiscount > 0)
                {
                    pnlCoupon.Visible = true;
                    LoadCouponForm(currentUser);
                }
            }

            litAuthDotNetSeal.Text = WebConfigurationManager.AppSettings["AUTHORIZE_NET_SEAL"].ToString();
            btnPublish.Visible = (shoppingCartJobs.Count > 0);
        }

        protected void BindStateAndCountry()
        {
            CountryManager countryManager = new CountryManager();
            ddlBillingCountry.DataSource = countryManager.GetCountries();
            ddlBillingCountry.DataBind();

            ddlBillingState.DataSource = countryManager.GetStates(ddlBillingCountry.SelectedItem.Value);
            ddlBillingState.DataBind();
            ddlBillingState.SelectedValue = WebConfigurationManager.AppSettings["DEFAULT_STATE"].ToString();
        }

        protected void heShoppingCart_DeleteJob(object sender, _shoppingcart.DeleteJobEventArgs e)
        {
            JobManager jobManager = new JobManager();
            User currentUser = GetUser();
            if (jobManager.UpdateJobPostRemoveFromCart(currentUser.UserId, e.JobPostId))
            {
                this.AddSystemMessage(GetLocalResourceObject("strRemoveFromCartSuccess").ToString(), GeneralMasterPageBase.SystemMessageTypes.OK, GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
            }
            else
            {
                this.AddSystemMessage(GetLocalResourceObject("strRemoveFromCartFailure").ToString(), GeneralMasterPageBase.SystemMessageTypes.Error, GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
            }
            LoadCheckout();
        }

        private void LoadBillingAddress(User currentUser)
        {
            rdblBillingAddresses.Items.Clear();
            BillingAddressManager billingAddressManager = new BillingAddressManager();
            List<BillingAddress> billingAddresses = billingAddressManager.GetBillingAddresses(currentUser.UserId);

            foreach (BillingAddress billingAddress in billingAddresses)
            {
                ListItem liBillingAddress = new ListItem(
                    billingAddress.Address1 + " " + billingAddress.Address2 + " " +
                    billingAddress.City + " " + billingAddress.State + "," + billingAddress.Zip,
                    billingAddress.BillingAddressId.ToString());
                liBillingAddress.Attributes.Add("OnClick", "javascript:ChangeBillingOption(this.value);");
                rdblBillingAddresses.Items.Add(liBillingAddress);
            }

            ListItem liAddNew = new ListItem(GetLocalResourceObject("rdblBillingAddresses_Item1").ToString(), "-1");
            liAddNew.Attributes.Add("OnClick", "javascript:ChangeBillingOption(this.value);");
            rdblBillingAddresses.Items.Add(liAddNew);
            if (billingAddresses.Count == 0)
            {
                //display panel (clear class that hides it)
                rdblBillingAddresses.SelectedValue = liAddNew.Value;
                divNewBillingAddress.Attributes["style"] = "";
            }
        }

        private void LoadCreditCardForm()
        {
            ddlCreditCardExpirationYear.Items.Clear();
            ddlCreditCardExpirationYear.Items.Add(new ListItem(GetLocalResourceObject("ddlCreditCardExpirationYear_Item1").ToString(), ""));
            for (int i = DateTime.Now.Year; i <= DateTime.Now.AddYears(6).Year; i++)
            {
                ddlCreditCardExpirationYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        private void LoadCouponForm(User currentUser)
        {
            bool noCoupon = true;
            btnRemoveCoupon.Visible = false;
            btnEnterCoupon.Visible = false;
            if (currentUser.CouponId.HasValue)
            {
                Coupon coupon = couponManager.GetActiveCoupon(currentUser.CouponId.Value, currentUser.CompanyId.Value, currentUser.UserId);
                if(coupon != null)
                {
                    noCoupon = false;
                    txtEnterCoupon.Enabled = false;
                    txtEnterCoupon.Text = coupon.CouponCode;
                    btnRemoveCoupon.Visible = true;
                }
            }

            if (noCoupon)
            {
                txtEnterCoupon.Enabled = true;
                btnEnterCoupon.Visible = true;
                txtEnterCoupon.Text = "";
            }
        }

        protected void btnEnterCoupon_Click(object sender, EventArgs e)
        {
            UserManager userManger = new UserManager();
            User user = userManger.GetUser();
            
            Coupon coupon = couponManager.GetActiveCoupon(txtEnterCoupon.Text, user.CompanyId.Value, user.UserId);
            if (coupon == null)
            {
                AddSystemMessage(String.Format(GetLocalResourceObject("strEnterCouponInvalid").ToString(),
                    urlManager.GetUrlRedirectAbsolute("/Employer/MyCoupons.aspx", null)),
                    GeneralMasterPageBase.SystemMessageTypes.Error,
                    GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                return;
            }

            if (userManger.UpdateUserCoupon(user, coupon.CouponId))
            {
                AddSystemMessage(GetLocalResourceObject("strEnterCouponOK").ToString(),
                    GeneralMasterPageBase.SystemMessageTypes.OK,
                    GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                LoadCheckout();
            }
            else
            {
                AddSystemMessage(GetLocalResourceObject("strEnterCouponError").ToString(),
                    GeneralMasterPageBase.SystemMessageTypes.Error,
                    GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
            }
        }

        protected void btnRemoveCoupon_Click(object sender, EventArgs e)
        {
            UserManager userManger = new UserManager();
            User user = userManger.GetUser();
            if (userManger.UpdateUserCoupon(user, null))
            {
                AddSystemMessage(GetLocalResourceObject("strRemoveCouponOK").ToString(),
                    GeneralMasterPageBase.SystemMessageTypes.OK,
                    GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                LoadCheckout();
            }
        }
    }
}