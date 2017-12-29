using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.IO.Compression;


namespace Grad_Proxy.Common
{
    public class HtmlTag
    {
        private String m_Name;
        private String m_BeginTag;
        private String m_InnerHTML;
        private Hashtable m_Attributes = new Hashtable();

        static Regex attrReg = new Regex(@"([a-zA-Z1-9_-]+)\s*=\s*(\x27|\x22)([^\x27\x22]*)(\x27|\x22)", RegexOptions.IgnoreCase);

        private HtmlTag(string name, string beginTag, string innerHTML)
        {
            m_Name = name;
            m_BeginTag = beginTag;
            m_InnerHTML = innerHTML;

            MatchCollection matchs = attrReg.Matches(beginTag);
            foreach (Match match in matchs)
            {
                m_Attributes[match.Groups[1].Value.ToUpper()] = match.Groups[3].Value;
            }
        }

        public List<HtmlTag> FindTag(String name)
        {
            return FindTag(m_InnerHTML, name, String.Format(@"<{0}(\s[^<>]*|)>", name));
        }

        public List<HtmlTag> FindTag(String name, String format)
        {
            return FindTag(m_InnerHTML, name, format);
        }

        public List<HtmlTag> FindTagByAttr(String tagName, String attrName, String attrValue)
        {
            return FindTagByAttr(m_InnerHTML, tagName, attrName, attrValue);
        }

        public String TagName
        {
            get { return m_Name; }
        }

        public String InnerHTML
        {
            get { return m_InnerHTML; }
        }

        public String GetAttribute(string name)
        {
            return m_Attributes[name.ToUpper()] as String;
        }

        /// 获取源代码

        /// <param name="url"></param>

        /// <returns></returns>

        public static string GetHtmlContent(string url, string encoding)
        {

            HttpWebRequest request = null;

            HttpWebResponse response = null;

            StreamReader reader = null;

            try
            {

                request = (HttpWebRequest)WebRequest.Create(url);

                request.Timeout = 20000;

                request.AllowAutoRedirect = false;

                response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
                {

                    if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))

                        reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), Encoding.GetEncoding(encoding));

                    else

                        reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));

                    string html = reader.ReadToEnd();

                    return html;

                }

            }

            catch
            {

            }

            finally
            {

                if (response != null)
                {

                    response.Close();

                    response = null;

                }

                if (reader != null)

                    reader.Close();

                if (request != null)

                    request = null;

            }

            return string.Empty;

        }


        public static string GetHtmlinfo(string PageUrl)
        {
            WebRequest request = WebRequest.Create(PageUrl);
            request.Timeout = 30000;

            WebResponse response = request.GetResponse();

            Stream resStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(resStream, System.Text.Encoding.UTF8);
            string htmlinfo = sr.ReadToEnd();
            resStream.Close();
            sr.Close();
            return htmlinfo;
        }

        public static String GetHtml(string url, Encoding encoding)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "mp.weixin.qq.com";
                //request.KeepAlive = true;
                request.Timeout = 20000;
                request.AllowAutoRedirect = false;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
                {
                    reader = new StreamReader(response.GetResponseStream(), encoding);
                    string html = reader.ReadToEnd();
                    return html;
                }
            }
            catch { }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                    reader.Close();
                if (request != null)
                    request = null;
            }
            return string.Empty;
        }

        public static string GetHtml2(string BookUri)
        {
            string Contentstring = "";
            Uri UriTag = new Uri(BookUri);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UriTag);
            //声明一个HttpWebRequest请求  
            request.Timeout = 30000;
            //设置连接超时时间  
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(streamReceive, Encoding.Default); //采用默认编码先读出源代码  
            Contentstring = streamReader.ReadToEnd().ToLower(); //初次读取源代码.  
            byte[] buffer = Encoding.Default.GetBytes(Contentstring);
            string enCode = Regex.Match(Contentstring, "charset=.*?\"").ToString();
            if (enCode != "")
            {//如果指定编码不为空，则用取得的编码翻译数组  
                enCode = enCode.Replace("charset=", "");
                enCode = enCode.Replace("\"", "");
                //streamReader = new StreamReader(streamReceive, Encoding.GetEncoding(enCode)); //采用指定编码先读出源代码  
                //Contentstring = streamReader.ReadToEnd().ToLower();  
                Contentstring = Encoding.GetEncoding(enCode).GetString(buffer);
            }
            return Contentstring;
        }

        /// <summary>
        /// 在文本html的文本查找标志名为tagName,并且属性attrName的值为attrValue的所有标志
        /// 例如：FindTagByAttr(html, "div", "class", "demo")
        /// 返回所有class为demo的div标志
        /// </summary>
        public static List<HtmlTag> FindTagByAttr(String html, String tagName, String attrName, String attrValue)
        {
            String format = String.Format(@"<{0}\s[^<>]*{1}\s*=\s*(\x27|\x22){2}(\x27|\x22)[^<>]*>", tagName, attrName, attrValue);
            return FindTag(html, tagName, format);
        }

        public static List<HtmlTag> FindTag(String html, String name, String format)
        {
            Regex reg = new Regex(format, RegexOptions.IgnoreCase);
            Regex tagReg = new Regex(String.Format(@"<(\/|)({0})(\s[^<>]*|)>", name), RegexOptions.IgnoreCase);

            List<HtmlTag> tags = new List<HtmlTag>();
            int start = 0;

            while (true)
            {
                Match match = reg.Match(html, start);
                if (match.Success)
                {
                    start = match.Index + match.Length;
                    Match tagMatch = null;
                    int beginTagCount = 1;

                    while (true)
                    {
                        tagMatch = tagReg.Match(html, start);
                        if (!tagMatch.Success)
                        {
                            tagMatch = null;
                            break;
                        }
                        start = tagMatch.Index + tagMatch.Length;
                        if (tagMatch.Groups[1].Value == "/") beginTagCount--;
                        else beginTagCount++;
                        if (beginTagCount == 0) break;
                    }

                    if (tagMatch != null)
                    {
                        HtmlTag tag = new HtmlTag(name, match.Value, html.Substring(match.Index + match.Length, tagMatch.Index - match.Index - match.Length));
                        tags.Add(tag);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return tags;
        }
    }
}
