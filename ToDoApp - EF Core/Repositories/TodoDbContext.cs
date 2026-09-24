using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;
using ToDoApp.Repositories.Config;

namespace ToDoApp.Repositories
{
    public class TodoDbContext : DbContext
    {
        public readonly DbSet<Todo> Todos;
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {

        }
        // EF Core'un veri tabanı modelini (tablolar, ilişkiler, kısıtlamalar vb.) ilk kez
        // belleğe çıkarırken otomatik tetiklediği metodu ezerek (override) kendi kurallarımızı tanımlıyoruz.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Todo entity'sine ait Fluent API ayarlarını (tablo adı, kolon uzunlukları, 
            // indeksler ve tohum/seed verileri) barındıran TodoConfiguration sınıfını modele uygular.
            modelBuilder.ApplyConfiguration(new TodoConfiguration());

            // Miras alınan üst DbContext sınıfının varsayılan model yapılandırmasını çalıştırır.
            // Özellikle IdentityDbContext gibi türetilmiş sınıflarda temel tablo ve anahtar 
            // ilişkilerinin bozulmaması için bu çağrının korunması kritik önem taşır.
            base.OnModelCreating(modelBuilder);
        }
    }
}
