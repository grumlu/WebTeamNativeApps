using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WebTeam_ENSEA_Universal.Resources;
using Windows.UI.ViewManagement;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace WebTeam_ENSEA_Universal
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
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
        private async void Connexion_Click(object sender, RoutedEventArgs e)
        {
            ERROR rs;
            MessageDialog messageDialog;
            desactiverControles();
            StatusBar.GetForCurrentView().ProgressIndicator.ShowAsync();
            try
            {
                rs = await APIWebTeam.BeginCheckLogin(login.Text, password.Password);
            }
            catch
            {
                messageDialog = new MessageDialog("Problème de connexion Internet. Reessayez");
                messageDialog.ShowAsync();
                rs = ERROR.TIMEOUT;
                
            }
            switch (rs)
            {

                case ERROR.INCORRECT_LOGIN_OR_PWD:
                    messageDialog = new MessageDialog("Login ou mot de passe incorrect.");
                    await messageDialog.ShowAsync();
                    goto default;
                case ERROR.NO_LOGIN_OR_PWD:
                    messageDialog = new MessageDialog("Login ou mot de passe manquant.");
                    await messageDialog.ShowAsync();
                    goto default;
                case ERROR.NO_ERR:
                    //goToWebTeamPage(this);
                    goto default;
                default:
                    StatusBar.GetForCurrentView().ProgressIndicator.HideAsync();
                    activerControles();
                    break;
            }
        }

        private void ConnexionCaligula_Click(object sender, RoutedEventArgs e)
        {
            desactiverControles();
            BeginCaligulaLogin();
        }


        /* Donne le Focus au password quand on appuie sur la touche entrée */
        private void login_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                password.Focus(FocusState.Programmatic);
            }
        }

        /* Bouton entrée = tap connexion */
        private void password_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Connexion_Click(this, null);
            }
        }

        private void desactiverControles()
        {
            login.IsEnabled = false;
            password.IsEnabled = false;
            Connexion.IsEnabled = false;
            ConnexionCaligula.IsEnabled = false;
        }

        private void activerControles()
        {
            login.IsEnabled = true;
            password.IsEnabled = true;
            Connexion.IsEnabled = true;
            ConnexionCaligula.IsEnabled = true;
        }

        private void reinitialiserChamps()
        {
            login.Text = String.Empty;
            password.Password = String.Empty;
        }

        private void SettingsAbout_ApplicationBarMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        #endregion


    }
}
