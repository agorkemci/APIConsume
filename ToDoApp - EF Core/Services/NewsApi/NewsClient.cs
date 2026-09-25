using ToDoApp.Services.NewsApi;

namespace ToDoApp.Services.NewsApi
{
    public class NewsClient:INewsClient
    {
        private readonly HttpClient _httpClient;
        public NewsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    

    public async Task<IEnumerable<NewsArticleDto>> GetArticlesAsync(CancellationToken cancellationToken)
    {
        var articles = await _httpClient.GetFromJsonAsync<IEnumerable<NewsArticleDto>>("api/NewArticles",cancellationToken);
        return articles;
    }

    public async Task<NewsArticleDto?> GetArticlesAsync(int id, CancellationToken cancellationToken)
    {
            var article = await _httpClient.GetFromJsonAsync<NewsArticleDto?>($"api/NewsArticles/{id}", cancellationToken);
            if(article== null)  
                return null;
            return article;
    }
}
}
