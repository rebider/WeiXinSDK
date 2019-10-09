using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Semantic
{
    /// <summary>
    /// 地点描述协议
    /// </summary>
    public class LocationResult
    {
        /// <summary>
        /// 大类型：“LOC” 
        /// LOC 又细分为如下类别：LOC_COUNTRY、 LOC_PROVINCE、LOC_CITY、LOC_TOWN、 LOC_POI、NORMAL_POI。
        /// 注：LOC_POI和NORMAL_POI的区别：
        ///     LOC_POI是指地名除了国家、省、市、区县的详细地址；
        ///     NORMAL_POI是指地图上偏向机构的poi点，比如：饭馆、酒店、大厦等等
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// 省全称，例如：广东省 
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 省简称，例如：广东|粤 
        /// </summary>
        public string province_simple { get; set; }

        /// <summary>
        /// 市全称，例如：北京市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 市简称，例如：北京 
        /// </summary>
        public string city_simple { get; set; }

        /// <summary>
        /// 县区全称，例如：海淀区 
        /// </summary>
        public string town { get; set; }

        /// <summary>
        /// 县区简称，例如：海淀 
        /// </summary>
        public string town_simple { get; set; }

        /// <summary>
        /// poi详细地址
        /// </summary>
        public string poi { get; set; }

        /// <summary>
        /// 原始地名串 
        /// </summary>
        public string loc_ori { get; set; }
    }

    public enum LocationType
    {
        LOC,//大类型
        LOC_COUNTRY,//国家
        LOC_PROVINCE,//省
        LOC_CITY,//市
        LOC_TOWN,//县区
        LOC_POI,//详细地址 
        NORMAL_POI//附近机构地址
    }
}
