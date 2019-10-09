using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Material
{
    public class ModifyNews : NewsResult
    {
        /// <summary>
        /// 要修改的图文消息的id 
        /// </summary>
        public string media_id { get; set; }
        /// <summary>
        /// 要更新的文章在图文消息中的位置（多图文消息时，此字段才有意义），第一篇为0 
        /// </summary>
        public int index { get; set; }
    }
}
