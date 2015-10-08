using System;
using System.Threading.Tasks;
using WebTeamWindows10Universal.Model;
using WebTeamWindows10Universal.Resources;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;

namespace WebTeamWindows10Universal.ViewModel
{
    class ProfileViewModel : ViewModelBase
    {
        User _appUser;

        public ProfileViewModel()
        {
            GetAppUser();
        }

        public async void GetAppUser()
        {
            //On charge depuis la mémoire locale l'utilisateur
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            string userNickname = (string)roamingSettings.Values["user_nickname"];

            _appUser = await User.LoadUserFromTemporaryStorage(userNickname);

            RaisePropertyChanged("Username");
            RaisePropertyChanged("Promo");
            RaisePropertyChanged("Groupe");
            RaisePropertyChanged("DateDeNaissance");

            //Récupération de l'avatar
            profilePicture = await _appUser.GetAvatar();
            RaisePropertyChanged("ProfilePicture");

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
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                {
                    return "Username";
                }
                return _appUser?.nom + " " + _appUser?.prenom;
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
                return _appUser?.dateDeNaissance.ToString();
            }
        }
        
    }
}
