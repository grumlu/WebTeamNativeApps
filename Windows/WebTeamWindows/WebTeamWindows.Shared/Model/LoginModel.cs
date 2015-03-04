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
            IsProgressRingActive = true;
            //progressRingWebTeam.IsActive = true;

            try
            {
                ERROR err = await APIWebTeam.CheckTokenAsync();

                if (err != ERROR.NO_ERR)
                {
                    IsProgressRingActive = false;
                    //progressRingWebTeam.IsActive = false;
                    return err;
                }

                await APIWebTeam.GetUserAsync();
                return ERROR.NO_ERR;
            }
            catch (Exception excep)
            {
                var dispatcher = Window.Current.Dispatcher;
                dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    string errMsg = "Une erreur est survenue :\n" + excep.StackTrace.ToString();
                    MessageDialog dialog = new MessageDialog(errMsg);
                    await dialog.ShowAsync();
                }
                );
                IsProgressRingActive = false;
                return ERROR.ERR_UNKNOWN;
                //progressRingWebTeam.IsActive = false;
            }
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
