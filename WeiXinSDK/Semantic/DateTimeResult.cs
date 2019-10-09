using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Semantic
{
    public class DateTimeResult
    {
        /// <summary>
        /// 大类型：“DT_SINGLE” 。 
        ///     DT_SINGLE又细分为两个类别：DT_ORI和DT_INFER。 
        ///     DT_ORI 是字面时间，比如：“上午九点”；
        ///     DT_INFER 是推理时间，比如：“提前 5 分钟”
        /// --------------------------------------------------
        /// 类型：“DT_INTERVAL”
        /// --------------------------------------------------
        /// 类型：“DT_REPEAT” 
        ///     DT_REPEAT 又细分为两个类别：DT_RORI和DT_RINFER。 
        ///     DT_RORI 是字面时间，比如：“每天上午九点”；
        ///     DT_RINFER 是推理时间，比如：“工作日除外” 
        /// </summary>
        public string type { get; set; }
    }
    /// <summary>
    /// 单时间描述协议
    /// </summary>
    public class SingleDtResult : DateTimeResult
    {
        /// <summary>
        /// 格式：YYYY-MM-DD，默认是当天时间 
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// date 的原始字符串
        /// </summary>
        public string date_ori { get; set; }

        /// <summary>
        /// 24 小时制，格式：HH:MM:SS，默认为00:00:00 
        /// </summary>
        public string time { get; set; }

        /// <summary>
        /// time 的原始字符串
        /// </summary>
        public string time_ori { get; set; }
    }

    /// <summary>
    /// 时间段描述协议
    /// </summary>
    public class IntervalDtResult : SingleDtResult
    {
        /// <summary>
        /// 格式：YYYY-MM-DD，默认是当天时间 
        /// </summary>
        public string end_date { get; set; }

        /// <summary>
        /// date 的原始字符串
        /// </summary>
        public string end_date_ori { get; set; }

        /// <summary>
        /// 24 小时制，格式：HH:MM:SS，默认为00:00:00 
        /// </summary>
        public string end_time { get; set; }

        /// <summary>
        /// time 的原始字符串
        /// </summary>
        public string end_time_ori { get; set; }
    }

    /// <summary>
    /// 重复时间描述协议
    /// </summary>
    public class RepeatDtResult : DateTimeResult
    {
        /// <summary>
        ///  重复标记：0000000 
        ///  注：依次代表周日，周六，…，周一；
        ///  1 表示该天要重复，0表示不重复 
        /// </summary>
        public string repeat { get; set; }

        /// <summary>
        /// repeat 的原始字符串
        /// </summary>
        public string repeat_ori { get; set; }

        /// <summary>
        /// 24 小时制，格式：HH:MM:SS，默认为00:00:00 
        /// </summary>
        public string time { get; set; }

        /// <summary>
        /// time 的原始字符串
        /// </summary>
        public string time_ori { get; set; }
    }

    public enum DateTimeType
    {
        DT_SINGLE,//单时间
        DT_ORI,//字面时间
        DT_INFER,//推理时间
        DT_INTERVAL,//时间段
        DT_REPEAT,//重复时间
        DT_RORI,//字面时间
        DT_RINFER//推理时间
    }
}
