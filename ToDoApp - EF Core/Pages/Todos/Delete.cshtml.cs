using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Pages.Todos
{
    public class DeleteModel : PageModel
    {

        private readonly ITodoStore _store;
        public DeleteModel(ITodoStore store)
        {
            _store = store;
        }
        [BindProperty]
        public Todo? Todo { get; set; }
        public IActionResult OnGet(Guid id)
        {
            Todo = _store.Get(id);
            if (Todo == null)
            {
                TempData["Message"] = "Todo not found.";
                return RedirectToPage("Index");
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Invalid request.";
                return RedirectToPage("Index");
            }

            if (Todo == null || Todo.Id == Guid.Empty)
            {
                TempData["Message"] = "Todo not found.";
                return RedirectToPage("Index");
            }

            var myBool = _store.Delete(Todo.Id);
            TempData["Message"] = myBool ? "Todo deleted successfully." : "Delete failed.";
            return RedirectToPage("Index");
        }
    }
}