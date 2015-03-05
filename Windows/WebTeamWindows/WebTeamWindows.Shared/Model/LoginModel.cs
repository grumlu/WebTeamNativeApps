using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebTeamWindows.Common;
using WebTeamWindows.Resources;
using WebTeamWindows.View;
using WebTeamWindows.ViewModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace WebTeamWindows.Model
{
    class LoginModel
    {
        public string GetUsername()
        {
            string output = "Bonjour, ";
            //On vérifie que l'application est loggée pour donner le nom de l'utilisateur
            if (APIWebTeam.IsConnected())
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

                var nickname = LoginViewHelper.nickname;
                var adjective = LoginViewHelper.adjective;

                output += nickname[random.Next() % nickname.Length] + " "
                    + adjective[random.Next() % adjective.Length];
            }

            return output + "!";
        }

        public bool IsChangeUsernameVisible()
        {
            return (APIWebTeam.IsConnected());
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

        public async Task<ERROR> Connect()
        {
            ERROR err = await APIWebTeam.CheckTokenAsync();

            if(err == ERROR.NOT_CONNECTED){
                 err = await APIWebTeam.RequestAccessTokenAsync();
#if WINDOWS_PHONE_APP
                return err;
#endif
            }

            if(err == ERROR.NO_ERR)
                await APIWebTeam.GetUserAsync();

            return err;

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
    }
}
