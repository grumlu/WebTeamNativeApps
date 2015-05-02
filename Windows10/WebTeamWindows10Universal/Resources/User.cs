using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WebTeamWindows10Universal.Resources
{
    public class User
    {
        public int id { get; set; }
        public string prenom { get; set; }
        public string pseudo { get; set; }
        public string nom { get; set; }
        public string promo { get; set; }
        public string groupe { get; set; }
        public DateTime dateDeNaissance { get; set; }
        public string numeroPortable { get; set; }
        public string email { get; set; }
        public string adresse { get; set; }
        public string avatarURL { get; set; }

        private BitmapImage _avatar = null;
        public BitmapImage avatar
        {
            get
            {
                return _avatar;
            }
            set
            {
                _avatar = value;
            }
        }

        public User()
        {
            id = 0;
            prenom = "Jean Michel";
            nom = "JARRE";
            promo = "53A";
            groupe = "53G12TP15";
            dateDeNaissance = DateTime.Now;
            numeroPortable = "0611111111";
            email = "dummy@stupid.sx";
            adresse = "Nowhere";

        }

        public async Task GetAvatar()
        {
            avatar = await APIWebTeam.UserManagement.GetUserImageAsync(id);
        }

    }
}
