using System.Threading.Tasks;
using WebTeamWindows10Universal.Model;
using Windows.UI.Xaml.Media.Imaging;

namespace WebTeamWindows10Universal.ViewModel
{
    class ProfileViewModel : ViewModelBase
    {
        AppUserModel _user;

        public ProfileViewModel()
        {
            IsLoaded = false;
            _user = new AppUserModel();
            GetAppUser();
        }

        public async Task GetAppUser()
        {
            await _user.GetAppUser();
            RaisePropertyChanged("Username");
            RaisePropertyChanged("Promo");
            RaisePropertyChanged("Groupe");

            await _user.GetProfilePicture();
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
                return _user.Username;
            }
        }

        public string Promo
        {
            get { return _user.AppUser.promo; }
        }

        public string Groupe
        {
            get { return _user.AppUser.groupe; }
        }

        public BitmapImage ProfilePicture
        {
            get { return _user.AppUser.avatar; }
        }



    }
}
