﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Custom
{
    /// <summary>
    /// 客服聊天记录列表
    /// </summary>
    public class GetRecordListResult : ReturnCode
    {
        public int retcode { get; set; }
        /// <summary>
        /// 聊天记录列表
        /// </summary>
        public List<Record> recordlist { get; set; }
    }

    public class Record
    {
        /// <summary>
        /// 客服账号
        /// </summary>
        public string worker { get; set; }

        /// <summary>
        /// 用户的标识，对当前公众号唯一  
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 操作ID（会话状态）
        ///     1000  创建未接入会话  
        ///     1001  接入会话  
        ///     1002  主动发起会话  
        ///     1003  转接会话  
        ///     1004  关闭会话  
        ///     1005  抢接会话  
        ///     2001  公众号收到消息  
        ///     2002  客服发送消息  
        ///     2003  客服收到消息
        /// </summary>
        public int opercode { get; set; }

        /// <summary>
        /// 操作时间，UNIX时间戳 
        /// </summary>
        public long time { get; set; }

        /// <summary>
        /// 聊天记录
        /// </summary>
        public string text { get; set; }
    }
}