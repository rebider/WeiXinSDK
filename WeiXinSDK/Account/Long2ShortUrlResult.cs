using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiXinSDK;

namespace WeiXinSDK
{
    /// <summary>
    /// 长链接转短链接返回结果
    /// </summary>
    public class Long2ShortUrlResult : ReturnCode
    {
        /// <summary>
        ///  短链接
        /// </summary>
        public string short_url { get; set; }
    }
}
