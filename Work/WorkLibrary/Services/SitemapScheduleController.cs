using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Web.Configuration;
using System.Xml;
using System.IO;
using HristoEvtimov.Websites.Work.WorkDal;
using HristoEvtimov.Websites.Work.WorkLibrary;
using HristoEvtimov.Websites.Work.WorkLibrary.Services.Authentication;

namespace HristoEvtimov.Websites.Work.WorkSearch.Services
{
    public class SitemapScheduleController : ApiController
    {
        [HttpGet]
        [BasicAuthentication(Users="schedulerunner")]
        public bool CreateSitemap()
        {
            bool result = false;

            try
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(xmlDeclaration);

                XmlNode xmlRoot = doc.CreateNode(XmlNodeType.Element, "urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
                doc.AppendChild(xmlRoot);

                Dictionary<String, Boolean> pages = new Dictionary<String, Boolean>()
                {
                    {"/register-job-seeker", true},
                    {"/job-search", true},
                    {"/register-employer", true},
                    {"/pricing", false},
                    {"/job-detail", false},
                    {"/login", true},
                    {"/reset-password", true},
                    {"/about", false},
                    {"/privacy", false},
                    {"/terms", false},
                    {"/contact-us", true},
                    {"/employers-welcome", true},
                    {"/industry-list", false},
                    {"/resources", false},
                    {"/blog", false},
                };

                foreach (KeyValuePair<string, bool> page in pages)
                {
                    string link = Request.RequestUri.Host + page.Key;
                    link = (page.Value ? "https://" : "http://") + link;

                    string priority = "0.5";
                    if (page.Key == "/job-search")
                    {
                        priority = "1.0";
                    }
                    AddUrlNode(doc, xmlRoot, link, "weekly", priority, null);
                }

                //add jobs
                JobManager jobManager = new JobManager();
                List<JobSearchIndex> jobs = jobManager.GetJobPostsForSearch();
                UrlManager urlManager = new UrlManager();
                foreach (JobSearchIndex job in jobs)
                {
                    string link = JobManager.GetAbsoluteUrl(urlManager, job.Job);
                    AddUrlNode(doc, xmlRoot, link, "weekly", "1.0",
                        (job.Job.StartDate != null) ? job.Job.StartDate.Value.ToString("yyyy-MM-dd") : null);
                }

                //add categories
                CategoryManager categoryManager = new CategoryManager();
                List<Category> categories = categoryManager.GetCategories();
                foreach (Category category in categories)
                {
                    Dictionary<string, string> categoryDetailDict = new Dictionary<string, string>() { 
                        { "categoryid", category.CategoryId.ToString() }, 
                        { "seurl", category.SeUrl.ToString() } 
                    };

                    string link = urlManager.GetUrlRedirectAbsolute(UrlManager.PageLink.Industries, categoryDetailDict);
                    AddUrlNode(doc, xmlRoot, link, "daily", "1.0", null);
                }

                //add blogs
                BlogManager blogManager = new BlogManager();
                int totalNumberOfBlogs = -1;
                List<Blog> blogs = blogManager.GetBlogsForList(-1, 1, 10000, out totalNumberOfBlogs);
                foreach (Blog blog in blogs)
                {
                    string link = BlogManager.GetAbsoluteUrl(urlManager, blog);
                    AddUrlNode(doc, xmlRoot, link, "monthly", "1.0", blog.UpdatedDate.ToString("yyyy-MM-dd"));
                }

                //add blog topics
                TopicManager topicManager = new TopicManager();
                List<Topic> topics = topicManager.GetTopicsActive(false);
                foreach (Topic topic in topics)
                {
                    string link = TopicManager.GetAbsoluteUrl(urlManager, topic);
                    AddUrlNode(doc, xmlRoot, link, "weekly", "1.0", null);
                }

                //save sitemap
                doc.Save(HttpContext.Current.Server.MapPath("/sitemap.xml"));
                result = true;

                //submit to search engines.
                SubmitSitemapToSearchEngines();
            }
            catch (System.Exception ex)
            {
                ExceptionManager exceptionManager = new ExceptionManager();
                exceptionManager.AddException(ex);
                result = false;
            }

            return result;
        }

        private void AddUrlNode(XmlDocument doc, XmlNode xmlRoot, string link, string changeFreq, string priority, string lastmod)
        {
            XmlNode xmlUrl = doc.CreateNode(XmlNodeType.Element, "url", "http://www.sitemaps.org/schemas/sitemap/0.9");
            XmlNode xmlLoc = doc.CreateNode(XmlNodeType.Element, "loc", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xmlLoc.InnerText = link;

            XmlNode xmlChangeFreq = doc.CreateNode(XmlNodeType.Element, "changefreq", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xmlChangeFreq.InnerText = changeFreq;

            XmlNode xmlPriority = doc.CreateNode(XmlNodeType.Element, "priority", "http://www.sitemaps.org/schemas/sitemap/0.9");
            xmlPriority.InnerText = priority;

            xmlUrl.AppendChild(xmlLoc);
            xmlUrl.AppendChild(xmlChangeFreq);
            xmlUrl.AppendChild(xmlPriority);
            if (!String.IsNullOrEmpty(lastmod))
            {
                XmlNode xmlLastMod = doc.CreateNode(XmlNodeType.Element, "lastmod", "http://www.sitemaps.org/schemas/sitemap/0.9");
                xmlLastMod.InnerText = lastmod;
                xmlUrl.AppendChild(xmlLastMod);
            }
            xmlRoot.AppendChild(xmlUrl);
        }

        private void SubmitSitemapToSearchEngines()
        {
            UrlManager urlManager = new UrlManager();
            LogManager logManager = new LogManager();

            string[] searchEngineAddresses = WebConfigurationManager.AppSettings["SEARCH_ENGINE_PING_ADDRESSES"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string searchEngineAddress in searchEngineAddresses)
            {
                string sitemapUrl = urlManager.GetUrlRedirectAbsolute("/", null) + "sitemap.xml";
                string searchEngineUrl = String.Format(searchEngineAddress, HttpContext.Current.Server.UrlEncode(sitemapUrl));

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(searchEngineUrl);
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                string result = reader.ReadToEnd();
                                if (!result.Contains("successfully") && !result.Contains("Thanks"))
                                {
                                    logManager.AddLog("Sitemap submitted", -1, searchEngineUrl, result);
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ExceptionManager exceptionManager = new ExceptionManager();
                    exceptionManager.AddException(ex);
                }
            }
        }
    }
}
