using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary.HttpModules
{
    public class FriendlyUrlModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(FriendlyUrlModule_BeginRequest);
        }

        public void FriendlyUrlModule_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication context = (HttpApplication)sender;
            Uri url = context.Context.Request.Url;
            UrlManager urlManager = new UrlManager();
                
            if (!url.LocalPath.Contains(".") && url.LocalPath.Length > 1)
            {
                bool requireSsl = false;
                string urlRewriteTo = urlManager.GetUrlRewriteRelative(url, out requireSsl);
                if (!String.IsNullOrEmpty(urlRewriteTo))
                {
                    if (requireSsl && context.Context.Request.Url.Scheme == "http")
                    {
                        context.Context.Response.Redirect(context.Context.Request.Url.AbsoluteUri.Replace("http://", "https://"), true);
                    }
                    context.Context.RewritePath(urlRewriteTo, false);
                    return;
                }
            }

            //maybe try an redirect
            if (url.LocalPath.Contains(".aspx"))
            {
                Dictionary<string, string> queryStrings = new Dictionary<string, string>();
                foreach (string queryKey in context.Request.QueryString.AllKeys)
                {
                    queryStrings.Add(queryKey, context.Request.QueryString[queryKey]);
                }

                string redirectUrlAbsolute = urlManager.GetUrlRedirectAbsolute(url.LocalPath, queryStrings);
                if (redirectUrlAbsolute != url.AbsoluteUri)
                {
                    context.Context.Response.Redirect(redirectUrlAbsolute, true);
                }
            }
        }

        public void Dispose()
        {

        }
    }
}
