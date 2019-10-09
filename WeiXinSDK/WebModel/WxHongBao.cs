using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.WebModel
{
    public class WxHongBao
    {
        /// <summary>
        /// 商户发放红包的商户订单号
        /// </summary>
        public string mch_billno { get; set; }

        /// <summary>
        /// 接受收红包的用户在wxappid下的openid
        /// </summary>
        public string re_openid { get; set; }

        /// <summary>
        /// 付款金额，单位分
        /// </summary>
        public int total_amount { get; set; }

        /// <summary>
        /// 红包发送时间
        /// </summary>
        public int send_time { get; set; }

        /// <summary>
        /// 红包状态
        ///     SENDING:发放中 
        ///     SENT:已发放待领取 
        ///     FAILED：发放失败 
        ///     RECEIVED:已领取 
        ///     REFUND:已退款
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string send_name { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string act_name { get; set; }

        /// <summary>
        /// 红包祝福语
        /// </summary>
        public string wishing { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 发送红包响应xml
        /// </summary>
        public string xml { get; set; }
    }
}
