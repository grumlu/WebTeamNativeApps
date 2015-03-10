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
        Utilisateur appUser;

        public AppUserModel()
        {
        }

        public async Task GetAppUser()
        {
            Utilisateur appUser = await UserManagement.GetUserAsync();
        }


    }

    
        
}
