using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WeiXinSDK;
using WeiXinSDK.Tools;
using WeiXinSDK.User;

namespace WeiXinSDK.WebPage
{
    public class OAuth : System.Web.UI.Page
    {
        /// <summary>
        /// 授权成功之后默认跳转页面（默认：hycenter.aspx）
        /// </summary>
        protected string jumpPage = "hycenter.aspx";

        /// <summary>
        /// 应用授权作用域（默认：snsapi_base）
        ///     snsapi_base、snsapi_userinfo 
        /// </summary>
        protected string scope = "snsapi_base";

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            string url = Request.QueryString["url"];
            if (string.IsNullOrEmpty(url))
            {
                url = "http://" + HttpContext.Current.Request.Url.Host + "/" + jumpPage;
            }
            //授权后重定向的回调链接地址
            string redirect_uri = Server.UrlEncode("http://" + HttpContext.Current.Request.Url.Host + "/OAuth.aspx?url=" + Server.UrlEncode(url));

            string code = Request.QueryString["code"];
            //重定向后会带上state参数，开发者可以填写a-zA-Z0-9的参数值（默认：base）
            string state = Request.QueryString["state"];

            if (string.IsNullOrEmpty(code))
            {
                string codeurl = WeiXinSDK.WeiXin.BuildWebCodeUrl(redirect_uri, scope, "1");
                
                //第一步：引导用户打开如下页面授权
                Log.Debug(this.GetType().ToString(), "跳转到授权页面：" + codeurl);
                Response.Redirect(codeurl);
                return;
            }
            else
            {
                //第二步：通过code换取网页授权access_token
                Credential.WebCredential wc = WeiXinSDK.WeiXin.GetWebAccessToken(code);

                Log.Debug(this.GetType().ToString(), "网页授权access_token：" + wc.access_token);


                //判断access_token是否有效
                if (wc.error != null)
                {
                    Log.Debug(this.GetType().ToString(), "网页授权access_token无效");

                    //重新获取access_token
                    wc = WeiXinSDK.WeiXin.GetWebAccessToken(code);
                    if (wc.error != null)
                    {
                        Response.Write(wc.error.ToString());
                        Response.End();
                    }
                }

                if (IsUserInDataBase(wc.openid)) //用户已存在
                {
                    //用户登录
                    Login(wc.openid);
                }
                else
                {
                    if (state == "2")
                    {
                        //第四步：拉取用户信息(需scope为 snsapi_userinfo)
                        WebUserInfo userinfo = WeiXinSDK.WeiXin.GetWebUserInfo(wc.access_token, wc.openid, WeiXinSDK.LangType.zh_CN);
                        //向数据库添加用户信息并登录
                        InsertUserInfoAndLogin(userinfo);
                    }
                    else
                    {
                        Log.Debug(this.GetType().ToString(), "用户不存在并获取网页授权snsapi_userinfo信息：" + wc.openid);
                        //第二步：通过code换取网页授权access_token
                        scope = "snsapi_userinfo";
                        string codeurl = WeiXinSDK.WeiXin.BuildWebCodeUrl(redirect_uri, scope, "2");
                        
                        Response.Redirect(codeurl);
                        return;
                    }
                }
                Response.Redirect(url);
            }
        }

        /// <summary>
        /// 判断用户是否存在于数据库中
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        protected virtual bool IsUserInDataBase(string openid)
        {
            return true;
        }

        /// <summary>
        /// 添加登录Session
        /// </summary>
        protected virtual void Login(string openid)
        {
        }

        /// <summary>
        /// 向数据库添加用户信息并登录
        /// </summary>
        protected virtual void InsertUserInfoAndLogin(WebUserInfo useinfo)
        {

        }
    }
}
