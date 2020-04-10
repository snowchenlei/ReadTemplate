using System;
using System.ComponentModel;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp;
using Snow.ReadTemplate.Data;
using Snow.ReadTemplate.Models;
using Snow.ReadTemplate.Pages.Article;
using Snow.ReadTemplate.Pages.Search;
using Snow.ReadTemplate.ViewModels;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Media;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Snow.ReadTemplate
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private MainViewModel ViewModel => MainPage.Current.ViewModel;
        private CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

        public HomePage()
        {
            this.InitializeComponent();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //var menuItem = MainPage.Current.NavigationView.MenuItems.Cast<Microsoft.UI.Xaml.Controls.NavigationViewItem>().ElementAt(1);
            //menuItem.IsSelected = true;
            MainPage.Current.NavigationView.Header = string.Empty;

            //Items = ControlInfoDataSource.Instance.Groups.SelectMany(g => g.Items).OrderBy(i => i.Title).ToList();
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ArticleViewModel article = ViewModel.CurrentArticle;
            DetailFrame.Navigate(typeof(ArticleDetail), article.Id);
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
            DetailFrame.Navigate(typeof(SearchIndex));
        }

        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            ArticleList.Current.RefreshAsync();
        }

    }
}
