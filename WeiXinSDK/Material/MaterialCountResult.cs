﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Material
{
    /// <summary>
    ///  永久素材的总数
    /// </summary>
    public class MaterialCountResult
    {
        /// <summary>
        /// 语音总数量 
        /// </summary>
        public int voice_count { get; set; }
        /// <summary>
        /// 视频总数量 
        /// </summary>
        public int video_count { get; set; }
        /// <summary>
        /// 图片总数量 
        /// </summary>
        public int image_count { get; set; }
        /// <summary>
        /// 图文总数量 
        /// </summary>
        public int news_count { get; set; }

        public ReturnCode error { get; set; }
    }
}
