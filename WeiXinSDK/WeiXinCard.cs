﻿using System;
using System.Collections.Generic;
using System.Text;
using WeiXinSDK.Shop;
using WeiXinSDK.Card;
using WeiXinSDK.Credential;

namespace WeiXinSDK
{
    /// <summary>
    /// 微信卡券
    /// </summary>
    public class WeiXinCard
    {
        #region 卡券管理接口

        /// <summary>
        /// 创建卡券
        /// </summary>
        /// <param name="cardjson"></param>
        /// <returns></returns>
        public static AddCardResult AddCard(string cardjson)
        {
            var url = "https://api.weixin.qq.com/card/create?access_token=";
            var access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var json = Util.HttpPost2(url, cardjson);
            return Util.JsonTo<AddCardResult>(json);
        }

        /// <summary>
        /// 删除卡券
        /// </summary>
        /// <param name="card_id"></param>
        /// <returns></returns>
        public static ReturnCode DeleteCard(string card_id)
        {
            var url = "https://api.weixin.qq.com/card/delete?access_token=";
            var access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var json = Util.HttpPost2(url, Util.ToJson(new { card_id = card_id }));
            return Util.JsonTo<ReturnCode>(json);
        }

        /// <summary>
        /// 查询卡券code 的有效性
        /// </summary>
        /// <param name="code"></param>
        /// <param name="card_id"></param>
        /// <returns></returns>
        public static CheckCodeResult CheckCode(string code, string card_id)
        {
            var url = "https://api.weixin.qq.com/card/code/get?access_token=";
            var access_token = WeiXin.GetAccessToken();
            url = url + access_token;

            string jsonstr = "";
            if (string.IsNullOrEmpty(card_id))
            {
                jsonstr = Util.ToJson(new { code = code });
            }
            else
            {
                jsonstr = Util.ToJson(new { code = code, card_id = card_id });
            }

            var json = Util.HttpPost2(url, jsonstr);
            return Util.JsonTo<CheckCodeResult>(json);
        }

        /// <summary>
        /// 获取卡券列表
        /// </summary>
        /// <param name="offset">查询卡列表的起始偏移量，从0 开始</param>
        /// <param name="count">需要查询的卡片的数量（数量最大50）</param>
        /// <returns></returns>
        public static GetCardListResult GetCardList(int offset, int count)
        {
            var url = "https://api.weixin.qq.com/card/batchget?access_token=";
            var access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var json = Util.HttpPost2(url, Util.ToJson(new { offset = offset, count = count }));
            return Util.JsonTo<GetCardListResult>(json);
        }

        /// <summary>
        /// 获取所有卡券列表
        /// </summary>
        /// <returns></returns>
        public static GetCardListResult GetAllCardList()
        {
            WeiXin.CheckGlobalCredential();
            GetCardListResult allCard = new GetCardListResult();
            allCard.total_num = 0;
            allCard.card_id_list = new List<string>();
            allCard.errcode = 0;
            allCard.errmsg = "ok";

            int offset = 0;
            int count = 50;
            int totalnum = 0;
            do
            {
                var c = GetCardList(offset, count);
                if (c.errcode != 0)
                {
                    allCard.errcode = c.errcode;
                    allCard.errmsg = c.errmsg;
                    break;
                }
                else
                {
                    if (c.total_num > 0)
                    {
                        foreach (var cdid in c.card_id_list)
                        {
                            allCard.card_id_list.Add(cdid);
                        }
                    }
                    totalnum = c.total_num;
                    offset = offset + count;
                }
            } while (offset < totalnum);

            allCard.total_num = totalnum;
            return allCard;
        }
        #endregion

        #region 卡券信息
        /// <summary>
        /// 得到卡券基本信息
        /// </summary>
        /// <param name="cardid"></param>
        /// <returns></returns>
        public static CardReturn GetCardInfo(string cardid)
        {
            string url = "https://api.weixin.qq.com/card/get?access_token=";
            string access_token = WeiXinSDK.WeiXin.GetAccessToken();
            url = url + access_token;

            var json = Util.HttpPost2(url, Util.ToJson(new { card_id = cardid }));

            return Util.JsonTo<CardReturn>(json);
        }
        #endregion

        #region 生成卡券二维码

        /// <summary>
        /// 创建二维码ticket
        /// </summary>
        /// <param name="QrCard"></param>
        /// <returns></returns>
        public static QRCardTicket CreateQRCard(QrCard card)
        {
            string url = "https://api.weixin.qq.com/card/qrcode/create?access_token=";
            string access_token = WeiXinSDK.WeiXin.GetAccessToken();
            url = url + access_token;

            var json = Util.HttpPost2(url, Util.ToJson(card));
            return Util.JsonTo<QRCardTicket>(json);
        }

