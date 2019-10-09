
namespace WeiXinSDK.Message
{
    /// <summary>
    /// 多客服转接会话事件
    /// </summary>
    public class EventKfSwitchSession : EventBaseMsg
    {
        public override string Event
        {
            get { return "kf_switch_session"; }
        }
        /// <summary>
        /// 转接前客服账号
        /// </summary>
        public string FromKfAccount { get; set; }

        /// <summary>
        /// 转接后客服账号
        /// </summary>
        public string ToKfAccount { get; set; }
    }
}
