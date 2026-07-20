namespace MoneyManager.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
     public DbSet<Category> Categories => Set<Category>();
     public DbSet<Expense> Expenses => Set<Expense>();
     public DbSet<User> Users => Set<User>();

     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
         base.OnModelCreating(modelBuilder);
         
         modelBuilder.Entity<User>()
             .HasIndex(u => u.Email)
             .IsUnique();
         
         modelBuilder.Entity<Category>()
             .HasMany(c => c.Expenses)
             .WithOne(e => e.Category)
             .HasForeignKey(e => e.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
     }
}