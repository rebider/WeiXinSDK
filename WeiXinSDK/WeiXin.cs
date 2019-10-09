using System;
using System.Collections.Generic;
using System.IO;
using WeiXinSDK.Menu;
using WeiXinSDK.Message;
using WeiXinSDK.Mass;
using WeiXinSDK.Media;
using WeiXinSDK.Config;
using WeiXinSDK.User;
using WeiXinSDK.Crypt;
using WeiXinSDK.Credential;
using System.Configuration;

namespace WeiXinSDK
{
    /// <summary>
    /// 消息事件处理委托
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public delegate TResult MyFunc<T1, TResult>(T1 t);

    /// <summary>
    /// 微信接口API
    /// </summary>
    public class WeiXin
    {
        internal static AppConfig appConfig = new AppConfig()
        {
            APP_ID = ConfigurationManager.AppSettings["APP_ID"],
            APP_SECRET = ConfigurationManager.AppSettings["APP_SECRET"],
            APP_TOKEN = ConfigurationManager.AppSettings["APP_TOKEN"],
            WEB_URL = ConfigurationManager.AppSettings["WEB_URL"],
            ENCODING_AES_KEY = ConfigurationManager.AppSettings["ENCODING_AES_KEY"],
            MCHID = ConfigurationManager.AppSettings["MCHID"],
            KEY = ConfigurationManager.AppSettings["KEY"],
            SSLCERT_PASSWORD = ConfigurationManager.AppSettings["SSLCERT_PASSWORD"],
            SSLCERT_PATH = ConfigurationManager.AppSettings["SSLCERT_PATH"],
            IP = ConfigurationManager.AppSettings["IP"],
            NOTIFY_URL = ConfigurationManager.AppSettings["NOTIFY_URL"],
            LOG_LEVENL = int.Parse(ConfigurationManager.AppSettings["LOG_LEVENL"]),
        };
        static object lockObj = new object();

        /// <summary>
        /// 获取微信配置
        /// </summary>
        /// <returns></returns>
        public static AppConfig GetAppConfig()
        {
            return appConfig;
        }

        /// <summary>
        /// 获取每次操作微信API的Token访问令牌
        /// </summary>
        public static string GetAccessToken()
        {
            CheckGlobalCredential();
            //正常情况下access_token有效期为7200秒,这里使用缓存设置短于这个时间即可
            string access_token = MemoryCacheHelper.GetCacheItem<string>("access_token", delegate ()
            {
                var credential = ClientCredential.GetCredential(appConfig.APP_ID, appConfig.APP_SECRET);
                return credential.access_token;

            },
                new TimeSpan(0, 0, 600)//300秒过期
                , DateTime.Now.AddSeconds(600)
            );

            return access_token;
        }

        /// <summary>
        /// 检验signature
        /// </summary>
        /// <param name="signature">微信加密签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="token">由AppId和AppSecret得到的凭据</param>
        /// <returns></returns>
        public static bool CheckSignature(string signature, string timestamp, string nonce, string token)
        {
            if (string.IsNullOrEmpty(signature)) return false;
            List<string> tmpList = new List<string>(3);
            tmpList.Add(token);
            tmpList.Add(timestamp);
            tmpList.Add(nonce);
            tmpList.Sort();
            var tmpStr = string.Join("", tmpList.ToArray());
            //string strResult = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            string strResult = WeiXinSDK.Tools.HashCode.SHA1(tmpStr);
            return signature.Equals(strResult, StringComparison.InvariantCultureIgnoreCase);
        }

        #region 消息

