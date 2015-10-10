using System;
using System.Threading.Tasks;
using WebTeamWindows10Universal.Resources;
using WebTeamWindows10Universal.Resources.APIWebTeam;

namespace WebTeamWindows10Universal.Model
{
    class LoginModel
    {
        public string GetUsername()
        {
            string output = "Bonjour, ";
            //On vérifie que l'application est loggée pour donner le nom de l'utilisateur

            if (Connection.IsConnected())
            {
                var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

                string firstName = (string)roamingSettings.Values["user_firstName"];
                string lastName = (string)roamingSettings.Values["user_lastName"];

                output += UppercaseFirst(firstName) + " " + UppercaseFirst(lastName);
            }
            //Génération d'un nom aléatoire
            else
            {
                var random = new Random();

                output += nickname[random.Next() % nickname.Length] + " "
                    + adjective[random.Next() % adjective.Length];
            }

            return output + "!";
        }

        public bool IsChangeUsernameVisible()
        {
            return (Connection.IsConnected());
        }

        private bool isProgressRingActive = false;
        public bool IsProgressRingActive
        {
            get
            {
                return isProgressRingActive;
            }
            set
            {
                isProgressRingActive = value;
            }
        }

        public async Task Connect()
        {
            try
            {
                await Connection.CheckTokenAsync();
            }
            catch (APIWebteamException exception)
            {
                if (exception.Error == APIWebteamException.ERROR.NOT_CONNECTED)
                    await Connection.RequestAccessTokenAsync();
                else
                    return;
            }

            //Récupération de l'utilisateur de l'application
            User user = await UserManagement.GetUserAsync();

            //Enregistrement de l'utilisateur dans le cloud
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["user_firstName"] = user.prenom;
            roamingSettings.Values["user_lastName"] = user.nom;
            roamingSettings.Values["user_nickname"] = user.pseudo;

            //Enregistrement de l'objet user en local pour ne pas le recharger plus tard
            User.SaveUserToTemporaryStorage(user);
        }

        /// <summary>
        /// Met la première lettre en maj
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Input avec 1ère lettre en maj</returns>
        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }


        public static string[] nickname ={
                                   "eucalyptus",
                                   "panda",
                                   "maître",
                                   "connard",
                                   "étudiant",
                                   "ourson",
                                   "loup",
                                   "rat",
                                   "débile",
                                   "mangeur de chaussettes",
                                   "petit ange",
                                   "anchois",
                                   "couile de tetard",
                                   "primate",
                                   "parasite",
                                   "cafard",
                                   "cochon sauvage",
                                   "affabulateur",
                                   "pithécantrope",
                                   "petit boufon",
                                   "cancrelat",
                                   "mythocondriaque"
                               };

        public static string[] adjective = {
                                             "en chaleur",
                                             "doux",
                                             "vert",
                                             "doré",
                                             "argenté",
                                             "sauvage",
                                             "inutile",
                                             "sexy",
                                             "qui pue",
                                             "qui sent bon",
                                             "qu'on aime fort",
                                             "boutonneux",
                                             "analphabète",
                                             "beau gosse",
                                             "jovial",
                                             "transexuel",
                                             "pénis",
                                             "tête de bite",
                                             "mignonne"
                                        };
    }
}
