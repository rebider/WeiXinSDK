using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiXinSDK;

namespace WeiXinSDK.Custom
{
    /// <summary>
    /// 客服管理
    /// </summary>
    public class CustomService
    {
        #region 客服管理
        /// <summary>
        /// 获取客服列表
        /// </summary>
        /// <returns></returns>
        public static GetCustomListResult GetCustomList()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/customservice/getkflist?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;

            var json = Util.HttpGet2(url);

            if (Util.ContainsKey(json, "errcode"))
            {
                var cl = new GetCustomListResult();
                cl.error = Util.JsonTo<ReturnCode>(json);
                return cl;
            }
            else
            {
                return Util.JsonTo<GetCustomListResult>(json);
            }
        }

        /// <summary>
        /// 获取在线客服接待信息列表
        /// </summary>
        /// <returns></returns>
        public static GetOnlineCustomListResult GetOnlineCustomList()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/customservice/getonlinekflist?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;

            var json = Util.HttpGet2(url);

            if (Util.ContainsKey(json, "errcode"))
            {
                var ocl = new GetOnlineCustomListResult();
                ocl.error = Util.JsonTo<ReturnCode>(json);
                return ocl;
            }
            else
            {
                return Util.JsonTo<GetOnlineCustomListResult>(json);
            }
        }

        /// <summary>
        /// 添加客服账号
        /// </summary>
        /// <returns></returns>
        public static ReturnCode AddCustomAccount(CustomAccount account)
        {
            string url = "https://api.weixin.qq.com/customservice/kfaccount/add?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;

            var json = Util.HttpPost2(url, Util.ToJson(account));
            return Util.JsonTo<ReturnCode>(json);
        }

        /// <summary>
        /// 设置客服信息
        /// </summary>
        /// <returns></returns>
        public static ReturnCode UpdateCustomAccount(CustomAccount account)
        {
            string url = "https://api.weixin.qq.com/customservice/kfaccount/update?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;

            var json = Util.HttpPost2(url, Util.ToJson(account));
            return Util.JsonTo<ReturnCode>(json);
        }

        /// <summary>
        /// 上传客服头像(头像图片文件必须是jpg格式，推荐使用640*640大小的图片以达到最佳效果)
        /// </summary>
        /// <param name="kf_account">客服账号</param>
        /// <param name="file">头像文件</param>
        /// <returns></returns>
        public static ReturnCode UploadCustomHeadimg(string kf_account, string file)
        {
            string url = "http://api.weixin.qq.com/customservice/kfaccount/uploadheadimg?access_token={0}&kf_account={1}";
            string access_token = WeiXin.GetAccessToken();
            url = string.Format(url, access_token, kf_account);

            var json = Util.HttpUpload(url, file);
            return Util.JsonTo<ReturnCode>(json);
        }

        public static ReturnCode DelCustomAccount(string kf_account)
        {
            string url = "http://api.weixin.qq.com/customservice/kfaccount/uploadheadimg?access_token={0}&kf_account={1}";
            string access_token = WeiXin.GetAccessToken();
            url = string.Format(url, access_token, kf_account);

            var json = Util.HttpGet2(url);
            return Util.JsonTo<ReturnCode>(json);
        }
        #endregion

        #region 多会话控制
        /// <summary>
        /// 创建会话
        ///     开发者可以使用本接口，为多客服的客服工号创建会话，将某个客户直接指定给客服工号接待，
        ///     需要注意此接口不会受客服自动接入数以及自动接入开关限制。
        ///     只能为在线的客服（PC客户端在线，或者已绑定多客服助手）创建会话。 
        /// </summary>
        /// <param name="kf_account">完整客服账号，格式为：账号前缀@公众号微信号</param>
        /// <param name="openid">客户openid </param>
        /// <param name="text">附加信息,文本会展示在客服人员的多客服客户端（可选）</param>
        /// <returns></returns>
        public static ReturnCode CreateCustomSession(string kf_account, string openid, string text = "")
        {
            string url = "https://api.weixin.qq.com/customservice/kfaccount/add?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;

            var data = new
            {
                kf_account = kf_account,
                openid = openid,
                text = text
            };

            var json = Util.HttpPost2(url, Util.ToJson(data));
            return Util.JsonTo<ReturnCode>(json);
        }

        /// <summary>
        /// 关闭会话
        /// </summary>
        /// <param name="kf_account">完整客服账号，格式为：账号前缀@公众号微信号</param>
        /// <param name="openid">客户openid </param>
        /// <param name="text">附加信息,文本会展示在客服人员的多客服客户端（可选）</param>
        /// <returns></returns>
        public static CloseSessionResult CloseSession(string kf_account, string openid, string text = "")
        {
            string url = " https://api.weixin.qq.com/customservice/kfsession/close?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;

            var data = new
            {
                kf_account = kf_account,
                openid = openid,
                text = text
            };

            var json = Util.HttpPost2(url, Util.ToJson(data));
            return Util.JsonTo<CloseSessionResult>(json);
        }


        /// <summary>
        /// 获取客服的会话列表
        /// </summary>
        /// <returns></returns>
        public static GetSessionListResult GetSessionList(string kf_account)
        {
            string url = "https://api.weixin.qq.com/customservice/kfsession/getsessionlist?access_token={0}&kf_account={1}";
            string access_token = WeiXin.GetAccessToken();
            url = string.Format(url, access_token, kf_account);

            var json = Util.HttpGet2(url);

            if (Util.ContainsKey(json, "errcode"))
            {
                var sl = new GetSessionListResult();
                sl.error = Util.JsonTo<ReturnCode>(json);
                return sl;
            }
            else
            {
                return Util.JsonTo<GetSessionListResult>(json);
            }
        }
        /// <summary>
        /// 获取未接入会话列表
        ///     获取当前正在等待队列中的会话列表，此接口最多返回最早进入队列的100个未接入会话。 
        /// </summary>
        /// <returns></returns>
        public static GetWaitCaseListResult GetWaitCaseList()
        {
            string url = "https://api.weixin.qq.com/customservice/kfsession/getwaitcase?access_token={0}";
            string access_token = WeiXin.GetAccessToken();
            url = string.Format(url, access_token);

            var json = Util.HttpGet2(url);

            if (Util.ContainsKey(json, "errcode"))
            {
                var wcl = new GetWaitCaseListResult();
                wcl.error = Util.JsonTo<ReturnCode>(json);
                return wcl;
            }
            else
            {
                return Util.JsonTo<GetWaitCaseListResult>(json);
            }
        }
        #endregion

        #region 客服聊天记录
        /// <summary>
        /// 获取客服聊天记录
        /// </summary>
        /// <param name="pageindex">查询第几页，从1开始 </param>
        /// <param name="pagesize">每页大小，每页最多拉取50条 </param>
        /// <param name="starttime">查询开始时间，UNIX时间戳 </param>
        /// <param name="endtime">查询结束时间，UNIX时间戳，每次查询不能跨日查询</param>
        /// <returns></returns>
        public static GetRecordListResult GetRecordList(int pageindex, int pagesize, long starttime, long endtime)
        {
            string url = " https://api.weixin.qq.com/customservice/msgrecord/getrecord?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;

            var data = new
            {
                pageindex = pageindex,
                pagesize = pagesize,
                starttime = starttime,
                endtime = endtime
            };

            var json = Util.HttpPost2(url, Util.ToJson(data));
            return Util.JsonTo<GetRecordListResult>(json);
        }
        #endregion
    }
}
