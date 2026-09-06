using Microsoft.EntityFrameworkCore;

var databasePath = Path.Combine(Environment.CurrentDirectory, "catalog.db");
var options = new DbContextOptionsBuilder<CatalogDbContext>()
    .UseSqlite($"Data Source={databasePath}")
    .Options;

using var db = new CatalogDbContext(options);
CatalogSeedData.Initialize(db);

Console.WriteLine("EF Core SQLite Dashboard");
Console.WriteLine($"Database: {databasePath}");
Dashboard.Run(db);

