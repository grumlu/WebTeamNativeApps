using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTeamWindows10Universal.Resources;

namespace WebTeamWindows10Universal.ViewModel
{
    class NewsViewModel : PageViewModel
    {
        public override string PageTitle
        {
            get { return "Actualités"; }
        }

        public NewsViewModel()
        {
            RaisePropertyChanged("PageTitle");
            GetArticles();
        }

        public ObservableCollection<Article> ArticleList { get; set; }

        public async Task GetArticles()
        {
            ObservableCollection<Article> articleList = new ObservableCollection<Article>();
            if (_isInDesignMode)
            {
                for (int i = 0; i < 15; i++)
                {
                    articleList.Add(new Article { Title = "Article " + i });
                }
            }

            else
            {
                await WebTeamWindows10Universal.Resources.APIWebTeam.NewsManagment.GetArticlesListOnPage(1);
            }

            if (articleList.Count > 0)
            {
                ArticleList = articleList;
            }
                return;
            
        }
    }
}
