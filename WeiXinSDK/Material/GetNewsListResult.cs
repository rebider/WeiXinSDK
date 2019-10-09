using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Material
{
    public class GetNewsListResult
    {
        /// <summary>
        /// 该类型的素材的总数 
        /// </summary>
        public int total_count { get; set; }

        /// <summary>
        /// 本次调用获取的素材的数量 
        /// </summary>
        public int item_count { get; set; }
        public List<NewsItemList> item { get; set; }
        public ReturnCode error { get; set; }
    }
    public class NewsItemList
    {
        /// <summary>
        /// 媒体文件上传后，获取时的唯一标识
        /// </summary>
        public string media_id { get; set; }

        public NewsItem content { get; set; }

        /// <summary>
        /// 这篇图文消息素材的最后更新时间 
        /// </summary>
        public long update_time { get; set; }
    }
    public class NewsItem
    {
        /// <summary>
        /// 图文消息，一个图文消息支持1到10条图文
        /// </summary>
        public List<ArticleResult> news_item { get; set; }
    }
}