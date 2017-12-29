using Grad_Proxy.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// WeiXin_Item.xaml 的交互逻辑
    /// </summary>
    public partial class WeiXin_Item : UserControl
    {
        WeiPublicEntity WeiPublicEntity = null;
        public WeiXin_Item(WeiPublicEntity entity)
        {
            InitializeComponent();
            
           
            WeiPublicEntity =entity;
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Wei_Content wei = new Wei_Content();
            wei.webBrowser.Navigate(WeiPublicEntity.SourseUrl);
            wei.Show();
        }
    }
}
