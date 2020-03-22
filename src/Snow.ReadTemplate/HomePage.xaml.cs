using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp;
using Snow.ReadTemplate.Data;
using Snow.ReadTemplate.Models;
using Snow.ReadTemplate.Pages.Article;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Snow.ReadTemplate
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {


        public HomePage()
        {
            this.InitializeComponent();
        }

        private void HomePage_OnLoaded(object sender, RoutedEventArgs e)
        {
            var collection = new IncrementalLoadingCollection<BookSource, Book>
            {
                OnStartLoading = OnStartLoading, 
                OnEndLoading = OnEndLoading
            };
            NewsListView.ItemsSource = collection;
        }

        private void OnEndLoading()
        {
            //NewsListViewLoadingProgressRing.IsActive = false;
            //NewsListViewLoadingProgressRing.Visibility = Visibility.Collapsed;
            NewsListViewLoadingProgressRing.IsLoading = false;
        }

        private void OnStartLoading()
        {
            //NewsListViewLoadingProgressRing.IsActive = true;
            //NewsListViewLoadingProgressRing.Visibility = Visibility.Visible;
            NewsListViewLoadingProgressRing.IsLoading = true;
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            Book book = e.ClickedItem as Book ?? new Book();
            DetailFrame.Navigate(typeof(ArticleDetail), book.Id);

        }
    }
}
