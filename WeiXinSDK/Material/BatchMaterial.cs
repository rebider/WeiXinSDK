using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Material
{
    /// <summary>
    ///  永久素材的列表请求
    /// </summary>
    public class BatchMaterial
    {
        /// <summary>
        /// 素材的类型，图片（image）、视频（video）、语音 （voice）、图文（news）  
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 从全部素材的该偏移位置开始返回，0表示从第一个素材 返回  
        /// </summary>
        public int offset { get; set; }
        /// <summary>
        /// 返回素材的数量，取值在1到20之间  
        /// </summary>
        public int count { get; set; }
    }
}
