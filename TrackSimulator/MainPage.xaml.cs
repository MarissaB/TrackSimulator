using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

namespace TrackSimulator
{
    /// <summary>
    /// The primary parent page of the application
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
        }

        /// <summary>
        /// Mapping of tags and pages used for dictating navigation
        /// </summary>
        private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("home", typeof(HomePage)),
            ("drivers", typeof(DriversPage)),
            ("categories", typeof(CategoriesPage)),
            ("reports", typeof(ReportsPage))
        };

        private void Navigation_Loaded(object sender, RoutedEventArgs e)
        {
            // Add handler for ContentFrame navigation.
            ContentFrame.Navigated += On_Navigated;

            // Navigation doesn't load any page by default, so load home page.
            Navigation.SelectedItem = Navigation.MenuItems[0];
        }

        /// <summary>
        /// Event triggered when user clicks a navigation menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Navigation_SelectionChanged(muxc.NavigationView sender, muxc.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer != null)
            {
                string navItemTag = args.SelectedItemContainer.Tag.ToString();
                Navigation_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        /// <summary>
        /// Navigate to the selected page
        /// </summary>
        /// <param name="navItemTag">Tag of the selected nav menu item</param>
        /// <param name="transitionInfo">Animation for transition</param>
        private void Navigation_Navigate(string navItemTag, Windows.UI.Xaml.Media.Animation.NavigationTransitionInfo transitionInfo)
        {
            Type _page = null;

            (string Tag, Type Page) item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
            _page = item.Page;

            // Get the page type before navigation so you can prevent duplicate entries in the backstack.
            Type preNavPageType = ContentFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (!(_page is null) && !Equals(preNavPageType, _page))
            {
                _ = ContentFrame.Navigate(_page, null, transitionInfo);
            }
        }

        /// <summary>
        /// Handle the selected item and set the header after navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            if (ContentFrame.SourcePageType != null)
            {
                (string Tag, Type Page) item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);

                Navigation.SelectedItem = Navigation.MenuItems
                    .OfType<muxc.NavigationViewItem>()
                    .First(n => n.Tag.Equals(item.Tag));

                //Navigation.Header = ((muxc.NavigationViewItem)Navigation.SelectedItem)?.Content?.ToString();
            }
        }
    }
}
