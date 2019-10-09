using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinSDK.Media
{
    public class Media
    {
        #region 临时素材

        /// <summary>
        /// 上传多媒体文件(临时素材)
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"> 媒体文件类型 image(2M),voice(2M),video(10M),thumb(64K),news</param>
        /// <returns></returns>
        public static MediaInfo UploadMedia(string file, string type)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/media/upload?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token + "&type=" + type.ToString();
            var json = Util.HttpUpload(url, file);
            if (json.IndexOf("errcode") > 0)
            {
                var mi = new MediaInfo();
                mi.error = Util.JsonTo<ReturnCode>(json);
                return mi;
            }
            else
            {
                return Util.JsonTo<MediaInfo>(json);
            }
        }

        /// <summary>
        /// 下载多媒体文件(临时素材)
        /// </summary>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public static DownloadFile DownloadMedia(string media_id)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/media/get?access_token=";
            string access_token = WeiXin.GetAccessToken();
            url = url + access_token + "&media_id=" + media_id;
            var tup = Util.HttpGet(url);
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

        #endregion
    }
}
