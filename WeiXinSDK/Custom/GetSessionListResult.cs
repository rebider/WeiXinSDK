using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Custom
{
    /// <summary>
    /// 客服的会话列表
    /// </summary>
    public class GetSessionListResult
    {
        /// <summary>
        /// 会话列表
        /// </summary>
        public List<SessionItem> sessionlist { get; set; }
        public ReturnCode error { get; set; }
    }

    public class SessionItem
    {
        /// <summary>
        /// 会话创建时间，UNIX时间戳 
        /// </summary>
        public long createtime { get; set; }

        /// <summary>
        /// 客户openid 
        /// </summary>
        public string openid { get; set; }

    }
}
