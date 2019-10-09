using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.WebModel
{
    public class WxProduct
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public string product_id { get; set; }
        
        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 商品现价（单位分）
        /// </summary>
        public int price { get; set; }

        /// <summary>
        /// 商品原价(单位为分)
        /// </summary>
        public int ori_price { get; set; }

        /// <summary>
        /// 商品主图
        /// </summary>
        public string main_img { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public short status { get; set; }
    }
}