        /// <summary>
        /// 处理用户消息和事件
        /// </summary>
        /// <returns></returns>
        public static ReplyBaseMsg ReplyMsg()
        {
            Stream inputStream = System.Web.HttpContext.Current.Request.InputStream;
            long pos = inputStream.Position;
            inputStream.Position = 0;
            byte[] buffer = new byte[inputStream.Length];
            inputStream.Read(buffer, 0, buffer.Length);
            inputStream.Position = pos;
            string xml = System.Text.Encoding.UTF8.GetString(buffer);
            
            #region 消息解密

            string encrypt_type = System.Web.HttpContext.Current.Request.QueryString["encrypt_type"];
            if (encrypt_type == "aes" && !string.IsNullOrEmpty(appConfig.ENCODING_AES_KEY))
            {
                WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(appConfig.APP_TOKEN, appConfig.ENCODING_AES_KEY, appConfig.APP_ID);

                string msg_signature = System.Web.HttpContext.Current.Request.QueryString["msg_signature"];
                string timestamp = System.Web.HttpContext.Current.Request.QueryString["timestamp"];
                string nonce = System.Web.HttpContext.Current.Request.QueryString["nonce"];

                string stmp = "";
                if (wxcpt.DecryptMsg(msg_signature, timestamp, nonce, xml, ref stmp) == 0)
                    xml = stmp;//解密之后的消息
                else
                    return new ReplyEmptyMsg();//解密错误
            }

            #endregion


            //消息排重
            xml = MsgQueue.GetNoRepeatMsgByXml(xml);
            if (string.IsNullOrEmpty(xml))//消息重复，不处理
            {
                return new ReplyEmptyMsg();
            }

            var dict = Util.GetDictFromXml(xml);

            string key = string.Empty;
            ReplyBaseMsg replyMsg = ReplyEmptyMsg.Instance;
            if (dict.ContainsKey("Event"))
            {
                #region 接收事件消息
                var evt = dict["Event"].ToLower();
                key = "event_";
                switch (evt)
                {
                    case "click"://点击菜单拉取消息时的事件推送 
                        {
                            var msg = new EventClickMsg { CreateTime = Int64.Parse(dict["CreateTime"]), FromUserName = dict["FromUserName"], ToUserName = dict["ToUserName"], MyEventType = MyEventType.Click, EventKey = dict["EventKey"] };
                            replyMsg = GetReply<EventClickMsg>(key + MyEventType.Click.ToString(), msg);
                            break;
                        }
                    case "view"://点击菜单跳转链接时的事件推送 
                        {
                            var msg = new EventViewMsg { CreateTime = Int64.Parse(dict["CreateTime"]), FromUserName = dict["FromUserName"], ToUserName = dict["ToUserName"], MyEventType = MyEventType.View, EventKey = dict["EventKey"] };
                            replyMsg = GetReply<EventViewMsg>(key + MyEventType.View.ToString(), msg);
                            break;
                        }
                    case "location"://上报地理位置事件
                        {
                            var msg = new EventLocationMsg { CreateTime = Int64.Parse(dict["CreateTime"]), FromUserName = dict["FromUserName"], ToUserName = dict["ToUserName"], MyEventType = MyEventType.Location, Latitude = double.Parse(dict["Latitude"]), Longitude = double.Parse(dict["Longitude"]), Precision = double.Parse(dict["Precision"]) };
                            replyMsg = GetReply<EventLocationMsg>(key + MyEventType.Location.ToString(), msg);
                            break;
                        }
                    case "scan"://用户已关注时扫描带参数二维码的事件推送 
                        {
                            var msg = new EventFansScanMsg { CreateTime = Int64.Parse(dict["CreateTime"]), FromUserName = dict["FromUserName"], ToUserName = dict["ToUserName"], MyEventType = MyEventType.FansScan, EventKey = dict["EventKey"], Ticket = dict["Ticket"] };
                            replyMsg = GetReply<EventFansScanMsg>(key + MyEventType.FansScan.ToString(), msg);
                            break;
                        }
                    case "unsubscribe"://取消关注事件
                        {
                            var msg = new EventUnattendMsg { CreateTime = Int64.Parse(dict["CreateTime"]), FromUserName = dict["FromUserName"], ToUserName = dict["ToUserName"], MyEventType = MyEventType.Unattend };
                            replyMsg = GetReply<EventUnattendMsg>(key + MyEventType.Unattend.ToString(), msg);
                            break;
                        }
                    case "subscribe"://关注事件
                        {
                            if (dict.ContainsKey("Ticket"))
                            {//用户未关注时，扫描二维码进行关注后的事件推送 
                                var msg = new EventUserScanMsg { CreateTime = Int64.Parse(dict["CreateTime"]), FromUserName = dict["FromUserName"], ToUserName = dict["ToUserName"], MyEventType = MyEventType.UserScan, Ticket = dict["Ticket"], EventKey = dict["EventKey"] };
                                replyMsg = GetReply<EventUserScanMsg>(key + MyEventType.UserScan.ToString(), msg);
                            }
                            else//普通关注事件
                            {
                                var msg = new EventAttendMsg { CreateTime = Int64.Parse(dict["CreateTime"]), FromUserName = dict["FromUserName"], ToUserName = dict["ToUserName"], MyEventType = MyEventType.Attend };
                                replyMsg = GetReply<EventAttendMsg>(key + MyEventType.Attend.ToString(), msg);
                            }
                            break;
                        }
                    case "masssendjobfinish"://事件推送群发结果 
                        {
                            var msg = new EventMassSendJobFinishMsg
                            {
                                CreateTime = Int64.Parse(dict["CreateTime"]),
                                FromUserName = dict["FromUserName"],
                                ToUserName = dict["ToUserName"],
                                MyEventType = MyEventType.MASSSENDJOBFINISH,
                                ErrorCount = int.Parse(dict["ErrorCount"]),
                                FilterCount = int.Parse(dict["FilterCount"]),
                                MsgID = int.Parse(dict["MsgID"]),
                                SentCount = int.Parse(dict["SentCount"]),
                                TotalCount = int.Parse(dict["TotalCount"]),
                                Status = dict["Status"]
                            };

                            replyMsg = GetReply<EventMassSendJobFinishMsg>(key + MyEventType.MASSSENDJOBFINISH.ToString(), msg);
                            break;
                        }
                    case "merchant_order"://订单付款通知
                        {
                            var msg = new EventMerchantOrderMsg
                            {
                                CreateTime = Int64.Parse(dict["CreateTime"]),
                                FromUserName = dict["FromUserName"],
                                ToUserName = dict["ToUserName"],
                                MyEventType = MyEventType.MerchantOrder,
                                OrderId = dict["OrderId"],
                                OrderStatus = int.Parse(dict["OrderStatus"]),
                                ProductId = dict["ProductId"],
                                SkuInfo = dict["SkuInfo"]
                            };
                            replyMsg = GetReply<EventMerchantOrderMsg>(key + MyEventType.MerchantOrder.ToString(), msg);
                            break;
                        }
                    case "card_pass_check"://卡券审核成功通知
                        {
                            var msg = new EventCardPassCheckMsg
                            {
                                CreateTime = Int64.Parse(dict["CreateTime"]),
                                FromUserName = dict["FromUserName"],
                                ToUserName = dict["ToUserName"],
                                MyEventType = MyEventType.CardPassCheck,
                                CardId = dict["CardId"]
                            };
                            replyMsg = GetReply<EventCardPassCheckMsg>(key + MyEventType.CardPassCheck.ToString(), msg);
                            break;
                        }
                    case "card_not_pass_check"://卡券审核失败通知
                        {
                            var msg = new EventCardPassCheckMsg
                            {
                                CreateTime = Int64.Parse(dict["CreateTime"]),
                                FromUserName = dict["FromUserName"],
                                ToUserName = dict["ToUserName"],
                                MyEventType = MyEventType.CardNotPassCheck,
                                CardId = dict["CardId"]
                            };
                            replyMsg = GetReply<EventCardPassCheckMsg>(key + MyEventType.CardNotPassCheck.ToString(), msg);
                            break;
                        }
                    case "user_get_card"://领取卡券通知
                        {
                            var msg = new EventUserGetCardMsg
                            {
                                CreateTime = Int64.Parse(dict["CreateTime"]),
                                FromUserName = dict["FromUserName"],
                                ToUserName = dict["ToUserName"],
                                MyEventType = MyEventType.UserGetCard,
                                FriendUserName = dict["FriendUserName"],
                                CardId = dict["CardId"],
                                IsGiveByFriend = Convert.ToByte(dict["IsGiveByFriend"]),
                                UserCardCode = dict["UserCardCode"],
                                OuterId = dict["OuterId"]

                            };
                            replyMsg = GetReply<EventUserGetCardMsg>(key + MyEventType.UserGetCard.ToString(), msg);
                            break;
                        }
                    case "user_del_card"://删除卡券通知
                        {
                            var msg = new EventUserDelCardMsg
                            {
                                CreateTime = Int64.Parse(dict["CreateTime"]),
                                FromUserName = dict["FromUserName"],
                                ToUserName = dict["ToUserName"],
                                MyEventType = MyEventType.UserDelCard,
                                CardId = dict["CardId"],
                                UserCardCode = dict["UserCardCode"]

                            };
                            replyMsg = GetReply<EventUserDelCardMsg>(key + MyEventType.UserDelCard.ToString(), msg);
                            break;
                        }
                    case "user_consume_card"://核销卡券通知
                        {
                            var msg = new EventUserConsumeCardMsg
                            {
                                CreateTime = Int64.Parse(dict["CreateTime"]),
                                FromUserName = dict["FromUserName"],
                                ToUserName = dict["ToUserName"],
                                MyEventType = MyEventType.UserConsumeCard,
                                CardId = dict["CardId"],
                                UserCardCode = dict["UserCardCode"],
                                ConsumeSource = dict["ConsumeSource"]
                            };
                            replyMsg = GetReply<EventUserConsumeCardMsg>(key + MyEventType.UserConsumeCard.ToString(), msg);
                            break;
                        }
                    case "kf_create_session"://多客服接入
                        {
                            var msg = new EventKfCreateSession
                            {
                                CreateTime = Int64.Parse(dict["CreateTime"]),
                                FromUserName = dict["FromUserName"],
                                ToUserName = dict["ToUserName"],
                                MyEventType = MyEventType.UserConsumeCard,
                                KfAccount = dict["KfAccount"]
                            };
                            replyMsg = GetReply<EventKfCreateSession>(key + MyEventType.KfCreateSession.ToString(), msg);
                            break;
                        }
                    case "kf_close_session"://多客服关闭
                        {
                            var msg = new EventKfCloseSession
                            {
                                CreateTime = Int64.Parse(dict["CreateTime"]),
                                FromUserName = dict["FromUserName"],
                                ToUserName = dict["ToUserName"],
                                MyEventType = MyEventType.UserConsumeCard,
                                KfAccount = dict["KfAccount"]
                            };
                            replyMsg = GetReply<EventKfCloseSession>(key + MyEventType.KfCloseSession.ToString(), msg);
                            break;
                        }
                    case "kf_switch_session"://多客服转接
                        {
                            var msg = new EventKfSwitchSession
                            {
                                CreateTime = Int64.Parse(dict["CreateTime"]),
                                FromUserName = dict["FromUserName"],
                                ToUserName = dict["ToUserName"],
                                MyEventType = MyEventType.UserConsumeCard,
                                FromKfAccount = dict["FromKfAccount"],
                                ToKfAccount = dict["ToKfAccount"]
                            };
                            replyMsg = GetReply<EventKfSwitchSession>(key + MyEventType.KfSwitchSession.ToString(), msg);
                            break;
                        }
                }
                #endregion
            }
            else if (dict.ContainsKey("MsgId"))
            {
                #region 接收普通消息
                var msgType = dict["MsgType"];
                key = msgType;
                switch (msgType)
                {
                    case "text":
                        {
                            var msg = new RecTextMsg { CreateTime = Int64.Parse(dict["CreateTime"]), FromUserName = dict["FromUserName"], ToUserName = dict["ToUserName"], MsgId = long.Parse(dict["MsgId"]), Content = dict["Content"] };
                            replyMsg = GetReply<RecTextMsg>(key, msg);
                            break;
                        }
                    case "image":
                        {

                            var msg = new RecImageMsg { CreateTime = Int64.Parse(dict["CreateTime"]), FromUserName = dict["FromUserName"], ToUserName = dict["ToUserName"], MsgId = long.Parse(dict["MsgId"]), PicUrl = dict["PicUrl"], MediaId = dict["MediaId"] };
                            replyMsg = GetReply<RecImageMsg>(key, msg);
                            break;
                        }
                    case "voice":
                        {
                            string recognition;
                            dict.TryGetValue("Recognition", out recognition);
                            var msg = new RecVoiceMsg { CreateTime = Int64.Parse(dict["CreateTime"]), FromUserName = dict["FromUserName"], ToUserName = dict["ToUserName"], MsgId = long.Parse(dict["MsgId"]), Format = dict["Format"], MediaId = dict["MediaId"], Recognition = recognition };
                            replyMsg = GetReply<RecVoiceMsg>(key, msg);
                            break;
                        }
                    case "shortvideo":
                        {
                            var msg = new RecShortVideoMsg { CreateTime = Int64.Parse(dict["CreateTime"]), FromUserName = dict["FromUserName"], ToUserName = dict["ToUserName"], MsgId = long.Parse(dict["MsgId"]), ThumbMediaId = dict["ThumbMediaId"], MediaId = dict["MediaId"] };
                            replyMsg = GetReply<RecShortVideoMsg>(key, msg);
                            break;
                        }
                    case "video":
                        {
                            var msg = new RecVideoMsg { CreateTime = Int64.Parse(dict["CreateTime"]), FromUserName = dict["FromUserName"], ToUserName = dict["ToUserName"], MsgId = long.Parse(dict["MsgId"]), ThumbMediaId = dict["ThumbMediaId"], MediaId = dict["MediaId"] };
                            replyMsg = GetReply<RecVideoMsg>(key, msg);
                            break;
                        }
                    case "location":
                        {
                            var msg = new RecLocationMsg { CreateTime = Int64.Parse(dict["CreateTime"]), FromUserName = dict["FromUserName"], ToUserName = dict["ToUserName"], MsgId = long.Parse(dict["MsgId"]), Label = dict["Label"], Location_X = double.Parse(dict["Location_X"]), Location_Y = double.Parse(dict["Location_Y"]), Scale = int.Parse(dict["Scale"]) };
                            replyMsg = GetReply<RecLocationMsg>(key, msg);
                            break;
                        }
                    case "link":
                        {
                            var msg = new RecLinkMsg { CreateTime = Int64.Parse(dict["CreateTime"]), FromUserName = dict["FromUserName"], ToUserName = dict["ToUserName"], MsgId = long.Parse(dict["MsgId"]), Description = dict["Description"], Title = dict["Title"], Url = dict["Url"] };
                            replyMsg = GetReply<RecLinkMsg>(key, msg);
                            break;
                        }
                }
                #endregion
            }
            return replyMsg;
        }

