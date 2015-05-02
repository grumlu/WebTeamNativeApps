using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using WebTeamWindows.Resources;
using Windows.ApplicationModel.Activation;
using Windows.Security.Authentication.Web;
using System.Threading.Tasks;
using Windows.Web.Http;
using WebTeamWindows.ViewModel;
using WebTeamWindows.Resources.APIWebTeam;
using WebTeamWindows.Common;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace WebTeamWindows.View
{
	/// <summary>
	/// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
	/// </summary>
	public sealed partial class LoginView : Page, IWebAuthenticationContinuable
    {
        public LoginView()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoqué lorsque cette page est sur le point d'être affichée dans un frame.
        /// </summary>
        /// <param name="e">Données d’événement décrivant la manière dont l’utilisateur a accédé à cette page.
        /// Ce paramètre est généralement utilisé pour configurer la page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: préparer la page pour affichage ici.
            ((LoginViewModel)DataContext).CanPerformAction = true;

            // TODO: si votre application comporte plusieurs pages, assurez-vous que vous
            // gérez le bouton Retour physique en vous inscrivant à l’événement
            // Événement Windows.Phone.UI.Input.HardwareButtons.BackPressed.
            // Si vous utilisez le NavigationHelper fourni par certains modèles,
            // cet événement est géré automatiquement.
        }


		public async void ContinueWebAuthentication(WebAuthenticationBrokerContinuationEventArgs args)
		{
            if (args.WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
            {
                string result_string = args.WebAuthenticationResult.ResponseData.ToString();
                //extraction du token de request
                string request_token = result_string.Substring(result_string.IndexOf("code")).Split('=')[1];


                await Connection.RequestAccessTokenContinueAsync(request_token);

                await UserManagement.GetUserAsync();

                ((LoginViewModel)DataContext).NotifyUsernameChanged();
                
                NavigationService.Navigate(typeof(WebTeamView));
            }

            ((LoginViewModel)DataContext).CanPerformAction = true;
		}


	}
}
