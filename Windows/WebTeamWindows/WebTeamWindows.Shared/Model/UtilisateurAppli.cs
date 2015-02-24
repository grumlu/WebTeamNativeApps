using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WebTeamWindows.Resources
{
	public static class UtilisateurAppli
	{
		private static Utilisateur _user;

		private static Utilisateur User {
			get { return _user;}
		}

		public static string Username
		{
			get
			{
				return _user.pseudo;
			}
		}

		public static BitmapImage Avatar
		{
			get
			{
				return _user.avatar;
			}
		}
        
    }
}
