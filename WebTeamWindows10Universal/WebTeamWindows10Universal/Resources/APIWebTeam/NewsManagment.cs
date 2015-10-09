using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebTeamWindows10Universal.Model;

namespace WebTeamWindows10Universal.Resources.APIWebTeam
{
    class NewsManagment
    {
        /// <summary>
        /// !!UNTESTED CODE!!
        /// Récupère le nombre de page total
        /// </summary>
        /// <returns></returns>
        public async static Task<int> GetTotalPages()
        {
            //Vérification de l'âge de l'access_token
            if (await APIWebTeam.Connection.CheckTokenAsync() != ERROR.NO_ERR)
                return -1;

            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            //Préparation de l'URL pour récupérer la première page. La réponse contient le nombre de pages au total
            string request_url = Constants.WTArticlePageUrl(1);

            request_url += "?";
            request_url += "access_token" + "=" + roamingSettings.Values["access_token"];

            //Récupération du JSON réponse
            HttpClient httpClient = new HttpClient();

            var httpResponseMessage = await httpClient.GetAsync(new Uri(request_url));
            string response = await httpResponseMessage.Content.ReadAsStringAsync();

            //Parse de la réponse
            JObject list = JObject.Parse(response);

            return (int)list["total_pages"];
        }

        public async static Task<List<Article>> GetArticlesListOnPage(int pageNumber)
        {
            //Vérification de l'âge de l'access_token
            if (await APIWebTeam.Connection.CheckTokenAsync() != ERROR.NO_ERR)
                return null;

            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            //Préparation de l'URL pour récupérer la première page. La réponse contient le nombre de pages au total
            string request_url = Constants.WTArticlePageUrl(pageNumber);

            request_url += "?";
            request_url += "access_token" + "=" + roamingSettings.Values["access_token"];

            //Récupération du JSON réponse
            HttpClient httpClient = new HttpClient();

            var httpResponseMessage = await httpClient.GetAsync(new Uri(request_url));
            string response = await httpResponseMessage.Content.ReadAsStringAsync();

            //Parse de la réponse
            JObject list = JObject.Parse(response);

            List<Article> articleList = new List<Article>();

            foreach(JToken token in list["articles"].Children())
            {
                Article article = new Article();
                article.ID = (int)token["id"];
                if ((bool)token["is_from_asso"])
                {
                    article.AuthorName = (string)(token["asso_author"]["name"]);
                }
                article.Title = (string)token["title"];
                article.Content = Windows.Data.Html.HtmlUtilities.ConvertToText((string)token["content"]);

                articleList.Add(article);
            }
            return articleList;
        }
    }
}