        static Dictionary<string, object> m_msgHandlers = new Dictionary<string, object>();

        /// <summary>
        /// 注册消息处理程序
        /// </summary>
        /// <typeparam name="TMsg"></typeparam>
        /// <param name="handler"></param>
        public static void RegisterMsgHandler<TMsg>(MyFunc<TMsg, ReplyBaseMsg> handler) where TMsg : RecBaseMsg
        {
            var type = typeof(TMsg);
            var key = string.Empty;
            if (type == typeof(RecTextMsg))
            {
                key = "text";
            }
            else if (type == typeof(RecImageMsg))
            {
                key = "image";
            }
            else if (type == typeof(RecLinkMsg))
            {
                key = "link";
            }
            else if (type == typeof(RecLocationMsg))
            {
                key = "location";
            }
            else if (type == typeof(RecShortVideoMsg))
            {
                key = "shortvideo";
            }
            else if (type == typeof(RecVideoMsg))
            {
                key = "video";
            }
            else if (type == typeof(RecVoiceMsg))
            {
                key = "voice";
            }
            else
            {
                return;
            }
            m_msgHandlers[key.ToLower()] = handler;
        }
        /// <summary>
        /// 注册事件处理程序
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="handler"></param>
        public static void RegisterEventHandler<TEvent>(MyFunc<TEvent, ReplyBaseMsg> handler) where TEvent : EventBaseMsg
        {
            var type = typeof(TEvent);
            var key = "event_";
            if (type == typeof(EventClickMsg))//自定义菜单事件
            {
                key += MyEventType.Click.ToString();
            }
            else if (type == typeof(EventFansScanMsg))//用户已关注时,扫描带参数二维码事件
            {
                key += MyEventType.FansScan.ToString();
            }
            else if (type == typeof(EventAttendMsg))//用户关注事件
            {
                key += MyEventType.Attend.ToString();
            }
            else if (type == typeof(EventLocationMsg))//上报地理位置事件
            {
                key += MyEventType.Location.ToString();
            }
            else if (type == typeof(EventUnattendMsg))//取消关注事件
            {
                key += MyEventType.Unattend.ToString();
            }
            else if (type == typeof(EventUserScanMsg))//用户未关注,扫描带参数二维码事件
            {
                key += MyEventType.UserScan.ToString();
            }
            else if (type == typeof(EventMassSendJobFinishMsg))//事件推送群发结果 
            {
                key += MyEventType.MASSSENDJOBFINISH.ToString();
            }
            else if (type == typeof(EventViewMsg))//点击菜单跳转链接时的事件推送 
            {
                key += MyEventType.View.ToString();
            }
            else if (type == typeof(EventMerchantOrderMsg))//订单付款通知
            {
                key += MyEventType.MerchantOrder.ToString();
            }
            else if (type == typeof(EventCardPassCheckMsg))//卡券审核成功通知
            {
                key += MyEventType.CardPassCheck.ToString();
            }
            else if (type == typeof(EventCardNotPassCheckMsg))//卡券审核失败通知
            {
                key += MyEventType.CardNotPassCheck.ToString();
            }
            else if (type == typeof(EventUserGetCardMsg))//领取卡券通知
            {
                key += MyEventType.UserGetCard.ToString();
            }
            else if (type == typeof(EventUserDelCardMsg))//删除卡券通知
            {
                key += MyEventType.UserDelCard.ToString();
            }
            else if (type == typeof(EventUserConsumeCardMsg))//核销卡券通知
            {
                key += MyEventType.UserConsumeCard.ToString();
            }
            else if (type == typeof(EventKfCreateSession))//多客服接入
            {
                key += MyEventType.KfCreateSession.ToString();
            }
            else if (type == typeof(EventKfCloseSession))//多客服关闭
            {
                key += MyEventType.KfCloseSession.ToString();
            }
            else if (type == typeof(EventKfSwitchSession))//多客服转接
            {
                key += MyEventType.KfSwitchSession.ToString();
            }
            else
            {
                return;
            }
            m_msgHandlers[key.ToLower()] = handler;
        }

