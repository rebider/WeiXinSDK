using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Semantic
{
    /// <summary>
    /// 输出协议
    /// </summary>
    public class SemanticResponse<T> : ReturnCode where T : class
    {
        /// <summary>
        /// 用户的输入字符串
        /// </summary>
        public string query { get; set; }

        /// <summary>
        /// 服务的全局类型id，详见协议文档中垂直服务协议定义
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 语义理解后的结构化标识，各服务不同
        /// </summary>
        public T semantic { get; set; }

        /// <summary>
        /// 部分类别的结果（否）
        /// </summary>
        public List<object> result { get; set; }

        /// <summary>
        /// 部分类别的结果html5展示，目前不支持（否）
        /// </summary>
        public string answer { get; set; }
        /// <summary>
        /// 特殊回复说明（否）
        /// </summary>
        public string text { get; set; }
    }
}
