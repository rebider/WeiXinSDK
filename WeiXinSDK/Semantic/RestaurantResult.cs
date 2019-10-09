using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Semantic
{
    /// <summary>
    /// 旅店
    /// </summary>
    public class RestaurantResult
    {
        public RestaurantDetail details { get; set; }

        public string intent { get; set; }
    }

    public class RestaurantDetail
    {
        /// <summary>
        /// 地点
        /// </summary>
        public LocationResult location { get; set; }

        /// <summary>
        /// 餐馆名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 餐馆类型/菜系
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// 菜名
        /// </summary>
        public string special { get; set; }

        /// <summary>
        /// 价格（单位元） 
        /// </summary>
        public NumberResult price { get; set; }

        /// <summary>
        /// 距离（单位米） 
        /// </summary>
        public NumberResult radius { get; set; }

        /// <summary>
        /// Int 优惠信息： 0 无（默认）， 1优惠券，2 团购
        /// </summary>
        public int coupon { get; set; }

        /// <summary>
        /// 排序类型：0距离（默认），1点评高优先级，2服务质量高优先级，
        ///           3环境高优先级，4价格高到低，5价格低到高
        /// </summary>
        public int sort { get; set; }
    }
}
