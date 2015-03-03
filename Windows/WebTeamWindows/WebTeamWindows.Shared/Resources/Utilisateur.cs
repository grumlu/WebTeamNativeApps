using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WebTeamWindows.Resources
{
    public class Utilisateur
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

        public BitmapImage avatar { get; set; }

        public Utilisateur()
        {

        }

        public Utilisateur(int idUtilisateur)
        {
            id = idUtilisateur;
            /*APIWebTeam.sendRequest(RequestType.PROFIL, (Newtonsoft.Json.Linq.JObject reponse) =>
            {
                avatar = new BitmapImage(new Uri(((string)reponse["urlImageProfil"]), UriKind.Absolute));
                nom = ((string)reponse["nom"]).ToUpper();
                prenom = ((string)reponse["prenom"]);
                groupe = ((string)reponse["classe"]).ToUpper();
                numeroPortable = ((string)reponse["telephone"]);
                numeroFixe = ((string)reponse["telephoneFixe"]);
                numeroParents = ((string)reponse["telephoneParent"]);
                email = ((string)reponse["email"]);
                residence = ((string)reponse["residence"]);
                adresseParents = ((string)reponse["adresseDesParents"]);
                siteInternet = ((string)reponse["siteInternet"]);
                signature = ((string)reponse["signature"]);
                NOMPrenom = nom + " " + char.ToUpper(prenom[0]) + prenom.Substring(1).ToLower();

                dateDeNaissance = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(int.Parse(((string)reponse["dateDeNaissance"]))).ToLocalTime().ToString().Substring(0,10);
            },"&idProfil=" + idUtilisateur);*/


        }

        public async void telechargerLesInfos(Object cde)
        {
            //var retour = await APIWebTeam.sendRequest(RequestType.PROFIL, "&idProfil=" + id);
            //processInfos(retour);
                /*APIWebTeam.sendRequest(RequestType.PROFIL, (Newtonsoft.Json.Linq.JObject reponse) =>
                {
                    processInfos(reponse);
                    ((CountdownEvent)cde).Signal();
                }, "&idProfil=" + id);
            */
        }
        /*
        public void processInfos(Newtonsoft.Json.Linq.JObject reponse)
        {
            //System.Diagnostics.Debug.WriteLine("Analyse de la réponse");
            avatarURL = ((string)reponse["urlImageProfil"]);
            nom = ((string)reponse["nom"]).ToUpper();
            prenom = ((string)reponse["prenom"]);
            groupe = ((string)reponse["classe"]).ToUpper();
            numeroPortable = ((string)reponse["telephone"]);
            numeroFixe = ((string)reponse["telephoneFixe"]);
            numeroParents = ((string)reponse["telephoneParent"]);
            email = ((string)reponse["email"]);
            residence = ((string)reponse["residence"]);
            adresseParents = ((string)reponse["adresseDesParents"]);
            siteInternet = ((string)reponse["siteInternet"]);
            signature = ((string)reponse["signature"]);
            NOMPrenom = nom + " " + char.ToUpper(prenom[0]) + prenom.Substring(1).ToLower();

            DateTime dateDeNaissanceDateTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(int.Parse(((string)reponse["dateDeNaissance"])));
            dateDeNaissance = dateDeNaissanceDateTime.ToLocalTime().ToString().Substring(0, 10);
            age = (DateTime.Today.Year - dateDeNaissanceDateTime.Year) + " ans";
        }*/

        public void getAvatar()
        {
            if (avatar == null)
                avatar = new BitmapImage(new Uri(avatarURL, UriKind.Absolute));
        }

    }
}
