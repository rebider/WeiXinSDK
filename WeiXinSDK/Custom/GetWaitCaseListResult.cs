using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Custom
{
    /// <summary>
    /// 未接入会话列表
    /// </summary>
    public class GetWaitCaseListResult
    {
        /// <summary>
        /// 未接入会话数量
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 未接入会话列表，最多返回100条数据
        /// </summary>
        public List<WaitCase> sessionlist { get; set; }
        public ReturnCode error { get; set; }
    }

    public class WaitCase
    {
        /// <summary>
        /// 用户来访时间，UNIX时间戳 
        /// </summary>
        public long createtime { get; set; }

        /// <summary>
        /// 客户openid 
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 指定接待的客服，为空表示未指定客服 
        /// </summary>
        public string kf_account { get; set; }

    }
}
