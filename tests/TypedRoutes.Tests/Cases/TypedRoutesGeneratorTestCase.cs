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

    public string GlobalUsings { get; } = $"""
        global using PodNet.Blazor.TypedRoutes;
        global using {typeof(T).Name};
        """;

    public override CSharpCompilation CreateCompilation(ImmutableArray<SyntaxTree> syntaxTrees, ImmutableArray<AdditionalText> additionalFiles)
        => base.CreateCompilation(syntaxTrees, additionalFiles)
               .AddReferences(MetadataReference.CreateFromFile(typeof(INavigableComponent).Assembly.Location))
               .AddSyntaxTrees(CSharpSyntaxTree.ParseText(additionalFiles.Where(f => f.Path.EndsWith(".razor")).ToList() switch
               {
                   // Adding a global usings for the INavigableComponent and IRoutableComponent interfaces and the test case
                   { Count: 0 } => GlobalUsings,
                   // Faking the partial component classes so they're public and not internal by default
                   var razors => $"""
                    {GlobalUsings}

                    namespace {typeof(T).Name};
                    {string.Join("\r\n", razors.Select(r => $"public partial class {Path.GetFileNameWithoutExtension(r.Path)}"))};
                    """
               }));
}
