using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Pages.Todos

{
    public class IndexModel : PageModel
    {
        private readonly ITodoStore _store;
        [BindProperty(SupportsGet = true)]  //Normalde BindProperty özelliği post işleminde kullanılır ancak get için kullanabilmek için true olarak atama yapmak gerekiyor.Sayfa ilk yüklendiğinde query string parametrelerini alabilmek için SupportsGet = true olarak ayarlanır. Bu sayede sayfa GET isteği ile çağrıldığında, query string parametreleri otomatik olarak q özelliğine bağlanır.
        public string? q { get; set; }
        public IEnumerable<Todo> Items { get; private set; } = Enumerable.Empty<Todo>();  //public IEnumerable<Todo> Items: Sayfanın HTML (Razor) tarafının okuyacağı vitrindir. Enumerable.Empty<Todo>() ile başlatılmasının sebebi, sayfa ilk yüklendiğinde liste null kalıp ön yüzde hata (NullReferenceException) patlatmasın diye içi boş güvenli bir liste vermektir.
        [BindProperty(SupportsGet = true)]
        public TodoPriority? priority { get; set; } = null;

        [BindProperty(SupportsGet = true)]
        public string? status { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? sort { get; set; }//due_asc or due_desc





        public IndexModel(ITodoStore store)
        {
            _store = store;
        }
        public void OnGet()
        {
            bool? isDone = null;
            switch (status?.ToLower())
            {
                case "done":
                    isDone = true;
                    break;

                case "pending":
                    isDone = false;
                    break;

                default:
                    isDone = null;
                    break;
            }
            bool? dueAsc=null;
            
            switch (sort?.ToLower())
            {
                case "due_asc":
                    dueAsc = true;
                    break;
                case "due_desc":
                    dueAsc = false;
                    break;
                default:
                    dueAsc = null;
                    break;
            }
            Items = _store.Search(q, priority, isDone, dueAsc);
        }
    }
}
