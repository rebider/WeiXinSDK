using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Semantic
{
    /// <summary>
    /// 地点描述协议
    /// </summary>
    public class NumberResult
    {
        /// <summary>
        /// 大类型：“NUMBER” NUMBER 又细分为如下类别：
        ///  NUM_PRICE：价格相关，例：200元左右 
        ///  NUM_RADIUS：距离相关，例：200 米以内 
        ///  NUM_DISCOUNT：折扣相关，例：八折 
        ///  NUM_SEASON：部，季相关，例：第一部 
        ///  NUM_EPI：集相关，例：第一集
        ///  NUM_CHAPTER：章节相关，例：第一章 
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 开始， 如果为“-1”表示无上限或者下限，如果为“-2”，表示该字段无信息
        /// </summary>
        public string begin { get; set; }

        /// <summary>
        /// 结束， 如果为“-1”表示无上限或者下限，如果为“-2”，表示该字段无信息
        /// </summary>
        public string end { get; set; }
    }

    public enum NumberType
    {
        NUMBER,//大类型
        NUM_PRICE,//价格相关
        NUM_PADIUS,//距离相关
        NUM_DISCOUNT,//折扣相关
        NUM_SEASON,//：部，季相关
        NUM_EPI,//集相关 
        NUM_CHAPTER//章节相关
    }
}
