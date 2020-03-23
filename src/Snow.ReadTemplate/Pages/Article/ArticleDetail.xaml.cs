using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        public ArticleViewModel Article { get; set; }
        public ArticleDetail()
        {
            this.InitializeComponent();
        }

        private int _id;

        private async void NewDetail_OnLoaded(object sender, RoutedEventArgs e)
        {
            NewDetailViewLoadingProgressRing.IsLoading = true;

            Article = await BookManager.GetBook(_id);
            Title.Text = Article.Title;
            Author.Text = Article.Author;
            CreationTime.Text = Article.CreationTime;
            Content.Text = Article.CoverImage;

            NewDetailViewLoadingProgressRing.IsLoading = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                _id = Convert.ToInt32(e.Parameter.ToString());
            }
        }
    }
}
