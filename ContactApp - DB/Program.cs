using ContactApp.Repositories;
using ContactApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();//kontroller ve view ekleme

builder.Services.AddDbContext<ContactDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(connectionString);
});

//Dependency Injection - Register the IContactRepository interface with db
builder.Services.AddScoped<IContactRepository, EfContactRepository>();//her istekte tek bir EFcontactRepo nesnesi kullanılacak

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Uygulama ayağa kalkarken Scoped yaşam döngüsüne sahip servisleri (DbContext gibi)
// güvenle tüketebilmek için manuel bir bağımlılık çözme kapsamı (scope) oluşturulur.
// using bloğu sona erdiğinde bu scope ve türetilen nesneler bellekten (dispose) temizlenir.
using (var scope = app.Services.CreateScope())
{
    // 1. Uygulamanın ana dizininde "App_Data" klasörünün tam yolunu oluştur
    var dataDir = Path.Combine(app.Environment.ContentRootPath, "App_Data");

    // Klasör disk üzerinde mevcut değilse oluştur (genellikle SQLite veya yerel dosya tabanlı DB'ler için)
    Directory.CreateDirectory(dataDir);

    // 2. DI konteyneri üzerinden ContactDbContext örneğini çöz (resolve et)
    var db = scope.ServiceProvider.GetRequiredService<ContactDbContext>();

    // 3. Varsa bekleyen EF Core migrasyonlarını veri tabanına uygula 
    // (Veri tabanı hiç yoksa oluşturur, varsa şemasını en son sürüme günceller)
    db.Database.Migrate();

    // 4. Başlangıç verilerini (rol, yönetici hesabı, varsayılan kayıtlar vb.) veri tabanına ekle
    DbSeeder.Seed(db);
}
app.Run();
