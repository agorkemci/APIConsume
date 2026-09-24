using System.Collections.Concurrent;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class InMemoryTodoStore : ITodoStore
    {   
        private readonly ConcurrentDictionary<Guid, Todo> _items= new ConcurrentDictionary<Guid, Todo>();

        private readonly ILogger<InMemoryTodoStore> _logger;
        public InMemoryTodoStore(ILogger<InMemoryTodoStore> logger)
        {
            _logger = logger;
            Seed();
        }//dependency injection ile logger sınıfını alıyoruz. Bu sayede loglama işlemlerini gerçekleştirebiliriz.Loosly coupled bir yapı oluşturuyoruz. Bu sayede InMemoryTodoStore sınıfı, ILogger arayüzüne bağımlı hale gelir ve farklı loglama sağlayıcıları ile çalışabilir.

        private void Seed()
        {
            if(_items.Count > 0) {
                return;
            }
            // Seed the database with some initial data
            var today = DateTime.Today;
            var samples = new Todo[]
            {
                new Todo { Id = Guid.NewGuid(), Title = "Buy groceries", Description = "Milk, Bread, Eggs", Priority = TodoPriority.Medium, DueDate = today.AddDays(1), IsDone = false },
                new Todo { Id = Guid.NewGuid(), Title = "Finish project", Description = "Complete the project by the end of the week", Priority = TodoPriority.High, DueDate = today.AddDays(3), IsDone = false },
                new Todo { Id = Guid.NewGuid(), Title = "Call mom", Description = "Check in with mom and see how she's doing", Priority = TodoPriority.Low, DueDate = today.AddDays(2), IsDone = false },
                new Todo { Id = Guid.NewGuid(), Title = "Clean the house", Description = "Vacuum, dust, and mop the floors", Priority = TodoPriority.Medium, DueDate = today.AddDays(4), IsDone = false },
                new Todo { Id = Guid.NewGuid(), Title = "Exercise", Description = "Go for a run or hit the gym", Priority = TodoPriority.High, DueDate = today.AddDays(1), IsDone = false }
            };
            foreach (var todo in samples)
            {
                Add(todo);
            }
        }

        public void Add(Todo todo)
        {
            if(todo.Id == Guid.Empty)
            {
                todo.Id = Guid.NewGuid();
            }
            _items[todo.Id] = todo;
            _logger.LogInformation("Todo item added: {Id} - {Title}", todo.Id, todo.Title);
        }

        public bool Delete(Guid id)
        {
            var ok = _items.TryRemove(id,out Todo todo);
            if(ok)
            {
                _logger.LogInformation("Todo item deleted: {Id} - {Title}", todo.Id, todo.Title);
            }
            else
            {
                _logger.LogWarning("Todo item not found for deletion: {Id}", id);
            }
            return ok;
        }

        public IEnumerable<Todo> GetAll()
        {
            return _items.Values.OrderBy(t => t.DueDate);
        }

        public Todo? Get(Guid id)
        {
            _items.TryGetValue(id, out Todo? todo);
            return todo;
        }

        public IEnumerable<Todo> Search(string? term, TodoPriority? priority, bool? isDone, bool? dueDateAsc)
        {
            IEnumerable<Todo> query = _items.Values;
            if(!string.IsNullOrEmpty(term))
            {
                var term1=term.Trim();
                query=query.Where(t => t.Title.Contains(term1, StringComparison.CurrentCultureIgnoreCase) || (t.Description != null && t.Description.Contains(term1, StringComparison.CurrentCultureIgnoreCase)));
            }
            if(priority.HasValue)
            {
                query = query.Where(t => t.Priority == priority.Value);
            }
            if(isDone.HasValue)
            {
                query = query.Where(t => t.IsDone == isDone.Value);
            }
            if (dueDateAsc==false)
            {
                query = query.OrderByDescending(x => x.DueDate != null ? x.DueDate.Value : DateTime.MinValue);
            }
            else
            {
                query = query.OrderBy(x => x.DueDate != null ? x.DueDate.Value : DateTime.MaxValue);
            }
            return query;



        }

        public bool Update(Todo todo)
        {
            if (!_items.ContainsKey(todo.Id)){
                return false;
            }
            else
            {
                _items[todo.Id] = todo;
                _logger.LogInformation("Todo item updated {Id} - {Title}", todo.Id, todo.Title);
                return true;
            }
            
        }
    }
}
