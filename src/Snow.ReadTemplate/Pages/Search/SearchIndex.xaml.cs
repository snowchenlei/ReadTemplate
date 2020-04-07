using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Snow.ReadTemplate.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Snow.ReadTemplate.Pages.Search
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchIndex : Page
    {
        public ObservableCollection<string> SearchHistories { get; set; }

        public SearchIndex()
        {
            this.InitializeComponent();
            SearchHistories =new ObservableCollection<string>();
        }

        private void OrderSearchBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is UserControls.CollapsibleSearchBox searchBox)
            {
                searchBox.AutoSuggestBox.QuerySubmitted += OrderSearch_QuerySubmitted;
                searchBox.AutoSuggestBox.PlaceholderText = "请输入要搜索的内容";
                //searchBox.AutoSuggestBox.ItemTemplate = (DataTemplate)Resources["SearchSuggestionItemTemplate"];
                //searchBox.AutoSuggestBox.ItemContainerStyle = (Style)Resources["SearchSuggestionItemStyle"];
            }
        }

        private void OrderSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!String.IsNullOrWhiteSpace(args.QueryText))
            {
                SearchResult.Visibility = Visibility.Visible;
                WelcomeResult.Visibility = Visibility.Collapsed;
                SearchResult.Navigate(typeof(SearchList), args.QueryText);
            }
            else
            {
                SearchResult.Visibility = Visibility.Collapsed;
                WelcomeResult.Visibility = Visibility.Visible;
            }
        }

        private void Clear_OnClick(object sender, RoutedEventArgs e)
        {
            SearchHistories.Clear();
        }
    }
}
