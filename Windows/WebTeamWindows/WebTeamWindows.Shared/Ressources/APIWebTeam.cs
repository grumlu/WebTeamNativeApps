using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Net.Http;
using Windows.UI.Popups;
using Windows.Security.Authentication.Web;

namespace WebTeamWindows.Ressources
{
    /// <summary>
    ///  Différents types de requête
    /// </summary>
    public enum RequestType
    {
        LOGIN,
        APPLI_UPDATE,
        ANNIVERSAIRE,
        BOITEDERECEPTION,
        BOITEDENVOI,
        BOITE_MESSAGERIE_SERVICES,
        PROFIL,
        MONPROFIL,
        RAGOTSLECTURE,
        RAGOTSENVOI,
        TROMBI
    }

    /// <summary>
    /// Différents types de retour
    /// </summary>
    public enum ERROR
    {
        NO_ERR,
        NO_LOGIN_OR_PWD,
        INCORRECT_LOGIN_OR_PWD,
        TIMEOUT,
        ERR_UNKNOWN
    }

    /// <summary>
    /// Classe permettant de faciliter les requêtes à la WT au sein de l'application
    /// </summary>
    public static class APIWebTeam
	{
		public static string WTAuthUrl = "https://webteam.ensea.fr/oauth/v2/auth";

		public static string WTAuthDoneUrl = "https://webteam.ensea.fr/oauth/v2/done";

		public static string WTTokenUrl = "https://webteam.ensea.fr/oauth/v2/token";

		public static string WTClientID = "2_49cibza0l4kkwcgs8cw0cw4kok0g04oc0wcss8cc4gccockgww";

		public static string WTSecretID = "5ugzch5c28g8g0okswswk4gk448c8okw04c8c4c0kg88wkokk4";

		public static async Task<bool> initiateConnection(string login, string password)
        {
            ERROR rs;
            MessageDialog messageDialog;
            try
            {
                rs = await APIWebTeam.BeginCheckLogin(login, password);
            }
            catch
            {
                rs = ERROR.TIMEOUT;
            }
            switch (rs)
            {

                case ERROR.INCORRECT_LOGIN_OR_PWD:
                    messageDialog = new MessageDialog("Login ou mot de passe incorrect.");
                    await messageDialog.ShowAsync();
                    break;
                case ERROR.NO_LOGIN_OR_PWD:
                    messageDialog = new MessageDialog("Login ou mot de passe manquant.");
                    await messageDialog.ShowAsync();
                    break;
                case ERROR.TIMEOUT:
                    messageDialog = new MessageDialog("Problème de connexion Internet. Reessayez");
                    await messageDialog.ShowAsync();
                    break;
                case ERROR.NO_ERR:
                    //goToWebTeamPage(this);
                    return true;
                default:
                    break;
            }
            return false;
        }

		public static async void RequestToken()
		{
			string WeCASUrl = WTAuthUrl;
			WeCASUrl += "?" + "client_id=" + WTClientID;
			WeCASUrl += "&" + "response_type=code";
			WeCASUrl += "&" + "scope=user";
			WeCASUrl += "&" + "redirect_uri=" + WTAuthDoneUrl;

			try
			{
#if WINDOWS_APP
				WebAuthenticationResult webAuthenticationResult =
					await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.UseTitle, new Uri(WeCASUrl), new Uri(WTAuthDoneUrl));
#endif
#if WINDOWS_PHONE_APP
				//string oAuth_Token = await GetWeCASRequestTokenAsync(WeCASCallBackUri, WeCASConsumerKey);


				System.Diagnostics.Debug.WriteLine(WeCASUrl);

				WebAuthenticationBroker.AuthenticateAndContinue(new Uri(WeCASUrl), new Uri(WTAuthDoneUrl));
#endif
			}
			catch (Exception)
			{
				// do something when an exception occurred
			}

		}

		
		/*public static async Task<Newtonsoft.Json.Linq.JObject> sendRequest(RequestType requestType, string getSupplementaire = "", string post = "")
        {
            var applicationData = Windows.Storage.ApplicationData.Current;

            var localSettings = applicationData.LocalSettings;

            string PageURI = "page=";
            switch (requestType)
            {
                case RequestType.LOGIN:
                    PageURI += "login";
                    break;
                case RequestType.ANNIVERSAIRE:
                    PageURI += "anniversaire";
                    break;
                case RequestType.APPLI_UPDATE:
                    PageURI += "appli_update";
                    break;
                case RequestType.BOITEDENVOI:
                    PageURI += "boite_d_envoi";
                    break;
                case RequestType.BOITEDERECEPTION:
                    PageURI += "boite_de_reception";
                    break;
                case RequestType.MONPROFIL:
                    PageURI += "ficheEleve&idProfil=self";
                    break;
                case RequestType.PROFIL:
                    PageURI += "ficheEleve";
                    break;
                case RequestType.RAGOTSENVOI:
                    PageURI += "ragots";
                    break;
                case RequestType.RAGOTSLECTURE:
                    PageURI += "ragots";
                    break;
                case RequestType.TROMBI:
                    PageURI += "trombinoscope";
                    break;
                case RequestType.BOITE_MESSAGERIE_SERVICES:
                    PageURI += "boite_messagerie_services";
                    break;
            }


            HttpClient httpClient = new HttpClient();
            string request = "http://webteam.ensea.fr/api/?" + PageURI + "&pseudo=" + localSettings.Values["login"] + "&mdp=" + localSettings.Values["hashedPwd"] + "&appli=" + App.APLLICATION_NAME + getSupplementaire;
            StringContent stringContent = new StringContent(post, Encoding.UTF8, "application/x-www-form-urlencoded");
            System.Diagnostics.Debug.WriteLine(stringContent.Headers);
            System.Diagnostics.Debug.WriteLine(await stringContent.ReadAsStringAsync());
            var response = await httpClient.PostAsync(new Uri(request), stringContent);

            string content = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine("Q: " + request);
            System.Diagnostics.Debug.WriteLine("R: " + content);

            Newtonsoft.Json.Linq.JObject list;
            try
            {
                list = Newtonsoft.Json.Linq.JObject.Parse(content);
            }
            catch
            {
                list = Newtonsoft.Json.Linq.JObject.Parse(content.Substring(1));
            }


            /*WebClient client = new WebClient();
            client.UploadStringCompleted += client_UploadStringCompleted;
            client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            
            string requete = "http://webteam.ensea.fr/api/?" + PageURI + "&pseudo=" + ((App)Application.Current).getUtilisateurAppli().userLogin + "&mdp=" + ((App)Application.Current).getUtilisateurAppli().userHashedPassword + "&appli=" + App.APLLICATION_NAME + "&iso88591" + getSupplementaire;
            client.UploadStringAsync(new Uri(requete), "POST", post, callback);
            System.Diagnostics.Debug.WriteLine("Demande à la WebTeam : " + requete);
            TimerCallback timerCallback = c =>
            {
                WebClient webClient = c as WebClient;
                if (!webClient.IsBusy) { return; }
                webClient.CancelAsync();
            };
            Timer timerDownload = new Timer(timerCallback, client, App.TIMEOUTMS, 0);*/

