using System.ComponentModel;

namespace WebTeamWindows10Universal.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Create the RaisePropertyChanged method to raise the event 
        protected void RaisePropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        protected bool _isInDesignMode = Windows.ApplicationModel.DesignMode.DesignModeEnabled;

    }
}
