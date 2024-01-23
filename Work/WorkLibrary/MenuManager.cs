using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using HristoEvtimov.Websites.Work.WorkDal;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class MenuManager
    {
        public const string _PATH_TO_NAVIGATION_CONTROLS = "~/controls/Menu{{RoleName}}.ascx";
        
        public void RegenerateMenuControls()
        {
            MenuDataAccess mda = new MenuDataAccess();
            UrlManager urlManager = new UrlManager();
            List<MenuItem> menuItems = mda.GetMenuDataAll();

            Dictionary<string, List<MenuItem>> menuItemsDictionary = new Dictionary<string, List<MenuItem>>();
            foreach (MenuItem menuItem in menuItems)
            {
                if (menuItemsDictionary.ContainsKey(menuItem.MenuType.UserRole))
                {
                    menuItemsDictionary[menuItem.MenuType.UserRole].Add(menuItem);
                }
                else
                {
                    menuItemsDictionary.Add(menuItem.MenuType.UserRole, new List<MenuItem>() { menuItem });
                }
            }

            foreach (KeyValuePair<string, List<MenuItem>> menuItemsKeyValue in menuItemsDictionary)
            {
                using (StreamWriter sr = new StreamWriter(HttpContext.Current.Server.MapPath(GetMenuPath(menuItemsKeyValue.Key))))
                {
                    sr.WriteLine("<ul class=\"nav navbar-nav\">");
                    Dictionary<String, List<MenuItem>> rootMenuNodes = new Dictionary<string, List<MenuItem>>();
                    foreach (MenuItem menuItem in menuItemsKeyValue.Value)
                    {
                        if (rootMenuNodes.ContainsKey(menuItem.ParentMenuText))
                        {
                            rootMenuNodes[menuItem.ParentMenuText].Add(menuItem);
                        }
                        else
                        {
                            rootMenuNodes.Add(menuItem.ParentMenuText, new List<MenuItem>() { menuItem });
                        }
                    }

                    foreach (KeyValuePair<String, List<MenuItem>> rootMenuNode in rootMenuNodes)
                    {
                        if (rootMenuNode.Value.Count > 0)
                        {
                            sr.WriteLine("<li class=\"dropdown\">");
                            sr.WriteLine("<a class=\"dropdown-toggle\" data-toggle=\"dropdown\" href=\"javascript:void(0);\">{0}<b class=\"caret\"></b></a>", rootMenuNode.Key);
                        }
                        else
                        {
                            sr.WriteLine("<li>");
                            sr.WriteLine("<a href=\"javascript:void(0);\">{0}</a>", rootMenuNode.Key);
                        }
                                                
                        if (rootMenuNode.Value.Count > 0)
                        {
                            sr.WriteLine("<ul class=\"dropdown-menu\">");
                            foreach (MenuItem menuItem in rootMenuNode.Value)
                            {
                                sr.WriteLine("<li>");
                                string openInNewTab = "";
                                if (menuItem.OpenInNewTab)
                                {
                                    openInNewTab = " target=\"_blank\"";
                                }
                                sr.WriteLine("<a href=\"{0}\"{2}>{1}</a>", urlManager.GetUrlRedirectAbsolute(menuItem.Url.RedirectTo, null), menuItem.MenuText, openInNewTab);
                                sr.WriteLine("</li>");
                            }
                            sr.WriteLine("</ul>");
                        }
                        sr.WriteLine("</li>");
                    }
                    sr.WriteLine("</ul>");
                    sr.Close();
                }
            }
        }

        private string GetMenuPath(string roleName)
        {
            return _PATH_TO_NAVIGATION_CONTROLS.Replace("{{RoleName}}", roleName);
        }

        /// <summary>
        /// check the navigatoin cookie. if that is not present
        /// then look up the role from the database and create the cookie.
        /// </summary>
        /// <returns></returns>
        public string GetMenuPath()
        {
            CookieManager cookieManager = new CookieManager();
            UserManager.UserRoles roleName = cookieManager.GetNavigationCookie();

            if (roleName == UserManager.UserRoles.Unknown)
            {
                UserManager userManager = new UserManager();
                roleName = userManager.GetMembershipUserRole();
                cookieManager.SetNavigationCookie(roleName);
            }
            return _PATH_TO_NAVIGATION_CONTROLS.Replace("{{RoleName}}", roleName.ToString());
        }

        
    }
}
