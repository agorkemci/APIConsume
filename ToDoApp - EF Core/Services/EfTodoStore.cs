using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ToDoApp.Models;
using ToDoApp.Repositories;

namespace ToDoApp.Services
{
    public class EfTodoStore : ITodoStore
    {
        private TodoDbContext _db { get; set; }
        private ILogger<EfTodoStore> _logger { get; set; }

        public EfTodoStore(TodoDbContext db, ILogger<EfTodoStore> logger)
        {
            _db= db;
            _logger= logger;
        }
        public void Add(Todo todo)
        {
            if(todo.Id.Equals(Guid.Empty))
                todo.Id=Guid.NewGuid();
            _db.Todos.Add(todo);
            _db.SaveChanges();
            _logger.LogInformation($"Operation succeed:{todo.Title}");
        }

        public bool Delete(Guid id)
        {
            var entity = _db.Todos.FirstOrDefault(c => c.Id == id);
            if (entity == null)
                return false;
            _db.Todos.Remove(entity);
            return true;
        }

        public Todo? Get(Guid id)
        {
            var entity = _db.Todos.AsNoTracking().FirstOrDefault(c => c.Id.Equals(id));
            return entity;
        }

        public IEnumerable<Todo> GetAll()
        {
            return _db.Todos.AsNoTracking().OrderBy(x => x.DueDate).ToList();
        }

        public IEnumerable<Todo> Search(string? term, 
            TodoPriority? priority,
            bool? isDone, 
            bool? dueDateAsc)
        {
            IQueryable<Todo> q=_db.Todos.AsNoTracking();
            if (!string.IsNullOrEmpty(term))
            {
                var t = term.Trim();
                q = q.Where(x => (x.Title != null && EF.Functions.Like(x.Title, $"%{t}%")) || (x.Description != null && EF.Functions.Like(x.Description, $"%{t}%")));
             
            }
            if (priority.HasValue)
            {
                q=q.Where(x=>x.Priority.Equals(priority.Value));
            }
            if (isDone.HasValue) {
                q = q.Where(x => x.IsDone.Equals(isDone.Value));
            }
            q = dueDateAsc == false
                ? q.OrderByDescending(x => x.DueDate ?? DateTime.MinValue): q.OrderBy(x=>x.DueDate?? DateTime.MaxValue);
            return q.ToList();
        }

        public bool Update(Todo todo)
        {
            var status= _db.Todos.Any(x=>x.Id==todo.Id);
            if (status == false) 
                return status;
            else
            {
                _db.Todos.Update(todo);
                _db.SaveChanges();
                return true;
            }
        }
    }
}
