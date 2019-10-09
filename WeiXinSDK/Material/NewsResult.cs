using System;
using System.Collections.Generic;
using System.Text;

namespace WeiXinSDK.Material
{
    public class NewsResult
    {
        /// <summary>
        /// 图文消息，一个图文消息支持1到10条图文
        /// </summary>
        public List<ArticleResult> news_item { get; set; }

        public ReturnCode error { get; set; }
    }
}
