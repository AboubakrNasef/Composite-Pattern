using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

var databasePath = Path.Combine(Environment.CurrentDirectory, "catalog.db");
var options = new DbContextOptionsBuilder<CatalogDbContext>()
    .UseSqlite($"Data Source={databasePath}")
    .ReplaceService<IQueryTranslationPostprocessorFactory, CapturingQueryTranslationPostprocessorFactory>()
    .Options;

using var db = new CatalogDbContext(options);
CatalogSeedData.Initialize(db);

Console.WriteLine("EF Core SQLite Dashboard");
Console.WriteLine($"Database: {databasePath}");
Dashboard.Run(db);


          