using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Scripting;
using PodNet.Analyzers.Testing;
using PodNet.Blazor.TypedRoutes.Generator;
using System.Collections.Immutable;

namespace PodNet.Blazor.TypedRoutes.Tests.Cases;

public class TypedRoutesGeneratorTestCase<T>(List<DiagnosticDescriptor>? expectedDescriptorsForGenerator = null, bool ignoreSources = false) : EmbeddedTestCase<TypedRoutesGenerator, T>
{
    public TypedRoutesGeneratorTestCase(bool ignoreSources) : this(null, ignoreSources) { }
    public override bool IgnoreSources => ignoreSources;
    public override List<DiagnosticDescriptor>? ExpectedDescriptorsForGenerator => expectedDescriptorsForGenerator;

    public override ScriptOptions ConfigureScriptOptions(ScriptOptions options)
        => base.ConfigureScriptOptions(options)
               .AddReferences(MetadataReference.CreateFromFile(typeof(INavigableComponent).Assembly.Location))
               .AddImports("PodNet.Blazor.TypedRoutes", typeof(T).Name);

    public SyntaxTree CommonTree { get; } = CSharpSyntaxTree.ParseText($$"""
        global using PodNet.Blazor.TypedRoutes;
        global using {{typeof(T).Name}};

        // Define the namespace so that it won't cause compilation errors
        namespace {{typeof(T).Name}} { }

        // Have to define the symbol so that the generator can pick it up in C# source.
        namespace Microsoft.AspNetCore.Components
        {
            [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
            public class RouteAttribute(string template) : Attribute
            {
                public string Template { get; } = template;
            }
        }
        """);

    public override CSharpCompilation CreateCompilation(ImmutableArray<SyntaxTree> syntaxTrees, ImmutableArray<AdditionalText> additionalFiles)
    {
        var compilation = base.CreateCompilation(syntaxTrees, additionalFiles)
               .AddReferences(MetadataReference.CreateFromFile(typeof(INavigableComponent).Assembly.Location))
               .AddSyntaxTrees(CommonTree);
        if (additionalFiles.Where(f => f.Path.EndsWith(".razor")).ToList() is { Count: > 0 } razors)
            compilation = compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText($"""
                    // Faking the partial component classes so they're public and not internal by default
                    namespace {typeof(T).Name};
                    {string.Join("\r\n", razors.Select(r => $"public partial class {Path.GetFileNameWithoutExtension(r.Path)}"))};
                    """));
        return compilation;
    }
}
