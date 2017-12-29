using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Grad
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
        }

        public static void Test1()
        {
            String html = HtmlTag.GetHtmlinfo("http://weixin.sogou.com/weixin?type=1&query=北京第二外国语学院&ie=utf8&_sug_=n&_sug_type_=");
            List<HtmlTag> tags = HtmlTag.FindTagByAttr(html, "div", "class", "results mt7");
            if (tags.Count > 0)
            {
                List<HtmlTag> item_tags = tags[0].FindTagByAttr("div", "target", "_blank");
                foreach (HtmlTag item_tag in item_tags)
                {
                    var htmlUri = item_tag.GetAttribute("href");
                }
            }
        }
        //string Pattern = @"<\/a>[^\n]*问题点数:";
        public static void Test2()
        {
            //String html = HtmlTag.GetHtml("http://mp.weixin.qq.com/profile?src=3&timestamp=1474535878&ver=1&signature=K7Syb3ygPnbI9A6SEqtr1IcZwrLDe99UDy2O6Y-4leYQdq4R0EzIgLVrjg9FIRABvfBoSc1HRkeTaNQboBcyvw==", 
            //    Encoding.UTF8);

            String html = HtmlTag.GetHtmlinfo("http://mp.weixin.qq.com/profile?src=3&timestamp=1474592663&ver=1&signature=OlstH1J8Y*px*k9-mJ2oEcWGYb70ZjRnZZasvynMo*tWmsVWfEWr5H2kbi5gq-ZDqoqmuxW2ViAS-mXdx9kWEg==");

            //var match =   regex.Matches(html);
            List<HtmlTag> tags = HtmlTag.FindTagByAttr(html, "script", "type", "text/javascript");

            string content = tags[tags.Count -1].InnerHTML;
            string pattern = "var msgList = '{";
            string tt = content.Substring(content.IndexOf(pattern) + pattern.Length);

            string[] sArr = tt.Split(new string[] { "&quot;title&quot" }, StringSplitOptions.RemoveEmptyEntries);
            //string[] pse = tt.Split(tt, @"&quot;,&quot;digest&quot;:&quot;" , RegexOptions.IgnoreCase);

            for (int i = 0; i < sArr.Length; i++)
            {
                sArr[i] = sArr[i].Replace("&quot", "");
                sArr[i] = sArr[i].Replace("\\\\/", "\"");


                string[] dsc = sArr[i].Split(new string[] { ";,;" }, StringSplitOptions.RemoveEmptyEntries);

                if(i ==0) continue;

                WeiPublicEntity entity = new WeiPublicEntity();
                
                for (int j = 0; j < dsc.Count(); j++)
                {

                    if(j==0)
                    {
                        entity.Title = dsc[j];
                    }
                    else if(j == 1)
                    {
                        entity.Content = dsc[j];
                    }
                    else if (j == 3)
                    {
                        entity.DateTime = dsc[j];
                    }
                    else if (j == 5)
                    {
                        entity.ImageUrl = dsc[j];
                    }
                }
                
                
            }

            

            if (tags.Count > 0)
            {
                List<HtmlTag> item_tags = tags[0].FindTagByAttr("div", "class", "weui_msg_card_list");
               
            }
        }

    }
}
