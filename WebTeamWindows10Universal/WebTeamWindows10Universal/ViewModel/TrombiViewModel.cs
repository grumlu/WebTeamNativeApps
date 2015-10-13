using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace WebTeamWindows10Universal.ViewModel
{
    class TrombiViewModel : ViewModelBase
    {
        public TrombiViewModel()
        {
            LookForUserCommand = new RelayCommand<string>(LookForUser);
        }
        
        public RelayCommand<string> LookForUserCommand {get;set;}

        private async void LookForUser(string userID)
        {
            try
            {
                int ID = int.Parse(userID);
            }
            catch
            {
                await DispatcherHelper.RunAsync( async () =>
                {
                    MessageDialog dialog = new MessageDialog("ID not a string.");
                    await dialog.ShowAsync();
                });
            }
        }

        
    }
}
