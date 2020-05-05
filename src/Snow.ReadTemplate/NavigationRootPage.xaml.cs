using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Snow.ReadTemplate.Common;
using Snow.ReadTemplate.Pages.Article;
using Snow.ReadTemplate.ViewModels;
using muxc = Microsoft.UI.Xaml.Controls;
using NavigationViewBackRequestedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs;
using NavigationViewDisplayModeChangedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewDisplayModeChangedEventArgs;
using NavigationViewItemInvokedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs;
using NavigationViewPaneClosingEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewPaneClosingEventArgs;
using NavigationViewSelectionChangedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Snow.ReadTemplate
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NavigationRootPage : Page
    {
        private CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

        public static NavigationRootPage Current;
        private PageHeader _header;

        public MainViewModel ViewModel { get; } = new MainViewModel();

        public muxc.NavigationView NavigationView
        {
            get { return NavView; }
        }

        public PageHeader PageHeader
        {
            get
            {
                return _header ?? (_header = UIHelper.GetDescendantsOfType<PageHeader>(NavView).FirstOrDefault());
            }
        }

        public string PageAppTitle { get; set; }


        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("home", typeof(MasterDetailPage)),
            ("SamplePage2", typeof(SamplePage2))
        };

        public NavigationRootPage()
        {
            this.InitializeComponent();
            AddNavigationMenuItems();
            Current = this;
            
            Window.Current.SetTitleBar(AppTitleBar);            
            ContentFrame.Navigate(typeof(MasterDetailPage));
            coreTitleBar.LayoutMetricsChanged += (s, e) => UpdateAppTitle(s);   
        }

        private void AddNavigationMenuItems()
        {
            //NavView.MenuItems.Add();
        }

        void UpdateAppTitle(CoreApplicationViewTitleBar coreTitleBar)
        {
            //ensure the custom title bar does not overlap window caption controls
            Thickness currMargin = AppTitleBar.Margin;
            AppTitleBar.Margin = new Thickness(currMargin.Left, currMargin.Top, coreTitleBar.SystemOverlayRightInset, currMargin.Bottom);
        }

        public void SetAppTitle(string pageName = null)
        {
            string title = Windows.ApplicationModel.Package.Current.DisplayName;
            if (!String.IsNullOrEmpty(pageName))
            {
                title = $"{pageName} - {title}";
            }
            AppTitle.Text = title;
        }
        
        private void NavView_OnLoaded(object sender, RoutedEventArgs e)
        {
            // NavView doesn't load any page by default, so load home page.
            NavView.SelectedItem = NavView.MenuItems[0];

            // If navigation occurs on SelectionChanged, this isn't needed.
            // Because we use ItemInvoked to navigate, we need to call Navigate
            // here to load the home page.
            NavView_Navigate("home", new EntranceNavigationTransitionInfo());

            // Add keyboard accelerators for backwards navigation.
            var goBack = new KeyboardAccelerator { Key = VirtualKey.GoBack };
            goBack.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(goBack);

            // ALT routes here
            var altLeft = new KeyboardAccelerator
            {
                Key = VirtualKey.Left,
                Modifiers = VirtualKeyModifiers.Menu
            };
            altLeft.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(altLeft);
        }

        private void NavView_OnItemInvoked(muxc.NavigationView sender,
            NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
            }
            else
            {
                var navItemTag = args.InvokedItemContainer.Tag.ToString();
                NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void NavView_OnSelectionChanged(muxc.NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected == true)
            {
                SetAppTitle();
                NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
            }
            else if (args.SelectedItemContainer != null)
            {
                SetAppTitle(args.SelectedItemContainer.Content?.ToString());
                var navItemTag = args.SelectedItemContainer.Tag.ToString();
                NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void NavView_Navigate(string navItemTag, NavigationTransitionInfo transitionInfo)
        {
            Type _page = null;
            if (navItemTag == "settings")
            {
                _page = typeof(SettingsPage);
            }
            else
            {
                _page = typeof(MasterDetailPage);
            }
            ContentFrame.Navigate(_page, null, transitionInfo);
            //else
            //{
            //    var item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
            //    _page = item.Page;
            //}
            //// Get the page type before navigation so you can prevent duplicate
            //// entries in the backstack.
            //var preNavPageType = ContentFrame.CurrentSourcePageType;

            //// Only navigate if the selected page isn't currently loaded.
            //if (!(_page is null) && !Type.Equals(preNavPageType, _page))
            //{
            //    ContentFrame.Navigate(_page, null, transitionInfo);
            //}
        }

        private void NavView_OnPaneClosing(muxc.NavigationView sender, NavigationViewPaneClosingEventArgs args)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            UpdateAppTitleMargin(sender);
        }

        private void NavView_OnPaneOpening(muxc.NavigationView sender, object args)
        {
            UpdateAppTitleMargin(sender);
        }
       
        private void NavView_OnDisplayModeChanged(muxc.NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
        {
            Thickness currMargin = AppTitleBar.Margin;
            if (sender.DisplayMode == muxc.NavigationViewDisplayMode.Minimal)
            {
                AppTitleBar.Margin = new Thickness((sender.CompactPaneLength * 2), currMargin.Top, currMargin.Right, currMargin.Bottom);

            }
            else
            {
                AppTitleBar.Margin = new Thickness(sender.CompactPaneLength, currMargin.Top, currMargin.Right, currMargin.Bottom);
            }

            UpdateAppTitleMargin(sender);
            UpdateHeaderMargin(sender);
        }

        private void UpdateAppTitleMargin(muxc.NavigationView sender)
        {
            const int smallLeftIndent = 4, largeLeftIndent = 24;

            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
            {
                AppTitle.TranslationTransition = new Vector3Transition();

                if ((sender.DisplayMode == muxc.NavigationViewDisplayMode.Expanded && sender.IsPaneOpen) ||
                    sender.DisplayMode == muxc.NavigationViewDisplayMode.Minimal)
                {
                    AppTitle.Translation = new System.Numerics.Vector3(smallLeftIndent, 0, 0);
                }
                else
                {
                    AppTitle.Translation = new System.Numerics.Vector3(largeLeftIndent, 0, 0);
                }
            }
            else
            {
                Thickness currMargin = AppTitle.Margin;

                if ((sender.DisplayMode == muxc.NavigationViewDisplayMode.Expanded && sender.IsPaneOpen) ||
                    sender.DisplayMode == muxc.NavigationViewDisplayMode.Minimal)
                {
                    AppTitle.Margin = new Thickness(smallLeftIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
                }
                else
                {
                    AppTitle.Margin = new Thickness(largeLeftIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
                }
            }
        }

        private void UpdateHeaderMargin(muxc.NavigationView sender)
        {
            if (PageHeader != null)
            {
                if (sender.DisplayMode == muxc.NavigationViewDisplayMode.Minimal)
                {
                    Current.PageHeader.HeaderPadding = (Thickness)App.Current.Resources["PageHeaderMinimalPadding"];
                }
                else
                {
                    Current.PageHeader.HeaderPadding = (Thickness)App.Current.Resources["PageHeaderDefaultPadding"];
                }
            }
        }

        private void NavView_OnBackRequested(muxc.NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            On_BackRequested();

        }
        private void BackInvoked(KeyboardAccelerator sender,
            KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        private bool On_BackRequested()
        {
            if (!ContentFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed.
            if (NavView.IsPaneOpen &&
                (NavView.DisplayMode == muxc.NavigationViewDisplayMode.Compact ||
                 NavView.DisplayMode == muxc.NavigationViewDisplayMode.Minimal))
                return false;

            ContentFrame.GoBack();
            return true;
        }
        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType == typeof(SettingsPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
                NavView.SelectedItem = (muxc.NavigationViewItem)NavView.SettingsItem;
                NavView.Header = "Settings";
            }
            else if (ContentFrame.SourcePageType != null)
            {
                var item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);

                NavView.SelectedItem = NavView.MenuItems
                    .OfType<muxc.NavigationViewItem>()
                    .First(n => n.Tag.Equals(item.Tag));

                NavView.Header =
                    ((muxc.NavigationViewItem)NavView.SelectedItem)?.Content?.ToString();
            }
        }

        private void OnRootFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (ContentFrame.SourcePageType == typeof(SettingsPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
                NavView.SelectedItem = (muxc.NavigationViewItem)NavView.SettingsItem;
                //NavView.Header = "Settings";
            }
            else
            {
                //NavView.Header = String.Empty;// ((muxc.NavigationViewItem)NavView.SelectedItem)?.Content?.ToString();
            }
            if (new List<Type>() { typeof(MasterDetailPage) , typeof(ArticleDetail)}.Contains(e.SourcePageType))
            {
                //NavView.AlwaysShowHeader = false;
            }
            else
            {
                //NavView.AlwaysShowHeader = true;
            }
        }
    }
}
