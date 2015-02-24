using System;
using WebTeamWindows.Ressources;
using WebTeamWindows.ViewModel;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace WebTeamWindows
{
	/// <summary>
	/// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
	/// </summary>
	public sealed partial class LoginView : Page
    {
        public LoginView()
        {
            this.InitializeComponent();
			DataContext = new LoginViewModel();
        }


#region Boutons
       private async void Connexion_Click(object sender, RoutedEventArgs e)
        {
			WebTeamWindows.Ressources.APIWebTeam.RequestToken();
        }
#endregion


		private void SettingsAbout_ApplicationBarMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }


    }


}
