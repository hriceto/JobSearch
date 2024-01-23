using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Runtime.InteropServices;

namespace HristoEvtimov.Websites.Work.WorkLibrary.Security
{
    public static class StringSecurity
    {
        public static string ConvertToUnsecureString(this SecureString secureString)
        {
            if (secureString == null)
                throw new ArgumentNullException("security error");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static SecureString ConvertToSecureString(this string unsecureString)
        {
            if (unsecureString == null)
                throw new ArgumentNullException("security error");

            SecureString securePassword = new SecureString();
            foreach (char c in unsecureString)
            {
                securePassword.AppendChar(c);
            }
            securePassword.MakeReadOnly();
            return securePassword;
        }
    }
}
