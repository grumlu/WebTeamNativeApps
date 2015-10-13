using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using WebTeamWindows10Universal.Model;
using Windows.ApplicationModel.Calls;
using Windows.Devices.Geolocation;
using Windows.Foundation.Metadata;
using Windows.Services.Maps;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace WebTeamWindows10Universal.ViewModel
{
    class UserViewModel : ViewModelBase
    {
        User _userModel;

        private bool _isLoaded = false;
        public bool IsLoaded { get { return _isLoaded; } private set { _isLoaded = value; RaisePropertyChanged("IsLoaded"); } }

        public UserViewModel()
        {
            // Create dummy user if we are in design mode
            if(_isInDesignMode)
            {
                _userModel = new User();
                GenerateActions();
                IsLoaded = true;
            }
        }

        public async void SetUserFromID(int id)
        {
            try
            {
                // Try to retrieve the user from the WebTeam server
                _userModel = await Resources.APIWebTeam.UserManagement.GetUserAsync(id);
            }
            catch
            {
                // Something went wrong. Disconnect.
                Resources.APIWebTeam.Connection.Disconnect();
                return;
            }

            ProcessUser();
        }

        public void SetUserFromObject(User _user)
        {
            _userModel = _user;
            ProcessUser();
        }

        /// <summary>
        /// Processes the user model to fill the properties
        /// </summary>
        private async void ProcessUser()
        {
            //Signaler à l'affichage que les champs ont été mis à jour
            RaiseUserFieldsChanged();

            //Génère la liste d'actions du profil
            GenerateActions();

            //Récupération de l'avatar
            profilePicture = await _userModel.GetAvatar();
            RaisePropertyChanged("ProfilePicture");

            //Chargement terminé
            IsLoaded = true;
        }

        #region USER PROPERTIES
        /// <summary>
        /// Notify the XAML page the User fields has changed
        /// </summary>
        private void RaiseUserFieldsChanged()
        {
            RaisePropertyChanged("Username");
            RaisePropertyChanged("Nickname");
            RaisePropertyChanged("Promo");
            RaisePropertyChanged("Groupe");
            RaisePropertyChanged("DateDeNaissance");
        }

        public string Username
        {
            get
            {
                return _userModel?.nom + " " + _userModel?.prenom;
            }
        }

        public string Nickname
        {
            get
            {
                return _userModel?.pseudo;
            }
        }

        public string Promo
        {
            get { return _userModel?.promo; }
        }

        public string Groupe
        {
            get { return _userModel?.groupe; }
        }

        public string DateDeNaissance
        {
            get
            {
                return _userModel?.dateDeNaissance.ToString("d");
            }
        }

        private BitmapImage profilePicture = null;
        public BitmapImage ProfilePicture
        {
            get { return profilePicture; }
        }
        #endregion

        #region PROFILE_ACTIONS

        private void GenerateActions()
        {
            ProfileCommands = new ObservableCollection<ProfileCommand>();

            //Mise en place des champs de commande
            //Commande pour appeler
            if(!_userModel.numeroPortable.Equals(""))
            {
                Action command;
                //Si on peut appeler
                if(ApiInformation.IsApiContractPresent("Windows.ApplicationModel.Calls.CallsPhoneContract", 1, 0))
                {
                    command = new Action(() =>
                    {
                        PhoneCallManager.ShowPhoneCallUI(_userModel.numeroPortable, Username);
                    });
                }
                //Si le device ne peut passer d'appel
                else
                {
                    command = new Action(async () =>
                    {
                        string errMsg = "Désolé, les appels ne sont pas disponnibles sur votre appareil";
                        MessageDialog dialog = new MessageDialog(errMsg);
                        await dialog.ShowAsync();
                    });
                }
                ProfileCommands.Add(new ProfileCommand("Appeler", _userModel.numeroPortable, command));
            }
            //Commande pour naviguer vers
            if(!_userModel.adresse.Equals(""))
            {
                Action command = new Action(async () =>
                {
                    var locFinderResult = await MapLocationFinder.FindLocationsAsync(_userModel.adresse, new Geopoint(new BasicGeoposition()));

                    if(locFinderResult.Locations[0] == null)
                    {
                        string errMsg = "Désolé, impossible de trouver où " + _userModel.prenom + " habite";
                        MessageDialog dialog = new MessageDialog(errMsg);
                        await dialog.ShowAsync();
                        return;
                    }

                    var geoPos = locFinderResult.Locations[0].Point.Position;

                    var driveToUri = new Uri(String.Format(
                         "ms-drive-to:?destination.latitude={0}&destination.longitude={1}&destination.name={2}",
                         geoPos.Latitude,
                         geoPos.Longitude,
                         _userModel.prenom + " " + _userModel.nom));

                    await Windows.System.Launcher.LaunchUriAsync(driveToUri);
                });

                ProfileCommands.Add(new ProfileCommand("Obtenir un itinéraire", _userModel.adresse, command));
                RaisePropertyChanged("ProfileCommands");
            }
        }



        /// <summary>
        /// RelayCommand pour l'appui sur une des commandes du profil
        /// </summary>
        public RelayCommand<TappedRoutedEventArgs> ExecuteProfileCommand
        {
            get; private set;
        } = new RelayCommand<TappedRoutedEventArgs>(selectedItem =>
        {
            FrameworkElement lvip = (FrameworkElement)selectedItem.OriginalSource;
            ProfileCommand pc = (ProfileCommand)lvip.DataContext;
            pc.Command.Invoke();
        });

        /// <summary>
        /// Liste des commades du profil
        /// </summary>
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
        #endregion
    }
}
