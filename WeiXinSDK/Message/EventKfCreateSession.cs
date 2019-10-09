
namespace WeiXinSDK.Message
{
    /// <summary>
    /// 多客服接入会话事件
    /// </summary>
    public class EventKfCreateSession : EventBaseMsg
    {
        public override string Event
        {
            get { return "kf_create_session"; }
        }

        /// <summary>
        /// 客服账号
        /// </summary>
        public string KfAccount { get; set; }
    }
}
