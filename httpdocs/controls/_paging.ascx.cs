using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    public partial class _paging : System.Web.UI.UserControl
    {
        private int defaultPageSize = 20;
        private int defaultCurrentPage = 1;
        private int defaultTotalNumberOfItems = 0;

        public delegate void PagingEventHandler(object sender, PagingEventArgs e);
        public event PagingEventHandler Paging;
        protected virtual void OnPaging(PagingEventArgs e)
        {
            if (Paging != null)
                Paging(this, e);
        }
        public class PagingEventArgs
        {
            public int Page { get; set; }

            public PagingEventArgs(int page)
            {
                Page = page;
            }
        }
        public int PageSize
        {
            get
            {
                if (ViewState["PageSize_" + this.ClientID] != null)
                {
                    return Int32.Parse(ViewState["PageSize_" + this.ClientID].ToString());
                }
                return defaultPageSize;
            }
            set
            {
                ViewState["PageSize_" + this.ClientID] = value;
            }
        }

        public int CurrentPage
        {
            get
            {
                if (ViewState["CurrentPage_" + this.ClientID] != null)
                {
                    return Int32.Parse(ViewState["CurrentPage_" + this.ClientID].ToString());
                }
                return defaultCurrentPage;
            }
            set
            {
                ViewState["CurrentPage_" + this.ClientID] = value;
            }
        }

        public int TotalNumberOfItems
        {
            get
            {
                if (ViewState["TotalNumberOfItems_" + this.ClientID] != null)
                {
                    return Int32.Parse(ViewState["TotalNumberOfItems_" + this.ClientID].ToString());
                }
                return defaultTotalNumberOfItems;
            }
            set
            {
                ViewState["TotalNumberOfItems_" + this.ClientID] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BuildPaging();
        }

        public void BuildPaging()
        {
            ulNavigation.Controls.Clear();
            int numberOfPages = (int)Math.Ceiling((double)TotalNumberOfItems / (double)PageSize);
            if (numberOfPages > 1)
            {
                for (int i = 1; i <= numberOfPages; i++)
                {
                    HtmlGenericControl liPage = new HtmlGenericControl("li");
                    LinkButton lnkbPage = new LinkButton();
                    lnkbPage.Text = i.ToString();
                    lnkbPage.ID = "lnkbPage" + i.ToString();
                    lnkbPage.Command += new CommandEventHandler(lnkbPage_Command);
                    lnkbPage.CommandArgument = i.ToString();
                    if (i == CurrentPage)
                    {
                        lnkbPage.Enabled = false;
                        liPage.Attributes.Add("class", "active");
                    }
                    liPage.Controls.Add(lnkbPage);
                    ulNavigation.Controls.Add(liPage);
                }
                ulNavigation.Visible = true;
            }
        }

        protected void lnkbPage_Command(object sender, CommandEventArgs e)
        {
            int page = Int32.Parse(e.CommandArgument.ToString());
            OnPaging(new PagingEventArgs(page));
        }
    }
}