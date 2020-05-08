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
using Windows.UI.Xaml.Media.Animation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Snow.ReadTemplate
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MasterDetailPage : Page
    {
        public static MasterDetailPage Current { get; private set; }
        public Frame MasterDetailFrame
        {
            get { return DetailFrame; }
        }
        private MainViewModel ViewModel => NavigationRootPage.Current.ViewModel;
        private CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

        public MasterDetailPage()
        {
            this.InitializeComponent();
            Current = this;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            MasterFrame.Navigate(typeof(ArticleView));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string navItemTag = e.Parameter as string;
            
            if (navItemTag == "home")
            {
                MasterFrame.Navigate(typeof(ArticleView));
            }
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ArticleViewModel article = ViewModel.CurrentArticle;
            DetailFrame.Navigate(typeof(ArticleDetail), article.Id);
            UpdateForVisualState(AdaptiveStates.CurrentState);
        }

        private void AdaptiveStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            UpdateForVisualState(e.NewState, e.OldState);
        }

        private void UpdateForVisualState(VisualState newState, VisualState oldState = null)
        {
            if (DetailFrame.CurrentSourcePageType != null)
            {
                ((Setter)NarrowState.Setters[1]).Value = "0";
                ((Setter)NarrowState.Setters[2]).Value = "0";
                ((Setter)NarrowState.Setters[3]).Value = "*";
            }
            else
            {
                ((Setter)NarrowState.Setters[1]).Value = "720";
                ((Setter)NarrowState.Setters[2]).Value = "*";
                ((Setter)NarrowState.Setters[3]).Value = "0";
            }
            var isNarrow = newState == NarrowState;

            if (isNarrow && oldState == DefaultState)
            {                
                //NarrowState.Setters.Remove()
                //NarrowState.Setters[0]
                //Frame.Navigate(DetailFrame.SourcePageType, null, new SuppressNavigationTransitionInfo());
                //GridSplitter.Visibility = Visibility.Collapsed;
                //if (DetailFrame.CurrentSourcePageType != null)
                //{
                //    MasterColumn.MinWidth = 0;
                //    MasterColumn.Width = new GridLength(0);
                //    DetailColumn.Width = new GridLength(1, GridUnitType.Star);
                //    //DetailColumn.Width = new GridLength(1, GridUnitType.Star);
                //    //MasterColumn.Width = new GridLength(0, GridUnitType.Star);
                //    //MasterColumn.MinWidth = 0;
                //}
                //else
                //{
                //    DetailColumn.Width = new GridLength(0);
                //    MasterColumn.MaxWidth = 720;
                //    MasterColumn.Width = new GridLength(1, GridUnitType.Star);
                //    //Frame.Navigate(MasterFrame.SourcePageType, null, new SuppressNavigationTransitionInfo());

                //    //DetailColumn.Width = new GridLength(0, GridUnitType.Star);
                //    //MasterColumn.Width = new GridLength(1, GridUnitType.Star);
                //}
            }
        }
    }
}
