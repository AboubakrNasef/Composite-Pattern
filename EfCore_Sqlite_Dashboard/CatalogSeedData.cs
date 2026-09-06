using Microsoft.EntityFrameworkCore;

public static class CatalogSeedData
{
    public static void Initialize(CatalogDbContext db)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var electronics = new Category { Name = "Electronics" };
        var books = new Category { Name = "Books" };
        var groceries = new Category { Name = "Groceries" };

        db.Categories.AddRange(electronics, books, groceries);
        db.Products.AddRange(
            new Product { Name = "Wireless Keyboard", Price = 49.99m, StockQuantity = 12, Category = electronics },
            new Product { Name = "USB-C Hub", Price = 29.50m, StockQuantity = 20, Category = electronics },
            new Product { Name = "Clean Code", Price = 35.00m, StockQuantity = 8, Category = books },
            new Product { Name = "Domain-Driven Design", Price = 49.99m, StockQuantity = 5, Category = books },
            new Product { Name = "Coffee Beans", Price = 14.75m, StockQuantity = 30, Category = groceries },
            new Product { Name = "Olive Oil", Price = 18.25m, StockQuantity = 16, Category = groceries });

        db.SaveChanges();
    }
}

