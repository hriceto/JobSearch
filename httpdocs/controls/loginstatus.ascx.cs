using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HristoEvtimov.Websites.Work.WorkLibrary;

namespace HristoEvtimov.Websites.Work.Web.Controls
{
    public partial class loginstatus : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lsLogin.LoggedOut += new EventHandler(lsLogin_LoggedOut);

            lbtnLogOut.Visible = false;
            lsLogin.Visible = false;
            
            CookieManager cookieManager = new CookieManager();
            UserManager.UserRoles roleName = cookieManager.GetNavigationCookie();
            
            //display login if hte user has never logged in
            //display my account link if they have logged in before
            if (roleName == UserManager.UserRoles.Unknown || roleName == UserManager.UserRoles.None)
            {
                lsLogin.Visible = true;
            }
            else
            {
                //if user is currently logged in show logout link.
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    lsLogin.Visible = true;
                }
                else
                {
                    if (Request.Url.Scheme.ToLower() == "http")
                    {
                        if (cookieManager.GetLoggedInCookie())
                        {
                            lbtnLogOut.Visible = true;
                        }
                    }
                    else if (Request.Url.Scheme.ToLower() == "https")
                    {
                        cookieManager.ExpireLoggedInCookie();
                    }
                }
            }
        }

        protected void lsLogin_LoggedOut(object sender, EventArgs e)
        {
            CookieManager cookieManager = new CookieManager();
            cookieManager.ExpireLoggedInCookie();
        }

        protected void lbtnLogOut_Click(object sender, EventArgs e)
        {
            UserManager userManager = new UserManager();
            userManager.SignOut();
            
            CookieManager cookieManager = new CookieManager();
            cookieManager.ExpireLoggedInCookie();
        }
    }
}