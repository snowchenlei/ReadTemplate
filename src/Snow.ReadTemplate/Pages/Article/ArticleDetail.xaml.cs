using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Snow.ReadTemplate.Models;
using Snow.ReadTemplate.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Snow.ReadTemplate.Pages.Article
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ArticleDetail : Page
    {
        private MainViewModel ViewModel => NavigationRootPage.Current.ViewModel;
        private ArticleViewModel _article;
        public ArticleDetail()
        {
            this.InitializeComponent();
        }

        private int _id;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (ViewModel.CurrentArticle != null)
            {
                _id = ViewModel.CurrentArticle.Id;
            }
        }

        private async void ArticleDetail_OnLoaded(object sender, RoutedEventArgs e)
        {
            ArticleDetailViewLoadingProgressRing.IsLoading = true;

            _article = await BookManager.GetBook(_id);
            if (_article != null)
            {
                Title.Text = _article.Title;
                Author.Text = _article.Author;
                CreationTime.Text = _article.CreationTime;
                Content.NavigateToString(_article.Description);
            }
            else
            {
                Content.NavigateToString("非法的标识");
            }

            ArticleDetailViewLoadingProgressRing.IsLoading = false;
        }
        private async void Content_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            Content.Height = Convert.ToInt32(await Content.InvokeScriptAsync("eval",
                new string[] { "document.documentElement.scrollHeight.toString();" }));
        }
    }
}
