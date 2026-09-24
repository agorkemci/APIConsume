using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; // Bu using eklenmeli
using ToDoApp.Models;

namespace ToDoApp.Repositories.Config
{
    public class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.ToTable("Todos");
            builder.HasKey(X=>X.Id);
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(1000);
            builder.Property(x=>x.Description)
                .HasMaxLength(1000);
            builder.Property(x => x.Priority)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(TodoPriority.Low);
            builder.Property(x => x.IsDone)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasIndex(x => x.DueDate);
            builder.HasIndex(x => x.Priority);
            builder.HasIndex(x => x.IsDone);

            var seedDate = DateTime.Today;
            builder.HasData(
            new Todo
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Title = "Set up CI/CD pipeline",
                Description = "Configure GitHub Actions workflow for automated build, test, and container deployment.",
                Priority = TodoPriority.High,
                IsDone = false,
                DueDate = seedDate.AddDays(2)
            },
            new Todo
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Title = "Implement JWT authentication",
                Description = "Add token-based authorization with refresh token rotation and role-based policies.",
                Priority = TodoPriority.High,
                IsDone = true,
                DueDate = seedDate.AddDays(-1)
            },
            new Todo
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Title = "Write unit tests for repository layer",
                Description = "Cover CRUD operations and query specifications using xUnit and Moq.",
                Priority = TodoPriority.Medium,
                IsDone = false,
                DueDate = seedDate.AddDays(5)
            },
            new Todo
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Title = "Optimize EF Core queries",
                Description = "Inspect SQL execution plans, add missing indexes, and apply AsNoTracking where appropriate.",
                Priority = TodoPriority.Medium,
                IsDone = false,
                DueDate = seedDate.AddDays(7)
            },
            new Todo
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Title = "Update API documentation",
                Description = "Document all public endpoints with request/response schemas using Swagger/OpenAPI.",
                Priority = TodoPriority.Low,
                IsDone = false,
                DueDate = seedDate.AddDays(14)
            }
        );


        }
    }
}