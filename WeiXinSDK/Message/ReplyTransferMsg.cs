using System;
namespace WeiXinSDK.Message
{
    /// <summary>
    /// 消息转发到多客服
    /// </summary>
    public class ReplyTransferMsg : ReplyBaseMsg
    {
        public override string MsgType
        {
            get { return "transfer_customer_service"; }
        }

        /// <summary>
        /// 指定会话接入的客服账号
        /// </summary>
        public string KfAccount { get; set; }

        protected override string GetXMLPart()
        {
            if (!string.IsNullOrEmpty(KfAccount))
                return "<TransInfo><KfAccount><![CDATA[" + KfAccount + "]]></KfAccount></TransInfo>";
            else
                return string.Empty;
        }
    }
}
