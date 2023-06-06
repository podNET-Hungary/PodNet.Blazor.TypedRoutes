using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PodNet.Blazor.TypedRoutes;

internal record BuildInfo(
    string RootNamespace,
    string ProjectDirectory);

internal record PageInfo(
    string Namespace,
    string ClassName,
    ImmutableArray<Route> Routes)
{
    public static readonly IEqualityComparer<PageInfo> s_comparer = new PageInfoComparer();
    private class PageInfoComparer : IEqualityComparer<PageInfo>
    {
        public bool Equals(PageInfo x, PageInfo y) =>
            x is not null && y is not null
                && x.Namespace == y.Namespace
                && x.ClassName == y.ClassName
                && x.Routes.Length == y.Routes.Length
                && x.Routes.Select(static r => r.Template).SequenceEqual(y.Routes.Select(static r => r.Template));

        public int GetHashCode(PageInfo page) =>
            $"{page.Namespace}\n{page.ClassName}\n{string.Join("\n", page.Routes.Select(r => r.Template))}".GetHashCode();
    }
};

internal record Route(
    string Template,
    ImmutableArray<RouteParameter> Parameters);

internal record RouteInfo(
    ClassDeclarationSyntax? SyntaxNode,
    string Namespace,
    string ClassName,
    ImmutableArray<string> RouteTemplates,
    bool IsPartialClass);

internal record RouteParameter(
    string Type,
    string Name,
    bool IsCatchAll,
    bool IsOptional)
{
    public string TypeName => Type switch
    {
        "bool" or "decimal" or "double" or "float" or "int" or "long" => Type,
        "datetime" => "DateTime",
        "guid" => "Guid",
        _ => "string"
    };
};
