using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

namespace PodNet.Blazor.TypedRoutes;

[Generator(LanguageNames.CSharp)]
public class TypedRoutesGenerator : IIncrementalGenerator
{
    private static readonly Regex s_pageDirective = new("^\\s*@page\\s+\"(?<template>/.*)\"\\s*;?\\s*$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
    private static readonly Regex s_namespaceDirective = new("^\\s*@namespace\\s+(?<namespace>[a-zA-Z_][a-zA-Z_0-9]*(?:\\.[a-zA-Z_][a-zA-Z_0-9]*)*)\\s*$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
    private static readonly Regex s_typeparamDirective = new("^\\s*@typeparam\\s+(?<typeparam>[a-zA-Z_][a-zA-Z_0-9]*)\\s*$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
    internal static readonly Regex s_routeParameters = new("/(?<parameter>{(?<catchall>\\*)?(?<name>.+?)(:(?<type>bool|datetime|decimal|double|float|guid|int|long))?(?<optional>\\?)?})", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
    private static readonly DiagnosticDescriptor s_routedComponentShouldBePartialDescriptor = new("PN1501", "Make component partial", "Add the 'partial' modifier to the class {0} so that typed routes can be generated for it", "Design", DiagnosticSeverity.Warning, true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var build = context.AnalyzerConfigOptionsProvider.Select(static (o, _) => new BuildInfo(
            o.GlobalOptions.TryGetValue("build_property.rootnamespace", out var rootNamespace) ? rootNamespace : "ASP",
            o.GlobalOptions.TryGetValue("build_property.msbuildprojectdirectory", out var projectDir) ? projectDir : ""));

        var classesWithRouteAttributes = context.SyntaxProvider.ForAttributeWithMetadataName(
            "Microsoft.AspNetCore.Components.RouteAttribute",
            static (node, _) => node is ClassDeclarationSyntax,
            static (context, cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                var symbol = (INamedTypeSymbol)context.TargetSymbol;
                var node = (ClassDeclarationSyntax)context.TargetNode;
                return new RouteInfo(
                    node,
                    symbol.ContainingNamespace?.ToDisplayString() ?? "",
                    symbol.Name,
                    context.Attributes.Select(static a => a.ConstructorArguments[0].Value?.ToString()!).ToImmutableArray(),
                    node.Modifiers.Any(static m => m.IsKind(SyntaxKind.PartialKeyword)));
            });

        var nonPartialsWithRouteAttributes = classesWithRouteAttributes.Where(static r => !r.IsPartialClass);
        context.RegisterSourceOutput(nonPartialsWithRouteAttributes,
            static (context, routeInfo) => context.ReportDiagnostic(Diagnostic.Create(s_routedComponentShouldBePartialDescriptor, routeInfo.SyntaxNode!.GetLocation(), routeInfo.ClassName)));

        var partialsWithRouteAttributes = classesWithRouteAttributes.Where(static r => r.IsPartialClass).Collect();

        var assemblyName = context.CompilationProvider.Select(static (c, _) => c.Assembly.ToDisplayString());
        var razorFiles = context.AdditionalTextsProvider.Where(static t => t.Path.EndsWith(".razor"));
        var razorFilesWithPageDirectives = razorFiles.Combine(build).SelectMany(GetRoutesFromPageDirectives).Collect();
        var pages = razorFilesWithPageDirectives
            .Combine(partialsWithRouteAttributes)
            .SelectMany(static (routePairs, _) => routePairs.Left.Concat(routePairs.Right)
                .ToLookup(static e => (e.Namespace, e.ClassName), static e => e.RouteTemplates)
                .Select(static e => new PageInfo(e.Key.Namespace, e.Key.ClassName, e
                    .SelectMany(static ts => ts.Select(static t => new Route(t, s_routeParameters.Matches(t).Cast<Match>().Select(static m => new RouteParameter(m.Groups["type"].Value is { Length: > 0 } t ? t : "string", m.Groups["name"].Value, m.Groups["catchall"].Success, m.Groups["optional"].Success)).ToImmutableArray())))
                    .ToImmutableArray())))
            .WithComparer(PageInfo.s_comparer);

        context.RegisterPostInitializationOutput(GeneratePostInitializationOutput);

        context.RegisterSourceOutput(pages, GenerateSourceOutput);

        return;

        static IEnumerable<RouteInfo> GetRoutesFromPageDirectives((AdditionalText AdditionalText, BuildInfo Build) input, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var sourceText = input.AdditionalText.GetText(cancellationToken);
            if (sourceText == null)
                yield break;

            cancellationToken.ThrowIfCancellationRequested();
            string? @namespace = null;
            string? typeparam = null;
            var templates = new List<string>();
            foreach (var textLine in sourceText.Lines)
            {
                var line = sourceText.ToString(textLine.Span);
                cancellationToken.ThrowIfCancellationRequested();
                if (typeparam == null && s_typeparamDirective.Match(line) is { Success: true } tpMatch)
                    typeparam = tpMatch.Groups["typeparam"].Value;
                if (@namespace == null && s_namespaceDirective.Match(line) is { Success: true } nsMatch)
                    @namespace = nsMatch.Groups["namespace"].Value;
                if (s_pageDirective.Match(line) is { Success: true } match)
                    templates.Add(match.Groups["template"].Value);
            }

            if (templates.Count == 0)
                yield break;

            if (@namespace == null)
            {
                if (string.IsNullOrWhiteSpace(input.Build.ProjectDirectory))
                    yield break;
                var razorFileFolder = Path.GetFullPath(Path.GetDirectoryName(input.AdditionalText.Path));
                var projectFolder = Path.GetFullPath(input.Build.ProjectDirectory);
                if (razorFileFolder?.StartsWith(projectFolder) == true)
                {
                    var relativeNamespace = new string(razorFileFolder.Substring(projectFolder.Length)
                        .Replace(Path.DirectorySeparatorChar, '.')
                        .Trim('.')
                        .Select(static c => char.IsWhiteSpace(c) ? '_' : c)
                        .Where(static c => c is '_' or '.' || char.IsLetterOrDigit(c)).ToArray());
                    @namespace = relativeNamespace.Length > 0 ? $"{input.Build.RootNamespace}.{relativeNamespace}" : input.Build.RootNamespace;
                }
                else
                    @namespace = input.Build.RootNamespace;
            }

            var @className = Path.GetFileNameWithoutExtension(input.AdditionalText.Path);
            if (typeparam != null)
                @className += $"<{typeparam}>";

            yield return new RouteInfo(null, @namespace, className, templates.ToImmutableArray(), false);
        }

        static void GeneratePostInitializationOutput(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource("_PodNet_Blazor_TypedRoutes.g.cs", """
            // <auto-generated />
            #nullable enable

            using System;
            using System.Collections.Generic;

            namespace PodNet.Blazor.TypedRoutes
            {
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

                public interface INavigableComponent
                {
                    /// <summary>
                    /// Returns the absolute page URI: <c>"{uri}"</c>. This is only possible if the page has a 
                    /// primary route with no required parameters.
                    /// </summary>
                    public static abstract string PageUri { get; }
                }
            }
            #nullable restore
            """);
        }

        static void GenerateSourceOutput(SourceProductionContext context, PageInfo page)
        {
            var parameterless = page.Routes.FirstOrDefault(static r => r.Parameters.All(static p => p.IsOptional || p.IsCatchAll));
            var primaryRoute = parameterless ?? page.Routes[0];
            var interfaces = parameterless == null ? "IRoutableComponent" : "IRoutableComponent, INavigableComponent";

            var sourceBuilder = new StringBuilder();
            sourceBuilder.AppendLine($$"""
                // <auto-generated />
                #nullable enable

                using System;
                using System.Collections.Generic;
                using System.Collections.Immutable;
                using PodNet.Blazor.TypedRoutes;

                namespace {{page.Namespace}}
                {
                    partial class {{page.ClassName}} : {{interfaces}}
                    {
                """);

            sourceBuilder.AppendLine($$"""
                        /// <summary>
                        /// The primary route template for the component, the constant string <c>"{{primaryRoute.Template}}"</c>.
                        /// </summary>
                        public static string PageRouteTemplate => "{{primaryRoute.Template}}";
                    
                        /// <summary>
                        /// All available route templates for the component, containing the string{{(page.Routes.Length > 0 ? "s" : "")}}: {{string.Join(", ", page.Routes.Select(static r => $"<c>\"{r.Template}\"</c>"))}}.
                        /// </summary>
                        public static IReadOnlyList<string> AllPageRouteTemplates { get; } = ImmutableArray.Create({{string.Join(", ", page.Routes.Select(static r => $"\"{r.Template}\""))}});

                """);

            if (parameterless != null)
            {
                // We already know parameterless has only optional or catch-all parameters (if any) as per the above check, so we can safely remove them (including the leading '/').
                var uri = parameterless.Parameters.Length == 0 ? parameterless.Template : s_routeParameters.Replace(parameterless.Template, "");
                sourceBuilder.AppendLine($$"""
                        /// <summary>
                        /// Returns the absolute page URI: <c>"{{uri}}"</c>.
                        /// </summary>
                        public static string PageUri => "{{uri}}";

                """);
            }

            // If there are additional routes or if any route has parameters, we'll generate methods for all of them.
            // We could try to be "smart" and defer generating in certain cases (or generate properties instead of methods where there are no parameters), but it's easier to reason about both at generation time and at usage time if all routes are available here, and be more explicit instead.
            // Same goes for optional method parameters, as changing the number of parameters in a route, even if optional, should be a breaking change at the consuming site.
            // It's not reasonable to statically type out the interface for these methods, because the number (ID) of the route differs and the number and type of parameters differ as well for each generated method. So while it would be possible to do, not much can be gained by having a strongly typed IPageRoutableComponent5<string, string, int>.
            if (page.Routes.Length > 1 || primaryRoute.Parameters.Length > 0)
            {
                for (int routeNumber = 1; routeNumber <= page.Routes.Length; routeNumber++)
                {
                    var route = page.Routes[routeNumber - 1];
                    var routeId = parameterless == null && page.Routes.Length == 1 ? "" : routeNumber.ToString();
                    var parameters = string.Join(", ", route.Parameters.Select(static p => $"{p.TypeName}{(p.IsOptional || p.IsCatchAll ? "?" : "")} {p.Name}{(p.IsCatchAll ? " = null" : "")}"));
                    var returnValue = s_routeParameters.Replace(route.Template, static m =>
                    {
                        var name = m.Groups["name"].Value;
                        return (m.Groups["type"].Value, m.Groups["optional"].Success || m.Groups["catchall"].Success) switch
                        {
                            ("string" or "", false) => $$"""/{Uri.EscapeDataString({{name}})}""",
                            ("string" or "", true) => $$"""/{({{name}} == null ? null : Uri.EscapeDataString({{name}}))}""",
                            ("bool", false) => $$"""/{({{name}} ? "true" : "false")}""",
                            ("bool", true) => $$"""/{({{name}} == true ? "true" : {{name}} == false ? "false" : null)}""",
                            ("datetime", false) => $$"""/{{{name}}.ToString({{name}}.TimeOfDay == default ? "yyyy-MM-dd" : "s")}""",
                            ("datetime", true) => $$"""/{{{name}}?.ToString({{name}}.Value.TimeOfDay == default ? "yyyy-MM-dd" : "s")}""",
                            _ => $"/{{{name}}}"
                        };
                    });
                    sourceBuilder.AppendLine($$"""
                        /// <summary>
                        /// Returns the URI for the page constructed from the template <c>"{{route.Template}}"</c> with
                        /// the provided parameters.
                        /// </summary>
                        public static string PageUri{{routeId}}({{parameters}}) => $"{{returnValue}}";
                            
                """);
                }
            }

            sourceBuilder.AppendLine($$"""
                    }
                }
                #nullable restore
                """);

            context.AddSource($"{$"{page.Namespace}_{page.ClassName}".Replace('.', '_')}.g.cs", sourceBuilder.ToString());
        }
    }
}