using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DownloadPictures
{
    class Program
    {

        static void Main(string[] args)
        {

            //下载失败的原因
            //1.url解析错误
            //2.网站缺少资源获取或者资源有问题（用浏览器打开也无法加载图片）
            //3.304 问题没有解决

            //False 并不一定是应为图片下载失败，也会在本章只有24张图，尝试下载第25张时出现
            while (true)
            {
                Console.Write("请输入要搜索的动漫：");//不能为空格
                string name = Console.ReadLine();
                if (name.Replace(" ","")=="")
                {
                    Console.WriteLine("输入不能为空或空格");
                    continue;
                }
                Console.WriteLine("正在搜索漫画...");
                int lenList = ComicsHelp.GetComicsInfo(name);
                if (lenList<=0)
                {
                    Console.WriteLine("很抱歉！没能找到漫画。");
                    continue;
                }
                int index = 0;
                string str;
                while (true)
                {
                    Console.Write("请输入要下载的动漫的索引(输入#返回起始位置）：");
                    str = Console.ReadLine();

                    if (str == "#")
                    {
                        break;
                    }
                    //验证
                    if (!int.TryParse(str, out index))
                    {
                        Console.WriteLine("无效索引！");
                        continue;
                    }


                    if (index < 0 || index >= lenList)
                    {
                        Console.WriteLine("索引为负或索引太大");
                        continue;
                    }
                    break;
                }
                if (str == "#")
                {
                    continue;
                }
                int lenChapter = ComicsHelp.GetOneComics(index);
                //当chapter小于5且资源有问题时可能会报错 判断下
                Console.WriteLine("正在构造url...");
                int j;
                int num = lenChapter >= 5 ? 5 : lenChapter;
                for (j = 0; j < num; j++)
                {
                    if (!(ComicsHelp.CheckUrl(lenChapter-1- j) == "/1/"))
                    {
                        break;
                    }
                }
                if (j >= 5)
                {
                    Console.WriteLine("抱歉！url造失败，请重新下载，或另选资源。");
                    continue;
                }
                else
                {
                    Console.Write("url构造成功，输入任意键开始下载（输入#返回起始位置）：");
                    if (Console.ReadLine() == "#")
                    {
                        continue;
                    }
                }
                ComicsHelp.GetFrontOfImageUrls();

                Stopwatch sp = new Stopwatch();
                sp.Start();
                ComicsHelp.DoadloadComics();
                sp.Stop();
                Console.WriteLine("耗时：" + sp.ElapsedMilliseconds.ToString() + "ms");

                Console.ReadKey();
            }




        }

    }
}
