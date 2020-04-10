using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp;
using Snow.ReadTemplate.Data;
using Snow.ReadTemplate.Models;
using Snow.ReadTemplate.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Snow.ReadTemplate.Pages.Article
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ArticleList : Page
    {
        public MainViewModel ViewModel => NavigationRootPage.Current.ViewModel;
        public static ArticleList Current;
        public ArticleList()
        {
            this.InitializeComponent();
            Current = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var collection = new IncrementalLoadingCollection<ArticleSource, ArticleViewModel>
            {
                OnStartLoading = OnStartLoading,
                OnEndLoading = OnEndLoading
            };
            ArticlesListView.ItemsSource = collection;
        }

        private void OnEndLoading()
        {
            NewsListViewLoadingProgressRing.IsLoading = false;
        }

        private void OnStartLoading()
        {
            NewsListViewLoadingProgressRing.IsLoading = true;
        }

        private void ArticlesListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.CurrentArticle = e.ClickedItem as ArticleViewModel;
        }

        public async void RefreshAsync()
        {
            var collection = (IncrementalLoadingCollection<ArticleSource, ArticleViewModel>)ArticlesListView.ItemsSource;
            await collection.RefreshAsync();
        }
    }
}
