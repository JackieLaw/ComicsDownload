using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;


namespace DownloadPictures
{
    public class ComicsHelp
    {
        static string[] nameArr;
        static string[] urlArr;
        static string[] nameArrOne;
        static string[] urlArrOne;

        static string[] imageFrontUrlEn;
        static int nameIndex;

        static string baseUrl = "http://www.mh160.com";
        static string imageBaseUrl5 = "http://mhpic5.lineinfo.cn/mh160tuku";
        static string imageBaseUrl6 = "http://mhpic6.lineinfo.cn/mh160tuku";
        static string imageBaseUrl7 = "http://mhpic7.lineinfo.cn/mh160tuku";
        static string partOfUrl = "/a/";
        static Encoding en = Encoding.GetEncoding("gb2312");
        /// <summary>
        /// 搜索漫画
        /// </summary>
        /// <param name="comicsName">漫画名</param>
        /// <returns>获取的html页面</returns>
        public static string SearchComics(string comicsName)
        {
            string par = HttpUtility.UrlEncode(comicsName, en).ToUpper();

            string parameters = "key=" + par + "&button=%CB%D1%CB%F7%C2%FE%BB%AD";
            string comicsUrl = baseUrl + "/e/search/";

            return HtmlHelp.Post(comicsUrl, parameters, en);

        }
        /// <summary>
        /// 获取漫画列表
        /// </summary>
        /// <param name="comicsName">漫画名</param>
        /// <returns>漫画列表的长度</returns>
        public static int GetComicsInfo(string comicsName)
        {
            string page = SearchComics(comicsName);
            string inStr1 = "漫画关键词：";
            string inStr2 = "footer\">漫画160为您提供";

            string nameRe = "alt=\"([^\"]*)\" />";
            string urlRe = "<dt><a href=\"([^\"]*)\"";
            GetUrls(page, inStr1, inStr2, nameRe, urlRe, out nameArr, out urlArr);
            return nameArr.Length;


        }
        /// <summary>
        /// 获取制定漫画的章节名和对应的url
        /// </summary>
        /// <param name="index">漫画索引</param>
        /// <returns>章节列表的长度</returns>
        public static int GetOneComics(int index)
        {

            string OneComicsUrl = baseUrl + urlArr[index];
            nameIndex = index;
            string page = WebHelp.GetHtmlPage(OneComicsUrl);
            //Console.WriteLine(page);
            //Console.ReadKey();
            string inStr1 = "class=\"new\"";
            string inStr2 = "class=\"blank_8\"";
            string nameRe = "title=\"([^\"]*)\"";
            string urlRe = "href=\"([^\"]*)\"";
            //搞反了。。。
            //string nameRe = "href=\"(.{20,30}[.]html)\""; 
            //string urlRe = "title=\"(.{0,40})\" target=";
            GetUrls(page, inStr1, inStr2, nameRe, urlRe, out nameArrOne, out urlArrOne);

            return nameArrOne.Length;
        }
        /// <summary>
        /// 获取漫画(章节）名及其对应的url
        /// </summary>
        /// <param name="htmlPage">网页</param>
        /// <param name="inStr1">切割字符串 头部</param>
        /// <param name="inStr2">切割字符串 尾部</param>
        /// <param name="nameRe">匹配漫画（章节）名的正则</param>
        /// <param name="urlRe">匹配url的正则</param>
        /// <param name="nameArr1">用于存储漫画（章节）名</param>
        /// <param name="urlArr1">用于存储url</param>
        public static void GetUrls(string htmlPage, string inStr1, string inStr2,
            string nameRe, string urlRe, out string[] nameArr1, out string[] urlArr1)
        {

            int index1 = htmlPage.IndexOf(inStr1);
            int index2 = htmlPage.IndexOf(inStr2);
            string pageSub = htmlPage.Substring(index1, index2 - index1);

            MatchCollection nameMc = Regex.Matches(pageSub, nameRe);
            MatchCollection urlMc = Regex.Matches(pageSub, urlRe);
            int len = nameMc.Count;
            nameArr1 = new string[len];
            urlArr1 = new string[len];
            int num1 = 0;
            int num2 = 0;
            foreach (Match item in nameMc)
            {                                         //可能还有其他的字符需要替换下
                nameArr1[num1] = item.Groups[1].Value.Replace('，','-').Replace('！','-');
                num1++;
            }
            foreach (Match item in urlMc)
            {
                urlArr1[num2] = item.Groups[1].Value;
                num2++;
            }
            for (int i = 0; i < len; i++)
            {
                Console.WriteLine(i + "." + nameArr1[i]);
            }

        }
        /// <summary>
        /// 获取图片url的前半部分
        /// </summary>
        public static void GetFrontOfImageUrls()
        {
            int len = nameArrOne.Length;

            imageFrontUrlEn = new string[len];
            string str1 = nameArr[nameIndex];
            string str2;
            MatchCollection mc;
            string num1 = "";
            string num2 = "";

            string url2;
            string imageBaseUrl;
            for (int i = 0; i < len; i++)
            {

                str2 = nameArrOne[i].Replace(" ", "");
                string str3 = urlArrOne[i];

                Console.WriteLine(str2);
                mc = Regex.Matches(urlArrOne[i], "kanmanhua/(\\d{1,10})/(\\d{1,10})[.]html");
                foreach (Match item in mc)
                {
                    num1 = item.Groups[1].Value;
                    num2 = item.Groups[2].Value;
                }
                if (int.Parse(num2) > 542724)
                {

                    imageBaseUrl = imageBaseUrl5;

                }
                else if (int.Parse(num1) > 10000)
                {

                    imageBaseUrl = imageBaseUrl6;
                }
                else
                {

                    imageBaseUrl = imageBaseUrl7;
                }

                //此处编码 不用 gb2312 
                url2 = imageBaseUrl + partOfUrl + HttpUtility.UrlEncode(str1).ToUpper() + "_" + num1 + "/"
                    + HttpUtility.UrlEncode(str2).ToUpper() + "_" + num2 + "/";

                imageFrontUrlEn[i] = url2;


            }

        }
        
