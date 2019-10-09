using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Account
{
    public class Account
    {
        #region 二维码
        /// <summary>
        /// 创建二维码ticket
        /// </summary>
        /// <param name="isTemp"></param>
        /// <param name="scene_id"></param>
        /// <returns></returns>
        public static QRCodeTicket CreateQRCode(bool isTemp, int scene_id)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var action_name = isTemp ? "QR_SCENE" : "QR_LIMIT_SCENE";
            string data;
            if (isTemp)
            {
                data = "{\"expire_seconds\": 1800, \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\":" + scene_id + "}}}";
            }
            else
            {
                data = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + scene_id + "}}}";
            }

            var json = Util.HttpPost2(url, data);
            if (json.IndexOf("ticket") > 0)
            {
                return Util.JsonTo<QRCodeTicket>(json);
            }
            else
            {
                QRCodeTicket tk = new QRCodeTicket();
                tk.error = Util.JsonTo<ReturnCode>(json);
                return tk;
            }
        }

        /// <summary>
        /// 创建二维码ticket
        /// </summary>
        /// <param name="scene_str"></param>
        /// <returns></returns>
        public static QRCodeTicket CreateQRCode2(string scene_str)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            string data;

            data = "{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": {\"scene\": {\"scene_str\":\"" + scene_str + "\"}}}";

            var json = Util.HttpPost2(url, data);
            if (json.IndexOf("ticket") > 0)
            {
                return Util.JsonTo<QRCodeTicket>(json);
            }
            else
            {
                QRCodeTicket tk = new QRCodeTicket();
                tk.error = Util.JsonTo<ReturnCode>(json);
                return tk;
            }
        }

        /// <summary>
        /// 得到QR图片地址
        /// </summary>
        /// <param name="qrcodeTicket"></param>
        /// <returns></returns>
        public static string GetQRUrl(string qrcodeTicket)
        {
            return "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + System.Web.HttpUtility.HtmlEncode(qrcodeTicket);
        }
        #endregion

        #region 长链接转短链接
        /// <summary>
        /// 长链接转短链接
        /// </summary>
        /// <returns></returns>
        public static Long2ShortUrlResult Long2ShortUrl(string long_url, string action = "long2short")
        {
            string url = "https://api.weixin.qq.com/cgi-bin/shorturl?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;

            var data = new
            {
                action = action,
                long_url = long_url
            };

            var json = Util.HttpPost2(url, Util.ToJson(data));
            return Util.JsonTo<Long2ShortUrlResult>(json);
        }
        #endregion

    }
}
