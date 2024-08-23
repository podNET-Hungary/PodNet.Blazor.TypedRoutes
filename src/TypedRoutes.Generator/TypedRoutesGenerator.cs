using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PodNet.Analyzers.CodeAnalysis;
using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

namespace PodNet.Blazor.TypedRoutes.Generator;

[Generator(LanguageNames.CSharp)]
public class TypedRoutesGenerator : IIncrementalGenerator
{
    private static readonly Regex s_pageDirective = new("^\\s*@page\\s+\"(?<template>/.*)\"\\s*;?\\s*$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
    private static readonly Regex s_namespaceDirective = new("^\\s*@namespace\\s+(?<namespace>[a-zA-Z_][a-zA-Z_0-9]*(?:\\.[a-zA-Z_][a-zA-Z_0-9]*)*)\\s*$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
    private static readonly Regex s_typeparamDirective = new("^\\s*@typeparam\\s+(?<typeparam>[a-zA-Z_][a-zA-Z_0-9]*)\\s*", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
    internal static readonly Regex s_routeParameters = new("/(?<parameter>{(?<catchall>\\*)?(?<name>.+?)(:(?<type>bool|datetime|decimal|double|float|guid|int|long|nonfile))?(?<optional>\\?)?})", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
    private static readonly Regex hintNameSanitizer = new("[\\.<>]", RegexOptions.Compiled);
    public static readonly DiagnosticDescriptor RoutedComponentShouldBePartialDescriptor = new("PN1501", "Make component partial", "Add the 'partial' modifier to the class {0} so that typed routes can be generated for it", "Design", DiagnosticSeverity.Warning, true);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var build = context.AnalyzerConfigOptionsProvider.Select(static (o, _) => new BuildInfo(
            o.GlobalOptions.GetRootNamespace() ?? "ASP",
            o.GlobalOptions.GetProjectDirectory() ?? ""));

        var classesWithRouteAttributes = context.SyntaxProvider.ForAttributeWithMetadataName<DiagnosticsOrResults<RouteInfo>>(
            "Microsoft.AspNetCore.Components.RouteAttribute",
            static (node, _) => node is ClassDeclarationSyntax,
            static (context, cancellationToken) =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                var symbol = (INamedTypeSymbol)context.TargetSymbol;
                var className = symbol.Name;
                if (symbol.TypeParameters is not [] typeParameters)
                    className = $"{className}<{string.Join(", ", typeParameters.Select(t => t.Name))}>";
                var node = (ClassDeclarationSyntax)context.TargetNode;
                if (!node.Modifiers.Any(static m => m.IsKind(SyntaxKind.PartialKeyword)))
                    return Diagnostic.Create(RoutedComponentShouldBePartialDescriptor, node.GetLocation(), className);
                return new RouteInfo(
                    symbol.ContainingNamespace?.ToDisplayString() ?? "",
                    className,
                    context.Attributes.Select(static a => a.ConstructorArguments[0].Value?.ToString()!).ToImmutableArray());
            });

        context.ReportDiagnostics(classesWithRouteAttributes);

        var partialsWithRouteAttributes = classesWithRouteAttributes.SelectResults().Collect();

        var assemblyName = context.CompilationProvider.Select(static (c, _) => c.Assembly.ToDisplayString());
        var razorFiles = context.AdditionalTextsProvider.Where(static t => t.Path.EndsWith(".razor"));
        var razorFilesWithPageDirectives = razorFiles.Combine(build).SelectMany(GetRoutesFromPageDirectives).Collect();
        var pages = razorFilesWithPageDirectives
            .Combine(partialsWithRouteAttributes)
            .SelectMany(static (routePairs, _) => routePairs.Left.Concat(routePairs.Right)
                .ToLookup(static e => (e.Namespace, e.ClassName), static e => e.RouteTemplates)
                .Select(static e => new PageInfo(e.Key.Namespace, e.Key.ClassName, e
                    .SelectMany(static ts => ts.Select(static t => new Route(t, s_routeParameters.Matches(t).Cast<Match>().Select(static m => new RouteParameter(m.Groups["type"].Value is { Length: > 0 } t ? t : "string", m.Groups["name"].Value, m.Groups["catchall"].Success, m.Groups["optional"].Success)).ToImmutableArray())))
                    .ToImmutableArray())));

        context.RegisterSourceOutput(pages, GenerateSourceOutput);

        return;

        static IEnumerable<RouteInfo> GetRoutesFromPageDirectives((AdditionalText AdditionalText, BuildInfo Build) input, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (input.AdditionalText.GetText(cancellationToken) is not { } sourceText)
                yield break;

            cancellationToken.ThrowIfCancellationRequested();
            string? @namespace = null;
            var typeparams = new List<string>();
            var templates = new List<string>();
            foreach (var textLine in sourceText.Lines)
            {
                var line = sourceText.ToString(textLine.Span);
                cancellationToken.ThrowIfCancellationRequested();
                if (s_pageDirective.Match(line) is { Success: true } match)
                    templates.Add(match.Groups["template"].Value);
                else if (s_typeparamDirective.Match(line) is { Success: true } tpMatch)
                    typeparams.Add(tpMatch.Groups["typeparam"].Value);
                else if (@namespace == null && s_namespaceDirective.Match(line) is { Success: true } nsMatch)
                    @namespace = nsMatch.Groups["namespace"].Value;
            }

            if (templates.Count == 0)
                yield break;

            if (@namespace == null)
            {
                if (!string.IsNullOrWhiteSpace(input.Build.ProjectDirectory))
                {
                    var relativePath = PathProcessing.GetRelativePath(input.Build.ProjectDirectory, Path.GetDirectoryName(input.AdditionalText.Path) ?? ".");
                    @namespace = TextProcessing.GetNamespace(relativePath?.Length > 0 && !relativePath.StartsWith("..") ? $"{input.Build.RootNamespace}.{relativePath}" : input.Build.RootNamespace);
                }
                else
                    @namespace = input.Build.RootNamespace;
            }
            var @className = TextProcessing.GetClassName(Path.GetFileNameWithoutExtension(input.AdditionalText.Path));
            if (typeparams.Count > 0)
                @className += $"<{string.Join(", ", typeparams)}>";

            yield return new RouteInfo(@namespace, className, templates.ToImmutableArray());
        }

        static void GenerateSourceOutput(SourceProductionContext context, PageInfo page)
        {
            var parameterless = page.Routes.FirstOrDefault(static r => r.Parameters.All(static p => p.IsOptional || p.IsCatchAll));
            var isParameterless = parameterless != default;
            var primaryRoute = isParameterless ? parameterless : page.Routes[0];
            var interfaces = isParameterless ? "IRoutableComponent, INavigableComponent" : "IRoutableComponent";

            var sourceBuilder = new StringBuilder();
            sourceBuilder.AppendLine($$"""
                // <auto-generated />
                #nullable enable

                using System;
                using System.Collections.Generic;
                using System.Collections.Immutable;
                using PodNet.Blazor.TypedRoutes;
                using static System.FormattableString;

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
                        /// All available route templates for the component, containing the string{{(page.Routes.Length > 1 ? "s" : "")}}: {{string.Join(", ", page.Routes.Select(static r => $"<c>\"{r.Template}\"</c>"))}}.
                        /// </summary>
                        public static IReadOnlyList<string> AllPageRouteTemplates { get; } = ImmutableArray.Create({{string.Join(", ", page.Routes.Select(static r => $"\"{r.Template}\""))}});

                """);

            if (isParameterless)
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
                    var routeId = isParameterless || page.Routes.Length != 1 ? routeNumber.ToString() : "";
                    var parameters = string.Join(", ", route.Parameters.Select(static p => $"{p.TypeName}{(p.IsOptional || p.IsCatchAll ? "?" : "")} {p.Name}{((p.IsOptional || p.IsCatchAll) ? " = null" : "")}"));
                    var returnValue = s_routeParameters.Replace(route.Template, static m =>
                    {
                        var name = m.Groups["name"].Value;
                        return (m.Groups["type"].Value, m.Groups["optional"].Success, m.Groups["catchall"].Success) switch
                        {
                            ("string" or "nonfile" or "", false, false) => $$"""/{Uri.EscapeDataString({{name}})}""",
                            ("string" or "nonfile" or "", true, false) => $$"""/{({{name}} == null ? null : Uri.EscapeDataString({{name}}))}""",
                            ("string" or "nonfile" or "", _, true) => $$"""/{({{name}} == null ? null : Uri.UnescapeDataString({{name}}))}""",
                            ("bool", false, _) => $$"""/{({{name}} ? "true" : "false")}""",
                            ("bool", true, _) => $$"""/{({{name}} == true ? "true" : {{name}} == false ? "false" : null)}""",
                            ("datetime", false, _) => $$"""/{{{name}}.ToString({{name}}.TimeOfDay == default ? "yyyy-MM-dd" : "s")}""",
                            ("datetime", true, _) => $$"""/{{{name}}?.ToString({{name}}.Value.TimeOfDay == default ? "yyyy-MM-dd" : "s")}""",
                            _ => $"/{{{name}}}"
                        };
                    });
                    sourceBuilder.AppendLine($$"""
                        /// <summary>
                        /// Returns the URI for the page constructed from the template <c>"{{route.Template}}"</c> with
                        /// the provided parameters, using the invariant culture.
                        /// </summary>
                        public static string PageUri{{routeId}}({{parameters}}) => Invariant($"{{returnValue}}");
                            
                """);
                }
            }

            sourceBuilder.AppendLine($$"""
                    }
                }
                #nullable restore
                """);

            context.AddSource($"{hintNameSanitizer.Replace($"{page.Namespace}_{page.ClassName}", "_")}.g.cs", sourceBuilder.ToString());
        }
    }
}