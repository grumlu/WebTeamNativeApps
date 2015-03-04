using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebTeamWindows.Common;
using WebTeamWindows.Model;
using WebTeamWindows.Resources;
using WebTeamWindows.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace WebTeamWindows.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private LoginModel loginModel;

        public RelayCommand ConnectCommand { get; set; }

        public LoginViewModel()
        {
            loginModel = new LoginModel();
            ConnectCommand = new RelayCommand(async () =>
            {
                IsProgressRingActive = true;
                ERROR err = await loginModel.Connect();
                OnPropertyChanged("Username");
                OnPropertyChanged("IsChangeUsernameVisible");
                IsProgressRingActive = false;

                if (err == ERROR.NO_ERR)
                {
                    var dispatcher = Window.Current.Dispatcher;
                    dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                    {
                        NavigationService.Navigate(typeof(WebTeamView));
                    });
                }
            });
        }

        /// <summary>
        /// Affiche le nom d'utilisateur ou un nom "aléatoire"
        /// </summary>
        public string Username
        {
            get
            {
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                    return "Nom d'utilisateur";
                return loginModel.GetUsername();

            }
        }

        public Visibility IsChangeUsernameVisible { get { return loginModel.IsChangeUsernameVisible() ? Visibility.Visible : Visibility.Collapsed; } }

        public bool IsProgressRingActive
        {
            get
            {
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                    return true;
                return loginModel.IsProgressRingActive;
            }
            set
            {
                loginModel.IsProgressRingActive = value;
                OnPropertyChanged("IsProgressRingActive");
            }
        }

        public void Disconnect()
        {
            APIWebTeam.Disconnect();
            OnPropertyChanged("Username");
            OnPropertyChanged("IsChangeUsernameVisible");
        }

        // Create the OnPropertyChanged method to raise the event 
        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


    }

}
