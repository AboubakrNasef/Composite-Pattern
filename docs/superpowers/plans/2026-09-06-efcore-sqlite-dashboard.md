# EF Core SQLite Dashboard Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build a self-contained console dashboard that uses EF Core with SQLite to store categories and products, prints generated SQL for each query, and then prints the query results.

**Architecture:** Add one console project, `EfCore_Sqlite_Dashboard`, to the existing solution. Keep the example intentionally small: entity classes define the object model, `CatalogDbContext` maps it to SQLite, `CatalogQueries` owns the four demonstration queries, and `Program` renders each query's description, SQL, and results. No visitor, repository, service, or migration layer is introduced.

**Tech Stack:** .NET 10 console app, C#, Entity Framework Core 10 SQLite provider, SQLite database file, LINQ `IQueryable`, `ToQueryString()`.

**Spec:** `docs/superpowers/specs/2026-09-06-efcore-sqlite-dashboard-design.md`

## Global Constraints

- Use normal EF Core only; do not add a visitor abstraction.
- Use a local `catalog.db` file and deterministic startup seeding.
- Keep queries as `IQueryable` until `ToQueryString()` is printed and the query is enumerated.
- Print the SQL before printing each operation's results.
- Do not add migrations, repositories, or service layers.

---

### Task 1: Scaffold the project and solution entry

**Files:**
- Create: `EfCore_Sqlite_Dashboard/EfCore_Sqlite_Dashboard.csproj`
- Create: `EfCore_Sqlite_Dashboard/Program.cs`
- Modify: `CompositeFileSystem.slnx`

**Interfaces:**
- Produces a buildable `net10.0` console project named `EfCore_Sqlite_Dashboard`.
- Adds package reference `Microsoft.EntityFrameworkCore.Sqlite` version `10.0.0`.

- [ ] **Step 1: Create the project file**

Use this project definition:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="10.0.0" />
  </ItemGroup>
</Project>
```

- [ ] **Step 2: Add the project to `CompositeFileSystem.slnx`**

Add the project entry using the repository's existing solution-file format:

```xml
<Project Path="EfCore_Sqlite_Dashboard/EfCore_Sqlite_Dashboard.csproj" />
```

- [ ] **Step 3: Add a minimal entry point**

Create `Program.cs` with `Console.WriteLine("EF Core SQLite Dashboard");` so the project can be compiled before the remaining types are added.

- [ ] **Step 4: Build the new project**

Run:

```powershell
dotnet build .\EfCore_Sqlite_Dashboard\EfCore_Sqlite_Dashboard.csproj
```

Expected: exit code `0` and no compilation errors.

- [ ] **Step 5: Commit the scaffold**

```powershell
git add EfCore_Sqlite_Dashboard/EfCore_Sqlite_Dashboard.csproj EfCore_Sqlite_Dashboard/Program.cs CompositeFileSystem.slnx
git commit -m "feat: scaffold EF Core SQLite dashboard"
```

### Task 2: Add the category/product model and SQLite context

**Files:**
- Create: `EfCore_Sqlite_Dashboard/Category.cs`
- Create: `EfCore_Sqlite_Dashboard/Product.cs`
- Create: `EfCore_Sqlite_Dashboard/CatalogDbContext.cs`
- Modify: `EfCore_Sqlite_Dashboard/Program.cs`

**Interfaces:**
- `Category`: `int Id`, `string Name`, `ICollection<Product> Products`.
- `Product`: `int Id`, `string Name`, `decimal Price`, `int StockQuantity`, `int CategoryId`, `Category Category`.
- `CatalogDbContext(DbContextOptions<CatalogDbContext> options)`.
- `CatalogDbContext.Categories` and `.Products` as `DbSet<Category>` and `DbSet<Product>`.

- [ ] **Step 1: Define `Category`**

```csharp
public sealed class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
```

- [ ] **Step 2: Define `Product`**

```csharp
public sealed class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
```

- [ ] **Step 3: Configure `CatalogDbContext`**

Configure a required one-to-many relationship and decimal precision:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Category>().HasKey(category => category.Id);
    modelBuilder.Entity<Product>().HasKey(product => product.Id);
    modelBuilder.Entity<Product>().Property(product => product.Price).HasPrecision(18, 2);
    modelBuilder.Entity<Category>()
        .HasMany(category => category.Products)
        .WithOne(product => product.Category)
        .HasForeignKey(product => product.CategoryId)
        .IsRequired();
}
```

- [ ] **Step 4: Configure the SQLite options in `Program.cs`**

Use `Path.Combine(Environment.CurrentDirectory, "catalog.db")` and register the context with `UseSqlite($"Data Source={databasePath}")`.

- [ ] **Step 5: Build**

Run:

```powershell
dotnet build .\EfCore_Sqlite_Dashboard\EfCore_Sqlite_Dashboard.csproj
```

Expected: exit code `0`.

- [ ] **Step 6: Commit the model and context**

```powershell
git add EfCore_Sqlite_Dashboard
git commit -m "feat: add EF Core catalog model"
```

### Task 3: Seed deterministic catalog data

**Files:**
- Create: `EfCore_Sqlite_Dashboard/CatalogSeedData.cs`
- Modify: `EfCore_Sqlite_Dashboard/Program.cs`

**Interfaces:**
- `CatalogSeedData.Initialize(CatalogDbContext db)` creates the database schema, clears previous rows, and inserts deterministic categories/products.

- [ ] **Step 1: Implement database initialization**

Use `db.Database.EnsureDeleted()` followed by `db.Database.EnsureCreated()` so each teaching run starts from the same SQLite database without migrations.

- [ ] **Step 2: Seed these records**

