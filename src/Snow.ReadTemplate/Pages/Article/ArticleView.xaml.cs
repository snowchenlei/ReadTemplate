using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Snow.ReadTemplate.Pages.Article
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ArticleView : Page
    {
        public ArticleView()
        {
            this.InitializeComponent();

            ListFrame.Navigate(typeof(ArticleList));

            DispatcherTimer time = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 5)
            };
            time.Tick += Time_Tick;
            time.Start();
        }

        private void Time_Tick(object sender, object e)
        {
            int i = ArticleFv.SelectedIndex;
            i++;
            if (i >= ArticleFv.Items.Count)
            {
                i = 0;
            }

            ArticleFv.SelectedIndex = i;
        }

        /// <summary>
        /// Workaround to support earlier versions of Windows. 
        /// </summary>
        private void CommandBar_Loaded(object sender, RoutedEventArgs e)
        {
            if (false)//(Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            {
                (sender as CommandBar).DefaultLabelPosition = CommandBarDefaultLabelPosition.Bottom;
            }
            else
            {
                (sender as CommandBar).DefaultLabelPosition = CommandBarDefaultLabelPosition.Right;
            }
        }

        private void Search_OnClick(object sender, RoutedEventArgs e)
        {
            //DetailFrame.Navigate(typeof(SearchIndex));
        }

        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            ArticleList.Current.RefreshAsync();
        }

        private void Top_OnClick(object sender, RoutedEventArgs e)
        {
            ArticleList.Current.GoTop();
        }
    }
}
