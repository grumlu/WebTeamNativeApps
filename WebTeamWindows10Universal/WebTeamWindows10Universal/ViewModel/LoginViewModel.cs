using System;
using WebTeamWindows10Universal.Model;
using WebTeamWindows10Universal.Resources;
using WebTeamWindows10Universal.Resources.APIWebTeam;
using WebTeamWindows10Universal.Resources.NavigationService;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WebTeamWindows10Universal.ViewModel
{
    class LoginViewModel : ViewModelBase
    {
        private LoginModel loginModel;

        public RelayCommand ConnectCommand { get; set; }

        public RelayCommand DisconnectCommand { get; set; }

        public LoginViewModel()
        {
            loginModel = new LoginModel();
            DisconnectCommand = new RelayCommand(Disconnect);
            ConnectCommand = new RelayCommand(async () =>
            {
                try
                {
                    CanPerformAction = false;
                    ERROR err = await loginModel.Connect();

                    RaisePropertyChanged("Username");
                    RaisePropertyChanged("IsChangeUsernameVisible");

                    CanPerformAction = true;
                    if (err == ERROR.NO_ERR)
                    {
#pragma warning disable CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
                        DispatchService.Invoke(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>

                        {
                            Frame frame = new Frame();
                            frame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
                            (App.Current as App).NavigationService = new NavigationService(frame);

                            Window.Current.Content = new View.WebTeamShell(frame);

                        });
                    }

                }
                catch (Exception excep)
                {
                    DispatchService.Invoke(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                    {
                        string errMsg = "Une erreur est survenue :\n" + excep.StackTrace.ToString();
                        MessageDialog dialog = new MessageDialog(errMsg);
                        await dialog.ShowAsync();
                    }
                    );
#pragma warning restore CS4014 // Dans la mesure où cet appel n'est pas attendu, l'exécution de la méthode actuelle continue avant la fin de l'appel
                    IsProgressRingActive = false;
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
                    return "Nom d'utilisateur un peu trop long sur deux lignes";
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
                RaisePropertyChanged("IsProgressRingActive");
            }
        }

        private bool _canPerformAction = true;
        public bool CanPerformAction
        {
            get { return _canPerformAction; }
            set
            {
                IsProgressRingActive = !value;
                _canPerformAction = value;
                RaisePropertyChanged("CanPerformAction");
            }
        }

        public void Disconnect()
        {
            Connection.Disconnect();
            RaisePropertyChanged("Username");
            RaisePropertyChanged("IsChangeUsernameVisible");
        }

        /// <summary>
        /// Workaround for the WP app with Continue function
        /// </summary>
        public void NotifyUsernameChanged()
        {
            RaisePropertyChanged("Username");
        }
    }

}
