using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiXinSDK.WxPayAPI;
using WeiXinSDK.WebModel;

namespace WeiXinSDK.WebPage.Pay
{
    /// <summary>
    ///wxHbPay 微信现金红包
    /// </summary>
    public class WxHbPay
    {
        /// <summary>
        /// 红包查询
        /// </summary>
        /// <param name="model">红包模型</param>
        /// <returns>修改模型的status</returns>
        public static HbResult GetHongBaoInfo(WxHongBao model)
        {
            HbResult hbResult = new HbResult();
            hbResult.model = model;
            //参数
            string nonce_str = DateTime.Now.Ticks.ToString();
            string bill_type = "MCHT";

            string xml = WxHbPayAPI.GetHbInfo(nonce_str, model.mch_billno, bill_type);
            if (string.IsNullOrEmpty(xml))
            {
                //出错
                hbResult.errcode = -1;
                hbResult.errmsg = "服务器返回xml为空";
            }
            else
            {
                Dictionary<string, string> dic = WeiXinSDK.Util.GetDictFromXml(xml);
                if (dic["return_code"] == "SUCCESS" && dic["result_code"] == "SUCCESS")//成功
                {
                    hbResult.errcode = 0;
                    hbResult.model.status = dic["status"];//红包状态
                }
                else
                {
                    //出错
                    hbResult.errcode = -1;
                    hbResult.errmsg = dic["return_msg"];
                }
            }
            return hbResult;
        }

        /// <summary>
        /// 发送现金红包
        /// </summary>
        /// <param name="model">红包模型</param>
        /// <returns>更新模型的xml、send_time</returns>
        public static HbResult SendRedPack(WxHongBao model)
        {
            HbResult hbResult = new HbResult();
            try
            {
                string re_openid = model.re_openid;
                string mch_billno = model.mch_billno;
                string nick_name = model.send_name;
                string send_name = model.send_name;
                int total_amount = model.total_amount;
                int min_value = model.total_amount;
                int max_value = model.total_amount;
                string wishing = model.wishing;
                string act_name = model.act_name;
                string remark = model.remark;
                int total_num = 1;

                string nonce_str = DateTime.Now.Ticks.ToString();
                //string sub_mch_id = "";
                //string logo_imgurl = "";
                //string share_content = "";
                //string share_url = "";
                //string share_imgurl = "";

                string xml = WxHbPayAPI.SendRedPack(nonce_str,mch_billno,send_name,re_openid,total_amount,total_num,wishing,act_name,remark);
                model.xml = xml;
                if (!string.IsNullOrEmpty(xml))
                {
                    Dictionary<string, string> dic = WeiXinSDK.Util.GetDictFromXml(xml);
                    if (dic["return_code"] == "SUCCESS" && dic["result_code"] == "SUCCESS")//成功
                    {
                        string r1 = dic["mch_billno"];//商户订单号
                        string r2 = dic["re_openid"];//用户openid
                        string r3 = dic["total_amount"];//付款金额
                        string r4 = dic["send_time"];//发放成功时间
                        string r5 = dic["send_listid"];//红包订单的微信单号

                        int send_time = 0;
                        if (int.TryParse(r4, out send_time))
                        {
                            model.send_time = send_time;
                        }
                        hbResult.errcode = 0;
                        hbResult.model = model;
                    }
                    else
                    {
                        hbResult.errcode = -1;
                        hbResult.errmsg = dic["return_msg"];
                    }
                }
                else
                {
                    hbResult.errcode = -1;
                    hbResult.errmsg = "服务器返回xml为空";
                }
            }
            catch (Exception ex)
            {
                hbResult.errcode = -1;
                hbResult.errmsg = ex.ToString();
            }
            return hbResult;
        }

        public class HbResult : ReturnCode
        {
            public WxHongBao model { get; set; }
        }

        //生成28位订单（mch_id+yyyymmdd+10位一天内不能重复的数字。）
        public static string CreateMchBillNo()
        {
            string str1 = WeiXin.appConfig.MCHID + DateTime.Now.ToString("yyyyMMddhhmmss");//十四位
            string str2 = new Random().Next(1000, 9999).ToString();
            string mch_billno = str1 + str2;
            return mch_billno;
        }
    }
}