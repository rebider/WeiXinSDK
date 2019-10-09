using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WeiXinSDK.WxPayAPI;
using WeiXinSDK.Tools;

namespace WeiXinSDK.WebPage.Pay
{
    /// <summary>
    /// 1)准备js下单参数 PrepareParam
    /// 2)订单ID已经存在 OrderIsExist
    /// 3)下单失败 OrderFail
    /// </summary>
    public class WxJsPay : System.Web.UI.Page
    {
        public string wxJsApiParam { get; set; } //H5调起JS API参数

        /// <summary>
        /// 1)准备js下单参数
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="openid"></param>
        /// <param name="total_fee"></param>
        /// <returns>返回新orderid</returns>
        protected string PrepareParam(string orderid, string openid, int total_fee, string body)
        {
            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay(this);

            jsApiPay.access_token = WeiXinSDK.WeiXin.GetAccessToken();
            jsApiPay.openid = openid;
            jsApiPay.total_fee = total_fee;

            //JSAPI支付预处理
            try
            {
                if (QueryOrder(orderid) == true)
                {
                    //订单已经存在
                    OrderIsExist();
                }
                else
                {
                    WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(body);
                    orderid = jsApiPay.orderid;

                    wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数    
                    Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);

                }
            }
            catch (Exception ex)
            {
                Log.Error(this.GetType().ToString(), ex.ToString());
                OrderFail(ex);
            }
            return orderid;
        }
        
        //查询订单
        protected bool QueryOrder(string out_trade_no)
        {
            WxPayData req = new WxPayData();
            req.SetValue("out_trade_no", out_trade_no);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 2)订单ID已经存在执行的操作
        /// </summary>
        protected virtual void OrderIsExist()
        {
        }

        /// <summary>
        /// 3)下单失败执行的操作
        /// </summary>
        protected virtual void OrderFail(Exception ex)
        {
        }
    }
}
