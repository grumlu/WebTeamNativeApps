using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using WebTeamWindows10Universal.Model;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;

namespace WebTeamWindows10Universal.Resources.APIWebTeam
{
    static class UserManagement
    {
        /// <summary>
        /// Récupération d'un User
        /// </summary>
        /// <param name="id">id de l'utilisateur. Si aucun ID n'est donné, retourne l'utilisateur de l'app</param>
        /// <returns>objet utilisateur avec les infos dedans</returns>
        public async static Task<User> GetUserAsync(int id = -1)
        {
            //Vérification de l'âge de l'access_token
            try
            {
                await APIWebTeam.Connection.CheckTokenAsync();
            }
            catch
            {
                //Token incorrect, déconnexion
                APIWebTeam.Connection.Disconnect();
            }

            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            //Préparation de l'URL de récupération de l'user suivant l'ID
            string request_url = Constants.WTProfileUrl;

            //Récupération de l'user qu'on veut
            if (id != -1)
                request_url += "s/id";

            request_url += "?";
            request_url += "access_token" + "=" + roamingSettings.Values["access_token"];

            //Récupération du JSON pour l'utilisateur
            HttpClient httpClient = new HttpClient();

            var httpResponseMessage = await httpClient.GetAsync(new Uri(request_url));
            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine(response);
            //lecture du JSON
            User user = ParseUser(response);
            
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

            //Parse de la date de naissance sans l'heure (fournie également par la WT)
           user.dateDeNaissance = ((DateTime)list["birthday"]);

            return user;

        }

        /// <summary>
        /// Télécharge l'image de profil
        /// </summary>
        /// <param name="id">L'ID de l'utilisateur</param>
        /// <returns>Un buffer contenant l'image de l'utilisateur</returns>
        public static async Task<IBuffer> GetUserImageAsyncAsBuffer(int id)
        {
            //Vérification de la connexion
            try
            {
                await APIWebTeam.Connection.CheckTokenAsync();
            }
            catch
            {
                //Token incorrect, déconnexion
                APIWebTeam.Connection.Disconnect();
            }

            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            //Préparation de l'URL
            string request_url = Constants.WTProfileUrlByID(id);
            request_url += "/photo" + "?" + "access_token" + "=" + roamingSettings.Values["access_token"];

            HttpClient client = new HttpClient();
            
            return await client.GetBufferAsync(new Uri(request_url));
        }
    }
}
