using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Material
{
    /// <summary>
    ///  永久素材文件
    /// </summary>
    public class MaterialResult
    {
        /// <summary>
        /// 媒体文件上传后，获取时的唯一标识
        /// </summary>
        public string media_id { get; set; }
        /// <summary>
        /// 新增的图片素材的图片URL（仅新增图片素材时会返回该字段）
        /// </summary>
        public string url { get; set; }

        public ReturnCode error { get; set; }
    }
}
