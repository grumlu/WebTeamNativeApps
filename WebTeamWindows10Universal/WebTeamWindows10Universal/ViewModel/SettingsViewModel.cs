using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTeamWindows10Universal.Resources;
using WebTeamWindows10Universal.View;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WebTeamWindows10Universal.ViewModel
{
    class SettingsViewModel : ViewModelBase
    {
        public RelayCommand DisconnectCommand
        {
            get; set;
        } = new RelayCommand(Disconnect);

        public string Version { get
            {
                var version = Package.Current.Id.Version;
                return version.Major + "."
                + version.Minor + "." + version.Build + "." + version.Revision;
            }
        }
        private static void Disconnect()
        {
            Resources.APIWebTeam.Connection.Disconnect();
        }
    }
}
