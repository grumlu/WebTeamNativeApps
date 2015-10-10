using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTeamWindows10Universal.Resources;
using WebTeamWindows10Universal.Resources.NavigationService;
using WebTeamWindows10Universal.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WebTeamWindows10Universal.ViewModel
{
    class SettingsViewModel : ViewModelBase
    {
        public RelayCommand DisconnectCommand
        {
            get; set;
        } = new RelayCommand(Disconnect);

        private static void Disconnect()
        {
            Resources.APIWebTeam.Connection.Disconnect();

            Frame frame = new Frame();
            frame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
            (App.Current as App).NavigationService = new NavigationService(frame);

            Window.Current.Content = frame;
            frame.Navigate(typeof(LoginView));
        }
    }
}
