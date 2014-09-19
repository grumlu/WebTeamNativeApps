using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTeamWindows.Resources
{
    class APICaligula
    {
        public static string caligulaUrl = "http://caligula.ensea.fr/ade/standard/index.jsp";
        public static string caligulaUrlLogin = "http://caligula.ensea.fr/ade/standard/gui/menu.jsp?projectId=1";


        //Permet de transformer une string en un tableau de byte pour l'envoi POST
        public static byte[] StringToAscii(string s)
        {
            byte[] retval = new byte[s.Length];
            for (int ix = 0; ix < s.Length; ++ix)
            {
                char ch = s[ix];
                if (ch <= 0x7f) retval[ix] = (byte)ch;
                else retval[ix] = (byte)'?';
            }
            return retval;
        }
    }
}
