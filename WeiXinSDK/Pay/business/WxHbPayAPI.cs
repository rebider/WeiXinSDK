using System;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;

namespace WeiXinSDK.WxPayAPI
{
    /// <summary>
    /// 微信现金红包接口
    /// http://pay.weixin.qq.com/wiki/doc/api/cash_coupon.php?chapter=13_6
    /// </summary>
    public class WxHbPayAPI
    {
        /// <summary>
        /// 获取微信现金红包信息接口
        /// 是否需要证书:需要。 
        /// http://pay.weixin.qq.com/wiki/doc/api/cash_coupon.php?chapter=13_6
        /// 使用说明 
        /// 用于商户对已发放的红包进行查询红包的具体信息，可支持普通红包和裂变包。
        /// </summary>
        /// <param name="nonce_str">(必填) String(32) 随机字符串,不长于32位</param>
        /// <param name="mch_billno">(必填) String(28) 商户发放红包的商户订单号</param>
        /// <param name="bill_type">(必填) String(32) 订单类型  例子：MCHT ，通过商户订单号获取红包信息。</param>
        /// <returns>返回xml字符串，格式参见：http://pay.weixin.qq.com/wiki/doc/api/cash_coupon.php?chapter=13_6 </returns>
        public static string GetHbInfo(string nonce_str, string mch_billno, string bill_type)
        {
            try
            {
                WxPayData inputObj = new WxPayData();
                inputObj.SetValue("nonce_str", nonce_str);
                inputObj.SetValue("mch_billno", mch_billno);
                inputObj.SetValue("mch_id", WeiXin.appConfig.MCHID);
                inputObj.SetValue("appid", WeiXin.appConfig.APP_ID);
                inputObj.SetValue("bill_type", bill_type);
                inputObj.SetValue("sign", inputObj.MakeSign());//签名
                string postdata = inputObj.ToXml();

                var url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/gethbinfo";

                // 要注意的这是这个编码方式，还有内容的Xml内容的编码方式
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                byte[] data = encoding.GetBytes(postdata);

                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

                //X509Certificate cer = new X509Certificate(cert_path, cert_password);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                X509Certificate cer = new X509Certificate(WeiXin.appConfig.SSLCERT_PATH, WeiXin.appConfig.SSLCERT_PASSWORD);

                #region 该部分是关键，若没有该部分则在IIS下会报 CA证书出错
                X509Certificate2 certificate = new X509Certificate2(WeiXin.appConfig.SSLCERT_PATH, WeiXin.appConfig.SSLCERT_PASSWORD);
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                store.Remove(certificate);   //可省略
                store.Add(certificate);
                store.Close();

                #endregion

                HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
                webrequest.ClientCertificates.Add(cer);
                webrequest.Method = "post";
                webrequest.ContentLength = data.Length;


                Stream outstream = webrequest.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Flush();
                outstream.Close();


                HttpWebResponse webreponse = (HttpWebResponse)webrequest.GetResponse();
                Stream instream = webreponse.GetResponseStream();
                string resp = string.Empty;
                using (StreamReader reader = new StreamReader(instream))
                {
                    resp = reader.ReadToEnd();
                }
                return resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// http://pay.weixin.qq.com/wiki/doc/api/cash_coupon.php?chapter=13_5
        /// 1.应用场景:向指定微信用户的openid发放指定金额红包
        ///是否需要证书:需要。
        ///<param name="nonce_str">(必填) String(32) 随机字符串,不长于32位</param>
        ///<param name="mch_billno">(必填) String(32) 商户订单号（每个订单号必须唯一）组成： mch_id+yyyymmdd+10位一天内不能重复的数字。</param>
        /// <param name="send_name">(必填) String(32) 红包发送者名称</param>
        /// <param name="re_openid">(必填) String(32) 接受收红包的用户 用户在wxappid下的openid</param>
        /// <param name="total_amount">(必填) int 付款金额，单位分</param>
        /// <param name="total_num">(必填) int 红包发放总人数</param>
        /// <param name="wishing">(必填) String(128) 红包祝福语</param>
        /// <param name="act_name">(必填) String(32) 活动名称</param>
        /// <param name="remark">(必填) String(256) 备注信息</param>
        /// <returns>返回xml字符串，格式参见：http://pay.weixin.qq.com/wiki/doc/api/cash_coupon.php?chapter=13_5 </returns>
        public static string SendRedPack(string nonce_str, string mch_billno,string send_name, string re_openid, int total_amount,
                                        int total_num, string wishing,string act_name, string remark)
        {
            try
            {
                WxPayData inputObj = new WxPayData();
                inputObj.SetValue("nonce_str", nonce_str);
                inputObj.SetValue("mch_billno", mch_billno);
                inputObj.SetValue("mch_id", WeiXin.appConfig.MCHID);
                //inputObj.SetValue("sub_mch_id", sub_mch_id);
                inputObj.SetValue("wxappid", WeiXin.appConfig.APP_ID);
                //inputObj.SetValue("nick_name", nick_name);
                inputObj.SetValue("send_name", send_name);
                inputObj.SetValue("re_openid", re_openid);
                inputObj.SetValue("total_amount", total_amount.ToString());
                //inputObj.SetValue("min_value", min_value.ToString());
                //inputObj.SetValue("max_value", max_value.ToString());
                inputObj.SetValue("total_num", total_num.ToString());
                inputObj.SetValue("wishing", wishing.ToString());
                //inputObj.SetValue("client_ip", WeiXin.appConfig.IP);
                inputObj.SetValue("act_name", act_name);
                inputObj.SetValue("remark", remark);
                //inputObj.SetValue("logo_imgurl", logo_imgurl);
                //inputObj.SetValue("share_content", share_content);
                //inputObj.SetValue("share_url", share_url);
                //inputObj.SetValue("share_imgurl", share_imgurl);
                inputObj.SetValue("sign", inputObj.MakeSign());//签名
                string postdata = inputObj.ToXml();

                var url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";

                // 要注意的这是这个编码方式，还有内容的Xml内容的编码方式
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                byte[] data = encoding.GetBytes(postdata);

                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //X509Certificate cer = new X509Certificate(cert_path, cert_password);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                X509Certificate cer = new X509Certificate(WeiXin.appConfig.SSLCERT_PATH, WeiXin.appConfig.SSLCERT_PASSWORD);

                #region 该部分是关键，若没有该部分则在IIS下会报 CA证书出错
                X509Certificate2 certificate = new X509Certificate2(WeiXin.appConfig.SSLCERT_PATH, WeiXin.appConfig.SSLCERT_PASSWORD);
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadWrite);
                store.Remove(certificate);   //可省略
                store.Add(certificate);
                store.Close();

                #endregion

                HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
                webrequest.ClientCertificates.Add(cer);
                webrequest.Method = "post";
                webrequest.ContentLength = data.Length;


                Stream outstream = webrequest.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Flush();
                outstream.Close();

                HttpWebResponse webreponse = (HttpWebResponse)webrequest.GetResponse();
                Stream instream = webreponse.GetResponseStream();
                string resp = string.Empty;
                using (StreamReader reader = new StreamReader(instream))
                {
                    resp = reader.ReadToEnd();
                }
                return resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
    }
}
