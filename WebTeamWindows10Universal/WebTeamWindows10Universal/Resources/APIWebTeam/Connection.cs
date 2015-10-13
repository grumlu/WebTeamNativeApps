using System;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace WebTeamWindows10Universal.Resources.APIWebTeam
{
    public static class Connection
    {
        /// <summary>
        /// Demande de l'access_token et enregistrement dans les settings
        /// </summary>
        public static async Task RequestAccessTokenAsync()
        {
            string WeCASUrl = Constants.WTAuthUrl;
            WeCASUrl += "?" + "client_id=" + Constants.WTClientID;
            WeCASUrl += "&" + "response_type=code";
            WeCASUrl += "&" + "scope=user";
            WeCASUrl += "&" + "redirect_uri=" + Constants.WTAuthDoneUrl;

            WebAuthenticationResult webAuthenticationResult =
                await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, new Uri(WeCASUrl), new Uri(Constants.WTAuthDoneUrl));

            if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
            {
                //La réponse du serveur
                string response = webAuthenticationResult.ResponseData;
                //extraction du token de request
                string request_token = response.Substring(response.IndexOf("code")).Split('=')[1];

                await RequestAccessTokenContinueAsync(request_token);
            }

            else
                throw new APIWebteamException(APIWebteamException.ERROR.AUTHENTICATION_FAILED);
        }


        /// <summary>
        /// Renvoie le retour serveur pour parser
        /// </summary>
        /// <param name="access_code">le code d'accès fourni après la phase 1 d'authentification</param>
        /// <returns>Erreur lors de la récupération</returns>
        public static async Task RequestAccessTokenContinueAsync(string access_code)
        {
            //Demande de l'access_token
            var server_answer = await GetAccessTokenFromCodeAsync(access_code);

            ParseTokenAndStore(server_answer);
        }


        /// <summary>
        /// Récupère l'access_token, et autres fioritures à partir du token d'authorisation après avoir entré le login
        /// Le retour est renvoyé brut (JSON)
        /// </summary>
        /// <param name="request_token">réponse du serveur</param>
        /// <returns>Réponse JSON du serveur</returns>
        private static async Task<string> GetAccessTokenFromCodeAsync(string request_token)
        {
            //Préparation de l'URL de demande du token
            string request_url = Constants.WTTokenUrl + "?";

            request_url += "client_id" + "=" + Constants.WTClientID;
            request_url += "&" + "client_secret" + "=" + Constants.WTSecretID;
            request_url += "&" + "grant_type" + "=" + "authorization_code";
            request_url += "&" + "redirect_uri" + "=" + Constants.WTAuthDoneUrl;
            request_url += "&" + "code" + "=" + request_token;

            //Récupération du JSON avec le token
            HttpClient httpClient = new HttpClient();
            
            try
            {
                var httpResponseMessage = await httpClient.GetStringAsync(new Uri(request_url));
                return httpResponseMessage;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace.ToString());
            }

            return null;

        }

        /// <summary>
        /// Parse la réponse du serveur WebTeam et range ça dans les settings de l'app
        /// </summary>
        /// <param name="jsonString">réponse du serveur WT</param>
        private static void ParseTokenAndStore(string jsonString)
        {
            Newtonsoft.Json.Linq.JObject list;
            try
            {
                list = Newtonsoft.Json.Linq.JObject.Parse(jsonString);
            }
            catch
            {
                throw new APIWebteamException(APIWebteamException.ERROR.WEBTEAM_UNAVAILABLE);
            }

            //S'il y a une erreur
            if (list.Value<string>("error") != null)
            {
                throw new APIWebteamException(APIWebteamException.ERROR.ERR_UNKNOWN);
            }

            //Enregistrement des valeurs dans les settings de l'app
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            roamingSettings.Values["access_token"] = list.Value<string>("access_token");
            roamingSettings.Values["refresh_token"] = list.Value<string>("refresh_token");

            //expiration date
            DateTime expirationDate = DateTime.Now.AddSeconds(list.Value<double>("expires_in"));
            roamingSettings.Values["expiration_date"] = expirationDate.Ticks;
        }

        public static async Task CheckTokenAsync()
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            //S'il n'y a plus d'access_token, ou si on a pas de refresh, on recommence toute la procédure
            if (!IsConnected())
            {
                throw new APIWebteamException(APIWebteamException.ERROR.NOT_CONNECTED);
            }

            ///Sinon, on vérifie que le token est toujours valable.
            ///Le cas contraire, on fait un refresh
            else if (DateTime.Now.Ticks > (long)roamingSettings.Values["expiration_date"])
            {
                string request_url = Constants.WTTokenUrl + "?";
                request_url += "client_id" + "=" + Constants.WTClientID;
                request_url += "&" + "client_secret" + "=" + Constants.WTSecretID;
                request_url += "&" + "grant_type" + "=" + "refresh_token";
                request_url += "&" + "refresh_token" + "=" + roamingSettings.Values["refresh_token"];


                HttpClient httpClient = new HttpClient();

                var httpResponseMessage = await httpClient.GetAsync(new Uri(request_url));
                string response = await httpResponseMessage.Content.ReadAsStringAsync();

                ParseTokenAndStore(response);
            }
        }

        /// <summary>
        /// Vérifie que les informations de connexion sont bien gardées dans les paramètres
        /// de l'application
        /// </summary>
        /// <returns>True si l'application est connectée</returns>
        public static bool IsConnected()
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            return ((roamingSettings.Values.ContainsKey("access_token") &&
                roamingSettings.Values.ContainsKey("refresh_token") &&
                roamingSettings.Values.ContainsKey("expiration_date")));
        }

        /// <summary>
        /// Retire l'utilisateur courant (pour changer se déconnecter)
        /// </summary>
        public static void Disconnect()
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            roamingSettings.Values["user_firstName"] = null;
            roamingSettings.Values["user_lastName"] = null;

            roamingSettings.Values["access_token"] = null;

            //Nettoyer l'historique et charger la page de connexion si on n'est pas déjà dessus
            //(App.Current as App).NavigationService.
            var navService = (App.Current as App).NavigationService;
            if (!navService.CurrentPageKey.Equals("LoginView"))
            {
                Frame frame = new Frame();
                frame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
                //(App.Current as App).NavigationService = new NavigationService.NavigationService(frame);

                Window.Current.Content = frame;
                frame.Navigate(typeof(View.LoginView));
            }
        }

    }
}
