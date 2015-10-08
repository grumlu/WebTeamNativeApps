using System;
using System.Threading.Tasks;
using WebTeamWindows10Universal.Resources;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace WebTeamWindows10Universal.Model
{
    public class User
    {
        public int id { get; set; }
        public string prenom { get; set; }
        public string pseudo { get; set; }
        public string nom { get; set; }
        public string promo { get; set; }
        public string groupe { get; set; }
        public string dateDeNaissance { get; set; }
        public string numeroPortable { get; set; }
        public string email { get; set; }
        public string adresse { get; set; }

        /// <summary>
        /// Créé un utilisateur avec des valeurs par défaut
        /// </summary>
        public User()
        {
            id = 0;
            prenom = "Jean Michel";
            nom = "JARRE";
            promo = "53A";
            groupe = "53G12TP15";
            dateDeNaissance = DateTime.Now.ToString();
            numeroPortable = "0611111111";
            email = "dummy@stupid.sx";
            adresse = "Nowhere";

        }

        public async Task<BitmapImage> GetAvatar()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            BitmapImage avatar = new BitmapImage();
            try
            {
                StorageFile avatarFile = await localFolder.GetFileAsync(pseudo + "_avatar.png" );
                FileRandomAccessStream stream = (FileRandomAccessStream)await avatarFile.OpenAsync(FileAccessMode.Read);
                avatar.SetSource(stream);
            }
            catch (Exception)
            {
                // Avatar not found not found.
                var bufferAvatar = await Resources.APIWebTeam.UserManagement.GetUserImageAsyncAsBuffer(id);
                StorageFile avatarFile = await localFolder.CreateFileAsync(pseudo + "_avatar.png", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteBufferAsync(avatarFile, bufferAvatar);
                FileRandomAccessStream stream = (FileRandomAccessStream)await avatarFile.OpenAsync(FileAccessMode.Read);
                avatar.SetSource(stream);
            }

            return avatar;
        }
        
        /// <summary>
        /// Sauvegarde l'utilisateur dans l'espace de stockage local
        /// Afin d'éviter de recharcher plusieurs fois un même utilisateur
        /// </summary>
        /// <param name="userToSave"></param>
        public static async void SaveUserToLocalStorage(User userToSave)
        {
            var osh = new ObjectStorageHelper<User>(StorageType.Local);
            await osh.SaveAsync(userToSave,userToSave.pseudo);
        }

        /// <summary>
        /// Charge un utilisateur depuis l'espace de stockage local
        /// </summary>
        /// <param name="userNickname">nom de l'utilisateur à charger</param>
        /// <returns></returns>
        public static async Task<User> LoadUserFromLocalStorage(string userNickname)
        {
            var osh = new ObjectStorageHelper<User>(StorageType.Local);
            return await osh.LoadAsync(userNickname);
        }

    }
}
