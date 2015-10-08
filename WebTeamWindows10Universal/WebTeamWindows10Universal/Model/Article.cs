using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTeamWindows10Universal.Model
{
    class Article
    {
        public int ID { get; set; }
        public DateTime PostTime { get; set; }
        public string AuthorName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

    }
}
