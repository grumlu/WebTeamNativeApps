using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTeamWindows.Resources;
using WebTeamWindows.View;
using Windows.UI.Xaml.Media.Imaging;

namespace WebTeamWindows.ViewModel 
{
    class LoginViewModel : INotifyPropertyChanged
	{
		public string Username
		{
			get
			{
                string output = "Bonjour, ";
                    //On vérifie que l'application est loggée pour donner le nom de l'utilisateur
                if (APIWebTeam.isConnected())
                {
                    var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

                    string firstName = (string)roamingSettings.Values["user_firstName"];
                    string lastName = (string)roamingSettings.Values["user_lastName"];

                    output += UppercaseFirst(firstName) + " " + lastName.ToUpper();
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
		}

		public BitmapImage Avatar
		{
			get
			{
				return UtilisateurAppli.Avatar;
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

        public event PropertyChangedEventHandler PropertyChanged;

	}
}
