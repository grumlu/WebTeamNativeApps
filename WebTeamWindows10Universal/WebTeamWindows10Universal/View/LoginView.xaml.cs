using WebTeamWindows10Universal.ViewModel;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace WebTeamWindows10Universal.View
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class LoginView : Page
    {
        public LoginView()
        {
            this.InitializeComponent();
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Color.FromArgb(0, 183, 0, 80);
            titleBar.ButtonHoverBackgroundColor = Color.FromArgb(0, 131,0,42);
            titleBar.ButtonPressedBackgroundColor = Color.FromArgb(0, 212, 102, 150);
            titleBar.ButtonBackgroundColor = Color.FromArgb(0, 183, 0, 80);
            titleBar.ForegroundColor = Color.FromArgb(0, 255, 255, 255);
        }

        /// <summary>
        /// Les TextBlock ne prenant pas en compte les command, on doit ruser.
        /// </summary>
        private void changeUser_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ((LoginViewModel)WebTeamPanel.DataContext).Disconnect();
            ((LoginViewModel)WebTeamPanel.DataContext).ConnectCommand.Execute(null);
        }

    }


}
