using Windows.UI.Xaml.Media.Imaging;
using WebTeamWindows10Universal.Resources;

namespace WebTeamWindows10Universal.Model
{
	public static class UtilisateurAppli
	{
		private static User _user;

		private static User User {
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
