namespace ToDoApp.Services.NewsApi
{
    public interface INewsClient
    {
        Task<IEnumerable<NewsArticleDto>> GetArticlesAsync(CancellationToken cancellationToken);

        Task<NewsArticleDto?> GetArticlesAsync(int id, CancellationToken cancellationToken);

    }
}
