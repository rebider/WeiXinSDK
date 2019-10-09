using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Custom
{
    /// <summary>
    /// 关闭会话
    /// </summary>
    public class CloseSessionResult : ReturnCode
    {
        /// <summary>
        /// 正在接待的客服，为空表示没有人在接待
        /// </summary>
        public string kf_account { get; set; }

        /// <summary>
        /// 正在接待的客服，为空表示没有人在接待
        /// </summary>
        public long createtime { get; set; }
    }
}
