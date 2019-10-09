using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using WeiXinSDK.Media;
using WeiXinSDK.Tools.HttpHelper;

namespace WeiXinSDK.Material
{
    public class Material
    {

        #region  永久素材

        /// <summary>
        /// 上传图文消息素材
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public static MaterialResult UploadNews(News news)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/material/add_news?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var json = Util.HttpPost2(url, Util.ToJson(news));
            if (json.IndexOf("errcode") > 0)
            {
                var mi = new MaterialResult();
                mi.error = Util.JsonTo<ReturnCode>(json);
                return mi;
            }
            else
            {
                return Util.JsonTo<MaterialResult>(json);
            }
        }


        /// <summary>
        /// 上传图文消息内的图片获取URL ,图片仅支持jpg/png格式，大小必须在1MB以下
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static NewsPic UploadNewsPic(string file)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/media/uploadimg?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var json = Util.HttpUpload(url, file);
            if (json.IndexOf("errcode") > 0)
            {
                var np = new NewsPic();
                np.error = Util.JsonTo<ReturnCode>(json);
                return np;
            }
            else
            {
                return Util.JsonTo<NewsPic>(json);
            }
        }


        /// <summary>
        /// 上传其它永久素材
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"> 媒体文件类型 (图片（image）、语音（voice）和缩略图（thumb）)</param>
        /// <returns></returns>
        public static MaterialResult UploadMaterial(string file, string type)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/material/add_material?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token + "&type=" + type.ToString();

            string json = Util.HttpUpload(url, file);

            if (json.IndexOf("errcode") > 0)
            {
                var mi = new MaterialResult();
                mi.error = Util.JsonTo<ReturnCode>(json);
                return mi;
            }
            else
            {
                return Util.JsonTo<MaterialResult>(json);
            }
        }

        /// <summary>
        /// 上传视频素材
        /// </summary>
        /// <param name="videoInfo"></param>
        /// <returns></returns>
        public static MaterialResult UploadVideo(string file, string title, string introduction)
        {
            var url = "https://api.weixin.qq.com/cgi-bin/material/add_material?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var description = new { title = title, introduction = introduction };
            var nvc = new NameValueCollection();
            nvc.Add("description", Util.ToJson(description));
            var json = Util.HttpUploadFile(url, file, "media", "application/octet-stream", nvc);
            if (json.IndexOf("errcode") > 0)
            {
                var mi = new MaterialResult();
                mi.error = Util.JsonTo<ReturnCode>(json);
                return mi;
            }
            else
            {
                return Util.JsonTo<MaterialResult>(json);
            }
        }

        /// <summary>
        /// 下载图文消息素材
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public static NewsResult DownloadNews(string media_id)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var post = "{\"media_id\":\"" + media_id + "\"}";
            var json = Util.HttpPost2(url, post);
            if (json.IndexOf("errcode") > 0)
            {
                var rn = new NewsResult();
                rn.error = Util.JsonTo<ReturnCode>(json);
                return rn;
            }
            else
            {
                return Util.JsonTo<NewsResult>(json);
            }
        }
        /// <summary>
        /// 下载视频消息素材
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public static VideoResult DownloadVideo(string media_id)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var post = "{\"media_id\":\"" + media_id + "\"}";
            var json = Util.HttpPost2(url, post);
            if (json.IndexOf("errcode") > 0)
            {
                var rv = new VideoResult();
                rv.error = Util.JsonTo<ReturnCode>(json);
                return rv;
            }
            else
            {
                return Util.JsonTo<VideoResult>(json);
            }
        }
        /// <summary>
        /// 其他类型的素材
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public static DownloadFile DownloadMaterial(string media_id)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var post = "{\"media_id\":\"" + media_id + "\"}";

            var tup = Util.HttpPost(url, post);
            var dm = new DownloadFile();
            dm.ContentType = tup.Item2;

            if (tup.Item1 == null)
            {
                dm.error = Util.JsonTo<ReturnCode>(tup.Item3);
            }
            else
            {
                dm.Stream = tup.Item1;
            }
            return dm;
        }
        /// <summary>
        /// 删除永久素材
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public static ReturnCode DelteMaterial(string media_id)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/material/del_material?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var post = "{\"media_id\":\"" + media_id + "\"}";
            var json = Util.HttpPost2(url, post);
            return Util.JsonTo<ReturnCode>(json);
        }

        /// <summary>
        /// 获取永久素材的总数
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public static MaterialCountResult GetMaterialCount()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/material/get_materialcount?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var json = Util.HttpGet2(url);
            if (json.IndexOf("errcode") > 0)
            {
                var mc = new MaterialCountResult();
                mc.error = Util.JsonTo<ReturnCode>(json);
                return mc;
            }
            else
            {
                return Util.JsonTo<MaterialCountResult>(json);
            }
        }

        /// <summary>
        /// 获取图文素材列表
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static GetNewsListResult GetNewsList(int offset, int count)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var data = new { type = "news", offset = offset, count = count };
            var json = Util.HttpPost2(url, Util.ToJson(data));
            if (json.IndexOf("errcode") > 0)
            {
                var mc = new GetNewsListResult();
                mc.error = Util.JsonTo<ReturnCode>(json);
                return mc;
            }
            else
            {
                return Util.JsonTo<GetNewsListResult>(json);
            }
        }

        /// <summary>
        /// 获取其它永久素材列表
        /// </summary>
        /// <param name="type">图片（image）、语音（voice）和缩略图（thumb）</param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static GetMaterialListResult GetMaterialList(string type, int offset, int count)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token;
            var data = new { type = type, offset = offset, count = count };
            var json = Util.HttpPost2(url, Util.ToJson(data));
            if (json.IndexOf("errcode") > 0)
            {
                var mc = new GetMaterialListResult();
                mc.error = Util.JsonTo<ReturnCode>(json);
                return mc;
            }
            else
            {
                return Util.JsonTo<GetMaterialListResult>(json);
            }
        }
        #endregion
    }
}
