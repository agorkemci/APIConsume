using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoApp.Services.NewsApi;

namespace ToDoApp.Pages.News
{
    public class DetailsModel : PageModel
    {
        private readonly INewsClient _newsClient;

        public NewsArticleDto? _newsArticleDto { get; set; }

        public DetailsModel(INewsClient newsClient)
        {
            _newsClient = newsClient;
        }

        public async Task<IActionResult> OnGet(int id,CancellationToken cancellationToken)
        {
            _newsArticleDto=await _newsClient.GetArticlesAsync(id,cancellationToken);
            if (_newsArticleDto == null)
                return NotFound();
            return Page();

        }
    }
}
