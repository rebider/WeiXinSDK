using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Custom
{
    /// <summary>
    /// 在线客服接待信息列表
    /// </summary>
    public class GetOnlineCustomListResult
    {
        /// <summary>
        /// 在线客服接待信息
        /// </summary>
        public List<CustomOnlineInfo> kf_online_list { get; set; }
        public ReturnCode error { get; set; }
    }
    /// <summary>
    ///  在线客服接待信息
    /// </summary>
    public class CustomOnlineInfo
    {
        /// <summary>
        /// 完整客服账号，格式为：账号前缀@公众号微信号
        /// </summary>
        public string kf_account { get; set; }
        /// <summary>
        /// 客服工号
        /// </summary>
        public string kf_id { get; set; }
        /// <summary>
        /// 客服在线状态 1：pc在线，2：手机在线。若pc和手机同时在线则为 1+2=3 
        /// </summary>
        public short status { get; set; }
        /// <summary>
        /// 客服设置的最大自动接入数 
        /// </summary>
        public string auto_accept { get; set; }
        /// <summary>
        /// 客服当前正在接待的会话数 
        /// </summary>
        public string accepted_case { get; set; }
    }
}
