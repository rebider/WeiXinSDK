using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Custom
{
    /// <summary>
    /// 客服列表
    /// </summary>
    public class GetCustomListResult
    {
        /// <summary>
        /// 客服列表
        /// </summary>
        public List<CustomInfo> kf_list { get; set; }
        public ReturnCode error { get; set; }
    }
    /// <summary>
    ///  客服信息
    /// </summary>
    public class CustomInfo
    {
        /// <summary>
        /// 完整客服账号，格式为：账号前缀@公众号微信号
        /// </summary>
        public string kf_account { get; set; }
        /// <summary>
        /// 客服昵称
        /// </summary>
        public string kf_nick { get; set; }
        /// <summary>
        /// 客服工号
        /// </summary>
        public string kf_id { get; set; }
        /// <summary>
        /// 客服头像
        /// </summary>
        public string kf_headimgurl { get; set; }
    }
}
