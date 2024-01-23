using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary.Validation;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class UrlManager
    {
        public enum PageLink { VerifyEmail, ResetPassword, JobDetail, JobSearch, JobApplication, 
            RegisterCompanyUser, RegisterUser, Login, TermsConditions, PrivacyPolicy, AboutUs, Pricing,
            ContactUs, Unsubscribe, Industries, Resources, BlogDetail, BlogList }

        public string GetUrlRedirectAbsolute(PageLink page, Dictionary<string, string> queryStringParameters)
        {
            string urlRedirectFrom = "/" + page.ToString() + ".aspx";
            return GetUrlRedirectAbsolute(urlRedirectFrom, queryStringParameters);
        }

        public string GetUrlRedirectAbsolute(string urlRedirectFrom, Dictionary<string, string> queryStringParameters)
        {
            UrlDataAccess uda = new UrlDataAccess();
            
            //get all rules from cache or db
            List<Url> allUrlRules = uda.GetUrls();
            
            //figure out which rules apply to the current base url
            List<Url> urlRules = new List<Url>();
            foreach (Url urlRule in allUrlRules)
            {
                if (urlRule.RedirectFrom.StartsWith(urlRedirectFrom, true, null) || urlRule.RedirectFrom.StartsWith(urlRedirectFrom.Replace(".","\\."), true, null))
                {
                    urlRules.Add(urlRule);
                }
            }
            
            //add querystrings to the current base url
            if (queryStringParameters != null)
            {
                string queryStrings = "";
                string separator = "";
                foreach (KeyValuePair<string, string> queryStringParameter in queryStringParameters)
                {
                    queryStrings = queryStrings + separator + queryStringParameter.Key + "=" + HttpContext.Current.Server.UrlEncode(queryStringParameter.Value);
                    separator = "&";
                }
                if (!String.IsNullOrEmpty(queryStrings))
                {
                    urlRedirectFrom = urlRedirectFrom + "?" + queryStrings;
                }
            }

            //match all rules that apply to the current base url to the current base url + querysting
            string urlRedirectTo = urlRedirectFrom;
            string protocol = "http://";
            foreach (Url urlRule in urlRules)
            {
                Regex regex = new Regex("^" + urlRule.RedirectFrom + "$", RegexOptions.IgnoreCase);
                if (regex.IsMatch(urlRedirectFrom))
                {
                    urlRedirectTo = regex.Replace(urlRedirectFrom, urlRule.RedirectTo);
                    if (urlRule.RequireSsl)
                    {
                        protocol = "https://";
                    }
                }
            }

            //append the domain & web protocol
            urlRedirectTo = protocol + HttpContext.Current.Request.Url.Host + urlRedirectTo;

            return urlRedirectTo;
        }

        /// <summary>
        /// get the url to rewrite to
        /// </summary>
        /// <param name="urlRewriteFrom"></param>
        /// <returns></returns>
        public string GetUrlRewriteRelative(Uri urlRewriteFrom, out bool requiresSsl)
        {
            requiresSsl = false;
            if (urlRewriteFrom == null)
            {
                return null;
            }
            if(String.IsNullOrEmpty(urlRewriteFrom.LocalPath))
            {
                return null;
            }
            if(urlRewriteFrom.LocalPath.Length < 2)
            {
                return null;
            }

            string localPath = urlRewriteFrom.LocalPath;
            string urlRewriteFromFirstPart = localPath;
            if (localPath.StartsWith("/"))
            {
                string tmpUrlRewriteFromFirstPart = localPath.Remove(0, 1);
                if (tmpUrlRewriteFromFirstPart.Contains("/"))
                {
                    urlRewriteFromFirstPart = tmpUrlRewriteFromFirstPart.Substring(0, tmpUrlRewriteFromFirstPart.IndexOf('/'));
                    urlRewriteFromFirstPart = "/" + urlRewriteFromFirstPart + "/";
                }
            }
            
            UrlDataAccess uda = new UrlDataAccess();

            //get all rules from cache or db
            List<Url> allUrlRules = uda.GetUrls();

            //figure out which rules apply to the current base url
            List<Url> urlRules = new List<Url>();
            foreach (Url urlRule in allUrlRules)
            {
                if (urlRule.RewriteFrom.StartsWith(urlRewriteFromFirstPart, true, null))
                {
                    urlRules.Add(urlRule);
                }
            }

            //match all rules that apply to the current base url to the current base url + querysting
            string urlRewriteTo = null;
            
            foreach (Url urlRule in urlRules)
            {
                Regex regex = new Regex("^" + urlRule.RewriteFrom + "$", RegexOptions.IgnoreCase);
                if (regex.IsMatch(urlRewriteFrom.PathAndQuery))
                {
                    urlRewriteTo = regex.Replace(urlRewriteFrom.PathAndQuery, urlRule.RewriteTo);
                    requiresSsl = urlRule.RequireSsl;
                    break;
                }
            }
            
            return urlRewriteTo;
        }

        public string GetSeUrl(JobPost jobPost)
        {
            string result = jobPost.Title;
            result = result.ToLower();
            result = Regex.Replace(result, "[^\\w-]", "-", RegexOptions.IgnoreCase);

            while (result.Contains("--"))
            {
                result = result.Replace("--", "-");
            }

            result = result.Trim(new char[] { '-' });
            if (result.Length > 256)
            {
                result = result.Substring(0, 256);
            }

            return result;
        }

        public string GetWwwUrlRedirectAbsolute(PageLink page, Dictionary<string, string> queryStringParameters)
        {
            string urlRedirectFrom = "/" + page.ToString() + ".aspx";
            return GetWwwUrlRedirectAbsolute(urlRedirectFrom, queryStringParameters);
        }

        public string GetWwwUrlRedirectAbsolute(string page, Dictionary<string, string> queryStringParameters)
        {
            string domain = WebConfigurationManager.AppSettings["WWW_DOMAIN"].ToString();

            string absoluteUrl = String.Format("http://{0}{1}", domain, page);

            return absoluteUrl;
        }

        public string GetSeDescription(string seJobText)
        {
            int length = 200;
            string result = "";

            HtmlParser htmlParserNoHtml = new HtmlParser(HtmlParser.ParseRules.FilterOutHtml);
            result = htmlParserNoHtml.FilterHtml(seJobText.Trim());
            result = result.Replace("\"", "");
            if (result.Length > length)
            {
                result = result.Substring(0, length - 3);
                if (result.LastIndexOf(' ') > length - 25)
                {
                    result = result.Substring(0, result.LastIndexOf(' '));
                }
                result = result + "...";
            }

            return result;
        }
    }
}
