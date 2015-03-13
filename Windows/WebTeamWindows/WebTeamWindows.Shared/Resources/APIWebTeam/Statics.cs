using System;
using System.Collections.Generic;
using System.Text;

namespace WebTeamWindows.Resources.APIWebTeam
{
    /// <summary>
    /// Différents types de retour
    /// </summary>
    public enum ERROR
    {
        NO_ERR,
        INCORRECT_LOGIN_OR_PWD,
        ERR_UNKNOWN,
        NOT_CONNECTED,
        AUTHENTICATION_FAILED
    }

    /// <summary>
    /// Ensemble d'informations statiques (adresses, clefs, ...)
    /// </summary>
    static class Statics
    {
        /// <summary>
        /// Adresse d'authentification de l'utilisateur
        /// </summary>
        public static string WTAuthUrl = "https://webteam.ensea.fr/oauth/v2/auth";

        /// <summary>
        /// Adresse de Callback après authentification de l'utilisateur
        /// Le retour n'est jamais chargé, mais contiendra à sa suite "/code=CODE" qu'il
        /// faut échanger avec l'accesstoken grace à WTTokenURL
        /// </summary>
        public static string WTAuthDoneUrl = "https://webteam.ensea.fr/oauth/v2/done";

        /// <summary>
        /// Recuperation du token une fois l'authentification effectuée
        /// </summary>
        public static string WTTokenUrl = "https://webteam.ensea.fr/oauth/v2/token";

        /// <summary>
        /// Demande d'un profil
        /// </summary>
        public static string WTProfileUrl = "https://webteam.ensea.fr/api/user";

        /// <summary>
        /// Demande d'un profil avec un id
        /// </summary>
        /// <param name="id">id du profil</param>
        /// <returns>L'URL demandée</returns>
        public static string WTProfileUrlByID(int id)
        {
            return "https://webteam.ensea.fr/api/users/" + id;
        }

        public static string WTClientID = "2_49cibza0l4kkwcgs8cw0cw4kok0g04oc0wcss8cc4gccockgww";

        public static string WTSecretID = "5ugzch5c28g8g0okswswk4gk448c8okw04c8c4c0kg88wkokk4";
    }
}
