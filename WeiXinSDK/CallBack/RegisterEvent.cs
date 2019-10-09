using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiXinSDK;
using WeiXinSDK.Message;

namespace WeiXinSDK.CallBack
{
    public class RegisterEvent
    {
        public RegisterEvent()
        {
            //点击菜单拉取消息时的事件推送
            WeiXinSDK.MyFunc<EventClickMsg, ReplyBaseMsg> clickhandler = ClickHandler;
            WeiXinSDK.WeiXin.RegisterEventHandler<EventClickMsg>(clickhandler);

            //用户未关注时，扫描二维码进行关注后的事件推送
            WeiXinSDK.MyFunc<EventUserScanMsg, ReplyBaseMsg> scanhandler = ScanHandler;
            WeiXinSDK.WeiXin.RegisterEventHandler<EventUserScanMsg>(scanhandler);

            //用户已关注时扫描二维码的事件推送
            WeiXinSDK.MyFunc<EventFansScanMsg, ReplyBaseMsg> fansscanhandler = FansScanHandler;
            WeiXinSDK.WeiXin.RegisterEventHandler<EventFansScanMsg>(fansscanhandler);

            //普通关注事件
            WeiXinSDK.MyFunc<EventAttendMsg, ReplyBaseMsg> attendhandler = AttendHandler;
            WeiXinSDK.WeiXin.RegisterEventHandler<EventAttendMsg>(attendhandler);

            //取消关注事件
            WeiXinSDK.MyFunc<EventUnattendMsg, ReplyBaseMsg> unattendhandler = UnattendHandler;
            WeiXinSDK.WeiXin.RegisterEventHandler<EventUnattendMsg>(unattendhandler);

            //订单付款通知
            WeiXinSDK.MyFunc<EventMerchantOrderMsg, ReplyBaseMsg> merchantorderhandler = MerchantOrderHandler;
            WeiXinSDK.WeiXin.RegisterEventHandler<EventMerchantOrderMsg>(merchantorderhandler);

            //领取卡券通知
            WeiXinSDK.MyFunc<EventUserGetCardMsg, ReplyBaseMsg> getcardhandler = GetCardHandler;
            WeiXinSDK.WeiXin.RegisterEventHandler<EventUserGetCardMsg>(getcardhandler);

            //删除卡券通知
            WeiXinSDK.MyFunc<EventUserDelCardMsg, ReplyBaseMsg> delcardhandler = DelCardHandler;
            WeiXinSDK.WeiXin.RegisterEventHandler<EventUserDelCardMsg>(delcardhandler);

            //核销卡券通知
            WeiXinSDK.MyFunc<EventUserConsumeCardMsg, ReplyBaseMsg> consumecardhandler = ConsumeCardHandler;
            WeiXinSDK.WeiXin.RegisterEventHandler<EventUserConsumeCardMsg>(consumecardhandler);

            //多客服接入
            WeiXinSDK.MyFunc<EventKfCreateSession, ReplyBaseMsg> ffcreatesessionhandler = KfCreateSessionHandler;
            WeiXinSDK.WeiXin.RegisterEventHandler<EventKfCreateSession>(ffcreatesessionhandler);

            //多客服关闭
            WeiXinSDK.MyFunc<EventKfCloseSession, ReplyBaseMsg> kfclosesessionhandler = KfCloseSessionHandler;
            WeiXinSDK.WeiXin.RegisterEventHandler<EventKfCloseSession>(kfclosesessionhandler);

            //多客服转接
            WeiXinSDK.MyFunc<EventKfSwitchSession, ReplyBaseMsg> kfswitchsessionhandler = KfSwitchSessionHandler;
            WeiXinSDK.WeiXin.RegisterEventHandler<EventKfSwitchSession>(kfswitchsessionhandler);
        }

        /// <summary>
        /// 点击菜单拉取消息时的事件推送 
        /// </summary>
        public virtual ReplyBaseMsg ClickHandler(EventClickMsg msg)
        {
            return GetDefaultMsg();
        }

        /// <summary>
        /// 用户未关注时，扫描二维码进行关注后的事件推送
        /// 1.带参数二维码
        /// 2.非带参数二维码
        /// </summary>
        public virtual ReplyBaseMsg ScanHandler(EventUserScanMsg msg)
        {
            ReplyTextMsg replymsg = new ReplyTextMsg();
            replymsg.Content = "欢迎关注！";
            return replymsg;
        }

        /// <summary>
        /// 用户已关注时扫描带参数二维码的事件推送 
        /// </summary>
        public virtual ReplyBaseMsg FansScanHandler(EventFansScanMsg msg)
        {
            return ReplyEmptyMsg.Instance;
        }

        /// <summary>
        /// 普通关注事件
        /// </summary>
        public virtual ReplyBaseMsg AttendHandler(EventAttendMsg msg)
        {
            ReplyTextMsg replymsg = new ReplyTextMsg();
            replymsg.Content = "欢迎关注！";
            return replymsg;
        }

        /// <summary>
        /// 取消关注事件
        /// </summary>
        public virtual ReplyBaseMsg UnattendHandler(EventUnattendMsg msg)
        {
            return ReplyEmptyMsg.Instance;
        }

        /// <summary>
        /// 订单付款通知
        /// </summary>
        public virtual ReplyBaseMsg MerchantOrderHandler(EventMerchantOrderMsg msg)
        {
            return ReplyEmptyMsg.Instance;
        }

        /// <summary>
        /// 领取卡券通知
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual ReplyBaseMsg GetCardHandler(EventUserGetCardMsg msg)
        {
            return ReplyEmptyMsg.Instance;
        }

        /// <summary>
        /// 删除卡券通知
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual ReplyBaseMsg DelCardHandler(EventUserDelCardMsg msg)
        {
            return ReplyEmptyMsg.Instance;
        }

        /// <summary>
        /// 核销卡券通知
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual ReplyBaseMsg ConsumeCardHandler(EventUserConsumeCardMsg msg)
        {
            return GetDefaultMsg();
        }

        /// <summary>
        /// 多客服接入
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual ReplyBaseMsg KfCreateSessionHandler(EventKfCreateSession msg)
        {
            return GetDefaultMsg();
        }

        /// <summary>
        /// 多客服关闭
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual ReplyBaseMsg KfCloseSessionHandler(EventKfCloseSession msg)
        {
            return GetDefaultMsg();
        }

        /// <summary>
        /// 多客服转接
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual ReplyBaseMsg KfSwitchSessionHandler(EventKfSwitchSession msg)
        {
            return GetDefaultMsg();
        }

        /// <summary>
        /// 默认回复
        /// </summary>
        protected virtual ReplyBaseMsg GetDefaultMsg()
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