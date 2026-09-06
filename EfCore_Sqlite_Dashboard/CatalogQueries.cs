using Microsoft.EntityFrameworkCore;

public sealed record ProductWithCategory(
	string ProductName,
	string CategoryName,
	decimal Price,
	int StockQuantity);

public sealed record CategoryCount(string CategoryName, int ProductCount);

public sealed record CategoryInventoryTotal(string CategoryName, decimal InventoryValue);

public static class CatalogQueries
{
	public static IQueryable<ProductWithCategory> ProductsWithCategoriesWithFilter(CatalogDbContext db) =>
	db.Products
		.AsNoTracking()
		.OrderBy(product => product.Category.Name)
		.ThenBy(product => product.StockQuantity)
		.ThenBy(product => product.Price)
		.Where(product => product.StockQuantity > 0)
		.Where(p => p.Price > 20)
		.Select(product => new ProductWithCategory(
			product.Name,
			product.Category.Name,
			product.Price,
			product.StockQuantity));

	public static IQueryable<ProductWithCategory> ProductsWithCategories(CatalogDbContext db) =>
		db.Products
			.AsNoTracking()
			.OrderBy(product => product.Category.Name)
			.ThenBy(product => product.Name)
			.Select(product => new ProductWithCategory(
				product.Name,
				product.Category.Name,
				product.Price,
				product.StockQuantity));

	public static IQueryable<ProductWithCategory> SearchProducts(
		CatalogDbContext db,
		string term) =>
		db.Products
			.AsNoTracking()
			.Where(product => product.Name.Contains(term))
			.OrderBy(product => product.Name)
			.Select(product => new ProductWithCategory(
				product.Name,
				product.Category.Name,
				product.Price,
				product.StockQuantity));

	public static IQueryable<CategoryCount> CategoryProductCounts(CatalogDbContext db) =>
		db.Categories
			.AsNoTracking()
			.OrderBy(category => category.Name)
			.Select(category => new CategoryCount(category.Name, category.Products.Count()));

	public static IQueryable<CategoryInventoryTotal> CategoryInventoryTotals(CatalogDbContext db) =>
		db.Categories
			.AsNoTracking()
			.OrderBy(category => category.Name)
			.Select(category => new CategoryInventoryTotal(
				category.Name,
				category.Products.Sum(product => product.Price * product.StockQuantity)));
}



