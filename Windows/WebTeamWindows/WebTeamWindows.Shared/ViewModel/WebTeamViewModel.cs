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
    class WebTeamViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        WebTeamModel wtModel;

        
        public WebTeamViewModel()
        {
            wtModel = new WebTeamModel();
        }

        public async Task GetAppUser()
        {
            await wtModel.GetAppUser();

        }

    }
}