Create categories `Electronics`, `Books`, and `Groceries`. Add at least two products per category, including `Wireless Keyboard`, `USB-C Hub`, `Clean Code`, `Domain-Driven Design`, `Coffee Beans`, and `Olive Oil`, with nonzero prices and stock quantities.

- [ ] **Step 3: Call the initializer before queries**

Create a context, call `CatalogSeedData.Initialize(db)`, and dispose the context after the dashboard completes.

- [ ] **Step 4: Run the project**

Run:

```powershell
dotnet run --project .\EfCore_Sqlite_Dashboard\EfCore_Sqlite_Dashboard.csproj
```

Expected: the project creates `catalog.db` and exits without an exception.

- [ ] **Step 5: Commit the seed data**

```powershell
git add EfCore_Sqlite_Dashboard
git commit -m "feat: seed SQLite catalog data"
```

### Task 4: Implement the four SQL-visible queries

**Files:**
- Create: `EfCore_Sqlite_Dashboard/CatalogQueries.cs`

**Interfaces:**
- `IQueryable<ProductWithCategory> ProductsWithCategories(CatalogDbContext db)`.
- `IQueryable<ProductWithCategory> SearchProducts(CatalogDbContext db, string term)`.
- `IQueryable<CategoryCount> CategoryProductCounts(CatalogDbContext db)`.
- `IQueryable<CategoryInventoryTotal> CategoryInventoryTotals(CatalogDbContext db)`.
- `ProductWithCategory`, `CategoryCount`, and `CategoryInventoryTotal` are immutable result records.

- [ ] **Step 1: Implement the product/category projection**

Project `Product` to `ProductWithCategory` with `ProductName`, `CategoryName`, `Price`, and `StockQuantity`, using `Select` so the query remains translatable.

- [ ] **Step 2: Implement the search query**

Reuse the projection, filter with `Contains(term)`, and order by product name.

- [ ] **Step 3: Implement category counts**

Group products by category name and project `CategoryName` plus `ProductCount`.

- [ ] **Step 4: Implement inventory totals**

Group products by category name and project `CategoryName` plus `InventoryValue = Sum(product => product.Price * product.StockQuantity)`.

- [ ] **Step 5: Build**

Run the project build and confirm the query methods compile without forcing enumeration.

- [ ] **Step 6: Commit the queries**

```powershell
git add EfCore_Sqlite_Dashboard/CatalogQueries.cs
git commit -m "feat: add SQL-visible catalog queries"
```

### Task 5: Render the dashboard with SQL before results

**Files:**
- Create: `EfCore_Sqlite_Dashboard/Dashboard.cs`
- Modify: `EfCore_Sqlite_Dashboard/Program.cs`

**Interfaces:**
- `Dashboard.Run(CatalogDbContext db)` prints all four operations in order.
- A private generic renderer accepts an operation title, a query, and a result formatter, prints `query.ToQueryString()`, then enumerates the query and prints results.

- [ ] **Step 1: Add the SQL/result renderer**

Use this ordering for every operation:

```csharp
Console.WriteLine(title);
Console.WriteLine("SQL generated by EF Core:");
Console.WriteLine(query.ToQueryString());
Console.WriteLine("Results:");
foreach (var result in query)
    Console.WriteLine(format(result));
```

- [ ] **Step 2: Add result formatters**

Print product name/category/price/stock, search matches, category counts, and currency-formatted inventory totals.

- [ ] **Step 3: Add section separators and explanations**

Print a short header explaining that LINQ is translated first and results are materialized only after the SQL is displayed.

- [ ] **Step 4: Run the complete dashboard**

Run:

```powershell
dotnet run --project .\EfCore_Sqlite_Dashboard\EfCore_Sqlite_Dashboard.csproj
```

Expected: four sections appear; each section prints SQL before rows; rows include seeded products and aggregate values.

- [ ] **Step 5: Commit the dashboard**

```powershell
git add EfCore_Sqlite_Dashboard
git commit -m "feat: add EF Core SQL dashboard"
```

### Task 6: Final verification and handoff

**Files:**
- Verify: `EfCore_Sqlite_Dashboard/EfCore_Sqlite_Dashboard.csproj`
- Verify: `EfCore_Sqlite_Dashboard/Program.cs`
- Verify: `EfCore_Sqlite_Dashboard/Category.cs`
- Verify: `EfCore_Sqlite_Dashboard/Product.cs`
- Verify: `EfCore_Sqlite_Dashboard/CatalogDbContext.cs`
- Verify: `EfCore_Sqlite_Dashboard/CatalogSeedData.cs`
- Verify: `EfCore_Sqlite_Dashboard/CatalogQueries.cs`
- Verify: `EfCore_Sqlite_Dashboard/Dashboard.cs`

- [ ] **Step 1: Build the project**

```powershell
dotnet build .\EfCore_Sqlite_Dashboard\EfCore_Sqlite_Dashboard.csproj
```

Expected: exit code `0`.

- [ ] **Step 2: Run the dashboard and capture output**

```powershell
dotnet run --project .\EfCore_Sqlite_Dashboard\EfCore_Sqlite_Dashboard.csproj
```

Confirm each operation prints `SQL generated by EF Core:` before `Results:` and that the result contains all three categories.

- [ ] **Step 3: Check the working tree**

```powershell
git status --short
git diff --check
```

Confirm there are no whitespace errors and that only the intended project, solution, database, and documentation files changed.

- [ ] **Step 4: Commit final verification changes if needed**

```powershell
git add EfCore_Sqlite_Dashboard CompositeFileSystem.slnx
git commit -m "chore: verify EF Core SQLite dashboard"
```
