using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Semantic
{
    public enum ServiceType
    {
        #region 生活类
        /// <summary>
        /// 餐馆
        /// </summary>
        restaurant,
        /// <summary>
        /// 地图
        /// </summary>
        map,
        /// <summary>
        /// 周边
        /// </summary>
        nearby,
        /// <summary>
        /// 优惠券/团购
        /// </summary>
        coupon,
        #endregion
        #region 旅游类
        /// <summary>
        /// 酒店
        /// </summary>
        hotel,
        /// <summary>
        /// 旅游
        /// </summary>
        travel,
        /// <summary>
        /// 航班
        /// </summary>
        flight,
        /// <summary>
        /// 火车
        /// </summary>
        train,
        #endregion
        #region 娱乐类
        /// <summary>
        /// 上映电影
        /// </summary>
        movie,
        /// <summary>
        /// 音乐
        /// </summary>
        music,
        /// <summary>
        /// 视频
        /// </summary>
        video,
        /// <summary>
        /// 小说
        /// </summary>
        novel,
        #endregion
        #region 工具类
        /// <summary>
        /// 天气
        /// </summary>
        weather,
        /// <summary>
        /// 股票
        /// </summary>
        stock,
        /// <summary>
        /// 提醒
        /// </summary>
        remind,
        /// <summary>
        /// 常用电话
        /// </summary>
        telephone,
        #endregion
        #region 知识类
        /// <summary>
        /// 菜谱
        /// </summary>
        cookbook,
        /// <summary>
        /// 百科
        /// </summary>
        baike,
        /// <summary>
        /// 资讯
        /// </summary>
        news,
        #endregion
        #region 其它类
        /// <summary>
        /// 电视节目预告
        /// </summary>
        tv,
        /// <summary>
        /// 通用指令
        /// </summary>
        instruction,
        /// <summary>
        /// 电视指令
        /// </summary>
        tv_instruction,
        /// <summary>
        /// 车载指令
        /// </summary>
        car_instruction,
        /// <summary>
        /// 应用
        /// </summary>
        app,
        /// <summary>
        /// 网址
        /// </summary>
        website,
        /// <summary>
        /// 网页搜索
        /// </summary>
        search
        #endregion
    }
}
