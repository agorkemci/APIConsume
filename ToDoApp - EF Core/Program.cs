using Microsoft.EntityFrameworkCore;
using ToDoApp.Repositories;
using ToDoApp.Services;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
builder.Services.AddRazorPages();
//Uygulamanın herhangi bir yerinde birisi ITodoStore arayüzünü (interface) talep ederse, ona InMemoryTodoStore sınıfından oluşturduğun tek bir örneği (instance) ver.
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(connectionString)
);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<ITodoStore,InMemoryTodoStore>();
}
else
{
    builder.Services.AddScoped<ITodoStore,InMemoryTodoStore>();
}
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
        db.Database.Migrate();
    }
    catch(Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
        logger.LogError(ex, "Database Migration failed.");
    }
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

//app.MapStaticAssets();
//app.MapRazorPages()
// .WithStaticAssets();
app.MapRazorPages();
app.Run();