		/*return list;
    }*/


		/*private static void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            Newtonsoft.Json.Linq.JObject list;
            if (e.Cancelled == true)
                System.Diagnostics.Debug.WriteLine("TimeOut");
            else
            {

                //On parse la réception
                try
                {
                    list = Newtonsoft.Json.Linq.JObject.Parse(e.Result);
                }
                catch
                {
                    list = Newtonsoft.Json.Linq.JObject.Parse(e.Result.Substring(1));
                }
                // parse response to get the rate value
                System.Diagnostics.Debug.WriteLine("Réponse de la WebTeam : " + e.Result);

                // if a callback was specified, call it passing the rate.
                var callback = (Action<Newtonsoft.Json.Linq.JObject>)e.UserState;
                if (callback != null)
                    callback(list);
            }
        }*/

		public static async Task<ERROR> BeginCheckLogin(string login, string password)
        {
            //Utilisation des paramètres locaux pour garder les infos
            var applicationData = Windows.Storage.ApplicationData.Current;

            var localSettings = applicationData.LocalSettings;

            if (login == "")
            {
                return ERROR.NO_LOGIN_OR_PWD;
            }

            else if (password == "")
            {
                return ERROR.NO_LOGIN_OR_PWD;
            }

            else
            {
                //Refus des redirections
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.AllowAutoRedirect = false;

                //Création du httpClient pour envoi des données
                HttpClient httpClient = new HttpClient(httpClientHandler);
                httpClient.BaseAddress = new Uri("https://webteam.ensea.fr");

                //Variables post
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("_username", login),
                    new KeyValuePair<string, string>("_password", password),
                    new KeyValuePair<string, string>("_apiKey", "55340427148d33f57ee48be681fb4868f9fef60ad90b30033739e983939a9689d7a8f37db8403212c83db9bca89c6119982654387836e14132ced2a9a9ff07dc")
                });

                //Envoi et enregistrement de la réponse
                var response = httpClient.PostAsync("/api_login_check", content).Result;

                Newtonsoft.Json.Linq.JObject list;
                
                //Si le code de retour indique OK, le MDP est juste
                if (response.StatusCode == System.Net.HttpStatusCode.OK){
                    string jsonReturn = await response.Content.ReadAsStringAsync();
                    list = Newtonsoft.Json.Linq.JObject.Parse(jsonReturn);
                }
                  
                //MDP Faux ==> redirection. on renvoit une erreur
                else if (response.StatusCode == System.Net.HttpStatusCode.Redirect)
                    return ERROR.INCORRECT_LOGIN_OR_PWD;

                //Erreur inconnue
                return ERROR.ERR_UNKNOWN;
            }

        }




        /* Mise à jour des informations utilisateur une fois la connexion réussie*/
        /*private void chargementPagePrincipale()
        {
            APIWebTeam.sendRequest(RequestType.MONPROFIL, (Newtonsoft.Json.Linq.JObject reponse) =>
            {
                progressIndicator.Value = 0.60;
                System.Diagnostics.Debug.WriteLine("Téléchargement des infos de l'utilisateur de l'application effectué");
                utilisateurAppli.processInfos(reponse);
                utilisateurAppli.getAvatar();

                progressIndicator.Value = 0.75;
                progressIndicator.Text = "Mise à jour anniversaires";


                App.AnniversaryViewModel.mettreAJourListeAnniversaires(() =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        progressIndicator.Value = 1;
                        SystemTray.SetProgressIndicator(this, null);
                        NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                    });
                });
            });
        }*/

    }


}
