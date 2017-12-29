using Grad_Proxy.Common;
using Grad_Proxy.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Grad_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        List<WeiPublicEntity> weiPublickList = new List<WeiPublicEntity>();

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                this.parameters();
            }
            catch (Exception ex)
            {
            }
        }

        public void parameters()
        {
            try
            {
                //var da = GetTime("1474607572");

                List<string> htmlUrlList = GetHtmlUri();

                if (htmlUrlList != null && htmlUrlList.Count > 1)
                {
                    new Thread(() =>
            {
                GetEntityList(htmlUrlList[0]);
            }) { IsBackground = true }.Start();
                    new Thread(() =>
            {
                GetEntityList(htmlUrlList[1]);
            }) { IsBackground = true }.Start();
                }
            }
            catch (Exception ex)
            {


            }
        }

        /// <summary>
        /// 时间戳的随机数
        /// </summary>
        /// <returns></returns>
        public static string timeStamp(string defaultTimer)
        {
            string timeStr = null;
            try
            {
                DateTime dt1 = Convert.ToDateTime(defaultTimer);
                TimeSpan ts = DateTime.Now - dt1;
                timeStr = Math.Ceiling(ts.TotalSeconds).ToString();
            }
            catch (Exception ex)
            {

            }
            return timeStr;
        }


        /// <summary>  
        /// 时间戳转为C#格式时间  
        /// </summary>  
        /// <param name="timeStamp">Unix时间戳格式</param>  
        /// <returns>C#格式时间</returns>  
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = default(DateTime);
            TimeSpan toNow = default(TimeSpan);
            try
            {
                dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long lTime = long.Parse(timeStamp + "0000000");
                 toNow = new TimeSpan(lTime);
            }
            catch (Exception ex)
            {


            }
            return dtStart.Add(toNow);
        }


        public List<string> GetHtmlUri()
        {
            List<string> list = new List<string>() { };
            try
            {
                String html = HtmlTag.GetHtmlinfo("http://weixin.sogou.com/weixin?type=1&query=%E5%8C%97%E4%BA%AC%E7%AC%AC%E4%BA%8C%E5%A4%96%E5%9B%BD%E8%AF%AD%E5%AD%A6%E9%99%A2&ie=utf8&_sug_=y&_sug_type_=&w=01019900&sut=8621&sst0=1474615736417&lkt=0%2C0%2C0");
                List<HtmlTag> tags = HtmlTag.FindTagByAttr(html, "div", "class", "results mt7");
                if (tags.Count > 0)
                {
                    List<HtmlTag> item_tags = tags[0].FindTagByAttr("div", "target", "_blank");
                    foreach (HtmlTag item_tag in item_tags)
                    {
                        string htmlUri = item_tag.GetAttribute("href");
                        list.Add(htmlUri.Replace("amp;", string.Empty));
                    }
                }
                else
                {
                    var list1 = System.Configuration.ConfigurationManager.AppSettings["list1"];
                    var list2 = System.Configuration.ConfigurationManager.AppSettings["list2"];
                    list.Add(list1);
                    list.Add(list2);
                }
            }
            catch (Exception ex)
            {
            }

            return list;
        }


        public void GetEntityList(string htmlUri)
        {
            try
            {
                weiPublickList.Clear();
                String html = HtmlTag.GetHtmlinfo(htmlUri);


                //List<HtmlTag> tags1 = HtmlTag.FindTagByAttr(html, "div", "id", "history");
                //var match =   regex.Matches(html);
                List<HtmlTag> tags = HtmlTag.FindTagByAttr(html, "script", "type", "text/javascript");

                string content = tags[tags.Count - 1].InnerHTML;
                string pattern = "var msgList = '{";
                string tt = content.Substring(content.IndexOf(pattern) + pattern.Length);

                string[] sArr = tt.Split(new string[] { "&quot;title&quot" }, StringSplitOptions.RemoveEmptyEntries);
                //string[] pse = tt.Split(tt, @"&quot;,&quot;digest&quot;:&quot;" , RegexOptions.IgnoreCase);

                for (int i = 0; i < sArr.Length; i++)
                {
                    sArr[i] = sArr[i].Replace("&quot", "");
                    sArr[i] = sArr[i].Replace("\\\\/", "/");


                    string[] dsc = sArr[i].Split(new string[] { ";,;" }, StringSplitOptions.RemoveEmptyEntries);

                    if (i != 0)
                    {

                        WeiPublicEntity entity = new WeiPublicEntity();

                        for (int j = 0; j < dsc.Count(); j++)
                        {

                            if (j == 0)
                            {
                                entity.Title = dsc[j].Replace(";:;", string.Empty);
                            }
                            else if (j == 1)
                            {

                                entity.Content = dsc[j].Replace("digest;:;", string.Empty).Replace("&nbsp;", string.Empty).Replace("\\\\n", string.Empty);
                            }
                            else if (j == 3)
                            {
                                string P1 = dsc[j].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[1];
                                entity.SourseUrl = "http://mp.weixin.qq.com/" + P1.Replace("amp;", string.Empty);
                            }
                            else if (j == 5)
                            {
                                string P2 = dsc[j].Split(new string[] { "cover;:;" }, StringSplitOptions.RemoveEmptyEntries)[0];
                                //P2 = P2.Replace("\"\"", "\\");

                                entity.ImageUrl = P2;

                            }
                        }
                        this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                WeiXin_Item III = new WeiXin_Item(entity);
                                III.txtTitle.Text = entity.Title;
                                III.txtContent.Text = entity.Content;
                                III.img.Source = new BitmapImage(new Uri(entity.ImageUrl));
                                listBox.Items.Add(III);
                            }));


                        weiPublickList.Add(entity);
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
