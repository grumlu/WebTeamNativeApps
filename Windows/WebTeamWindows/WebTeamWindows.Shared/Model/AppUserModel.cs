using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebTeamWindows.Resources;
using WebTeamWindows.Resources.APIWebTeam;

namespace WebTeamWindows.Model
{
    class AppUserModel
    {
        private User _appUser;
        public User AppUser
        {
            get
            {
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                {
                    return new User();
                }
                return _appUser;
                    
            }
            private set
            {
                _appUser = value;
            }
        }

        public AppUserModel()
        {
            AppUser = new User();            //On évite ainsi une erreur à la création de la page
        }

        public async Task GetAppUser()
        {
            AppUser = await UserManagement.GetUserAsync();
        }

        public string Username
        {
            get
            {
                return AppUser.nom + " " + AppUser.prenom;
            }
        }

        /// <summary>
        /// Récupère la photo du profil
        /// </summary>
        /// <returns></returns>
        public async Task GetProfilePicture()
        {
            await AppUser.GetAvatar();
        }
    }



}
