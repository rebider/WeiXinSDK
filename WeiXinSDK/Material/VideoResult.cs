using System;
using System.Collections.Generic;
using System.Text;

namespace WeiXinSDK.Material
{
    /// <summary>
    /// 图文消息
    /// </summary>
    public class VideoResult
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 下载地址 
        /// </summary>
        public string down_url { get; set; }
        
        public ReturnCode error { get; set; }
    }
}
