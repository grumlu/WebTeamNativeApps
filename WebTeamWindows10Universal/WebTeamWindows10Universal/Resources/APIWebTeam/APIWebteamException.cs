using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTeamWindows10Universal.Resources.APIWebTeam
{
    class APIWebteamException : Exception
    {
        /// <summary>
        /// Différents types de retour
        /// </summary>
        public enum ERROR
        {
            NO_ERR,
            ERR_UNKNOWN,
            WEBTEAM_UNAVAILABLE,
            NOT_CONNECTED,
            AUTHENTICATION_FAILED
        }

        public ERROR Error { get; private set; }

        public APIWebteamException(ERROR err) : base()
        {
            Error = err;
        }
    }
}
