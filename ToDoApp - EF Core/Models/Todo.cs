namespace ToDoApp.Models
{
    public class Todo
    {
        public Guid Id { get; set; }=new Guid();
        /*
         Çakışma İhtimali Sıfıra Yakındır: İki farklı sunucu veya kullanıcı aynı anda yeni bir görev oluştursa bile asla aynı ID üretilmez.

        Veritabanına Sormadan ID Üretilebilir: Sıralı sayılarda son ID'nin kaç olduğunu veritabanının söylemesi gerekir. GUID'de ise kod tarafında doğrudan kimlik atanabilir.

        Tahmin Edilemez (Güvenlidir): Bir URL [site.com/todo/1](https://site.com/todo/1) olduğunda, kötü niyetli biri sayıyı 2 yaparak başkasının verisini kurcalayabilir. GUID karmaşık olduğu için tahmin edilemez.

        Ekrandaki Guid.NewGuid() kodu, her yeni görev nesnesi oluşturulduğunda bu alana otomatik olarak yepyeni ve benzersiz bir kimlik atanmasını sağlar.
         */
        public string Title { get; set; }= string.Empty;

        public string? Description{ get; set; }

        public TodoPriority Priority{ get; set; }

        public DateTime? DueDate{ get; set; }

        public bool IsDone { get; set; }
    }
}
