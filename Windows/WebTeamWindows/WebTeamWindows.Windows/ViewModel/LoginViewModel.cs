using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTeamWindows.Resources;
using Windows.UI.Xaml.Media.Imaging;

namespace WebTeamWindows.ViewModel
{
	class LoginViewModel
	{
		public string Username
		{
			get
			{
				return "ldol";
			}
		}

		public BitmapImage Avatar
		{
			get
			{
				return UtilisateurAppli.Avatar;
			}
		}
	}
}
