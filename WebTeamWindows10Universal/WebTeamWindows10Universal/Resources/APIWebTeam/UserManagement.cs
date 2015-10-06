using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;

namespace WebTeamWindows10Universal.Resources.APIWebTeam
{
    static class UserManagement
    {
        /// <summary>
        /// Récupération d'un User
        /// </summary>
        /// <param name="id">id de l'utilisateur</param>
        /// <returns>objet utilisateur avec les infos dedans</returns>
        public async static Task<User> GetUserAsync(int id = -1)
        {
            //Vérification de l'âge de l'access_token
            if (await APIWebTeam.Connection.CheckTokenAsync() != ERROR.NO_ERR)
                return null;

            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            //Préparation de l'URL de récupération de l'user suivant l'ID
            string request_url = Statics.WTProfileUrl;

            //Récupération de l'user qu'on veut
            if (id != -1)
                request_url += "s/id";
                
            request_url += "?";
            request_url += "access_token" + "=" + roamingSettings.Values["access_token"];

            //Récupération du JSON pour l'utilisateur
            HttpClient httpClient = new HttpClient();

            var httpResponseMessage = await httpClient.GetAsync(new Uri(request_url));
            string response = await httpResponseMessage.Content.ReadAsStringAsync();

            //lecture du JSON
            User user = ParseUser(response);

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
        private static User ParseUser(string jsonString)
        {
            JObject list = JObject.Parse(jsonString);

            User user = new User();

            user.email = (string)list["email"];
            user.nom = (string)list["last_name"];
            user.prenom = (string)list["first_name"];
            user.numeroPortable = (string)list["phone"];
            user.adresse = (string)list["address"];
            user.groupe = (string)list["group"];
            user.id = (int)list["id"];
            user.pseudo = (string)list["username"];
            user.promo = (string)list["promo"];

            string strippedBirthdate = ((string)list["birthday"]).Substring(0, 10);
            user.dateDeNaissance = DateTime.ParseExact(strippedBirthdate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            //user.dateDeNaissance = DateTime.Parse((string)list["birthday"]);

            return user;

        }

        /// <summary>
        /// Télécharge l'image de profil
        /// </summary>
        /// <param name="id">L'ID de l'utilisateur</param>
        /// <returns>Le BitmapImage de l'utilisateur</returns>
        public static async Task<BitmapImage> GetUserImageAsync(int id)
        {
            //Vérification de la connexion
            if (await APIWebTeam.Connection.CheckTokenAsync() != ERROR.NO_ERR)
            {
                APIWebTeam.Connection.Disconnect();
                (App.Current as App).NavigationService.Navigate(typeof(WebTeamWindows10Universal.View.LoginView));
                return null;
            }

            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            BitmapImage image;

            //Préparation de l'URL
            string request_url = Statics.WTProfileUrlByID(id);
            request_url += "/photo" + "?" + "access_token" + "=" + roamingSettings.Values["access_token"];

            image = new BitmapImage(new Uri(request_url));

            return image;
        }
    }
}
