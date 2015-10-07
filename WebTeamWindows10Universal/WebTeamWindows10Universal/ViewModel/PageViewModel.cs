using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTeamWindows10Universal.ViewModel
{
    abstract class PageViewModel : ViewModelBase
    {
        public abstract String PageTitle { get; }
    }
}
