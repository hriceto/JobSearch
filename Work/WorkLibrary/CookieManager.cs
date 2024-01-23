using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HristoEvtimov.Websites.Work.WorkLibrary
{
    public class CookieManager
    {
        public void SetNavigationCookie(UserManager.UserRoles roleName)
        {
            int navigationIndex = 0;
            string [] roles = Enum.GetNames(typeof(UserManager.UserRoles));
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == roleName.ToString())
                {
                    navigationIndex = i;
                }
            }

            CookieManagerLogic<int> cookieManagerLogic = new CookieManagerLogic<int>();
            cookieManagerLogic.SetCookie(CookieManagerLogic<int>.CookieNames.Navigation, navigationIndex);
        }

        /// <summary>
        /// Gets the user's navigation role. This is the user's role for navigation display purposes only.
        /// </summary>
        /// <returns></returns>
        public UserManager.UserRoles GetNavigationCookie()
        {
            UserManager.UserRoles roleName = UserManager.UserRoles.None;
            CookieManagerLogic<string> cookieManagerLogic = new CookieManagerLogic<string>();
            string navigationIndex = cookieManagerLogic.GetCookie(CookieManagerLogic<string>.CookieNames.Navigation);

            if(!Enum.TryParse<UserManager.UserRoles>(navigationIndex, out roleName))
            {
                roleName = UserManager.UserRoles.Unknown;
            }
            return roleName;
        }

        public void SetLoggedInCookie(bool loggedIn)
        {
            CookieManagerLogic<bool> cookieManagerLogic = new CookieManagerLogic<bool>();
            cookieManagerLogic.SetCookie(CookieManagerLogic<bool>.CookieNames.In, loggedIn);
        }

        public bool GetLoggedInCookie()
        {
            CookieManagerLogic<bool> cookieManagerLogic = new CookieManagerLogic<bool>();
            return cookieManagerLogic.GetCookie(CookieManagerLogic<bool>.CookieNames.In);
        }

        public void ExpireLoggedInCookie()
        {
            CookieManagerLogic<bool> cookieManagerLogic = new CookieManagerLogic<bool>();
            cookieManagerLogic.ExpireCookie(CookieManagerLogic<bool>.CookieNames.In);
        }

        public void SetShoppingCartCountCookie(int count)
        {
            CookieManagerLogic<int> cookieManagerLogic = new CookieManagerLogic<int>();
            cookieManagerLogic.SetCookie(CookieManagerLogic<int>.CookieNames.Cart, count);
        }

        public int GetShoppingCartCountCookie()
        {
            int result = -1;
            CookieManagerLogic<string> cookieManagerLogic = new CookieManagerLogic<string>();
            string val = cookieManagerLogic.GetCookie(CookieManagerLogic<string>.CookieNames.Cart);
            if(!Int32.TryParse(val, out result))
            {
                result = -1;
            }
            return result;
        }

        protected class CookieManagerLogic<T>
        {
            /// <summary>
            /// Navigation -- Gets the user's navigation role. This is the user's role for navigation display purposes only.
            /// In -- Whether the user is logged in or not. For displaying the logout link when not under hhtps.
            /// Cart -- number of items in cart for user
            /// </summary>
            public enum CookieNames { Navigation, In, Cart };
            private Dictionary<string, int> CookieDurations = new Dictionary<string, int>() { 
            {CookieNames.Navigation.ToString(), 365}, 
            {CookieNames.In.ToString(), -1 },
            {CookieNames.Cart.ToString(), 30 }
        };


            /// <summary>
            /// set a new cookie or update the value in an existing cookie
            /// </summary>
            /// <param name="cookieName"></param>
            /// <param name="value"></param>
            public void SetCookie(CookieNames cookieName, T value)
            {
                int DurationDays = -1;
                if (CookieDurations.ContainsKey(cookieName.ToString()))
                {
                    DurationDays = CookieDurations[cookieName.ToString()];
                }

                if (HttpContext.Current.Response.Cookies[cookieName.ToString()] != null)
                {
                    HttpCookie cookie = HttpContext.Current.Response.Cookies[cookieName.ToString()];
                    if (cookie.Values["val1"] != null)
                    {
                        cookie.Values["val1"] = value.ToString();
                    }
                    else
                    {
                        cookie.Values.Add("val1", value.ToString());
                    }
                    cookie.HttpOnly = true;
                    if (DurationDays > 0)
                    {
                        cookie.Expires = DateTime.Now.AddDays(DurationDays);
                    }
                }
                else
                {
                    HttpCookie cookie = new HttpCookie(cookieName.ToString());
                    cookie.Values.Add("val1", value.ToString());
                    cookie.HttpOnly = true;
                    if (DurationDays > 0)
                    {
                        cookie.Expires = DateTime.Now.AddDays(DurationDays);
                    }
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }

            /// <summary>
            /// get the value stored in the cookie
            /// </summary>
            /// <returns></returns>
            public T GetCookie(CookieNames cookieName)
            {
                T result = default(T);
                if (HttpContext.Current.Request.Cookies[cookieName.ToString()] != null)
                {
                    if (HttpContext.Current.Request.Cookies[cookieName.ToString()].Values["val1"] != null)
                    {
                        string val1 = HttpContext.Current.Request.Cookies[cookieName.ToString()].Values["val1"];
                        result = (T)Convert.ChangeType(val1, typeof(T));
                    }
                }
                return result;
            }

            public void ExpireCookie(CookieNames cookieName)
            {
                if (HttpContext.Current.Request.Cookies[cookieName.ToString()] != null)
                {
                    HttpContext.Current.Response.Cookies[cookieName.ToString()].Expires = DateTime.Now.AddDays(-1);
                }
            }
        }
    }
}
