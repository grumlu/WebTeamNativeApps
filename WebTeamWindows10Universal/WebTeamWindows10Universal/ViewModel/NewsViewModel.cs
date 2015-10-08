using System;
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
            ObservableCollection<Article> articleList;
            if (_isInDesignMode)
            {
                articleList = new ObservableCollection<Article>();
                for (int i = 0; i < 15; i++)
                {
                    Article sampleArticle = new Article();
                    sampleArticle.Title = "Article " + i;
                    sampleArticle.AuthorName = "TrouDuc " + (15 - i);
                    sampleArticle.Content = "Contenu sample!Contenu sample!Contenu sample!Contenu sample!Contenu sample!Contenu sample!Contenu sample!Contenu sample!";
                    sampleArticle.ID = i;
                    sampleArticle.PostTime = DateTime.Now;
                    articleList.Add(sampleArticle);
                }
            }

            else
            {
                articleList = new ObservableCollection<Article>(await Resources.APIWebTeam.NewsManagment.GetArticlesListOnPage(1));
            }

            if (articleList.Count > 0)
            {
                ArticleList = articleList;
                RaisePropertyChanged("ArticleList");
            }
            return;

        }
    }
}
