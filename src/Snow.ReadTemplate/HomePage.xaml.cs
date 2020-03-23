﻿using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp;
using Snow.ReadTemplate.Data;
using Snow.ReadTemplate.Models;
using Snow.ReadTemplate.Pages.Article;
using Snow.ReadTemplate.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Snow.ReadTemplate
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private MainViewModel ViewModel => MainPage.Current.ViewModel;

        public HomePage()
        {
            this.InitializeComponent();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            ListFrame.Navigate(typeof(ArticleList));
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ArticleViewModel article = ViewModel.CurrentArticle;
            DetailFrame.Navigate(typeof(ArticleDetail), article.Id);
        }
    }
}