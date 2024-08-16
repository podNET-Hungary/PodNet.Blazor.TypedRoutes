using PodNet.Analyzers.Equality;

namespace PodNet.Blazor.TypedRoutes.Generator;

internal readonly record struct BuildInfo(
    string RootNamespace,
    string ProjectDirectory);

internal readonly record struct PageInfo(
    string Namespace,
    string ClassName,
    EquatableArray<Route> Routes);

internal readonly record struct Route(
    string Template,
    EquatableArray<RouteParameter> Parameters);

internal readonly record struct RouteInfo(
    string Namespace,
    string ClassName,
    EquatableArray<string> RouteTemplates);

internal readonly record struct RouteParameter(
    string Type,
    string Name,
    bool IsCatchAll,
    bool IsOptional)
{
    public readonly string TypeName => Type switch
    {
        "bool" or "decimal" or "double" or "float" or "int" or "long" => Type,
        "datetime" => "DateTime",
        "guid" => "Guid",
        _ => "string"
    };
};
