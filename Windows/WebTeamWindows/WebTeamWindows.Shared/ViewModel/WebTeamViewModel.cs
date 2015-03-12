﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using WebTeamWindows.Model;
using WebTeamWindows.Resources;
using WebTeamWindows.Resources.APIWebTeam;
using Windows.UI.Xaml.Data;

namespace WebTeamWindows.ViewModel
{
    class WebTeamViewModel : ViewModelBase
    {
        
        ///Champs properties
        private string _title;
        public string Title {
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



    }
}
