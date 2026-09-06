using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Spectre.Console;

public static class ExpressionTreeRenderer
{
    public static void Render<TResult>(IQueryable<TResult> source)
    {
        var queryingEnumerable = source.Provider.Execute<IEnumerable>(source.Expression);
        var sql = source.ToQueryString();
        var selectExpression = CapturingQueryTranslationPostprocessor.LastSelectExpression;

        AnsiConsole.Write(new Rule("[yellow]EF Core translated SelectExpression[/]"));
        AnsiConsole.MarkupLine("[grey]Execution path:[/]");
        AnsiConsole.WriteLine("source.Provider.Execute<IEnumerable>(source.Expression)");
        AnsiConsole.WriteLine("is IQueryingEnumerable queryingEnumerable");
        AnsiConsole.WriteLine();

        AnsiConsole.MarkupLine("[cyan]Provider:[/] " + Markup.Escape(source.Provider.GetType().FullName ?? source.Provider.GetType().Name));
        AnsiConsole.MarkupLine("[cyan]Returned object:[/] " + Markup.Escape(queryingEnumerable.GetType().FullName ?? queryingEnumerable.GetType().Name));
        AnsiConsole.WriteLine();

        if (selectExpression is null)
        {
            AnsiConsole.MarkupLine("[red]No translated SelectExpression was captured.[/]");
        }
        else
        {
            RenderSelectExpression(selectExpression);
        }

        AnsiConsole.MarkupLine("[grey]Generated SQL from the translated SelectExpression:[/]");
        AnsiConsole.WriteLine(sql);
    }

    private static void RenderSelectExpression(SelectExpression selectExpression)
    {
        var tree = new Tree("[yellow]SelectExpression[/]").Guide(TreeGuide.Line);
        var root = tree.AddNode("[cyan]Relational SELECT[/]");
        AddObjectProperties(root, selectExpression, 0, new HashSet<object>(ReferenceEqualityComparer.Instance));
        AnsiConsole.Write(tree);
    }

    private static void AddObjectProperties(
        TreeNode parent,
        object value,
        int depth,
        HashSet<object> visited)
    {
        if (depth > 8 || !visited.Add(value))
        {
            parent.AddNode("[grey]...[/]");
            return;
        }

        foreach (var property in GetDisplayProperties(value.GetType()))
        {
            object? propertyValue;
            try
            {
                propertyValue = property.GetValue(value);
            }
            catch (TargetInvocationException)
            {
                continue;
            }

            var propertyNode = parent.AddNode("[green]" + Markup.Escape(property.Name) + "[/]");
            AddValue(propertyNode, propertyValue, depth + 1, visited);
        }
    }

    private static void AddValue(TreeNode parent, object? value, int depth, HashSet<object> visited)
    {
        if (value is null)
        {
            parent.AddNode("[grey]null[/]");
            return;
        }

        if (value is string || value.GetType().IsPrimitive || value is decimal || value is Type)
        {
            parent.AddNode("[grey]" + Markup.Escape(value.ToString() ?? "null") + "[/]");
            return;
        }

        if (value is IEnumerable items)
        {
            var index = 0;
            foreach (var item in items)
            {
                var itemNode = parent.AddNode("[blue]Item " + index++ + "[/]");
                AddValue(itemNode, item, depth, visited);
            }

            if (index == 0)
            {
                parent.AddNode("[grey](empty)[/]");
            }

            return;
        }

        if (value is Expression expression)
        {
            parent.AddNode("[grey]" + Markup.Escape(expression.ToString()) + "[/]");

            if (IsSqlExpression(value.GetType()) && depth <= 8)
            {
                AddObjectProperties(parent, value, depth, visited);
            }

            return;
        }

        if (IsSqlExpression(value.GetType()) && depth <= 8)
        {
            AddObjectProperties(parent, value, depth, visited);
            return;
        }

        parent.AddNode("[grey]" + Markup.Escape(value.ToString() ?? "null") + "[/]");
    }

    private static IEnumerable<PropertyInfo> GetDisplayProperties(Type type) =>
        type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.GetIndexParameters().Length == 0)
            .Where(property => property.Name is not "Type"
                and not "NodeType"
                and not "Accessors"
                and not "TypeMapping"
                and not "DebugView"
                and not "Method")
            .OrderBy(property => property.Name);

    private static bool IsSqlExpression(Type type) =>
        type.Namespace?.StartsWith("Microsoft.EntityFrameworkCore.Query.SqlExpressions", StringComparison.Ordinal) == true;
}
