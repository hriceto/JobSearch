using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Security;

namespace HristoEvtimov.Websites.Work.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = GetGlobalResourceObject("PageTitles", "strDefault").ToString();

            if (!IsPostBack)
            {
                UrlManager urlManager = new UrlManager();
                string urlSearchPage = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.JobSearch, null);
                string urlPricingPage = urlManager.GetWwwUrlRedirectAbsolute(UrlManager.PageLink.Pricing, null);
                string urlEmployerStartPage = urlManager.GetUrlRedirectAbsolute("/Employers.aspx", null);

                lblHeader1.Text = String.Format(GetLocalResourceObject("lblHeader1").ToString(), urlSearchPage);
                lblHeader2.Text = String.Format(GetLocalResourceObject("lblHeader2").ToString(), urlPricingPage);
                lblEmployers.Text = String.Format(GetLocalResourceObject("strEmployers").ToString(), WebConfigurationManager.AppSettings["FREE_ADS_REFRESH_INTERVAL_DAYS"].ToString());
                
                string loginLink = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.Login, null);
                lblLogin.Text = String.Format(GetLocalResourceObject("lblLogin").ToString(), loginLink);
                hypEmployerRegistration.NavigateUrl = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.RegisterCompanyUser, null);
                hypJobSeekerRegistration.NavigateUrl = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.RegisterUser, null);

                lblEmployersPricing.Text = String.Format(GetLocalResourceObject("lblEmployersPricing").ToString(), urlPricingPage, urlEmployerStartPage);
            }
        }
    }
}