using System;
using System.Collections.Generic;
using System.Web;

namespace WeiXinSDK.WxPayAPI
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}