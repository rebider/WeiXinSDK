using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiXinSDK.Tools
{
    public class HashCode
    {
        #region Funcs
        public static string SHA1(string str)
        {
            string rethash = string.Empty;
            try
            {
                var hash = System.Security.Cryptography.SHA1.Create();
                var encoder = new System.Text.ASCIIEncoding();
                byte[] combined = encoder.GetBytes(str);
                hash.ComputeHash(combined);
                rethash = Convert.ToBase64String(hash.Hash);
            }
            catch (Exception ex)
            {
                string strerr = "Error in HashCode : " + ex.Message;
            }
            return rethash;
        }
        #endregion

    }
}
