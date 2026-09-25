using ContactApp.Controllers;
using ContactApp.Repositories;
using ContactApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();//kontroller ve view ekleme

builder.Services.AddDbContext<ContactDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    // If the connection uses a relative App_Data path, make it absolute so EF uses the same file
    if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("Data Source=App_Data", StringComparison.OrdinalIgnoreCase))
    {
        var relative = connectionString.Substring("Data Source=".Length).Trim();
        var absolutePath = Path.Combine(builder.Environment.ContentRootPath, relative.Replace('/', Path.DirectorySeparatorChar));
        var finalConnectionString = $"Data Source={absolutePath}";
        options.UseSqlite(finalConnectionString);
    }
    else
    {
        options.UseSqlite(connectionString);
    }
});

//Dependency Injection - Register the IContactRepository interface with db
builder.Services.AddScoped<IContactRepository, EfContactRepository>();//her istekte tek bir EFcontactRepo nesnesi kullanılacak
var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
builder.Services.AddHttpClient<INewsService, NewsService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);//Inewservice çağrıldığında newsservice otomatik olarak dönerken client ifadesi için base url i doğrudan çözümlüyor


});
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
    ;

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

    try
    {
        // If SQLite WAL/SHM files exist from previous runs, remove them to avoid file locks
        var dbFile = Path.Combine(dataDir, "contacts.db");
        var walFile = dbFile + "-wal";
        var shmFile = dbFile + "-shm";
        if (File.Exists(walFile)) File.Delete(walFile);
        if (File.Exists(shmFile)) File.Delete(shmFile);

        // 3. Varsa bekleyen EF Core migrasyonlarını veri tabanına uygula
        db.Database.Migrate();

        // 4. Başlangıç verilerini (rol, yönetici hesabı, varsayılan kayıtlar vb.) veri tabanına ekle
        DbSeeder.Seed(db);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Database migration/seed failed");
        throw;
    }
}
app.Run();
