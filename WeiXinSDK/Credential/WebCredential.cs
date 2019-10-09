﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WeiXinSDK.Credential
{
    public class WebCredential
    {
        /// <summary>
        /// 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 过期秒数
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 记录添加时间
        /// </summary>
        public DateTime add_time { get; set; }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string scope { get; set; }

        public ReturnCode error { get; set; }

        static string TokenUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
        //static string RefreshTokenUrl = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}";

        internal static WebCredential GetCredential(string code)
        {
            WebCredential cred = null;

            var json = Util.HttpGet2(string.Format(TokenUrl, WeiXin.appConfig.APP_ID, WeiXin.appConfig.APP_SECRET, code));
            if (json.IndexOf("errcode") >= 0)
            {
                cred = new WebCredential();
                cred.error = Util.JsonTo<ReturnCode>(json);
            }
            else
            {
                cred = Util.JsonTo<WebCredential>(json);
                cred.add_time = DateTime.Now;
            }
            return cred;
        }
    }
}
