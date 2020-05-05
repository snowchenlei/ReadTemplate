using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Snow.ReadTemplate.Pages.Search;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Snow.ReadTemplate
{
    public sealed partial class ListHeader : UserControl
    {
        public event RoutedEventHandler Refresh;
        public event RoutedEventHandler GoTop;

        public ListHeader()
        {
            this.InitializeComponent();
        }

        private void Search_OnClick(object sender, RoutedEventArgs e)
        {
            MasterDetailPage.Current.MasterDetailFrame.Navigate(typeof(SearchIndex));
        }

        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            Refresh?.Invoke(sender, e);
        }

        private void Top_OnClick(object sender, RoutedEventArgs e)
        {
            GoTop?.Invoke(sender, e);
        }
    }
}
