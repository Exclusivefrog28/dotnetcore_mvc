using beadando.Models;

namespace beadando.Data;

public class DbInitializer{
    public static void Initialize(StoreContext context){
        context.Database.EnsureCreated();

        if (context.Categories.Any()){
            // If the database has been seeded, there is nothing to do.
            return;
        }

        var categories = new Category[]
        {
            new Category { Title = "Plüss", Description = "Plüssállatok" },
            new Category { Title = "Babajátékok", Description = "Kisbabák számára alkalmas játékok" },
            new Category { Title = "LEGO", Description = "LEGO készletek" }
        };
        foreach (var category in categories){
            context.Categories.Add(category);
        }

        context.SaveChanges();

        var toys = new Toy[]
        {
            new Toy { Title = "Krtek", Description = "Csehszlovák vakond", Price = 100, Category = categories[0] },
            new Toy { Title = "Telefon", Description = "Piros telefon", Price = 150, Category = categories[1] },
            new Toy { Title = "Színész", Description = "Színész minifigura", Price = 200, Category = categories[2] }
        };

        foreach (var toy in toys){
            context.Toys.Add(toy);
        }

        context.SaveChanges();
    }
}