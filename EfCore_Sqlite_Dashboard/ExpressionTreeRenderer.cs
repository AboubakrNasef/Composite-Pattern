using System.Collections;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

public static class ExpressionTreeRenderer
{
    public static void Render<TResult>(IQueryable<TResult> source)
    {
        AnsiConsole.Write(new Rule("[yellow]EF Core query execution pipeline[/]"));
        AnsiConsole.MarkupLine("[grey]Teaching snippet:[/]");
        AnsiConsole.WriteLine("if (source.Provider.Execute<IEnumerable>(source.Expression) is IQueryingEnumerable queryingEnumerable)");
        AnsiConsole.WriteLine("    queryingEnumerable = source.Provider.Execute<IEnumerable>(source.Expression);");
        AnsiConsole.WriteLine();

        var queryingEnumerable = source.Provider.Execute<IEnumerable>(source.Expression);

        AnsiConsole.MarkupLine(
            "[cyan]Provider:[/] " +
            Markup.Escape(source.Provider.GetType().FullName ?? source.Provider.GetType().Name));
        AnsiConsole.MarkupLine(
            "[cyan]Returned object:[/] " +
            Markup.Escape(queryingEnumerable.GetType().FullName ?? queryingEnumerable.GetType().Name));
        AnsiConsole.MarkupLine("[grey]The provider returns an EF Core querying enumerable; enumeration executes the SQL.[/]");

        AnsiConsole.MarkupLine("[grey]Generated SQL:[/]");
        AnsiConsole.WriteLine(source.ToQueryString());
    }
}
