using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using WebTeamWindows.Model;
using WebTeamWindows.Resources;
using WebTeamWindows.Resources.APIWebTeam;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace WebTeamWindows.ViewModel
{
    class WebTeamViewModel : ViewModelBase
    {

        ///Champs properties
        public string Title
        {
            get
            {
                return "WebTeam";
            }
        }
    }

    class AppUserViewModel : ViewModelBase
    {
        AppUserModel _user;

        public string Title
        {
            get { return "Profil"; }
        }

        public AppUserViewModel()
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
