using WebTeamWindows10Universal.ViewModel;
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
