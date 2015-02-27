using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Net.Http;
using Windows.UI.Popups;
using Windows.Security.Authentication.Web;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace WebTeamWindows.Resources
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
        /// <summary>
        /// Ensemble de liens utiles pour accéder à la Webteam
        /// </summary>
        public struct Links
        {
            /// <summary>
            /// Adresse d'authentification de l'utilisateur
            /// </summary>
            public static string WTAuthUrl = "https://webteam.ensea.fr/oauth/v2/auth";

            /// <summary>
            /// Adresse de Callback après authentification de l'utilisateur
            /// Le retour n'est jamais chargé, mais contiendra à sa suite "/code=CODE" qu'il
            /// faut échanger avec l'accesstoken grace à WTTokenURL
            /// </summary>
            public static string WTAuthDoneUrl = "https://webteam.ensea.fr/oauth/v2/done";

            /// <summary>
            /// Recuperation du token une fois l'authentification effectuée
            /// </summary>
            public static string WTTokenUrl = "https://webteam.ensea.fr/oauth/v2/token";

            /// <summary>
            /// Demande d'un profil
            /// </summary>
            public static string WTProfileUrl = "https://webteam.ensea.fr/api/profile";
        }

        public static string WTClientID = "2_49cibza0l4kkwcgs8cw0cw4kok0g04oc0wcss8cc4gccockgww";

        public static string WTSecretID = "5ugzch5c28g8g0okswswk4gk448c8okw04c8c4c0kg88wkokk4";

        /// <summary>
        /// Demande du token s'il n'est pas déjà donné
        /// </summary>
        /// <returns>Erreur de connexion</returns>
        /// TODO : vérifier la présence d'un refresh_token pour se reconnecter automatiquement
		private static async Task<ERROR> RequestToken()
		{
			string WeCASUrl = Links.WTAuthUrl;
            WeCASUrl += "?" + "client_id=" + WTClientID;
			WeCASUrl += "&" + "response_type=code";
			WeCASUrl += "&" + "scope=user";
            WeCASUrl += "&" + "redirect_uri=" + Links.WTAuthDoneUrl;

			try
			{
#if WINDOWS_APP
				WebAuthenticationResult webAuthenticationResult =
					await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, new Uri(WeCASUrl), new Uri(Links.WTAuthDoneUrl));

				if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
				{
                    //La réponse du serveur
					string response = webAuthenticationResult.ResponseData;
                    //extraction du token de request
                    string request_token = response.Substring(response.IndexOf("code")).Split('=')[1];

                    //Récupération de la réponse du serveur
                    var jsonResponse = await GetAccessTokenAsync(request_token);

                    //plutot explicite
                    ParseTokenAndStore(jsonResponse);
                    return ERROR.NO_ERR;
				}
				else if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
				{
					// do something when the request failed
                    return ERROR.ERR_UNKNOWN;
				}
				else
				{
					// do something when an unknown error occurred
                    return ERROR.ERR_UNKNOWN;
				}
#endif
#if WINDOWS_PHONE_APP
				//string oAuth_Token = await GetWeCASRequestTokenAsync(WeCASCallBackUri, WeCASConsumerKey);
				System.Diagnostics.Debug.WriteLine(WeCASUrl);

				WebAuthenticationBroker.AuthenticateAndContinue(new Uri(WeCASUrl), new Uri(WTAuthDoneUrl));

                return ERROR.NO_ERR;
#endif
            }
			catch (Exception)
			{
				// do something when an exception occurred
                return ERROR.ERR_UNKNOWN;
			}

		}

        /// <summary>
        /// Récupère l'access_token, et autres fioritures à partir du token d'authorisation après avoir entré le login
        /// </summary>
        /// <param name="request_token">réponse du serveur</param>
        /// <returns></returns>
        private static async Task<string> GetAccessTokenAsync(string request_token)
		{
            //Préparation de l'URL de demande du token
            string request_url = APIWebTeam.Links.WTTokenUrl + "?";

            request_url += "client_id" + "=" + APIWebTeam.WTClientID;
            request_url += "&" + "client_secret" + "=" + APIWebTeam.WTSecretID;
			request_url += "&" + "grant_type" + "=" + "authorization_code";
            request_url += "&" + "redirect_uri" + "=" + APIWebTeam.Links.WTAuthDoneUrl;
			request_url += "&" + "code" + "=" + request_token;

            //Récupération du JSON avec le token
			HttpClient httpClient = new HttpClient();

			var httpResponseMessage = await httpClient.GetAsync(new Uri(request_url));
			string response = await httpResponseMessage.Content.ReadAsStringAsync();

            return response;
        }

        /// <summary>
        /// Parse la réponse du serveur WebTeam et range ça dans les settings de l'app
        /// </summary>
        /// <param name="jsonString">réponse du serveur WT</param>
        private static void ParseTokenAndStore(string jsonString){
            Newtonsoft.Json.Linq.JObject list = Newtonsoft.Json.Linq.JObject.Parse(jsonString);

            //Enregistrement des valeurs dans les settings de l'app
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
           
            roamingSettings.Values["access_token"] = list.Value<string>("access_token");
            roamingSettings.Values["refresh_token"] = list.Value<string>("refresh_token");
            
            //expiration date
            DateTime expirationDate = DateTime.Now.AddSeconds(list.Value<double>("expires_in"));
            roamingSettings.Values["expiration_date"] = expirationDate.Ticks;
        }

        public static async Task<ERROR> CheckToken()
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            //S'il n'y a plus d'access_token, ou si on a pas de refresh, on recommence toute la procédure
            if((roamingSettings.Values["access_token"] == null) ||
                (roamingSettings.Values["refresh_token"] == null) ||
                (roamingSettings.Values["expiration_date"] == null))
            {
                ERROR err;
                if ((err = await RequestToken()) != ERROR.NO_ERR)
                {
                    return err;
                }

            }

            ///Sinon, on vérifie que le token est toujours valable.
            ///Le cas contraire, on fait un refresh
            else if (DateTime.Now.Ticks > (long)roamingSettings.Values["expiration_date"])
            {
                string request_url = Links.WTAuthUrl + "?";
                request_url += "client_id" + "=" + WTClientID;
                request_url += "&" + "client_secret" + "=" + WTSecretID;
                request_url += "&" + "grand_type" + "=" + "refresh_token";
                request_url += "&" + "refresh_token" + "=" + roamingSettings.Values["refresh_token"];


                HttpClient httpClient = new HttpClient();

                var httpResponseMessage = await httpClient.GetAsync(new Uri(request_url));
                string response = await httpResponseMessage.Content.ReadAsStringAsync();

                ParseTokenAndStore(response);

                return ERROR.NO_ERR;
            }

            ///Sinon tout va bien
            return ERROR.NO_ERR;

        }
    
        /// <summary>
        /// Récupération d'un User
        /// </summary>
        /// <param name="id">id de l'utilisateur</param>
        /// <returns>objet utilisateur avec les infos dedans</returns>
        /// TODO : une fois que l'API prendra en charge d'autres utilisateurs, les prendre aussi
        public async static Task<Utilisateur> GetUser(int id = -1)
        {
            //Vérification de l'âge de l'access_token
            if (await CheckToken() != ERROR.NO_ERR)
                return null;
           
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            //Préparation de l'URL de récupération de l'user suivant l'ID
            string request_url = APIWebTeam.Links.WTProfileUrl + "?";

            request_url += "access_token" + "=" + roamingSettings.Values["access_token"];

            //Récupération du JSON pour l'utilisateur
            HttpClient httpClient = new HttpClient();

            var httpResponseMessage = await httpClient.GetAsync(new Uri(request_url));
            string response = await httpResponseMessage.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine(response);

            //lecture du JSON
            Utilisateur user = ParseUser(response);

            //Si on cherche l'utilisateur de l'application, on entre son nom dans les settings de l'app
            if (id == -1)
            {
                roamingSettings.Values["user_firstName"] = user.prenom;
                roamingSettings.Values["user_lastName"] = user.nom;
            }

            return user;
        }

        /// <summary>
        /// Parse la réponse d'un user de la webteam pour la transformer en utilisateur
        /// </summary>
        /// <param name="json_user">réponse du serveur</param>
        /// <returns>un user tout frais</returns>
        private static Utilisateur ParseUser(string jsonString)
        {
            JObject list = JObject.Parse(jsonString);

            Utilisateur user = new Utilisateur();

            user.email = (string)list["email"];
            user.nom = (string)list["name"]["lastName"];
            user.prenom = (string)list["name"]["firstName"];
            user.numeroPortable = (string)list["phone"];
            user.adresse = (string)list["address"];
            user.groupe = (string)list["group"];
            user.id = (int)list["id"];
            user.pseudo = (string)list["username"];
            user.promo = (string)list["promo"];
            user.avatarURL = (string)list["photo"];

            user.dateDeNaissance = DateTime.ParseExact((string)list["birthday"]["date"], "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);

            return user;

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
