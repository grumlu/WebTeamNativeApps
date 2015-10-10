using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebTeamWindows10Universal.Model;
using WebTeamWindows10Universal.Resources;
using Windows.ApplicationModel.Calls;
using Windows.Devices.Geolocation;
using Windows.Foundation.Metadata;
using Windows.Services.Maps;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace WebTeamWindows10Universal.ViewModel
{
    class ProfileViewModel : ViewModelBase
    {
        User _appUser;

        public ProfileViewModel()
        {
            ProfileCommands = new ObservableCollection<ProfileCommand>();
            GetAppUser();
        }

        public async void GetAppUser()
        {
            // On créé un dummy profile si on est en mode design
            if (_isInDesignMode)
            {
                _appUser = new User();
            }
            else
            {
                //On charge depuis la mémoire locale l'utilisateur
                try
                {
                    var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                    string userNickname = (string)roamingSettings.Values["user_nickname"];

                    _appUser = await User.LoadUserFromTemporaryStorage(userNickname);
                }
                catch
                {
                    //L'utilisateur n'existe pas dans la mémoire locale, on se déconnecte
                    Resources.APIWebTeam.Connection.Disconnect();
                }
            }

            RaisePropertyChanged("Username");
            RaisePropertyChanged("Nickname");
            RaisePropertyChanged("Promo");
            RaisePropertyChanged("Groupe");
            RaisePropertyChanged("DateDeNaissance");

            //Mise en place des champs de commande
            if (!_appUser.numeroPortable.Equals(""))
            {
                Action command;
                if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.Calls.CallsPhoneContract", 1, 0))
                {
                    command = new Action(() =>
                    {
                        PhoneCallManager.ShowPhoneCallUI(_appUser.numeroPortable, Username);
                    });
                }
                else
                {
                    command = new Action(async () =>
                    {
                        string errMsg = "Désolé, les appels ne sont pas disponnibles sur votre appareil";
                        MessageDialog dialog = new MessageDialog(errMsg);
                        await dialog.ShowAsync();
                    });
                }
                ProfileCommands.Add(new ProfileCommand("Appeler", _appUser.numeroPortable, command));
            }
            if (!_appUser.adresse.Equals(""))
            {
                Action command = new Action(async () =>
                {
                    var locFinderResult = await MapLocationFinder.FindLocationsAsync(_appUser.adresse, new Geopoint(new BasicGeoposition()));

                    if(locFinderResult.Locations[0] == null)
                    {
                        string errMsg = "Désolé, impossible de trouver où " + _appUser.prenom + " habite";
                        MessageDialog dialog = new MessageDialog(errMsg);
                        await dialog.ShowAsync();
                        return;
                    }

                    var geoPos = locFinderResult.Locations[0].Point.Position;

                    var driveToUri = new Uri(String.Format(
                         "ms-drive-to:?destination.latitude={0}&destination.longitude={1}&destination.name={2}",
                         geoPos.Latitude,
                         geoPos.Longitude,
                         _appUser.prenom + " " +_appUser.nom));

                    await Windows.System.Launcher.LaunchUriAsync(driveToUri);
                });

                ProfileCommands.Add(new ProfileCommand("Obtenir un itinéraire", _appUser.adresse, command));
            }

            if (!_isInDesignMode)
            {
                //Récupération de l'avatar
                profilePicture = await _appUser.GetAvatar();
                RaisePropertyChanged("ProfilePicture");
            }

            IsLoaded = true;
        }

        private bool _isLoaded = false;
        public bool IsLoaded
        {
            get
            {
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                    return true;
                return _isLoaded;
            }
            set
            {
                _isLoaded = value;
                RaisePropertyChanged("IsLoaded");
            }
        }

        public string Username
        {
            get
            {
                return _appUser?.nom + " " + _appUser?.prenom;
            }
        }

        public string Nickname
        {
            get
            {
                return _appUser?.pseudo;
            }
        }

        public string Promo
        {
            get { return _appUser?.promo; }
        }

        public string Groupe
        {
            get { return _appUser?.groupe; }
        }

        private BitmapImage profilePicture = null;
        public BitmapImage ProfilePicture
        {
            get { return profilePicture; }
        }

        public string DateDeNaissance
        {
            get
            {
                return _appUser?.dateDeNaissance.ToString("d");
            }
        }

        public RelayCommand<TappedRoutedEventArgs> ExecuteProfileCommand
        {
            get; private set;
        } = new RelayCommand<TappedRoutedEventArgs>(haha =>
        {
            ListViewItemPresenter lvip = (ListViewItemPresenter)haha.OriginalSource;
            ProfileCommand pc = (ProfileCommand)lvip.DataContext;
            pc.Command.Invoke();
        });

        public ObservableCollection<ProfileCommand> ProfileCommands { get; private set; }

        public class ProfileCommand
        {
            public string HeaderText { get; private set; }
            public string Text { get; private set; }
            public Action Command { get; private set; }

            internal ProfileCommand(string header, string text, Action command)
            {
                HeaderText = header;
                Text = text;
                Command = command;
            }
        }

    }
}
