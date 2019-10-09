using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Custom
{
    /// <summary>
    /// 客服账号
    /// </summary>
    public class CustomAccount
    {
        /// <summary>
        /// 完整客服账号，格式为：账号前缀@公众号微信号，账号前缀最多10个字符，
        ///     必须是英文或者数字字符。如果没有公众号微信号，请前往微信公众平台设置。
        /// </summary>
        public string kf_account { get; set; }
        /// <summary>
        /// 客服昵称，最长6个汉字或12个英文字符 
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 客服账号登录密码，格式为密码明文的32位加密MD5值
        /// </summary>
        public string password { get; set; }
    }
}
