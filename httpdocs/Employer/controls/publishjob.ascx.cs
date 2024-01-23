using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.BusinessObjects;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.Web.Employer.Controls
{
    public partial class publishjob : JobEditControlBase, IFormValidation
    {
        public enum PriceValues { Free, Paid, PaidAnonymous }

        protected void Page_Load(object sender, EventArgs e)
        {
            StringValidation stringValidation = new StringValidation();
            regReplyUrl.ValidationExpression = String.Format(
                regReplyUrl.ValidationExpression,
                stringValidation.GetAllowedCharacters(StringValidation.SanitizeEntityNames.Url));

            if (!IsPostBack)
            {
                ListItem liFree = rdbPrice.Items[0];
                ListItem liPaid = rdbPrice.Items[1];
                ListItem liPaidAnonymous = rdbPrice.Items[2];
                liFree.Value = ((int)PriceValues.Free).ToString();
                liPaid.Value = ((int)PriceValues.Paid).ToString();
                liPaidAnonymous.Value = ((int)PriceValues.PaidAnonymous).ToString();

                bool isUpdate = (GetEditJobId() > 0);
                if (isUpdate)
                {
                    JobPost jobPost = GetEditJob();
                    if (jobPost != null)
                    {
                        User currentUser = GetUser();
                        CompanyManager companyManager = new CompanyManager();
                        Company currentUserCompany = companyManager.GetCompany(currentUser.UserId);

                        Checkout checkout = new Checkout();
                        Prices prices = checkout.GetPrices(currentUserCompany);

                        liFree.Text = String.Format(liFree.Text, prices.FreeAdDuration);
                        liPaid.Text = String.Format(liPaid.Text, String.Format("{0:C}", prices.BasicAdPrice), prices.PaidAdDuration);
                        liPaidAnonymous.Text = String.Format(liPaidAnonymous.Text, String.Format("{0:C}", prices.AnonymousAdPrice), prices.PaidAdDuration);

                        PricingOptions pricingOptions = checkout.GetAllowedPriceOptions(currentUser, currentUserCompany, jobPost.JobPostId);
                        //if the company has not been reviewed yet display a message to the user.
                        if (pricingOptions.DisplayCompanyNotReviewed)
                        {
                            AddSystemMessage(GetLocalResourceObject("strNotReviewedYet").ToString(),
                                        GeneralMasterPageBase.SystemMessageTypes.Info,
                                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                        }

                        if (!pricingOptions.AllowFreeAds)
                        {
                            
                            rdbPrice.Items.Remove(liFree);
                            //display a label that tells the user how long until next free add
                            if (pricingOptions.NextFreeJob > TimeSpan.MinValue)
                            {
                                int nextFreeJobAdDays = pricingOptions.NextFreeJob.Days;
                                if(nextFreeJobAdDays == 0) nextFreeJobAdDays = 1;
                                if (nextFreeJobAdDays == 1)
                                {
                                    AddSystemMessage(String.Format(GetLocalResourceObject("strNextFreeJobAd1").ToString(), nextFreeJobAdDays),
                                        GeneralMasterPageBase.SystemMessageTypes.Info,
                                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                                }
                                else
                                {
                                    AddSystemMessage(String.Format(GetLocalResourceObject("strNextFreeJobAd2").ToString(), nextFreeJobAdDays),
                                        GeneralMasterPageBase.SystemMessageTypes.Info,
                                        GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                                }
                            }                            
                        }
                        if (!pricingOptions.AllowPaidAds)
                        {
                            rdbPrice.Items.Remove(liPaid);
                        }
                        if (!pricingOptions.AllowPaidAnonymousAds)
                        {
                            rdbPrice.Items.Remove(liPaidAnonymous);
                        }

                        if (jobPost.AddedToCart)
                        {
                            lblTitle.Text = String.Format(GetLocalResourceObject("strEditingCartItem").ToString(), jobPost.Title);

                            //Item has already been added to the cart. Preselected everything that was input last time.
                            if (jobPost.IsFreeAd)
                            {
                                if (rdbPrice.Items.Contains(liFree))
                                {
                                    rdbPrice.SelectedValue = liFree.Value;
                                }
                            }
                            if (jobPost.IsPaidAd)
                            {
                                if (rdbPrice.Items.Contains(liPaid))
                                {
                                    rdbPrice.SelectedValue = liPaid.Value;
                                }
                            }
                            if (jobPost.IsAnonymousAd)
                            {
                                if (rdbPrice.Items.Contains(liPaidAnonymous))
                                {
                                    rdbPrice.SelectedValue = liPaidAnonymous.Value;
                                }
                            }

                            if (jobPost.IsPaidAd || jobPost.IsAnonymousAd)
                            {
                                txtKeywords.Text = jobPost.Keywords;
                                txtReplyEmail.Text = jobPost.ReplyEmail;
                                txtReplyUrl.Text = jobPost.ReplyUrl;
                                txtStartDate.Text = jobPost.StartDate != null ? ((DateTime)jobPost.StartDate).ToString("MM/dd/yyyy") : "";
                                if (jobPost.StartDate < DateTime.Now)
                                {
                                    txtStartDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                                }
                                divPaid.Attributes["style"] = "";
                            }
                        }
                        else
                        {
                            lblTitle.Text = String.Format(GetLocalResourceObject("strAddingCartItem").ToString(), jobPost.Title);

                            //adding for the first tiem. preselect cheapest available option
                            if (rdbPrice.Items.Contains(liFree))
                            {
                                rdbPrice.SelectedValue = liFree.Value;
                            }
                            else if (rdbPrice.Items.Contains(liPaid))
                            {
                                rdbPrice.SelectedValue = liPaid.Value;
                                divPaid.Attributes["style"] = "";
                            }
                            else if (rdbPrice.Items.Contains(liPaidAnonymous))
                            {
                                rdbPrice.SelectedValue = liPaidAnonymous.Value;
                                divPaid.Attributes["style"] = "";
                            }
                        }
                    }
                    else
                    {
                        AddSystemMessage(GetGlobalResourceObject("GlobalResources", "strInvalidJobToPublish").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.NextLoad);
                        UrlManager urlManager = new UrlManager();
                        Response.Redirect(urlManager.GetUrlRedirectAbsolute("/Employer/MyJobs.aspx", null));
                    }
                }
                else
                {
                    UrlManager urlManager = new UrlManager();
                    Response.Redirect(urlManager.GetUrlRedirectAbsolute("/Employer/MyJobs.aspx", null));
                }
            }     
        }
        
        protected void btnPublish_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                int publishJobId = GetEditJobId();
                bool isUpdate = (publishJobId > 0);
                if (isUpdate)
                {
                    bool isFree = (rdbPrice.SelectedValue == ((int)PriceValues.Free).ToString());
                    bool isPaid = (rdbPrice.SelectedValue == ((int)PriceValues.Paid).ToString() || rdbPrice.SelectedValue == ((int)PriceValues.PaidAnonymous).ToString());
                    bool isPaidAnonymous = (rdbPrice.SelectedValue == ((int)PriceValues.PaidAnonymous).ToString());

                    DateTime startDate = DateTime.Now;
                    DateTime.TryParse(txtStartDate.Text, out startDate);

                    JobManager jobManager = new JobManager();

                    User currentUser = GetUser();
                    bool updateOk = jobManager.UpdateJobPostPublishInfo(currentUser.UserId, publishJobId, isFree, isPaid,
                        isPaidAnonymous, txtKeywords.Text, txtReplyEmail.Text, txtReplyUrl.Text, startDate);

                    if (updateOk)
                    {
                        UrlManager urlManger = new UrlManager();
                        Response.Redirect(urlManger.GetUrlRedirectAbsolute("/Employer/Checkout.aspx", null));
                    }
                    else
                    {
                        AddSystemMessage(GetGlobalResourceObject("GlobalResources", "strPublishFailed").ToString(),
                            GeneralMasterPageBase.SystemMessageTypes.Error,
                            GeneralMasterPageBase.SystemMessageDisplayTimes.Now);
                    }
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
            if (rdbPrice.SelectedValue != ((int)PriceValues.Free).ToString())
            {
                StringValidation stringValidation = new StringValidation();
                txtKeywords.Text = stringValidation.SanitizeUserInputString(txtKeywords.Text, StringValidation.SanitizeEntityNames.Keywords);
                txtReplyUrl.Text = stringValidation.SanitizeUserInputString(txtReplyUrl.Text, StringValidation.SanitizeEntityNames.Url);
                Page.Validate("PaidAd");
            }
            return Page.IsValid && heFormValidator.ValidateForm();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            UrlManager urlManager = new UrlManager();
            Response.Redirect(urlManager.GetUrlRedirectAbsolute("/Employer/MyJobs.aspx", null));
        }
    }
}