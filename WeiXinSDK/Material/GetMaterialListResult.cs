using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Material
{
    public class GetMaterialListResult
    {
        /// <summary>
        /// 该类型的素材的总数 
        /// </summary>
        public int total_count { get; set; }

        /// <summary>
        /// 本次调用获取的素材的数量 
        /// </summary>
        public int item_count { get; set; }
        public List<MaterialItemList> item { get; set; }
        public ReturnCode error { get; set; }
    }
    public class MaterialItemList
    {
        /// <summary>
        /// 媒体文件上传后，获取时的唯一标识
        /// </summary>
        public string media_id { get; set; }

        /// <summary>
        /// 文件名称 
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 下载路径 
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 这篇图文消息素材的最后更新时间 
        /// </summary>
        public int update_time { get; set; }
    }
}