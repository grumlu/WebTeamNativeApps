using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using WebTeamWindows10Universal.Resources;
using WebTeamWindows10Universal.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WebTeamWindows10Universal.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebTeamShell : Page
    {
        public WebTeamShell(Frame frame)
        {
            this.InitializeComponent();
            this.ShellSplitView.Content = frame;
            
            //Action réalisée lorsqu'on appuie sur un des boutons
            Action update = new Action(() =>
            {
                // update radiobuttons after frame navigates
                var type = frame.CurrentSourcePageType;
                foreach (var radioButton in AllRadioButtons(this))
                {
                    var target = radioButton.CommandParameter as NavType;
                    if (target == null)
                        continue;
                    radioButton.IsChecked = target.Type.Equals(type);
                }
                this.ShellSplitView.IsPaneOpen = false;
                switch (frame.CurrentSourcePageType.Name)
                {
                    case "NewsView": PageHeader.Text = "Actualités"; break;
                    case "ProfileView": PageHeader.Text = "Profil"; break;
                    case "SettingsView": PageHeader.Text = "Paramètres"; break;
                    case "FUSEView": PageHeader.Text = "Player Radio FUSE"; break;
                    default: PageHeader.Text = "En construction..."; break;
                }
                this.BackCommand.RaiseCanExecuteChanged();
            });
            frame.Navigated += (s, e) => update();
            this.Loaded += (s, e) => update();
            this.DataContext = this;

            //Ajout du back button
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            //Navigation vers la page d'accueil
            (App.Current as App).NavigationService.Navigate(typeof(NewsView));
        }

        // menu
        RelayCommand _menuCommand;
        public RelayCommand MenuCommand { get { return _menuCommand ?? (_menuCommand = new RelayCommand(ExecuteMenu)); } }
        private void ExecuteMenu()
        {
            this.ShellSplitView.IsPaneOpen = !this.ShellSplitView.IsPaneOpen;
        }

        // nav
        RelayCommand<NavType> _navCommand;
        public RelayCommand<NavType> NavCommand { get { return _navCommand ?? (_navCommand = new RelayCommand<NavType>(ExecuteNav)); } }
        private void ExecuteNav(NavType navType)
        {
            var type = navType.Type;
            var nav = (App.Current as App).NavigationService;

            // when we nav home, clear history
            if (type.Equals(typeof(View.ProfileView)))
                nav.ClearHistory();

            // navigate only to new pages
            if (nav.CurrentPageType != null && nav.CurrentPageType != type)
                nav.Navigate(type, navType.Parameter);
        }

        // back
        RelayCommand _backCommand;
        public RelayCommand BackCommand { get { return _backCommand ?? (_backCommand = new RelayCommand(ExecuteBack, CanBack)); } }
        private bool CanBack()
        {
            var nav = (App.Current as App).NavigationService;
            return nav.CanGoBack;
        }
        private void ExecuteBack()
        {
            var nav = (App.Current as App).NavigationService;
            nav.GoBack();
        }

        private void DontCheck(object s, RoutedEventArgs e)
        {
            (s as RadioButton).IsChecked = false;
        }

        // utility
        public List<RadioButton> AllRadioButtons(DependencyObject parent)
        {
            var list = new List<RadioButton>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is RadioButton)
                {
                    list.Add(child as RadioButton);
                    continue;
                }
                list.AddRange(AllRadioButtons(child));
            }
            return list;
        }
    }

    public class NavType
    {
        public Type Type { get; set; }
        public string Parameter { get; set; }
    }
}