        static ReplyBaseMsg GetReply<TMsg>(string key, TMsg msg) where TMsg : RecEventBaseMsg
        {
            key = key.ToLower();
            if (m_msgHandlers.ContainsKey(key))
            {
                var handler = m_msgHandlers[key] as MyFunc<TMsg, ReplyBaseMsg>;
                var replyMsg = handler(msg);

                if (replyMsg.CreateTime == 0) replyMsg.CreateTime = Util.TimeToUnixTime(DateTime.Now);
                if (string.IsNullOrEmpty(replyMsg.FromUserName)) replyMsg.FromUserName = msg.ToUserName;
                if (string.IsNullOrEmpty(replyMsg.ToUserName)) replyMsg.ToUserName = msg.FromUserName;
                return replyMsg;
            }
            else
            {
                return ReplyEmptyMsg.Instance;
            }
        }

        /// <summary>
        /// 主动给用户发消息（用户）
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>errcode=0为成功</returns>
        public static ReturnCode SendMsg(SendBaseMsg msg)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token;
            var json = msg.GetJSON();

            var retJson = Util.HttpPost2(url, json);
            return Util.JsonTo<ReturnCode>(retJson);
        }
        #endregion

        #region 群发
        /// <summary>
        /// 根据分组进行群发
        /// </summary>
        /// <param name="mess"></param>
        /// <returns></returns>
        public SendReturnCode SendMessByGroup(FilterMess mess)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token;
            var json = Util.ToJson(mess);
            var retJson = Util.HttpPost2(url, json);
            return Util.JsonTo<SendReturnCode>(retJson);
        }

        /// <summary>
        /// 根据OpenID列表群发
        /// </summary>
        /// <param name="mess"></param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public SendReturnCode SendMessByUsers(ToUserMess mess)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token;
            var json = Util.ToJson(mess);
            var retJson = Util.HttpPost2(url, json);
            return Util.JsonTo<SendReturnCode>(retJson);
        }

        /// <summary>
        /// 删除群发.
        /// 请注意，只有已经发送成功的消息才能删除删除消息只是将消息的图文详情页失效，已经收到的用户，还是能在其本地看到消息卡片。 另外，删除群发消息只能删除图文消息和视频消息，其他类型的消息一经发送，无法删除。
        /// </summary>
        /// <param name="msgid"></param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public ReturnCode DeleteMess(int msgid)
        {
            var url = "https://api.weixin.qq.com//cgi-bin/message/mass/delete?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token;
            var json = "{\"msgid\":" + msgid.ToString() + "}";
            var retJson = Util.HttpPost2(url, json);
            return Util.JsonTo<ReturnCode>(retJson);
        }


        #endregion

        #region 自定义菜单

        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static ReturnCode CreateMenu(CustomMenu menu)
        {
            var json = menu.GetJSON();
            return CreateMenu(json);
        }

        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        /// <param name="menuJSON"></param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static ReturnCode CreateMenu(string menuJSON)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token;
            var retJson = Util.HttpPost2(url, menuJSON);
            return Util.JsonTo<ReturnCode>(retJson);
        }

        /// <summary>
        /// 直接返回自定义菜单json字符串，
        /// </summary>
        /// <returns></returns>
        public static string GetMenu()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token;
            var json = Util.HttpGet2(url);
            return json;
        }

        /// <summary>
        /// 删除自定义菜单
        /// </summary>
        /// <returns></returns>
        public static ReturnCode DeleteMenu()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token;
            var json = Util.HttpGet2(url);
            return Util.JsonTo<ReturnCode>(json);
        }
        #endregion

        #region 获取关注者列表

        /// <summary>
        /// 获取关注者列表
        /// </summary>
        /// <param name="next_openid"></param>
        /// <returns></returns>
        public static Followers GetFollowers(string next_openid)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token;
            if (!string.IsNullOrEmpty(next_openid))
            {
                url = url + "&next_openid=" + next_openid;
            }
            var json = Util.HttpGet2(url);
            if (json.IndexOf("errcode") > 0)
            {
                var fs = new Followers();
                fs.error = Util.JsonTo<ReturnCode>(json);
                return fs;
            }
            else
            {
                return Util.JsonTo<Followers>(json);
            }
        }

        /// <summary>
        /// 获取所有关注者列表
        /// </summary>
        /// <returns></returns>
        public static Followers GetAllFollowers()
        {
            Followers allFollower = new Followers();
            allFollower.data = new Followers.Data();
            allFollower.data.openid = new List<string>();

            string next_openid = string.Empty;
            do
            {
                var f = GetFollowers(next_openid);
                if (f.error != null)
                {
                    allFollower.error = f.error;
                    break;
                }
                else
                {
                    if (f.count > 0)
                    {
                        foreach (var opid in f.data.openid)
                        {
                            allFollower.data.openid.Add(opid);
                        }
                    }
                    next_openid = f.next_openid;
                }
            } while (!string.IsNullOrEmpty(next_openid));

            allFollower.count = allFollower.total;
            return allFollower;
        }
        #endregion

        #region 用户信息
        /// <summary>
        /// 得到用户基本信息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static UserInfo GetUserInfo(string openid, LangType lang)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token + "&openid=" + openid + "&lang=" + lang.ToString();

            var json = Util.HttpGet2(url);

            if (json.IndexOf("errcode") > 0)
            {
                var ui = new UserInfo();
                ui.error = Util.JsonTo<ReturnCode>(json);
                return ui;
            }
            else
            {
                return Util.JsonTo<UserInfo>(json);
            }
        }
        #endregion

        #region 分组

        /// <summary>
        /// 创建分组
        /// </summary>
        /// <param name="name">分组名字（30个字符以内）</param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static GroupInfo CreateGroup(string name)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/groups/create?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token;
            var post = "{\"group\":{\"name\":\"" + name + "\"}}";
            var json = Util.HttpPost2(url, post);
            if (json.IndexOf("errcode") > 0)
            {
                var gi = new GroupInfo();
                gi.error = Util.JsonTo<ReturnCode>(json);
                return gi;
            }
            else
            {
                var dict = Util.JsonTo<Dictionary<string, Dictionary<string, object>>>(json);
                var gi = new GroupInfo();
                var gpdict = dict["group"];
                gi.id = Convert.ToInt32(gpdict["id"]);
                gi.name = gpdict["name"].ToString();
                return gi;
            }
        }

        /// <summary>
        /// 查询所有分组
        /// </summary>
        /// <returns></returns>
        public static Groups GetGroups()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/groups/get?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token;
            string json = Util.HttpGet2(url);
            if (json.IndexOf("errcode") > 0)
            {
                var gs = new Groups();
                gs.error = Util.JsonTo<ReturnCode>(json);
                return gs;
            }
            else
            {
                var dict = Util.JsonTo<Dictionary<string, List<Dictionary<string, object>>>>(json);
                var gs = new Groups();
                var gilist = dict["groups"];
                foreach (var gidict in gilist)
                {
                    var gi = new GroupInfo();
                    gi.name = gidict["name"].ToString();
                    gi.id = Convert.ToInt32(gidict["id"]);
                    gi.count = Convert.ToInt32(gidict["count"]);
                    gs.Add(gi);
                }
                return gs;
            }
        }

        /// <summary>
        /// 查询用户所在分组
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public static GroupID GetUserGroup(string openid)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/groups/getid?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token;
            var post = "{\"openid\":\"" + openid + "\"}";
            var json = Util.HttpPost2(url, post);
            if (json.IndexOf("errcode") > 0)
            {
                var gid = new GroupID();
                gid.error = Util.JsonTo<ReturnCode>(json);
                return gid;
            }
            else
            {
                var dict = Util.JsonTo<Dictionary<string, int>>(json);
                var gid = new GroupID();
                gid.id = dict["groupid"];
                return gid;
            }
        }

        /// <summary>
        /// 修改分组名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static ReturnCode UpdateGroup(int id, string name)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/groups/update?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token;
            var post = "{\"group\":{\"id\":" + id + ",\"name\":\"" + name + "\"}}";
            var json = Util.HttpPost2(url, post);
            return Util.JsonTo<ReturnCode>(json);
        }

        /// <summary>
        /// 移动用户分组
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public static ReturnCode MoveGroup(string openid, int groupid)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token;
            var post = "{\"openid\":\"" + openid + "\",\"to_groupid\":" + groupid + "}";
            var json = Util.HttpPost2(url, post);
            return Util.JsonTo<ReturnCode>(json);
        }
        #endregion

        #region 多媒体文件
        /// <summary>
        /// 上传多媒体文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"> 媒体文件类型,image,voice,video,thumb,news</param>
        /// <returns></returns>
        public static MediaInfo UploadMedia(string file, string type)
        {
            string url = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token + "&type=" + type.ToString();
            var json = Util.HttpUpload(url, file);
            if (json.IndexOf("errcode") > 0)
            {
                var mi = new MediaInfo();
                mi.error = Util.JsonTo<ReturnCode>(json);
                return mi;
            }
            else
            {
                return Util.JsonTo<MediaInfo>(json);
            }
        }

        public static MediaInfo UploadVideoForMess(UploadVideoInfo videoInfo)
        {
            var url = "https://file.api.weixin.qq.com/cgi-bin/media/uploadvideo?access_token=";
            var access_token = GetAccessToken();
            url = url + access_token;
            var json = Util.HttpPost2(url, Util.ToJson(videoInfo));
            if (json.IndexOf("errcode") > 0)
            {
                var mi = new MediaInfo();
                mi.error = Util.JsonTo<ReturnCode>(json);
                return mi;
            }
            else
            {
                return Util.JsonTo<MediaInfo>(json);
            }
        }

        /// <summary>
        /// 上传图文消息素材,用于群发
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public static MediaInfo UploadNews(News news)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token;
            var json = Util.HttpPost2(url, Util.ToJson(news));
            if (json.IndexOf("errcode") > 0)
            {
                var mi = new MediaInfo();
                mi.error = Util.JsonTo<ReturnCode>(json);
                return mi;
            }
            else
            {
                return Util.JsonTo<MediaInfo>(json);
            }
        }

        /// <summary>
        /// 下载多媒体文件
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public static DownloadFile DownloadMedia(string media_id)
        {
            string url = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=";
            string access_token = GetAccessToken();
            url = url + access_token + "&media_id=" + media_id;
            var tup = Util.HttpGet(url);
            var dm = new DownloadFile();
            dm.ContentType = tup.Item2;

            if (tup.Item1 == null)
            {
                dm.error = Util.JsonTo<ReturnCode>(tup.Item3);
            }
            else
            {
                dm.Stream = tup.Item1;
            }
            return dm;
        }
        #endregion

        #region 网页授权获取用户基本信息
        /// <summary>
        /// 得到获取code的Url
        /// </summary>
        /// <param name="redirect">授权后重定向的回调链接地址，请使用urlencode对链接进行处理</param>
        /// <param name="scope">应用授权作用域，snsapi_base （不弹出授权页面，直接跳转，只能获取用户openid），snsapi_userinfo （弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息）</param>
        /// <param name="state">重定向后会带上state参数，开发者可以填写a-zA-Z0-9的参数值</param>
        /// <returns></returns>
        public static string BuildWebCodeUrl(string redirect, string scope, string state = "")
        {
            CheckGlobalCredential();
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect", appConfig.APP_ID, redirect, scope, state);
        }

        /// <summary>
        /// 通过code换取网页授权access_token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static WebCredential GetWebAccessToken(string code)
        {
            CheckGlobalCredential();
            return WebCredential.GetCredential(code);
        }

        /// <summary>
        /// 得到网页授权用户信息
        /// </summary>
        /// <param name="access_token">网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同</param>
        /// <param name="openid">用户的唯一标识</param>
        /// <param name="lang">返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语</param>
        /// <returns></returns>
        public static WebUserInfo GetWebUserInfo(string access_token, string openid, LangType lang)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang={2}", access_token, openid, lang.ToString());

            var json = Util.HttpGet2(url);

            if (json.IndexOf("errcode") > 0)
            {
                var ui = new WebUserInfo();
                ui.error = Util.JsonTo<ReturnCode>(json);
                return ui;
            }
            else
            {
                return Util.JsonTo<WebUserInfo>(json);
            }
        }
        #endregion

        internal static void CheckGlobalCredential()
        {
            if (appConfig == null || string.IsNullOrEmpty(appConfig.APP_ID) || string.IsNullOrEmpty(appConfig.APP_SECRET))
            {
                throw new ArgumentNullException("全局AppID,AppSecret", "请先调用SetGlobalCredential");
            }
        }
    }
}
