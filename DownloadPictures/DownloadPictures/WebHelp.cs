using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DownloadPictures
{
    public class WebHelp
    {
        static WebClient client = new WebClient();
        //调用静态函数触发不了
        //public WebHelp()
        //{//编码
        //    client.Encoding = Encoding.GetEncoding("gb2312");
        //    //client.Headers.Add("User-Agent: Other");
        //    client.Headers.Add("Cache-Control", "no-cache");
        //}
        /// <summary>
        /// 设置client属性
        /// </summary>
        public static void Set()
        {
            client.Encoding = Encoding.GetEncoding("gb2312");
            client.Headers.Add("User-Agent: Other");
            //client.Headers.Add("Cache-Control", "no-cache");
        }
        /// <summary>
        /// 下载网页
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtmlPage(string url)
        {
            return client.DownloadString(url);
        }
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        public static bool DownLoadImage(string url, string fileName)
        {
            //client.DownloadFile(url, fileName);
            // return true;
            try
            {
                client.DownloadFile(url, fileName);
                return true;
            }
            catch
            {//验证一次
                try
                {
                    client.DownloadFile(url, fileName);
                    return true;
                }
                catch
                {

                    return false;
                }

            }

        }

        //使用失败，可能是post的差数设置有误
        /// <summary>
        /// Post请求网页
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters">参数</param>
        /// <returns>pageString</returns>
        public static string GetHtmlPagePost(string url, string parameters)
        {
            using (WebClient wc = new WebClient())
            {

                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                return wc.UploadString(url, parameters);
            }
        }


    }
}
