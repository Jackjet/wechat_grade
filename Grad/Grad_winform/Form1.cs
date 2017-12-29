using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Grad_Proxy.Common;
using Grad_Proxy.Entity;
using ConferenceCommon;
using ConferenceCommon.SharePointHelper;
using spClient = Microsoft.SharePoint.Client;

namespace Grad_winform
{
    public partial class Form1 : Form
    {

        ConferenceCommon.WebHelper.WebCredentialManage manage = null;
        static string webSite = "http://117.106.85.19/sites/Teacher";

        static string beforeImageSite = "http://117.106.85.19";

        string userName = "guanli1";
        string password = "pwd@123";
        string domain = "bjyqyz.com";
        public Form1()
        {
            InitializeComponent();

       
        //http://117.106.85.19/Lists/SchoolNews/AllItems.aspx#InplviewHashb7760fab-0d91-4dfe-ba3c-6e61d53f351d=Paged%3DTRUE-p_Created%3D20160713%252005%253a51%253a11-p_ID%3D123-PageFirstRow%3D31
        //http://117.106.85.19/Lists/SchoolNews/AllItems.aspx#InplviewHashb7760fab-0d91-4dfe-ba3c-6e61d53f351d=Paged%3DTRUE-p_Created%3D20160708%252008%253a46%253a32-p_ID%3D55-PageFirstRow%3D61
        //http://117.106.85.19/Lists/SchoolNews/AllItems.aspx#InplviewHashb7760fab-0d91-4dfe-ba3c-6e61d53f351d=Paged%3DTRUE-p_Created%3D20160708%252007%253a30%253a28-p_ID%3D25-PageFirstRow%3D91
            //this.webBrowser1.Navigate("");

            manage = new ConferenceCommon.WebHelper.WebCredentialManage(webBrowser1, "spadmin", "sp@2014");

            webBrowser1.Navigate("http://117.106.85.19/Lists/SchoolNews/AllItems.aspx#InplviewHashb7760fab-0d91-4dfe-ba3c-6e61d53f351d=Paged%3DTRUE-p_Created%3D20160713%252005%253a51%253a11-p_ID%3D123-PageFirstRow%3D31");

            this.webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
           
        }
        static ClientContextManage client = new ClientContextManage();
        private void button1_Click(object sender, EventArgs e)
        {
            var tt = this.webBrowser1.Document.Body.InnerHtml;

          //var pp =  this.webBrowser1.Document.GetElementsByTagName("s4-itm-cbx s4-itm-imgCbx");
           //var pp = tt.Split(new string[] { "s4-itm-cbx s4-itm-imgCbx" }, StringSplitOptions.RemoveEmptyEntries);
          
         //var html= webBrowser1.DocumentText;

         List<HtmlTag> tags = HtmlTag.FindTagByAttr(tt, "div", "class", "s4-itm-cbx s4-itm-imgCbx");
             List<HtmlTag> tags1 = HtmlTag.FindTagByAttr(tt, "div", "name", "Count");
             spClient.ListCollection listCollection = client.GetAllLists(webSite, userName, password, domain);
        }

       
    }
}
