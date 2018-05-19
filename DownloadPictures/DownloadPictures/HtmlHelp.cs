using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DownloadPictures
{
    public class HtmlHelp
    {



        /// <summary>
        /// 发送post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">要post的参数 格式"name=zhen&pass=1234"</param>
        /// <param name="encode">编码</param>
        /// <returns></returns>
        public static string Post(string url, string postData, Encoding encode)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] buffer = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            request.MaximumAutomaticRedirections = 1;
            request.AllowAutoRedirect = true;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResposeStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResposeStream, encode);
            string htmlString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResposeStream.Close();
            return htmlString;

        }

        private static readonly HttpClient client = new HttpClient();
        /// <summary>
        /// 异步发送post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="values">post要提交的参数</param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, Dictionary<string, string> values)
        {
            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(url, content);

            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}
