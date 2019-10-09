using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Config
{
    public class AppConfig
    {
        #region 基本配置
        /// <summary>
        /// APPID：公众号appid
        /// </summary>
        public string APP_ID;

        /// <summary>
        /// APPSECRET：公众帐号secert
        /// </summary>
        public string APP_SECRET;

        /// <summary>
        /// APPTOKEN：公众帐号token
        /// </summary>
        public string APP_TOKEN;

        /// <summary>
        /// 网站url，如http://wx.xxxx.com
        /// </summary>
        public string WEB_URL { get; set; }
        /// <summary>
        /// 加密秘钥
        /// </summary>
        public string ENCODING_AES_KEY { get; set; }
        #endregion

        #region 高级配置（微信支付、红包）
        /// <summary>
        /// MCHID：商户号（必须配置）
        /// </summary>
        public string MCHID;

        /// <summary>
        /// KEY：商户支付密钥，参考开户邮件设置（必须配置）
        /// </summary>
        public string KEY;

        /// <summary>
        /// =======【证书路径设置】==============================
        /// 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        /// </summary>
        public string SSLCERT_PATH;

        /// <summary>
        /// =======【证书密码】===
        /// </summary>
        public string SSLCERT_PASSWORD;

        /// <summary>
        /// =======【支付结果通知url】===========================
        /// 支付结果通知回调url，用于商户接收支付结果
        /// </summary>
        public string NOTIFY_URL;

        private string _ip = "8.8.8.8";
        /// <summary>
        /// 商户系统后台机器IP
        /// 此参数可手动配置也可在程序中自动获取
        /// </summary>
        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }

        private string _proxy_url = "http://0.0.0.0";
        /// <summary>
        /// =======【代理服务器设置】============================
        /// 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        /// </summary>
        public string PROXY_URL
        {
            get { return _proxy_url; }
            set { _proxy_url = value; }
        }

        /// <summary>
        /// =======【上报信息配置】===============================
        /// 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        /// </summary>
        public int REPORT_LEVENL { get; set; }

        /// <summary>
        /// =======【日志级别】===================================
        ///  日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        /// </summary>
        public int LOG_LEVENL { get; set; }
        #endregion
    }
}
