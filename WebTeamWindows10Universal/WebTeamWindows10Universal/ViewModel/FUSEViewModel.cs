using BackgroundAudioShared.Messages;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WebTeamWindows10Universal.Model;
using WebTeamWindows10Universal.Resources;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace WebTeamWindows10Universal.ViewModel
{
    class FUSEViewModel : ViewModelBase
    {


        public string Title
        {
            get { return "Radio FUSE Player"; }
        }

        private FUSEModel fuseModel = new FUSEModel();

        public RelayCommand PlayCommand { get; set; }
        public RelayCommand PauseCommand { get; set; }
        public RelayCommand StopCommand { get; set; }


        public FUSEViewModel()
        {
            PlayCommand = new RelayCommand(fuseModel.Play);
            PauseCommand = new RelayCommand(fuseModel.Pause);
            StopCommand = new RelayCommand(fuseModel.Stop);
        }

        public void setMediaElement(MediaElement meElement)
        {
            fuseModel.setMediaElement(meElement);
        }


    }
}
