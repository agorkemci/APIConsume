using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoApp.Services.NewsApi;

namespace ToDoApp.Pages.News
{
    public class IndexModel : PageModel
    {

        private readonly INewsClient _newsClient;

        public IndexModel(INewsClient newsClient)
        {
            _newsClient = newsClient;
        }

        public IEnumerable<NewsArticleDto> Articles { get; set; }   
        public async Task OnGet(CancellationToken cancellationToken)
        {
            Articles = await _newsClient.GetArticlesAsync(cancellationToken);


        }
    }
}
