
namespace WeiXinSDK.Message
{
    /// <summary>
    /// 接收的短视频消息
    /// </summary>
    public class RecShortVideoMsg : RecBaseMsg
    {
        /// <summary>
        /// 视频消息媒体id
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 视频消息缩略图的媒体id
        /// </summary>
        public string ThumbMediaId { get; set; }

        public override string MsgType
        {
            get { return "shortvideo"; }
        }
    }
}
