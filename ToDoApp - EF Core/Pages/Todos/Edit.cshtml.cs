using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Pages.Todos
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Todo Todo{ get; set; }

        public readonly ITodoStore _store;
        public EditModel(ITodoStore store)
        {
            _store = store;
        }


        public IActionResult OnGet(Guid id)
        {
            var item = _store.Get(id);
            if (item == null)
            {
                TempData["Message"] = "Todo item not found.";
                return RedirectToPage("Index");
            }

            // Doğrudan sınıf seviyesindeki Todo özelliğine atayın:
            Todo = item;
            return Page();
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            if (Todo == null)
            {
                TempData["Message"] = "Todo item not found.";
                return RedirectToPage("Index");
            }
            _store.Update(Todo);
            TempData["Message"] = "Todo item updated successfully.";
            return RedirectToPage("Index");
        }
    }
}
