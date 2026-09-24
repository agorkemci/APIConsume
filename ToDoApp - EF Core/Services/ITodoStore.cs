namespace ToDoApp.Services;
using ToDoApp.Models;

public interface ITodoStore
{
    IEnumerable<Todo> GetAll();
    IEnumerable<Todo> Search(String? term, TodoPriority? priority, bool? isDone, bool? dueDateAsc);
    Todo? Get(Guid id);
    void Add(Todo todo);
    bool Update(Todo todo);
    bool Delete(Guid id);

}

    