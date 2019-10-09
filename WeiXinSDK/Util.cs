using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeiXinSDK
{
    public class Tuple<T1, T2, T3>
    {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }
        public T3 Item3 { get; set; }
        public Tuple(T1 t1, T2 t2, T3 t3)
        {
            this.Item1 = t1;
            this.Item2 = t2;
            this.Item3 = t3;
        }
    }


    public class Util
    {
        #region 时间
        /// <summary>
        /// unix时间转换为datetime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeToTime(long timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long TimeToUnixTime(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(time - startTime).TotalSeconds;
        }
        #endregion

        #region Http
        public static Tuple<Stream, string, string> HttpPost(string action, string data)
        {
            HttpWebRequest myRequest = WebRequest.Create(action) as HttpWebRequest;
            myRequest.Method = "POST";
            myRequest.Timeout = 20 * 1000;
            myRequest.ContentType = "application/json;charset=UTF-8";
            var post = Encoding.UTF8.GetBytes(data);
            myRequest.ContentLength = post.Length;
            using (Stream newStream = myRequest.GetRequestStream())
            {
                newStream.Write(post, 0, post.Length);
            }
            HttpWebResponse myResponse = myRequest.GetResponse() as HttpWebResponse;
            var stream = myResponse.GetResponseStream();
            var ct = myResponse.ContentType;

            Stream MyStream = new MemoryStream();
            byte[] buffer = new Byte[4096];
            int bytesRead = 0;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                MyStream.Write(buffer, 0, bytesRead);
            MyStream.Position = 0;

            using (StreamReader sr = new StreamReader(stream))
            {
                var json = sr.ReadToEnd();

                if (ct.IndexOf("errcode") >= 0)
                {
                    return new Tuple<Stream, string, string>(null, ct, json);
                }
                else
                {
                    return new Tuple<Stream, string, string>(MyStream, ct, string.Empty);
                }
            }
        }
        public static Stream HttpPost(string action, byte[] data)
        {
            HttpWebRequest myRequest;
            myRequest = WebRequest.Create(action) as HttpWebRequest;
            myRequest.Method = "POST";
            myRequest.Timeout = 20 * 1000;
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            using (Stream newStream = myRequest.GetRequestStream())
            {
                newStream.Write(data, 0, data.Length);
            }
            HttpWebResponse myResponse = myRequest.GetResponse() as HttpWebResponse;
            return myResponse.GetResponseStream();
        }
        public static string HttpPost2(string action, string data)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            using (var stream = Util.HttpPost(action, buffer))
            {
                StreamReader sr = new StreamReader(stream);
                data = sr.ReadToEnd();
                return data;
            }
        }

        public static string HttpPost2(string action, byte[] data)
        {
            using (var stream = Util.HttpPost(action, data))
            {
                StreamReader sr = new StreamReader(stream);
                return sr.ReadToEnd();
            }
        }

        public static Stream HttpPostXml(string action, byte[] data)
        {
            HttpWebRequest myRequest;
            myRequest = WebRequest.Create(action) as HttpWebRequest;
            myRequest.Method = "POST";
            myRequest.Timeout = 20 * 1000;
            myRequest.ContentType = "text/xml";
            myRequest.ContentLength = data.Length;
            using (Stream newStream = myRequest.GetRequestStream())
            {
                newStream.Write(data, 0, data.Length);
            }
            HttpWebResponse myResponse = myRequest.GetResponse() as HttpWebResponse;
            return myResponse.GetResponseStream();
        }
        public static string HttpPostXml2(string action, string data)
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            using (var stream = Util.HttpPostXml(action, buffer))
            {
                StreamReader sr = new StreamReader(stream);
                data = sr.ReadToEnd();
                return data;
            }
        }

        public static string HttpUpload(string action, string file)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest myRequest = WebRequest.Create(action) as HttpWebRequest;
            myRequest.Method = "POST";
            myRequest.ContentType = "multipart/form-data;boundary=" + boundary;
            StringBuilder sb = new StringBuilder();
            sb.Append("--" + boundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; name=\"media\"; filename=\"" + file + "\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: application/octet-stream");
            sb.Append("\r\n\r\n");
            string head = sb.ToString();
            long length = 0;
            byte[] form_data = Encoding.UTF8.GetBytes(head);
            byte[] foot_data = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            length = form_data.Length + foot_data.Length;

            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                length += fileStream.Length;
                myRequest.ContentLength = length;
                Stream requestStream = myRequest.GetRequestStream();
                requestStream.Write(form_data, 0, form_data.Length);

                byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    requestStream.Write(buffer, 0, bytesRead);
                requestStream.Write(foot_data, 0, foot_data.Length);
            }
            HttpWebResponse myResponse = myRequest.GetResponse() as HttpWebResponse;
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string json = sr.ReadToEnd().Trim();
            sr.Close();
            if (myResponse != null)
            {
                myResponse.Close();
                myRequest = null;
            }
            if (myRequest != null)
            {
                myRequest = null;
            }
            return json;
        }

        public static string HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            string result = string.Empty;
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);

                result = reader2.ReadToEnd();
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }

            return result;
        }

        public static Tuple<Stream, string, string> HttpGet(string action)
        {
            HttpWebRequest myRequest = WebRequest.Create(action) as HttpWebRequest;
            myRequest.Method = "GET";
            myRequest.Timeout = 20 * 1000;
            HttpWebResponse myResponse = myRequest.GetResponse() as HttpWebResponse;
            var stream = myResponse.GetResponseStream();
            var ct = myResponse.ContentType;
            if (ct.IndexOf("json") >= 0 || ct.IndexOf("text") >= 0)
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    var json = sr.ReadToEnd();
                    return new Tuple<Stream, string, string>(null, ct, json);
                }
            }
            else
            {
                Stream MyStream = new MemoryStream();
                byte[] buffer = new Byte[4096];
                int bytesRead = 0;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    MyStream.Write(buffer, 0, bytesRead);
                MyStream.Position = 0;
                return new Tuple<Stream, string, string>(MyStream, ct, string.Empty);
            }
        }

        /// <summary>
        /// 发送HTTP请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="nvc">请求的参数</param>
        /// <returns>请求结果</returns>
        public static string Request(string url, NameValueCollection nvc, string method = "GET", string encoding = "utf-8")
        {
            HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            // 添加header
            foreach (var key in nvc.AllKeys)
            {
                request.Headers.Add(key, nvc.Get(key));
            }
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            Stream s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.GetEncoding(encoding));
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }
            return strValue;
        }

        public static string HttpGet2(string action)
        {
            return Util.HttpGet(action).Item3;
        }
        #endregion

        #region json
        static JsonSerializerSettings GetJSS()
        {
            var jSetting = new JsonSerializerSettings();
            //忽略值为null的
            jSetting.NullValueHandling = NullValueHandling.Ignore;

            return jSetting;
        }
        public static string ToJson(object obj)
        {
            var jss = GetJSS();
            return JsonConvert.SerializeObject(obj, 0, jss);
        }

        public static T JsonTo<T>(string json)
        {
            var jss = GetJSS();
            T obj = JsonConvert.DeserializeObject<T>(json, jss);
            return obj;
        }

        public static bool ContainsKey(string json, string key)
        {
            JObject jObject = JObject.Parse(json);
            if (jObject.Property(key) != null)
                return true;
            return false;
        }

        public static JObject ParseJson(string json)
        {
            JObject jObject = JObject.Parse(json);
            return jObject;
        }

        #endregion

        #region xml
        public static Dictionary<string, string> GetDictFromXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                dict.Add(node.Name, node.InnerText.Trim());
            }
            return dict;
        }
        #endregion
    }
}
