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

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace WebTeamWindows
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

            // TODO: si votre application comporte plusieurs pages, assurez-vous que vous
            // gérez le bouton Retour physique en vous inscrivant à l’événement
            // Événement Windows.Phone.UI.Input.HardwareButtons.BackPressed.
            // Si vous utilisez le NavigationHelper fourni par certains modèles,
            // cet événement est géré automatiquement.
        }

        #region Caligula

        private async void BeginCaligulaLogin()
        {
            /*HttpWebRequest client = (HttpWebRequest)WebRequest.Create(APICaligula.caligulaUrl);

            //Création du dictionnaire contenant les valeurs post
            Dictionary<string, string> queryString = new Dictionary<string, string>();
            queryString.Add("login", "ensea");
            queryString.Add("password", "ensea");
            string postdata = "";
            foreach (KeyValuePair<string, string> key in queryString)
            {
                postdata += key.Key + "=" + key.Value + "&";
            }

            var postBytes = APICaligula.StringToAscii(postdata);

            client.CookieContainer = new CookieContainer();
            client.Method = "POST";
            client.ContentType = "application/x-www-form-urlencoded";
            client.ContentLength = postBytes.Length;


            client.BeginGetResponse(resultat =>
            {
                HttpWebRequest request = (HttpWebRequest)resultat.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(resultat);
                System.Diagnostics.Debug.WriteLine(response.ResponseUri);

                foreach (Cookie cookieValue in response.Cookies)
                {
                    System.Diagnostics.Debug.WriteLine(cookieValue.ToString());
                }

            }, client);
            */
            MessageDialog messageDialog = new MessageDialog("Caligula arrivera dans une prochaine mise à jour");
            await messageDialog.ShowAsync();
            activerControles();


        }
        #endregion

        #region Boutons et Champs de texte
        private void Connexion_Click(object sender, RoutedEventArgs e)
        {
			WebTeamWindows.Resources.APIWebTeam.RequestToken();
        }

        private void ConnexionCaligula_Click(object sender, RoutedEventArgs e)
        {
            desactiverControles();
            BeginCaligulaLogin();
        }



        private void desactiverControles()
        {
            
        }

        private void activerControles()
        {

        }

        private void reinitialiserChamps()
        {

        }

        private void SettingsAbout_ApplicationBarMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

		#endregion

		public async void ContinueWebAuthentication(WebAuthenticationBrokerContinuationEventArgs args)
		{
			WebAuthenticationResult result = args.WebAuthenticationResult;

			if (result.ResponseStatus == WebAuthenticationStatus.Success)
			{
				System.Diagnostics.Debug.WriteLine("Cool.");
				System.Diagnostics.Debug.WriteLine(result.ResponseData.ToString());
				await APIWebTeam.GetAccessTokenAsync(result.ResponseData.ToString());
			}
			else if (result.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
			{
				MessageDialog HttpErrMsg = new MessageDialog(string.Format("There was an error connecting to Twitter: \n {0}", result.ResponseErrorDetail.ToString()), "Sorry");
				await HttpErrMsg.ShowAsync();
			}
			else
			{
				MessageDialog ErrMsg = new MessageDialog(string.Format("Error returned: \n{0}", result.ResponseStatus.ToString()), "Sorry");
				await ErrMsg.ShowAsync();
			}
		}

		

	}
}
