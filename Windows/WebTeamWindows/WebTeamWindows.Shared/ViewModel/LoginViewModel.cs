using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebTeamWindows.Resources;
using WebTeamWindows.View;
using Windows.UI.Xaml.Media.Imaging;

namespace WebTeamWindows.ViewModel 
{
    public class LoginViewModel : INotifyPropertyChanged
	{
        private DisconnectCommand _disconnectCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginViewModel()
        {
            _disconnectCommand = new DisconnectCommand(this);
        }
		public string Username
		{
			get
			{
                if (ViewModelBase.IsInDesignModeStatic)
                    return "Nom d'utilisateur";
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

        public void Disconnect()
        {
            APIWebTeam.Disconnect();
            OnPropertyChanged("Username");
        }

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            //if (handler != null)
            //{
                handler(this, new PropertyChangedEventArgs(name));
            //}
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

        

	}

    public class DisconnectCommand : ICommand
    {
        private LoginViewModel obj;
        public DisconnectCommand(LoginViewModel _obj) 
        {
            obj = _obj;
        }

        public bool CanExecute(object parameter) // Validations
        {
            return true;
        }
        public void Execute(object parameter) // Executions
        {
            obj.Disconnect();
        }

        public event EventHandler CanExecuteChanged;
    }
}
