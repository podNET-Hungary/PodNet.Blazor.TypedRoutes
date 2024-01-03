namespace PodNet.Blazor.TypedRoutes;

/// <summary>
/// Represents a component that has a primary route, that is, a route with no required parameters.
/// </summary>
public interface INavigableComponent
{
    /// <summary>
    /// Returns the absolute page URI: <c>"{uri}"</c>. This is only possible if the page has a 
    /// primary route with no required parameters.
    /// </summary>
    public static abstract string PageUri { get; }
}
