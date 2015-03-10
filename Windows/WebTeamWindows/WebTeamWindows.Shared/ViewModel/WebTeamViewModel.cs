using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using WebTeamWindows.Model;
using WebTeamWindows.Resources;
using WebTeamWindows.Resources.APIWebTeam;

namespace WebTeamWindows.ViewModel
{
    class WebTeamViewModel : ViewModelBase
    {
        AppUserModel wtModel;
                
        public WebTeamViewModel()
        {
            wtModel = new AppUserModel();
        }

        public async Task GetAppUser()
        {
            await wtModel.GetAppUser();

        }

        ///Champs properties
        private string _title = "Bite";
        public string Title {
            get
            {
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                    return "Bite";
                return "Bite";
            }
            private set
            {
                _title = value;
            }
        }


    }
}
