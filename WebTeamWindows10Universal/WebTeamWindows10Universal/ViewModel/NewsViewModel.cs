using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebTeamWindows10Universal.Model;

namespace WebTeamWindows10Universal.ViewModel
{
    class NewsViewModel : ViewModelBase
    {
        public NewsViewModel()
        {
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
