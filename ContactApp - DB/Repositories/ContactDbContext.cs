using ContactApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactApp.Repositories
{
    public class ContactDbContext:DbContext
    {
        public  DbSet<Contact> Contacts{ get; set; }
        public ContactDbContext(DbContextOptions<ContactDbContext> options): base(options)
        {
            
        }

        //Doğrudan nesneyi üretmiyoruz,DbContextOptions ile yapılandırılmış bir nesne üzerinden üretim yapıyoruz. Aldığımız options nesnesini Üst sınıfa DBcontexte iletmek için base(options) çağrısı yapıyoruz. Bu sayede DI (Dependency Injection) ile context nesnesini kullanabiliyoruz.

    }
}
