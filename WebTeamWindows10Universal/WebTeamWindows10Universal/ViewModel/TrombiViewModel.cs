using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTeamWindows10Universal.View;
using Windows.UI.Popups;
using Windows.UI.Xaml;

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

            await DispatcherHelper.RunAsync(() =>
           {
               ((WebTeamShell)Window.Current.Content).AppFrame.Navigate(
                   typeof(UserView),
                   userID,
                   new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
           });
            
        }

        
    }
}
