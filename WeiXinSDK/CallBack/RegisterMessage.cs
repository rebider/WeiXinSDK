using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using WeiXinSDK;
using WeiXinSDK.Message;

namespace WeiXinSDK.CallBack
{
    /// <summary>
    /// 注册微信公众号注册消息处理程序
    /// </summary>
    public class RegisterMessage
    {
        public RegisterMessage()
        {
            //发送文本消息时的事件推送
            MyFunc<RecTextMsg, ReplyBaseMsg> texthandler = TextHandler;
            WeiXin.RegisterMsgHandler<RecTextMsg>(texthandler);

            //发送图片消息时的事件推送
            MyFunc<RecImageMsg, ReplyBaseMsg> imagehandler = ImageHandler;
            WeiXin.RegisterMsgHandler<RecImageMsg>(imagehandler);

            //发送链接消息时的事件推送
            MyFunc<RecLinkMsg, ReplyBaseMsg> linkhandler = LinkHandler;
            WeiXin.RegisterMsgHandler<RecLinkMsg>(linkhandler);

            //发送位置消息时的事件推送
            MyFunc<RecLocationMsg, ReplyBaseMsg> locationhandler = LocationHandler;
            WeiXin.RegisterMsgHandler<RecLocationMsg>(locationhandler);

            //发送短视频消息时的事件推送
            MyFunc<RecShortVideoMsg, ReplyBaseMsg> shortvideohandler = ShortVideoHandler;
            WeiXin.RegisterMsgHandler<RecShortVideoMsg>(shortvideohandler);

            //发送视频消息时的事件推送
            MyFunc<RecVideoMsg, ReplyBaseMsg> videohandler = VideoHandler;
            WeiXin.RegisterMsgHandler<RecVideoMsg>(videohandler);

            //发送声音消息时的事件推送
            MyFunc<RecVoiceMsg, ReplyBaseMsg> voicehandler = VoiceHandler;
            WeiXin.RegisterMsgHandler<RecVoiceMsg>(voicehandler);
        }

        /// <summary>
        /// 文本消息
        /// </summary>
        public virtual ReplyBaseMsg TextHandler(RecTextMsg msg)
        {
            return GetDefaultMsg();
        }

        /// <summary>
        /// 图片消息
        /// </summary>
        public virtual ReplyBaseMsg ImageHandler(RecImageMsg msg)
        {
            return GetDefaultMsg();
        }

        /// <summary>
        /// 超链接消息
        /// </summary>
        public virtual ReplyBaseMsg LinkHandler(RecLinkMsg msg)
        {
            return GetDefaultMsg();
        }

        /// <summary>
        /// 位置消息
        /// </summary>
        public virtual ReplyBaseMsg LocationHandler(RecLocationMsg msg)
        {
            return GetDefaultMsg();
        }

        /// <summary>
        /// 短视频消息
        /// </summary>
        public virtual ReplyBaseMsg ShortVideoHandler(RecShortVideoMsg msg)
        {
            return GetDefaultMsg();
        }

        /// <summary>
        /// 视频消息
        /// </summary>
        public virtual ReplyBaseMsg VideoHandler(RecVideoMsg msg)
        {
            return GetDefaultMsg();
        }

        /// <summary>
        /// 声音消息
        /// </summary>
        public virtual ReplyBaseMsg VoiceHandler(RecVoiceMsg msg)
        {
            return GetDefaultMsg();
        }

        /// <summary>
        /// 默认回复
        /// </summary>
        protected ReplyBaseMsg GetDefaultMsg()
        {
            ReplyTextMsg replymsg = new ReplyTextMsg();

            if (string.IsNullOrEmpty(replymsg.Content))
            {
                replymsg.Content = "程序猿正在疯狂加班...";
            }
            return replymsg;
        }
    }
}