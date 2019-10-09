using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Semantic
{
    /// <summary>
    /// 天气
    /// </summary>
    public class WeatherResult
    {
        public WeatherDetail details { get; set; }

        public string intent { get; set; }
    }

    public class WeatherDetail
    {
        /// <summary>
        /// 地点
        /// </summary>
        public LocationResult location { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public IntervalDtResult datetime { get; set; }
    }
}
