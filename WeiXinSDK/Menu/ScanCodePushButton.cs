using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Menu
{
    public class ScanCodePushButton : SingleButton
    {
        public override string type
        {
            get { return "scancode_push"; }
        }
        /// <summary>
        /// scancode_push类型必须.菜单KEY值，用于消息接口推送，不超过128字节
        /// </summary>
        public string key { get; set; }
    }
}