        //待改进
        /// <summary>
        /// 获取图片url的/a/部分
        /// </summary>
        /// <param name="chapter">章节索引</param>
        /// <returns></returns>
        public static string CheckUrl(int chapter)
        {
            string num1 = "";
            string num2 = "";
            string imageBaseUrl;
            string url;
            string check = "abcdefghijklmnopqrstuvwxyz1";
            int num = 0;
            MatchCollection mc = Regex.Matches(urlArrOne[chapter], "kanmanhua/(\\d{1,10})/(\\d{1,10})[.]html");
            foreach (Match item in mc)
            {
                num1 = item.Groups[1].Value;
                num2 = item.Groups[2].Value;
            }
            if (int.Parse(num2) > 542724)
            {

                imageBaseUrl = imageBaseUrl5;

            }
            else if (int.Parse(num1) > 10000)
            {

                imageBaseUrl = imageBaseUrl6;
            }
            else
            {

                imageBaseUrl = imageBaseUrl7;
            }
            url = imageBaseUrl + "/" + check[num] + "/" + HttpUtility.UrlEncode(nameArr[nameIndex]).ToUpper() + "_" + num1 + "/"
                   + HttpUtility.UrlEncode(nameArrOne[chapter].Replace(" ", "")).ToUpper() + "_" + num2 + "/" + "0001.jpg";
            if (!WebHelp.DownLoadImage(url, "check.jpg"))
            {
                for (int i = 1; i < check.Length; i++)
                {//注意了
                    url = url.Replace("/" + check[i - 1] + "/", "/" + check[i] + "/");
                    num++;
                    //Console.WriteLine(url);
                    //Console.WriteLine(HttpUtility.UrlDecode(url));
                    if (WebHelp.DownLoadImage(url, "check.jpg"))
                    {
                        break;
                    }

                }
            }
            partOfUrl = "/" + check[num].ToString() + "/";
            return partOfUrl;

        }
        public static void DoadloadComics()
        {
            Console.WriteLine("开始下载漫画：" + nameArr[nameIndex]);
            string name;
            string directory;
            bool bl;
            int num = 1;

            for (int i = 0; i < imageFrontUrlEn.Length; i++)
            {
                do
                {
                    name = num < 10 ? "000" + num + ".jpg" : "00" + num + ".jpg";
                    //Console.WriteLine(imageFrontUrlEn[i] + name);
                    directory = "comics\\" + nameArr[nameIndex] + "\\" + nameArrOne[i] /*+ "\\" + name*/;
                    if (num == 1)
                    {
                        Directory.CreateDirectory(directory);
                    }
                    Console.WriteLine(directory + "\\" + name);
                    bl = WebHelp.DownLoadImage(imageFrontUrlEn[i] + name,
                        directory + "\\" + name);
                    Console.WriteLine(bl);
                    num++;
                } while (bl);

                num = 1;
            }
            Console.WriteLine("下载完成");
        }

    }
}