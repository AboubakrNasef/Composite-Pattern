using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public sealed class CatalogMethodCallTranslatorProvider : RelationalMethodCallTranslatorProvider
{
    public CatalogMethodCallTranslatorProvider(
        RelationalMethodCallTranslatorProviderDependencies dependencies,
        ISqlExpressionFactory sqlExpressionFactory)
        : base(dependencies)
    {
        AddTranslators(
        [
            new CatalogMethodCallTranslator(sqlExpressionFactory)
        ]);
    }
}

public sealed class CatalogMethodCallTranslator : IMethodCallTranslator
{
    private static readonly MethodInfo IsExpensiveMethod =
        typeof(CatalogQueries).GetMethod(
            nameof(CatalogQueries.IsExpensive),
            BindingFlags.Public | BindingFlags.Static)!;

    private readonly ISqlExpressionFactory _sqlExpressionFactory;

    public CatalogMethodCallTranslator(ISqlExpressionFactory sqlExpressionFactory)
    {
        _sqlExpressionFactory = sqlExpressionFactory;
    }

    public SqlExpression? Translate(
        SqlExpression? instance,
        MethodInfo method,
        IReadOnlyList<SqlExpression> arguments,
        IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (method != IsExpensiveMethod)
        {
            return null;
        }

        var price = arguments[0];
        var threshold = _sqlExpressionFactory.Constant(100m, price.TypeMapping);
        var zero = _sqlExpressionFactory.Constant(0);

        var comparison = _sqlExpressionFactory.Function(
            "ef_compare",
            [price, threshold],
            nullable: false,
            argumentsPropagateNullability: [true, true],
            returnType: typeof(int),
            typeMapping: zero.TypeMapping);

        return _sqlExpressionFactory.GreaterThan(comparison, zero);
    }
}

