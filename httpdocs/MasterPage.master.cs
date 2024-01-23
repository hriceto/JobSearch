using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.UserControls;
using System.Web.Configuration;

namespace HristoEvtimov.Websites.Work.Web
{
    public partial class MasterPage : GeneralMasterPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            form1.Action = Request.RawUrl;
            
            //setup google analytics
            if (!Request.Url.AbsolutePath.ToLower().Contains("admin"))
            {
                litGoogleAnalytics.Text = WebConfigurationManager.AppSettings["GOOGLE_ANALYTICS"].ToString();
            }

            MenuManager menuManager = new MenuManager();
            
            //read which menu to display from cookie;
            Control menu = LoadControl(menuManager.GetMenuPath());
            if (menu != null)
            {
                menu.ID = "menu";
                phMenu.Controls.Add(menu);
            }

            if (!IsPostBack)
            {
                UrlManager urlManager = new UrlManager();

                aLogo.HRef = hypFooter1.NavigateUrl = urlManager.GetUrlRedirectAbsolute("/", null);
                hypFooter1.Text = GetGlobalResourceObject("GlobalResources", "hypFooterGettingStarted").ToString();

                hypFooter2.NavigateUrl = urlManager.GetWwwUrlRedirectAbsolute(UrlManager.PageLink.AboutUs, null);
                hypFooter2.Target = "_blank";
                hypFooter2.Text = GetGlobalResourceObject("GlobalResources", "hypFooterAboutUs").ToString();
                
                hypFooter3.NavigateUrl = urlManager.GetWwwUrlRedirectAbsolute(UrlManager.PageLink.PrivacyPolicy, null);
                hypFooter3.Target = "_blank";
                hypFooter3.Text = GetGlobalResourceObject("GlobalResources", "hypFooterPrivacyPolicy").ToString();

                hypFooter4.NavigateUrl = urlManager.GetWwwUrlRedirectAbsolute(UrlManager.PageLink.TermsConditions, null);
                hypFooter4.Target = "_blank";
                hypFooter4.Text = GetGlobalResourceObject("GlobalResources", "hypFooterTermsConditions").ToString();

                hypFooter5.NavigateUrl = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.ContactUs, null);
                hypFooter5.Text = GetGlobalResourceObject("GlobalResources", "hypFooterContactUs").ToString();

                hypFooter6.NavigateUrl = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.Industries, null);
                hypFooter6.Text = GetGlobalResourceObject("GlobalResources", "hypFooterIndustryList").ToString();

                hypFooter7.NavigateUrl = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.Resources, null);
                hypFooter7.Text = GetGlobalResourceObject("GlobalResources", "hypFooterResources").ToString();
            }
        }
    }
}   