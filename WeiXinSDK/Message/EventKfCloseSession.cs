
namespace WeiXinSDK.Message
{
    /// <summary>
    /// 多客服关闭会话事件
    /// </summary>
    public class EventKfCloseSession : EventBaseMsg
    {
        public override string Event
        {
            get { return "kf_close_session"; }
        }

        /// <summary>
        /// 客服账号
        /// </summary>
        public string KfAccount { get; set; }
    }
}
