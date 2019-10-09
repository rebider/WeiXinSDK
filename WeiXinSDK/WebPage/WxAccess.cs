using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiXinSDK.Message;
using System.Web;
using WeiXinSDK.Tools;

namespace WeiXinSDK.WebPage
{
    public class Access : System.Web.UI.Page
    {
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            if (Request.HttpMethod.ToUpper() == "POST")
            {
                Log.Debug(this.GetType().ToString(), "POST请求开始");
                if (CheckSignature())
                {
                    //处理用户消息和事件
                    ReplyBaseMsg replymsg = WeiXin.ReplyMsg();
                    Log.Debug(this.GetType().ToString(), replymsg.GetXML());
                    Response.Write(replymsg.GetXML());
                    Response.End();
                }
                else
                {
                    //不是从微信服务器发起的Post请求
                    Log.Debug(this.GetType().ToString(), "非法POST请求");
                    Response.Write("非法请求");
                }
            }
            else
            {
                Log.Debug(this.GetType().ToString(), "GET请求开始");
                //只在首次设置微信公众号服务器URL时调用运行
                string echoStr = HttpContext.Current.Request.QueryString["echoStr"];
                if (CheckSignature())
                {
                    if (!string.IsNullOrEmpty(echoStr))
                    {
                        Log.Info(this.GetType().ToString(), "设置微信公众号服务器URL");
                        Response.Write(echoStr);
                    }
                }
                else
                {
                    Log.Debug(this.GetType().ToString(), "非法GET请求");
                    Response.Write("非法请求");
                }
            }
            Response.End();
        }

        /// <summary>
        /// 验证消息的真实性
        /// </summary>
        /// <returns></returns>
        protected bool CheckSignature()
        {
            string token = WeiXin.appConfig.APP_TOKEN;
            string signature = HttpContext.Current.Request.QueryString["signature"];
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = HttpContext.Current.Request.QueryString["nonce"];
            if (WeiXin.CheckSignature(signature, timestamp, nonce, token))
                return true;
            return false;
        }
    }
}
