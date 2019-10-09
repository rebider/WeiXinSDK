﻿
namespace WeiXinSDK.Message
{
    /// <summary>
    /// 发送文本消息
    /// </summary>
    public class SendTextMsg:SendBaseMsg
    {
        public override string msgtype
        {
            get { return "text";}
        }

        public Text text { get; set; }

        public class Text
        {
            /// <summary>
            /// 文本消息内容
            /// </summary>
            public string content { get; set; }
        }
    }
}
