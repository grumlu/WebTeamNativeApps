using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using WebTeamWindows10Universal.Controls;
using WebTeamWindows10Universal.Resources;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace WebTeamWindows10Universal.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebTeamShell : Page
    {
        //Elements du menu hamburger
        private List<NavMenuItem> navtoplist = new List<NavMenuItem>(
        new[]
        {
                new NavMenuItem()
                {
                    Symbol = Symbol.Home,
                    Label = "Actualités",
                    DestPage = typeof(NewsView)
                },
                //new NavMenuItem()
                //{
                //    Symbol = Symbol.Message,
                //    Label = "Discussions",
                //    DestPage = null,
                //    IsEnabled = false
                //},
                new NavMenuItem()
                {
                    Symbol = Symbol.People,
                    Label = "Trombinoscope (Alpha)",
                    DestPage = typeof(TrombiView)
                },
                new NavMenuItem()
                {
                    Symbol = Symbol.Audio,
                    Label = "Radio FUSE",
                    DestPage = typeof(FUSEView)
                },
        });
        private List<NavMenuItem> navbotlist = new List<NavMenuItem>(
        new[]
        {
                new NavMenuItem()
                {
                    Symbol = Symbol.Contact,
                    Label = "Profil",
                    DestPage = typeof(ProfileView)
                },
                new NavMenuItem()
                {
                    Symbol = Symbol.Setting,
                    Label = "Paramètres",
                    DestPage = typeof(SettingsView)
                }
        });

        public Frame AppFrame { get { return this.frame; } }

        public static WebTeamShell Current = null;
        public WebTeamShell()
        {
            this.InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                Current = this;
                this.DataContext = this;

                AppFrame.Navigate(typeof(NewsView));

                NavMenuList.SelectedIndex = 0;

                //Hide the back button
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            };

            //Ajout du back button
            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManager_BackRequested;

            NavMenuList.ItemsSource = navtoplist;
            NavMenuListBottom.ItemsSource = navbotlist;
        }


        RelayCommand _suggestionOrBugReport;
        public RelayCommand SuggestionOrBugReport { get { return _suggestionOrBugReport ?? (_suggestionOrBugReport = new RelayCommand(SendSuggestionOrBugCommand)); } }
        private async void SendSuggestionOrBugCommand()
        {
            var mailto = new Uri("mailto:?to=webteam@ensea.fr&subject=[W10App] Suggestion or Bug Report");
            await Windows.System.Launcher.LaunchUriAsync(mailto);
        }

        /// <summary>
        /// Navigate to the Page for the selected <paramref name="listViewItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="listViewItem"></param>
        private void NavMenuList_ItemInvoked(object sender, ListViewItem listViewItem)
        {
            var item = (NavMenuItem)((NavMenuListView)sender).ItemFromContainer(listViewItem);

            if (item != null)
            {
                if (item.DestPage != null &&
                    item.DestPage != this.AppFrame.CurrentSourcePageType)
                {
                    this.AppFrame.Navigate(item.DestPage, item.Arguments);
                }
            }
        }

        /// <summary>
        /// Ensures the nav menu reflects reality when navigation is triggered outside of
        /// the nav menu buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                var item = (from p in this.navtoplist where p.DestPage == e.SourcePageType select p).SingleOrDefault();
                if (item == null && this.AppFrame.BackStackDepth > 0)
                {
                    // In cases where a page drills into sub-pages then we'll highlight the most recent
                    // navigation menu item that appears in the BackStack
                    foreach (var entry in this.AppFrame.BackStack.Reverse())
                    {
                        item = (from p in this.navtoplist where p.DestPage == entry.SourcePageType select p).SingleOrDefault();
                        if (item != null)
                            break;
                    }
                }

                var container = (ListViewItem)NavMenuList.ContainerFromItem(item);

                // While updating the selection state of the item prevent it from taking keyboard focus.  If a
                // user is invoking the back button via the keyboard causing the selected nav menu item to change
                // then focus will remain on the back button.
                if (container != null) container.IsTabStop = false;
                NavMenuList.SetSelectedItem(container);
                if (container != null) container.IsTabStop = true;
            }
        }

        private void OnNavigatedToPage(object sender, NavigationEventArgs e)
        {
            // After a successful navigation set keyboard focus to the loaded page
            if (e.Content is Page && e.Content != null)
            {
                var control = (Page)e.Content;
                control.Loaded += Page_Loaded;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((Page)sender).Focus(FocusState.Programmatic);
            ((Page)sender).Loaded -= Page_Loaded;
        }

        /// <summary>
        /// Enable accessibility on each nav menu item by setting the AutomationProperties.Name on each container
        /// using the associated Label of each item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavMenuItemContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue && args.Item != null && args.Item is NavMenuItem)
            {
                args.ItemContainer.SetValue(AutomationProperties.NameProperty, ((NavMenuItem)args.Item).Label);
            }
            else
            {
                args.ItemContainer.ClearValue(AutomationProperties.NameProperty);
            }
        }

        /// <summary>
        /// Callback when the SplitView's Pane is toggled open or close.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TogglePaneButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.ShellSplitView.IsPaneOpen = !this.ShellSplitView.IsPaneOpen;
        }

        private void NavMenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Show the back button
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            //Change the title
            var selectedItem = (sender as NavMenuListView).SelectedValue;
            PageTitle.Text = (selectedItem as NavMenuItem)?.Label ?? "There should be a title here.";

            //Deselect the other item
            if (sender.Equals(NavMenuList))
            {
                NavMenuListBottom.SetSelectedItem(null);
            }
            else
            {
                NavMenuList.SetSelectedItem(null);
            }
        }

        #region BackRequested Handlers

        private void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            bool handled = e.Handled;
            this.BackRequested(ref handled);
            e.Handled = handled;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            bool ignored = false;
            this.BackRequested(ref ignored);
        }

        private void BackRequested(ref bool handled)
        {
            // Get a hold of the current frame so that we can inspect the app back stack.

            if (this.AppFrame == null)
                return;

            // Check to see if this is the top-most page on the app back stack.
            if (this.AppFrame.CanGoBack && !handled)
            {
                // If not, set the event to handled and go back to the previous page in the app.
                handled = true;
                this.AppFrame.GoBack();
                // Check if this is now the top-most page on the app back stack
                if(!this.AppFrame.CanGoBack)
                {
                    // If not, hide the back button
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                }
            }
        }

        #endregion
    }
}
