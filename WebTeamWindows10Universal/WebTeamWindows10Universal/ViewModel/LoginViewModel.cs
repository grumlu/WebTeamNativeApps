using System;
using System.Diagnostics;
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
            ConnectCommand = new RelayCommand(Connect);
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

        public bool IsChangeUsernameVisible { get { return loginModel.IsChangeUsernameVisible() ? true : false; } }

        public bool IsProgressRingActive
        {
            get
            {
                if (_isInDesignMode)
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

        public async void Connect()
        {
            CanPerformAction = false;
            try
            {
                await loginModel.Connect();
                RaisePropertyChanged("Username");
                RaisePropertyChanged("IsChangeUsernameVisible");
            }
            catch (Exception excep)
            {
                string errMsg;
                if (excep is APIWebteamException)
                {
                    if (((APIWebteamException)excep).Error == APIWebteamException.ERROR.WEBTEAM_UNAVAILABLE)
                    {
                        errMsg = "Il semblerait que la Webteam soit hors ligne. Si le problème persiste, contactez webteam@ensea.fr";
                    }
                    else
                    {
                        errMsg = "Une erreur inconnue est survenue lors de la réception de votre profil." +
                            "Assurez-vous que votre application est à jour, et contactez la webteam (webteam@ensea.fr) si le problème persiste";
                    }
                }
                else
                {
                    errMsg = "Une erreur est survenue :\n" + excep.StackTrace.ToString();
                }

                DispatchService.Invoke(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                Debugger.Break();
                MessageDialog dialog = new MessageDialog(errMsg);
                await dialog.ShowAsync();
            }
        );
                return;
            }
            finally
            {
                CanPerformAction = true;
            }

            DispatchService.Invoke(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>

            {
                Frame frame = new Frame();
                frame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
                (App.Current as App).NavigationService = new NavigationService(frame);

                Window.Current.Content = new View.WebTeamShell(frame);

            });
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
