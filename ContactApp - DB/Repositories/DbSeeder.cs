using ContactApp.Models;

namespace ContactApp.Repositories
{
    public static class DbSeeder
    {
        public static void Seed(ContactDbContext context)
        {
            if (context.Contacts.Any())
            {
                return;
            }
            var seed1 = new List<Contact>
            {
                new Contact("John", "Doe", "john.doe@example.com", "123-456-7890", "ABC Inc.", "Manager", "Initial contact"),
                new Contact("Emily", "Clarke", "emily.clarke@example.com", "212-555-0148", "Nova Tech", "Software Engineer", "Referred by a colleague"),
                new Contact("Mehmet", "Yılmaz", "mehmet.yilmaz@example.com", "532-111-2233", "Yılmaz Holding", "Finance Director", "Met at industry conference"),
                new Contact("Sara", "Ahmed", "sara.ahmed@example.com", "070-9988-7766", "Global Logistics", "Operations Lead", "Follow up next quarter"),
                new Contact("Carlos", "Mendes", "carlos.mendes@example.com", "011-4455-6677", "Sunrise Marketing", "Creative Director", "Interested in partnership"),
            };

            context.Contacts.AddRange(seed1);
            context.SaveChanges();
        }
    }

}