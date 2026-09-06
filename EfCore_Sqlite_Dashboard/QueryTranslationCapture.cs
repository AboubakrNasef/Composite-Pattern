using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public sealed class CapturingQueryTranslationPostprocessorFactory : IQueryTranslationPostprocessorFactory
{
    private readonly QueryTranslationPostprocessorDependencies _dependencies;
    private readonly RelationalQueryTranslationPostprocessorDependencies _relationalDependencies;

    public CapturingQueryTranslationPostprocessorFactory(
        QueryTranslationPostprocessorDependencies dependencies,
        RelationalQueryTranslationPostprocessorDependencies relationalDependencies)
    {
        _dependencies = dependencies;
        _relationalDependencies = relationalDependencies;
    }

    public QueryTranslationPostprocessor Create(QueryCompilationContext queryCompilationContext) =>
        new CapturingQueryTranslationPostprocessor(
            _dependencies,
            _relationalDependencies,
            queryCompilationContext as RelationalQueryCompilationContext ??
            throw new InvalidOperationException("A relational query compilation context is required."));
}

public sealed class CapturingQueryTranslationPostprocessor
    : RelationalQueryTranslationPostprocessor
{
    public static SelectExpression? LastSelectExpression { get; private set; }

    public CapturingQueryTranslationPostprocessor(
        QueryTranslationPostprocessorDependencies dependencies,
        RelationalQueryTranslationPostprocessorDependencies relationalDependencies,
        RelationalQueryCompilationContext queryCompilationContext)
        : base(dependencies, relationalDependencies, queryCompilationContext)
    {
    }

    public override Expression Process(Expression query)
    {
        var result = base.Process(query);

        if (result is ShapedQueryExpression shaped &&
            shaped.QueryExpression is SelectExpression selectExpression)
        {
            LastSelectExpression = selectExpression;
        }

        return result;
    }
}

