using Microsoft.EntityFrameworkCore;
using beadando.Models;

namespace beadando.Data;

public class StoreContext : DbContext{
    public StoreContext(DbContextOptions<StoreContext> options) : base(options){ }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Toy> Toys { get; set; }
    
    public DbSet<Downloadable> Downloadables { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<Category>().ToTable("Categories");
        modelBuilder.Entity<Toy>().ToTable("Toys");
        modelBuilder.Entity<Downloadable>().ToTable("Downloadables");
    }
}