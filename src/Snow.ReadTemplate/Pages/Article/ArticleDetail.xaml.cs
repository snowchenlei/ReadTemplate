using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Snow.ReadTemplate.Models;
using Snow.ReadTemplate.ViewModels;
using System.Threading.Tasks;

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
                Content.NavigateToString($"<div style=\"-ms-content-zooming:none;\" >{ _article.Description.Replace("<img", "<img width=100%").Replace("<pre", "<pre style=\"white-space: pre-wrap;word-wrap: break-word;\"")}</div>");
            }
            else
            {
                Content.NavigateToString("非法的标识");
            }

            ArticleDetailViewLoadingProgressRing.IsLoading = false;
        }
        private async void Content_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
           await  CalculatePageHeightAsync();
            SizeChanged += ArticleDetail_SizeChanged;
            ScrollContent.ChangeView(null, 200, null);
        }

        private void Top_Click(object sender, RoutedEventArgs e)
        {
            ScrollContent.ChangeView(null,0, null);
        }

        private async void ArticleDetail_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           await  CalculatePageHeightAsync();
        }

        private async Task CalculatePageHeightAsync()
        {
            // TODO:跳转至上次滚动条位置
            double offset = ScrollContent.VerticalOffset;
            // 如果WebView高度高于内容高度，下面的js获取的就是WebView高度。所以这里先清0
            Content.Height = 0;
            double height = Convert.ToDouble(await Content.InvokeScriptAsync("eval",
                new string[] { "document.documentElement.scrollHeight.toString();" }));
            Content.Height = height;
            ScrollContent.ChangeView(null, offset, null);
        }

        private void ScrollContent_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            //ScrollContent.ChangeView(null, 200, null);
        }
    }
}
