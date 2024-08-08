namespace PodNet.Blazor.TypedRoutes;

/// <summary>
/// Represents a component that has at least one route defined.
/// </summary>
public interface IRoutableComponent
{
    /// <summary>
    /// Returns the page component's primary route template.
    /// </summary>
    public static abstract string PageRouteTemplate { get; }

    /// <summary>
    /// Returns all of the page component's route templates.
    /// </summary>
    public static abstract IReadOnlyList<string> AllPageRouteTemplates { get; }
}