        #endregion

        #region 设置测试用户白名单

        /// <summary>
        /// 设置测试用户白名单
        /// </summary>
        /// <param name="whitelist"></param>
        /// <returns></returns>
        public static SetTestWhiteListResult SetTestWhiteList(TestWhiteList whitelist)
        {
            string url = "https://api.weixin.qq.com/card/testwhitelist/set?access_token=";
            string access_token = WeiXinSDK.WeiXin.GetAccessToken();
            url = url + access_token;

            var json = Util.HttpPost2(url, Util.ToJson(whitelist));
            return Util.JsonTo<SetTestWhiteListResult>(json);
        }
        #endregion

        #region 门店管理接口
        /// <summary>
        /// 获取颜色列表
        /// </summary>
        /// <param name="offset">查询卡列表的起始偏移量，从0 开始</param>
        /// <param name="count">需要查询的卡片的数量（数量最大50）</param>
        /// <returns></returns>
        public static GetColorsListResult GetColorsList()
        {
            var url = "https://api.weixin.qq.com/card/getcolors?access_token=";
            var access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var json = Util.HttpGet2(url);
            return Util.JsonTo<GetColorsListResult>(json);
        }
        #endregion

        #region 卡券JS-SDk

        /// <summary>
        /// 公众号用于调用微信卡券JS接口的临时票据
        /// </summary>
        /// <param name="appid">应用ID</param>
        /// <param name="secret">开发者凭据</param>
        /// <returns></returns>
        public static string GetCardApiTicket()
        {
            //正常情况下api_ticket有效期为7200秒,这里使用缓存设置短于这个时间即可
            string jsapi_ticket = MemoryCacheHelper.GetCacheItem<string>("api_ticket", delegate ()
            {
                string access_token = WeiXin.GetAccessToken();
                string data = Util.HttpGet2("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + access_token + "&type=wx_card ");
                JsApiTicket ticket = Util.JsonTo<JsApiTicket>(data);
                return ticket.ticket;
            },
                new TimeSpan(0, 0, 600)//300秒过期
                , DateTime.Now.AddSeconds(600)
            );

            return jsapi_ticket;
        }

        /// <summary>
        /// code 解码接口
        /// </summary>
        /// <param name="encrypt_code">加密的卡券code</param>
        /// <returns></returns>
        public static DecryptCodeResult DecryptCode(string encrypt_code)
        {
            string access_token = WeiXin.GetAccessToken();

            string url = "https://api.weixin.qq.com/card/code/decrypt?access_token=" + access_token;
            string data = Util.HttpPost2(url, Util.ToJson(new { encrypt_code = encrypt_code }));

            DecryptCodeResult relcode = Util.JsonTo<DecryptCodeResult>(data);
            return relcode;
        }

        /// <summary>
        /// 卡券核销接口
        /// </summary>
        /// <param name="code">要消耗序列号</param>
        /// <param name="card_id">卡券ID。创建卡券时use_custom_code 填写true 时必填。非自定义code 不必填写。</param>
        /// <returns></returns>
        public static ConsumeCardResult ConsumeCard(string code, string card_id)
        {
            string access_token = WeiXin.GetAccessToken();

            string jsonstr = "";
            if (string.IsNullOrEmpty(card_id))//卡券id为空
            {
                jsonstr = Util.ToJson(new { code = code });
            }
            else
            {
                jsonstr = Util.ToJson(new { code = code, card_id = card_id });
            }

            string url = "https://api.weixin.qq.com/card/code/consume?access_token=" + access_token;
            string data = Util.HttpPost2(url, jsonstr);

            ConsumeCardResult result = Util.JsonTo<ConsumeCardResult>(data);
            return result;
        }

        /// <summary>
        /// 设置卡券失效接口
        /// </summary>
        /// <param name="code">需要设置为失效的code</param>
        /// <param name="card_id">卡券ID。创建卡券时use_custom_code 填写true 时必填。非自定义code 不必填写。</param>
        /// <returns></returns>
        public static ReturnCode UnAvailableCard(string code, string card_id)
        {
            string access_token = WeiXin.GetAccessToken();

            string jsonstr = "";
            if (string.IsNullOrEmpty(card_id))//卡券id为空
            {
                jsonstr = Util.ToJson(new { code = code });
            }
            else
            {
                jsonstr = Util.ToJson(new { code = code, card_id = card_id });
            }

            string url = "https://api.weixin.qq.com/card/code/unavailable?access_token=" + access_token;
            string data = Util.HttpPost2(url, jsonstr);

            ReturnCode result = Util.JsonTo<ReturnCode>(data);
            return result;
        }

        /// <summary>
        /// 修改库存接口
        /// </summary>
        /// <param name="card_id">卡券ID。</param>
        /// <param name="increase_stock_value">增加多少库存，可以不填或填0code</param>
        /// <param name="reduce_stock_value">减少多少库存，可以不填或填0</param>
        /// <returns></returns>
        public static ReturnCode ModifyStock(string card_id, int increase_stock_value, int reduce_stock_value)
        {
            string access_token = WeiXin.GetAccessToken();

            string jsonstr = "";

            jsonstr = Util.ToJson(new { card_id = card_id, increase_stock_value = increase_stock_value, reduce_stock_value = reduce_stock_value });

            string url = "https://api.weixin.qq.com/card/modifystock?access_token=" + access_token;
            string data = Util.HttpPost2(url, jsonstr);

            ReturnCode result = Util.JsonTo<ReturnCode>(data);
            return result;
        }

        /// <summary>
        /// 更改卡券信息接口
        /// </summary>
        /// <param name="card_id">卡券ID。</param>
        /// <param name="increase_stock_value">增加多少库存，可以不填或填0code</param>
        /// <param name="reduce_stock_value">减少多少库存，可以不填或填0</param>
        /// <returns></returns>
        public static ReturnCode UpdateCardInfo(string jsonstr)
        {
            string access_token = WeiXin.GetAccessToken();

            string url = "https://api.weixin.qq.com/card/update?access_token=" + access_token;
            string data = Util.HttpPost2(url, jsonstr);

            ReturnCode result = Util.JsonTo<ReturnCode>(data);
            return result;
        }

        /// <summary>
        /// 添加到卡包 签名 http://mp.weixin.qq.com/debug/cgi-bin/sandbox?t=cardsign
        /// </summary>
        /// <param name="api_ticket">公众号用于调用微信JS接口的临时票据</param>
        /// <param name="card_id">卡券ID</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="code">(选填)</param>
        /// <param name="openid">(选填)</param>
        /// <param name="balance">(选填)</param>
        /// <returns>signature 微信加密签名</returns>
        public static string GetAddCardSignature(string api_ticket, string card_id, string timestamp, string code, string openid, string balance)
        {
            List<string> tmpList = new List<string>();

            tmpList.Add(api_ticket);
            tmpList.Add(card_id);
            tmpList.Add(timestamp);
            if (!string.IsNullOrEmpty(code))
            {
                tmpList.Add(code);
            }
            if (!string.IsNullOrEmpty(openid))
            {
                tmpList.Add(openid);
            }
            if (!string.IsNullOrEmpty(balance))
            {
                tmpList.Add(balance);
            }

            tmpList.Sort();
            var tmpStr = string.Join("", tmpList.ToArray());

            //string strResult = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1").ToLower();
            string strResult = WeiXinSDK.Tools.HashCode.SHA1(tmpStr).ToLower();
            return strResult;
        }

        /// <summary>
        /// 拉取本地卡券列表 签名 http://mp.weixin.qq.com/debug/cgi-bin/sandbox?t=cardsign
        /// </summary>
        /// <param name="api_ticket">公众号用于调用微信JS接口的临时票据</param>
        /// <param name="time_stamp">时间戳</param>
        /// <param name="nonce_str">随机字符串</param>
        /// <param name="location_id">门店信息(选填)</param>
        /// <param name="card_id">卡券ID(选填)</param>
        /// <param name="card_type">卡券类型(选填)</param>
        /// <returns>signature 微信加密签名</returns>
        public static string GetChooseCardSignature(string api_ticket, string location_id, string time_stamp, string nonce_str, string card_id, string card_type)
        {
            List<string> tmpList = new List<string>();

            tmpList.Add(api_ticket);
            tmpList.Add(WeiXin.appConfig.APP_ID);
            if (!string.IsNullOrEmpty(location_id))
            {
                tmpList.Add(location_id);
            }
            tmpList.Add(time_stamp);
            tmpList.Add(nonce_str);

            if (!string.IsNullOrEmpty(card_id))
            {
                tmpList.Add(card_id);
            }
            if (!string.IsNullOrEmpty(card_type))
            {
                tmpList.Add(card_type);
            }

            tmpList.Sort();
            var tmpStr = string.Join("", tmpList.ToArray());

            //string strResult = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1").ToLower();
            string strResult = WeiXinSDK.Tools.HashCode.SHA1(tmpStr).ToLower();

            return strResult;
        }

        #endregion

        #region 功能接口
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="data">图片数据</param>
        /// <returns></returns>
        public static UploadImgResult UploadImg(string file)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token=";
            var access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var json = Util.HttpUpload(url, file);

            return Util.JsonTo<UploadImgResult>(json);
        }
        #endregion
    }
}
