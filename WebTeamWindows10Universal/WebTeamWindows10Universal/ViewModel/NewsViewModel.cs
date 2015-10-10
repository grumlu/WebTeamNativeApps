using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebTeamWindows10Universal.Model;
using WebTeamWindows10Universal.Resources;

namespace WebTeamWindows10Universal.ViewModel
{
    class NewsViewModel : ViewModelBase
    {
        public NewsViewModel()
        {
            ArticleList = new ObservableCollection<Article>();
            NewsPageNavCommandList = new ObservableCollection<PageNavigationCommand>();
            IsLoading = true;
            GetPageAmount();
            GetArticlesInPage(1);
        }

        public Boolean IsLoading { get; set; }

        public ObservableCollection<Article> ArticleList { get; set; }
        public ObservableCollection<PageNavigationCommand> NewsPageNavCommandList { get; set; }

        public async void GetPageAmount()
        {
            if (_isInDesignMode)
            {
                for (int i = 1; i <= 3; i++)
                {
                    RelayCommand<int> rc = new RelayCommand<int>((x) =>{});
                    NewsPageNavCommandList.Add(new PageNavigationCommand(i.ToString(), rc));
                }
            }

            else
            {
                int pageAmount = 0;
                pageAmount = await Resources.APIWebTeam.NewsManagment.GetTotalPages();

                for (int i = 1; i <= pageAmount; i++)
                {
                    RelayCommand<int> rc = new RelayCommand<int>((x) =>
                    {
                        IsLoading = true;
                        RaisePropertyChanged("IsLoading");

                        ArticleList.Clear();
                        RaisePropertyChanged("ArticleList");
                        GetArticlesInPage(x);
                    });
                    NewsPageNavCommandList.Add(new PageNavigationCommand(i.ToString(), rc));
                }
            }
            RaisePropertyChanged("NewsPageNavCommandList");
        }
        public async void GetArticlesInPage(int page)
        {
            if (_isInDesignMode)
            {
                for (int i = 0; i < 15; i++)
                {
                    Article sampleArticle = new Article();
                    sampleArticle.Title = "Article " + i;
                    sampleArticle.AuthorName = "TrouDuc " + (15 - i);
                    sampleArticle.Content = "Contenu sample!Contenu sample!Contenu sample!Contenu sample!Contenu sample!Contenu sample!Contenu sample!Contenu sample!";
                    sampleArticle.ID = i;
                    sampleArticle.PostTime = DateTime.Now;
                    ArticleList.Add(sampleArticle);
                }
            }

            else
            {
                var articleList = await Resources.APIWebTeam.NewsManagment.GetArticlesListOnPage(page);

                IsLoading = false;
                RaisePropertyChanged("IsLoading");

                if(articleList != null)
                foreach (Article art in articleList)
                {
                    ArticleList.Add(art);

                    //Façon de tricher pour avoir une animation plus fluide
                    await Task.Delay(50);

                    RaisePropertyChanged("ArticleList");
                }
            }

            return;

        }

        public class PageNavigationCommand
        {
            public string Text{get; set;}

            public RelayCommand<int> Command { get; set; }

            public int PageToLoad { get; set; }
            internal PageNavigationCommand(string text, RelayCommand<int> command)
            {
                Text = text;
                Command = command;

                //Ok, je sais, c'est pas optimisé. Mais sinon les reférences posent problème
                PageToLoad = int.Parse(text);
            }
        }
    }
}
