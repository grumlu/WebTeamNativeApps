using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTeamWindows.Resources
{
    public class UtilisateurAppli : Utilisateur
    {
        public string userLogin { get; set; }
        public string userHashedPassword { get; set; }
        
    }
}
